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
    public class TblAddressBL : ITblAddressBL
    {




        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly IConnectionString _iConnectionString;
        public TblAddressBL(IConnectionString iConnectionString , ITblAddressDAO iTblAddressDAO )
        {
            _iTblAddressDAO = iTblAddressDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public  List<TblAddressTO> SelectAllTblAddressList()
        {
            return  _iTblAddressDAO.SelectAllTblAddress();
        }

        public  TblAddressTO SelectTblAddressTO(Int32 idAddr)
        {
            return  _iTblAddressDAO.SelectTblAddress(idAddr);
        
        }

        /// <summary>
        /// Sanjay [2017-02-10] To Get Specific Address Details of the Given Organization.
        /// It can be dealer,C&F agent Or Competitor
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public  TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblAddressTO tblAddressTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
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

        public  TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId, StaticStuff.Constants.AddressTypeE addressTypeE ,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAddressDAO.SelectOrgAddressWrtAddrType(orgId, addressTypeE, conn, tran);
        }

        public  List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            return _iTblAddressDAO.SelectOrgAddressList(orgId);
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization address list of particular type;
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <returns></returns>
        public  List <TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressDetailOfRegion(orgId, addressTypeE, conn, tran);
                return tblAddressTOList;

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

        #endregion

        #region Insertion
        public  int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            return _iTblAddressDAO.InsertTblAddress(tblAddressTO);
        }

        public  int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.InsertTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            return _iTblAddressDAO.UpdateTblAddress(tblAddressTO);
        }

        public  int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.UpdateTblAddress(tblAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblAddress(Int32 idAddr)
        {
            return _iTblAddressDAO.DeleteTblAddress(idAddr);
        }

        public  int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddressDAO.DeleteTblAddress(idAddr, conn, tran);
        }

        #endregion
        
    }
}
