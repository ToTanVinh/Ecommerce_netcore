using System.Text;
namespace EcomerceMVC.Helpers
{
    public class Util
    { 
        public static string UpLoadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);
                }
                return Hinh.FileName;
            } catch (Exception ex ) { 
                return string.Empty;    
            }
        }
        public static string GenerateRandomKey(int length = 5)
        {
            var pattern = @"fgafhawfhnanmrtmqwpquzxcqmAWBNFABWFKAFABNFMWWEQ!";
            var rd = new Random();
            var sb = new StringBuilder();
            for(int i = 0; i < length; i++) 
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            } 
            return sb.ToString();
        }
    }
}
