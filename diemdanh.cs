using Microsoft.AspNetCore.Mvc;
using System;

namespace DeviceLocationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase@page
@model IndexModel
@{
    ViewData["Title"] = "Định vị thiết bị";
}

<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Định vị thiết bị</title>
    <script>
        // Tọa độ gốc (thay đổi nếu chủ host muốn)
        const baseLatitude = 10.731566000000003;
        const baseLongitude = 106.6991891001512;

        function processCoordinate(value) {
            return Math.floor(parseFloat(value) * 10000);
        }

        const processedBaseLatitude = processCoordinate(baseLatitude);
        const processedBaseLongitude = processCoordinate(baseLongitude);

        window.onload = function() {
            if (localStorage.getItem("submitted") === "true") {
                const latitude = localStorage.getItem("latitude");
                const longitude = localStorage.getItem("longitude");

                document.body.innerHTML = `
                    <h1>Bạn đã nhập thông tin trước đó và không thể nhập lại.</h1>
                    <p>Vĩ độ: ${latitude}, Kinh độ: ${longitude}</p>
                    <button onclick="showPasswordPrompt()">Nhập mật khẩu để đặt lại</button>
                    <div id="passwordPrompt" style="display:none;">
                        <label for="password">Nhập mật khẩu:</label><br>
                        <input type="password" id="password"><br><br>
                        <button onclick="checkPassword()">Xác nhận</button>
                    </div>`;
            }
        };

        function showPasswordPrompt() {
            document.getElementById("passwordPrompt").style.display = "block";
        }

        function checkPassword() {
            const password = document.getElementById("password").value;
            const correctPassword = "12345";

            if (password === correctPassword) {
                localStorage.clear();
                alert("Trạng thái đã được reset. Bạn có thể nhập lại thông tin.");
                location.reload();  // Reload web
            } else {
                alert("Mật khẩu không đúng!");
            }
        }

        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                document.getElementById("location").innerHTML = "Trình duyệt của bạn không hỗ trợ Geolocation.";
            }
        }

        function showPosition(position) {
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;

            const name = document.getElementById("name").value;
            const mssv = document.getElementById("mssv").value;

            if (name && mssv) {
                const { latDifference, lonDifference } = compareCoordinates(latitude, longitude);

                sendData(name, mssv, `${latitude}, ${longitude}`, latDifference, lonDifference);

                localStorage.setItem("submitted", "true");
                localStorage.setItem("latitude", latitude);
                localStorage.setItem("longitude", longitude);

                document.body.innerHTML = `
                    <h1>Thông tin đã được gửi thành công!</h1>
                    <p>Vĩ độ: ${latitude}, Kinh độ: ${longitude}</p>
                    <p>Chênh lệch vĩ độ: ${latDifference}, Chênh lệch kinh độ: ${lonDifference}</p>`;
            } else {
                alert("Vui lòng điền đầy đủ Họ và tên và MSSV.");
            }
        }

        function compareCoordinates(lat, lon) {
            const processedLat = processCoordinate(lat);
            const processedLon = processCoordinate(lon);

            const latDifference = Math.abs(processedLat - processedBaseLatitude);
            const lonDifference = Math.abs(processedLon - processedBaseLongitude);

            return { latDifference, lonDifference };
        }

        function sendData(name, mssv, coordinates, latDifference, lonDifference) {
            fetch("https://script.google.com/macros/s/AKfycbzVktM1TS1nezvY2_NpDDhtacDFgl09H_xavSMaN537DzCY1lfPNhv9kLB1pW6bjokZ/exec", {
                method: "POST",
                body: new URLSearchParams({
                    name: name,
                    mssv: mssv,
                    coordinates: coordinates,
                    latDifference: latDifference.toString(),
                    lonDifference: lonDifference.toString()
                }),
                headers: { "Content-Type": "application/x-www-form-urlencoded" }
            })
            .then(response => response.ok ? response.text() : Promise.reject('Error submitting data'))
            .then(text => console.log("Response from server:", text))
            .catch(error => console.error('Lỗi khi gửi dữ liệu:', error));
        }

        function showError(error) {
            const errorMessages = {
                [error.PERMISSION_DENIED]: "Bạn đã từ chối quyền truy cập vị trí.",
                [error.POSITION_UNAVAILABLE]: "Thông tin vị trí không khả dụng.",
                [error.TIMEOUT]: "Yêu cầu vị trí bị hết thời gian.",
                [error.UNKNOWN_ERROR]: "Lỗi không xác định."
            };
            document.getElementById("location").innerHTML = errorMessages[error.code] || "Lỗi không xác định.";
        }
    </script>
</head>
<body>

<h1>Định vị thiết bị di động</h1>

<form id="infoForm">
    <label for="name">Họ và tên:</label><br>
    <input type="text" id="name" name="name" required><br><br>
    <label for="mssv">MSSV:</label><br>
    <input type="text" id="mssv" name="mssv" required><br><br>
</form>

<button onclick="getLocation()">Lấy vị trí hiện tại</button>

<p id="location"></p>

</body>
</html>

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
