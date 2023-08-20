using CT.DAL;
using CT.MOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CT.BUS
{
    public class TaiKhoanBUS

    {
        public BaseResultMOD DangNhap(TaiKhoanMOD login)
        {
            var Result = new BaseResultMOD();
            try
            {
                if (login.PhoneNumber == null || login.PhoneNumber == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "sdt ko dc de trong";
                    return Result;

                } else if (login.Password == null || login.Password == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "password ko dc de trong";
                    return Result;

                }
                else
                {
                    var Userlogin = new TaiKhoanDAL().LoginDAL(login);
                    var quyen = new TaiKhoanDAL().function;
                    if(Userlogin != null)
                    {
                       if(Userlogin.role == 1)
                        {
                            Result.Status= 1;
                            Result.Messeage = "Dang nhap thanh cong";
                            Result.Data = new TaiKhoanDAL().function(Userlogin.role);

                        }
                        if (Userlogin.role == 2)
                        {
                            Result.Status = 1;
                            Result.Messeage = "Dang nhap thanh cong";
                            Result.Data = new TaiKhoanDAL().function(Userlogin.role);

                        }
                        if (Userlogin.role == 3)
                        {
                            Result.Status = 1;
                            Result.Messeage = "Dang nhap thanh cong";
                            Result.Data = new TaiKhoanDAL().function(Userlogin.role);

                        }

                    }
                    if (Userlogin == null)
                    {
                        Result.Status = 0;
                        Result.Messeage = "tai khoan hoac mk ko dung";
                    }
                    return Result;
                }
            }
            catch(Exception ex)
            {
                throw;
            }

            return Result;
        }
        public BaseResultMOD DangKyTaiKhoan(DangKyTK item)
        {
            var Result = new BaseResultMOD();
            try
            {
                if(item == null || item.Name == null || item.Name == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "name k dc de trong";

                }
                else if (item == null || item.Password == null || item.Password == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "mk k dc de trong";

                }
                else if (item == null || item.PhoneNumber == null || item.PhoneNumber == "")
                {
                    Result.Status = 0;
                    Result.Messeage = "sdt k dc de trong";

                }
                else { return new TaiKhoanDAL().RegisterDAL(item); }


            }
            catch(Exception ex)
            {
                Result.Status = -1;
                Result.Messeage = " dien du moi thong tin";
                throw;

            }
            return Result;
        }
        public BaseResultMOD DanhSachTK(int page)
        {
            var result = new BaseResultMOD();
            try
            {
                if(page >0 ) { result = new TaiKhoanDAL().DanhSachTK(page); }
                else
                {
                    result.Status = 0;
                    result.Messeage = "loi page";
                }
            }
            catch (Exception)
            {
                result.Status = -1;
                result.Messeage = "loi he thong";
                result.Data = null;
                throw;

            }
            return result;

        }
       

        public BaseResultMOD DoiMatKhau(DoiMK item)
        {
            var Result = new BaseResultMOD();
           
                if(item==null|| item.PhoneNumber == null || item.PhoneNumber ==" ")
                {
                    Result.Status=0;
                    Result.Messeage = "SDT ko dc de trong";
                }else if(item == null || item.Password == null || item.Password==" ")
                {
                    Result.Status = 0;
                    Result.Messeage = "MK ko dc de trong";
                }else if(item == null || item.RePassword == null|| item.RePassword == " ")
                {
                    Result.Status = 0;
                    Result.Messeage = " MK ko dc de trong";
                }
                else
                {
                    var checktk = new TaiKhoanDAL().inforTK(item.PhoneNumber);
                    if(checktk != null && item.Password == item.RePassword )
                    {
                        return Result = new TaiKhoanDAL().DoiMatKhau(item);

                    }
                    else
                    {
                        Result.Status = -1;
                        Result.Messeage = " SDT sai";
                    }
                }

            return Result;
        }
        public BaseResultMOD XoaTK(string sdt)
        {
            var Result = new BaseResultMOD();
            if(sdt ==null || sdt == "")
            {
                Result.Status = 0;
                Result.Messeage = "SDT ko dc de trong";
                return Result;


            }
            else
            {
                var thongtintk = new TaiKhoanDAL().inforTK(sdt);
                if(thongtintk == null)
                {
                    Result.Status = 0;
                    Result.Messeage = "SDT ko ton tai";
                    return Result;
                }
                else
                {
                    return new TaiKhoanDAL().XoaTK(sdt);
                }

            }
        }
            
        }
        
    }

