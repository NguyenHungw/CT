using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucNangController : ControllerBase
    {

        [HttpGet]
        [Route("DSChucNang")]
        [Authorize]
        
        public IActionResult dsChucNang(int page)
        {
            var userclaim = User.Claims;
            bool check = false;
            foreach(var claim in userclaim)
            {
                if(claim.Type == "CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xem"))
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
                    var Result = new ChucNangBUS().dsChucNang(page);
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
        [Route("ThemChucNang")]
        [Authorize]

        public IActionResult ThemCN(string namecn)
        {

            var userclaim = User.Claims;
            var check = false;
            foreach(var claim in userclaim)
            {
                if(claim.Type=="CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Them"))
                {
                    check = true;
                    break;
                }
            }
            if (check)
            {
                if (namecn == null || namecn == "") return BadRequest();
                else
                {
                    var Result = new ChucNangBUS().ThemCN(namecn);
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
        [Route("SuaChucNang")]
        [Authorize]

        public IActionResult SuaCN([FromBody] ChucNangMOD item)
        {
            var userclaim = User.Claims;
            var check = false;
            foreach(var claim in userclaim)
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
            foreach(var claim in userclaim)
            {
                if(claim.Type=="CN" && claim.Value.Contains("QLCN") && claim.Value.Contains("Xoa"))
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
                });;
            }
           
        }
    }
}
