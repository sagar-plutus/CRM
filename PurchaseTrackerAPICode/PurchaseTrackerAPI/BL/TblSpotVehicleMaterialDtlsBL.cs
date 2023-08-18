using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblSpotVehicleMaterialDtlsBL : ITblSpotVehicleMaterialDtlsBL
    {
        private readonly ITblSpotVehicleMaterialDtlsDAO _iTblSpotVehicleMaterialDtlsDAO;
        public TblSpotVehicleMaterialDtlsBL(ITblSpotVehicleMaterialDtlsDAO iTblSpotVehicleMaterialDtlsDAO)
        {
            _iTblSpotVehicleMaterialDtlsDAO = iTblSpotVehicleMaterialDtlsDAO ;
        }
        #region Selection
        public  List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtls()
        {
            return _iTblSpotVehicleMaterialDtlsDAO.SelectAllTblSpotVehicleMaterialDtls();
        }

        public  List<TblSpotVehMatDtlsTO> SelectAllTblSpotVehicleMaterialDtlsList()
        {
            return _iTblSpotVehicleMaterialDtlsDAO.SelectAllTblSpotVehicleMaterialDtls();
            
        }

        public  TblSpotVehMatDtlsTO SelectTblSpotVehicleMaterialDtlsTO(Int32 idTblSpotVehicleMaterialDtls)
        {
            List<TblSpotVehMatDtlsTO> tblSpotVehicleMaterialDtlsTOList = _iTblSpotVehicleMaterialDtlsDAO.SelectTblSpotVehicleMaterialDtls(idTblSpotVehicleMaterialDtls);
            if (tblSpotVehicleMaterialDtlsTOList != null && tblSpotVehicleMaterialDtlsTOList.Count == 1)
                return tblSpotVehicleMaterialDtlsTOList[0];
            else
                return null;
        }

        public  List<TblSpotVehMatDtlsTO> SelectAllSpotVehMatDtlsBySpotVehId(Int32 spotEntryVehicleId)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.SelectTblSpotVehicleMaterialDtls(spotEntryVehicleId);
        }
        

        #endregion

        #region Insertion
        public  int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.InsertTblSpotVehicleMaterialDtls(tblSpotVehicleMaterialDtlsTO);
        }

        public  int InsertTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.InsertTblSpotVehicleMaterialDtls(tblSpotVehicleMaterialDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public  int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.UpdateTblSpotVehicleMaterialDtls(tblSpotVehicleMaterialDtlsTO);
        }

        public  int UpdateTblSpotVehicleMaterialDtls(TblSpotVehMatDtlsTO tblSpotVehicleMaterialDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.UpdateTblSpotVehicleMaterialDtls(tblSpotVehicleMaterialDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public  int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.DeleteTblSpotVehicleMaterialDtls(idTblSpotVehicleMaterialDtls);
        }

        public  int DeleteTblSpotVehicleMaterialDtls(Int32 idTblSpotVehicleMaterialDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.DeleteTblSpotVehicleMaterialDtls(idTblSpotVehicleMaterialDtls, conn, tran);
        }
         public  int DeleteSpotVehMaterialDtls(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotVehicleMaterialDtlsDAO.DeleteSpotVehMaterialDtls(spotVehicleId, conn, tran);
        }
        #endregion

    }
}
