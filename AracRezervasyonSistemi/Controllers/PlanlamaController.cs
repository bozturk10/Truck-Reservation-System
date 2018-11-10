using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;

using System.Collections.Generic;
using ARS.Business.DTO;
using ARS.DataAccess.DataSQL;
using ARS.DataAccess.Planlama;
using Microsoft.AspNetCore.Mvc;

namespace AracRezervasyonSistemi.Controllers
{

    public class PlanlamaController : Controller
    {
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]

        public IEnumerable<PlanlamaBase> GetOnayBekleyenYuklemeler()
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var data = pProvider.GetOnayBekleyenYuklemelerData();
            return data;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpGet]
        public List<PlanlamaBase> GetOnaylanmisRezYapilmamisEmirler()
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var data = pProvider.GetOnaylanmisRezYapilmamisEmirlerData();
            return data;
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpGet]
        public List<YuklemeNoktasiBase> GetYuklemeNoktalari()
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var yuklemeNoktalariList = pProvider.GetYuklemeNoktasiList();
            return yuklemeNoktalariList;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpPost]
        public void UpdateOnayBekleyenYuklemeler([FromBody]PlanlamaBase obj )
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.UpdateOBYCmdQuery(obj);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpPost]
        public void UpdateOnaylanmisRezervasyonYapilmamisEmirler([FromBody]PlanlamaBase obj)
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.UpdateORYEQuery(obj);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="Admin")]
        //[HttpGet]
        public List<UretimYeri> GetUretimYerleri()
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var yuklemeNoktalariList = pProvider.GetUretimYerleriList();
            return yuklemeNoktalariList;
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpPost]
        public void RezOnayla([FromBody] int id) {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.RezOnaylaQuery(id);
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        //[HttpDelete]
        public void DeleteOnayBekleyenYuklemeler(int id)
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.DeleteOnayBekleyenYuklemelerQuery(id);
        }


    }
}