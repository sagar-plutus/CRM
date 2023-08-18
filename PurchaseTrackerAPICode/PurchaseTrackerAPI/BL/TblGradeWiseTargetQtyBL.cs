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

namespace PurchaseTrackerAPI.BL
{
    public class TblGradeWiseTargetQtyBL : ITblGradeWiseTargetQtyBL

    {
        private readonly ITblGradeWiseTargetQtyDAO _iTblGradeWiseTargetQtyDAO;
        public TblGradeWiseTargetQtyBL(ITblGradeWiseTargetQtyDAO iTblGradeWiseTargetQtyDAO)
        {
            _iTblGradeWiseTargetQtyDAO = iTblGradeWiseTargetQtyDAO;
        }


        #region Selection
        public  List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty()
        {
            return _iTblGradeWiseTargetQtyDAO.SelectAllTblGradeWiseTargetQty();
        }

        public  List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQtyList()
        {
             return _iTblGradeWiseTargetQtyDAO.SelectAllTblGradeWiseTargetQty();
        }

        public  TblGradeWiseTargetQtyTO SelectTblGradeWiseTargetQtyTO(Int32 idGradeWiseTargetQty)
        {
            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList= _iTblGradeWiseTargetQtyDAO.SelectTblGradeWiseTargetQty(idGradeWiseTargetQty);
            if(tblGradeWiseTargetQtyTOList != null && tblGradeWiseTargetQtyTOList.Count == 1)
                return tblGradeWiseTargetQtyTOList[0];
            else
                return null;
        }

       public  List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId,Int32 pmId)
        {
            return  _iTblGradeWiseTargetQtyDAO.SelectGradeWiseTargetQtyDtls(rateBandPurchaseId,pmId);
          
        }
         public  List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId,Int32 pmId,SqlConnection conn,SqlTransaction tran)
        {
            return  _iTblGradeWiseTargetQtyDAO.SelectGradeWiseTargetQtyDtls(rateBandPurchaseId,pmId,conn,tran);
          
        }

        #endregion
        
        #region Insertion
        public  int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO)
        {
            return _iTblGradeWiseTargetQtyDAO.InsertTblGradeWiseTargetQty(tblGradeWiseTargetQtyTO);
        }

        public  int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGradeWiseTargetQtyDAO.InsertTblGradeWiseTargetQty(tblGradeWiseTargetQtyTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO)
        {
            return _iTblGradeWiseTargetQtyDAO.UpdateTblGradeWiseTargetQty(tblGradeWiseTargetQtyTO);
        }

        public  int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGradeWiseTargetQtyDAO.UpdateTblGradeWiseTargetQty(tblGradeWiseTargetQtyTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty)
        {
            return _iTblGradeWiseTargetQtyDAO.DeleteTblGradeWiseTargetQty(idGradeWiseTargetQty);
        }

        public  int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGradeWiseTargetQtyDAO.DeleteTblGradeWiseTargetQty(idGradeWiseTargetQty, conn, tran);
        }

        #endregion
        
    }
}
