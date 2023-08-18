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
    public class TblWeighingMachineBL
    {
        #region Selection
        public static List<TblWeighingMachineTO> SelectAllTblWeighingMachineList()
        {
            return  TblWeighingMachineDAO.SelectAllTblWeighingMachine();
        }

        public static List<TblWeighingMachineTO> SelectAllTblWeighingMachineOfWeighingList(int loadingId)
        {
            return TblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(loadingId);
        }

        public static List<TblWeighingMachineTO> SelectAllTblWeighingMachineOfWeighingList(int loadingId,SqlConnection conn,SqlTransaction tran)
        {
            return TblWeighingMachineDAO.SelectAllTblWeighingMachineOfWeighingList(loadingId, conn, tran);
        }
        public static List<DropDownTO> SelectTblWeighingMachineDropDownList()
        {
            return TblWeighingMachineDAO.SelectTblWeighingMachineDropDownList();
        }
        public static TblWeighingMachineTO SelectTblWeighingMachineTO(Int32 idWeighingMachine)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblWeighingMachineDAO.SelectTblWeighingMachine(idWeighingMachine, conn, tran);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public static TblWeighingMachineTO SelectTblWeighingMachineTO(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMachineDAO.SelectTblWeighingMachine(idWeighingMachine, conn, tran);
        }
        #endregion

        #region Insertion
        public static int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            return TblWeighingMachineDAO.InsertTblWeighingMachine(tblWeighingMachineTO);
        }

        public static int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMachineDAO.InsertTblWeighingMachine(tblWeighingMachineTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            return TblWeighingMachineDAO.UpdateTblWeighingMachine(tblWeighingMachineTO);
        }

        public static int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMachineDAO.UpdateTblWeighingMachine(tblWeighingMachineTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblWeighingMachine(Int32 idWeighingMachine)
        {
            return TblWeighingMachineDAO.DeleteTblWeighingMachine(idWeighingMachine);
        }

        public static int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran)
        {
            return TblWeighingMachineDAO.DeleteTblWeighingMachine(idWeighingMachine, conn, tran);
        }

        #endregion
        
    }
}
