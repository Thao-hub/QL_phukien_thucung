using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QL_phukien.Models;
using System.Security.Cryptography;
using System.Text;

namespace QL_phukien.Controllers
{
    [Authorize] // Chỉ cho phép người dùng đã xác thực truy cập
    public class NguoiDungController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        // GET: NguoiDung
        public ActionResult Index(string searchTerm)
        {
            var nguoiDungs = db.NguoiDungs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                nguoiDungs = nguoiDungs.Where(nd => nd.Ten.Contains(searchTerm) || nd.Email.Contains(searchTerm));
            }

            return View(nguoiDungs.OrderByDescending(nd => nd.ID).ToList());
        }

        // GET: NguoiDung/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null) return HttpNotFound();

            return View(nguoiDung);
        }

        // GET: NguoiDung/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NguoiDung/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ten,SoLienHe,Email,MatKhau,VaiTro,TrangThai")] NguoiDung nguoiDung, string ConfirmMatKhau)
        {
            if (nguoiDung.MatKhau != ConfirmMatKhau)
            {
                ModelState.AddModelError("ConfirmMatKhau", "Mật khẩu xác nhận không khớp.");
            }

            // Kiểm tra email đã tồn tại chưa
            if (db.NguoiDungs.Any(nd => nd.Email == nguoiDung.Email))
            {
                ModelState.AddModelError("Email", "Email đã tồn tại trong hệ thống.");
                return View(nguoiDung);
            }

            if (ModelState.IsValid)
            {
                // Mã hóa mật khẩu trước khi lưu vào database
                nguoiDung.MatKhau = HashPassword(nguoiDung.MatKhau);
                db.NguoiDungs.Add(nguoiDung);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nguoiDung);
        }

        // GET: NguoiDung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null) return HttpNotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ten,SoLienHe,Email,VaiTro,TrangThai")] NguoiDung nguoiDung, string MatKhauMoi, string ConfirmMatKhauMoi)
        {
            try
            {
                var existingUser = db.NguoiDungs.Find(nguoiDung.ID);
                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                // Kiểm tra nếu người dùng muốn thay đổi mật khẩu
                if (!string.IsNullOrEmpty(MatKhauMoi))
                {
                    if (MatKhauMoi != ConfirmMatKhauMoi)
                    {
                        ModelState.AddModelError("ConfirmMatKhauMoi", "Mật khẩu xác nhận không khớp.");
                        return View(nguoiDung);
                    }

                    existingUser.MatKhau = HashPassword(MatKhauMoi);
                }

                // Cập nhật các trường khác từ form
                
                existingUser.Ten = nguoiDung.Ten;
                existingUser.SoLienHe = nguoiDung.SoLienHe;
                existingUser.Email = nguoiDung.Email;
                existingUser.VaiTro = nguoiDung.VaiTro;
                existingUser.TrangThai = nguoiDung.TrangThai;
                if (ModelState.IsValid)
                {
                    db.Entry(existingUser).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return View(nguoiDung);
        }


        [HttpPost]
        public ActionResult UpdateTrangThai(int id, bool trangThai)
        {
            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null) return HttpNotFound();

            nguoiDung.TrangThai = trangThai;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: NguoiDung/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null) return HttpNotFound();

            return View(nguoiDung);
        }

        // POST: NguoiDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung != null)
            {
                // Chặn xóa Admin
                if (nguoiDung.VaiTro == "Admin")
                {
                    ModelState.AddModelError("", "Không thể xóa tài khoản Admin.");
                    return View("Delete", nguoiDung);
                }

                db.NguoiDungs.Remove(nguoiDung);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
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
