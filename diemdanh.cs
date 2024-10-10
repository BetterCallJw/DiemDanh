using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LocationApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Mssv { get; set; }

        [BindProperty]
        public string Latitude { get; set; }

        [BindProperty]
        public string Longitude { get; set; }

        [BindProperty]
        public string LatDifference { get; set; }

        [BindProperty]
        public string LonDifference { get; set; }

        public IActionResult OnPostSubmit()
        {
            // Lưu dữ liệu vào CSDL hoặc xử lý dữ liệu
            // Ví dụ: Lưu vào file hoặc gọi API lưu dữ liệu tại đây

            // Xử lý sau khi lưu, ví dụ: Chuyển hướng người dùng hoặc hiển thị thông báo
            return RedirectToPage("/Success");
        }
    }
}
