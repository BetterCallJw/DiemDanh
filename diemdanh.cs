using Microsoft.AspNetCore.Mvc;
using System;

namespace DeviceLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromForm] DeviceLocationData data)
        {
            if (data == null || string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.MSSV) ||
                string.IsNullOrEmpty(data.Coordinates) || data.LatDifference == null || data.LonDifference == null)
            {
                return BadRequest("Missing parameters");
            }

            // Giả lập lưu dữ liệu và xử lý
            var timestamp = DateTime.Now;
            // Bạn có thể lưu dữ liệu này vào cơ sở dữ liệu tại đây (tùy chọn).

            return Ok("Success");
        }
    }

    public class DeviceLocationData
    {
        public string Name { get; set; }
        public string MSSV { get; set; }
        public string Coordinates { get; set; }
        public int? LatDifference { get; set; }
        public int? LonDifference { get; set; }
    }
}
