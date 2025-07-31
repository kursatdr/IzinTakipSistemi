using Microsoft.AspNetCore.Mvc;
using IzinTakipSistemi.Models;
using System.Text.Json;

namespace IzinTakipSistemi.Controllers
{
    public class IzinController : Controller
    {
        public IActionResult Index()
        {
            var izinler = DosyadanOku();
            var kullaniciTipi = HttpContext.Session.GetString("KullaniciTipi");
            if (kullaniciTipi == null)
            {
                kullaniciTipi = "user";
            }
            ViewBag.KullaniciTipi = kullaniciTipi;
            
            string kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
            if (kullaniciAdi != null && kullaniciAdi != "")
            {
                int toplam = 0;
                foreach (var izin in izinler)
                {
                    if (izin.CalisanAdi == kullaniciAdi && izin.OnayDurumu == "Onaylandı")
                    {
                        toplam = toplam + izin.ToplamGun;
                    }
                }
                ViewBag.KalanIzinHakki = 20 - toplam;
                ViewBag.ToplamKullanilanGun = toplam;
            }
            
            return View(izinler);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Izin izin)
        {
            var liste = DosyadanOku();
            int kullanilan = 0;
            
            foreach (var item in liste)
            {
                if (item.CalisanAdi == izin.CalisanAdi && item.OnayDurumu == "Onaylandı")
                {
                    kullanilan = kullanilan + item.ToplamGun;
                }
            }
            
            int yeniGun = GunHesapla(izin.BaslangicTarihi, izin.BitisTarihi);
            
            if (kullanilan + yeniGun > 20)
            {
                ViewBag.Hata = "Çok fazla gün!";
                return View();
            }
            
            izin.Id = liste.Count + 1;
            izin.ToplamGun = yeniGun;
            izin.OnayDurumu = "Beklemede";
            liste.Add(izin);
            DosyayaYaz(liste);
            
            return RedirectToAction("Index");
        }

        public IActionResult Onayla(int id)
        {
            var kullaniciTipi = HttpContext.Session.GetString("KullaniciTipi");
            if (kullaniciTipi != "admin")
            {
                return RedirectToAction("Index");
            }

            var izinler = DosyadanOku();
            var bulundu = false;
            
            foreach (var izin in izinler)
            {
                if (izin.Id == id)
                {
                    izin.OnayDurumu = "Onaylandı";
                    DosyayaYaz(izinler);
                    bulundu = true;
                    break;
                }
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Reddet(int id)
        {
            var kullaniciTipi = HttpContext.Session.GetString("KullaniciTipi");
            if (kullaniciTipi != "admin")
            {
                return RedirectToAction("Index");
            }

            var izinler = DosyadanOku();
            var bulundu = false;
            
            foreach (var izin in izinler)
            {
                if (izin.Id == id)
                {
                    izin.OnayDurumu = "Reddedildi";
                    DosyayaYaz(izinler);
                    bulundu = true;
                    break;
                }
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Sil(int id)
        {
            var izinler = DosyadanOku();
            var bulundu = false;
            
            for (int i = 0; i < izinler.Count; i++)
            {
                if (izinler[i].Id == id)
                {
                    izinler.RemoveAt(i);
                    DosyayaYaz(izinler);
                    bulundu = true;
                    break;
                }
            }
            
            return RedirectToAction("Index");
        }

        private List<Izin> DosyadanOku()
        {
            List<Izin> sonuc = new List<Izin>();
            
            if (System.IO.File.Exists("izinler.json"))
            {
                string json = System.IO.File.ReadAllText("izinler.json");
                var liste = JsonSerializer.Deserialize<List<Izin>>(json);
                if (liste != null)
                {
                    sonuc = liste;
                }
            }
            
            return sonuc;
        }

        private void DosyayaYaz(List<Izin> izinler)
        {
            string json = JsonSerializer.Serialize(izinler);
            System.IO.File.WriteAllText("izinler.json", json);
        }

        private int GunHesapla(DateTime baslangic, DateTime bitis)
        {
            int gun = 0;
            DateTime tarih = baslangic;
            
            while (tarih <= bitis)
            {
                if (tarih.DayOfWeek == DayOfWeek.Saturday)
                {
                    
                }
                else if (tarih.DayOfWeek == DayOfWeek.Sunday)
                {
                    
                }
                else
                {
                    gun = gun + 1;
                }
                
                tarih = tarih.AddDays(1);
            }
            
            return gun;
        }
    }
} 