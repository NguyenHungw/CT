using CT.DAL;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class GiaBanSanPhamBUS
    {
        public BaseResultMOD DanhSachGiaBan(int page)


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

                    Result = new GiaBanSanPhamDAL().getdsgiaBan(page);

                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }

        public BaseResultMOD DanhSachSanPhamChuaApGia(int page)


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

                    Result = new GiaBanSanPhamDAL().DanhSachChuaApGia(page);

                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }


        public BaseResultMOD SuaDonGiaBan([FromBody]ThemGiaBanSanPham item)
        {
            var Result = new BaseResultMOD();
            try
            {
              
                if (item == null || item.MSanPham == null )
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";

                }
                if (item == null || item.NgayBatDau == null )
                {
                    Result.Status = 0;
                    Result.Message = "Ngày bắt đầu không được để trống";

                }
                if (item == null || item.GiaBan == null)
                {
                    Result.Status = 0;
                    Result.Message = "Giá bán không được để trống";

                }


                else
                {
                    var sua = new GiaBanSanPhamDAL().ThongTinGiaBanSP(int.Parse(item.MSanPham));
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã đơn vị không tồn tại";
                        return Result;
                    }
                    else
                    {
                        return new GiaBanSanPhamDAL().SuaGiaBan(item);
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

        public BaseResultMOD ThemGiaBan(ThemGiaBanSanPham item)
        {
            var Result = new BaseResultMOD();
            try
            {

               
                if (item == null || item.MSanPham == null)
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";

                }
                if (item == null || item.NgayBatDau == null)
                {
                    Result.Status = 0;
                    Result.Message = "Ngày bắt đầu không được để trống";

                }
                if (item == null || item.GiaBan == null)
                {
                    Result.Status = 0;
                    Result.Message = "Giá bán không được để trống";

                }


                else
                {
                  
                        return new GiaBanSanPhamDAL().ThemGiaBanSP(item);
                    
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

        public BaseResultMOD XoaGiaBan(int id)
        {
            var Result = new BaseResultMOD();
            if (id == null)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng mã đơn vị ";
                return Result;
            }
            else
            {
                var chitietsp = new GiaBanSanPhamDAL().ThongTinGiaBanSP(id);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Message = "Mã đơn không vị tồn tại";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                    return new GiaBanSanPhamDAL().XoaGiaBan(id);
                }

            }
            return Result;
        }
    }
}
