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
    public class TblMachineCalibrationBL
    {
        #region Selection
        public static List<TblMachineCalibrationTO> SelectAllTblMachineCalibration()
        {
            return TblMachineCalibrationDAO.SelectAllTblMachineCalibration();
        }

        public static List<TblMachineCalibrationTO> SelectAllTblMachineCalibrationList()
        {
            return TblMachineCalibrationDAO.SelectAllTblMachineCalibration();
        }

        public static TblMachineCalibrationTO SelectTblMachineCalibrationTO(Int32 idMachineCalibration)
        {
            return TblMachineCalibrationDAO.SelectTblMachineCalibration(idMachineCalibration);
        }

        public static TblMachineCalibrationTO SelectTblMachineCalibrationTOByWeighingMachineId(Int32 weighingMachineId)
        {
            return TblMachineCalibrationDAO.SelectTblMachineCalibrationByWeighingMachineId(weighingMachineId);
        }

        #endregion

        #region Insertion
        public static ResultMessage InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            StaticStuff.ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                int result = 0;
                result = TblMachineCalibrationDAO.InsertTblMachineCalibration(tblMachineCalibrationTO);
                if (result <= 0)
                {
                    resultMessage.Text = "";
                    resultMessage.DefaultBehaviour("Record Could not be Saved");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception)
            {
                return null;
            }
               
        }

        public static int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMachineCalibrationDAO.InsertTblMachineCalibration(tblMachineCalibrationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public static int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            return TblMachineCalibrationDAO.UpdateTblMachineCalibration(tblMachineCalibrationTO);
        }

        public static int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblMachineCalibrationDAO.UpdateTblMachineCalibration(tblMachineCalibrationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblMachineCalibration(Int32 idMachineCalibration)
        {
            return TblMachineCalibrationDAO.DeleteTblMachineCalibration(idMachineCalibration);
        }

        public static int DeleteTblMachineCalibration(Int32 idMachineCalibration, SqlConnection conn, SqlTransaction tran)
        {
            return TblMachineCalibrationDAO.DeleteTblMachineCalibration(idMachineCalibration, conn, tran);
        }

        #endregion
        
    }
}
