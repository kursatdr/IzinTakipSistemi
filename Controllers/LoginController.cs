using Microsoft.AspNetCore.Mvc;

namespace IzinTakipSistemi.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string kullaniciAdi, string sifre)
        {
            if (kullaniciAdi == "admin")
            {
                if (sifre == "1234")
                {
                    HttpContext.Session.SetString("KullaniciTipi", "admin");
                    return RedirectToAction("Index", "Izin");
                }
            }
            
            if (kullaniciAdi == "user")
            {
                if (sifre == "1234")
                {
                    HttpContext.Session.SetString("KullaniciTipi", "user");
                    return RedirectToAction("Index", "Izin");
                }
            }
            
            ViewBag.Hata = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
} 