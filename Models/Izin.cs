using System.ComponentModel.DataAnnotations;

namespace IzinTakipSistemi.Models
{
    public class Izin
    {
        public int Id { get; set; }
        public string CalisanAdi { get; set; }
        public string Departman { get; set; }
        public string IzinTuru { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int ToplamGun { get; set; }
        public string OnayDurumu { get; set; }
        
        public Izin()
        {
            CalisanAdi = "";
            Departman = "";
            IzinTuru = "";
            OnayDurumu = "Beklemede";
        }
    }
} 