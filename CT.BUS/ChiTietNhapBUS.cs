using CT.DAL;
using CT.ULT;
using CT.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class ChiTietNhapBUS
    {
        public BaseResultMOD DanhSachChiTietNhap(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChiTietNhapDAL().getDSChiTietNhap(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;

            }
            return result;

        }
        public BaseResultMOD ThemChiTietNhap(ThemChiTietNhap item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.ID_PhieuNhap == null)
                {
                    result.Status = 0;
                    result.Message = " ID_PhieuNhap Không được để trống ";
                }
                if (item == null || item.MSanPham == null || item.MSanPham=="")
                {
                    result.Status = 0;
                    result.Message = " MSanPham Không được để trống ";
                }
                if (item == null || item.SoLuong == null )
                {
                    result.Status = 0;
                    result.Message = " SoLuong Không được để trống ";
                }
                if (item == null || item.DonGia == null)
                {
                    result.Status = 0;
                    result.Message = " Đơn giá Không được để trống ";
                }
                if(item==null || item.GiaBan == null)
                {
                    result.Status = 0;
                    result.Message = " Giá bán Không được để trống ";
                }
                else
                {
                    result = new ChiTietNhapDAL().ThemChiTietNhap(item);
                }
            }
            catch (Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaChiTietNhap(ThemChiTietNhap item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item == null || item.ID_PhieuNhap == null)
                {
                    result.Status = 0;
                    result.Message = " ID_PhieuNhap Không được để trống ";
                }
                if (item == null || item.MSanPham == null || item.MSanPham == "")
                {
                    result.Status = 0;
                    result.Message = " MSanPham Không được để trống ";
                }
                if (item == null || item.SoLuong == null)
                {
                    result.Status = 0;
                    result.Message = " SoLuong Không được để trống ";
                }
                if (item == null || item.DonGia == null)
                {
                    result.Status = 0;
                    result.Message = " Đơn giá Không được để trống ";
                }
                if (item == null || item.GiaBan == null)
                {
                    result.Status = 0;
                    result.Message = " Giá bán Không được để trống ";
                }
                else
                {
                    result = new ChiTietNhapDAL().SuaChiTietPhieuNhap(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaChiTietNhap(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                if (id == null || id <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID không hợp lệ";
                }
                else
                {
                    result = new ChiTietNhapDAL().XoaChiTietNhap(id);
                }
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }

            return result;
        }
        public BaseResultMOD DanhSachKho(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChiTietNhapDAL().GetDSKho(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;

            }
            return result;

        }
        public BaseResultMOD DanhSachKhoSapHetHang(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChiTietNhapDAL().GetDSKhoSapHetHang(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;

            }
            return result;

        }
        public BaseResultMOD DanhSachKhoDaHetHang(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChiTietNhapDAL().GetDSKhoDaHetHang(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;

            }
            return result;

        }
        public BaseResultMOD DanhSachPhieuNhapKho(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChiTietNhapDAL().GetDSPhieuNhapKho(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "Lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = ULT.Constant.API_Error_System;
                result.Data = null;
                throw;

            }
            return result;

        }
    }
}

