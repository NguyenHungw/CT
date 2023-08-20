using CT.DAL;
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
                    Result.Messeage = "ma sp k dc de trong";
                }
                if (item == null || item.TenSP == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "ten sp k dc de trong";
                }
                if (item == null || item.LoaiSanPham == null || item.LoaiSanPham == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "LoaiSanPhamk dc de trong";
                }
                if (item == null || item.SoLuong < 0)
                {
                    Result.Status = 0;
                    Result.Messeage = "ten sp k dc de trong";
                }
                if (item == null || item.SoLuong < 0)
                {
                    Result.Status = 0;
                    Result.Messeage = "SoLuong k dc de trong";
                }
                else
                {
                    var checksp = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (checksp != null)
                    {
                        Result.Status = -1;
                        Result.Messeage = "SP da ton tai";
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
                Result.Messeage = "Vui lòng điền đầy đủ thông tin";
                throw;
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
                    Result.Messeage = "anh khong duoc de trong";

                }
                if (item == null || item.TenSP == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "anh ko dc de trong";

                }
                if (item == null || item.LoaiSanPham == null || item.TenSP == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "loai sp ko dc de trong";

                }
                if (item == null || item.SoLuong < 0)
                {
                    Result.Status = 0;
                    Result.Messeage = "So luong ko dc de trong";
                }
                if (item == null || item.DonGia < 0)
                {
                    Result.Status = 0;
                    Result.Messeage = "don gia ko dc de trong";
                }
                else
                {
                    var sua = new SanPhamDAL().ThongTinSp(item.MSanPham);
                    if (sua == null)
                    {
                        Result.Status = 0;
                        Result.Messeage = "ma sp ko dung";
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
                Result.Messeage = "Vui long dien day du thong tin";
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
                Result.Messeage = "vui long nhap sp";
                return Result;
            }
            else
            {
                var chitietsp = new SanPhamDAL().ThongTinSp(msp);
                if (chitietsp == null)
                {
                    Result.Status = 0;
                    Result.Messeage = "Ma sp ko ton tai";
                    return Result;

                }
                else

                {
                    Result.Status = 1;
                    Result.Messeage = "xoa thanh cong";
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
                    Result.Messeage = "Xoa thanh cong";
                   
                }
                else
                {
                    Result.Status = 0;
                    Result.Messeage = "Không tìm thấy dữ liệu.";
                }

            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
            
        
        public BaseResultMOD DanhSachSP(int page)


        {
            var Result = new BaseResultMOD();
            try
            {
                if (page == 0)
                {
                    Result.Status = 0;
                    Result.Messeage = "Vui long nhap so trang";

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
                Result.Messeage = "Loi khi lay ds sp" + ex.Message;

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
                    Result.Messeage = "Không tìm thấy dữ liệu.";
                }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Messeage = "Lỗi khi lấy danh sách sản phẩm: " + ex.Message;
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
                    Result.Messeage = "vui long nhap ten sp";
                }
                else
                {


                    var checksp = new SanPhamDAL().TBNM(name);
                    if (checksp == null)
                    {
                        Result.Status = 0;
                        Result.Messeage = "ko tim thay sp";

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
                    Result.Messeage = "vui long nhap ten sp";
                }
                else
                {
                   
                   
                            var checksp = new SanPhamDAL().SearchByName(name);
                            if (checksp == null)
                            {
                        Result.Status = 0;
                        Result.Messeage = "ko tim thay sp";

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
                    Result.Messeage = "vui long chon lai loai sp";


                }
                else
                {
                    Result = new SanPhamDAL().DanhSachSPbyTypeSP(loaisp, page);
                }
            }
            catch (Exception ex )
            {
                Result.Status = -1;
                Result.Messeage = "Loi khi lay ds san pham " + ex.Message;
                throw;

            }
            return Result;
        }
        }
    }

