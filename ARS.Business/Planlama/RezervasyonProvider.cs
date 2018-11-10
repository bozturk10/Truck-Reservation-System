using ARS.Business.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using ARS.DataAccess.DataSQL;

namespace ARS.Business.Planlama
{
    public interface IRezervasyonProvider
    {
        List<RezervasyonBase> GetRezervasyonBekleyenYE(int n);
        List<RezervasyonBase> GetRezervasyonYapilmisYE(int n);
        //List<PlanlamaBase> UpdatePlanlamaData(String s);

    }

    public class RezervasyonProvider : IRezervasyonProvider
    {
        private readonly IDBManager dBManager;

        public RezervasyonProvider(IDBManager dBManager)
        {
            this.dBManager = dBManager;
        }

        public List<RezervasyonBase> GetRezervasyonBekleyenYE( int nakliyeciId)
        {
            String select_query = @"SELECT * FROM 
            (SELECT YK.[Id], NK.[NakliyeciAdi] ,    YK.[NakliyeBelgesi] , YK.[PlanlamaTarihi] ,YK.[YuklemeYeriId] , YK.[AracTipiId] , round( Cast(YK.[ToplamTonaj] as Float),0) as ToplamTonaj ,
            YK.[YuklemeNoktasiId] ,YK.[AciklamaSM] , 
             YK.[YuklemeStatus],
             (SELECT [dbo].[fn_Musteriler] (YK.[NakliyeBelgesi]) ) as Musteriler,
             (SELECT [dbo].[fn_TeslimatYerleri] (YK.[NakliyeBelgesi]) ) as TYerleri
        
        FROM  [tblYuklemeEmirleri]  YK
        INNER JOIN [tblNakliyeFirmalari] NK ON  NK.[FirmaKodu] =YK.[NakliyeciId]
        WHERE (YK.[YuklemeStatus] = 1 and YK.[NakliyeciId]  = @NakliyeciId  and YK.GirisSaati is  null and YK.CikisSaati is  null) 
          ) AS A UNION SELECT * FROM 
        ( select y.[Id],  NK.[NakliyeciAdi] ,y.[NakliyeBelgesi] , y.[PlanlamaTarihi] ,  y.[YuklemeYeriId] ,y.[AracTipiId] , y.[ToplamTonaj] , y.[YuklemeNoktasiId] , y.[AciklamaSM] ,  
            y.[YuklemeStatus]   ,
                     (SELECT [dbo].[fn_Musteriler] (y.[NakliyeBelgesi]) ) as Musteriler,
                     (SELECT [dbo].[fn_TeslimatYerleri] (y.[NakliyeBelgesi]) ) as TYerleri  
                from tblIhale i 
         inner join  tblYuklemeEmirleri y on y.nakliyebelgesi = i.nakliyeno
        INNER JOIN [tblNakliyeFirmalari] NK ON  NK.[FirmaKodu] =i.NakliyeFirmaKodu      
         where i.NakliyeFirmaKodu  = @NakliyeciId and  i.NaliyeDurumu = 1 and y.[YuklemeStatus]=1       and y.GirisSaati is  null and y.CikisSaati is  null) AS B  order by  planlamatarihi desc";
            SqlParameter[] parameters = new SqlParameter[1] {
                new SqlParameter("NakliyeciId", nakliyeciId)
            };

            var dataTable = dBManager.ExecuteQuery(select_query, parameters);
            List<RezervasyonBase> result = new List<RezervasyonBase>();
            RezervasyonBase rb = null;

            foreach (DataRow row in dataTable.Rows)
            {
                rb = new RezervasyonBase();
                rb.NakliyeBelgesi = int.Parse(row["NakliyeBelgesi"].ToString());
                rb.PlanlamaTarihi = DateTime.Parse(row["PlanlamaTarihi"].ToString());
                rb.Musteriler = row["Musteriler"].ToString();
                rb.SevkYeri = row["TYerleri"].ToString();
                rb.AciklamaSM = row["AciklamaSM"].ToString();
                rb.ToplamTonaj = int.Parse(row["ToplamTonaj"].ToString());
                int.TryParse(row["YuklemeNoktasiId"].ToString(), out int YnId); rb.YuklemeNoktasiId = YnId;
                int.TryParse(row["YuklemeYeriId"].ToString(), out int UId); rb.YuklemeYeriId = UId;
                //pb.AracTipiId = row["AracTipiId"].ToString();
                result.Add(rb);
            }
            return result;
        }       

