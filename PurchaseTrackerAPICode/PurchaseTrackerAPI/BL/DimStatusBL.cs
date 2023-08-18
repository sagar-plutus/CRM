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
    public class DimStatusBL : IDimStatusBL
    {

        private IDimStatusDAO _idimStatusDAO;
        private readonly IConnectionString _iConnectionString;
        public DimStatusBL(IDimStatusDAO idimStatusDAO, IConnectionString iConnectionString)
        {
            _idimStatusDAO = idimStatusDAO;
            _iConnectionString = iConnectionString;

        }
        #region Selection
        public  List<DimStatusTO> SelectAllDimStatusList()
        {
            return _idimStatusDAO.SelectAllDimStatus();
        }


        public DimStatusTO SelectDimStatusTOByIotStatusId(Int32 iotStatusId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _idimStatusDAO.SelectDimStatusTOByIotStatusId(iotStatusId, conn, tran);
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
        /// <summary>
        /// Sanjay [2017-03-07] Returns list of status against given transaction type
        /// If param value= 0 then return all statuses
        /// </summary>
        /// <param name="txnTypeId"></param>
        /// <returns></returns>
        public  List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId)
        {
            return _idimStatusDAO.SelectAllDimStatus(txnTypeId);
        }

        public  DimStatusTO SelectDimStatusTO(Int32 idStatus)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _idimStatusDAO.SelectDimStatus(idStatus, conn, tran);
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
        public  int InsertDimStatus(DimStatusTO dimStatusTO)
        {
            return _idimStatusDAO.InsertDimStatus(dimStatusTO);
        }

        public  int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return _idimStatusDAO.InsertDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimStatus(DimStatusTO dimStatusTO)
        {
            return _idimStatusDAO.UpdateDimStatus(dimStatusTO);
        }

        public  int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return _idimStatusDAO.UpdateDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimStatus(Int32 idStatus)
        {
            return _idimStatusDAO.DeleteDimStatus(idStatus);
        }

        public  int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran)
        {
            return _idimStatusDAO.DeleteDimStatus(idStatus, conn, tran);
        }

        #endregion
        
    }
}
