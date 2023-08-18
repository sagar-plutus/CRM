using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Linq;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseItemDescBL : ITblPurchaseItemDescBL
    {

        private readonly ITblProdItemDescDAO _iTblProdItemDescDAO;
        private readonly ITblPurchaseItemDescDAO _iTblPurchaseItemDescDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        public TblPurchaseItemDescBL(ITblPurchaseItemDescDAO iTblPurchaseItemDescDAO, Icommondao iCommonDAO, ITblProdItemDescDAO iTblProdItemDescDAO, IConnectionString iConnectionString)
        {
            _iTblProdItemDescDAO = iTblProdItemDescDAO;
            _iTblPurchaseItemDescDAO = iTblPurchaseItemDescDAO;
            _iConnectionString = iConnectionString;
            _iCommonDAO = iCommonDAO;
        }
        #region Selection
        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDesc()
        {
            return _iTblPurchaseItemDescDAO.SelectAllTblPurchaseItemDesc();
        }

        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDescList()
        {
            return _iTblPurchaseItemDescDAO.SelectAllTblPurchaseItemDesc();

        }

        public List<TblPurchaseItemDescTO> SelectAllTblPurchaseItemDescList(int rootScheduleId, int itemId, int stageId, int phaseId)
        {
            try
            {
                List<TblPurchaseItemDescTO> TblPurchaseItemDescTOListReturn = new List<TblPurchaseItemDescTO>();
                List<TblProdItemDescTO> TblProdItemDescTOList = _iTblProdItemDescDAO.SelectAllTblProdItemDesc(itemId);
                List<TblPurchaseItemDescTO> TblPurchaseItemDescTOList = _iTblPurchaseItemDescDAO.SelectAllTblPurchaseItemDesc(rootScheduleId, itemId, stageId);
                if (TblProdItemDescTOList != null && TblProdItemDescTOList.Count > 0)
                {

                    foreach (var item in TblProdItemDescTOList)
                    {
                        if (TblPurchaseItemDescTOList != null && TblPurchaseItemDescTOList.Count > 0)
                        {
                            TblPurchaseItemDescTO TblPurchaseItemDescToGradding = TblPurchaseItemDescTOList.Where(w => w.ProdItemDescId == item.IdProdItemDesc && w.PhaseId == phaseId).FirstOrDefault();
                            if (TblPurchaseItemDescToGradding != null)
                            {
                                TblPurchaseItemDescToGradding.IsSelected = true;
                                TblPurchaseItemDescTOListReturn.Add(TblPurchaseItemDescToGradding);
                            }
                            else
                            {
                                TblPurchaseItemDescTO TblPurchaseItemDescToTemp = new TblPurchaseItemDescTO();
                                TblPurchaseItemDescToTemp.ProdItemDescId = item.IdProdItemDesc;
                                TblPurchaseItemDescToTemp.Name = item.Name;
                                TblPurchaseItemDescToTemp.Description = item.Description;
                                TblPurchaseItemDescTOListReturn.Add(TblPurchaseItemDescToTemp);
                            }
                        }
                        else
                        {
                            TblPurchaseItemDescTO TblPurchaseItemDescToTemp = new TblPurchaseItemDescTO();
                            TblPurchaseItemDescToTemp.ProdItemDescId = item.IdProdItemDesc;
                            TblPurchaseItemDescToTemp.Name = item.Name;
                            TblPurchaseItemDescToTemp.Description = item.Description;

                            TblPurchaseItemDescTOListReturn.Add(TblPurchaseItemDescToTemp);
                        }
                    }

                }

                else
                {
                    TblPurchaseItemDescTOListReturn = TblPurchaseItemDescTOList.Where(w => w.PhaseId == phaseId).ToList();
                    if (TblPurchaseItemDescTOListReturn != null)
                    {
                        foreach (var item in TblPurchaseItemDescTOListReturn)
                        {
                            item.IsSelected = true;
                        }
                    }
                }

                return TblPurchaseItemDescTOListReturn;
            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public List<TblPurchaseItemDescTO> GetAllDescriptionListForCorrection(int rootScheduleId, int itemId, int phaseId)
        {
            try
            {
                return _iTblPurchaseItemDescDAO.GetAllDescriptionListForCorrection(rootScheduleId, itemId, phaseId);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public TblPurchaseItemDescTO SelectTblPurchaseItemDescTO(Int32 idPurchaseItemDesc)
        {
            return _iTblPurchaseItemDescDAO.SelectTblPurchaseItemDesc(idPurchaseItemDesc);
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO)
        {
            return _iTblPurchaseItemDescDAO.InsertTblPurchaseItemDesc(tblPurchaseItemDescTO);
        }

        public ResultMessage InsertTblPurchaseItemDesc(List<TblPurchaseItemDescTO> purchaseProdItemDescTOList, int loginUserId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            DateTime serverDateTime = _iCommonDAO.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int rootScheduleId = purchaseProdItemDescTOList[0].RootScheduleId;
                int phaseId = purchaseProdItemDescTOList[0].PhaseId;
                int itemId = purchaseProdItemDescTOList[0].ProdItemId;
                int stageId = purchaseProdItemDescTOList[0].StageId;
                int result = 1;
                List<TblPurchaseItemDescTO> purchaseProdItemDescTOListTemp = _iTblPurchaseItemDescDAO.SelectAllTblPurchaseItemDesc(conn, tran, rootScheduleId, itemId, stageId);
                if (purchaseProdItemDescTOListTemp != null && purchaseProdItemDescTOListTemp.Count > 0)
                {
                    List<TblPurchaseItemDescTO> ItemDescTOListTemp = purchaseProdItemDescTOListTemp.Where(w => w.PhaseId == phaseId).ToList();
                    if (ItemDescTOListTemp != null && ItemDescTOListTemp.Count > 0)
                    {
                        ItemDescTOListTemp[0].UpdatedBy = loginUserId;
                        ItemDescTOListTemp[0].UpdatedOn = serverDateTime;
                        ItemDescTOListTemp[0].IsActive = 0;
                        ItemDescTOListTemp[0].StageId = purchaseProdItemDescTOList[0].StageId; ;

                        result = _iTblPurchaseItemDescDAO.UpdateAllTblPurchaseItemDescPhaseAndRootWise(ItemDescTOListTemp[0], conn, tran);
                        if (result <= 0)
                        {
                            throw new Exception("Error while UpdateTblPurchaseItemDesc");
                        }
                    }
                }
                if (result > 0)
                {
                    foreach (var item in purchaseProdItemDescTOList)
                    {
                        if (item.IsSelected == true)
                        {
                            item.CreatedBy = loginUserId;
                            item.CreatedOn = serverDateTime;
                            item.IsActive = 1;
                            result = _iTblPurchaseItemDescDAO.InsertTblPurchaseItemDesc(item, conn, tran);
                            if (item.PhaseId == (int)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED)
                            {
                                item.PhaseId = (int)Constants.PurchaseVehiclePhasesE.GRADING;
                                result = _iTblPurchaseItemDescDAO.InsertTblPurchaseItemDesc(item, conn, tran);
                            }
                            if (result <= 0)
                            {
                                throw new Exception("Error while InsertTblPurchaseItemDesc");
                            }
                        }
                    }
                }

                if (result >= 1)
                {
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Submitted Successfully.";
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While  Submitting Description";
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPurchaseItemDesc(List<TblPurchaseItemDescTO> purchaseProdItemDescTOList, int loginUserId)");
                return resultMessage;
            }
            return null;
        }
        public int InsertTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseItemDescDAO.InsertTblPurchaseItemDesc(tblPurchaseItemDescTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO)
        {
            return _iTblPurchaseItemDescDAO.UpdateTblPurchaseItemDesc(tblPurchaseItemDescTO);
        }

        public int UpdateTblPurchaseItemDesc(TblPurchaseItemDescTO tblPurchaseItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseItemDescDAO.UpdateTblPurchaseItemDesc(tblPurchaseItemDescTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc)
        {
            return _iTblPurchaseItemDescDAO.DeleteTblPurchaseItemDesc(idPurchaseItemDesc);
        }

        public int DeleteTblPurchaseItemDesc(Int32 idPurchaseItemDesc, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseItemDescDAO.DeleteTblPurchaseItemDesc(idPurchaseItemDesc, conn, tran);
        }

        public int DeleteAllPurchaseItemDescAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseItemDescDAO.DeleteAllPurchaseItemDescAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }

        #endregion

    }
}
