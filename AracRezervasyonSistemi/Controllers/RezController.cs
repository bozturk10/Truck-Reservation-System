using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARS.DataAccess.DataSQL;
using ARS.DataAccess.Planlama;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ARS.Business.DTO;

namespace AracRezervasyonSistemi.Controllers
{
    public class RezController : Controller
    {

        public IEnumerable<RezervasyonBase> GetRezBekleyenYuklemeEmirleri()
        {


            IDBManager dBManager = new MSSQLDBManager();
            IRezervasyonProvider rProvider = new RezervasyonProvider(dBManager);
            var data = rProvider.GetRezervasyonBekleyenYE(1);
            return data;
        }


        public IEnumerable<RezervasyonBase> GetRezYapilmisYuklemeEmirleri()
        {

            IDBManager dBManager = new MSSQLDBManager();
            IRezervasyonProvider rProvider = new RezervasyonProvider(dBManager);
            var data = rProvider.GetRezervasyonYapilmisYE(1);
            return data;
        }

    }
}