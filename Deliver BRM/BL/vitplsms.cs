using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class VitplSMS
    {
        public static string SendSMSAsync(SalesTrackerAPI.Models.TblSmsTO smsTO)
        {
            String result;

            #region Promotional VITPL account

            string userName = "sanjay.gunjal@vegainnovations.co.in";
            string hash = "d9b0b399d729176d57c1f2f385e1096d4b6d8c6f1f1bd74603bc44d5f192dffb";
            string sender = "TXTLCL";

            #endregion

            #region Polaad SMS Account

            //string userName = "kt@polaad.in";
            //string hash = "7f4a9b02a2cd9bac62b9b7d8ebdc8bd9c61daf356bc126c484e01f6a119c64e9";
            //string sender = "SALESP";

            #endregion

            //string numbers = "919764681346"; // in a comma seperated list
            string numbers = smsTO.MobileNo;
            string message = smsTO.SmsTxt;


            //Text Local SMS Gateway API Key
            //String url = "http://api.textlocal.in/send/?username=" + userName + "&hash=" + hash + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            ////http://api.textlocal.in/send/?username=kt@polaad.in&hash=7f4a9b02a2cd9bac62b9b7d8ebdc8bd9c61daf356bc126c484e01f6a119c64e9&sender=SALESP&numbers=919860000099&message=Your Order Of Qty x MT with Rate x (Rs/MT) is x Your Ref No : x
            //refer to parameters to complete correct url string


            //Pinncale SMS Gateway API Key
            String url = "http://smsjust.com/sms/user/urlsms.php?username=polaad&pass=polaad@55&senderid=SALESP&message=" + message + " BRMPL&dest_mobileno=" + numbers + "&msgtype=TXT&response=Y";

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            //objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse =  objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Dispose();
                smsTO.ReplyTxt = result;
            }
            return result;
        }

        public static async Task<string> SendSMSViasmsLaneAsync(SalesTrackerAPI.Models.TblSmsTO smsTO)
        {
            String result;
            string userName = "sanjay.gunjal@vegainnovations.co.in";
            string hash = "b718ad96d0015c126e11dd0d5ed78a64f614fcf862c305ffb493a24b9c67484b";

            //string numbers = "919764681346"; // in a comma seperated list
            string numbers = smsTO.MobileNo;
            string message = smsTO.SmsTxt;
            string sender = "TXTLCL";

            String url = "http://api.textlocal.in/send/?username=" + userName + "&hash=" + hash + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            url = "http://apps.smslane.com/vendorsms/pushsms.aspx?user=abc&password=xyz&msisdn=919898xxxxxx&sid=SenderId&msg=test%20message&fl=0";
            //refer to parameters to complete correct url string

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            //objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = await objRequest.GetRequestStreamAsync();
                //myWriter = new StreamWriter(objRequest.GetRequestStreamAsync());
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = await objRequest.GetResponseAsync();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Dispose();
                smsTO.ReplyTxt = result;
            }
            return result;
        }

        
    }
}
