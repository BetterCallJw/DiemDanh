using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace LocationTrackingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SendDataController : ControllerBase
    {
        private const double BaseLatitude = 10.731566;
        private const double BaseLongitude = 106.699189;

        // Hàm xử lý tọa độ (lấy 4 số sau dấu)
        private int ProcessCoordinate(double value)
        {
            return (int)(value * 10000);
        }

        [HttpPost]
        public async Task<IActionResult> SendData([FromBody] JObject data)
        {
            string name = data["name"]?.ToString();
            string mssv = data["mssv"]?.ToString();
            string coordinates = data["coordinates"]?.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(mssv) || string.IsNullOrEmpty(coordinates))
            {
                return BadRequest(new { success = false, message = "Thiếu thông tin." });
            }

            // Tách tọa độ
            var coords = coordinates.Split(", ");
            double latitude = double.Parse(coords[0]);
            double longitude = double.Parse(coords[1]);

            // So sánh tọa độ
            int latDifference = System.Math.Abs(ProcessCoordinate(latitude) - ProcessCoordinate(BaseLatitude));
            int lonDifference = System.Math.Abs(ProcessCoordinate(longitude) - ProcessCoordinate(BaseLongitude));

            // Gửi dữ liệu đến Google Sheets
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "name", name },
                    { "mssv", mssv },
                    { "coordinates", coordinates },
                    { "latDifference", latDifference.ToString() },
                    { "lonDifference", lonDifference.ToString() }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://script.google.com/macros/s/AKfycbzVktM1TS1nezvY2_NpDDhtacDFgl09H_xavSMaN537DzCY1lfPNhv9kLB1pW6bjokZ/exec", content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return StatusCode((int)response.StatusCode, new { success = false, message = "Gửi dữ liệu thất bại." });
                }
            }
        }
    }
}
