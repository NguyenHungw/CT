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
    public class ChucNangBUS
    {
        public BaseResultMOD dsChucNang(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new ChucNangDAL().getdsChucNang(page); }
                else
                {
                    result.Status = 0;
                    result.Message = "lỗi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Message = " Lỗi hệ thống ";
                result.Data = null;
                throw;

            }
            return result;

        }
        public BaseResultMOD ThemCN(string namecn)
        {
            var result = new BaseResultMOD();
            try
            {
                if (namecn == null || namecn == "")
                {
                    result.Status = 0;
                    result.Message = " Tên chức năng không được để trống";
                }
                else
                {
                    result = new ChucNangDAL().ThemChucNang(namecn);
                }
            }catch(Exception ex)
            {
                result.Data = -1;
                result.Message = Constant.API_Error_System;

            }
            return result;
        }
        public BaseResultMOD SuaCN(ChucNangMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if(item ==null || item.ChucNangid==null || item.ChucNangid <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID chức năng không hợp lệ";
                }else if(item==null || item.TenChucNang == null || item.TenChucNang == "")
                {
                    result.Status = 0;
                    result.Message = "Tên Chức năng không được để trống";

                }
                else
                {
                    result = new ChucNangDAL().SuaChucNang(item);
                }
            }
            catch
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
        public BaseResultMOD XoaCN(int id)
        {
            var result = new BaseResultMOD();
            try
            {
                if(id ==null|| id <= 0)
                {
                    result.Status= 0;
                    result.Message = "ID không hợp lệ";
                }
                else
                {
                    result = new ChucNangDAL().XoaChucNang(id);
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }

            return result;
        }
        
    }
}
