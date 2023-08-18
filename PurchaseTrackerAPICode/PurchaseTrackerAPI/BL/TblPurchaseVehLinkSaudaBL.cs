using PurchaseTrackerAPI.BL;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseVehLinkSaudaBL : ITblPurchaseVehLinkSaudaBL
    {
        public readonly ITblPurchaseVehLinkSaudaDAO _iTblPurchaseVehLinkSaudaDAO;

        public TblPurchaseVehLinkSaudaBL(ITblPurchaseVehLinkSaudaDAO iTblPurchaseVehLinkSaudaDAO)
        {
            _iTblPurchaseVehLinkSaudaDAO = iTblPurchaseVehLinkSaudaDAO;
        }

        #region Selection
        public List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda()
        {
            return _iTblPurchaseVehLinkSaudaDAO.SelectAllTblPurchaseVehLinkSauda();
        }

        public  List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSaudaList()
        {
            return _iTblPurchaseVehLinkSaudaDAO.SelectAllTblPurchaseVehLinkSauda();
        }
        public List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 rootScheduleId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            if (conn != null && tran != null)
                return _iTblPurchaseVehLinkSaudaDAO.SelectTblPurchaseVehLinkSauda(rootScheduleId, conn, tran);
            else
                return _iTblPurchaseVehLinkSaudaDAO.SelectPurchaseVehLinkSauda(rootScheduleId);
        }
      
        public  TblPurchaseVehLinkSaudaTO SelectTblPurchaseVehLinkSaudaTO(Int32 idVehLinkSauda)
        {
            List<TblPurchaseVehLinkSaudaTO> tblPurchaseVehLinkSaudaTOList  = _iTblPurchaseVehLinkSaudaDAO.SelectTblPurchaseVehLinkSauda(idVehLinkSauda);
            if(tblPurchaseVehLinkSaudaTOList != null && tblPurchaseVehLinkSaudaTOList.Count == 1)
                return tblPurchaseVehLinkSaudaTOList[0];
            else
                return null;
        }

    

        #endregion
        
        #region Insertion
        public   int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO)
        {
            return _iTblPurchaseVehLinkSaudaDAO.InsertTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO);
        }

        public   int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehLinkSaudaDAO.InsertTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public   int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO)
        {
            return _iTblPurchaseVehLinkSaudaDAO.UpdateTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO);
        }

        public   int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehLinkSaudaDAO.UpdateTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public   int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda)
        {
            return _iTblPurchaseVehLinkSaudaDAO.DeleteTblPurchaseVehLinkSauda(idVehLinkSauda);
        }

        public   int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseVehLinkSaudaDAO.DeleteTblPurchaseVehLinkSauda(idVehLinkSauda, conn, tran);
        }

        #endregion
        
        public ResultMessage SavePurchaseVehLinkSaudaDtls(TblPurchaseScheduleSummaryTO scheduleSummaryTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            try
            {
                if(scheduleSummaryTO == null)
                {
                    throw new Exception("scheduleSummaryTO == NULL");
                }

                if(scheduleSummaryTO.PurchaseVehLinkSaudaTOList != null && scheduleSummaryTO.PurchaseVehLinkSaudaTOList.Count > 0)
                {
                    for (int i = 0; i < scheduleSummaryTO.PurchaseVehLinkSaudaTOList.Count; i++)
                    {
                        TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO = scheduleSummaryTO.PurchaseVehLinkSaudaTOList[i];
                        tblPurchaseVehLinkSaudaTO.IsActive = 1;
                        result = InsertTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);
                        if(result != 1)
                        {
                            throw new Exception("Error in InsertTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);");

                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SavePurchaseVehLinkSaudaDtls(TblPurchaseScheduleSummaryTO scheduleSummaryTO)");
                return resultMessage;
            }
        }

        public ResultMessage DeactivatePreviousLinkSauda(TblPurchaseScheduleSummaryTO scheduleSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            try
            {
                if (scheduleSummaryTO == null)
                {
                    throw new Exception("scheduleSummaryTO == NULL");
                }

                List< TblPurchaseVehLinkSaudaTO> purchaseVehLinkSaudaTOList = _iTblPurchaseVehLinkSaudaDAO.SelectTblPurchaseVehLinkSauda(scheduleSummaryTO.ActualRootScheduleId, conn, tran);

                if (purchaseVehLinkSaudaTOList != null && purchaseVehLinkSaudaTOList.Count > 0)
                {
                    for (int i = 0; i < purchaseVehLinkSaudaTOList.Count; i++)
                    {
                        TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO = purchaseVehLinkSaudaTOList[i];
                        tblPurchaseVehLinkSaudaTO.IsActive = 0;
                        result = UpdateTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error in InsertTblPurchaseVehLinkSauda(tblPurchaseVehLinkSaudaTO, conn, tran);");

                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SavePurchaseVehLinkSaudaDtls(TblPurchaseScheduleSummaryTO scheduleSummaryTO)");
                return resultMessage;
            }
        }
    }
}
