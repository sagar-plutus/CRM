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
    public class TblTransferRequestBL : ITblTransferRequestBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblTransferRequestDAO _iTblTransferRequestDAO;
        private readonly ITblEntityRangeBL _iTblEntityRangeBL;
        private readonly Idimensionbl _idimensionbl;
        private readonly Icommondao _icommondao;

        public TblTransferRequestBL(ITblTransferRequestDAO iTblTransferRequestDAO, IConnectionString iConnectionString, ITblEntityRangeBL iTblEntityRangeBL
            , Idimensionbl idimensionbl, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iTblTransferRequestDAO = iTblTransferRequestDAO;
            _iTblEntityRangeBL = iTblEntityRangeBL;
            _idimensionbl = idimensionbl;
            _icommondao = icommondao;
        }

        #region Selection
         

        public List<TblTransferRequestTO> GetTransferRequestDtlList(InternalTransferFilterTO InternalTransferFilterTO)
        {
            return _iTblTransferRequestDAO.GetTransferRequestDtlList (InternalTransferFilterTO); 
        }

        public TblTransferRequestTO SelectTblTransferRequestTO(Int32 idTransferRequest)
        {
            return _iTblTransferRequestDAO.SelectTblTransferRequest(idTransferRequest);
            
        }

        public TblTransferRequestTO SelectTblTransferRequestTO(Int32 idTransferRequest, SqlConnection conn, SqlTransaction tran)
        {
            return  _iTblTransferRequestDAO.SelectTblTransferRequest(idTransferRequest,conn,tran);
        }

        List<TblTransferRequestTO> ConvertDTToList(DataTable tblTransferRequestTODT)
        {
            List<TblTransferRequestTO> tblTransferRequestTOList = new List<TblTransferRequestTO>();
            if (tblTransferRequestTODT != null)
            {
                for (int rowCount = 0; rowCount < tblTransferRequestTODT.Rows.Count; rowCount++)
                {
                    TblTransferRequestTO tblTransferRequestTONew = new TblTransferRequestTO();
                    if (tblTransferRequestTODT.Rows[rowCount]["idTransferRequest"] != DBNull.Value)
                        tblTransferRequestTONew.IdTransferRequest = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["idTransferRequest"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialTypeId"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialTypeId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["materialTypeId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialSubTypeId"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialSubTypeId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["materialSubTypeId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["fromLocationId"] != DBNull.Value)
                        tblTransferRequestTONew.FromLocationId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["fromLocationId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["toLocationId"] != DBNull.Value)
                        tblTransferRequestTONew.ToLocationId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["toLocationId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["unloadingPointId"] != DBNull.Value)
                        tblTransferRequestTONew.UnloadingPointId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["unloadingPointId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblTransferRequestTONew.StatusId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["statusId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedBy = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedBy = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedOn = Convert.ToDateTime(tblTransferRequestTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedOn = Convert.ToDateTime(tblTransferRequestTODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["isAutoCreated"] != DBNull.Value)
                        tblTransferRequestTONew.IsAutoCreated = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["isAutoCreated"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblTransferRequestTONew.IsActive = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["isActive"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["qty"] != DBNull.Value)
                        tblTransferRequestTONew.Qty = Convert.ToDouble(tblTransferRequestTODT.Rows[rowCount]["qty"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["scheduleqty"] != DBNull.Value)
                        tblTransferRequestTONew.Scheduleqty = Convert.ToDouble(tblTransferRequestTODT.Rows[rowCount]["scheduleqty"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["requestDisplayNo"] != DBNull.Value)
                        tblTransferRequestTONew.RequestDisplayNo = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["requestDisplayNo"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["narration"] != DBNull.Value)
                        tblTransferRequestTONew.Narration = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["narration"].ToString());

                    if (tblTransferRequestTODT.Rows[rowCount]["statusChangedBy"] != DBNull.Value)
                        tblTransferRequestTONew.StatusChangedBy  = Convert.ToInt32 (tblTransferRequestTODT.Rows[rowCount]["statusChangedBy"].ToString());

                    if (tblTransferRequestTODT.Rows[rowCount]["statusChangedOn"] != DBNull.Value)
                        tblTransferRequestTONew.StatusChangedOn  = Convert.ToDateTime (tblTransferRequestTODT.Rows[rowCount]["statusChangedOn"].ToString());

                    if (tblTransferRequestTODT.Rows[rowCount]["UpdatedByName"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedByName  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["UpdatedByName"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["CreatedByName"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedByName  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["CreatedByName"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["Status"] != DBNull.Value)
                        tblTransferRequestTONew.Status  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["Status"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["unloadingPoint"] != DBNull.Value)
                        tblTransferRequestTONew.UnloadingPoint  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["unloadingPoint"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["toLocation"] != DBNull.Value)
                        tblTransferRequestTONew.ToLocation  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["toLocation"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["fromLocation"] != DBNull.Value)
                        tblTransferRequestTONew.FromLocation  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["fromLocation"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["MaterialSubType"] != DBNull.Value) 
                        tblTransferRequestTONew.MaterialsSubType  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["MaterialSubType"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialType"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialType  = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["materialType"].ToString());
                    tblTransferRequestTOList.Add(tblTransferRequestTONew);
                }
            }
            return tblTransferRequestTOList;
        }

       
        #endregion

        #region Insertion
        public int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO)
        { 
            return _iTblTransferRequestDAO.InsertTblTransferRequest(tblTransferRequestTO);
        }

        public int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransferRequestDAO.InsertTblTransferRequest(tblTransferRequestTO, conn, tran);
        }

        public ResultMessage InsertTblTransferRequestData(TblTransferRequestTO tblTransferRequestTO)
        {
            ResultMessage resultMessage = new ResultMessage(); 
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();
                DimFinYearTO dimFinYearTO = _idimensionbl.GetCurrentFinancialYear(_icommondao.ServerDateTime, conn, tran);
                TblEntityRangeTO tblEntityRangeTO = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.INTERNAL_TRANSFER_REQUEST, dimFinYearTO.IdFinYear, conn, tran);
                string txnRefNo = string.Empty;
                Int32 entityPrevVal = tblEntityRangeTO.EntityPrevValue;
                entityPrevVal = entityPrevVal + tblEntityRangeTO.IncrementBy;
                txnRefNo = tblEntityRangeTO.Prefix + entityPrevVal;

                tblEntityRangeTO.EntityPrevValue = entityPrevVal;
                int updateResult = _iTblEntityRangeBL.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
                if (updateResult < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateTblEntityRange()");
                    return resultMessage;
                }
                tblTransferRequestTO.RequestDisplayNo = txnRefNo;
                tblTransferRequestTO.CreatedOn = _icommondao.ServerDateTime;
                int result = _iTblTransferRequestDAO.InsertTblTransferRequest(tblTransferRequestTO, conn, tran);
                if (result > 0)
                {
                    tran.Commit(); 
                }
                else
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in data Save");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Data Saved Succeesfully..");
                resultMessage.data = tblTransferRequestTO;
                return resultMessage;
            }
            catch (Exception ex)
            { 
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in InsertTBLTrasferRequest()"); 
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// AmolG[2022-Feb-04] Save Transfer request using Connection Transaction.
        /// </summary>
        /// <param name="tblTransferRequestTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ResultMessage InsertTblTransferRequestData(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                DimFinYearTO dimFinYearTO = _idimensionbl.GetCurrentFinancialYear(_icommondao.ServerDateTime, conn, tran);
                TblEntityRangeTO tblEntityRangeTO = _iTblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(Constants.INTERNAL_TRANSFER_REQUEST, dimFinYearTO.IdFinYear, conn, tran);
                string txnRefNo = string.Empty;
                Int32 entityPrevVal = tblEntityRangeTO.EntityPrevValue;
                entityPrevVal = entityPrevVal + tblEntityRangeTO.IncrementBy;
                txnRefNo = tblEntityRangeTO.Prefix + entityPrevVal;

                tblEntityRangeTO.EntityPrevValue = entityPrevVal;
                int updateResult = _iTblEntityRangeBL.UpdateTblEntityRange(tblEntityRangeTO, conn, tran);
                if (updateResult < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in UpdateTblEntityRange()");
                    return resultMessage;
                }
                tblTransferRequestTO.RequestDisplayNo = txnRefNo;
                tblTransferRequestTO.CreatedOn = _icommondao.ServerDateTime;
                int result = _iTblTransferRequestDAO.InsertTblTransferRequest(tblTransferRequestTO, conn, tran);
                if (result > 0)
                {
                    
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error in data Save");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Data Saved Succeesfully..");
                resultMessage.data = tblTransferRequestTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in InsertTBLTrasferRequest()");
                return resultMessage;
            }
            finally
            {
            }
        }


        #endregion

        #region Updation
        public int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO)
        {
            return _iTblTransferRequestDAO.UpdateTblTransferRequest(tblTransferRequestTO);
        }

        public int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransferRequestDAO.UpdateTblTransferRequest(tblTransferRequestTO, conn, tran);
        }

        /// <summary>
        /// AmolG[2022-Feb-05] Update the Status and Schedule Qty
        /// </summary>
        /// <param name="tblTransferRequestTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateTblTransferRequestScheduleQtyNStatus(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransferRequestDAO.UpdateTblTransferRequestScheduleQtyNStatus(tblTransferRequestTO, conn, tran);
        }

        /// <summary>
        /// AmolG[2022-Feb-05] Update the Schedule Qty
        /// </summary>
        /// <param name="tblTransferRequestTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateTblTransferRequestScheduleQty(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTransferRequestDAO.UpdateTblTransferRequestScheduleQty(tblTransferRequestTO, conn, tran);
        }

        #endregion

    }
}
