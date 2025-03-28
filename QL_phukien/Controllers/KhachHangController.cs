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
        // GET: KhachHang
        QL_Cua_Hang_Thu_Cung_2Entities db = new QL_Cua_Hang_Thu_Cung_2Entities();
        public ActionResult Index()
        {
            var kq = db.KhachHangs.ToList();
            return View(kq);
        }
    }
}