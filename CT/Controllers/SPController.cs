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

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SPController : ControllerBase
    {
        [HttpPost]
        [Route("ThemSP")]
        [Authorize]


        public IActionResult ThemSP(IFormFile file, [FromForm] SanPhamMOD item)
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
                if (file.Length > 0)
                {
                    var Result = new SanPhamBUS().ThemSP(item, file);
                    if (Result != null)
                    {
                        return Ok(Result);
                    }
                    else return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
         
          
            
            else
            {

                return NotFound(new BaseResultMOD
                {
                    Status = -99,
                    Message= Constant.NOT_ACCESS
                });
            }
        }


        [HttpPost]
        [Route("ThemSPBase64")]
        [Authorize]
        public IActionResult ThemSPBase64( IFormFile file, [FromForm] SanPhamMOD item )
        {
            var userclaim = User.Claims;
                var check = false;
            foreach(var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLSP") && claim.Value.Contains("Them"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                var Result = new SanPhamBUS().ThemSPBase64(item, file);
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



        [HttpPut]
        [Route("SuaSP")]
        [Authorize]
        public IActionResult SuaSP(IFormFile file, [FromForm] SanPhamMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLSP") && claim.Value.Contains("Sua"))
                {
                    check = true;
                    break;
                }
            }

            if (check)
            {
                if (item == null) return BadRequest();
                if (file.Length > 0)
                {
                    var Result = new SanPhamBUS().SuaSP(item, file);
                    if (Result != null) return Ok(Result);
                    else return NotFound();
                }
                else
                {
                    return NotFound();
                }
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
        public IActionResult XoaSP(string msp)
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
                    var Result = new SanPhamBUS().XoaSP(msp);
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
                var Result = new SanPhamBUS().XoaAllSP();
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
        public IActionResult DanhSachSP (int page)
        {
            if(page<1) return BadRequest();
            else
            {
                var Result = new SanPhamBUS().DanhSachSP(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpGet]
        [Route("DanhSachSP-2-")]
        public IActionResult DanhSachSPKP(int page)
        {
            var result = new SanPhamBUS().DanhSachSPKP(page); 

            if (result.Status == 1)
            {
                return Ok(result.Data); // Trả về dữ liệu nếu thành công
            }
            else if (result.Status == 0)
            {
                return NotFound(result.Message); // Trả về thông báo lỗi nếu không tìm thấy dữ liệu
            }
            else
            {
                return BadRequest(result.Message); // Trả về thông báo lỗi nếu có lỗi xảy ra
            }
        }

        [HttpPost]
        [Route("TimKiemSPModal")]
        [AllowAnonymous]
        public IActionResult TimKiemSPModal(string name)
        {
            if (name == null || name == " ") return BadRequest();
            var Result = new SanPhamDAL().TBNM(name);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("TimKiemSP")]
        [AllowAnonymous]
        public IActionResult TimKiemSP(string name)
        {
            if (name == null || name == " ") return BadRequest();
            var Result = new SanPhamBUS().TimKiemByName(name);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("ChiTietSp")]
        [AllowAnonymous]
        public IActionResult ChiTietSP(string msp)
        {
            if (msp == null || msp == " ") return BadRequest();
            var Result = new SanPhamBUS().ChiTSP(msp);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpPost]
        [Route("PhanLoaiSP")]
        [AllowAnonymous]
        public IActionResult PhanLoaiSP(string loaisp, int page)
        {
            if(page<1) return BadRequest();
            else
            {
                var Result = new SanPhamBUS().PhanLoaiSP(loaisp, page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}
