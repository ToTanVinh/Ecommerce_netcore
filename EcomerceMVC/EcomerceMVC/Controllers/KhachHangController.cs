using AutoMapper;
using EcomerceMVC.Data;
using EcomerceMVC.Helpers;
using EcomerceMVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomerceMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2024Context db;
        private readonly IMapper _mapper;    
        public KhachHangController(Hshop2024Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        #region Register
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                var KhachHang = _mapper.Map<KhachHang>(model);
                KhachHang.RandomKey = Util.GenerateRandomKey();
                KhachHang.MatKhau = model.MatKhau.ToMd5Hash(KhachHang.RandomKey);
                KhachHang.HieuLuc = true; //
                KhachHang.VaiTro = 0;

                if (Hinh != null)
                {
                    KhachHang.Hinh = Util.UpLoadHinh(Hinh, "KhachHang");
                }

                db.Add(KhachHang);
                db.SaveChanges();
                return RedirectToAction("Index", "HoangHoa");   
            }

            return View();
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {   
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model,string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachhang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachhang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");

                }
                else
                {
                    if (!khachhang.HieuLuc)
                    {
                        ModelState.AddModelError("loi", "Tại khoản đã bị khóa");
                    }
                    else 
                    {
                       if (khachhang.MatKhau != model.Password.ToMd5Hash(khachhang.RandomKey))
                       {
                            ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                       }
                       else 
                       {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, khachhang.Email),
                                new Claim(ClaimTypes.Name, khachhang.HoTen),
                                new Claim(MySetting.CLAIM_CUSTOMER, khachhang.MaKh),
                                // claim role
                                new Claim(ClaimTypes.Role, "Customer")

                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPrincipal); // asyn task await
                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            } 
                            else
                            {
                                return Redirect("/");
                            }    
                        }
                    } 
                        
                }
            }
            return View();
        }

        #endregion

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
