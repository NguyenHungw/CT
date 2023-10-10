using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungTrongNhomController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNND")]
        [Authorize]

        public IActionResult DanhSachNDTN(int page)
        {
            var userClaim = User.Claims;
            bool check = false;
            foreach (var claim in userClaim)
            {
                if(claim.Type=="CN" && claim.Value.Contains("QLNND")&& claim.Value.Contains("Xem"))
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
                    var Result = new NguoiDungTrongNhomBUS().dsNguoiDungTrongNhom(page);
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
        [Route("ThenNDvaoNhom")]
        [Authorize]
        public IActionResult ThemNDvaoNhom([FromBody] NguoiDungTrongNhomMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach(var claim in userclaim)
            {
                if( claim.Type=="CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Them"))
                {
                    check   =true;
                    break;
                }
            }

            if (check)
            {
                {
                    if (item == null) return BadRequest();
                    var Result = new NguoiDungTrongNhomBUS().ThemNDvaoNhom(item);
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

        [HttpPut]
        [Route("SuaNDtrongNhom")]
        public IActionResult SuaNDtrongNhom([FromBody] NguoiDungTrongNhomMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach(var claim in userclaim)
            {
                if(claim.Type=="CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Sua"))
                {
                    check = true;
                    break;

                }
            }
            if (check)
            {
                if (item == null) return BadRequest();
                var Result = new NguoiDungTrongNhomBUS().SuaNDtrongNhom(item);
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
        [Route("XoaNDTrongNhom")]
        public IActionResult XoaNDtrongNhom([FromBody]NguoiDungTrongNhomMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach (var claim in userclaim)
            {
                if(claim.Type == "CN" && claim.Value.Contains("QLNND") && claim.Value.Contains("Xoa"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (item == null) return BadRequest();
                var Reuslt = new NguoiDungTrongNhomBUS().XoaNDtrongNhom(item);
                if (Reuslt != null) return Ok(Reuslt);
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
