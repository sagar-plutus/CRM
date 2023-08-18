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
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.BL
{
    
    public class TblMachineCalibrationBL : ITblMachineCalibrationBL
    {
        private readonly ITblMachineCalibrationDAO _iTblMachineCalibrationDAO;
        public TblMachineCalibrationBL(ITblMachineCalibrationDAO iTblMachineCalibrationDAO)
        {
            _iTblMachineCalibrationDAO = iTblMachineCalibrationDAO;
        }

        #region Selection
        public  List<TblMachineCalibrationTO> SelectAllTblMachineCalibration()
        {
            return _iTblMachineCalibrationDAO.SelectAllTblMachineCalibration();
        }

        public  List<TblMachineCalibrationTO> SelectAllTblMachineCalibrationList()
        {
            return _iTblMachineCalibrationDAO.SelectAllTblMachineCalibration();
        }

        public  TblMachineCalibrationTO SelectTblMachineCalibrationTO(Int32 idMachineCalibration)
        {
            return _iTblMachineCalibrationDAO.SelectTblMachineCalibration(idMachineCalibration);
        }

        public  TblMachineCalibrationTO SelectTblMachineCalibrationTOByWeighingMachineId(Int32 weighingMachineId)
        {
            return _iTblMachineCalibrationDAO.SelectTblMachineCalibrationByWeighingMachineId(weighingMachineId);
        }

        #endregion

        #region Insertion
        public  ResultMessage InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            StaticStuff.ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                int result = 0;
                result = _iTblMachineCalibrationDAO.InsertTblMachineCalibration(tblMachineCalibrationTO);
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

        public  int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMachineCalibrationDAO.InsertTblMachineCalibration(tblMachineCalibrationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            return _iTblMachineCalibrationDAO.UpdateTblMachineCalibration(tblMachineCalibrationTO);
        }

        public  int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMachineCalibrationDAO.UpdateTblMachineCalibration(tblMachineCalibrationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblMachineCalibration(Int32 idMachineCalibration)
        {
            return _iTblMachineCalibrationDAO.DeleteTblMachineCalibration(idMachineCalibration);
        }

        public  int DeleteTblMachineCalibration(Int32 idMachineCalibration, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMachineCalibrationDAO.DeleteTblMachineCalibration(idMachineCalibration, conn, tran);
        }

        #endregion
        
    }
}
