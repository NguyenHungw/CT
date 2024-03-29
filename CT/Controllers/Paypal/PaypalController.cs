using CT.Controllers.Paypal;
using CT.Controllers.Paypal;
using CT.MOD;
using CT.ULT;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Caching.Memory;
using PayPalCheckoutSdk.Orders;
using Raven.Database.Util;
using System.Data.SqlClient;
using System.Net;
using System.Net.WebSockets;
using System.ServiceModel.Channels;

using System.Text;
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PayPalService payPalService;
    private readonly DatabaseHelper1 databaseHelper;
    private const string DefaultCurrencyCode = "USD";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _memoryCache;

    public PaymentController(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
    {
        // Khởi tạo PayPalService và DatabaseHelper với các thông số cần thiết.
        this.payPalService = new PayPalService("AUGz-LsUJYmCrwMprlEBzVb8h1YqPYOy6SHxSiMwQ267_252ijLgL0zFMP5bPejg0C2VnaS-JgmBchkP", "EN0hJnc1RiX4UHz4lsduRWJ7ve1jh7aMZZ9NHZlaEhjTOX61L2uGNUt4xsJGqN8E4HAcA5tFQiPAZu-S");
        this.databaseHelper = new DatabaseHelper1("Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100");
        _httpContextAccessor = httpContextAccessor;

    }


    [HttpPost("process-payment")]
    public async Task<IActionResult> ProcessPayment(int idUser)
    {
        try
        {
            // Thay thế 1 bằng giá trị thực tế của userId bạn muốn sử dụng

            // Lấy thông tin sản phẩm trong giỏ hàng từ cơ sở dữ liệu
            if (idUser <= 0)
            {
                return BadRequest("ID không hợp lệ");
            }
            else
            {
                var cartItems = databaseHelper.GetCartItems(idUser);
                // Kiểm tra xem giỏ hàng có sản phẩm không
                if (cartItems == null || cartItems.Count == 0)
                {
                    return BadRequest(new { Message = "Giỏ hàng trống" });
                }

                // Tạo đơn hàng từ PayPal
                //var order = await payPalService.CreateOrder(amount, DefaultCurrencyCode);
                var order = new Order1
                {
                    Items = cartItems.Select(item => new OrderItem
                    {
                        idUser = idUser,
                        UserName = item.Username,
                        ProductName = item.TenSanPham,
                        ProductId = item.MSanPham,
                        Quantity = item.GioSoLuong,
                        Price = item.GiaBan,
                    }).ToList(),

                };



                _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Order", order);


                var orderResponse = await payPalService.CreateOrder(order.TotalAmount, DefaultCurrencyCode);

                return Ok(new { OrderId = orderResponse.Id, Items = order.Items });

            }


            //return Ok(new { OrderId = order.ID });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }


    [AllowAnonymous]
    [HttpGet("start-payment/{orderId}")]
    public IActionResult StartPayment(int iduser, string orderId)
    {
        // gọi phương thức trong PayPalService để lấy URL thanh toán từ OrderId
        var paymentUrl = payPalService.GetPaymentUrl(orderId).GetAwaiter().GetResult();
        /* _httpContextAccessor.HttpContext.Session.SetString("OrderId", orderId);
         var test = HttpContext.Session.GetString("OrderId");*/
        bool check = false;

        var DonHang = new DonHangp
        {
            OrderID = orderId, // Lưu idUser vào đối tượng Order
            idUser = iduser,
            PhuongThucThanhToan = "PayPal",
            NgayMua = DateTime.UtcNow,// Ngày mua là thời điểm hiện tại
            Status = "Pending", // Trạng thái thanh toán là chờ xử lý ban đầu

        };

        databaseHelper.SaveOrder(orderId, iduser, DonHang.PhuongThucThanhToan, DonHang.NgayMua, DonHang.Status);


        // Lưu OrderId vào Session
        _httpContextAccessor.HttpContext.Session.SetString("OrderId", orderId);

        // trả về link thanh toán paypal

        return Ok(paymentUrl);

    }

    //api check thu tiền
    [HttpPost("capture-order/{orderId}V1")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        try
        {
            var captureResult = await payPalService.CaptureOrder(orderId);

            if (captureResult)
            {
                //var order = databaseHelper.GetCartItems();
                var order = HttpContext.Session.GetObjectFromJson<Order1>("Order");

                // Kiểm tra xem session có tồn tại hay không
                if (order == null)
                {
                    return BadRequest("Order information not found in session");
                }

                // Lặp qua từng sản phẩm trong đơn hàng và lưu thông tin chi tiết đơn hàng
                foreach (var product in order.Items)
                {
                    var chitietdonhang = new ChiTietDonHang
                    {
                        idUser = product.idUser,

                        OrderId = orderId,
                        MSanPham = product.ProductId,
                        SoLuong = product.Quantity,
                        DonGia = product.Price, // Giá mặc định hoặc lấy từ sản phẩm
                        TrietKhau = 0, // Triết khấu mặc định hoặc tính toán từ sản phẩm
                        ThanhTien = product.Quantity * product.Price,
                        // Cập nhật các thông tin khác nếu cần
                    };

                    // Gọi phương thức trong repository để lưu chi tiết đơn hàng vào cơ sở dữ liệu
                    databaseHelper.SaveOrderDetail(orderId, chitietdonhang.MSanPham, chitietdonhang.SoLuong, chitietdonhang.DonGia, chitietdonhang.TrietKhau, chitietdonhang.ThanhTien);

                    databaseHelper.TruSoLuongSanPham(chitietdonhang.MSanPham, chitietdonhang.SoLuong);
                    databaseHelper.RemoveProductFromCart(product.idUser);
                }

                // Cập nhật trạng thái thanh toán cho đơn hàng thành công
                databaseHelper.UpdatePaymentStatus(orderId, "Completed");

                return Ok(new
                {
                    Status = 1,
                    Message = "Payment captured successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Payment capture failed"
                });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = -1,
                Message = CT.ULT.Constant.API_Error_System
            });
        }


    }
    [HttpPost("capture-order/{orderId} V2")]
    public async Task<IActionResult> CaptureOrder2(string orderId)
    {
        try
        {
            var captureResult = await payPalService.CaptureOrder(orderId);

            if (captureResult)
            {

                return Ok(new
                {
                    Status = 1,
                    Message = "Payment captured successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Payment capture failed"
                });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Status = -1,
                Message = CT.ULT.Constant.API_Error_System
            });
        }


    }

    [HttpPost("check-order/{orderId}V2")]
    public async Task<IActionResult> CheckOrder(string orderId)
    {
        try
        {
            var orderStatusResult = await payPalService.IsOrderPaid(orderId);
            var order = HttpContext.Session.GetObjectFromJson<Order1>("Order");

            // Kiểm tra xem session có tồn tại hay không
            if (order == null)
            {
                return BadRequest("Order information not found in session");
            }

            // Lặp qua từng sản phẩm trong đơn hàng và lưu thông tin chi tiết đơn hàng
            foreach (var product in order.Items)
            {
                var chitietdonhang = new ChiTietDonHang
                {
                    idUser = product.idUser,

                    OrderId = orderId,
                    MSanPham = product.ProductId,
                    SoLuong = product.Quantity,
                    DonGia = product.Price, // Giá mặc định hoặc lấy từ sản phẩm
                    TrietKhau = 0, // Triết khấu mặc định hoặc tính toán từ sản phẩm
                    ThanhTien = product.Quantity * product.Price,
                    // Cập nhật các thông tin khác nếu cần
                };

                // Gọi phương thức trong repository để lưu chi tiết đơn hàng vào cơ sở dữ liệu
                databaseHelper.SaveOrderDetail(orderId, chitietdonhang.MSanPham, chitietdonhang.SoLuong, chitietdonhang.DonGia, chitietdonhang.TrietKhau, chitietdonhang.ThanhTien);

                databaseHelper.TruSoLuongSanPham(chitietdonhang.MSanPham, chitietdonhang.SoLuong);
                databaseHelper.RemoveProductFromCart(product.idUser);
            }

            // Cập nhật trạng thái thanh toán cho đơn hàng thành công
            databaseHelper.UpdatePaymentStatus(orderId, "Completed");
            HttpContext.Session.Remove("Order");


            return Ok(new { Status = orderStatusResult.Status, Message = orderStatusResult.Message });
        }
        catch (Exception ex)
        {
            // Xử lý ngoại lệ
            return StatusCode(500, new { Status = false, Message = "An error occurred while processing the request" });
        }
    }
}
    
