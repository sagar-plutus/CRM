using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Z.Expressions;
using System.Data;
using System.Web;
using System.IO;


namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseSchStatusHistoryBL : ITblPurchaseSchStatusHistoryBL
    {

        private readonly ITblPurchaseSchStatusHistoryDAO _iTblPurchaseSchStatusHistoryDAO;

        public TblPurchaseSchStatusHistoryBL(ITblPurchaseSchStatusHistoryDAO iTblPurchaseSchStatusHistoryDAO)
        {
            _iTblPurchaseSchStatusHistoryDAO = iTblPurchaseSchStatusHistoryDAO;
        }

        #region Selection
        public List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistory()
        {
            return _iTblPurchaseSchStatusHistoryDAO.SelectAllTblPurchaseSchStatusHistory();
        }

        public List<TblPurchaseSchStatusHistoryTO> SelectAllTblPurchaseSchStatusHistoryList()
        {
           return _iTblPurchaseSchStatusHistoryDAO.SelectAllTblPurchaseSchStatusHistory();
        }

        public TblPurchaseSchStatusHistoryTO SelectTblPurchaseSchStatusHistoryTO(Int32 idPurchaseSchStatusHistory)
        {
            List<TblPurchaseSchStatusHistoryTO> tblPurchaseSchStatusHistoryTOList = _iTblPurchaseSchStatusHistoryDAO.SelectTblPurchaseSchStatusHistory(idPurchaseSchStatusHistory);
            if(tblPurchaseSchStatusHistoryTOList != null && tblPurchaseSchStatusHistoryTOList.Count == 1)
                return tblPurchaseSchStatusHistoryTOList[0];
            else
                return null;
        }

       
        #endregion
        
        #region Insertion
        public int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO)
        {
            return _iTblPurchaseSchStatusHistoryDAO.InsertTblPurchaseSchStatusHistory(tblPurchaseSchStatusHistoryTO);
        }

        public int InsertTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchStatusHistoryDAO.InsertTblPurchaseSchStatusHistory(tblPurchaseSchStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO)
        {
            return _iTblPurchaseSchStatusHistoryDAO.UpdateTblPurchaseSchStatusHistory(tblPurchaseSchStatusHistoryTO);
        }

        public  int UpdateTblPurchaseSchStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchStatusHistoryDAO.UpdateTblPurchaseSchStatusHistory(tblPurchaseSchStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory)
        {
            return _iTblPurchaseSchStatusHistoryDAO.DeleteTblPurchaseSchStatusHistory(idPurchaseSchStatusHistory);
        }

        public  int DeleteTblPurchaseSchStatusHistory(Int32 idPurchaseSchStatusHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchStatusHistoryDAO.DeleteTblPurchaseSchStatusHistory(idPurchaseSchStatusHistory, conn, tran);
        }

        public int DeletePurchaseVehHistoryDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchStatusHistoryDAO.DeletePurchaseVehHistoryDtls(purchaseScheduleId, conn, tran);
        }


        #endregion

        public ResultMessage SavePurVehStatusHistory(TblPurchaseScheduleSummaryTO scheduleSummaryTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;

            try
            {

                if(scheduleSummaryTO == null)
                {
                    throw new Exception("scheduleSummaryTO == NULL");
                }

                TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO = new TblPurchaseSchStatusHistoryTO();
                if (scheduleSummaryTO.IsStatusUpdate == 1)
                    tblPurchaseSchStatusHistoryTO.StatusId = scheduleSummaryTO.StatusId;
                else
                    tblPurchaseSchStatusHistoryTO.VehiclePhaseId = scheduleSummaryTO.VehiclePhaseId;

                if(scheduleSummaryTO.UpdatedBy > 0)
                {
                    tblPurchaseSchStatusHistoryTO.CreatedBy = scheduleSummaryTO.UpdatedBy;
                    tblPurchaseSchStatusHistoryTO.CreatedOn = scheduleSummaryTO.UpdatedOn;
                }
                else
                {
                    tblPurchaseSchStatusHistoryTO.CreatedBy = scheduleSummaryTO.CreatedBy;
                    tblPurchaseSchStatusHistoryTO.CreatedOn = scheduleSummaryTO.CreatedOn;
                }

                //if(scheduleSummaryTO.StatusId == (Int32)Constants.TranStatusE.New)
                //{
                //    tblPurchaseSchStatusHistoryTO.CreatedBy = scheduleSummaryTO.CreatedBy;
                //    tblPurchaseSchStatusHistoryTO.CreatedOn = scheduleSummaryTO.CreatedOn;
                //}
                //else
                //{
                //    tblPurchaseSchStatusHistoryTO.CreatedBy = scheduleSummaryTO.UpdatedBy;
                //    tblPurchaseSchStatusHistoryTO.CreatedOn = scheduleSummaryTO.UpdatedOn;
                //}

                tblPurchaseSchStatusHistoryTO.PurchaseScheduleSummaryId = scheduleSummaryTO.ActualRootScheduleId;

                result = _iTblPurchaseSchStatusHistoryDAO.InsertTblPurchaseSchStatusHistory(tblPurchaseSchStatusHistoryTO, conn, tran);
                if(result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in ResultMessage SavePurVehStatusHistory(TblPurchaseSchStatusHistoryTO tblPurchaseSchStatusHistoryTO,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;

            }
        }

    }
}
