using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NNDController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNND")]
        public IActionResult DanhSachNND(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new NhomNguoiDungBUS().DanhSachNND(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost]
        [Route("ThemNND")]

        public IActionResult ThemNND([FromBody] ThemMoiNND item)
        {
            if (item == null) return BadRequest();
            var Result = new NhomNguoiDungBUS().ThemNND(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }
        [HttpPut]
        [Route("SuaNND")]

        public IActionResult SuaNND([FromForm] DanhSachNhomNDMOD item)
        {
            if (item == null) return BadRequest();
            var Result = new NhomNguoiDungBUS().SuaNND(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }
        [HttpDelete]
        [Route("DeleteNND")]

        public IActionResult DeleteNND(int id)
        {
            if( id == null) return BadRequest();
            var Result = new NhomNguoiDungBUS().XoaNND(id);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }
    }
}
