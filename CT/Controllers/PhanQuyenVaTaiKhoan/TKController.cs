using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;

namespace CT.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]

    public class TKController : ControllerBase
    {

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


    }

}
