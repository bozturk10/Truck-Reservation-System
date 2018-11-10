using ARS.Business.DTO;
using ARS.DataAccess.DataSQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace ARS.Business.Planlama
{
    public interface IPlanlamaProvider
    {
        List<PlanlamaBase> GetOnayBekleyenYuklemelerData();
        List<PlanlamaBase> GetOnaylanmisRezYapilmamisEmirlerData();
        //List<PlanlamaBase> UpdatePlanlamaData(String s);
        List<YuklemeNoktasiBase> GetYuklemeNoktasiList();
        List<UretimYeri> GetUretimYerleriList();
        void UpdateOBYCmdQuery(PlanlamaBase obj);
        void UpdateORYEQuery(PlanlamaBase obj);
        void RezOnaylaQuery(int keyId);
        void DeleteOnayBekleyenYuklemelerQuery(int keyId);
        void SendNotifs();
    }

    public class PlanlamaProvider : IPlanlamaProvider
    {
        private readonly IDBManager dBManager;

        public PlanlamaProvider(IDBManager dBManager)
        {
            this.dBManager = dBManager;
        }

        public List<PlanlamaBase> GetOnayBekleyenYuklemelerData()
        {

            String select_query = @"SELECT y.[Id], y.[NakliyeciId], n.[NakliyeciAdi],Email,y.[NakliyeBelgesi], y.[PlanlamaTarihi], y.[YuklemeYeriId],y.[YuklemeNoktasiId],
                    y.[AciklamaSM],y.[AracTipiId],round( Cast(y.ToplamTonaj as Float),0) as ToplamTonaj, y.[YuklemeStatus], y.[KantarOnKayitDurumu],
             (SELECT [dbo].[fn_Musteriler] (
              y.[NakliyeBelgesi]) ) as Musteriler,
               (SELECT [dbo].[fn_TeslimatYerleri] (
              y.[NakliyeBelgesi]) ) as TYerleri
             FROM  [tblYuklemeEmirleri] y                  
                    INNER JOIN [tblNakliyeFirmalari] n ON  n.[FirmaKodu] =y.[NakliyeciId] WHERE (y.[YuklemeStatus] = 0   and y.GirisSaati is  null and y.CikisSaati is  null) 
                    ORDER BY y.[PlanlamaTarihi] desc";

            var dataTable = dBManager.ExecuteQuery(select_query, null);
            List<PlanlamaBase> result = new List<PlanlamaBase>();
            PlanlamaBase pb = null;

            foreach (DataRow row in dataTable.Rows)
            {
                pb = new PlanlamaBase();
                pb.NakliyeBelgesi = int.Parse(row["NakliyeBelgesi"].ToString());
                int.TryParse(row["Id"].ToString(), out int Id); pb.Id = Id;
                pb.PlanlamaTarihi = DateTime.Parse(row["PlanlamaTarihi"].ToString());
                int.TryParse(row["YuklemeYeriId"].ToString(), out int UId); pb.YuklemeYeriId = UId;
                int.TryParse(row["NakliyeciId"].ToString(), out int NId); pb.NakliyeciId = NId;
                pb.Nakliyeci = row["NakliyeciAdi"].ToString();
                pb.Musteriler = row["Musteriler"].ToString();
                pb.SevkYeri = row["TYerleri"].ToString();
                pb.AciklamaSM = row["AciklamaSM"].ToString();
                pb.ToplamTonaj = int.Parse(row["ToplamTonaj"].ToString());
                //pb.AracTipiId = row["AracTipiId"].ToString();
                int.TryParse(row["YuklemeNoktasiId"].ToString(), out int YnId); pb.YuklemeNoktasiId = YnId;
                result.Add(pb);
            }
            return result;
        }

     
        public List<PlanlamaBase> GetOnaylanmisRezYapilmamisEmirlerData()
        {
            String select_query = @"SELECT n.[NakliyeciAdi],Email,y.[Id],y.[NakliyeciId] ,y.[OnayZamani]  , y.[AciklamaNK] ,   y.[NakliyeBelgesi],y.[Ihalede],  y.[PlanlamaTarihi], y.[YuklemeYeriId],
                    y.[YuklemeNoktasiId], y.[AciklamaSM],y.[AciklamaNK], y.[AracTipiId],round( Cast(y.ToplamTonaj as Float),0) as ToplamTonaj, 
                    y.[YuklemeStatus], y.[RezervasyonTarihi],  y.[AracId],y.[SoforId],y.[YuklemeRampaId],y.[OnayZamani],
             (SELECT [dbo].[fn_Musteriler] (
              y.[NakliyeBelgesi]) ) as Musteriler,
               (SELECT [dbo].[fn_TeslimatYerleri] (
              y.[NakliyeBelgesi]) ) as TYerleri
                     FROM  [tblYuklemeEmirleri] y INNER JOIN [tblNakliyeFirmalari] n ON  n.[FirmaKodu] =y.[NakliyeciId]                  
                    WHERE (y.[YuklemeStatus] = 1 and  y.GirisSaati is  null and y.CikisSaati is  null) ORDER BY y.[PlanlamaTarihi] desc";
            var dataTable = dBManager.ExecuteQuery(select_query, null);
            List<PlanlamaBase> result = new List<PlanlamaBase>();
            PlanlamaBase pb = null;

            foreach (DataRow row in dataTable.Rows)
            {
                pb = new PlanlamaBase();
                pb.NakliyeBelgesi = int.Parse(row["NakliyeBelgesi"].ToString());
                int.TryParse(row["Id"].ToString(), out int Id); pb.Id = Id;

                pb.PlanlamaTarihi = DateTime.Parse(row["PlanlamaTarihi"].ToString());
                int.TryParse(row["YuklemeYeriId"].ToString(), out int UId); pb.YuklemeYeriId = UId;
                int.TryParse(row["NakliyeciId"].ToString(), out int NId); pb.NakliyeciId = NId;
                pb.Nakliyeci = row["NakliyeciAdi"].ToString();
                pb.Musteriler = row["Musteriler"].ToString();
                pb.SevkYeri = row["TYerleri"].ToString();
                pb.AciklamaSM = row["AciklamaSM"].ToString();
                pb.ToplamTonaj = int.Parse(row["ToplamTonaj"].ToString());
                pb.AciklamaNK = row["AciklamaNK"].ToString();
                //pb.OnayZamani = DateTime.Parse(row["OnayZamani"].ToString());
                //pb.AracTipiId = row["AracTipiId"].ToString();
                int.TryParse(row["YuklemeNoktasiId"].ToString(), out int YnId); pb.YuklemeNoktasiId = YnId;
                result.Add(pb);
            }
            return result;
        }

        public List<YuklemeNoktasiBase> GetYuklemeNoktasiList()
        {
            string query = @"SELECT YNAdi,Id FROM [tblYuklemeNoktalari]   order by [YNAdi] ASC";
            var dataTable = dBManager.ExecuteQuery(query, null);
            List<YuklemeNoktasiBase> result = new List<YuklemeNoktasiBase>();
            YuklemeNoktasiBase yb = null;

            foreach (DataRow row in dataTable.Rows)
            {
                yb = new YuklemeNoktasiBase();
                int.TryParse(row["Id"].ToString(), out int Id); yb.Id = Id;
                yb.YNAdi = row["ynAdi"].ToString();
                result.Add(yb);
            }
            return result;
        }

        public List<UretimYeri> GetUretimYerleriList()
        {
            string query = @"SELECT UretimYeriAdi,UretimyeriKodu FROM [tblYuklemeYerleri]   order by [UretimYeriAdi] ASC";
            var dataTable = dBManager.ExecuteQuery(query, null);
            List<UretimYeri> result = new List<UretimYeri>();
            UretimYeri ub = null;

            foreach (DataRow row in dataTable.Rows)
            {
                ub = new UretimYeri();
                int.TryParse(row["uretimyeriKodu"].ToString(), out int Id); ub.UretimyeriKodu = Id;
                ub.UretimYeriAdi = row["UretimYeriAdi"].ToString();
                result.Add(ub);
            }
            return result;
        }

        public void UpdateOBYCmdQuery(PlanlamaBase obj)
        {
            Dictionary<string, object> columnValues = new Dictionary<string, object>();
            columnValues.Add("NakliyeBelgesi", obj.NakliyeBelgesi);
            columnValues.Add("PlanlamaTarihi", obj.PlanlamaTarihi);
            columnValues.Add("YuklemeYeriId", obj.YuklemeYeriId);
            columnValues.Add("NakliyeciId", obj.NakliyeciId);
            columnValues.Add("AciklamaSM", obj.AciklamaSM);
            columnValues.Add("YuklemeNoktasiId", obj.YuklemeNoktasiId);
            columnValues.Add("ToplamTonaj", obj.ToplamTonaj);
            //columnValues.Add("Id", obj.Id);

            dBManager.UpdateTableRow("tblYuklemeEmirleri", columnValues, "Id", obj.Id);
        }
        public void UpdateORYEQuery(PlanlamaBase obj)
        {
            Dictionary<string, object> columnValues = new Dictionary<string, object>();
            columnValues.Add("NakliyeBelgesi", obj.NakliyeBelgesi);
            columnValues.Add("PlanlamaTarihi", obj.PlanlamaTarihi);
            columnValues.Add("YuklemeYeriId", obj.YuklemeYeriId);
            columnValues.Add("NakliyeciId", obj.NakliyeciId);
            columnValues.Add("AciklamaSM", obj.AciklamaSM);
            columnValues.Add("YuklemeNoktasiId", obj.YuklemeNoktasiId);
            columnValues.Add("ToplamTonaj", obj.ToplamTonaj);
            //columnValues.Add("Id", obj.Id);
            columnValues.Add("AciklamaNK", obj.AciklamaNK);

            dBManager.UpdateTableRow("tblYuklemeEmirleri", columnValues, "Id", obj.Id);
        }
        public void RezOnaylaQuery(int keyId)
        {
            DateTime selected_row_date = DateTime.Now;
            Dictionary<string, object> columnValues = new Dictionary<string, object>();
            
            columnValues.Add("OnayZamani", selected_row_date);
            columnValues.Add("YuklemeStatus", 1);

            MSSQLDBManager mSSQLDB = new MSSQLDBManager();
            mSSQLDB.UpdateTableRow("tblYuklemeEmirleri", columnValues, "Id", keyId);

        }
        public void DeleteOnayBekleyenYuklemelerQuery(int keyId)
        {
            //Dictionary<string, object> columnValues = new Dictionary<string, object>();
            MSSQLDBManager mSSQLDB = new MSSQLDBManager();
            mSSQLDB.DeleteTableRow("tblYuklemeEmirleri", "Id", keyId);
        }
        public void SendNotifs() {
            dBManager.NotifExecuteQuery();
        }

    }

}