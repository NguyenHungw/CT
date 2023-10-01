using CT.BUS;
using CT.MOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChucNangCuaNNDController : ControllerBase
    {

        [HttpGet]
        [Route("DanhSachCNCuaNND")]
        public IActionResult dscncuannd(int page)
        {
            if (page < 1) return BadRequest();
            else
            {
                var Result = new ChucNangCuaNNDBUS().dsCNCuannd(page);
                if (Result != null) return Ok(Result);
                else return NotFound();
            }
        }
        [HttpPost]
        [Route("ThemChucNangCuaNND")]
        public IActionResult ThemCN([FromBody] ThemChucNangCuaNNDMOD item)
        {
            if(item == null) return BadRequest();
            else
            {
                var result = new ChucNangCuaNNDBUS().Them(item);
                if (result != null) return Ok(result);
                else return NotFound();
            }
        }
        [HttpPut]
        [Route("SuaCNCN")]
        public IActionResult SuaCN([FromBody] ChucNangCuaNNDMOD item)
        {
            if (item == null) return BadRequest();
            else
            {
                var result = new ChucNangCuaNNDBUS().Sua(item);
                if (result != null) return Ok(result);
                else return NotFound();

            }
        }

        [HttpDelete]
        [Route("XoaCNCN")]
        public IActionResult XoaCN(int id)
        {
            if (id == null) return BadRequest();
            else
            {
                var result = new ChucNangCuaNNDBUS().Xoa(id);
                if (result != null) return Ok(result);
                else return NotFound();

            }
        }

        [HttpPost]
        [Route("ChiTietCNCNND")]
        public IActionResult ChiTietCNCNND(int id)
        {
            if (id == null || id <=0) return BadRequest();
            var Result = new ChucNangCuaNNDBUS().ChiTCNCN(id);
            if (Result != null) return Ok(Result);
            else return NotFound();
        }



    }
}
