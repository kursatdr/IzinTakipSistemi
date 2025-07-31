using Microsoft.AspNetCore.Mvc;
using IzinTakipSistemi.Models;
using System.Text.Json;

namespace IzinTakipSistemi.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            var izinler = DosyadanOku();
            return View(izinler);
        }

        public IActionResult Ara(string? calisanAdi, string? departman, string? izinTuru)
        {
            var tumIzinler = DosyadanOku();
            var sonuc = new List<Izin>();

            foreach (var izin in tumIzinler)
            {
                bool ekle = true;
                
                if (calisanAdi != null && calisanAdi != "")
                {
                    if (!izin.CalisanAdi.ToLower().Contains(calisanAdi.ToLower()))
                    {
                        ekle = false;
                    }
                }
                
                if (departman != null && departman != "")
                {
                    if (izin.Departman != departman)
                    {
                        ekle = false;
                    }
                }
                
                if (izinTuru != null && izinTuru != "")
                {
                    if (izin.IzinTuru != izinTuru)
                    {
                        ekle = false;
                    }
                }
                
                if (ekle)
                {
                    sonuc.Add(izin);
                }
            }

            ViewBag.CalisanAdi = calisanAdi;
            ViewBag.Departman = departman;
            ViewBag.IzinTuru = izinTuru;

            return View("Index", sonuc);
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
    }
} 