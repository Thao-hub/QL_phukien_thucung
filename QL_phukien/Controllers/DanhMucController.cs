using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    [Authorize] // Chỉ cho phép người dùng đã xác thực truy cập
    public class DanhMucController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();

        // GET: DanhMuc
        public ActionResult Index(string searchTerm)
        {
            var danhMucs = db.DanhMucs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                danhMucs = danhMucs.Where(dm => dm.Ten.Contains(searchTerm));
            }

            var danhMucList = danhMucs.OrderByDescending(dm => dm.TaoVao).ToList();
            ViewBag.SearchTerm = searchTerm; // Store search term to keep input filled in the view

            return View(danhMucList);
        }

        // GET: DanhMuc/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DanhMuc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DanhMuc danhMuc, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/img/products"), fileName);
                    file.SaveAs(filePath);
                    danhMuc.HinhAnh = fileName;
                }
                else
                {
                    danhMuc.HinhAnh = "default.png";
                }

                danhMuc.TaoVao = DateTime.Now;
                danhMuc.CapNhatVao = DateTime.Now;
                db.DanhMucs.Add(danhMuc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        // GET: DanhMuc/Edit/5
        public ActionResult Edit(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }
            if (danhMuc.TrangThai == null)
            {
                danhMuc.TrangThai = false;
            }
            return View(danhMuc);
        }

        // POST: DanhMuc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DanhMuc danhMuc, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var danhMucInDb = db.DanhMucs.Find(danhMuc.ID);
                if (danhMuc.TrangThai == null)
                {
                    danhMuc.TrangThai = false;
                }
                if (danhMucInDb == null)
                {
                    return HttpNotFound();
                }

                // Xử lý hình ảnh
                if (file != null && file.ContentLength > 0)
                {
                    // Xóa ảnh cũ nếu không phải ảnh mặc định
                    if (!string.IsNullOrEmpty(danhMucInDb.HinhAnh) && !danhMucInDb.HinhAnh.Contains("default.png"))
                    {
                        string oldFilePath = Server.MapPath(danhMucInDb.HinhAnh);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Lưu ảnh mới
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/assets/img/products"), fileName);
                    file.SaveAs(filePath);
                    danhMucInDb.HinhAnh = fileName;
                }

                // Cập nhật thông tin danh mục
                danhMucInDb.Ten = danhMuc.Ten;
                danhMucInDb.CapNhatVao = DateTime.Now;

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(danhMuc);
        }

        [HttpPost]
        public ActionResult UpdateTrangThai(int id, bool trangThai)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null)
            {
                return HttpNotFound();
            }

            danhMuc.TrangThai = trangThai;
            danhMuc.CapNhatVao = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Xóa danh mục
        public ActionResult Delete(int id)
        {
            var danhMuc = db.DanhMucs.Find(id);
            if (danhMuc == null) return HttpNotFound();

            if (!string.IsNullOrEmpty(danhMuc.HinhAnh) && !danhMuc.HinhAnh.Contains("default.png"))
            {
                string filePath = Server.MapPath(danhMuc.HinhAnh);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.DanhMucs.Remove(danhMuc);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
