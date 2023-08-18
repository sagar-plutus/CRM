using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Data.SqlTypes;
using System.Xml.Schema;
using System.Collections;
using FlexCel.Core;
using FlexCel.XlsAdapter;
using FlexCel.Report;
using System.Diagnostics;
using System.Reflection;
//using System.Data.SqlServerCe;
using System.Configuration;
using FlexCel.Pdf;
using FlexCel.Render;


using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using System.Net;
using Microsoft.AspNetCore.Http;
//using IdentityModel.Client;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL
{
    class RunVegaFlexCelReport : IRunVegaFlexCelReport
    {
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        public RunVegaFlexCelReport(IDimReportTemplateBL iDimReportTemplateBL)
        {
            _iDimReportTemplateBL = iDimReportTemplateBL;
        }
        //Added by Archana[2011-10-18]
        String reportPassword = string.Empty;

        public Boolean Run(DataSet dataSet, String templateFileName)
        {
            try
            {
                String fileNm=string.Empty;
                //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                using (FlexCelReport ordersReport = CreateReport(dataSet))
                {
                    //SaveFileDialog saveXlsFile = new SaveFileDialog();

                    ordersReport.SetValue("Date", DateTime.Now);
                    //saveFileDialog1.InitialDirectory = @"D:\VegaFlexcel\Reports\Excel Template";
                    //if (saveXlsFile.ShowDialog() == DialogResult.OK)
                    //{
                        //ordersReport.Run("D:\\VegaFlexcel\\ExcelFiles\\employeedetails.template.xls", saveFileDialog1.FileName);
                        ordersReport.Run(templateFileName, fileNm);

                        //if (MessageBox.Show("Do you want to open the generated file?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                            Process.Start(fileNm);
                        //}
                    //}
                }
                return true;
            }
            catch (Exception ex)
            {
                //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
                return false;
            }
        }
        /// <summary>
        ///  Vijaymala[15 - 10 - 2018] added to generate report from template
        /// </summary>
        public ResultMessage Run(DataSet dataSet, String templateFileName, String fileName,Boolean IsProduction)
        {
            ResultMessage resultMessage = new ResultMessage();


            try
            {
                ////create memory stream

                string pdfUrl = templateFileName;
                // Member variable to store the MemoryStream Data
                MemoryStream pdfMemoryStream = null;

                if (IsProduction)
                {
                    if (pdfMemoryStream == null)
                    {
                        WebClient client = new WebClient();
                        try
                        {
                            pdfMemoryStream = new MemoryStream(client.DownloadData(pdfUrl));
                        }
                        finally
                        {
                            client.Dispose();
                        }
                    }
                }
                else
                {
                   
                    DirectoryInfo dirInfo = Directory.GetParent(fileName);
                    if (!Directory.Exists(dirInfo.FullName))
                    {
                        Directory.CreateDirectory(dirInfo.FullName);
                    }
                    pdfMemoryStream = new MemoryStream(File.ReadAllBytes(pdfUrl));
                }
                if (pdfMemoryStream != null && pdfMemoryStream.Length > 0)
                {
                    //DownloadFile();
                    using (FlexCelReport ordersReport = CreateReport(dataSet))
                {
                   
                    try
                    {
                        using (FileStream fs = File.OpenWrite(fileName))
                        {
                            if (fs == null)
                            {
                                //  return;
                            }
                           ordersReport.Run(pdfMemoryStream, fs);


                        }

                    }
                    catch (Exception ex)
                    {
                        resultMessage.DefaultExceptionBehaviour(ex, "");
                        return resultMessage;
                    }

                         //ordersReport.Run(templateFileName, fileName);


                        // TO GENERATE PDF FILE

                        //Amol[2011-09-16] For Mulisheet report
                    String reportFileName = System.IO.Path.GetFileNameWithoutExtension(templateFileName);
                    reportFileName = reportFileName.Replace(".template", "");
                    Boolean isMultisheetReportAllow = _iDimReportTemplateBL.isVisibleAllowMultisheetReport(reportFileName);
                    #region PDF GENERATE

                    String pdfFileName = fileName.Replace("xls", "pdf");

                    FlexCel.Render.FlexCelPdfExport flexCelPdfExport1 = new FlexCelPdfExport();
                    flexCelPdfExport1.Workbook = new XlsFile();

                    flexCelPdfExport1.Workbook.Open(fileName);

                    using (FileStream Pdf = new FileStream(pdfFileName, FileMode.Create))
                    {

                        int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
                        try
                        {
                            flexCelPdfExport1.BeginExport(Pdf);
                            flexCelPdfExport1.Workbook.PrintPaperSize = TPaperSize.Legal;

                            flexCelPdfExport1.Compress = false;

                                flexCelPdfExport1.Workbook.PrintToFit = false;
                               // flexCelPdfExport1.UseExcelProperties = true;
                                flexCelPdfExport1.PageLayout = TPageLayout.FullScreen; //To how the bookmarks when opening the file.
                                                                                   //flexCelPdfExport1.PageLayout = TPageLayout.None;
                                flexCelPdfExport1.Compress = false; //To how the bookmarks when opening the file.
                                //flexCelPdfExport1.PageSize = null;
                                //flexCelPdfExport1.PrintRangeBottom = 0;
                                //flexCelPdfExport1.PrintRangeTop = 0;
                                //flexCelPdfExport1.PrintRangeLeft = 0;
                                //flexCelPdfExport1.PrintRangeRight = 0;
                           

                                    if (isMultisheetReportAllow)
                                flexCelPdfExport1.ExportAllVisibleSheets(true, null);
                            else
                                flexCelPdfExport1.ExportSheet();

                            flexCelPdfExport1.EndExport();
                        }
                        finally
                        {
                            flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
                        }
                    }

                    #endregion

                }
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
               }
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Failed to Create report dataset.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }


        //public ResultMessage Run(DataSet dataSet, String templateFileName, String fileName)
        //{
        //    ResultMessage resultMessage = new ResultMessage();

        //    try
        //    {
        //        using (FlexCelReport ordersReport = CreateReport(dataSet))
        //        {
        //            //SaveFileDialog saveXlsFile = new SaveFileDialog();

        //            ordersReport.SetValue("Date", DateTime.Now);
        //            //saveFileDialog1.InitialDirectory = @"D:\VegaFlexcel\Reports\Excel Template";
        //            //ordersReport.Run("D:\\VegaFlexcel\\ExcelFiles\\employeedetails.template.xls", saveFileDialog1.FileName);
                    
        //                //File.Create(fileName);
        //                //DirectoryInfo dirInfo = Directory.GetParent(fileName);
        //                //if (!Directory.Exists(dirInfo.FullName))
        //                //{
        //                //    Directory.CreateDirectory(dirInfo.FullName);
        //                //}

        //            //HERE TO CHECK FILE IS OPEN OR NOT
        //            try
        //            {
        //                using (FileStream fs = File.OpenWrite(fileName))
        //                {
        //                    if (fs == null)
        //                    {
        //                        //  return;
        //                    }
        //                }

        //                //Assembly a = Assembly.GetExecutingAssembly();
        //                //using (Stream InStream = this.getfile(templateFileName))
        //                //{
        //                //    fileName = string.Empty;
        //                //    using (FileStream OutStream = new FileStream(fileName, FileMode.Create))
        //                //    {
        //                //        ordersReport.Run(InStream, OutStream);
        //                //    }
        //                //}

                    
        //            }
        //            catch (Exception ex)
        //            {
        //                //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
        //                //vMessage.MessageType = EMessageType.Error;
        //                //vMessage.Text = "Existing report already open Please close it.";
        //                //vMessage.Exception = ex;
        //                //VegaERPFrameWork.VErrorList.Add("Exception In Method Run At Class VDLL.VegaFlexCel.RunVegaFlexCelReport", EMessageType.Error, ex, null);
        //                resultMessage.DefaultExceptionBehaviour(ex, "");
        //                return resultMessage;
        //            }
        //            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);

        //            // Create the blob client.
        //            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //            // Retrieve reference to a target container.
        //            CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);

        //            //For Unique Id.
                    
        //            //String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;

        //           // CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

                   

        //            //// full path to file in temp location
        //            var filePath = templateFileName;
                                                    
        //            Stream instream=this.getfile(templateFileName);


        //            //WebClient webClient = new WebClient();

        //            //String saveLocation = "D:\\ Projects \\Backup\\file1.xls";
        //            //webClient.DownloadFile(templateFileName, fileName);
        //            ordersReport.Run(fileName, fileName);//--main code

        //            Stream outstream=this.getfile(fileName);
        //            byte[] buffer = new byte[32768];
        //            int read;
        //            while ((read = instream.Read(buffer, 0, buffer.Length)) > 0)
        //            {
        //                outstream.Write(buffer, 0, read);
        //            }
        //            ordersReport.Run(instream, outstream);


        //                // TO GENERATE PDF FILE
                        
        //            //Amol[2011-09-16] For Mulisheet report
        //                String reportFileName = System.IO.Path.GetFileNameWithoutExtension(templateFileName);
        //                reportFileName = reportFileName.Replace(".template", "");
        //                Boolean isMultisheetReportAllow =BL._iDimReportTemplateBL.isVisibleAllowMultisheetReport(reportFileName);
        //                #region PDF GENERATE

        //                String pdfFileName = fileName.Replace("xls", "pdf");

        //                FlexCel.Render.FlexCelPdfExport flexCelPdfExport1 = new FlexCelPdfExport();
        //                flexCelPdfExport1.Workbook = new XlsFile();
                    
        //                flexCelPdfExport1.Workbook.Open(fileName);

        //                using (FileStream Pdf = new FileStream(pdfFileName, FileMode.Create))
        //                {
                            
        //                    int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
        //                    try
        //                    {
        //                        flexCelPdfExport1.BeginExport(Pdf);
        //                        flexCelPdfExport1.Workbook.PrintPaperSize = TPaperSize.A4;
                                
        //                        flexCelPdfExport1.Compress = false;

        //                        flexCelPdfExport1.Workbook.PrintToFit = false;
        //                        flexCelPdfExport1.PageLayout = TPageLayout.None; //To how the bookmarks when opening the file.
        //                        //flexCelPdfExport1.PageLayout = TPageLayout.None;
        //                        if (isMultisheetReportAllow)
        //                            flexCelPdfExport1.ExportAllVisibleSheets(true, null);
        //                        else
        //                            flexCelPdfExport1.ExportSheet();

        //                        flexCelPdfExport1.EndExport();
        //                    }
        //                    finally
        //                    {
        //                        flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
        //                    }
        //                }

        //            #endregion

        //            // PDF COMPLETE

        //            resultMessage.DefaultSuccessBehaviour();
        //            return resultMessage;
        //        }
        //        resultMessage.MessageType = ResultMessageE.Error;
        //        resultMessage.Text = "Failed to Create report dataset.";
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        //String error = ex.Message.ToString();
        //        //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
        //        //vMessage.MessageType = EMessageType.Error;
        //        //vMessage.Text = ex.ToString();
        //        //vMessage.Exception = ex;
        //        //VegaERPFrameWork.VErrorList.Add("Exception In Method Run At Class VDLL.VegaFlexCel.RunVegaFlexCelReport", EMessageType.Error, ex, null);
        //        return resultMessage;                
        //    }
        //}

        /// <summary>
        /// KISHOR [2014-11-28] add with conn tran
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="templateFileName"></param>
        /// <param name="fileName"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            //vMessage.MessageType = EMessageType.None;

            try
            {
                string pdfUrl = templateFileName;


                // Member variable to store the MemoryStream Data
                MemoryStream pdfMemoryStream = null;
                MemoryStream outmemoryStream = null;
                if (pdfMemoryStream == null)
                {
                    WebClient client = new WebClient();
                    try
                    {
                        pdfMemoryStream = new MemoryStream(client.DownloadData(pdfUrl));
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
                if (pdfMemoryStream != null)
                {

                    using (FlexCelReport ordersReport = CreateReport(dataSet))
                    {
                        //SaveFileDialog saveXlsFile = new SaveFileDialog();

                        //ordersReport.SetValue("Date", DateTime.Now);
                        ////saveFileDialog1.InitialDirectory = @"D:\VegaFlexcel\Reports\Excel Template";
                        ////ordersReport.Run("D:\\VegaFlexcel\\ExcelFiles\\employeedetails.template.xls", saveFileDialog1.FileName);

                        ////File.Create(fileName);
                        //DirectoryInfo dirInfo = Directory.GetParent(fileName);
                        //if (!Directory.Exists(dirInfo.FullName))
                        //{
                        //    Directory.CreateDirectory(dirInfo.FullName);
                        //}

                        //HERE TO CHECK FILE IS OPEN OR NOT
                        try
                        {
                            using (FileStream fs = File.OpenWrite(fileName))
                            {
                                if (fs == null)
                                {
                                    //  return;
                                }
                                ordersReport.Run(pdfMemoryStream, fs);
                            }
                        }
                        catch (Exception ex)
                        {
                            //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
                            //vMessage.MessageType = EMessageType.Error;
                            //vMessage.Text = "Existing report already open Please close it.";
                            //vMessage.Exception = ex;
                            //VegaERPFrameWork.VErrorList.Add("Exception In Method Run At Class VDLL.VegaFlexCel.RunVegaFlexCelReport", EMessageType.Error, ex, null);
                            return resultMessage;
                        }


                       // ordersReport.Run(templateFileName, fileName);


                        // TO GENERATE PDF FILE

                        //Amol[2011-09-16] For Mulisheet report
                        String reportFileName = System.IO.Path.GetFileNameWithoutExtension(templateFileName);
                        reportFileName = reportFileName.Replace(".template", "");
                        Boolean isMultisheetReportAllow = _iDimReportTemplateBL.isVisibleAllowMultisheetReport(reportFileName, conn, tran);
                        #region PDF GENERATE

                        String pdfFileName = fileName.Replace("xls", "pdf");

                        FlexCel.Render.FlexCelPdfExport flexCelPdfExport1 = new FlexCelPdfExport();
                        flexCelPdfExport1.Workbook = new XlsFile();

                        flexCelPdfExport1.Workbook.Open(fileName);

                        using (FileStream Pdf = new FileStream(pdfFileName, FileMode.Create))
                        {

                            int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
                            try
                            {
                                flexCelPdfExport1.BeginExport(Pdf);
                                flexCelPdfExport1.Workbook.PrintPaperSize = TPaperSize.A4;

                                flexCelPdfExport1.Compress = false;

                                flexCelPdfExport1.Workbook.PrintToFit = false;
                                flexCelPdfExport1.PageLayout = TPageLayout.None; //To how the bookmarks when opening the file.
                                                                                 //flexCelPdfExport1.PageLayout = TPageLayout.None;
                                if (isMultisheetReportAllow)
                                    flexCelPdfExport1.ExportAllVisibleSheets(true, null);
                                else
                                    flexCelPdfExport1.ExportSheet();

                                flexCelPdfExport1.EndExport();
                            }
                            finally
                            {
                                flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
                            }
                        }

                        #endregion

                        // PDF COMPLETE
                    }
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Report Generated Successfully.";
                    return resultMessage;
                }
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Failed to Create report dataset.";
                return resultMessage;
            }
            catch (Exception ex)
            {
                String error = ex.Message.ToString();
                //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
                //vMessage.MessageType = EMessageType.Error;
                //vMessage.Text = ex.ToString();
                //vMessage.Exception = ex;
                //VegaERPFrameWork.VErrorList.Add("Exception In Method Run(DataSet dataSet, String templateFileName, String fileName, SqlConnection conn, SqlTransaction tran) At Class VDLL.VegaFlexCel.RunVegaFlexCelReport", EMessageType.Error, ex, null);
                return resultMessage;
            }
        }

        private FlexCelReport CreateReport(DataSet dataSet)
        {
            FlexCelReport Result = new FlexCelReport(true);
            Result.AddTable(dataSet);
            return Result;
        }

        //Added by Archana[2011-10-18]
        #region ReportsWithPassword

        public ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, String password)
        {
            ResultMessage resultMessage = new ResultMessage();
           // vMessage.MessageType = EMessageType.None;
            reportPassword = password;

            try
            {
                using (FlexCelReport ordersReport = CreateReport(dataSet))
                {
                   // SaveFileDialog saveXlsFile = new SaveFileDialog();

                    ordersReport.BeforeReadTemplate += new GenerateEventHandler(ordersReport_BeforeReadTemplate);
                    // ordersReport.AfterGenerateSheet += new GenerateEventHandler(ordersReport_AfterGenerateSheet);
                    ordersReport.AfterGenerateWorkbook += new GenerateEventHandler(ordersReport_AfterGenerateWorkbook);
                    ordersReport.SetValue("Date", DateTime.Now);

                    //File.Create(fileName);
                    DirectoryInfo dirInfo = Directory.GetParent(fileName);
                    if (!Directory.Exists(dirInfo.FullName))
                    {
                        Directory.CreateDirectory(dirInfo.FullName);
                    }

                    //HERE TO CHECK FILE IS OPEN OR NOT                
                    try
                    {
                        using (FileStream fs = File.OpenWrite(fileName))
                        {
                            if (fs == null)
                            {
                                //  return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //CommonUtil.Error.LogError.Log(ex, "VegaFlexCel.RunVegaFlexCelReport", "public Boolean Run(DataSet dataSet, String templateFileName)");
                        //vMessage.MessageType = EMessageType.Error;
                        resultMessage.DefaultExceptionBehaviour(ex, "Run");
                        return resultMessage;
                    }

                    HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(@"https://apkupdates.blob.core.windows.net/kalikadocuments/InvoiceVoucher.template.xls");

                    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

                    Stream responseStream = httpResponse.GetResponseStream();
                    StreamReader sr = new StreamReader(responseStream);

                    ordersReport.Run(responseStream, responseStream);
                    // TO GENERATE PDF FILE

                    #region PDF GENERATE

                    String pdfFileName = fileName.Replace("xls", "pdf");

                    FlexCel.Render.FlexCelPdfExport flexCelPdfExport1 = new FlexCelPdfExport();
                    flexCelPdfExport1.Workbook = new XlsFile();
                    flexCelPdfExport1.Workbook.Protection.OnPassword += new OnPasswordEventHandler(ordersReport_onpasswordEventHandler);

                    TWorkbookProtectionOptions twork = new TWorkbookProtectionOptions();
                    //flexCelPdfExport1.Workbook.Protection.OpenPassword = "Vega";
                    //flexCelPdfExport1.Workbook.Open(fileName);

                    //OnPasswordEventArgs e = new OnPasswordEventArgs(flexCelPdfExport1.Workbook);
                    //e.Password = "Vega";
                    flexCelPdfExport1.Workbook.Open(fileName);

                    using (FileStream Pdf = new FileStream(pdfFileName, FileMode.Create))
                    {
                        //flexCelPdfExport1.Workbook.Open(Pdf);

                        flexCelPdfExport1.Workbook.Protection.SetWorkbookProtection(password, twork);
                        //flexCelPdfExport1.Workbook.Protection.SetModifyPassword("Vega", true, "Admin");
                        flexCelPdfExport1.Workbook.Protection.OpenPassword = password;

                        int SaveSheet = flexCelPdfExport1.Workbook.ActiveSheet;
                        try
                        {
                            flexCelPdfExport1.BeginExport(Pdf);
                            TProtection t;

                            t = flexCelPdfExport1.Workbook.Protection;
                            flexCelPdfExport1.BeforeNewPage += new PageEventHandler(flexCelPdfExport1_BeforeNewPage);
                            flexCelPdfExport1.FontSubset = TFontSubset.Subset;
                            flexCelPdfExport1.Workbook.PrintToFit = false;
                            flexCelPdfExport1.PageLayout = TPageLayout.Outlines; //To how the bookmarks when opening the file.                            
                            flexCelPdfExport1.ExportAllVisibleSheets(false, null);// ExportSheet();
                            flexCelPdfExport1.EndExport();
                        }
                        finally
                        {
                            flexCelPdfExport1.Workbook.ActiveSheet = SaveSheet;
                        }
                    }

                    #endregion

                    // PDF COMPLETE

                    resultMessage.DefaultSuccessBehaviour();
                  
                }
                resultMessage.DefaultBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;

            }
        }

        void flexCelPdfExport1_BeforeNewPage(object sender, PageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ordersReport_BeforeReadTemplate(object sender, FlexCel.Report.GenerateEventArgs e)
        {
            e.File.Protection.OnPassword += new OnPasswordEventHandler(ordersReport_onpasswordEventHandler);
        }

        //private void ordersReport_AfterGenerateSheet(object sender, FlexCel.Report.GenerateEventArgs e)
        //{
        //    e.File.Protection.SetSheetProtection("Vega", new TSheetProtectionOptions(true));
        //}

        private void ordersReport_AfterGenerateWorkbook(object sender, FlexCel.Report.GenerateEventArgs e)
        {
            //if (encryptionType.SelectedItem == encryptionType.Items[1])
            //    e.File.Protection.EncryptionType = TEncryptionType.Xor;
            //else 
            e.File.Protection.EncryptionType = TEncryptionType.Standard;
            e.File.Protection.OpenPassword = reportPassword;
            //e.File.Protection.SetModifyPassword("Vega",true,"Admin");
            e.File.Protection.SetWorkbookProtection(reportPassword, new TWorkbookProtectionOptions(true, true));
        }

        private void ordersReport_onpasswordEventHandler(OnPasswordEventArgs e)
        {
            e.Password = reportPassword;
        }

        //public Stream getfile(string filePath)
        //{
        //    try
        //    {
        //        // Create a request for the URL.   
        //        WebRequest request = WebRequest.Create(filePath);
        //        // If required by the server, set the credentials.  
        //        request.Credentials = CredentialCache.DefaultCredentials;
        //        // Get the response.  
        //        WebResponse response = request.GetResponse();
        //        // Display the status.  
        //        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        //        // Get the stream containing content returned by the server.  
        //        Stream dataStream = response.GetResponseStream();
        //        // Open the stream using a StreamReader for easy access.  
        //        StreamReader reader = new StreamReader(dataStream);
        //        // Read the content.  
        //        string responseFromServer = reader.ReadToEnd();
        //        // Display the content.  
        //        Console.WriteLine(responseFromServer);
        //        // Clean up the streams and the response.  

        //        reader.Close();
        //        response.Close();
        //        return dataStream;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
                
        //    }
           
        
        //}

        //public async void DownloadFile()
        //{
        //    Download();
        //    //BlobWrapper _blobWrapper = new BlobWrapper();
        //    string fNm = "InvoiceVoucher.template.xls";
        //    Task<byte[]> data = DownloadFileFromBlob(fNm);
        //    byte[] d = await data;
        //}

        //public async Task<byte[]> DownloadFileFromBlob(string FileName)
        //{

        //    // Create azure storage  account connection.
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

        //    // Create the blob client.
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve reference to a target container.
        //    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);
        //    // Get Blob Container


        //    // Get reference to blob (binary content)
        //    CloudBlockBlob blockBlob = container.GetBlockBlobReference(FileName);

        //    // Read content
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        //blockBlob.DownloadToStreamen(ms);
        //        return ms.ToArray();
        //    }
        //}

        ////public ActionResult Download()
        ////{


        ////    string uri = "https://apkupdates.blob.core.windows.net/kalikadocuments/InvoiceVoucher.template.xls";
        ////    using (WebClient client = new WebClient())
        ////    {
        ////        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        ////        client.DownloadFile(uri, @"c:\nvd-rss.xls");
        ////    }

        ////    HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(@"https://apkupdates.blob.core.windows.net/kalikadocuments/InvoiceVoucher.template.xls");
            
        ////    HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

        ////    Stream responseStream = httpResponse.GetResponseStream();
        ////    StreamReader sr = new StreamReader(responseStream);

        ////    var storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);
        ////    var blobClient = storageAccount.CreateCloudBlobClient();
        ////    var container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);
        ////   // container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

        ////    //lines modified
        ////    var blockBlob = container.GetBlockBlobReference("InvoiceVoucher.template.xls");
        ////    using (var fileStream = System.IO.File.OpenWrite(@"D:/Report/a.xls"))
        ////    {
        ////        var pause = Task.Delay(2000);
        ////        blockBlob.DownloadToStreamAsync(fileStream);
        ////    }
        ////    //lines modified

        ////    Console.ReadLine();




        ////    //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Constants.AzureConnectionStr);
        ////    //var blobClient = storageAccount.CreateCloudBlobClient();
        ////    //var container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);
        ////    //var blob = container.GetBlockBlobReference("InvoiceVoucher.template.xls");
        ////    //var sasToken = blob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
        ////    //{
        ////    //    Permissions = SharedAccessBlobPermissions.Read,
        ////    //    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(10),//assuming the blob can be downloaded in 10 miinutes
        ////    //}, new SharedAccessBlobHeaders()
        ////    //{
        ////    //    ContentDisposition = "attachment; filename=file-name"
        ////    //});
        ////    //var blobUrl = string.Format("{0}{1}", blob.Uri, sasToken);

        ////   return null;
        ////}

        //StreamReader reader = null;

       // WebClient client = null;

      



        //public void DownloadFile(string urlAddress)

        //{

        //    client = new WebClient();

        //    //DownlaodFile method directely downlaod file on you given specific path ,Here i've saved in E: Drive

        //    client.DownloadFile(urlAddress, @"c:\\DownloadPdf.xls");



        //}



        //public void DownloadData(string strFileUrlToDownload)

        //{

        //    byte[] myDataBuffer = client.DownloadData((new Uri(strFileUrlToDownload)));



        //    MemoryStream storeStream = new MemoryStream();



        //    storeStream.SetLength(myDataBuffer.Length);

        //    storeStream.Write(myDataBuffer, 0, (int)storeStream.Length);



        //    storeStream.Flush();



        //    //TO save into certain file must exist on Local

        //    SaveMemoryStream(storeStream, "C:\\TestFile.xls");



        //    //The below Getstring method to get data in raw format and manipulate it as per requirement

        //    string download = Encoding.ASCII.GetString(myDataBuffer);



        //    Console.WriteLine(download);

        //    Console.ReadLine();

        //}

        //public void SaveMemoryStream(MemoryStream ms, string FileName)

        //{

        //    FileStream outStream = File.OpenWrite(FileName);

        //    ms.WriteTo(outStream);

        //    outStream.Flush();

        //    outStream.Close();

        //}

    

    #endregion
}
}
