using CT.DAL;
using CT.MOD;
using CT.ULT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class NguoiDungTrongNhomBUS
    {
        public BaseResultMOD dsNguoiDungTrongNhom(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if (page > 0) { result = new NguoiDungTrongNhomDAL().getdsndtn(page); }
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
        public BaseResultMOD ThemNDvaoNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if (item.NNDID == null)
                {
                    result.Status= 0;
                    result.Message = "NNDID ko dc de trong";
                }
                else if (item.idUser == null)
                {
                    result.Status = 0;
                    result.Message = "idUser ko dc de trong";
                }
                else
                {
                    result = new NguoiDungTrongNhomDAL().ThemNDvaoNhom(item);
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message="Loi he thong"+ex;
                throw;
            }
            return result;
        }
        public BaseResultMOD SuaNDtrongNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if(item== null|| item.idUser==null||item.idUser<=0)
                {
                    result.Status = 0;
                    result.Message = "ID người dùng không hợp lệ";
                }else if(item == null || item.NNDID == null || item.NNDID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID nhóm người dùng không hợp lệ";
                }
                else
                {
                    result = new NguoiDungTrongNhomDAL().SuaNDtrongNhom(item);
                }
            }catch(Exception ex)
            {
                result.Status = -1;
                result.Message= Constant.ERR_UPDATE;
            }
            return result;
        }
        public BaseResultMOD XoaNDtrongNhom(NguoiDungTrongNhomMOD item)
        {
            var result = new BaseResultMOD();
            try
            {
                if(item==null||item.idUser==null|| item.idUser<= 0)
                {
                    result.Status= 0;
                    result.Message = "ID user không hợp lệ";
                }else if(item == null || item.NNDID==null || item.NNDID <= 0)
                {
                    result.Status = 0;
                    result.Message = "ID nhóm người dùng không hợp lệ";
                }
                else
                {
                    result = new NguoiDungTrongNhomDAL().XoaNDKhoiNhom(item);
                }
            }catch (Exception ex)
            {
                result.Status = -1;
                result.Message = Constant.API_Error_System;
            }
            return result;

        }
    }
    
}
