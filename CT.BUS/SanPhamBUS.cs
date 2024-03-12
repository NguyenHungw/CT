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
    public class SanPhamBUS
    {
        public BaseResultMOD ThemSP(SanPhamMOD item, IFormFile file)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.MSanPham == null || item.MSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";
                }
                if (item == null || item.TenSP == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên sản phẩm không được để trống";
                }
                if (item == null || item.LoaiSanPham <=0)
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không hợp lệ";
                }
             
                else
                {
                    var checksp = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (checksp != null)
                    {
                        Result.Status = -1;
                        Result.Message = "Sản phẩm đã tồn tại";
                    }
                    else
                    {
                        return new SanPhamDAL().ThemSP(item, file);
                    }

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
        public BaseResultMOD ThemSPBase64(SanPhamMOD item, IFormFile file)
        {
            var Result = new BaseResultMOD();

            try
            {

                // Kiểm tra các điều kiện
                if (item == null || item.MSanPham == null || item.MSanPham == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";
                }
                if (item == null || item.TenSP == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên sản phẩm không được để trống";
                }
                if (item == null || item.LoaiSanPham <=0)
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không hợp lệ";
                }
             
                else
                {
                    var checksp = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (checksp != null)
                    {
                        Result.Status = -1;
                        Result.Message = "Sản phẩm đã tồn tại";
                    }
                    else
                    {

                   
                        return new SanPhamDAL().ThemSPBase64(item, file);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Result.Status = -1;
                Result.Message = "Thêm sản phẩm thất bại";
            }
            return Result;
        }

        public BaseResultMOD SuaSP(SanPhamMOD item, IFormFile file)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || file == null)
                {
                    Result.Status = 0;
                    Result.Message = "Ảnh không được để trống";

                }
                if (item == null || item.TenSP == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên sản phẩm không được để trống";

                }
                if (item == null || item.LoaiSanPham == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Message = "Loại sản phẩm không đượcc để trống";

                }
             
                else
                {
                    var sua = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Mã sản phẩm không tồn tại";
                        return Result;
                    }
                    else
                    {
                        return new SanPhamDAL().SuaSP(item, file);
                    }
                }
            }
            catch (Exception)
            {
                Result.Status = -1;
                Result.Message = "Vui long dien day du thong tin";
                throw;
            }
            return Result;
        }
        public BaseResultMOD XoaSP(string msp)
        {
            var Result = new BaseResultMOD();
            if (msp == null || msp == "")
            {
                Result.Status = 0;
                Result.Message = "Vui lòng nhập mã sản phẩm";
                return Result;
            }
            else
            {
                var chitietsp = new SanPhamDAL().ThongTinSp(msp);
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
                    return new SanPhamDAL().XoaSp(msp);
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

                    Result = new SanPhamDAL().GetDanhSachSP(page);
                  
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
        public BaseResultMOD DanhSachSPKP(int page)
        {
            var Result = new BaseResultMOD();
            try
            {
                SanPhamDAL dsModel = new SanPhamDAL();
                var danhSachItem = dsModel.DSDAL(page); // Lấy dữ liệu từ phương thức DSDAL của đối tượng dsModel

                if (danhSachItem != null)
                {
                    // Xử lý dữ liệu và gán vào Result
                    Result.Status = 1;
                    Result.Data = danhSachItem;
                }
                else
                {
                    Result.Status = 0;
                    Result.Message = "Không tìm thấy dữ liệu.";
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Message = "Lỗi khi lấy danh sách sản phẩm: " + ex.Message;
            }

            return Result;
        }


        public TimSp TimKiemByNameModal(string name)
        {
            var item = new TimSp();
            var Result = new BaseResultMOD();
            try
            {
                if (name == null || name == "")
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập tên sản phẩm";
                }
                else
                {


                    var checksp = new SanPhamDAL().TBNM(name);
                    if (checksp == null)
                    {
                        Result.Status = 0;
                        Result.Message = "Không tìm thấy sản phẩm";

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

        public TimSp TimKiemByName(string name)
        {
            var item = new TimSp();
            var Result = new BaseResultMOD();
            try
            {
                if(name == null|| name == "")
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập tên sản phẩm";
                }
                else
                {
  
                            var checksp = new SanPhamDAL().SearchByName(name);
                            if (checksp == null)
                            {
                        Result.Status = 0;
                        Result.Message = "Không tìm thấy sản phẩm";

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

