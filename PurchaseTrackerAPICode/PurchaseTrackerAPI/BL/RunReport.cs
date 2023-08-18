using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PurchaseTrackerAPI.BL.Interfaces;
namespace PurchaseTrackerAPI.BL
{
   public class RunReport : IRunReport
    {
        private readonly IRunVegaFlexCelReport _iRunVegaFlexCelReport;
        public RunReport(IRunVegaFlexCelReport iRunVegaFlexCelReport)
        {
            _iRunVegaFlexCelReport = iRunVegaFlexCelReport;
        }
        /// <summary>
        /// Vijaymala[15-10-2018] added to generate report from template
        /// </summary>
        /// <param name="invoiceDS">Invoice DataSet</param>
        /// <returns></returns>
        public ResultMessage GenrateMktgInvoiceReport(DataSet invoiceDS, String templatePath, String localFilePath, Constants.ReportE reportE,Boolean IsProduction) 
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
               // RunVegaFlexCelReport runVegaReport = new RunVegaFlexCelReport();

                String filePath = String.Empty;
                filePath = localFilePath;
                //if (IsProduction)
                //{
                //    filePath = localFilePath;
                //}
                //else
                //{
                //    String objFilePath = String.Empty;
                //    String driveName = GetDriveNameNotOSDrive();
                //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName("TEMP_REPORT_PATH");
                //    if (tblConfigParamsTO != null)
                //    {
                //        objFilePath = tblConfigParamsTO.ConfigParamVal;
                //    }
                //    String path = objFilePath.ToString();
                //    if (!Directory.Exists(path))
                //    {
                //        Directory.CreateDirectory(path);
                //    }

                //    filePath = driveName + path;
                //    String fileName = Path.GetFileName(localFilePath);
                //    filePath = filePath + "\\" + fileName;
                //}

