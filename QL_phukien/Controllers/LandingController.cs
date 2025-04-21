using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    public class LandingController : Controller
    {
        // GET: Landing
        public ActionResult Index()
        {
            return View();
        }
    }
}