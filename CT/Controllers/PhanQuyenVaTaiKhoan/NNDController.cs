using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]
    public class NNDController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNND")]
        [Authorize]
        public IActionResult DanhSachNND(int page)
        {
            var userClaims = User.Claims;

            bool hasSecondPermission = false; // Biến này để kiểm tra quyền thứ hai

            foreach (var claim in userClaims)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Xem")) // Kiểm tra quyền thứ hai
                {
                    hasSecondPermission = true;
                    break; // Nếu tìm thấy quyền thứ hai, thoát khỏi vòng lặp
                }
            }
            if (hasSecondPermission)
            {

                if (page < 1) return BadRequest();
                else
                {
                    var Result = new NhomNguoiDungBUS().DanhSachNND(page);
                    if (Result != null) return Ok(Result);
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
        [Route("ThemNND")]
        [Authorize]
        public IActionResult ThemNND([FromBody] ThemMoiNND item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Them"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (item == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().ThemNND(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
        [HttpPut]
        [Route("SuaNND")]
        [Authorize]
        public IActionResult SuaNND([FromForm] DanhSachNhomNDMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Sua"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (item == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().SuaNND(item);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
        [HttpDelete]
        [Route("DeleteNND")]
        [Authorize]

        public IActionResult DeleteNND(int id)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if (claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Xoa"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (id == null) return BadRequest();
                var Result = new NhomNguoiDungBUS().XoaNND(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
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
    }
}
