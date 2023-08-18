using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseVehFreightDtlsBL : ITblPurchaseVehFreightDtlsBL
    {
        private readonly ITblPurchaseVehFreightDtlsDAO _iTblPurchaseVehFreightDtlsDAO;
        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;

        public TblPurchaseVehFreightDtlsBL(
        Icommondao icommondao, ITblPurchaseVehFreightDtlsDAO iTblPurchaseVehFreightDtlsDAO,
        IConnectionString iConnectionString)
        {
            _iCommonDAO = icommondao;
            _iTblPurchaseVehFreightDtlsDAO = iTblPurchaseVehFreightDtlsDAO;
            _iConnectionString = iConnectionString;
        }

        #region Selection
        public List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtls()
        {
            return _iTblPurchaseVehFreightDtlsDAO.SlectAllTblPurchaseVehFreightDtls();
        }

        public  List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtlsList()
        {
            return _iTblPurchaseVehFreightDtlsDAO.SlectAllTblPurchaseVehFreightDtls();
            
        }

        public List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehFreightDtlsDAO.SelectFreightDtlsByPurchaseScheduleId(purchaseScheduleId,conn,tran);
        }
        public List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId)
        {
           return _iTblPurchaseVehFreightDtlsDAO.SelectFreightDtlsByPurchaseScheduleId(purchaseScheduleId);
        }


        public TblPurchaseVehFreightDtlsTO SelectTblPurchaseVehFreightDtlsTO(Int32 idPurchaseVehFreightDtls)
        {
            List<TblPurchaseVehFreightDtlsTO> tblPurchaseVehFreightDtlsTOList = _iTblPurchaseVehFreightDtlsDAO.SelectTblPurchaseVehFreightDtls(idPurchaseVehFreightDtls);
            if(tblPurchaseVehFreightDtlsTOList != null && tblPurchaseVehFreightDtlsTOList.Count == 1)
                return tblPurchaseVehFreightDtlsTOList[0];
            else
                return null;
        }

      

        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO)
        {
            return _iTblPurchaseVehFreightDtlsDAO.InsertTblPurchaseVehFreightDtls(tblPurchaseVehFreightDtlsTO);
        }

        public  int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehFreightDtlsDAO.InsertTblPurchaseVehFreightDtls(tblPurchaseVehFreightDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO)
        {
            return _iTblPurchaseVehFreightDtlsDAO.UpdateTblPurchaseVehFreightDtls(tblPurchaseVehFreightDtlsTO);
        }

        public  int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehFreightDtlsDAO.UpdateTblPurchaseVehFreightDtls(tblPurchaseVehFreightDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls)
        {
            return _iTblPurchaseVehFreightDtlsDAO.DeleteTblPurchaseVehFreightDtls(idPurchaseVehFreightDtls);
        }

        public  int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehFreightDtlsDAO.DeleteTblPurchaseVehFreightDtls(idPurchaseVehFreightDtls, conn, tran);
        }

        #endregion

        public ResultMessage PostVehicleFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO,Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = PostVehicleFreightDtls(tblPurchaseVehFreightDtlsTO, loginUserId, conn, tran);
                if(resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostVehicleFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO,Int32 loginUserId)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage PostVehicleFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, Int32 loginUserId,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {





                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in PostVehicleFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, Int32 loginUserId,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;
            }
        }

        public int DeletePurchaseVehFreightDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehFreightDtlsDAO.DeletePurchaseVehFreightDtls(purchaseScheduleId, conn, tran);
        }


    }
}
