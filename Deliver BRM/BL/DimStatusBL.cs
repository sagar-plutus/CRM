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
    public class DimStatusBL
    {
        #region Selection
        public static List<DimStatusTO> SelectAllDimStatusList()
        {
            return DimStatusDAO.SelectAllDimStatus();
        }

        /// <summary>
        /// Sanjay [2017-03-07] Returns list of status against given transaction type
        /// If param value= 0 then return all statuses
        /// </summary>
        /// <param name="txnTypeId"></param>
        /// <returns></returns>
        public static List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId)
        {
            return DimStatusDAO.SelectAllDimStatus(txnTypeId);
        }

        public static DimStatusTO SelectDimStatusTO(Int32 idStatus)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return DimStatusDAO.SelectDimStatus(idStatus, conn, tran);
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


        public static DimStatusTO SelectDimStatusTOByIotStatusId(Int32 iotStatusId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return DimStatusDAO.SelectDimStatusByIotStatusId(iotStatusId, conn, tran);
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
        public static int InsertDimStatus(DimStatusTO dimStatusTO)
        {
            return DimStatusDAO.InsertDimStatus(dimStatusTO);
        }

        public static int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.InsertDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateDimStatus(DimStatusTO dimStatusTO)
        {
            return DimStatusDAO.UpdateDimStatus(dimStatusTO);
        }

        public static int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.UpdateDimStatus(dimStatusTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteDimStatus(Int32 idStatus)
        {
            return DimStatusDAO.DeleteDimStatus(idStatus);
        }

        public static int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran)
        {
            return DimStatusDAO.DeleteDimStatus(idStatus, conn, tran);
        }

        #endregion
        
    }
}
