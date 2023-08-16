using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class PasePagingParam
    {
        //thuoc tinh luu tru khoa tim kiem mac dinh la chuoi rong
        public string Keyword { get; set; } = " ";
        //thuoc tinh tuy chon sap xep
        public string OderByOption { get; set; } = "";
        //thuoc tinh chua ten cot de sap xep 
        public string OderByName { get; set; } = "";
        //so luong tren moi trang = 10
        public int PageSize { get; set; } = 10;

        // so luong trang hien tai 
        public int PageNumber { get; set; } = 1;

        // Thuộc tính tính toán giá trị Offset dựa trên PageSize và PageNumber.
        // Offset thường được sử dụng để tính vị trí bắt đầu lấy dữ liệu từ cơ sở dữ liệu.
        public int Offset { get { return (PageSize == 0 ? 10 : PageSize) * ((PageNumber == 0 ? 1 : PageNumber) - 1); } }

        // Thuộc tính tính toán giá trị Limit dựa trên PageSize.


        // Limit thường được sử dụng để xác định số lượng mục lấy ra từ cơ sở dữ liệu.
        public int Limit { get { return (PageSize == 0 ? 10 : PageSize); } }

 
        public int VaiTro { get; set; }

        //  int? cho phép giá trị null.
        public int? TrangThai { get; set; }
    }
}
