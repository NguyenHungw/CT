using CT.BUS;
using CT.DAL;
using CT.MOD;
using CT.ULT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        [HttpPost]
        [Route("ThemSP")]
        [Authorize]


        public IActionResult ThemSP([FromForm] ThemSP_Gio item)
        {



            //var userId = User.FindFirst("PhoneNumber")?.Value; // Lấy PhoneNumber từ claim
            // Kiểm tra claim "NhomNguoiDung" để xác định quyền
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLSP") && claim.Value.Contains("Them"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (item == null) return BadRequest();
              
                    var Result = new GioHangBUS().ThemSP(item);
                    if (Result != null)
                    {
                        return Ok(Result);
                    }
                    else return NotFound();
                
             
            }



            else
            {

                return NotFound(new BaseResultMOD
                {
                    Status = -99,
                    Message = Constant.NOT_ACCESS
                });
            }
        }


      
        [HttpDelete]
        [Route("XoaSP")]
        [Authorize]
        public IActionResult XoaSP(string msp,string iduser)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLSP") && claim.Value.Contains("Xoa"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {


                if (msp == null || msp == "") return BadRequest();
                var Result = new GioHangBUS().XoaSP(msp,iduser);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }

            else
            {
                return NotFound(new BaseResultMOD
                {
                    Status = -99,
                    Message = Constant.NOT_ACCESS
                });
            }

        }
        [HttpDelete]
        [Route("XoaALLSP")]
        public IActionResult XoaAllSP()
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLSP") && claim.Value.Contains("Xoa"))
                {
                    check = true;
                    break;
                }
            }

            if (check)
            {
                var Result = new GioHangBUS().XoaAllSP();
                if (Result != null) return Ok(Result);
                else return NotFound();
            }

            else
            {
                return NotFound(new BaseResultMOD
                {
                    Status = -99,
                    Message = Constant.NOT_ACCESS
                });
            }

        }
        [HttpGet]
        [Route("DanhSachSP")]
        [AllowAnonymous]
        public IActionResult DanhSachSP(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new GioHangBUS().DanhSachSP(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
  

        
        [HttpPost]
        [Route("ChiTietSP {msp}")]
        [AllowAnonymous]
        public IActionResult ChiTietSP(string msp)
        {
            if (msp == null || msp == " ") return BadRequest();
            var Result = new SanPhamBUS().ChiTSP(msp);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

     
    }
}
