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
    public class PhieuNhapBUS
    {
        public BaseResultMOD DanhSachPhieuNhap(int page)


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

                    Result = new PhieuNhapDAL().getDSPhieuNhap(page);

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

        public BaseResultMOD SuaPhieuNhap([FromBody] PhieuNhapMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.NguoiNhapHang == null || item.NguoiNhapHang == "")
                {
                    Result.Status = 0;
                    Result.Message = "Người nhập không được để trống";

                }
               
                if (item == null || item.ID_DonVi == null)
                {
                    Result.Status = 0;
                    Result.Message = "ID đơn vị không được để trống";

                }


                else
                {
                    var sua = new DonViDAL().ThongTinDonVi(item.ID_DonVi);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã đơn vị không tồn tại";
                        return Result;
                    }
                    else
                    {
                        return new PhieuNhapDAL().SuaPhieuNhap(item);
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

        public BaseResultMOD ThemMoiPhieuNhap(ThemMoiPhieuNhapMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
             
                if (item == null || item.NguoiNhapHang == null || item.NguoiNhapHang == "")
                {
                    Result.Status = 0;
                    Result.Message = "Người nhập không được để trống";

                }
               
                if (item == null || item.ID_DonVi == null )
                {
                    Result.Status = 0;
                    Result.Message = "ID đơn vị không được để trống";

                }


                else
                {
                  
                        return new PhieuNhapDAL().ThemPhieuNhap(item);
                    
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

        public BaseResultMOD XoaPhieuNhap(int id)
        {
            var Result = new BaseResultMOD();
            if (id == null)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập mã phiếu ";
                return Result;
            }
            else
            {
                var chitietsp = new PhieuNhapDAL().ThongTinPhieuNhap(id);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Message = "Mã phiếu nhập tồn tại";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                    return new PhieuNhapDAL().XoaPhieuNhap(id);
                }

            }
            return Result;
        }
    }
}
