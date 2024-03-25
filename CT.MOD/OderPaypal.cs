using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class OrderItem
    {
        public int idUser { get; set; }
        public string UserName { set; get; }
        public string ProductName { set; get; }
        public string ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class Order1
    {

        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount => Items.Sum(item => item.Quantity * item.Price);
        public ItemList ItemList { get; set; }

        // Các thuộc tính khác của đơn hàng 
    }
    public class ItemList
    {
        public List<Item> Items { get; set; }
    }
    public class CartUser
    {
        public string Username { get; set; }
        public string TenSanPham { get; set; }
        public string MSanPham { get; set; }
        public int GioSoLuong { get; set; }
        public decimal GiaBan { get; set; }

    }
    public class PaymentDetails
    {
        public string OrderId { get; set; } // ID của đơn hàng
        public List<OrderItem> Products { get; set; } // Danh sách sản phẩm đã mua

        // Các thông tin khác nếu cần
    }
    public class ProductDetail
    {
        public string ProductId { get; set; } // ID của sản phẩm
        public int Quantity { get; set; } // Số lượng sản phẩm
        public decimal UnitPrice { get; set; } // Đơn giá của sản phẩm
        public decimal Discount { get; set; } // Giảm giá (nếu có)
        public decimal TotalPrice { get; set; } // Thành tiền của sản phẩm
                                                // Các thông tin khác nếu cần
    }
    public class ChiTietDonHang
    {
        public int idUser { get; set; }
        public string OrderId { get; set; }
        public string MSanPham { get; set; }
        public int SoLuong { get; set;}
        public decimal DonGia { get; set; }
        public decimal TrietKhau { get; set ; }
        public decimal ThanhTien { get; set;}
    }
    public class DonHangp
    {
       // public int ID_DonHang { get; set; }
        public string OrderID { get; set; }
        public int idUser { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public DateTime NgayMua { get; set; }
        public string Status { get; set; }

    }
    public class OrderDetail
    {
        public string OrderId { get; set; }
        public List<ChiTietDonHang> Products { get; set; }

        // Các thuộc tính khác nếu cần
    }
    


}