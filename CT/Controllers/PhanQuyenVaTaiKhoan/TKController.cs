using CT.BUS;
using CT.MOD;
using CT.ULT;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using Rhino.Licensing;
using System.ServiceModel.Channels;
using System.Net;
using System.Data.SqlClient;
using static Raven.Client.Linq.LinqPathProvider;

namespace CT.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]

    public class TKController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly SmtpClient _smtpClient;
        private readonly string _connect;

        public TKController(IMemoryCache memoryCache, SmtpClient smtpClient, IConfiguration configuration)
        {
            _cache = memoryCache;
            _smtpClient = smtpClient;
            _connect = configuration.GetConnectionString("Hungconnectstring");
        }

      
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]

        public IActionResult Login([FromBody] TaiKhoanMOD login)
        {
            if (Login == null) return BadRequest();
            var Result = new TaiKhoanBUS().DangNhap(login);
            if (Result != null) return Ok(Result);
            else return BadRequest();

        }
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]


        public IActionResult Register([FromBody] DangKyTK item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DangKyTaiKhoan(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }
        [HttpGet]
        [Route("admin/DanhSachTK")]
        /*   [Authorize(Policy = "CanViewTK")]*/
        [Authorize]
        public IActionResult DanhSachTK(int page)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLTK") && claim.Value.Contains("Xem"))
                {
                    check = true;
                    break;

                }
            }

            if (check)
            {
                if (page < 1) return BadRequest();
                else
                {
                    var totalrows = 0;
                    var result = new TaiKhoanBUS().DanhSachTK(page);
                    result.TotalRow = totalrows;
                    if (result != null) return Ok(result);
                    else return NotFound();
                }
            }
            else
            {
                return NotFound(new BaseResultMOD
                {
                    Status = -99,
                    Message = ULT.Constant.NOT_ACCESS

                });
            }


        }
        [HttpPost]
        [Route("DoiMK")]
        [AllowAnonymous]

        public IActionResult DoiMK([FromBody] DoiMK item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DoiMatKhau(item);
            if (Result != null) return Ok(Result);
            else return NotFound();


        }
        [HttpPost]
        [Route("DoiTen")]
        [AllowAnonymous]
        public IActionResult DoiTen([FromForm] Rename item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DoiTen(item);
            if (Result != null) return Ok(Result);
            else return NotFound();


        }
        [HttpDelete]
        [Route("XoaTK")]
        public IActionResult XoaTK(string sdt)
        {
            if (sdt == null || sdt == " ") return BadRequest();
            var Result = new TaiKhoanBUS().XoaTK(sdt);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }

        [HttpPost("SendCode")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
               
                // Tạo mã xác thực ngẫu nhiên
                var randomCode = Utils.Utilities.GenerateRandomCode(6);

                // Lưu mã xác thực vào cache với khóa là địa chỉ email
                _cache.Set(model.Email, randomCode, TimeSpan.FromMinutes(30)); // Đặt thời gian hết hạn là 30 phút
                 // Chuyển randomCode thành mảng các ký tự
                char[] codeDigits = randomCode.ToCharArray();
                string codeHtml = string.Join("", codeDigits.Select(digit => $"\t<div style='display:inline-block; width: 40px; height: 40px; border: 2px solid #3498db; margin: 5px; text-align: center; background-color: #f0f0f0; color: #3498db; font-family: Arial, sans-serif; font-size: 18px; line-height: 40px;'><b><font color=\"red\">{digit}</b></div>"));

                // Gửi email
                string emailBody = $@"
            <html>
                <body>
                    <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                        <tr>
                            <td>
                                <!-- Các dòng mã HTML khác -->
                            </td>
                        </tr>
                        <tr height='16'></tr>
                        <tr>
                            <td>
                                <table bgcolor='#FAFAFA' width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width:332px;max-width:600px;border:1px solid #f0f0f0;border-bottom:1px solid #c0c0c0;border-top:0;border-bottom-left-radius:3px;border-bottom-right-radius:3px'>
                                    <tr height='16px'>
                                        <td width='32px' rowspan='3'></td>
                                        <td></td>
                                        <td width='32px' rowspan='3'></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>Kính gửi người dùng</p>
                                            <p>Mã xác minh bạn cần dùng để đăng ký tài khoản của mình (<a href='mailto:{model.Email}' target='_blank'>{model.Email}</a>) là: </p>
                                            <div style='text-align: center;'>{codeHtml}</div>
                                            <p>Xin vui lòng sử dụng mã này trong vòng 30 phút.</p>
                                            <p> Nếu không yêu cầu mã này thì bạn có thể bỏ qua email này một cách an toàn. Có thể ai đó khác đã nhập địa chỉ email của bạn do nhầm lẫn.</p>
                                            <p> Xin cảm ơn
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
            </html>";
                await _smtpClient.SendMailAsync("your@gmail.com", model.Email, "Xác thực đăng ký", emailBody);

                // Trả về thành công nếu không có lỗi
                return Ok(new
                {
                    Status=1,
                    Message= "Gửi mã xác minh thành công"
                });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, $"Lỗi khi đăng ký: {ex.Message}");
            }

        }
        [HttpPost("verify")]
        public IActionResult Verify([FromBody] VerifyModel model)
        {
            var result = new BaseResultMOD();
            // Lấy mã xác thực từ cache dựa trên địa chỉ email
            if (_cache.TryGetValue(model.Email, out string cachedCode))
            {
                // Kiểm tra mã xác thực
                if (model.Code == cachedCode){
                   

                    // sau khi kiểm tra đúng thì xóa code ở cache đi
                    _cache.Remove(model.Code);
                    try
                    {
                        using (var connection = new SqlConnection(_connect))
                        {
                            connection.Open();
                            using (var cmd = new SqlCommand("Update  [User] set isActive = 1 where Email = @Email ", connection))
                            {
                                cmd.Parameters.AddWithValue("@Email", model.Email);
                                cmd.ExecuteNonQuery();
                                result.Status = 1;
                                result.Message = "Xác thực tài khoản thành công";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Status = -1;
                        result.Message = Constant.API_Error_System;
                    }
                }
                else
                {
                    result.Status = 0;
                    result.Message = "Mã xác thực sai";
                }
          
            }
            return Ok(result);

        }

    }

}
