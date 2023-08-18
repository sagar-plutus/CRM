using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblProductInfoBL : ITblProductInfoBL
    {

        private readonly IConnectionString _iConnectionString;
        private readonly ITblProductInfoDAO _iTblProductInfoDAO;
        public TblProductInfoBL(ITblProductInfoDAO itblProductInfoDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblProductInfoDAO = itblProductInfoDAO;
        }
        #region Selection
       
        public  List<TblProductInfoTO> SelectAllTblProductInfoList()
        {
            return  _iTblProductInfoDAO.SelectAllTblProductInfo();
        }

        public  List<TblProductInfoTO> SelectAllTblProductInfoList(SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProductInfoDAO.SelectAllLatestProductInfo(conn, tran);
        }

        /// <summary>
        /// Saket [2018-01-18] Added.
        /// </summary>
        /// <returns></returns>
        public  List<TblProductInfoTO> SelectAllTblProductInfoListLatest()
        {
            return _iTblProductInfoDAO.SelectTblProductInfoLatest();
        }

        public  TblProductInfoTO SelectTblProductInfoTO(Int32 idProduct)
        {
           return  _iTblProductInfoDAO.SelectTblProductInfo(idProduct);
        }

        public  List<TblProductInfoTO> SelectAllEmptyProductInfoList(int prodCatId)
        {
            return _iTblProductInfoDAO.SelectEmptyProductDetailsTemplate(prodCatId);
        }

        /*GJ@20170818 : Get the Product Info By Loading Slip Ext Ids for Budles Calculation*/
        public  List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            return _iTblProductInfoDAO.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }


        #endregion

        #region Insertion
        public  int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            return _iTblProductInfoDAO.InsertTblProductInfo(tblProductInfoTO);
        }

        public  int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductInfoDAO.InsertTblProductInfo(tblProductInfoTO, conn, tran);
        }
        public  ResultMessage SaveProductInformation(List<TblProductInfoTO> productInfoTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (productInfoTOList != null && productInfoTOList.Count > 0)
                {

                    for (int i = 0; i < productInfoTOList.Count; i++)
                    {
                        result = InsertTblProductInfo(productInfoTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblProductInfo";
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "productInfoTOList Found Null";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : UpdateDailyStock";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Updation
        public  int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            return _iTblProductInfoDAO.UpdateTblProductInfo(tblProductInfoTO);
        }

        public  int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductInfoDAO.UpdateTblProductInfo(tblProductInfoTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblProductInfo(Int32 idProduct)
        {
            return _iTblProductInfoDAO.DeleteTblProductInfo(idProduct);
        }

        public  int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductInfoDAO.DeleteTblProductInfo(idProduct, conn, tran);
        }

      

        #endregion

    }
}
