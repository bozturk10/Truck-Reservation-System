using System;
using System.Collections.Generic;
using System.Text;

namespace ARS.Business.DTO
{
    public class RezervasyonBase
    {

        public int Id { get; set; }
        public int NakliyeBelgesi { get; set; }
        public DateTime PlanlamaTarihi { get; set; }
        public int YuklemeYeriId { get; set; }
        public int NakliyeciId { get; set; }
        public string Nakliyeci { get; set; }
        public string Musteriler { get; set; }
        public string SevkYeri { get; set; }
        public string AciklamaSM { get; set; }
        public int ToplamTonaj { get; set; }
        public string AracTipiId { get; set; }
        public int YuklemeNoktasiId { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
        public String AciklamaNK { get; set; }
        public int YuklemeRampaId { get; set; }
        public DateTime GirisSaati { get; set; }
        public string kantarOnKayitDurumu { get; set; }
        public double CevapSuresi { get; set; }
        // şöföradsoyad ve araç plaka eklenecek



    }
}
