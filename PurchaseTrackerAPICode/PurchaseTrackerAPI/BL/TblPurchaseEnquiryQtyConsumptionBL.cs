using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseEnquiryQtyConsumptionBL : ITblPurchaseEnquiryQtyConsumptionBL
    {

        private readonly ITblPurchaseEnquiryQtyConsumptionDAO _iTblPurchaseEnquiryQtyConsumptionDAO;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseEnquiryQtyConsumptionBL(ITblPurchaseEnquiryQtyConsumptionDAO iTblPurchaseEnquiryQtyConsumptionDAO, Icommondao iCommonDAO)
        {
            _iTblPurchaseEnquiryQtyConsumptionDAO = iTblPurchaseEnquiryQtyConsumptionDAO;
            _iCommonDAO = iCommonDAO;
        }


        #region Selection

        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(DateTime serverDate)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.SelectAllTblPurchaseEnquiryQtyConsumption(serverDate);

        }
        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption()
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.SelectAllTblPurchaseEnquiryQtyConsumption();
        }

        public List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumptionList()
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.SelectAllTblPurchaseEnquiryQtyConsumption();
        }

        public TblPurchaseEnquiryQtyConsumptionTO SelectTblPurchaseEnquiryQtyConsumptionTO(Int32 idPurEnqQtyCons)
        {
            List<TblPurchaseEnquiryQtyConsumptionTO> tblPurchaseEnquiryQtyConsumptionTOList = _iTblPurchaseEnquiryQtyConsumptionDAO.SelectTblPurchaseEnquiryQtyConsumption(idPurEnqQtyCons);
            if (tblPurchaseEnquiryQtyConsumptionTOList != null && tblPurchaseEnquiryQtyConsumptionTOList.Count == 1)
                return tblPurchaseEnquiryQtyConsumptionTOList[0];
            else
                return null;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.InsertTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO);
        }

        public int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.InsertTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.UpdateTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO);
        }

        public int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.UpdateTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.DeleteTblPurchaseEnquiryQtyConsumption(idPurEnqQtyCons);
        }

        public int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseEnquiryQtyConsumptionDAO.DeleteTblPurchaseEnquiryQtyConsumption(idPurEnqQtyCons, conn, tran);
        }

        #endregion

        public ResultMessage SaveConsumptionQtyAgainstBooking(TblPurchaseEnquiryTO enquiryTO, double consumptionQty, Int32 isAuto, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            DateTime currentDate = _iCommonDAO.ServerDateTime;
            Int32 result = 0;

            try
            {

                if (enquiryTO == null)
                {
                    throw new Exception("enquiryTO == null");
                }

                if (consumptionQty == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO = new TblPurchaseEnquiryQtyConsumptionTO();
                tblPurchaseEnquiryQtyConsumptionTO.PurchaseEnqId = enquiryTO.IdPurchaseEnquiry;

                tblPurchaseEnquiryQtyConsumptionTO.StatusId = enquiryTO.StatusId;
                tblPurchaseEnquiryQtyConsumptionTO.ConsumptionQty = consumptionQty;
                tblPurchaseEnquiryQtyConsumptionTO.CreatedBy = loginUserId;
                tblPurchaseEnquiryQtyConsumptionTO.CreatedOn = currentDate;
                tblPurchaseEnquiryQtyConsumptionTO.IsAuto = isAuto;

                if (isAuto == 1)
                {
                    tblPurchaseEnquiryQtyConsumptionTO.Remark = "Auto completed booking status as pending qty is in tolerance val";
                }
                else
                {
                    tblPurchaseEnquiryQtyConsumptionTO.Remark = enquiryTO.SaudaCloseRemark;
                }

                result = _iTblPurchaseEnquiryQtyConsumptionDAO.InsertTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error in _iTblPurchaseEnquiryQtyConsumptionDAO.InsertTblPurchaseEnquiryQtyConsumption(tblPurchaseEnquiryQtyConsumptionTO,conn,tran);");
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (System.Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveConsumptionQtyAgainstBooking(TblPurchaseEnquiryTO enquiryTO,Boolean isAuto,Int32 loginUserId,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;

            }

        }

    }
}
