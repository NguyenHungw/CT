using 
    CT.DAL;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class GioHangBUS
    {
        public BaseResultMOD ThemSP(ThemSP_Gio item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.MSanPham == null || item.MSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";
                }
                if (item == null || item.idUser == null )
                {
                    Result.Status = 0;
                    Result.Message = "idUser không được để trống";
                }
             
             
                else
                {
                   /* var checksp = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (checksp != null)
                    {
                        Result.Status = -1;
                        Result.Message = "Sản phẩm đã tồn tại";
                    }
                    else
                    {*/
                        return new GioHangDAL().ThemSP_Gio(item);
                   // }

                }

            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "Vui lòng điền đầy đủ thông tin";
                throw;
            }
            return Result;

        }
   
     
        public BaseResultMOD XoaSP(string msanpham, string idUser)
        {
            var Result = new BaseResultMOD();
            if (msanpham == null || msanpham == "")
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập mã sản phẩm";
                return Result;
            }else if (idUser == null || idUser == "")
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập idUser";
                return Result;
            }
            else
            {
                var chitietsp = new GioHangDAL().XoaSp_Gio(msanpham,idUser);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Message = "mã sản phẩm không tồn tại";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                    return new GioHangDAL().XoaSp_Gio(msanpham, idUser);
                }

            }
            return Result;
        }
        public BaseResultMOD XoaAllSP()
        {
            var Result = new BaseResultMOD();
            /*   SanPhamDAL dsModel = new SanPhamDAL();*/
            SanPhamDAL checkxoa = new SanPhamDAL();
                var check = checkxoa.XoaAllSP(); // Lấy dữ liệu từ phương thức DSDAL của đối tượng dsModel
            try
            {

          
                if (check != null)
                {
                    // Xử lý dữ liệu và gán vào Result
                    Result.Status = 1;
                    Result.Message = "Xóa thành công";
                   
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Không tìm thấy dữ liệu.";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
            
        
        public BaseResultMOD DanhSachSP(int page )


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

                    Result = new GioHangDAL().GetDanhSachSP_Gio(page);
                  
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = "Lỗi khi lấy danh sách sản phẩm" + ex.Message;

            }
            return Result;
        }
      


        public ChiTietSP ChiTSP(string msp)
        {
            var item = new  ChiTietSP();
            var Result = new BaseResultMOD();
            try
            {
                if(msp == null|| msp == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";

                }
                else
                {
                    var checksp = new SanPhamDAL().CTSP(msp);
                        if(checksp == null)
                        {
                            Result.Status = 0;
                            Result.Message = "Sản phẩm không tồn tại";
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
            return item;
        }

        public BaseResultMOD PhanLoaiSP(string loaisp, int page)
        {
            var Result = new BaseResultMOD();
            try
            {
                if(loaisp == null || loaisp == "")
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng chọn loại sản phẩm";


                }
                else
                {
                    Result = new SanPhamDAL().DanhSachSPbyTypeSP(loaisp, page);
                }
            }
            catch (Exception ex )
            {
                Result.Status = -1;
                Result.Message = "Lỗi khi lấy danh sách sản phẩm " + ex.Message;
                throw;

            }
            return Result;
        }
        }
    }

