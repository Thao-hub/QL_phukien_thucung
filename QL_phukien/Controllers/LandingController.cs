using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    [AllowAnonymous]
    public class LandingController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();
        // GET: Landing
        public ActionResult Index(string searchTerm, int? danhMucId)
        {
            var sanPhams = db.SanPhams.Include("DanhMuc").AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sanPhams = sanPhams.Where(s => s.TenSanPham.Contains(searchTerm) ||
                                               s.ThuongHieu.Contains(searchTerm) ||
                                               s.SKU.Contains(searchTerm));
            }

            if (danhMucId.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DanhMucID == danhMucId.Value);
            }

            ViewBag.DanhMucs = db.DanhMucs.ToList();
            ViewBag.SelectedDanhMucId = danhMucId;

            return View(sanPhams.ToList());
        }
    }
}