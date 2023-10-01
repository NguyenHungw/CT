using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungTrongNhomController : ControllerBase
    {
        [HttpGet]
        [Route("DanhSachNND")]
        public IActionResult DanhSachNDTN(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new NguoiDungTrongNhomBUS().dsNguoiDungTrongNhom(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost]
        [Route("ThenNDvaoNhom")]

        public IActionResult ThemNDvaoNhom([FromBody] NguoiDungTrongNhomMOD item)
        {
            if (item == null) return BadRequest();
            var Result = new NguoiDungTrongNhomBUS().ThemNDvaoNhom(item);
            if (Result != null) return Ok(Result);
            else return NotFound();

        }

        [HttpPut]
        [Route("SuaNDtrongNhom")]
        public IActionResult SuaNDtrongNhom([FromBody] NguoiDungTrongNhomMOD item)
        {
            if(item == null) return BadRequest();
            var Result = new NguoiDungTrongNhomBUS().SuaNDtrongNhom(item);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }

        [HttpDelete]
        [Route("XoaNDTrongNhom")]
        public IActionResult XoaNDtrongNhom([FromBody]NguoiDungTrongNhomMOD item)
        {
            if (item == null) return BadRequest();
            var Reuslt = new NguoiDungTrongNhomBUS().XoaNDtrongNhom(item);
            if (Reuslt != null) return Ok(Reuslt);
            else return NotFound();
        }

    }

}
