using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class DonViController : Controller
    {
        // GET: DonVi
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();
        public ActionResult Index(string searchTerm)
        {
            var donVis = db.DonVis.AsQueryable(); // Use IQueryable for better performance

            if (!string.IsNullOrEmpty(searchTerm))
            {
                donVis = donVis.Where(dv => dv.Ten.Contains(searchTerm));
            }

            var result = donVis.OrderByDescending(dv => dv.CapNhatLuc).ToList();

            return View(result);
        }

        // GET: DonVi/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonVi/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonVi donVi)
        {
            if (ModelState.IsValid)
            {
                donVi.CapNhatLuc = DateTime.Now;
                db.DonVis.Add(donVi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donVi);
        }

        // GET: DonVi/Edit/5
        public ActionResult Edit(int id)
        {
            var donViInDb = db.DonVis.Find(id);
            if (donViInDb == null)
            {
                return HttpNotFound();
            }
            return View(donViInDb);
        }

        // POST: DonVi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonVi donVi)
        {
            if (ModelState.IsValid)
            {
                var donViDB = db.DonVis.Find(donVi.ID);
                if (donViDB == null)
                {
                    return HttpNotFound();
                }
                // Cập nhật thông tin DonVi
                donViDB.Ten = donVi.Ten;
                donViDB.CapNhatLuc = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donVi);
        }

        [HttpPost]
        public ActionResult UpdateTrangThai(int id, bool trangThai)
        {
            var donViDB = db.DonVis.Find(id);
            if (donViDB == null)
            {
                return HttpNotFound();
            }

            donViDB.TrangThai = trangThai;
            donViDB.CapNhatLuc = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Xóa DonVi
        public ActionResult Delete(int id)
        {
            var donVi = db.DonVis.Find(id);
            if (donVi == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.DonVis.Remove(donVi);
                db.SaveChanges();

                // Cập nhật lại số thứ tự
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Không thể xóa vì đang được sử dụng!";
            }

            return RedirectToAction("Index");
        }


    }
}


