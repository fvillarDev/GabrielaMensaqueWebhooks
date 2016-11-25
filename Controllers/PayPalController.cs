using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using GabrielaMensaqueWebhooks.Models;
using Newtonsoft.Json;
using PayPal.Api;

namespace GabrielaMensaqueWebhooks.Controllers
{
    public class PayPalController : ApiController
    {
        // POST: api/PayPal
        //([FromBody]string value)
        public void Post(WebhookEvent evt)
        {
            try
            {
                DataHelper.ExecuteNonQuery("Logs_Insert", JsonConvert.SerializeObject(evt));
            }
            catch (Exception ex)
            {
                DataHelper.ExecuteNonQuery("Logs_Insert", ex.ToString());
            }
        }
    }
}
