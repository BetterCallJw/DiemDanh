using Microsoft.AspNetCore.Mvc;
using System;

namespace DeviceLocationApp.Controllers
{
    public class LocationController : Controller
    {
        private const double BaseLatitude = 10.731566000000003;
        private const double BaseLongitude = 106.6991891001512;

        // Xử lý tọa độ (lấy 4 số sau dấu)
        private int ProcessCoordinate(double value)
        {
            return (int)(Math.Floor(value * 10000));
        }

        // Tính chênh lệch tọa độ
        private (int LatDifference, int LonDifference) CompareCoordinates(double lat, double lon)
        {
            int processedLat = ProcessCoordinate(lat);
            int processedLon = ProcessCoordinate(lon);

            int latDifference = Math.Abs(processedLat - ProcessCoordinate(BaseLatitude));
            int lonDifference = Math.Abs(processedLon - ProcessCoordinate(BaseLongitude));

            return (latDifference, lonDifference);
        }

        [HttpPost]
        public IActionResult SubmitLocation(LocationModel model)
        {
            if (ModelState.IsValid)
            {
                var (latDifference, lonDifference) = CompareCoordinates(model.Latitude, model.Longitude);

                // Lưu thông tin vào cơ sở dữ liệu hoặc xử lý tùy ý
                // ...

                // Gửi dữ liệu đến Google Sheets (tùy chọn)
                var isSent = SendDataToGoogleSheets(model, latDifference, lonDifference);

                if (isSent)
                {
                    ViewBag.Message = "Thông tin đã được gửi thành công!";
                    ViewBag.Latitude = model.Latitude;
                    ViewBag.Longitude = model.Longitude;
                    ViewBag.LatDifference = latDifference;
                    ViewBag.LonDifference = lonDifference;
                }
                else
                {
                    ViewBag.Message = "Lỗi khi gửi dữ liệu!";
                }

                return View("Result");
            }

            return View("Error");
        }

        // Gửi dữ liệu đến Google Sheets
        private bool SendDataToGoogleSheets(LocationModel model, int latDifference, int lonDifference)
        {
            try
            {
                var url = "https://script.google.com/macros/s/AKfycbzVktM1TS1nezvY2_NpDDhtacDFgl09H_xavSMaN537DzCY1lfPNhv9kLB1pW6bjokZ/exec";
                using (var client = new HttpClient())
                {
                    var postData = new Dictionary<string, string>
                    {
                        { "name", model.Name },
                        { "mssv", model.MSSV },
                        { "coordinates", $"{model.Latitude}, {model.Longitude}" },
                        { "latDifference", latDifference.ToString() },
                        { "lonDifference", lonDifference.ToString() }
                    };

                    var content = new FormUrlEncodedContent(postData);
                    var response = client.PostAsync(url, content).Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi gửi dữ liệu: " + ex.Message);
                return false;
            }
        }
    }
}
