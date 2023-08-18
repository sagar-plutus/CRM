using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SalesTrackerAPI.DAL;
namespace SalesTrackerAPI.BL
{
    public class VitplNotify
    {
        public static string NotifyToRegisteredDevices()
        {
            try
            {
                string applicationID = "AIzaSyBY3gLvgh8KrY0wUiUOBAaj-a1U1c8uafM";
                var senderId = "697536919216";

                string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";

                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId1,
                    notification = new
                    {
                        body = "New Order Booked",
                        title = "Notification",
                        sound = "default"
                    }
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse =  tRequest.GetResponseAsync().Result)
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                return  sResponseFromServer;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                string str = ex.Message;
                return str;
            }
        }


   public static string NotifyToRegisteredDevices(String[] devices, String body, String title, int instanceID)
        {
                string applicationID = "AIzaSyDFLdVZH4Ta7goZsQ0I9Oxkj0OnXgaRjPQ";
                //var senderId = "708323976317";
                var senderId = "292354044972";

            try
            {

                NotificationTO notification = notificationDAO.SelectNotificationTO(instanceID);


                //no android snooze for non transaction notification 
                if (notification == null)
                {
                    notification = new NotificationTO();
                    notification.NavigationUrl = "";
                    notification.AndroidUrl = "";
                    //to insure no snooze for non transactional alerts
                    instanceID = 0;
                }
                //string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";
                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";
                
                var data = new
                {
                    registrationds = devices,
                    notification = new
                    {
                        body = body,
                        title = title,
                        sound = "default"
                    },
                    data = new
                    {

                        instanceId = instanceID,
                        apiUrl = "",
                        commonUrl = Startup.DeliverUrl,
                        navigationUrl = notification.NavigationUrl,
                        androidUrl = notification.AndroidUrl,
                         notificationTitle = title,
                        notificationBody = body


                    }
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponseAsync().Result)
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                return sResponseFromServer;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                string str = ex.Message;
                return str;
            }
        }

        // public static string NotifyToRegisteredDevices(String [] devices, String body, String title)
        // {
        //     try
        //     {
        //         //string applicationID = "AIzaSyCDYpc9xCvzl21bfBfHRp9dS077zgijpdg";
        //         string applicationID = "AIzaSyDFLdVZH4Ta7goZsQ0I9Oxkj0OnXgaRjPQ";
        //         //var senderId = "708323976317";
        //         var senderId = "292354044972";

        //         //string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";
        //         var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
        //         tRequest.Method = "post";

        //         //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
        //         tRequest.ContentType = "application/json";

        //         var data = new
        //         {
        //             registration_ids = devices,
        //             notification = new
        //             {
        //                 body = body,
        //                 title = title,
        //                 sound = "default"
        //             }
        //         };

        //         var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

        //         Byte[] byteArray = Encoding.UTF8.GetBytes(json);

        //         //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
        //         //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
        //         tRequest.Headers["Authorization"] = "key=" + applicationID;
        //         tRequest.Headers["Sender"] = "id=" + senderId;
        //         tRequest.UseDefaultCredentials = true;
        //         //tRequest.PreAuthenticate = true;
        //         tRequest.Credentials = CredentialCache.DefaultCredentials;

        //         using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
        //         {
        //             dataStream.Write(byteArray, 0, byteArray.Length);
        //             using (WebResponse tResponse = tRequest.GetResponseAsync().Result)
        //             {
        //                 using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                 {
        //                     using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                     {
        //                         String sResponseFromServer = tReader.ReadToEnd();
        //                         return sResponseFromServer;
        //                     }
        //                 }
        //             }
        //         }

        //     }
        //     catch (Exception ex)
        //     {

        //         string str = ex.Message;
        //         return str;
        //     }
        // }

