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
    public class TblAddressBL
    {
        #region Selection
     
        public static List<TblAddressTO> SelectAllTblAddressList()
        {
            return  TblAddressDAO.SelectAllTblAddress();
        }

        public static TblAddressTO SelectTblAddressTO(Int32 idAddr)
        {
            return  TblAddressDAO.SelectTblAddress(idAddr);
        
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Get Specific Address Details of the Given Organization.
        /// It can be dealer,C&F agent Or Competitor
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public static TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblAddressTO tblAddressTO = TblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
                tblAddressTO.AddressTypeE = addressTypeE;
                return tblAddressTO;

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

        public static TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE ,SqlConnection conn,SqlTransaction tran)
        {
            return TblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
        }

        public static List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            return TblAddressDAO.SelectOrgAddressList(orgId);
        }

        #endregion

        #region Insertion
        public static int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            return TblAddressDAO.InsertTblAddress(tblAddressTO);
        }

        public static int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.InsertTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            return TblAddressDAO.UpdateTblAddress(tblAddressTO);
        }

        public static int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.UpdateTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblAddress(Int32 idAddr)
        {
            return TblAddressDAO.DeleteTblAddress(idAddr);
        }

        public static int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblAddressDAO.DeleteTblAddress(idAddr, conn, tran);
        }

        #endregion
        
    }
}
