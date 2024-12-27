using EcomerceMVC.Data;
using EcomerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomerceMVC.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        public readonly Hshop2024Context db;

        public MenuLoaiViewComponent(Hshop2024Context context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM
            {
                MaLoai = lo.MaLoai, TenLoai = lo.TenLoai, SoLuong = lo.HangHoas.Count
            });
            return View(data);
        }

    }
}

