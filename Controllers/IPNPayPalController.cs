using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GabrielaMensaqueWebhooks.Models;
using Newtonsoft.Json;

namespace GabrielaMensaqueWebhooks.Controllers
{
    public class IPNPayPalController : Controller
    {
        [HttpPost]
        public HttpStatusCodeResult Receive()
        {
            //Store the IPN received from PayPal
            LogRequest(Request);

            //Fire and forget verification task
            Task.Run(() => VerifyTask(Request));

            //Reply back a 200 code
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private void VerifyTask(HttpRequestBase ipnRequest)
        {
            var verificationResponse = string.Empty;

            try
            {
                var verificationRequest = (HttpWebRequest)WebRequest.Create("https://www.sandbox.paypal.com/cgi-bin/webscr");

                //Set values for the verification request
                verificationRequest.Method = "POST";
                verificationRequest.ContentType = "application/x-www-form-urlencoded";
                var param = Request.BinaryRead(ipnRequest.ContentLength);
                var strRequest = Encoding.UTF8.GetString(param);

                //Add cmd=_notify-validate to the payload
                strRequest = "cmd=_notify-validate&" + strRequest;
                verificationRequest.ContentLength = strRequest.Length;

                //Attach payload to the verification request
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var streamOut = new StreamWriter(verificationRequest.GetRequestStream(), Encoding.UTF8);
                streamOut.Write(strRequest);
                streamOut.Close();

                //Send the request to PayPal and get the response
                var streamIn = new StreamReader(verificationRequest.GetResponse().GetResponseStream());
                verificationResponse = streamIn.ReadToEnd();
                streamIn.Close();

            }
            catch (Exception ex)
            {
                //Capture exception for manual investigation
                DataHelper.ExecuteNonQuery("Logs_Insert", ex.ToString());
            }

            ProcessVerificationResponse(verificationResponse);
        }


        private void LogRequest(HttpRequestBase request)
        {
            try
            {
                // Persist the request values into a database or temporary data store
                var param = Request.BinaryRead(request.ContentLength);
                var strRequest = Encoding.ASCII.GetString(param);
                DataHelper.ExecuteNonQuery("Logs_Insert", strRequest);
            }
            catch (Exception ex)
            {
                //Capture exception for manual investigation
                DataHelper.ExecuteNonQuery("Logs_Insert", ex.ToString());
            }
        }

        private void ProcessVerificationResponse(string verificationResponse)
        {
            if (verificationResponse.Equals("VERIFIED"))
            {
                // check that Payment_status=Completed
                // check that Txn_id has not been previously processed
                // check that Receiver_email is your Primary PayPal email
                // check that Payment_amount/Payment_currency are correct
                // process payment
                DataHelper.ExecuteNonQuery("Logs_Insert", "VERIFIED: " + verificationResponse);
            }
            else if (verificationResponse.Equals("INVALID"))
            {
                //Log for manual investigation
                DataHelper.ExecuteNonQuery("Logs_Insert", "INVALID: " + verificationResponse);
                LogRequest(Request);
            }
            else
            {
                //Log error
                DataHelper.ExecuteNonQuery("Logs_Insert", "ERROR: " + verificationResponse);
            }
        }
    }
}