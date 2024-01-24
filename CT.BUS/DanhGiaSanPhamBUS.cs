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
    public class DanhGiaSanPhamBUS
    {
        public BaseResultMOD DanhSachDGSP(int page)


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

                    Result = new DanhGiaSanPhamDAL().getdsDanhGia(page);

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

        public BaseResultMOD SuaDanhGia([FromBody]SuaDanhGiaSanPhamMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.MSanPham == null  )
                {
                    Result.Status = 0;
                    Result.Message = "ID mã sản phẩm không được để trống";

                }
                if (item == null || item.idUser == null)
                {
                    Result.Status = 0;
                    Result.Message = "ID người dùng không được để trống";

                }
                if (item == null || item.DiemDanhGia == null || item.DiemDanhGia > 5 || item.DiemDanhGia < 0)
                {
                    Result.Status = 0;
                    Result.Message = "Điểm đánh giá không hợp lệ";

                }


                else
                {
                    var sua = new DanhGiaSanPhamDAL().ThongTinDanhGia(item.id);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã đơn vị không tồn tại";
                        return Result;
                    }
                    else
                    {
                        return new DanhGiaSanPhamDAL().SuaDanhGia(item);
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

        public BaseResultMOD ThemDanhGia(ThemMoiDanhGiaSanPhamMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
             
                if (item == null || item.DiemDanhGia == null || item.DiemDanhGia >5|| item.DiemDanhGia <0)
                {
                    Result.Status = 0;
                    Result.Message = "Điểm đánh giá không hợp lệ";

                }
               

                else
                {
                  
                        return new DanhGiaSanPhamDAL().ThemDanhGiaSP(item);
                    
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

        public BaseResultMOD XoaDonVi(int id)
        {
            var Result = new BaseResultMOD();
            if (id == null)
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập mã vị ";
                return Result;
            }
            else
            {
                var chitietsp = new TrangChu_DSSPDAL().ThongTinDanhGia(id);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Message = "Bạn đã đánh giá sản phẩm này";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                    return new DonViDAL().XoaDonVi(id);
                }

            }
            return Result;
        }
    }
}
