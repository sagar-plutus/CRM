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
    public class TblVehicleBL : ITblVehicleBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblVehicleDAO _iTblVehicleDAO;
        private readonly Icommondao _icommondao;
        public TblVehicleBL(ITblVehicleDAO iTblVehicleDAO, IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iTblVehicleDAO = iTblVehicleDAO;
            _icommondao = icommondao;
        }
        #region Selection
        public DataTable SelectAllTblVehicle()
        {
            return _iTblVehicleDAO.SelectAllTblVehicle();
        }

        public List<TblVehicleTO> SelectAllTblVehicleList()
        {
            DataTable tblVehicleTODT = _iTblVehicleDAO.SelectAllTblVehicle();
            return ConvertDTToList(tblVehicleTODT);
        }

        public TblVehicleTO SelectTblVehicleTO(Int32 idVehicle)
        {
            return  _iTblVehicleDAO.SelectTblVehicle(idVehicle); 
        }

        public TblVehicleTO SelectTblVehicleTO(Int32 idVehicle, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleDAO.SelectTblVehicle(idVehicle, conn, tran);
        }

        List<TblVehicleTO> ConvertDTToList(DataTable tblVehicleTODT)
        {
            List<TblVehicleTO> tblVehicleTOList = new List<TblVehicleTO>();
            if (tblVehicleTODT != null)
            {
                for (int rowCount = 0; rowCount < tblVehicleTODT.Rows.Count; rowCount++)
                {
                    TblVehicleTO tblVehicleTONew = new TblVehicleTO();
                    if (tblVehicleTODT.Rows[rowCount]["idVehicle"] != DBNull.Value)
                        tblVehicleTONew.IdVehicle = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["idVehicle"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleTypeId"] != DBNull.Value)
                        tblVehicleTONew.VehicleTypeId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["vehicleTypeId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvalUserId"] != DBNull.Value)
                        tblVehicleTONew.ApprovalUserId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvalUserId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvalStatusId"] != DBNull.Value)
                        tblVehicleTONew.ApprovalStatusId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvalStatusId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvedBy"] != DBNull.Value)
                        tblVehicleTONew.ApprovedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvedBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleStatusId"] != DBNull.Value)
                        tblVehicleTONew.VehicleStatusId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["vehicleStatusId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblVehicleTONew.CreatedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblVehicleTONew.UpdatedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvedOn"] != DBNull.Value)
                        tblVehicleTONew.ApprovedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["approvedOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblVehicleTONew.CreatedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblVehicleTONew.UpdatedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblVehicleTONew.IsActive = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["isActive"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleNo"] != DBNull.Value)
                        tblVehicleTONew.VehicleNo = Convert.ToString(tblVehicleTODT.Rows[rowCount]["vehicleNo"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["narration"] != DBNull.Value)
                        tblVehicleTONew.Narration = Convert.ToString(tblVehicleTODT.Rows[rowCount]["narration"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Vehical Type"] != DBNull.Value)
                        tblVehicleTONew.VehicalType  = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Vehical Type"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Approval user"] != DBNull.Value)
                        tblVehicleTONew.ApprovalUser   = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Approval user"].ToString());  
                    if (tblVehicleTODT.Rows[rowCount]["Vehical Status Name"] != DBNull.Value)
                        tblVehicleTONew.VehicalStatusName  = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Vehical Status Name"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Created By Name"] != DBNull.Value)
                        tblVehicleTONew.CreatedByName  = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Created By Name"].ToString());
                    tblVehicleTOList.Add(tblVehicleTONew);
                }
            }
            return tblVehicleTOList;
        }
        public List<TblVehicleTO> GetAllInternalTranferVehical(InternalTransferFilterTO InternalTransferFilterTO)
        {
            return  _iTblVehicleDAO .GetAllInternalTranferVehical(InternalTransferFilterTO); 
        }
        #endregion

        #region Insertion
        public ResultMessage  InsertTblVehicle(TblVehicleTO tblVehicleTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                Boolean isAlerdyExist = _iTblVehicleDAO.checkAlreadyVehicalByName(tblVehicleTO);
                if (isAlerdyExist)
                {
                    resultMessage.DefaultBehaviour("Vehicle No Already Exists");
                    resultMessage.data = tblVehicleTO;
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                tblVehicleTO.CreatedOn = _icommondao.ServerDateTime;
                int result = _iTblVehicleDAO.InsertTblVehicle(tblVehicleTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in data Save InsertTblVehicle()");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour("Data Saved Succeesfully..");
                resultMessage.data = tblVehicleTO; 
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in InsertTblVehicle()");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public int InsertTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleDAO.InsertTblVehicle(tblVehicleTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblVehicle(TblVehicleTO tblVehicleTO)
        {
            return _iTblVehicleDAO.UpdateTblVehicle(tblVehicleTO);
        }

        public int UpdateTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleDAO.UpdateTblVehicle(tblVehicleTO, conn, tran);
        }
        public ResultMessage  UpdateTblVehicleDtl(TblVehicleTO tblVehicleTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                Boolean isAleradyExist = _iTblVehicleDAO.checkAlreadyVehicalById(tblVehicleTO);
                if (!isAleradyExist)
                {
                    resultMessage.DefaultBehaviour("Data not found for Update");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblVehicleDAO.UpdateTblVehicle (tblVehicleTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in data updation UpdateTblVehicleDtl()");
                    return resultMessage;
                }
                else
                    tran.Commit();
                resultMessage.DefaultSuccessBehaviour("Data Updated Succeesfully..");
                resultMessage.data = tblVehicleTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateTblVehicleDtl()");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            } 
        }

        public ResultMessage UpdateVehicalApprovalStatus(TblVehicleTO tblVehicleTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblVehicleTO.ApprovalStatusId == (int)Constants.TransferVehicalApprovalStatusE.REJECTED)
                    tblVehicleTO.VehicleStatusId = (int)Constants.InternalTransferRequestVehicalStatusE.CLOSE;
                else if (tblVehicleTO.ApprovalStatusId == (int)Constants.TransferVehicalApprovalStatusE.AUTHORIZE)
                    tblVehicleTO.VehicleStatusId = (int)Constants.InternalTransferRequestVehicalStatusE.NEW;
                DateTime ServerDateTime = _icommondao.ServerDateTime;
                tblVehicleTO.UpdatedOn = ServerDateTime;
                tblVehicleTO.ApprovedOn = ServerDateTime;
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblVehicleDAO.UpdateVehicalApprovalStatus(tblVehicleTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in data updation UpdateTblVehicleDtl()");
                    return resultMessage;
                }
                else
                    tran.Commit();
                resultMessage.DefaultSuccessBehaviour("Data Updated Succeesfully..");
                resultMessage.data = tblVehicleTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateTblVehicleDtl()");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            } 
        }


        /// <summary>
        /// AmolG[2022-Feb-04] Update the Vehical Status
        /// </summary>
        /// <param name="tblVehicleTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ResultMessage UpdateVehicalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Boolean isAleradyExist = _iTblVehicleDAO.checkAlreadyVehicalById(tblVehicleTO);
                if (!isAleradyExist)
                {
                    resultMessage.DefaultBehaviour("Data not found for Update");
                    return resultMessage;
                }
            
                int result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in data updation UpdateTblVehicleDtl()");
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour("Data Updated Succeesfully..");
                    resultMessage.data = tblVehicleTO;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateTblVehicleDtl()");
                return resultMessage;
            }
            finally
            {
            }
        }
        public ResultMessage UpdateVehicalStatus(TblVehicleTO tblVehicleTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                if (tblVehicleTO == null)
                {
                    resultMessage.DefaultBehaviour("Vehicle Details Not Found");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();

                #region Set Basic Values
                int result = 0;
                DateTime ServerDateTime = _icommondao.ServerDateTime;
                tblVehicleTO.UpdatedOn = ServerDateTime;
                #endregion
                #region Update Vehicle Status
                result = _iTblVehicleDAO.UpdateVehicalStatus(tblVehicleTO, conn, tran);
                if(result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Failed to Update Vehicle Status - UpdateVehicalStatus");
                    return resultMessage;
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = tblVehicleTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateVehicalStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion


    }
}
