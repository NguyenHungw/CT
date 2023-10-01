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

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TKController : ControllerBase
    {

        [HttpPost]
        [Route("Login")]

        public IActionResult Login([FromBody] TaiKhoanMOD login)
        {
            if (Login == null) return BadRequest();
            var Result = new TaiKhoanBUS().DangNhap(login);
            if (Result != null) return Ok(Result);
            else return BadRequest();

        }
        [HttpPost]
        [Route("Register")]

        public IActionResult Register([FromBody] DangKyTK item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DangKyTaiKhoan(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }
        [HttpGet]
        [Route("DanhSachTK")]
        public IActionResult DanhSachTK(int page)
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
        [HttpPost]
        [Route("DoiMK")]

        public IActionResult DoiMK([FromBody] DoiMK item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DoiMatKhau(item);
            if (Result != null) return Ok(Result);
            else return NotFound();


        }
        [HttpPost]
        [Route("DoiTen")]
        public IActionResult DoiTen([FromForm] Rename item)
        {
            if (item == null) return BadRequest();
            var Result = new TaiKhoanBUS().DoiTen(item);
            if (Result != null) return Ok(Result);
            else return NotFound();


        }
        [HttpDelete]
        [Route("XoaTK")]
        public IActionResult XoaTK( string sdt)
        {
            if(sdt == null || sdt ==" ") return BadRequest();
            var Result = new TaiKhoanBUS().XoaTK(sdt);
            if (Result != null) return Ok(Result);
            else return NotFound();
            
        }
        

    }

}
