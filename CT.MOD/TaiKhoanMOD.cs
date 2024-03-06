using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class TaiKhoanMOD
    {
/*        public string Username { get; set; }*/

        //public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

    }

  
    public class TKModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
    
/*    public class UserConstants
    {
        public static List<TKModel> Users = new()
            {
                    new TKModel(){ PhoneNumber="naeem",Password="naeem_admin",Role="Admin"}
            };
    }
*/
    public class DangKyTK
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class TaiKhoanModel
    {
        public int idUser { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int isActive {get; set; }
    }

    public class DanhSachChucNang
    {
        /*public int MChucNang { get; set; }
        public string TenChucNang { get; set; }

        public int quyen { get; set; }*/
       // public string PhoneNumber { get; set; }
        public int idChucNang { get; set; }
        public string TenChucNang { get; set; }
    }
    public class DanhSachNhomND
    {
        public int idUser { get; set; }
        public string Username  { get; set; }
        public string TenNND { get; set; }

        public string ChucNang { get; set; }

        
        public Boolean Xem { get; set; }
        public Boolean Them { get; set; }
       
        public Boolean Sua { get; set; }
        public Boolean Xoa { get; set; }

    }
    
    public class DoiMK
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RePassword { get;set;}
    }
    public class Rename
    {
        public string Username { get; set;}
        public string PhoneNumber {get; set;}
        public string Password {get; set;}
    }
    
    public class RegisterModel
    {
        public string Email { get; set; }
    }
    public class VerifyModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
