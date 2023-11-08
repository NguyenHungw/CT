using CT.DAL;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class LoaiSanPhamBUS
    {
        public BaseResultMOD DanhSachLoaiSP(int page)


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

                    Result = new LoaiSanPhamDAL().getDanhSachLoaiSP(page);

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

        public BaseResultMOD SuaLoaiSP(LoaiSanPhamMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.ID_LoaiSanPham == null )
                {
                    Result.Status = 0;
                    Result.Message = "ID loại sản phẩm không được để trống";

                }
                if (item == null || item.TenLoaiSP == null || item.TenLoaiSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên Loại sản phẩm không được để trống";

                }
                if (item == null || item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được để trống";
                }
               
                else
                {
                    var sua = new LoaiSanPhamDAL().ThongTinLSp(item.ID_LoaiSanPham);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã sản phẩm không tồn tại";
                        return Result;
                    }
                    else
                    {
                        return new LoaiSanPhamDAL().SuaLoaiSP(item);
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ULT.Constant.API_Error_System;
                throw;
            }
            return Result;
        }

        public BaseResultMOD ThemLoaiSP(ThemMoiLoaiSP item)
        {
            var Result = new BaseResultMOD();
            try
            {
             
                if (item == null || item.TenLoaiSP == null || item.TenLoaiSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên Loại sản phẩm không được để trống";

                }
                if (item == null || item.TrangThai == null)
                {
                    Result.Status = 0;
                    Result.Message = "Trạng thái không được để trống";
                }

                else
                {
                  
                        return new LoaiSanPhamDAL().ThemLoaiSP(item);
                    
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = ULT.Constant.API_Error_System;
                throw;
            }
            return Result;
        }

        public BaseResultMOD XoaLSP(int id)
        {
            var Result = new BaseResultMOD();
            if (id == null)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập mã loại sản phẩm";
                return Result;
            }
            else
            {
                var chitietsp = new LoaiSanPhamDAL().ThongTinLSp(id);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không tồn tại";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                    return new LoaiSanPhamDAL().XoaLoaiSP(id);
                }

            }
            return Result;
        }
    }
}
