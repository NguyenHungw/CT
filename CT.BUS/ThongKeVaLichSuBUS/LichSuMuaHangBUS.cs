using CT.DAL;
using CT.MOD;
using CT.MOD.ThongKeVaLichSuMOD;
using CT.MOD.TrangChuMOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class LichSuMuaHangBUS
    {
        public BaseResultMOD DanhSachLSMuaHang(int page)


        {
            var Result = new BaseResultMOD();
            try
            {
                if (page == 0)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập số trang";

                }
                else
                {

                    Result = new LichSuDonHangUserDAL().getLS_MuaHang_User(page);

                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = "Lỗi khi lấy danh loại sản phẩm" + ex.Message;

            }
            return Result;
        }
        public List<LichSuDonHangUserMOD> ChiTLSDonhang(int page, string iduser)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (iduser == null || iduser == "")
                {
                    Result.Status = 0;
                    Result.Message = "iduser không được để trống";
                    // Trả về null hoặc danh sách trống nếu không có dữ liệu
                    return null; // hoặc new List<LichSuDonHangUserMOD>();
                }
                else
                {
                    var checksp = new LichSuDonHangUserDAL().CT_LSMuahang(page, iduser);
                    if (checksp == null || checksp.Count == 0) // Kiểm tra nếu danh sách rỗng
                    {
                        Result.Status = 0;
                        Result.Message = "Sản phẩm không tồn tại";
                        // Trả về null hoặc danh sách trống nếu không có dữ liệu
                        return null; // hoặc new List<LichSuDonHangUserMOD>();
                    }
                    else
                    {
                        return checksp;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
