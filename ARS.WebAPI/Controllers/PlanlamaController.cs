using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ARS.Business;
using ARS.Business.DTO;
using ARS.Business.Planlama;
using ARS.DataAccess.DataSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ARS.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Planlama")]
    public class PlanlamaController : Controller
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [Route("GetOnayBekleyenYuklemeler")]
        [HttpGet]
        public IEnumerable<PlanlamaBase> GetOnayBekleyenYuklemeler() //postman +
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var data = pProvider.GetOnayBekleyenYuklemelerData();
            return data;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpGet]
        [Route("GetOnaylanmisRezYapilmamisEmirler")]
        public List<PlanlamaBase> GetOnaylanmisRezYapilmamisEmirler() //postman +
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var data = pProvider.GetOnaylanmisRezYapilmamisEmirlerData();
            return data;
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Basic")]
        [HttpGet]
        [Route("GetYuklemeNoktalari")]
        public List<YuklemeNoktasiBase> GetYuklemeNoktalari() //postman +
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var yuklemeNoktalariList = pProvider.GetYuklemeNoktasiList();
            return yuklemeNoktalariList;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Basic")]
        [HttpGet]
        [Route("GetUretimYerleri")]
        public List<UretimYeri> GetUretimYerleri() //postman +
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            var yuklemeNoktalariList = pProvider.GetUretimYerleriList();
            return yuklemeNoktalariList;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPost]
        [Route("UpdateOnayBekleyenYuklemeler")]
        public void UpdateOnayBekleyenYuklemeler([FromBody]PlanlamaBase obj) //postman +
        {
            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.UpdateOBYCmdQuery(obj);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPost]
        [Route("UpdateOnaylanmisRezervasyonYapilmamisEmirler")]
        public void UpdateOnaylanmisRezervasyonYapilmamisEmirler([FromBody]PlanlamaBase obj) //postman +
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.UpdateORYEQuery(obj);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpPost]
        [Route("RezOnayla")]
        public void RezOnayla([FromBody] int id) // postman +
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.RezOnaylaQuery(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
        [HttpDelete]
        [Route("DeleteOnayBekleyenYuklemeler")]
        public void DeleteOnayBekleyenYuklemeler([FromBody] int id) // postman +
        {

            IDBManager dBManager = new MSSQLDBManager();
            IPlanlamaProvider pProvider = new PlanlamaProvider(dBManager);
            pProvider.DeleteOnayBekleyenYuklemelerQuery(id);
        }


    }
}