using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.MOD
{
    public class InsertUMOD
    {
        public int idUser { get; set; }
    }
    public class DanhSachChiTietUserMOD
    {
        public int id { get; set; }
        public int idUser {  get; set; }
        public string? AvatarUser { get; set; }
        public string? FullName { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public int? GioiTinh { get; set; }

    }
    public class CapNhatChiTietUserMOD
    {
        public int idUser { get; set; }
        //public string? AvatarUser { get; set; }
        public string? FullName { get; set; }
        public string? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public int? GioiTinh { get; set; }

    }
    public class ThemAvatarMOD
    {
        public int idUser { get; set; }
        public string? AvatarUser { get; set; }
    }
    public class ThemFullNameMOD
    {
        public int idUser { get; set; }
        public string? FullName { get; set; }
    }
    public class ThemNSMOD
    {
        public int idUser { get; set; }
        public DateTime? NgaySinh { get; set; }
    }
    public class ThemDCMOD
    {
        public int idUser { get; set; }
        public string? DiaChi { get; set; }
    }
    public class ThemGTMOD
    {
        public int idUser { get; set; }
        public int? GioiTinh { get; set; }
    }
    public class InsertUserMOD
    {
        public int idUser { get; set; }
    }
}
