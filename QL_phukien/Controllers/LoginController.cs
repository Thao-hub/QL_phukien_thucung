using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;

namespace QL_phukien.Controllers
{
    public class LoginController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        

        [AllowAnonymous] // Cho phép truy cập không cần xác thực
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]

        [AllowAnonymous] // Cho phép truy cập không cần xác thực
        public ActionResult Index(string EmailorTen, string MatKhau)
        {

            System.Diagnostics.Debug.WriteLine($"Đăng nhập: {EmailorTen} - {MatKhau}");

            var hashed = HashPassword(MatKhau);
            System.Diagnostics.Debug.WriteLine("Mật khẩu mã hóa: " + hashed);
            // Fix: Cast 'db.NguoiDung' to the correct type 'DbSet<NguoiDung>'
            var user = ((System.Data.Entity.DbSet<NguoiDung>)db.NguoiDungs)
                .FirstOrDefault(u => (u.Email == EmailorTen || u.Ten == EmailorTen) && u.MatKhau == hashed && u.TrangThai == true);

            if (user != null)
            {
                // Tạo cookie xác thực để [Authorize] nhận diện

                FormsAuthentication.SetAuthCookie(user.Email, false); // 'false' là không giữ cookie giữa các phiên làm việc
                Session["UserID"] = user.ID;
                Session["UserName"] = user.Email;
                Session["VaiTro"] = user.VaiTro;

                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                else 
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                // Nếu không tìm thấy người dùng, thông báo lỗi đăng nhập
                ViewBag.ThongBao = "Email hoặc mật khẩu không đúng!";
                return View();  // Trả lại View hiện tại với thông báo lỗi
            }

        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        // Hàm mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}