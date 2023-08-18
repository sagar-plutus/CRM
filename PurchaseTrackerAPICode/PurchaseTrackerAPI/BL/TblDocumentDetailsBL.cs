using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblDocumentDetailsBL: ITblDocumentDetailsBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblDocumentDetailsDAO _iTblDocumentDetailsDAO;
        public TblDocumentDetailsBL(ITblDocumentDetailsDAO iTblDocumentDetailsDAO, ITblConfigParamsBL iTblConfigParamsBL, Icommondao icommondao, IConnectionString iConnectionString)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
            _iTblDocumentDetailsDAO = iTblDocumentDetailsDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
        }
        #region Selection
        public  List<TblDocumentDetailsTO> SelectAllTblDocumentDetails()
        {
            return _iTblDocumentDetailsDAO.SelectAllTblDocumentDetails();
        }

        public  List<TblDocumentDetailsTO> SelectAllTblDocumentDetailsList()
        {
            List<TblDocumentDetailsTO> tblDocumentDetailsTODT = _iTblDocumentDetailsDAO.SelectAllTblDocumentDetails();
            return tblDocumentDetailsTODT;
        }

        public  TblDocumentDetailsTO SelectTblDocumentDetailsTO(Int32 idDocument)
        {
            TblDocumentDetailsTO tblDocumentDetailsTODT = _iTblDocumentDetailsDAO.SelectTblDocumentDetails(idDocument);
            if (tblDocumentDetailsTODT != null)
                return tblDocumentDetailsTODT;
            else
                return null;
        }

        public  List<TblDocumentDetailsTO> SelectAllTblDocumentDetails(string documentIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.SelectAllTblDocumentDetails(documentIds, conn, tran);
        }
        public  List<TblDocumentDetailsTO> GetUploadedFileBasedOnFileType(Int32 fileTypeId, Int32 createdBy)
        {
            return _iTblDocumentDetailsDAO.SelectDocumentDetailsBasedOnFileType(fileTypeId, createdBy);
        }

        #endregion

        #region Insertion
        public  int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            return _iTblDocumentDetailsDAO.InsertTblDocumentDetails(tblDocumentDetailsTO);
        }

        public  int InsertTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.InsertTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO)
        {
            return _iTblDocumentDetailsDAO.UpdateTblDocumentDetails(tblDocumentDetailsTO);
        }

        public  int UpdateTblDocumentDetails(TblDocumentDetailsTO tblDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.UpdateTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblDocumentDetails(Int32 idDocument)
        {
            return _iTblDocumentDetailsDAO.DeleteTblDocumentDetails(idDocument);
        }

        public  int DeleteTblDocumentDetails(Int32 idDocument, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDocumentDetailsDAO.DeleteTblDocumentDetails(idDocument, conn, tran);
        }

        #endregion

        //public  void TestUploadFile()
        //{
        //    ResultMessage resultMessage = new ResultMessage();
        //    List<TblDocumentDetailsTO> list = new List<TblDocumentDetailsTO>();

        //    TblDocumentDetailsTO tblDocumentDetailsTO = new TblDocumentDetailsTO();
        //    tblDocumentDetailsTO.CreatedBy = 1;
        //    tblDocumentDetailsTO.ModuleId = 1;
        //    tblDocumentDetailsTO.Extension = "png";
        //    tblDocumentDetailsTO.IsActive = 1;
        //    tblDocumentDetailsTO.DocumentDesc = "ABC";
        //    tblDocumentDetailsTO.CreatedOn =  _iCommonDAO.ServerDateTime;
        //    var webClient = new WebClient();
        //    byte[] imageBytes = webClientData("http://www.google.com/images/logos/ps_logo2.png");
        //    tblDocumentDetailsTO.FileData = imageBytes;
        //    list.Add(tblDocumentDetailsTO);
        //    resultMessage=UploadDocument(list);
        //}


        #region Upload Image 
        //Sudhir[24-APR-2018] Added for Uploading Image 
        public  ResultMessage UploadDocument(List<TblDocumentDetailsTO> tblDocumentDetailsTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = UploadDocumentWithConnTran(tblDocumentDetailsTOList, conn, tran);
                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Success..Invoice decomposed";
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "mergeInvoices");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala added[22-05-2018]added:to upload  document with connection and transaction
        /// </summary>
        /// <param name="tblDocumentDetailsTOList"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>

        public  ResultMessage UploadDocumentWithConnTran(List<TblDocumentDetailsTO> tblDocumentDetailsTOList, SqlConnection conn, SqlTransaction tran)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            String ErrorMessage = "";
            Boolean error = false;
            try
            {
                if (tblDocumentDetailsTOList != null)
                {

                    foreach (TblDocumentDetailsTO tblDocumentDetailsTO in tblDocumentDetailsTOList)
                    {
                        if (tblDocumentDetailsTO.FileData != null)
                        {
                            Boolean isLive = false;
                            isLive = Startup.isLive;

                            string AzureConnectionStr = "";
                            TblConfigParamsTO configParamTOForAzureConnStr = _iTblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION);
                            if (configParamTOForAzureConnStr == null)
                            {
                                throw new Exception("configParamTOForAzureConnStr == null");
                            }

                            AzureConnectionStr = configParamTOForAzureConnStr.ConfigParamVal;
                            // Create azure storage  account connection.
                            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionStr);

                            // Create the blob client.
                            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                            CloudBlobContainer container = null;

                            if (isLive == true)
                            {
                                // Retrieve reference to a target container.
                                container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);
                            }
                            else
                            {
                                // Retrieve reference to a target container.
                                container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForTestingDocument);
                            }


                            // Retrieve reference to a target container.
                            //CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);

                            //For Unique Id.
                            String UUID = Guid.NewGuid().ToString();

                            String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;

                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);


                            var fileStream = tblDocumentDetailsTO.FileData;

                            Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);


                            tblDocumentDetailsTO.IsActive = 1;
                            tblDocumentDetailsTO.Path = blockBlob.SnapshotQualifiedUri.AbsoluteUri;
                            tblDocumentDetailsTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                            result = InsertTblDocumentDetails(tblDocumentDetailsTO, conn, tran);
                            if (result == 1)
                            {
                                resultMessage.Tag = tblDocumentDetailsTOList;
                            }
                            else
                            {
                                ErrorMessage += "File Uploading Failed For" + tblDocumentDetailsTO.DocumentDesc + "|";
                                error = true;
                            }

                        }
                    }
                    if (error)
                    {
                        resultMessage.DefaultBehaviour(ErrorMessage);
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.DisplayMessage = "File Uploaded Succesfully";
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 1;
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("List is Empty");
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    return resultMessage;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        //Post Multi Part Files
        public async Task UploadMultipleTypesFile(List<IFormFile> files, Int32 createdBy, Int32 FileTypeId)
        {
            int result = 0;
            try
            {
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {

                        string AzureConnectionStr = "";
                        TblConfigParamsTO configParamTOForAzureConnStr = _iTblConfigParamsBL.SelectTblConfigParamsValByName(StaticStuff.Constants.CP_AZURE_CONNECTIONSTRING_FOR_DOCUMENTATION);
                        if (configParamTOForAzureConnStr == null)
                        {
                            throw new Exception("configParamTOForAzureConnStr == null");
                        }

                        AzureConnectionStr = configParamTOForAzureConnStr.ConfigParamVal;
                        // Create azure storage  account connection.

                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AzureConnectionStr);

                        // Create the blob client.
                        CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                        // Retrieve reference to a target container.
                        CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerNameForDocument);

                        //For Unique Id.
                        String UUID = Guid.NewGuid().ToString();

                        //String fileName = tblDocumentDetailsTO.DocumentDesc + UUID + "." + tblDocumentDetailsTO.Extension;

                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

                        long size = file.Length;

                        //// full path to file in temp location
                        var filePath = Path.GetTempFileName();

                        if (file.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            using (var stream = new FileStream(filePath, FileMode.Open))
                            {
                                await blockBlob.UploadFromStreamAsync(stream);
                            }

                            //Insertion into tblDocument Details for Maintaining Record.
                            TblDocumentDetailsTO tblDocumentDetailsTO = new TblDocumentDetailsTO();
                            tblDocumentDetailsTO.IsActive = 1;
                            tblDocumentDetailsTO.ModuleId = 1;
                            tblDocumentDetailsTO.DocumentDesc = file.FileName;
                            tblDocumentDetailsTO.Path = blockBlob.SnapshotQualifiedUri.AbsoluteUri;
                            tblDocumentDetailsTO.CreatedOn =  _iCommonDAO.ServerDateTime;
                            tblDocumentDetailsTO.CreatedBy = createdBy;
                            tblDocumentDetailsTO.FileTypeId = FileTypeId;
                            result = InsertTblDocumentDetails(tblDocumentDetailsTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #endregion

    }
}
