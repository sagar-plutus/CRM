using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using System.IO;
using Microsoft.AspNetCore;
using System.Net;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    #region sending the mail through the gmail account by vinod Thorat Dated:2/10/2017

    public class SendMailDAO
    {   
        public static int SendEmail(SendMail tblsendTO)
          {
             try
             {
                //From Address 
                string FromAddress = tblsendTO.From;
                string FromAdressTitle = tblsendTO.FromTitle;

                //To Address 
                string ToAddress = tblsendTO.To;
                string ToAdressTitle = tblsendTO.ToTitle;

                // Subject
                string Subject = tblsendTO.Subject;
                
                //Body content 
                string BodyContent = tblsendTO.BodyContent;

                //Smtp Server 
                string SmtpServer = "smtp.gmail.com";

                //Smtp Port Number 
                int SmtpPortNumber = tblsendTO.Port;

                var mimeMessage = new MimeMessage();
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
                mimeMessage.Subject = Subject;   
                //mimeMessage.Attachments=
                mimeMessage.Body = new TextPart("plain")
                {
                    Text = BodyContent
                };

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(SmtpServer, SmtpPortNumber, false);
                    // Note: only needed if the SMTP server requires authentication 
                    // Error 5.5.1 Authentication                     
                    client.Authenticate(tblsendTO.UserName, tblsendTO.Password);
                    client.Send(mimeMessage);                                      
                    Console.WriteLine("The mail has been sent successfully !!");
                    Console.ReadLine();
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
