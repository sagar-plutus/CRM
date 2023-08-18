using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblAddonsFunDtlsBL: ITblAddonsFunDtlsBL
    {
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblAddonsFunDtlsDAO _iTblAddonsFunDtlsDAO;
        public TblAddonsFunDtlsBL(ITblAddonsFunDtlsDAO iTblAddonsFunDtlsDAO, Icommondao icommondao, IConnectionString iConnectionString)
        {
            _iCommonDAO = icommondao;
            _iTblAddonsFunDtlsDAO = iTblAddonsFunDtlsDAO;
            _iConnectionString = iConnectionString;

        }

        #region Selection
        public  List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtlsList()
        {
            return _iTblAddonsFunDtlsDAO.SelectAllTblAddonsFunDtls();
        }

        public  TblAddonsFunDtlsTO SelectTblAddonsFunDtlsTO(int idAddonsfunDtls)
        {
            return _iTblAddonsFunDtlsDAO.SelectTblAddonsFunDtls(idAddonsfunDtls);

        }
        public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByRootScheduleId(Int32 rootScheduleId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.SelectTblAddonsFunDtlsByRootScheduleId(rootScheduleId,conn,tran);
        }

        public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByPurchaseInvoiceId(Int32 purchaseInvoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.SelectTblAddonsFunDtlsByPurchaseInvoiceId(purchaseInvoiceId,conn,tran);
        }
         public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsBySpotVehicleId(Int32 spotVehicleId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.SelectTblAddonsFunDtlsBySpotVehicleId(spotVehicleId,conn,tran);
        }

        public  List<TblAddonsFunDtlsTO> SelectAddonDetails(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds)
        {
            return _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(transId, ModuleId, TransactionType, PageElementId, transIds);
        }

        #endregion

        #region Insertion
        public  ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.CreatedOn =  _iCommonDAO.ServerDateTime;
            tblAddonsFunDtlsTO.IsActive = 1;
            result = _iTblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Inserting Data into TblAddonsFunDtls";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public  ResultMessage InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            result = _iTblAddonsFunDtlsDAO.InsertTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Updation
        public  ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            tblAddonsFunDtlsTO.UpdatedOn =  _iCommonDAO.ServerDateTime;
            tblAddonsFunDtlsTO.UpdatedBy = tblAddonsFunDtlsTO.CreatedBy;
            result = _iTblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Updating Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Updating Data";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public  ResultMessage UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            result = _iTblAddonsFunDtlsDAO.UpdateTblAddonsFunDtls(tblAddonsFunDtlsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Inserting Data into TblAddonsFunDtls");
                resultMessage.DisplayMessage = "Error While Sending Email";
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Deletion
        public  int DeleteTblAddonsFunDtls(int idAddonsfunDtls)
        {
            return _iTblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls);
        }

        public  int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.DeleteTblAddonsFunDtls(idAddonsfunDtls, conn, tran);
        }

        public  int DeleteAllPhotoAgainstVehScheduleId(int rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.DeleteAllPhotoAgainstVehScheduleId(rootScheduleId, conn, tran);
        }
        public  int DeleteAllPhotoAgainstVehInvoiceId(int purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.DeleteAllPhotoAgainstVehInvoiceId(purchaseInvoiceId, conn, tran);
        }
        public  int DeleteAllPhotoAgainstSpotVehId(int spotVehicleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAddonsFunDtlsDAO.DeleteAllPhotoAgainstSpotVehId(spotVehicleId, conn, tran);
        }

        #endregion

    }
}
