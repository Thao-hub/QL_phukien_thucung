using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        // Hiển thị danh sách khách hàng
        public ActionResult Index(string searchTerm)
        {
            var khachHangs = db.KhachHangs.AsQueryable(); // Use IQueryable for better performance

            if (!string.IsNullOrEmpty(searchTerm))
            {
                khachHangs = khachHangs.Where(kh => kh.Ten.Contains(searchTerm) || kh.SoLienHe.Contains(searchTerm) || kh.DiaChi.Contains(searchTerm));
            }

            var result = khachHangs.OrderByDescending(kh => kh.ID).ToList();

            return View(result);
        }

        // GET: Tạo mới khách hàng
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo mới khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Chỉnh sửa khách hàng
        public ActionResult Edit(int id)
        {
            var khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Cập nhật khách hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                var khachHangInDb = db.KhachHangs.Find(khachHang.ID);
                if (khachHangInDb == null)
                {
                    return HttpNotFound();
                }
                TryUpdateModel(khachHangInDb, new string[] { "Ten", "SoLienHe", "Email", "DiaChi", "GioiTinh" });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(khachHang);
        }

        // GET: Xóa khách hàng
        public ActionResult Delete(int id)
        {
            var khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }

            try
            {   // Xóa tất cả đơn hàng của khách hàng trước
                var donHangs = db.DonHangs.Where(dh => dh.KhachHangID == id).ToList();
                db.DonHangs.RemoveRange(donHangs);

                // Sau đó xóa khách hàng
                db.KhachHangs.Remove(khachHang);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
