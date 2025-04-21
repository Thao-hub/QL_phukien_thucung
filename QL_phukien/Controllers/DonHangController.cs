using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class DonHangController : Controller
    {
        private QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        // GET: DonHang (Danh sách đơn hàng)
        public ActionResult Index()
        {
            var donHangs = db.DonHangs.ToList();
            return View(donHangs);
        }

        // GET: DonHang/Details/5 (Xem chi tiết đơn hàng)
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            return View(donHang);
        }

        // GET: DonHang/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = GenerateOrderID();
            ViewBag.KhachHangList = new SelectList(db.KhachHangs, "ID", "Ten");
            ViewBag.Products = db.SanPhams.ToList();
            ViewBag.TongTien = 0; // Initial value

            return View();
        }

        // POST: DonHang/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,KhachHangID,NgayDat,TongTien,TrangThai,ThanhToan")] DonHang donHang, string[] MaSanPham, string[] TenSanPham, int[] SoLuong, decimal[] DonGia)
        {
            if (MaSanPham == null || MaSanPham.Length == 0)
            {
                ModelState.AddModelError("ChiTietDonHang", "Bạn phải chọn ít nhất một sản phẩm.");
                ViewBag.KhachHangList = new SelectList(db.KhachHangs, "ID", "Ten", donHang.KhachHangID);
                ViewBag.OrderID = GenerateOrderID();
                ViewBag.Products = db.SanPhams.Where(sp => sp.TrangThai == true && sp.SoLuong > 0).ToList();
                return View(donHang); // Trả về form và hiển thị lỗi
            }

            if (ModelState.IsValid)
            {
                // 1. Lưu đơn hàng
                donHang.NgayDat = DateTime.Now;
                donHang.TrangThai = "Đã Đặt";
                db.DonHangs.Add(donHang);
                db.SaveChanges();

                // 2. Lưu chi tiết đơn hàng
                decimal totalPrice = 0;
                for (int i = 0; i < MaSanPham.Length; i++)
                {
                    if (!string.IsNullOrEmpty(MaSanPham[i]) && SoLuong[i] > 0 && DonGia[i] > 0)
                    {
                        var chiTiet = new ChiTietDonHang
                        {
                            DonHangID = donHang.ID,
                            SanPhamID = Convert.ToInt32(MaSanPham[i]),
                            SoLuong = SoLuong[i],
                            Gia = DonGia[i]
                        };
                        totalPrice += chiTiet.SoLuong * chiTiet.Gia;
                        db.ChiTietDonHangs.Add(chiTiet);
                    }
                }
                db.SaveChanges();

                // 3. Cập nhật tổng tiền đơn hàng
                donHang.TongTien = totalPrice;
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.KhachHangList = new SelectList(db.KhachHangs, "ID", "Ten", donHang.KhachHangID);
            ViewBag.Products = db.SanPhams.Where(sp => sp.TrangThai == true && sp.SoLuong > 0).ToList();
            return View(donHang);
        }

        public JsonResult GetKhachHangDetails(int khachHangID)
        {
            var khachHang = db.KhachHangs.Where(k => k.ID == khachHangID)
                .Select(k => new { k.SoLienHe, k.Email, k.DiaChi })
                .FirstOrDefault();

            return Json(khachHang, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSanPhamList()
        {
            var sanPhamList = db.SanPhams
                .Where(sp => sp.TrangThai == true && sp.SoLuong > 0) // Only get active products
                .Select(sp => new { sp.ID, sp.TenSanPham, sp.Gia, sp.SoLuong })
                .ToList();

            return Json(sanPhamList, JsonRequestBehavior.AllowGet);
        }

        // Helper method to generate a new Order ID
        private string GenerateOrderID()
        {
            int latestID = db.DonHangs.Any() ? db.DonHangs.Max(d => d.ID) + 1 : 1;
            return "DH-" + latestID.ToString("D6");
        }

        // GET: DonHang/Edit/5 (Form chỉnh sửa đơn hàng)
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "ID", "Ten", donHang.KhachHangID);
            return View(donHang);
        }

        // POST: DonHang/Edit/5 (Xử lý chỉnh sửa đơn hàng)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,KhachHangID,NgayDat,TongTien,TrangThai,ThanhToan")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KhachHangID = new SelectList(db.KhachHangs, "ID", "TenKhachHang", donHang.KhachHangID);
            return View(donHang);
        }

        // GET: DonHang/Delete/5 (Xác nhận xóa đơn hàng)
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
                return HttpNotFound();

            return View(donHang);
        }

        // POST: DonHang/Delete/5 (Xử lý xóa đơn hàng)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Giải phóng tài nguyên database khi không cần thiết
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
