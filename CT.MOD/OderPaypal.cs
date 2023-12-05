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

        // Các thuộc tính khác của đơn hàng (nếu có)
    }
    public class ItemList
    {
        public List<Item> Items { get; set; }
    }
    public class CartUser{
        public string Username { get; set; }
        public string TenSanPham { get; set; }
        public string MSanPham { get; set; }
        public int GioSoLuong { get; set; }
        public decimal GiaBan { get; set; }

    }

}
