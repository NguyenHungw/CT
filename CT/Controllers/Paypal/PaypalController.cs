using CT.Controllers.Paypal;
using CT.Controllers.Paypal;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using Raven.Database.Util;
using System.Net;
using System.ServiceModel.Channels;

using System.Text;
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PayPalService payPalService;
    private readonly DatabaseHelper1 databaseHelper;
    private const string DefaultCurrencyCode = "USD";
    
    public PaymentController()
    {
        // Khởi tạo PayPalService và DatabaseHelper với các thông số cần thiết.
        this.payPalService = new PayPalService("AUGz-LsUJYmCrwMprlEBzVb8h1YqPYOy6SHxSiMwQ267_252ijLgL0zFMP5bPejg0C2VnaS-JgmBchkP", "EN0hJnc1RiX4UHz4lsduRWJ7ve1jh7aMZZ9NHZlaEhjTOX61L2uGNUt4xsJGqN8E4HAcA5tFQiPAZu-S");
        this.databaseHelper = new DatabaseHelper1("Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100");
    }


    [HttpPost("process-payment")]
    public async Task<IActionResult> ProcessPayment(int idUser)
    {
        try
        {
             // Thay thế 1 bằng giá trị thực tế của userId bạn muốn sử dụng

            // Lấy thông tin sản phẩm trong giỏ hàng từ cơ sở dữ liệu
            if(idUser <= 0)
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
                        UserName = item.Username,
                        ProductName = item.TenSanPham,
                        ProductId = item.MSanPham,
                        Quantity = item.GioSoLuong,
                        Price = item.GiaBan,
                    }).ToList(),
                    /*    ItemList = new ItemList
                        {
                            Items = cartItems.Select(item => new Item
                            {
                                Name = item.TenSanPham,
                                Description = item.MSanPham,
                                Quantity = item.GioSoLuong.ToString(),
                                Price = item.GiaBan.ToString(),
                                Currency = DefaultCurrencyCode,
                            }).ToList(),
                        },*/
                };


                // Cập nhật tổng số tiền của đơn hàng


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
    public IActionResult StartPayment(string orderId)
    {
        // gọi phương thức trong PayPalService để lấy URL thanh toán từ OrderId
        var paymentUrl = payPalService.GetPaymentUrl(orderId).GetAwaiter().GetResult();

        // Lưu orderId vào Session hoặc cơ sở dữ liệu để sử dụng sau này khi người dùng quay lại
        HttpContext.Session.SetString("OrderId", orderId);
        // trả về link thanh toán paypal

        return Ok(paymentUrl);
        
     
    }
  
    //api check thu tiền
    [HttpPost("capture-order/{orderId}")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        try
        {
            var captureResult = await payPalService.CaptureOrder(orderId);

            if (captureResult)
            {
                return Ok(new { 
                    Status=1,
                    Message = "Payment captured successfully" });
            }
            else
            {
                return BadRequest(new { 
                    Status =0,
                    Message = "Payment capture failed" });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                Status =-1,
                Message = "Internal server error" });
        }
    }


   /* private void UpdateProductQuantity(int productId, decimal purchasedAmount)
    {
        try
        {
            // Lấy số lượng hiện tại từ cơ sở dữ liệu
            int currentQuantity = databaseHelper.GetProductQuantity(productId);

            // Trừ đi số lượng đã mua
            int updatedQuantity = currentQuantity - Convert.ToInt32(purchasedAmount);

            // Cập nhật số lượng mới vào cơ sở dữ liệu
            databaseHelper.UpdateProductQuantity(productId, updatedQuantity);
        }
        catch (Exception ex)
        {
          
            Console.WriteLine($"Error updating product quantity: {ex.Message}");
            throw;
        }
    }*/
}
