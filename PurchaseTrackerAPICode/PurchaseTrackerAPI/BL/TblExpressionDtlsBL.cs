using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblExpressionDtlsBL : ITblExpressionDtlsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblExpressionDtlsDAO _iTblExpressionDtlsDAO;
        public TblExpressionDtlsBL(ITblExpressionDtlsDAO iTblExpressionDtlsDAO, IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _iCommonDAO = icommondao;
            _iTblExpressionDtlsDAO = iTblExpressionDtlsDAO;
        }
        #region Selection
        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId)
        {
            return _iTblExpressionDtlsDAO.SelectAllTblExpressionDtls(isActive, prodClassId);
        }

        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblExpressionDtlsDAO.SelectAllTblExpressionDtls(isActive, prodClassId, conn, tran);
        }
        

        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtlsList()
        {
            return _iTblExpressionDtlsDAO.SelectAllTblExpressionDtls();
            //return ConvertDTToList(tblExpressionDtlsTODT);
        }

        public TblExpressionDtlsTO SelectTblExpressionDtlsTO(Int32 idExpDtls)
        {
            List<TblExpressionDtlsTO> tblExpressionDtlsTOList = _iTblExpressionDtlsDAO.SelectTblExpressionDtls(idExpDtls);
            if (tblExpressionDtlsTOList != null && tblExpressionDtlsTOList.Count == 1)
                return tblExpressionDtlsTOList[0];
            else
                return null;
        }



        #endregion

        #region Insertion
        public int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO)
        {
            return _iTblExpressionDtlsDAO.InsertTblExpressionDtls(tblExpressionDtlsTO);
        }

        public int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblExpressionDtlsDAO.InsertTblExpressionDtls(tblExpressionDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO)
        {
            return _iTblExpressionDtlsDAO.UpdateTblExpressionDtls(tblExpressionDtlsTO);
        }

        public int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblExpressionDtlsDAO.UpdateTblExpressionDtls(tblExpressionDtlsTO, conn, tran);
        }

        public int UpdateTblExpressionDtlsEdit(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblExpressionDtlsDAO.UpdateTblExpressionDtlsEdit(tblExpressionDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblExpressionDtls(Int32 idExpDtls)
        {
            return _iTblExpressionDtlsDAO.DeleteTblExpressionDtls(idExpDtls);
        }

        public int DeleteTblExpressionDtls(Int32 idExpDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblExpressionDtlsDAO.DeleteTblExpressionDtls(idExpDtls, conn, tran);
        }

        #endregion
        public List<TblExpressionDtlsTO> GetHistoryOfExpressionsbyUniqueNo(int uniqueTrackId)
        {
            return _iTblExpressionDtlsDAO.GetHistoryOfExpressionsbyUniqueNo(uniqueTrackId);
        }

        public ResultMessage EditExpressionDetails(TblExpressionDtlsTO expTO, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Deactive previous active variables change updated on,updated by
                if (expTO != null)
                {
                    expTO.IsActive = 0;
                    expTO.UpdatedBy = loginUserId;
                    expTO.UpdatedOn = currentDate;

                    result = UpdateTblExpressionDtlsEdit(expTO, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        throw new Exception("Error while Updating active status of variable");
                    }

                }

                //Insert New records
                if (expTO != null)
                {
                    expTO.IsActive = 1;
                    expTO.CreatedBy = loginUserId;
                    expTO.CreatedOn = currentDate;
                    expTO.UpdatedBy = loginUserId;
                    expTO.UpdatedOn = currentDate;
                    result = InsertTblExpressionDtls(expTO, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        throw new Exception("Error while updating variable details");
                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour("Expression updated Successfully.");
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    throw new Exception("Error while saving expression details");
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostVariableDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }
    }


}
