using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.DashboardModels;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseCompetitorExtBL : ITblPurchaseCompetitorExtBL
    {

        private readonly ITblPurchaseCompetitorExtDAO _iTblPurchaseCompetitorExtDAO;
        public TblPurchaseCompetitorExtBL(ITblPurchaseCompetitorExtDAO iTblPurchaseCompetitorExtDAO)
        {
            _iTblPurchaseCompetitorExtDAO = iTblPurchaseCompetitorExtDAO;
        }

        #region Selection
        public  List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExt()
        {
            return _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt();
        }

        public  List<TblPurchaseCompetitorExtTO> SelectAllTblPurchaseCompetitorExtList()
        {
            return _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt();
            //return ConvertDTToList(tblPurchaseCompetitorExtTODT);
        }
          public  List<TblPurchaseCompetitorExtTO> GetMaterialListByCompetitorId(Int32 competitorId)
        {
            return _iTblPurchaseCompetitorExtDAO.GetMaterialListByCompetitorId(competitorId);
            //return ConvertDTToList(tblPurchaseCompetitorExtTODT);
        }

        public  TblPurchaseCompetitorExtTO SelectTblPurchaseCompetitorExtTO(Int32 idPurCompetitorExt)
        {
            List<TblPurchaseCompetitorExtTO> tblPurchaseCompetitorExtTOList = _iTblPurchaseCompetitorExtDAO.SelectTblPurchaseCompetitorExt(idPurCompetitorExt);
            //List<TblPurchaseCompetitorExtTO> tblPurchaseCompetitorExtTOList = ConvertDTToList(tblPurchaseCompetitorExtTODT);
            if(tblPurchaseCompetitorExtTOList != null && tblPurchaseCompetitorExtTOList.Count == 1)
                return tblPurchaseCompetitorExtTOList[0];
            else
                return null;
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public  int InsertTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO);
        }

        public  int UpdateTblPurchaseCompetitorExt(TblPurchaseCompetitorExtTO tblPurchaseCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblPurchaseCompetitorExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt);
        }

        public  int DeleteTblPurchaseCompetitorExt(Int32 idPurCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseCompetitorExtDAO.DeleteTblPurchaseCompetitorExt(idPurCompetitorExt, conn, tran);
        }

        #endregion
        
    }
}
