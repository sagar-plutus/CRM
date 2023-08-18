using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPartyWeighingMeasuresBL : ITblPartyWeighingMeasuresBL
    {
        private readonly ITblPartyWeighingMeasuresDAO _iTblPartyWeighingMeasuresDAO;
        public TblPartyWeighingMeasuresBL(ITblPartyWeighingMeasuresDAO iTblPartyWeighingMeasuresDAO)
        {
            _iTblPartyWeighingMeasuresDAO = iTblPartyWeighingMeasuresDAO;
        }
        #region Selection
        public  List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures()
        {
            return _iTblPartyWeighingMeasuresDAO.SelectAllTblPartyWeighingMeasures();
        }

        public  List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasuresList()
        {
            return  _iTblPartyWeighingMeasuresDAO.SelectAllTblPartyWeighingMeasures();
        }

        public  TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTO(Int32 idPartyWeighingMeasures)
        {
            return  _iTblPartyWeighingMeasuresDAO.SelectTblPartyWeighingMeasures(idPartyWeighingMeasures);
        }
        //Priyanka [12-02-2019]
        public  TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId,SqlConnection conn = null,SqlTransaction tran = null)
        {
            if(conn != null && tran != null)
                return _iTblPartyWeighingMeasuresDAO.SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(purchaseScheduleSummaryId,conn,tran);
            else
                return _iTblPartyWeighingMeasuresDAO.SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(purchaseScheduleSummaryId);
        }


        #endregion

        #region Insertion
        public  int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO)
        {
            return _iTblPartyWeighingMeasuresDAO.InsertTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO);
        }

        public  int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPartyWeighingMeasuresDAO.InsertTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO)
        {
            return _iTblPartyWeighingMeasuresDAO.UpdateTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO);
        }

        public  int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPartyWeighingMeasuresDAO.UpdateTblPartyWeighingMeasures(tblPartyWeighingMeasuresTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures)
        {
            return _iTblPartyWeighingMeasuresDAO.DeleteTblPartyWeighingMeasures(idPartyWeighingMeasures);
        }

        public  int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPartyWeighingMeasuresDAO.DeleteTblPartyWeighingMeasures(idPartyWeighingMeasures, conn, tran);
        }

        public int DeleteAllPartyWeighingDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPartyWeighingMeasuresDAO.DeleteAllPartyWeighingDtls(purchaseScheduleId, conn, tran);
        }

        #endregion

    }
}
