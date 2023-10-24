using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using CT.DAL;
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
using static CT.MOD.Jwt.jwtmod;
using Microsoft.AspNetCore.Routing.Patterns;
using System.Security.Authentication.ExtendedProtection;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using CT.MOD.Jwt;

namespace CT.Services
{
    // Khai báo namespace và sử dụng các thư viện cần thiết

    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private readonly AuthService authService; // Thêm biến thành viên để lưu thể hiện của AuthService
        private readonly string _issuer; // Khai báo biến _issuer
        private readonly string _audience; // Khai báo biến _audience
        private readonly string _secretKey;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            _issuer = _configuration["Jwt:Issuer"]; // Gán giá trị từ cấu hình
            _audience = _configuration["Jwt:Audience"]; // Gán giá trị từ cấu hình
            _secretKey = _configuration["Jwt:Key"];
            authService = new AuthService(

                _configuration["Jwt:Key"],
                _issuer,
                _audience
            );

        }



        private string strcon = "Data Source=DESKTOP-PMRM3DP\\SQLEXPRESS;Initial Catalog=CT;Persist Security Info=True;User ID=Hungw;Password=123456;Trusted_Connection=True;Max Pool Size=100";

        SqlConnection SQLCon = null;



        [HttpPost("login")]
        [AllowAnonymous]

        public IActionResult Login([FromBody] TaiKhoanMOD item)
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
                        }SQLCon.Close();
                        


                        // sau khi xác minh thông tin đăng nhập, tạo mã thông báo JWT và trả về 
                        if (isAuthenticated)
                        {
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
                                Role = R,
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
                            if (!string.IsNullOrEmpty(cvk.PhoneNumber) && !string.IsNullOrEmpty(refreshToken))
                            {
                                try
                                {
                                    using (SqlConnection SQLCon1 = new SqlConnection(strcon))
                                    {
                                        SQLCon1.Open(); // Mở kết nối

                                        // Kiểm tra xem dữ liệu có trùng lặp không
                                        bool isDuplicate = KiemTraTrung(cvk);
                                        if (isDuplicate)
                                        {
                                            string sqlDelete = "DELETE FROM RefreshTokens WHERE UserId = @UserId";
                                            using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, SQLCon1))
                                            {
                                                cmdDelete.Parameters.AddWithValue("@UserId", cvk.PhoneNumber);
                                                cmdDelete.ExecuteNonQuery();
                                            }
                                        }

                                        DateTime expirationDate = DateTime.UtcNow.AddDays(30);

                                        // Thực hiện lệnh INSERT
                                        string sqlInsert = "INSERT INTO RefreshTokens (UserId, TokenValue, ExpirationDate) VALUES (@UserId, @TokenValue, @ExpirationDate)";
                                        using (SqlCommand cmdInsert = new SqlCommand(sqlInsert, SQLCon1))
                                        {
                                            cmdInsert.Parameters.AddWithValue("@UserId", cvk.PhoneNumber);
                                            cmdInsert.Parameters.AddWithValue("@TokenValue", refreshToken);
                                            cmdInsert.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                                            cmdInsert.ExecuteNonQuery();
                                        }

                                        // Đảm bảo rằng kết nối được đóng sau khi đã sử dụng xong.
                                        SQLCon1.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                }
                            }



                            return Ok(rp);

                        }
                        else
                        {

                            // trả về thông báo lỗi nếu tài khoản bị vô hiệu hóa hoặc thông tin không hợp lệ
                            return Unauthorized(new BaseResultMOD
                            {
                                Status = 0,
                                Message = "Tài khoản hoặc mật khẩu không đúng, hoặc bị vô hiệu hóa"
                            });

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
        private bool KiemTraTrung(jwtmod cvk)
        {
            using (SqlConnection SQLCon = new SqlConnection(strcon))
            {
                SQLCon.Open();
                string checkQuery = "SELECT COUNT(*) FROM RefreshTokens WHERE UserId = @UserId";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, SQLCon))
                {
                    checkCmd.Parameters.AddWithValue("@UserId", cvk.PhoneNumber);
                    int existingCount = (int)checkCmd.ExecuteScalar();
                    return existingCount > 0;
                }
            }
        }
        [HttpPost("refresh")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            // Kiểm tra tính hợp lệ của AccessToken và lấy Principal
            List<Claim> claims = new List<Claim>();
            var principal = authService.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
            var cvk = new jwtmod();
            if (principal == null)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "AccessToken hết hạn hoặc không hợp lệ."
                });
            }

            // Lấy secret key từ cấu hình hoặc từ nơi khác
            string secretKey = _configuration["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var handler = new JwtSecurityTokenHandler();
            SecurityToken token;

            principal = handler.ValidateToken(refreshTokenRequest.AccessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false, // Chỉnh sửa cài đặt theo yêu cầu
                ValidateAudience = false, // Chỉnh sửa cài đặt theo yêu cầu
            }, out token);
            // Lấy UserID từ AccessToken

            var userId = principal.Claims.FirstOrDefault(c => c.Type == "PhoneNumber")?.Value;

            // Kiểm tra Refresh Token có tồn tại và hợp lệ trong cơ sở dữ liệu
            var refreshTokenFromDb = GetRefreshTokenFromDatabase(userId, refreshTokenRequest.RefreshToken);

            if (refreshTokenFromDb == null)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Refresh Token không tồn tại hoặc không hợp lệ."
                });
            }

            // Kiểm tra tính hợp lệ của Refresh Token
            if (refreshTokenFromDb.ExpirationDate < DateTime.UtcNow)
            {
                return Unauthorized(new BaseResultMOD
                {
                    Status = 0,
                    Message = "Refresh Token đã hết hạn."
                });
            }

            // Tạo AccessToken mới
          /*  var taiKhoanMOD = new TaiKhoanMOD
            {
                PhoneNumber = refreshTokenRequest.PhoneNumber,
            };*/

            var (jwtToken, newRefreshToken) = authService.GenerateJwtAndRefreshToken2( refreshTokenFromDb.UserId, principal.Claims.ToList());

            // Cập nhật Refresh Token mới vào cơ sở dữ liệu
            UpdateRefreshTokenInDatabase(userId, refreshTokenRequest.RefreshToken, newRefreshToken);

            // Trả về AccessToken và Refresh Token mới
       /*     List<string> chucNangClaimsz = claims
                                       .Where(c => c.Type == "CN") // Lọc ra các Claim có kiểu "CN"
                                       .Select(c => c.Value) // Chọn giá trị của các Claim
                                       .ToList();*/
            var aidi = principal.Claims.FirstOrDefault(n => n.Type == "idUser")?.Value;

            var name = principal.Claims.FirstOrDefault(n => n.Type == "Username")?.Value; 
            var time = principal.Claims.FirstOrDefault(t => t.Type == "ThoiHanDangNhap")?.Value;
            var R = principal.Claims.FirstOrDefault(r => r.Type == "NhomNguoiDung")?.Value;


            var response = new jwtRefreshMod
            {
                Status = 1,
                Message = "Refresh Token thành công",
                Token = jwtToken,
                RefreshToken = newRefreshToken,
               // ID = aidi,
              
                
            };

            return Ok(response);
        }

        private RefreshToken GetRefreshTokenFromDatabase(string userId, string refreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(strcon))
            {
                SQLCon.Open();

                string sqlQuery = "SELECT * FROM RefreshTokens WHERE UserId = @UserId AND TokenValue = @TokenValue";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TokenValue", refreshToken);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var dbRefreshToken = new RefreshToken

                            {
                                UserId = userId,
                            TokenValue = reader.GetString(reader.GetOrdinal("TokenValue")),
                                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"))
                            };
                            return dbRefreshToken;
                        }
                    }
                }
            }
            return null;
        }

        private void UpdateRefreshTokenInDatabase(string userId, string oldRefreshToken, string newRefreshToken)
        {
            using (SqlConnection SQLCon = new SqlConnection(strcon))
            {
                SQLCon.Open();

                string sqlQuery = "UPDATE RefreshTokens SET TokenValue = @NewRefreshToken WHERE UserId = @UserId AND TokenValue = @OldRefreshToken";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, SQLCon))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@OldRefreshToken", oldRefreshToken);
                    cmd.Parameters.AddWithValue("@NewRefreshToken", newRefreshToken);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

}





