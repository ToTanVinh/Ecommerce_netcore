using EcomerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using EcomerceMVC.Helpers;
namespace EcomerceMVC.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() 
        {
            var cart = HttpContext.Session.Get<List<CardItem>>(MySetting.CART_KEY) ?? new List<CardItem>();
            return View("CartPanel", new CartModels
            {
                Quantity = cart.Sum(p => p.SoLuong),
                Total = cart.Sum(p => p.ThanhTien)
            }); 
        }
    }
}
