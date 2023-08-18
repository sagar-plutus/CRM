using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.BL
{
    public class TblProductInfoBL
    {
        #region Selection
       
        public static List<TblProductInfoTO> SelectAllTblProductInfoList()
        {
            return  TblProductInfoDAO.SelectAllTblProductInfo();
        }

        public static List<TblProductInfoTO> SelectAllTblProductInfoList(SqlConnection conn,SqlTransaction tran)
        {
            return TblProductInfoDAO.SelectAllLatestProductInfo(conn, tran);
        }

        public static TblProductInfoTO SelectTblProductInfoTO(Int32 idProduct)
        {
           return  TblProductInfoDAO.SelectTblProductInfo(idProduct);
        }

        public static List<TblProductInfoTO> SelectAllEmptyProductInfoList(int prodCatId)
        {
            return TblProductInfoDAO.SelectEmptyProductDetailsTemplate(prodCatId);
        }

        /*GJ@20170818 : Get the Product Info By Loading Slip Ext Ids for Budles Calculation*/
        public static List<TblProductInfoTO> SelectProductInfoListByLoadingSlipExtIds(string strLoadingSlipExtIds)
        {
            return TblProductInfoDAO.SelectProductInfoListByLoadingSlipExtIds(strLoadingSlipExtIds);
        }
        public static List<TblProductInfoTO> SelectAllTblProductInfoListLatest()
        {
            return TblProductInfoDAO.SelectTblProductInfoLatest();
        }


        #endregion

        #region Insertion
        public static int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            return TblProductInfoDAO.InsertTblProductInfo(tblProductInfoTO);
        }

        public static int InsertTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductInfoDAO.InsertTblProductInfo(tblProductInfoTO, conn, tran);
        }
        internal static ResultMessage SaveProductInformation(List<TblProductInfoTO> productInfoTOList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
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
        public static int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO)
        {
            return TblProductInfoDAO.UpdateTblProductInfo(tblProductInfoTO);
        }

        public static int UpdateTblProductInfo(TblProductInfoTO tblProductInfoTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductInfoDAO.UpdateTblProductInfo(tblProductInfoTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblProductInfo(Int32 idProduct)
        {
            return TblProductInfoDAO.DeleteTblProductInfo(idProduct);
        }

        public static int DeleteTblProductInfo(Int32 idProduct, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductInfoDAO.DeleteTblProductInfo(idProduct, conn, tran);
        }

      

        #endregion

    }
}
