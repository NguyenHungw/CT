﻿using CT.DAL;
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
    public class DonViBUS
    {
        public BaseResultMOD DanhSachDonVi(int page)


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

                    Result = new DonViDAL().getdsDonVi(page);

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

        public BaseResultMOD SuaDonVi([FromBody]DonViMOD item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (item == null || item.ID_DonVi == null )
                {
                    Result.Status = 0;
                    Result.Message = "ID loại sản phẩm không được để trống";

                }
                if (item == null || item.TenDonVi == null || item.TenDonVi == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên Loại sản phẩm không được để trống";

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
                        return new DonViDAL().SuaDonVi(item);
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

        public BaseResultMOD ThemDonVi(ThemMoiDonVi item)
        {
            var Result = new BaseResultMOD();
            try
            {
             
                if (item == null || item.TenDonVi == null || item.TenDonVi == "")
                {
                    Result.Status = 0;
                    Result.Message = "Tên đơn vị không được để trống";

                }
               

                else
                {
                  
                        return new DonViDAL().ThemDonVi(item);
                    
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
                Result.Message = "Vui lòng mã đơn vị ";
                return Result;
            }
            else
            {
                var chitietsp = new DonViDAL().ThongTinDonVi(id);
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
                    return new DonViDAL().XoaDonVi(id);
                }

            }
            return Result;
        }
    }
}
