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
    public class TblTRSLABL : ITblTRSLABL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblTRSLADAO _iTblTRSLADAO;
        public TblTRSLABL(ITblTRSLADAO iTblTRSLADAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iTblTRSLADAO = iTblTRSLADAO;
        }


        #region Selection
        public DataTable SelectAllTblTRSLA()
        {
            return _iTblTRSLADAO.SelectAllTblTRSLA();
        }

        public List<TblTRSLATO> SelectAllTblTRSLAList()
        {
            DataTable tblTRSLATODT = _iTblTRSLADAO.SelectAllTblTRSLA();
            return ConvertDTToList(tblTRSLATODT);
        }

        public TblTRSLATO SelectTblTRSLATO(Int32 idSLA)
        {
            DataTable tblTRSLATODT = _iTblTRSLADAO.SelectTblTRSLA(idSLA);
            List<TblTRSLATO> tblTRSLATOList = ConvertDTToList(tblTRSLATODT);
            if (tblTRSLATOList != null && tblTRSLATOList.Count == 1)
                return tblTRSLATOList[0];
            else
                return null;
        }

        List<TblTRSLATO> ConvertDTToList(DataTable tblTRSLATODT)
        {
            List<TblTRSLATO> tblTRSLATOList = new List<TblTRSLATO>();
            if (tblTRSLATODT != null)
            {
                for (int rowCount = 0; rowCount < tblTRSLATODT.Rows.Count; rowCount++)
                {
                    TblTRSLATO tblTRSLATONew = new TblTRSLATO();
                    if (tblTRSLATODT.Rows[rowCount]["idSLA"] != DBNull.Value)
                        tblTRSLATONew.IdSLA = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["idSLA"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["transferRequestId"] != DBNull.Value)
                        tblTRSLATONew.TransferRequestId = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["transferRequestId"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["unloadingId"] != DBNull.Value)
                        tblTRSLATONew.UnloadingId = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["unloadingId"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["mixMaterialId"] != DBNull.Value)
                        tblTRSLATONew.MixMaterialId = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["mixMaterialId"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["waste"] != DBNull.Value)
                        tblTRSLATONew.Waste = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["waste"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["offChemistryId"] != DBNull.Value)
                        tblTRSLATONew.OffChemistryId = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["offChemistryId"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["descity"] != DBNull.Value)
                        tblTRSLATONew.Descity = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["descity"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblTRSLATONew.StatusId = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["statusId"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblTRSLATONew.CreatedBy = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblTRSLATONew.UpdatedBy = Convert.ToInt32(tblTRSLATODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblTRSLATONew.CreatedOn = Convert.ToDateTime(tblTRSLATODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblTRSLATONew.UpdatedOn = Convert.ToDateTime(tblTRSLATODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblTRSLATONew.IsActive = Convert.ToBoolean(tblTRSLATODT.Rows[rowCount]["isActive"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["overSizePer"] != DBNull.Value)
                        tblTRSLATONew.OverSizePer = Convert.ToDouble(tblTRSLATODT.Rows[rowCount]["overSizePer"].ToString());
                    if (tblTRSLATODT.Rows[rowCount]["displayNo"] != DBNull.Value)
                        tblTRSLATONew.DisplayNo = Convert.ToString(tblTRSLATODT.Rows[rowCount]["displayNo"].ToString());
                    tblTRSLATOList.Add(tblTRSLATONew);
                }
            }
            return tblTRSLATOList;
        }

        #endregion

        #region Insertion
        public int InsertTblTRSLA(TblTRSLATO tblTRSLATO)
        {
            return _iTblTRSLADAO.InsertTblTRSLA(tblTRSLATO);
        }

        public int InsertTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRSLADAO.InsertTblTRSLA(tblTRSLATO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblTRSLA(TblTRSLATO tblTRSLATO)
        {
            return _iTblTRSLADAO.UpdateTblTRSLA(tblTRSLATO);
        }

        public int UpdateTblTRSLA(TblTRSLATO tblTRSLATO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTRSLADAO.UpdateTblTRSLA(tblTRSLATO, conn, tran);
        }

        #endregion
    }
}
