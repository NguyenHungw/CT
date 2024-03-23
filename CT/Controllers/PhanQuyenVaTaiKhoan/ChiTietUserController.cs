using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CT.Controllers.PhanQuyenVaTaiKhoan
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietUserController : ControllerBase
    {

        [HttpGet]
        [Route("DSChiTietUsers")]

        public IActionResult dsChucNang(int page)
        {

            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChiTietUserBUS().DanhSachUsers(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }

        }
        [HttpPost]
        [Route("CapNhatUser")]

        public IActionResult CapNhatUser(IFormFile file, [FromForm] CapNhatChiTietUserMOD item)
        {



            if (item == null) return BadRequest();
            if (file.Length > 0)
            {
                var Result = new ChiTietUserBUS().CNChiTietUser(item, file);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
            else
            {
                return NotFound();
            }


        }
    }

   
    /*
            [HttpPut]
            [Route("SuaChucNang")]
            [Authorize]

            public IActionResult SuaCN([FromBody] ChucNangMOD item)
            {
                var userclaim = User.Claims;
                var check = false;
                foreach (var claim in userclaim)
                {
                    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Sua"))
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                {
                    if (item == null) return BadRequest();
                    else
                    {
                        var Result = new ChucNangBUS().SuaCN(item);
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
            [HttpDelete]
            [Route("XoaChucNang")]
            [Authorize]

            public IActionResult XoaCN(int id)
            {
                var userclaim = User.Claims;
                var check = false;
                foreach (var claim in userclaim)
                {
                    if (claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xoa"))
                    {
                        check = true;
                        break;
                    }
                }

                if (check)
                {

                    if (id == null) return BadRequest();
                    else
                    {
                        var Result = new ChucNangBUS().XoaCN(id);
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
                    }); ;
                }

            }*/
    //  }
}