                resultMessage = _iRunVegaFlexCelReport.Run(invoiceDS, templatePath, filePath,IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    switch (reportE)
                    {
                        case Constants.ReportE.NONE:
                            resultMessage.Tag = filePath;
                            break;
                        case Constants.ReportE.EXCEL:
                            resultMessage.Tag = filePath;
                            OpenExcelFileReport(filePath);
                            break;
                        case Constants.ReportE.PDF:
                            resultMessage.Tag = filePath.Replace(".xls", ".pdf");
                            OpenPDFFileReport(filePath);
                            break;
                        case Constants.ReportE.BOTH:
                            resultMessage.Tag = filePath;
                            OpenPDFFileReport(filePath);
                            OpenExcelFileReport(filePath);
                            break;
                        case Constants.ReportE.PDF_DONT_OPEN:
                            resultMessage.Tag = filePath.Replace(".xls", ".pdf");
                            break;
                        case Constants.ReportE.EXCEL_DONT_OPEN:
                            resultMessage.Tag = filePath;
                            break;
                        default:
                            break;
                    }

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        /// <summary>
        ///Vijaymala[15-10-2018] added to generate report from template
        /// </summary>
        /// <param name="mktgInvoiceDS"></param>
        /// <param name="templatePath"></param>
        /// <param name="localFilePath"></param>
        /// <param name="reportE"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ResultMessage GenrateMktgInvoiceReport(DataSet mktgInvoiceDS, String templatePath, String localFilePath, Constants.ReportE reportE, SqlConnection conn, SqlTransaction tran)
       {
           //RunVegaFlexCelReport runVegaReport = new RunVegaFlexCelReport();

           //Amol[2011-10-10] Here To save All Report Files In Temp Folder 
           ResultMessage resultMessage = new ResultMessage();
            //String driveName = GetDriveNameNotOSDrive();
            // String objFilePath = string.Empty;
            // TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO("TEMP_REPORT_PATH", conn, tran);
            //   if(tblConfigParamsTO!=null)
            // {
            //     objFilePath = tblConfigParamsTO.ConfigParamVal;
            // }
            //String path = objFilePath.ToString();
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            // String filePath = driveName + path;
            //String fileName = Path.GetFileName(localFilePath);
            String filePath = localFilePath;
                //filePath + "\\" + fileName;
           
           resultMessage = _iRunVegaFlexCelReport.Run(mktgInvoiceDS, templatePath, filePath, conn, tran);
           if (resultMessage.MessageType == ResultMessageE.Information)
           {
               switch (reportE)
               {
                   case Constants.ReportE.NONE:
                       resultMessage.Tag = filePath;
                       break;
                   case Constants.ReportE.EXCEL:
                       resultMessage.Tag = filePath;
                       OpenExcelFileReport(filePath);
                       break;
                   case Constants.ReportE.PDF:
                       resultMessage.Tag = filePath.Replace(".xls", ".pdf");
                       OpenPDFFileReport(filePath);
                       break;
                   case Constants.ReportE.BOTH:
                       resultMessage.Tag = filePath;
                       OpenPDFFileReport(filePath);
                       OpenExcelFileReport(filePath);
                       break;
                   case Constants.ReportE.PDF_DONT_OPEN:
                       resultMessage.Tag = filePath.Replace(".xls", ".pdf");
                       break;
                   case Constants.ReportE.EXCEL_DONT_OPEN:
                       resultMessage.Tag = filePath;
                       break;
                   default:
                       break;
               }

           }
           return resultMessage;
       }

        public void OpenExcelFileReport(String fileName)
        {
            System.Diagnostics.Process.Start(fileName);
        }

        public void OpenPDFFileReport(String fileName)
        {
            System.Diagnostics.Process.Start(fileName.Replace(".xls", ".pdf"));
        }


        //Added by Archana[2011-10-18] to generate report with password
        public ResultMessage GenrateReportWithPassword(DataSet mktgInvoiceDS, String templatePath, String localFilePath, String password)
        {
            //RunVegaFlexCelReport runVegaReport = new RunVegaFlexCelReport();

            return _iRunVegaFlexCelReport.Run(mktgInvoiceDS, templatePath, localFilePath, password);
        }

        public String GetDriveNameNotOSDrive()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            String directory = "";
            String rootDrive = Path.GetPathRoot(Environment.SystemDirectory);

            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.DriveType == DriveType.Fixed && !rootDrive.Equals(drive.Name))
                {
                    directory = drive.Name;
                    break;
                }
                else
                    directory = drive.Name;
            }
            return directory;
        }

        //public object GetValue(string csParamName)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = " SELECT * FROM mst_cs_param WHERE cs_param_display_name = '" + csParamName + "' ";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@cs_param_id", System.Data.SqlDbType.Int, 4).Value = mstCsParamTO.CsParamId;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        if (dt != null && dt.Rows.Count == 1)
        //        {
        //            return dt.Rows[0]["cs_param_val"];
        //        }
        //        //Nitin[2011-02-22] if record not found then return null
        //        //return dt;
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //        //String userName = System.Windows.Forms.SystemInformation.UserName;
        //        //MessageBox.Show("Computer Name:" + computerName + Environment.NewLine + "" + VDLL.Masters.GlobalConnectionString.ActiveSchemaName + ".user Name:" + userName + Environment.NewLine + "Class Name: MstCsParamDAO" + Environment.NewLine + "Method Name:SelectMstCsParam(MstCsParamTO mstCsParamTO)" + Environment.NewLine + "Exception Message:" + ex.Message.ToString() + "");
        //       // VErrorHandling.LogError.DumpLog(VErrorHandling.LogLevelE.ERROR, 1, DateTime.Now, ex, "Error in getting cs param value for csParamName =" + csParamName + "");
        //       // VegaERPFrameWork.VErrorList.Add("Error in getting cs param value for csParamName =" + csParamName + "", VegaERPFrameWork.EMessageType.Error, ex, null);
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        

        //public object GetValue(string csParamName, SqlConnection conn, SqlTransaction myTrans)
        //{

        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {

        //        cmdSelect.CommandText = " SELECT * FROM mst_cs_param WHERE cs_param_display_name = '" + csParamName + "' ";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = myTrans;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@cs_param_id", System.Data.SqlDbType.Int, 4).Value = mstCsParamTO.CsParamId;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        if (dt != null && dt.Rows.Count == 1)
        //        {
        //            return dt.Rows[0]["cs_param_val"];
        //        }
        //        //Nitin[2011-02-22] if record not found then return null
        //        //return dt;
        //        return null;
        //    }

        //    finally
        //    {

        //        cmdSelect.Dispose();
        //    }
        //}


      

    }
}
