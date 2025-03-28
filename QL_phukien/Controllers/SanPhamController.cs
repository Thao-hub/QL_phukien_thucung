﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();
        public ActionResult Index()
        {
            var kq = db.SanPhams.ToList();
            return View(kq);
        }
        
    }
}