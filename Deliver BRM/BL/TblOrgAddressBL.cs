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
    public class TblOrgAddressBL
    {
        #region Selection
       
        public static List<TblOrgAddressTO> SelectAllTblOrgAddressList()
        {
          return TblOrgAddressDAO.SelectAllTblOrgAddress();
           
        }

        public static TblOrgAddressTO SelectTblOrgAddressTO(Int32 idOrgAddr)
        {
            return  TblOrgAddressDAO.SelectTblOrgAddress(idOrgAddr);
           
        }

       

        #endregion
        
        #region Insertion
        public static int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            return TblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO);
        }

        public static int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            return TblOrgAddressDAO.UpdateTblOrgAddress(tblOrgAddressTO);
        }

        public static int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgAddressDAO.UpdateTblOrgAddress(tblOrgAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblOrgAddress(Int32 idOrgAddr)
        {
            return TblOrgAddressDAO.DeleteTblOrgAddress(idOrgAddr);
        }

        public static int DeleteTblOrgAddress(Int32 idOrgAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgAddressDAO.DeleteTblOrgAddress(idOrgAddr, conn, tran);
        }

        #endregion
        
    }
}
