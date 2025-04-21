using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QL_phukien
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SanPham",
                url: "{controller}/{action}",
                defaults: new { controller = "SanPham", action = "Index" }
            );
            routes.MapRoute(
                name: "DanhMuc",
                url: "{controller}/{action}",
                defaults: new { controller = "DanhMuc", action = "Index" }
            );

            routes.MapRoute(
                name: "DonHang",
                url: "{controller}/{action}",
                defaults: new { controller = "DonHang", action = "Index" }
            );

            routes.MapRoute(
                name: "KhachHang",
                url: "{controller}/{action}",
                defaults: new { controller = "KhachHang", action = "Index" }
            );
            routes.MapRoute(
                name: "NguoiDung",
                url: "{controller}/{action}",
                defaults: new { controller = "NguoiDung", action = "Index" }
            );
            routes.MapRoute(
                name: "Landing",
                url: "{controller}/{action}",
                defaults: new { controller = "landing", action = "Index" }
            );
        }
    }
}