        public static async void NotifyToRegisteredDevicesAsync()
        {
            try
            {
                // var applicationID = "AIzaSyB7tSzfqkgtoFDR-Pb5Kjo8fxl_uiTDLlw";
                string applicationID = "AIzaSyBY3gLvgh8KrY0wUiUOBAaj-a1U1c8uafM";
                var senderId = "697536919216";
                //  string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";

                //string deviceId1 = "fyrwLbLd8Yc:APA91bHMuZeNVf4hbN2XebAcfaRk0lANvpfVG7s3hq3LU4trka-wI37VQ4qORQpiPuumy4kGMajC8mi80zk2V1YTbwufh5g5CzLmjKKJQfVWTc17VhR4B3Mqc6PL6RZbJHJi9OFdEeSf";
                string deviceId1 = "dLNdCoSAYio:APA91bHUMngAw9PCRrLTeHApXGAVoG-sPtj12uOq2XKfNDhXpys_M5x8nik9hwOfjRvxgXimJ40lftnUdesS1H7VAEjYw0nieN9C5TEu8zDvenXZn7IcxYcnbn4MADDip_xZN8VrIBCf";
                //"dWQbNtmzSqA:APA91bE9NNCRiE-P0T1WdbeoAfzcyTqpYVxsHi9GBOMlLSPNte8GI_wExMup73snOHWEBRDRSxkn6qiNzDmZEP2CDkTC3Ph4UG1rzTk_WdNR6gsFRKODCzxFL3qW4Z8Jezd-UFo4kFjt";

                var tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send") as HttpWebRequest;
                // HttpWebRequest tRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";

                //tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.ContentType = "application/json";

                var data = new
                {
                    to = deviceId1,
                    notification = new
                    {
                        body = "New Order Booked",
                        title = "Notification"
                    }
                };

                //var serializer = new JavaScriptSerializer();

                //var json = serializer.Serialize(data);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                //tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                //tRequest.Headers["Authorization: key={0}"] = applicationID;
                //tRequest.Headers[(string.Format("Authorization: key={0}"))]= applicationID;
                //tRequest.Headers["Sender: id"] = senderId.ToString();
                //WebHeaderCollection myWebHeaderCollection = tRequest.Headers;
                tRequest.Headers["Authorization"] = "key=" + applicationID;
                tRequest.Headers["Sender"] = "id=" + senderId;
                //tRequest.ContentLength = byteArray.Length;
                tRequest.UseDefaultCredentials = true;
                //tRequest.PreAuthenticate = true;
                tRequest.Credentials = CredentialCache.DefaultCredentials;

                //Stream dataStream = await tRequest.GetRequestStreamAsync();

                using (Stream dataStream = await tRequest.GetRequestStreamAsync())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = await tRequest.GetResponseAsync())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

       //SystembrodCasting() method for send broadcasting msg to user @kiran 21/11/2018
        public static void SystembrodCasting(TblAlertInstanceTO alertInstanceTO)
        {
            //try
            //{ 
            //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName("CHAT_APP_CONFIGURATION_SETTINGS");
            //    if(tblConfigParamsTO != null )
            //    {
            //        realTimeDatabaseTO realTimeDatabase = JsonConvert.DeserializeObject<realTimeDatabaseTO>(tblConfigParamsTO.ConfigParamVal.ToString());
            //        if (alertInstanceTO.BroadCastinguserList != null && alertInstanceTO.BroadCastinguserList.Count > 0)
            //        {
            //            foreach (var item in alertInstanceTO.BroadCastinguserList)
            //            {
            //                String stringUrl = realTimeDatabase.DatabaseURL + "/" + item + "/.json";
            //                var tRequest = WebRequest.Create(stringUrl) as HttpWebRequest;
            //                //var tRequest = WebRequest.Create("https://chapapp-3c4db.firebaseio.com/1/.json") as HttpWebRequest;
            //                tRequest.Method = "post";
            //                tRequest.ContentType = "application/json";
            //                DateTime currentDate = StaticStuff.Constants.ServerDateTime;
            //                long timeStamp = (int)(currentDate - new DateTime(1970, 1, 1)).TotalSeconds;

            //                var data = new
            //                {
            //                    msg = alertInstanceTO.AlertComment,
            //                    currentTimeStamp = currentDate,
            //                    isRead = 1
            //                };

            //                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            //                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            //                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
            //                {
            //                    dataStream.Write(byteArray, 0, byteArray.Length);
            //                }
            //                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;

            //                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //            }
            //        }
             
            //    }
            //   // return null;
            //}
            //catch (Exception ex)
            //{

            //    string str = ex.Message;
            //    //return str;
            //}
        }

    }
}
