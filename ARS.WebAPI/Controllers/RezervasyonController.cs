using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/Rezervasyon")]
    public class RezervasyonController : Controller
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Basic")]
        [HttpGet]
        [Route("GetRezBekleyenYuklemeEmirleri")]
        public IEnumerable<RezervasyonBase> GetRezBekleyenYuklemeEmirleri([FromBody] int nakliyeciId )
        {
            IDBManager dBManager = new MSSQLDBManager();
            IRezervasyonProvider rProvider = new RezervasyonProvider(dBManager);
            var data = rProvider.GetRezervasyonBekleyenYE(nakliyeciId);
            return data;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Basic")]
        [HttpGet]
        [Route("GetRezYapilmisYuklemeEmirleri")]
        public IEnumerable<RezervasyonBase> GetRezYapilmisYuklemeEmirleri([FromBody] int nakliyeciId)
        {

            IDBManager dBManager = new MSSQLDBManager();
            IRezervasyonProvider rProvider = new RezervasyonProvider(dBManager);
            var data = rProvider.GetRezervasyonYapilmisYE(nakliyeciId);
            return data;
        }

        [AllowAnonymous]
        [Route("SendNotifications")]
        [HttpPost]
        public void SendNotifications()
        {

            IDBManager dBManager = new MSSQLDBManager();
            QueueService qs = new QueueService(dBManager);
            qs.DequeueAndProcess();

        }
    }
}