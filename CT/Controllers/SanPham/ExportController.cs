using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

[Route("api/[controller]")]
[ApiController]
public class ExcelExportController : ControllerBase
{
 

    private readonly string strcon = "Data Source=DESKTOP-BBQKCNB\\HUNGW;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100"; // Thay thế bằng chuỗi kết nối của bạn

    [HttpGet("export")]
    public IActionResult ExportToExcel(int page)
    {
        using (var package = new ExcelPackage())
        {
            const int Productperpage = 20;
            int startpage = Productperpage * (page - 1);
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(strcon))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select pn.ID_PhieuNhap,sp.MSanPham,sp.TenSanPham,lsp.TenLoaiSP,pn.NgayNhap,ct.SoLuong,dv.TenDonVi,ct.DonGia,ct.ThanhTien
                                        from ChiTietNhap ct
                                        join SanPham sp on ct.MSanPham =sp.MSanPham
                                        join LoaiSanPham lsp on sp.ID_LoaiSanPham = lsp.ID_LoaiSanPham
                                        join PhieuNhap pn on ct.ID_PhieuNhap = pn.ID_PhieuNhap
                                        join DanhMuc_DonVi dv on pn.ID_DonVi = dv.ID_DonVi
                                        ORDER BY ct.MSanPham
                                        OFFSET @StartPage ROWS
                                        FETCH NEXT @ProductPerPage ROWS ONLY;
                                        ";
                //var command = new SqlCommand("SELECT * FROM YourTable", connection);
                cmd.Parameters.AddWithValue("@StartPage", startpage);
                cmd.Parameters.AddWithValue("@ProductPerPage", Productperpage);
                var reader = cmd.ExecuteReader();
                dataTable.Load(reader);
            }

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            worksheet.Cells.LoadFromDataTable(dataTable, true);

            byte[] fileContents = package.GetAsByteArray();
            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Export.xlsx");
        }
    }
}
