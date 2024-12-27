using EcomerceMVC.Data;
using EcomerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcomerceMVC.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace EcomerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2024Context db;
        public CartController(Hshop2024Context context)
        {
            db = context;
        }
        public List<CardItem> Cart => HttpContext.Session.Get<List<CardItem>>(MySetting.CART_KEY) ?? new List<CardItem>();


        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CardItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("index");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if(item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            }    
            return RedirectToAction("index");   
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout() 
        {
            if (Cart.Count == 0)
            {
                return RedirectToAction("/");   
            }
			return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid) 
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMER).Value;
                var khachhang = new KhachHang();
                if (model.GiongKhachHang)
                {
                    khachhang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
                }
                var hoadon = new HoaDon
                {
                    MaKh = customerId,
                    HoTen = model.HoTen ?? khachhang.HoTen,
                    DiaChi = model.DiaChi ?? khachhang.DiaChi,
                    DienThoai = model.DienThoai ?? khachhang.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "GRAB",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu
                };
                db.Database.BeginTransaction();
                try
                {
                    db.Database.EnsureCreated();
                    db.Add(hoadon);
                    db.SaveChanges();

                    var cthds = new List<ChiTietHd>();
                    foreach(var item in Cart)
                    {
                        cthds.Add(new ChiTietHd
                        {
                            MaHd = hoadon.MaHd,
                            SoLuong = item.SoLuong,
                            DonGia = item.DonGia,
                            MaHh = item.MaHh,   
                            GiamGia = 0
                        });
                    }
                    db.AddRange(cthds);   
                    db.SaveChanges();

                    HttpContext.Session.Set<List<CardItem>>(MySetting.CART_KEY, new List<CardItem>());

                    return View("Success");
                }
                catch
                {
                    db.Database.RollbackTransaction();
                }
            }
            return View(Cart);
        }
    }
}
