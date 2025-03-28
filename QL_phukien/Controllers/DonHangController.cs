using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class DonHangController : Controller
    {
        QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();
        // GET: DonHang
        public ActionResult Index()
        {
            var kq = db.DonHangs.ToList();
            return View(kq);
        }
    }
}