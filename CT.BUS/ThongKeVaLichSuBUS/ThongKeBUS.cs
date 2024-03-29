using CT.DAL;
using CT.MOD;
using CT.MOD.ThongKeVaLichSuMOD;
using CT.MOD.TrangChuMOD;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class ThongKeBUS
    {
        public BaseResultMOD ThongKeChiTieuBUS(TK_Date item)


        {
            var Result = new BaseResultMOD();
            try
            {
    /*            if (page == 0)
                {
                    Result.Status = 0;
                    Result.Message = "Vui lòng nhập số trang";

                }
                else
                {*/

                    Result = new ThongKeDAL().TK_ChiTieu(item);

               // }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }
        public BaseResultMOD ThongKeSoLuongNhap(TK_Date item)


        {
            var Result = new BaseResultMOD();
            try
            {
                /*            if (page == 0)
                            {
                                Result.Status = 0;
                                Result.Message = "Vui lòng nhập số trang";

                            }
                            else
                            {*/

                Result = new ThongKeDAL().TK_SoLuongNhap(item);

                // }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }
        public BaseResultMOD ThongKeSoLuongDaBanBUS(TK_Date item)


        {
            var Result = new BaseResultMOD();
            try
            {
                /*            if (page == 0)
                            {
                                Result.Status = 0;
                                Result.Message = "Vui lòng nhập số trang";

                            }
                            else
                            {*/

                Result = new ThongKeDAL().TK_SoLuongDaBan(item);

                // }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }
        public BaseResultMOD ThongKeDoanhThuBUS(TK_Date item)


        {
            var Result = new BaseResultMOD();
            try
            {
                /*            if (page == 0)
                            {
                                Result.Status = 0;
                                Result.Message = "Vui lòng nhập số trang";

                            }
                            else
                            {*/

                Result = new ThongKeDAL().TK_DoanhThu(item);

                // }
            }
            catch (Exception ex)
            {
                Result.Status = -1;
                Result.Data = null;
                Result.Message = ULT.Constant.API_Error_System;

            }
            return Result;
        }
    }
}
