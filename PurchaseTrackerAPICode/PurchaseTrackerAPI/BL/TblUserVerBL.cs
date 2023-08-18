using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblUserVerBL : ITblUserVerBL
    {
        private readonly ITblUserVerDAO _iTblUserVerDAO;
        public TblUserVerBL(ITblUserVerDAO iTblUserVerDAO)
        {
            _iTblUserVerDAO = iTblUserVerDAO;
        }
        #region Selection
        public  List<TblUserVerTO> SelectAllTblUserVer()
        {
            return _iTblUserVerDAO.SelectAllTblUserVer();
        }

        public  List<TblUserVerTO> SelectAllTblUserVerList()
        {
            List<TblUserVerTO> tblUserVerTODT = _iTblUserVerDAO.SelectAllTblUserVer();
            //return ConvertDTToList(tblUserVerTODT);
            return tblUserVerTODT;
        }

        public  TblUserVerTO SelectTblUserVerTO(Int32 idUserVer)
        {
            List<TblUserVerTO> tblUserVerTOList = _iTblUserVerDAO.SelectTblUserVer(idUserVer);
           // List<TblUserVerTO> tblUserVerTOList = ConvertDTToList(tblUserVerTODT);
            if(tblUserVerTOList != null && tblUserVerTOList.Count == 1)
                return tblUserVerTOList[0];
            else
                return null;
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblUserVer(TblUserVerTO tblUserVerTO)
        {
            return _iTblUserVerDAO.InsertTblUserVer(tblUserVerTO);
        }

        public  int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserVerDAO.InsertTblUserVer(tblUserVerTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblUserVer(TblUserVerTO tblUserVerTO)
        {
            return _iTblUserVerDAO.UpdateTblUserVer(tblUserVerTO);
        }

        public  int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserVerDAO.UpdateTblUserVer(tblUserVerTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblUserVer(Int32 idUserVer)
        {
            return _iTblUserVerDAO.DeleteTblUserVer(idUserVer);
        }

        public  int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserVerDAO.DeleteTblUserVer(idUserVer, conn, tran);
        }

        #endregion
        
    }
}
