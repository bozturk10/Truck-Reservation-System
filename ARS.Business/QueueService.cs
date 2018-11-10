using ARS.DataAccess.DataSQL;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;

namespace ARS.Business
{
    public class QueueService
    {
        private readonly IDBManager dbmanager;

        public QueueService(IDBManager dbmanager)
        {
            this.dbmanager = dbmanager;
        }


        public void DequeueAndProcess()
        {
            dbmanager.ExecuteWithinTransaction((sc, tran) => {
                string dq =
            @"
            declare @BatchSize int
            set @BatchSize = 1

            update top(@BatchSize) NotifQueueMeta WITH (UPDLOCK, READPAST)
            SET Status = 1
            OUTPUT inserted.QueueID, inserted.QueueCreateDate, inserted.IdType, qd.JsonData
            FROM NotifQueueMeta qm
            INNER JOIN NotifQueueData qd
                ON qm.QueueID = qd.QueueID
            WHERE Status = 0
            ";
                var data = dbmanager.ExecuteQueryWithConn(sc, dq, null,tran);
                RestRequestQueueParams jsondata = JsonConvert.DeserializeObject<RestRequestQueueParams>(data.Rows[0]["JsonData"].ToString());
                //RestRequestQueueParams req = new RestRequestQueueParams();

                using (var client = new WebClient())
                {
                    //string postUrl = jsondata.url;
                    client.Headers.Set("Content-Type", "application/json");
                    client.UploadData(jsondata.url, "POST", Encoding.Default.GetBytes("{\"value\":" + jsondata.parameter + "}"));
                }
            });
        }


        public class RestRequestQueueParams
        {
            public string url { get; set; }
            public string parameter { get; set; }
        }
    }
}
