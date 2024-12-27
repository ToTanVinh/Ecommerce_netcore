namespace EcomerceMVC.ViewModels
{
    public class HoangHoaVM
    {
        public int MaHH { get; set; }   
        public string? tenHH { get; set; }   
        public string? Hinh { get; set; }    
        public double DonGia { get; set; }  
        public string? TenLoai { get; set; } 

        public string? moTaNgan { get; set; }    

    }

    public class ChiTietHangHoaVM
    {
        public int MaHH { get; set; }
        public string? tenHH { get; set; }
        public string? Hinh { get; set; }
        public double DonGia { get; set; }
        public string? TenLoai { get; set; }

        public string? moTaNgan { get; set; }

        public string? ChiTiet {  get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }


    }
}
