using CT.BUS;
using CT.MOD;
using CT.MOD.ThongKeVaLichSuMOD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CT.Controllers.SanPham
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        [HttpGet]
        [Route("ThongKeChiTieu")]
        public IActionResult ThongKeChiTieu([FromQuery] TK_Date item)
        {
           
                var Result = new ThongKeBUS().ThongKeChiTieuBUS(item);
            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Route("ThongKeSoLuongNhap")]
        public IActionResult ThongKeSoLuongNhap([FromQuery] TK_Date item)
        {

            var Result = new ThongKeBUS().ThongKeSoLuongNhap(item);
            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet]
        [Route("ThongKeSoLuongDaBan")]
        public IActionResult ThongKeSoLuongDaBan([FromQuery] TK_Date item)
        {

            var Result = new ThongKeBUS().ThongKeSoLuongDaBanBUS(item);
            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpGet]
        [Route("ThongKeDoanhThu")]
        public IActionResult ThongKeDoanhThu([FromQuery] TK_Date item)
        {

            var Result = new ThongKeBUS().ThongKeDoanhThuBUS(item);
            if (Result != null)
            {
                return Ok(Result);
            }
            else
            {
                return NotFound();
            }

        }


    }
}

