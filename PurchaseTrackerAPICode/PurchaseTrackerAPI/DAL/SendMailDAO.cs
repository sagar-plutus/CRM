using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using System.IO;
using Microsoft.AspNetCore;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    #region sending the mail through the gmail account by vinod Thorat Dated:2/10/2017

    public class SendMailDAO :ISendMailDAO
    {
        
        private readonly IConnectionString _iConnectionString;
        public SendMailDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public  int SendEmail(SendMail tblsendTO)
          {
             try
             {
                
                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(tblsendTO.FromTitle, tblsendTO.From));
                mimeMessage.To.Add(new MailboxAddress(tblsendTO.ToTitle, tblsendTO.To));
                mimeMessage.Subject = tblsendTO.Subject;
                var bodybuilder = new BodyBuilder();
                bodybuilder.HtmlBody = "<h4>Dear Client, </h4><p>We are contacting you in regard to a new invoice # 1 that has been created on your account. You may find the invoice attached.</p><h4>Kind Regards,</h4>";
                mimeMessage.Body = bodybuilder.ToMessageBody();
                byte[] bytes = System.Convert.FromBase64String(tblsendTO.Message.Replace("data:application/pdf;base64,", String.Empty));
                bodybuilder.Attachments.Add("TestPdf.pdf", bytes, ContentType.Parse("application/pdf"));
                mimeMessage.Body = bodybuilder.ToMessageBody();
                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", tblsendTO.Port, false);                   
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);                                     
                    client.Disconnect(true);
                    return 1;
                }
               
            }        
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }        
     
    }
    #endregion
}
