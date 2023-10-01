using CT.BUS;
using CT.MOD;
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
        public IActionResult dsChucNang(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChucNangBUS().dsChucNang(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPost]
        [Route("ThemChucNang")]
        public IActionResult ThemCN(string namecn)
        {
            if (namecn == null || namecn == "") return BadRequest();
            else
            {
                var Result = new ChucNangBUS().ThemCN(namecn);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }

        [HttpPut]
        [Route("SuaChucNang")]
        public IActionResult SuaCN([FromBody] ChucNangMOD item)
        {
            if(item == null) return BadRequest();
            else
            {
                var Result = new ChucNangBUS().SuaCN(item);
                if (Result != null) return Ok(Result);
                else return NotFound();

            }
        }
        [HttpDelete]
        [Route("XoaChucNang")]
        public IActionResult XoaCN(int id)
        {
            if(id == null) return BadRequest();
            else
            {
                var Result = new ChucNangBUS().XoaCN(id);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
    }
}
