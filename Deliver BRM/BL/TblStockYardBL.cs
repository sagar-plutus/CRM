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
    public class TblStockYardBL
    {
        #region Selection

        public static List<TblStockYardTO> SelectAllTblStockYardList()
        {
            return TblStockYardDAO.SelectAllTblStockYard();
        }

        public static TblStockYardTO SelectTblStockYardTO(Int32 idStockYard)
        {
            return TblStockYardDAO.SelectTblStockYard(idStockYard);
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblStockYard(TblStockYardTO tblStockYardTO)
        {
            return TblStockYardDAO.InsertTblStockYard(tblStockYardTO);
        }

        public static int InsertTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockYardDAO.InsertTblStockYard(tblStockYardTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblStockYard(TblStockYardTO tblStockYardTO)
        {
            return TblStockYardDAO.UpdateTblStockYard(tblStockYardTO);
        }

        public static int UpdateTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockYardDAO.UpdateTblStockYard(tblStockYardTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblStockYard(Int32 idStockYard)
        {
            return TblStockYardDAO.DeleteTblStockYard(idStockYard);
        }

        public static int DeleteTblStockYard(Int32 idStockYard, SqlConnection conn, SqlTransaction tran)
        {
            return TblStockYardDAO.DeleteTblStockYard(idStockYard, conn, tran);
        }

        #endregion
        
    }
}
