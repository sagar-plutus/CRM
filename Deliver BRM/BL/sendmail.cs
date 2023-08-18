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
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DAL;

namespace SalesTrackerAPI.BL
{
    public class sendmail
    {
        #region Sending the mail Date:2/10/2017 by vinod thorat

        public static int InsertTblSendMail(SendMail sendmailTO)
        {
          return SendMailDAO.SendEmail(sendmailTO);            
        }
       
        #endregion

    }
}