        public List<RezervasyonBase> GetRezervasyonYapilmisYE( int nakliyeciId)          {
            String select_query = @"SELECT nk.[NakliyeciAdi], yuk.[NakliyeBelgesi], yuk.[PlanlamaTarihi], yuk.[YuklemeYeriId], yuk.[AracTipiId], round(Cast(yuk.[ToplamTonaj] as Float),0) as ToplamTonaj,yuk.[YuklemeNoktasiId],
        yuk.[AciklamaSM], yuk.[AciklamaNK], yuk.[RezervasyonTarihi], yuk.[YuklemeRampaId],
        yuk.[GirisSaati],  yuk.[YuklemeStatus], yuk.[NakliyeciId], yuk.[KantarOnKayitDurumu],yuk.SoforId,yuk.AracId,r.BitisZamani,r.RezervasyonZamani, (r.RezervasyonZamani - yuk.PlanlamaTarihi) AS CevapSuresi
       FROM[tblYuklemeEmirleri] yuk
       INNER JOIN[tblNakliyeFirmalari] nk ON  nk.[FirmaKodu] =yuk.[NakliyeciId]
       INNER JOIN tblRezervasyonlar r on r.YuklemeEmriId=yuk.NakliyeBelgesi
       WHERE([YuklemeStatus] = 2   and[NakliyeciId] = @NakliyeciId and yuk.[GirisSaati] is null ) ORDER BY[PlanlamaTarihi] desc";

            SqlParameter[] parameters = new SqlParameter[1] {
                new SqlParameter("NakliyeciId", nakliyeciId)
            };

            var dataTable = dBManager.ExecuteQuery(select_query, parameters);
            List<RezervasyonBase> result = new List<RezervasyonBase>();
            RezervasyonBase rb = null;

            foreach (DataRow row in dataTable.Rows)
            {
                rb = new RezervasyonBase();
                rb.NakliyeBelgesi = int.Parse(row["NakliyeBelgesi"].ToString());
                rb.PlanlamaTarihi = DateTime.Parse(row["PlanlamaTarihi"].ToString());
                rb.RezervasyonTarihi = DateTime.Parse(row["RezervasyonTarihi"].ToString());
                rb.SevkYeri = row["TYerleri"].ToString();
                rb.AciklamaSM = row["AciklamaSM"].ToString();
                rb.AciklamaNK = row["AciklamaNK"].ToString();
                rb.ToplamTonaj = int.Parse(row["ToplamTonaj"].ToString());
                int.TryParse(row["YuklemeNoktasiId"].ToString(), out int YnId); rb.YuklemeNoktasiId = YnId;
                //pb.AracTipiId = row["AracTipiId"].ToString();
                TimeSpan difference = rb.RezervasyonTarihi - rb.PlanlamaTarihi;
                rb.CevapSuresi= difference.TotalHours;
                rb.Nakliyeci= row["NakliyeciAdi"].ToString();
                if (Convert.ToBoolean(row["KantarOnKayitDurumu"]))
                {
                    rb.kantarOnKayitDurumu = "Ön Kayıt Yapılmış";
                }
                else {
                    rb.kantarOnKayitDurumu = "Ön Kayıt Yapılmamış";
                }
                result.Add(rb);
            }
            return result;
        }
    }
}
