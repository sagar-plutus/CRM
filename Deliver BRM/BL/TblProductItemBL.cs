using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.BL
{
    public class TblProductItemBL
    {
        #region Selection

        public static List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0)
        {
            return  TblProductItemDAO.SelectAllTblProductItem(specificationId);
        }

        public static TblProductItemTO SelectTblProductItemTO(Int32 idProdItem)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblProductItemDAO.SelectTblProductItem(idProdItem,conn,tran);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static TblProductItemTO SelectTblProductItemTO(Int32 idProdItem,SqlConnection conn,SqlTransaction tran)
        {
            return TblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);
        }


        /// <summary>
        /// Sudhir[15-MAR-2018]
        /// Get List of ProductItemTO Based On Category/Subcategory or Specification
        /// </summary>
        /// <param name="tblProductItemTO"></param>
        /// <returns></returns>
        /// 
        public static List<TblProductItemTO> SelectProductItemList(Int32 idProdClass)
        {
            string prodClassIds = BL.TblProdClassificationBL.SelectProdtClassificationListOnType(idProdClass);
            if (prodClassIds != string.Empty)
            {
                return TblProductItemDAO.SelectListOfProductItemTOOnprdClassId(prodClassIds);
            }
            else
                return null;
        }
        /// <summary>
        /// Sudhir[04-APR-2017] Added for Get List of Items Based on isStockRequire Flag
        /// </summary>
        /// <param name="isStockRequire"></param>
        /// <returns></returns>
        public static List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            return TblProductItemDAO.SelectProductItemListStockUpdateRequire(isStockRequire);
        }

        #endregion

        #region Insertion
        public static int InsertTblProductItem(TblProductItemTO tblProductItemTO)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblProductItemTO.IsBaseItemForRate > 0)
                {
                    result = TblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        return result;
                    }
                }

                result = TblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;

            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }

        public static int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblProductItemTO.IsBaseItemForRate > 0)
                {
                    result = TblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        return result;
                    }
                }

                result= TblProductItemDAO.UpdateTblProductItem(tblProductItemTO,conn,tran);
                   if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }

        public static int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
        }
        //Priyanka [22-05-2018]: Added to change the product item tax type(HSN/SSN)
        public static int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, Int32 isActive, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductItemDAO.UpdateTblProductItemTaxType(idClassStr, codeTypeId, isActive, conn, tran);
        }
        #endregion

        #region Deletion
        public static int DeleteTblProductItem(Int32 idProdItem)
        {
            return TblProductItemDAO.DeleteTblProductItem(idProdItem);
        }

        public static int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            return TblProductItemDAO.DeleteTblProductItem(idProdItem, conn, tran);
        }

        #endregion
        
    }
}
