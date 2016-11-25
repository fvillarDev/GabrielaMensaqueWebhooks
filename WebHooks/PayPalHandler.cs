using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Microsoft.AspNet.WebHooks;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using GabrielaMensaqueWebhooks.Models;
using Newtonsoft.Json;

namespace GabrielaMensaqueWebhooks.WebHooks
{
    public class PayPalHandler : PaypalWebHookReceiver
    {
        public override Task<HttpResponseMessage> ReceiveAsync(string id, HttpRequestContext context, HttpRequestMessage request)
        {
            try
            {
                var jsonContext = JsonConvert.SerializeObject(context);
                var jsonRequest = JsonConvert.SerializeObject(request);
                DataHelper.ExecuteNonQuery("TestPayPalHandler_Insert", id, jsonContext, jsonRequest);
            }
            catch (Exception ex)
            {
                try
                {
                    DataHelper.ExecuteNonQuery("Logs_Insert", ex.ToString());
                }
                catch
                {
                }
            }
            return base.ReceiveAsync(id, context, request);
        }
    }
}