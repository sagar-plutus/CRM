using MailKit.Net.Smtp;
using MimeKit;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class SendMailBL
    {
        public static ResultMessage SendEmail(SendMail tblsendTO, String fileName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                #region Set configration of mail  

                TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    resultMessage.DefaultBehaviour("DimEmailConfigrationTO Found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;

                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                //mimeMessage.Subject = "Regards New Visit Details ";
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                if (tblsendTO.BodyContent != null)
                {
                    bodybuilder.HtmlBody = tblsendTO.BodyContent;
                }
                else
                {
                    bodybuilder.HtmlBody = "<h4>Dear Sir, </h4><p>I am sharing  Visit information with  you in regard to a new Visit Details that has been captured during  visit.   You may find the pdf file attached.</p><h4>Kind Regards,";
                }
                mimeMessage.Body = bodybuilder.ToMessageBody();
                if (fileName != null)
                {
                    byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                    //byte[] compressBytes = Compress(bytes);
                    //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    //System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms,System.IO.Compression.CompressionMode.Compress);

                    ////Compress
                    //sw.Write(bytes, 0, bytes.Length);
                    ////Close, DO NOT FLUSH cause bytes will go missing...
                    //sw.Close();

                    bodybuilder.Attachments.Add(fileName, bytes, ContentType.Parse("application/pdf"));
                    mimeMessage.Body = bodybuilder.ToMessageBody();
                }
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);
                    client.Disconnect(true);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Email Sent Succesfully";
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;

                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmail");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;

            }
            finally
            {

            }
        }
        

         public static async Task<ResultMessage> SendEmailAsync(SendMail tblsendTO, String fileName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                #region Set configration of mail  

                TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
                if (DimEmailConfigrationTO != null)
                {

                    tblsendTO.From = DimEmailConfigrationTO.EmailId;
                    tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
                    tblsendTO.Password = DimEmailConfigrationTO.Password;
                    tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
                }
                else
                {
                    resultMessage.DefaultBehaviour("DimEmailConfigrationTO Found Null");
                    resultMessage.DisplayMessage = "Error While Sending Email";
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;


                }
                #endregion
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                //mimeMessage.Subject = "Regards New Visit Details ";
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                if (tblsendTO.BodyContent != null)
                {
                    bodybuilder.HtmlBody = tblsendTO.BodyContent;
                }
                else
                {
                    bodybuilder.HtmlBody = "<h4>Dear Sir, </h4><p>I am sharing  Visit information with  you in regard to a new Visit Details that has been captured during  visit.   You may find the pdf file attached.</p><h4>Kind Regards,";
                }
                mimeMessage.Body = bodybuilder.ToMessageBody();
                if (fileName != null)
                {
                    byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                    bodybuilder.Attachments.Add(fileName, bytes, ContentType.Parse("application/pdf"));
                    mimeMessage.Body = bodybuilder.ToMessageBody();
                }
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    await client.SendAsync(mimeMessage);
                    client.Disconnect(true);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Email Sent Succesfully";
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;

                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendEmail");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;

            }
            finally
            {

            }
        }





        //public static byte[] Compress(byte[] raw)
        //{
        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
        //        {
        //            gzip.Write(raw, 0, raw.Length);
        //        }
        //        return memory.ToArray();
        //    }
        //}
    }
    //  public static ResultMessage SendEmailPsw(SendMail tblsendTO)
    // {
    //     ResultMessage resultMessage = new ResultMessage();
    //     try
    //     {
    //         #region Set configration of mail  

    //         TblEmailConfigrationTO DimEmailConfigrationTO = BL.TblEmailConfigrationBL.SelectDimEmailConfigrationTO();
    //         if (DimEmailConfigrationTO != null)
    //         {

    //             tblsendTO.From = DimEmailConfigrationTO.EmailId;
    //             tblsendTO.UserName = DimEmailConfigrationTO.EmailId;
    //             tblsendTO.Password = DimEmailConfigrationTO.Password;
    //             tblsendTO.Port = DimEmailConfigrationTO.PortNumber;
    //         }
    //         else
    //         {
    //             resultMessage.DefaultBehaviour("DimEmailConfigrationTO Found Null");
    //             resultMessage.DisplayMessage = "Error While Sending Email";
    //             resultMessage.MessageType = ResultMessageE.Error;
    //             return resultMessage;

    //         }
    //         #endregion
    //         var mimeMessage = new MimeMessage();
    //         mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
    //         mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
    //         //mimeMessage.Subject = "Regards New Visit Details ";
    //         mimeMessage.Subject = tblsendTO.Subject;
    //         var bodybuilder = new BodyBuilder();
    //         bodybuilder.HtmlBody = tblsendTO.BodyContent;
    //         mimeMessage.Body = bodybuilder.ToMessageBody();
    //         //byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
    //         //bodybuilder.Attachments.Add(fileName, bytes, ContentType.Parse("application/pdf"));
    //         mimeMessage.Body = bodybuilder.ToMessageBody();
    //         using (SmtpClient client = new SmtpClient())
    //         {
    //             client.Connect("smtp.gmail.com", tblsendTO.Port, false);
    //             client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
    //             client.Send(mimeMessage);
    //             client.Disconnect(true);
    //             resultMessage.DefaultSuccessBehaviour();
    //             resultMessage.DisplayMessage = "Email Sent Succesfully";
    //             resultMessage.MessageType = ResultMessageE.Information;
    //             return resultMessage;

    //         }

    //     }
    //     catch (Exception ex)
    //     {
    //         resultMessage.DefaultExceptionBehaviour(ex, "SendEmail");
    //         resultMessage.DisplayMessage = "Error While Sending Email";
    //         resultMessage.MessageType = ResultMessageE.Error;
    //         return resultMessage;

    //     }
    //     finally
    //     {

    //     }
    // }

    }


