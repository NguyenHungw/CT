using CT.DAL;
using CT.MOD;
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
    public class TrangChu_DSSPBUS
    {
        public BaseResultMOD DanhSachSP(int page)


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

                    Result = new TrangChu_DSSPDAL().getdssp(page);

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
        public TrangChu_CTSPMOD ChiTSP(string msp)
        {
            var item = new TrangChu_CTSPMOD();
            var Result = new BaseResultMOD();
            try
            {
                if (msp == null || msp == "")
                {
                    Result.Status = 0;
                    Result.Message = "Mã sản phẩm không được để trống";

                }
                else
                {
                    var checksp = new TrangChu_DSSPDAL().CTSP(msp);
                    if (checksp == null)
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
    }
}
