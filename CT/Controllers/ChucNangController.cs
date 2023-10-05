using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var UserClaimRole = User.FindFirst("CN")?.Value;
            if (!string.IsNullOrEmpty(UserClaimRole) && UserClaimRole.Contains("QLCN") && UserClaimRole.Contains("Xem"))
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
                return StatusCode(-99, "Không có quyền");
            }
         
        }

        [HttpPost]
        [Route("ThemChucNang")]
        [Authorize]

        public IActionResult ThemCN(string namecn)
        {
            var UserClaimRole = User.FindFirst("CN")?.Value;
            if(!string.IsNullOrEmpty(UserClaimRole) && UserClaimRole.Contains("QLCN") && UserClaimRole.Contains("Them"))
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
                return StatusCode(-99, "Không có quyền");
            }
           
        }

        [HttpPut]
        [Route("SuaChucNang")]
        [Authorize]

        public IActionResult SuaCN([FromBody] ChucNangMOD item)
        {
            var UserClaimRole = User.FindFirst("CN")?.Value;
            if (!string.IsNullOrEmpty(UserClaimRole) && UserClaimRole.Contains("QLCN") && UserClaimRole.Contains("Sua"))
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
                return StatusCode(-99, "Không có quyền");
            }
           
        }
        [HttpDelete]
        [Route("XoaChucNang")]
        [Authorize]

        public IActionResult XoaCN(int id)
        {
            var UserClaimRole = User.FindFirst("CN")?.Value;
            if (!String.IsNullOrEmpty(UserClaimRole) && UserClaimRole.Contains("QLCN") && UserClaimRole.Contains("Xoa"))
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
                return StatusCode(-99, "Không có quyền");
            }
           
        }
    }
}
