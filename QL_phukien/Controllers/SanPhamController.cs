using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        // Hiển thị danh sách sản phẩm
        public ActionResult Index(string searchTerm)
        {
            var sanPhams = db.SanPhams.Include("DanhMuc").ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(searchTerm) ||
                                               s.ThuongHieu.Contains(searchTerm) ||
                                               s.SKU.Contains(searchTerm)).ToList();
            }
            return View(sanPhams);
        }

        // Lấy danh sách don vi không trùng lặp tên
        private void PopulateDanhSachDonVi(int? selectedId = null)
        {
            var donviDS = db.DonVis
                .Select(dm => new { dm.ID, dm.Ten }) // Đảm bảo lấy đúng tên danh mục
                .Distinct()
                .ToList();
            ViewBag.DanhSachDonVi = new SelectList(donviDS, "ID", "Ten", selectedId);
        }

        // Lấy danh sách danh mục không trùng lặp tên
        private void PopulateDanhMucList(int? selectedId = null)
        {
            var danhMucs = db.DanhMucs
                .Select(dm => new { dm.ID, dm.Ten }) // Đảm bảo lấy đúng tên danh mục
                .Distinct()
                .ToList();
            ViewBag.DanhMucList = new SelectList(danhMucs, "ID", "Ten", selectedId);
        }


        // GET: Tạo mới sản phẩm
        public ActionResult Create()
        {
            PopulateDanhMucList();
            PopulateDanhSachDonVi();

            return View(new SanPham());
        }

        // POST: Tạo mới sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SanPham sanPham, HttpPostedFileBase file)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu nhập vào không hợp lệ.";

                PopulateDanhMucList(sanPham.DanhMucID);
                PopulateDanhSachDonVi(sanPham.DonViID);
                return View(sanPham);
            }

            try
            {
                // Xử lý ảnh
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/img/products"), fileName);
                    file.SaveAs(filePath);
                    sanPham.HinhAnh = fileName;
                }
                else
                {
                    sanPham.HinhAnh = "default.png";
                }

                db.SanPhams.Add(sanPham);
                db.SaveChanges();

                TempData["Success"] = "Tạo sản phẩm thành công!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;

                // 🛠️ FIX: Populate dropdowns again before showing error
                PopulateDanhMucList(sanPham.DanhMucID);
                PopulateDanhSachDonVi(sanPham.DonViID);

                return View(sanPham);
            }
        }

        // GET: Chỉnh sửa sản phẩm
        public ActionResult Edit(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            if (sanPham.TrangThai == null)
            {
                sanPham.TrangThai = false;
            }

            PopulateDanhMucList(sanPham.DanhMucID);
            PopulateDanhSachDonVi(sanPham.DonViID);

            return View(sanPham);
        }

        // POST: Cập nhật sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SanPham sanPham, HttpPostedFileBase file)
        {
            PopulateDanhMucList(sanPham.DanhMucID);
            PopulateDanhSachDonVi(sanPham.DonViID);

            if (ModelState.IsValid)
            {
                var sanPhamInDb = db.SanPhams.Find(sanPham.ID);
              
                if (sanPham == null || sanPham == null)
                {
                    return HttpNotFound();
                }

                // Xử lý hình ảnh
                if (file != null && file.ContentLength > 0)
                {
                    // Xóa ảnh cũ nếu không phải ảnh mặc định
                    if (!string.IsNullOrEmpty(sanPhamInDb.HinhAnh) && !sanPhamInDb.HinhAnh.Contains("default.jpg"))
                    {
                        string oldFilePath = Server.MapPath(sanPhamInDb.HinhAnh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu ảnh mới
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/img/products"), fileName);
                    file.SaveAs(filePath);
                    sanPhamInDb.HinhAnh = fileName;
                }

                // Cập nhật thông tin danh mục
                sanPhamInDb.TenSanPham = sanPham.TenSanPham;
                sanPhamInDb.ThuongHieu = sanPham.ThuongHieu;
                sanPhamInDb.SKU = sanPham.SKU;
                sanPhamInDb.Gia = sanPham.Gia;
                sanPhamInDb.SoLuong = sanPham.SoLuong;
                sanPhamInDb.TrangThai = sanPham.TrangThai ?? false;
                sanPhamInDb.DanhMucID = sanPham.DanhMucID;
                sanPhamInDb.DonViID = sanPham.DonViID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sanPham);
        }

        // Cap nhat trang thai san pham
        [HttpPost]
        public ActionResult UpdateTrangThai(int sanPhamId, bool trangThai)
        {
            var sanPham = db.SanPhams.Find(sanPhamId);
            if (sanPham != null)
            {
                sanPham.TrangThai = trangThai;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm
        public ActionResult Delete(int id)
        {
            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null) return HttpNotFound();

            if (!string.IsNullOrEmpty(sanPham.HinhAnh) && !sanPham.HinhAnh.Contains("default.png"))
            {
                string filePath = Path.Combine(Server.MapPath("~/Content/assets/img/products"), sanPham.HinhAnh);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.SanPhams.Remove(sanPham);
            db.SaveChanges();

            TempData["Success"] = "Xóa sản phẩm thành công!";
            return RedirectToAction("Index");
        }
    }
}