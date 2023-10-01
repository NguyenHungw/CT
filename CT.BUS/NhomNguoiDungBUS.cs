using CT.DAL;
using CT.MOD;
using CT.ULT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class NhomNguoiDungBUS
    {
        public BaseResultMOD DanhSachNND(int page) {
            var Result = new BaseResultMOD();
            try
            {
                if (page == 0)
                {
                    Result.Status = 0;
                    Result.Message = "Vui long nhap so trang";

                }
                else
                {

                    Result = new NhomNgoiDungDAL().getDanhSachNhomND(page);
                    /* if(loaisp != null)
                     {
                         Result = new SanPhamDAL().GetDanhSachSP(tensp);
                     }else if(tensp != null)
                     {
                         Result = new SanPhamDAL().GetDanhSachSP(tensp);
                     }*/
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = "Loi khi lay ds sp" + ex.Message;

            }
            return Result;
        }
        public BaseResultMOD ThemNND(ThemMoiNND item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.TenNND == null || item.TenNND == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên nhóm người dùng không được để trống";
                }
                else
                {
                    Result = new NhomNgoiDungDAL().ThemNND(item);
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.ERR_INSERT;
                throw;
            }
            return Result;

        }
        public BaseResultMOD SuaNND(DanhSachNhomNDMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.NNDID == null )
                {
                    Result.Status = 0;
                    Result.Message = "id không được để trống";
                }
                else if (item == null || item.TenNND == null || item.TenNND == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên nhóm người dùng không được để trống";

                }
                else
                {
                    Result = new NhomNgoiDungDAL().SuaNhomND(item);
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = Constant.API_Error_System;
                throw;
            }
            return Result;

        }
        public BaseResultMOD XoaNND(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                if(id == 0 || id == null )
                {
                    result.Status = 0;
                    result.Message = "ID không hợp lệ ";
                }
                else
                {
                    result = new NhomNgoiDungDAL().XoaNND(id);
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;
        }
    }
}
