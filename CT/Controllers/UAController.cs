using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using CT.MOD;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CT.ULT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static CT.MOD.jwtmod;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CT.Services
{
    // Khai báo namespace và sử dụng các thư viện cần thiết

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        

        private readonly IConfiguration _configuration;
     

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";

        SqlConnection SQLCon = null;

       

        [HttpPost("login")]
        [AllowAnonymous]

        public IActionResult Login([FromBody] TaiKhoanMOD item )
        {
            try
            {
                // đọc giá trị khóa bí mật từ cấu hình ứng dụng và chuyển thành mảng byte
                byte[] keyBytes = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                // Lấy Issuer và Audience từ cấu hình
                string _issuer = _configuration["Jwt:Issuer"];
                string _audience = _configuration["Jwt:Audience"];

                using (SqlConnection SQLCon = new SqlConnection(strcon))
                {
                    SQLCon.Open();

                    string sqlQuery = @"select u.idUser, u.PhoneNumber, u.Username, u.Email , u.Password,u.isActive, NND.TenNND,CN.TenChucNang, CNCNND.Xem,CNCNND.Them,CNCNND.Sua,CNCNND.Xoa
                                from [User] u 
                                inner join NguoiDungTrongNhom NDTN on u.idUser = NDTN.idUser
                                inner join NhomNguoiDung NND on NDTN.NNDID = NND.NNDID
                                inner join ChucNangCuaNhomND CNCNND on NND.NNDID = CNCNND.NNDID
                                inner join ChucNang CN on CNCNND.ChucNangid = CN.ChucNangid
                                where u.PhoneNumber=@PhoneNumber";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                    {
                        cmd.Parameters.AddWithValue("@PhoneNumber", item.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Password", item.Password);

                        SqlDataReader reader = cmd.ExecuteReader();
                        bool isAuthenticated = false; 
                        string userRole = null;
                        List<string> permissions = new List<string>();
                        List<string> functionalities = new List<string>();
                        List<string> CN = new List<string>();
                        List<Claim> claims = new List<Claim>();
                        //jwtmod cvk = null;
                        var cvk = new jwtmod();

                        while (reader.Read())
                        {
                         
                            string hashedPasswordFromDB = reader.GetString(reader.GetOrdinal("Password"));

                            if (BCrypt.Net.BCrypt.Verify(item.Password, hashedPasswordFromDB))
                            {
                                //cvk = new jwtmod();
                              
                                cvk.ID = reader.GetInt32(reader.GetOrdinal("idUser"));
                                cvk.Username = reader.GetString(reader.GetOrdinal("Username"));
                                cvk.Email = reader.GetString(reader.GetOrdinal("Email"));
                                cvk.PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"));
                                int isActive = reader.GetInt32(reader.GetOrdinal("isActive"));
                                string ChucNang = reader.IsDBNull(reader.GetOrdinal("TenChucNang")) ? null : reader.GetString(reader.GetOrdinal("TenChucNang"));
                                string quyen = "";
                                Boolean Xem = reader.GetBoolean(reader.GetOrdinal("Xem"));
                                Boolean Them = reader.GetBoolean(reader.GetOrdinal("Them"));
                                Boolean Sua = reader.GetBoolean(reader.GetOrdinal("Sua"));
                                Boolean Xoa = reader.GetBoolean(reader.GetOrdinal("Xoa"));
                                
                                //List<Claim> claims = new List<Claim>();
                                if (isActive == 1)
                                {

                                    isAuthenticated = true; // người dùng đã xác thực
                                    userRole = reader.GetString(reader.GetOrdinal("TenNND"));
                                  
                                    if (!string.IsNullOrEmpty(ChucNang))
                                    {

/*                                        permissions.Add(ChucNang);*/

                                        if (Xem)
                                        {
                                            quyen += "Xem,";
                                        }
                                        if (Them)
                                        {
                                            quyen += "Them,";
                                        }
                                        if (Sua)
                                        {
                                            quyen += "Sua,";
                                        }
                                        if (Xoa)
                                        {
                                            quyen += "Xoa,";
                                        }
                                        quyen = quyen.TrimEnd(',');
                                        


                                        string chucNangVaQuyen = $"{ChucNang}:{quyen}";
                                        claims.Add(new Claim("CN", chucNangVaQuyen));
                                        /*claims.Add(new Claim("TenChucNang", ChucNang));
                                        claims.Add(new Claim("Quyen", quyen));*/

                                    }

                                }
                            }
                        }
                      

                        // sau khi xác minh thông tin đăng nhập, tạo mã thông báo JWT và trả về 
                     if (isAuthenticated){
           

                            var authService = new AuthService(
                                _configuration["Jwt:Key"],
                                _issuer,
                                _audience
                            );

                            var taiKhoanMOD = new TaiKhoanMOD
                            {
                                PhoneNumber = item.PhoneNumber,
                            };

                            var (jwtToken, refreshToken) = authService.GenerateJwtAndRefreshToken(taiKhoanMOD, userRole, claims);
                            /*    var baseResult = new BaseResultMOD
                                {
                                    Status = 1,
                                    Message = "Đăng nhập thành công"
                                };*/
                            List<string> chucNangClaims = claims
                                        .Where(c => c.Type == "CN") // Lọc ra các Claim có kiểu "CN"
                                        .Select(c => c.Value) // Chọn giá trị của các Claim
                                        .ToList();
                            var aidi = claims.FirstOrDefault(n => n.Type == "idUser")?.Value;
                           
                            //var name = claims.FirstOrDefault(n => n.Type == "Username")?.Value; 
                            var time = claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
                            var R = claims.FirstOrDefault(r => r.Type == "NhomNguoiDung")?.Value;
                            var rp = new jwtmod
                            {
                                Status = 1,
                                Message = "Đăng nhập thành công",
                                ID = cvk.ID,
                                Username = cvk.Username,
                                Role =R,
                                PhoneNumber = cvk.PhoneNumber,
                                Email = cvk.Email,
                                TimeOut = time,
                                ChucNangVaQuyen = chucNangClaims,
                                Token = jwtToken,
                                RefreshToken = refreshToken


                            };
                           /* var response = new
                            {
                                BaseResult = rp,
                                Token = token
                              
                            };*/
                            return Ok(rp);
                          
                        }
                        else
                        {

                            // trả về thông báo lỗi nếu tài khoản bị vô hiệu hóa hoặc thông tin không hợp lệ
                            return Unauthorized(new BaseResultMOD
                            {
                                Status = 0,
                                Message = "Tài khoản hoặc mật khẩu không đúng, hoặc bị vô hiệu hóa"
                            }) ;

                        }
                    }
                }

                // trả về thông báo lỗi nếu không tìm thấy dữ liệu
                return NotFound(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Tài khoản không tồn tại"
                });
            }
            catch (Exception ex)
            {
                // trả về mã lỗi 500 nếu xảy ra lỗi trong quá trình xử lý
                Console.WriteLine("Caught exception: " + ex.Message);
                return StatusCode(500);
            }
        }


        // Phương thức xử lý cho trang quản trị (Admin)
        [HttpGet("admin")]
        public IActionResult AdminDashboard()
        {
            // Kiểm tra và xác thực mã thông báo JWT từ tiêu đề Authorization của yêu cầu
            var authHeader = HttpContext.Request.Headers["Authorization"];
            if (authHeader.ToString().StartsWith("Bearer "))
            {
                var token = authHeader.ToString().Substring(7);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                try
                {
                    // xác thực và giải mã token
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out SecurityToken validatedToken);

                    // Token hợp lệ, tiếp tục xử lý

                    return Ok("Trang quản trị dành cho Admin");
                }
                catch (Exception ex)
                {
                    // Trả về thông báo lỗi nếu mã thông báo không hợp lệ
                    return Unauthorized("Token không hợp lệ: " + ex.Message);
                }
            }
            else
            {
                // Trả về thông báo lỗi nếu không có mã thông báo hoặc mã thông báo không đúng định dạng
                return Unauthorized("Token không hợp lệ");
            }
        }


        // Một hành động ví dụ chỉ dành cho User
        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult UserDashboard()
        {
            return Ok("Trang dàng cho user");
        }
    }
}
 

