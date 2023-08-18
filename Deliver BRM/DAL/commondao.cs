using FlexCel.Core;
using FlexCel.Pdf;
using FlexCel.Render;
using FlexCel.Report;
using FlexCel.XlsAdapter;
using Microsoft.Extensions.Logging;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class CommonDAO
    {
      
        public static System.DateTime SelectServerDateTime()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                /*To get Server Date Time for Local DB*/
                String sqlQuery = SalesTrackerAPI.Startup.SERVER_DATETIME_QUERY_STRING;

                //*To get Server Date Time for Azure Server DB*/

                //String sqlQuery = " declare @Dfecha as datetime " +
                //                 " DECLARE @D AS datetimeoffset " +
                //                 " set @Dfecha= SYSDATETIME()   " +
                //                 " SET @D = CONVERT(datetimeoffset, @Dfecha) AT TIME ZONE 'India Standard Time'" +
                //                 " select CONVERT(datetime, @D)";


                cmdSelect = new SqlCommand(sqlQuery, conn);

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    if (TimeZoneInfo.Local.Id != "India Standard Time")
                        return Convert.ToDateTime(dateReader[0]).ToLocalTime();
                    else return Convert.ToDateTime(dateReader[0]);
                }

                return new DateTime();
            }
            catch (Exception ex)
            {
                return new DateTime();
            }
            finally
            {
                conn.Close();
                //cmdSelect.Dispose();
            }

        }

      //user Tracking checked login entry if logout time is not null then return true
        public static int CheckLogOutEntry(int loginId)
{
    int count=0; 
      String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
    try{

            String sqlQuery = "select count(*) from tbllogin where idLogin= "+loginId+"  and logoutDate IS NOT NULL";
            SqlCommand cmd=new SqlCommand(sqlQuery,conn);
             conn.Open();
         count= Convert.ToInt32(cmd.ExecuteScalar());
         return count;

    }
    catch(Exception ex)
            {
                throw ex;
                 return count;
            }
            finally
            {
                conn.Close();
                
            }
            return count;


}  

#region  isDeactivate User
 public static int IsUserDeactivate(int userId)
{
    int active=1;
      String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
    try{

            String sqlQuery = "select isActive from tblUser where idUser= "+userId;
            SqlCommand cmd=new SqlCommand(sqlQuery,conn);
             conn.Open();
         active= Convert.ToInt32(cmd.ExecuteScalar());
         return active;

    }
    catch(Exception ex)
            {
                throw ex;
                 return active;
            }
            finally
            {
                conn.Close();
                
            }
            return active;


}  
#endregion
         #region get Login id List which register on apk and then uninstall  user Tracking
