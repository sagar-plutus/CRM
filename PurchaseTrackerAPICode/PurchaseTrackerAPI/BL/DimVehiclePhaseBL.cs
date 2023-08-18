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

namespace PurchaseTrackerAPI.BL
{
    public class DimVehiclePhaseBL : IDimVehiclePhaseBL
    {
        private readonly IDimVehiclePhaseDAO _iDimVehiclePhaseDAO;
        public DimVehiclePhaseBL(IDimVehiclePhaseDAO iDimVehiclePhaseDAO)
            {
            _iDimVehiclePhaseDAO = iDimVehiclePhaseDAO;
            }
        #region Selection
        //public  List<DimVehiclePhaseTO> SelectAllDimVehiclePhase()
        //{
        //    return _iDimVehiclePhaseDAO.SelectAllDimVehiclePhase();
        //}

        public  List<DimVehiclePhaseTO> SelectAllDimVehiclePhaseList(Int32 isActive)
        {
            return  _iDimVehiclePhaseDAO.SelectAllDimVehiclePhase(isActive);
            //return ConvertDTToList(dimVehiclePhaseTODT);
        }

        public  DimVehiclePhaseTO SelectDimVehiclePhaseTO(Int32 idVehiclePhase)
        {
            List<DimVehiclePhaseTO> dimVehiclePhaseTOList = _iDimVehiclePhaseDAO.SelectDimVehiclePhase(idVehiclePhase);
            //List<DimVehiclePhaseTO> dimVehiclePhaseTOList = ConvertDTToList(dimVehiclePhaseTODT);
            if(dimVehiclePhaseTOList != null && dimVehiclePhaseTOList.Count == 1)
                return dimVehiclePhaseTOList[0];
            else
                return null;
        }

      

        #endregion
        
        #region Insertion
        public  int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO)
        {
            return _iDimVehiclePhaseDAO.InsertDimVehiclePhase(dimVehiclePhaseTO);
        }

        public  int InsertDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehiclePhaseDAO.InsertDimVehiclePhase(dimVehiclePhaseTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO)
        {
            return _iDimVehiclePhaseDAO.UpdateDimVehiclePhase(dimVehiclePhaseTO);
        }

        public  int UpdateDimVehiclePhase(DimVehiclePhaseTO dimVehiclePhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehiclePhaseDAO.UpdateDimVehiclePhase(dimVehiclePhaseTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimVehiclePhase(Int32 idVehiclePhase)
        {
            return _iDimVehiclePhaseDAO.DeleteDimVehiclePhase(idVehiclePhase);
        }

        public  int DeleteDimVehiclePhase(Int32 idVehiclePhase, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimVehiclePhaseDAO.DeleteDimVehiclePhase(idVehiclePhase, conn, tran);
        }

        #endregion
        
    }
}
