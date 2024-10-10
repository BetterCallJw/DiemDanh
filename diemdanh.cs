using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiemDanh.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiemDanhController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public DiemDanhController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Endpoint để xử lý POST từ client
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LocationData data)
        {
            // Kiểm tra dữ liệu từ client có hợp lệ không
            if (data == null || string.IsNullOrEmpty(data.Name) || string.IsNullOrEmpty(data.MSSV))
            {
                return BadRequest("Missing required fields.");
            }

            // Dữ liệu JSON gửi đến Google Apps Script
            var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                name = data.Name,
                mssv = data.MSSV,
                coordinates = data.Coordinates,
                latDifference = data.LatDifference,
                lonDifference = data.LonDifference
            });

            // Đường dẫn của Google Apps Script
            var scriptUrl = "https://script.google.com/macros/s/AKfycbzVktM1TS1nezvY2_NpDDhtacDFgl09H_xavSMaN537DzCY1lfPNhv9kLB1pW6bjokZ/exec"; // Thay YOUR_SCRIPT_ID bằng ID thật của bạn
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST đến Google Apps Script
            var response = await _httpClient.PostAsync(scriptUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return Ok("Success");
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Error submitting data");
            }
        }
    }

    // Lớp chứa dữ liệu từ client
    public class LocationData
    {
        public string Name { get; set; }
        public string MSSV { get; set; }
        public string Coordinates { get; set; }
        public string LatDifference { get; set; }
        public string LonDifference { get; set; }
    }
}
