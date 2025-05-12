using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QL_phukien.Models;

namespace QL_phukien.Controllers
{
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly QL_Cua_Hang_Thu_Cung_2Entities1 db = new QL_Cua_Hang_Thu_Cung_2Entities1();
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session["Cart"] as Dictionary<int, int>;

            if (cart == null || !cart.Any())
            {
                ViewBag.Total = 0;
                ViewBag.CartQuantities = new Dictionary<int, int>();
                return View(new List<SanPham>());
            }

            var productIds = cart.Keys.ToList();
            var products = db.SanPhams.Where(p => productIds.Contains(p.ID)).ToList();

            decimal total = 0;
            foreach (var product in products)
            {
                int quantity = cart[product.ID];
                total += product.Gia * quantity;
            }

            ViewBag.Total = total;
            ViewBag.CartQuantities = cart;

            return View(products);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public JsonResult AddToCart(int productId, int quantity = 1)
        {
            var cart = Session["Cart"] as Dictionary<int, int>;
            if (cart == null)
                cart = new Dictionary<int, int>();

            if (cart.ContainsKey(productId))
                cart[productId] += quantity;
            else
                cart[productId] = quantity;

            Session["Cart"] = cart;
            return Json(new { success = true, message = "Đã thêm vào giỏ hàng" });
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = Session["Cart"] as Dictionary<int, int>;
            if (cart != null && cart.ContainsKey(productId))
            {
                if (quantity > 0)
                    cart[productId] = quantity;
                else
                    cart.Remove(productId);
            }

            Session["Cart"] = cart;
            return RedirectToAction("Index");
        }

        // POST: Cart/Remove
        [HttpPost]
        public ActionResult Remove(int productId)
        {
            var cart = Session["Cart"] as Dictionary<int, int>;
            if (cart != null && cart.ContainsKey(productId))
            {
                cart.Remove(productId);
                Session["Cart"] = cart;
            }
            return RedirectToAction("Index");
        }

        // GET: Cart/Checkout
        // GET: Cart/Checkout
        // GET: Cart/Checkout
        public ActionResult Checkout()
        {
            var cart = Session["Cart"] as Dictionary<int, int>;
            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "Giỏ hàng trống, không thể thanh toán.";
                return RedirectToAction("Index");
            }

            return View(); // Trả về form để khách nhập thông tin
        }

        // POST: Cart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(KhachHang khachHang)
        {
            var cart = Session["Cart"] as Dictionary<int, int>;
            if (cart == null || !cart.Any())
            {
                TempData["Message"] = "Giỏ hàng trống, không thể thanh toán.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(khachHang); // Trả lại form nếu dữ liệu không hợp lệ
            }

            // 1. Lưu khách hàng (bao gồm cả giới tính)
            db.KhachHangs.Add(khachHang);
            db.SaveChanges();

            // 2. Tạo đơn hàng
            DonHang donHang = new DonHang
            {
                KhachHangID = khachHang.ID,
                OrderID = "DH-" + (db.DonHangs.Any() ? db.DonHangs.Max(d => d.ID) + 1 : 1).ToString("D6"),
                NgayDat = DateTime.Now,
                TrangThai = "Đã Đặt",
                ThanhToan = false
            };

            db.DonHangs.Add(donHang);
            db.SaveChanges();

            // 3. Tạo chi tiết đơn hàng
            var productIds = cart.Keys.ToList();
            var products = db.SanPhams.Where(p => productIds.Contains(p.ID)).ToList();

            decimal tongTien = 0;
            foreach (var sp in products)
            {
                int soLuong = cart[sp.ID];
                decimal gia = sp.Gia;

                var ct = new ChiTietDonHang
                {
                    DonHangID = donHang.ID,
                    SanPhamID = sp.ID,
                    SoLuong = soLuong,
                    Gia = gia
                };

                tongTien += gia * soLuong;
                db.ChiTietDonHangs.Add(ct);
            }

            db.SaveChanges();

            // 4. Cập nhật tổng tiền đơn hàng
            donHang.TongTien = tongTien;
            db.Entry(donHang).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            // 5. Xóa giỏ hàng
            Session.Remove("Cart");
            TempData["Message"] = "Đặt hàng thành công!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetCartItemCount()
        {
            var cart = Session["Cart"] as Dictionary<int, int> ?? new Dictionary<int, int>();
            int totalItems = cart.Values.Sum(); // Sum of all quantities
            return Json(new { count = totalItems }, JsonRequestBehavior.AllowGet);
        }

    }
}
