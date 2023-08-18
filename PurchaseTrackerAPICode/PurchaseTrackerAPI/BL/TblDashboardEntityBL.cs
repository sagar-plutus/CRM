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
    public class TblDashboardEntityBL : ITblDashboardEntityBL
    {

        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblDashboardEntityDAO _iTblDashboardEntityDAO;
        private readonly ITblDashboardEntityHistoryBL _iTblDashboardEntityHistoryBL;

        public TblDashboardEntityBL(Icommondao icommondao, IConnectionString iConnectionString, ITblDashboardEntityDAO iTblDashboardEntityDAO,
            ITblDashboardEntityHistoryBL iTblDashboardEntityHistoryBL)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblDashboardEntityDAO = iTblDashboardEntityDAO;
            _iTblDashboardEntityHistoryBL = iTblDashboardEntityHistoryBL;


        }

        #region Selection
        public List<TblDashboardEntityTO> SelectAllTblDashboardEntity()
        {
            return _iTblDashboardEntityDAO.SelectAllTblDashboardEntity();
        }

        public List<TblDashboardEntityTO> SelectAllTblDashboardEntityList()
        {
            return _iTblDashboardEntityDAO.SelectAllTblDashboardEntity();
            
        }

        public  TblDashboardEntityTO SelectTblDashboardEntityTO(Int32 idDashboardEntity)
        {
            List<TblDashboardEntityTO> tblDashboardEntityTOList = _iTblDashboardEntityDAO.SelectTblDashboardEntity(idDashboardEntity);
            if(tblDashboardEntityTOList != null && tblDashboardEntityTOList.Count == 1)
                return tblDashboardEntityTOList[0];
            else
                return null;
        }

        public List<TblDashboardEntityTO> SelectTblDashboardEntityListByModuleId(Int32 moduleId)
        {
            return _iTblDashboardEntityDAO.SelectAllDashboardEntityListByModuleId(moduleId);
        }

        public TblDashboardEntityTO SelectTblDashboardEntityTO(Int32 idDashboardEntity,SqlConnection conn,SqlTransaction tran)
        {
            List<TblDashboardEntityTO> tblDashboardEntityTOList = _iTblDashboardEntityDAO.SelectTblDashboardEntity(idDashboardEntity,conn,tran);
            if (tblDashboardEntityTOList != null && tblDashboardEntityTOList.Count == 1)
                return tblDashboardEntityTOList[0];
            else
                return null;
        }
        public List<TblDashboardEntityTO> SelectAllDashboardEntityList(Int32 moduleId, int dashboardEntityId, DateTime fromDate, DateTime toDate)
        {
            return _iTblDashboardEntityDAO.SelectAllDashboardEntityList(moduleId, dashboardEntityId,fromDate, toDate);
        }

        #endregion

        #region Insertion
        public int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO)
        {
            return _iTblDashboardEntityDAO.InsertTblDashboardEntity(tblDashboardEntityTO);
        }

        public int InsertTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityDAO.InsertTblDashboardEntity(tblDashboardEntityTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO)
        {
            return _iTblDashboardEntityDAO.UpdateTblDashboardEntity(tblDashboardEntityTO);
        }

        public int UpdateTblDashboardEntity(TblDashboardEntityTO tblDashboardEntityTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityDAO.UpdateTblDashboardEntity(tblDashboardEntityTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblDashboardEntity(Int32 idDashboardEntity)
        {
            return _iTblDashboardEntityDAO.DeleteTblDashboardEntity(idDashboardEntity);
        }

        public int DeleteTblDashboardEntity(Int32 idDashboardEntity, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblDashboardEntityDAO.DeleteTblDashboardEntity(idDashboardEntity, conn, tran);
        }

        #endregion

        public ResultMessage PostDashboardEntityDtls(List<TblDashboardEntityTO> tblDashboardEntityTOList,Int32 loginUserId)
        {

            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = UpdateDashboardEntity(tblDashboardEntityTOList, loginUserId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostDashboardEntityDtls(List<TblDashboardEntityTO> tblDashboardEntityTOList,Int32 loginUserId)");
                return resultMessage;
            }
        
         }

        public ResultMessage UpdateDashboardEntity(List<TblDashboardEntityTO> tblDashboardEntityTOList,Int32 loginUserId,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            DateTime serverDate = _iCommonDAO.ServerDateTime;
            try
            {

                if(tblDashboardEntityTOList == null)
                {
                    throw new Exception("tblDashboardEntityTOList == null");
                }

                for (int i = 0; i < tblDashboardEntityTOList.Count; i++)
                {

                    TblDashboardEntityTO  existingTO = SelectTblDashboardEntityTO(tblDashboardEntityTOList[i].IdDashboardEntity, conn, tran); 
                    if(existingTO == null)
                    {
                        throw new Exception(" existingTO == null");
                    }

                    TblDashboardEntityHistoryTO tblDashboardEntityHistoryTO = new TblDashboardEntityHistoryTO();
                    tblDashboardEntityHistoryTO.EntityName = existingTO.EntityName;
                    tblDashboardEntityHistoryTO.EntityValue = existingTO.EntityValue;
                    tblDashboardEntityHistoryTO.DashboardEntityId = existingTO.IdDashboardEntity;
                    tblDashboardEntityHistoryTO.ModuleId = existingTO.ModuleId;
                    tblDashboardEntityHistoryTO.CreatedBy = existingTO.CreatedBy;
                    tblDashboardEntityHistoryTO.CreatedOn = existingTO.CreatedOn;

                    result = _iTblDashboardEntityHistoryBL.InsertTblDashboardEntityHistory(tblDashboardEntityHistoryTO, conn, tran);
                    if(result != 1)
                    {
                        throw new Exception("Error in InsertTblDashboardEntityHistory(tblDashboardEntityHistoryTO, conn, tran);");
                    }

                    tblDashboardEntityTOList[i].CreatedBy = loginUserId;
                    tblDashboardEntityTOList[i].CreatedOn = serverDate;
                    result = UpdateTblDashboardEntity(tblDashboardEntityTOList[i], conn, tran);
                    if(result == -1)
                    {
                        throw new Exception("Error in UpdateTblDashboardEntity(tblDashboardEntityTOList[i], conn, tran);");
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in InsertOrUpdateDashboardEntity(List<TblDashboardEntityTO> tblDashboardEntityTOList,Int32 loginUserId,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }

        }


    }
}