public static string SelectApKLoginArray(int userId)
        {
            string LoginIds="";
            
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idLogin from tbllogin where logoutdate is  null and deviceId is not Null and userId="+userId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader ApkLoginReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while(ApkLoginReader.Read())
                {
                    if(!string.IsNullOrEmpty(LoginIds))
                    {
                       LoginIds+=",";

                    }
                    LoginIds+=ApkLoginReader["idLogin"];
                }
                return LoginIds;
            }
            catch (Exception ex)
            {
                return LoginIds;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion
  
        public static void SetDateStandards(Object classTO, Double timeOffsetMins)
        {

            Type type = classTO.GetType();
            PropertyInfo[] propertyInfoArray = type.GetProperties();

            for (int j = 0; j < propertyInfoArray.Length; j++)
            {
                PropertyInfo propInfo = propertyInfoArray[j];
                if (propInfo.PropertyType == typeof(DateTime))
                {
                    DateTime columnValue = Convert.ToDateTime(propInfo.GetValue(classTO, null));
                    columnValue = columnValue.AddMinutes(timeOffsetMins);
                    propInfo.SetValue(classTO, columnValue);
                }
            }

        }
        public static dynamic PostSalesInvoiceToSAP(TblInvoiceTO tblInvoiceTO)
        {
            var tRequest = WebRequest.Create(Startup.StockUrl + "Commercial/PostSalesInvoiceToSAP") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = tblInvoiceTO,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(tblInvoiceTO);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static String SelectReportFullName(String reportName)
        {
            String reportFullName = null;

            //MstReportTemplateTO mstReportTemplateTO = MstReportTemplateDAO.SelectMstReportTemplateTO(reportName);
            DimReportTemplateTO dimReportTemplateTO = SelectDimReportTemplate(reportName);
            if (dimReportTemplateTO != null)
            {

                TblConfigParamsTO templatePath = BL.TblConfigParamsBL.SelectTblConfigParamsValByName("REPORT_TEMPLATE_FOLDER_PATH");
                //object templatePath = BL.MstCsParamBL.GetValue("TEMP_REPORT_PATH");//For Testing Pramod InputRemovalExciseReport

                if (templatePath != null)
                    return templatePath.ConfigParamVal + dimReportTemplateTO.ReportFileName + "." + dimReportTemplateTO.ReportFileExtension;
            }
            return reportFullName;
        }

        public static DimReportTemplateTO SelectDimReportTemplate(String reportName)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate WHERE reportName = '" + reportName + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@report_name", System.Data.SqlDbType.NVarChar).Value = mstReportTemplateTO.ReportName;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
               return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DimReportTemplateTO> ConvertDTToList(SqlDataReader dimReportTemplateTODT)
        {
            List<DimReportTemplateTO> dimReportTemplateTOList = new List<DimReportTemplateTO>();
            if (dimReportTemplateTODT != null)
            {
                while (dimReportTemplateTODT.Read())
                {
                    DimReportTemplateTO dimReportTemplateTONew = new DimReportTemplateTO();
                    if (dimReportTemplateTODT["idReport"] != DBNull.Value)
                        dimReportTemplateTONew.IdReport = Convert.ToInt32(dimReportTemplateTODT["idReport"].ToString());
                    if (dimReportTemplateTODT["isDisplayMultisheetAllow"] != DBNull.Value)
                        dimReportTemplateTONew.IsDisplayMultisheetAllow = Convert.ToInt32(dimReportTemplateTODT["isDisplayMultisheetAllow"].ToString());
                    if (dimReportTemplateTODT["createdBy"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedBy = Convert.ToInt32(dimReportTemplateTODT["createdBy"].ToString());
                    if (dimReportTemplateTODT["createdOn"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedOn = Convert.ToDateTime(dimReportTemplateTODT["createdOn"].ToString());
                    if (dimReportTemplateTODT["reportName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportName = Convert.ToString(dimReportTemplateTODT["reportName"].ToString());
                    if (dimReportTemplateTODT["reportFileName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileName = Convert.ToString(dimReportTemplateTODT["reportFileName"].ToString());
                    if (dimReportTemplateTODT["reportFileExtension"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileExtension = Convert.ToString(dimReportTemplateTODT["reportFileExtension"].ToString());
                    if (dimReportTemplateTODT["reportPassword"] != DBNull.Value)
                        dimReportTemplateTONew.ReportPassword = Convert.ToString(dimReportTemplateTODT["reportPassword"].ToString());
                    dimReportTemplateTOList.Add(dimReportTemplateTONew);
                }
            }
            return dimReportTemplateTOList;
        }


        public   static ResultMessage GenrateMktgInvoiceReport(DataSet invoiceDS, String templatePath, String localFilePath, Constants.ReportE reportE, Boolean IsProduction)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
               String filePath = String.Empty;
                filePath = localFilePath;
               
                resultMessage = Run(invoiceDS, templatePath, filePath, IsProduction);
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

        public static void OpenExcelFileReport(String fileName)
        {
            System.Diagnostics.Process.Start(fileName);
        }

        public static void OpenPDFFileReport(String fileName)
        {
            System.Diagnostics.Process.Start(fileName.Replace(".xls", ".pdf"));
        }

        public static ResultMessage Run(DataSet dataSet, String templateFileName, String fileName, Boolean IsProduction)
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
                        ordersReport.GetImageData += new GetImageDataEventHandler(OrdersReport_GetImageData);
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
                        Boolean isMultisheetReportAllow = isVisibleAllowMultisheetReport(reportFileName);
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

        private static FlexCelReport CreateReport(DataSet dataSet)
        {
            FlexCelReport Result = new FlexCelReport(true);
            Result.AddTable(dataSet);
            return Result;
        }

        public static  Boolean isVisibleAllowMultisheetReport(String reportFileName)
        {

            List<DimReportTemplateTO> dimReportTemplateTOList = isVisibleAllowMultisheetReportList(reportFileName);
            if (dimReportTemplateTOList != null && dimReportTemplateTOList.Count == 1)
            {
                return true;
            }
            else
                return false;
        }

        public static List<DimReportTemplateTO> isVisibleAllowMultisheetReportList(string reportFileName)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate " +
                                    " WHERE reportFileName = '" + reportFileName + "' AND isDisplayMultisheetAllow =1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
                if (rdr != null) rdr.Dispose();
            }
        }



        private static void OrdersReport_GetImageData(object sender, GetImageDataEventArgs e)
        {
            if (String.Compare(e.ImageName, "<#PhotoCode>", true) == 0)
            {
                byte[] RealImageData = ImageUtils.StripOLEHeader(e.ImageData); //On access databases, images are stored with an OLE 
                //header that we have to strip to get the real image.
                //This is done automatically by flexcel in most cases,
                //but here we have the original image format.
                using (MemoryStream MemStream = new MemoryStream(RealImageData)) //Keep stream open until bitmap has been used
                {
                    using (Bitmap bmp = new Bitmap(MemStream))
                    {
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        using (MemoryStream OutStream = new MemoryStream())
                        {
                            bmp.Save(OutStream, System.Drawing.Imaging.ImageFormat.Png);
                            e.Width = bmp.Width;
                            e.Height = bmp.Height;
                            e.ImageData = OutStream.ToArray();
                        }
                    }
                }
            }
            // throw new NotImplementedException();
        }


    }
}
