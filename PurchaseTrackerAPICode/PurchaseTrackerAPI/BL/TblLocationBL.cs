using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblLocationBL : ITblLocationBL
    {
        private readonly ITblLocationDAO _iTblLocationDAO;
        public TblLocationBL(ITblLocationDAO iTblLocationDAO)
        {
            _iTblLocationDAO = iTblLocationDAO;
        }
        #region Selection
        // public  DataTable SelectAllTblLocation()
        // {
        //     return TblLocationDAO.SelectAllTblLocation();
        // }

        public  List<TblLocationTO> SelectAllTblLocationList()
        {
            return _iTblLocationDAO.SelectAllTblLocation();
            
        }

        public  TblLocationTO SelectTblLocationTO(Int32 idLocation)
        {
             List<TblLocationTO> tblLocationTOList = _iTblLocationDAO.SelectTblLocation(idLocation);
            if(tblLocationTOList != null && tblLocationTOList.Count == 1)
                return tblLocationTOList[0];
            else
                return null;
        }

       

        #endregion
        
        #region Insertion
        public  int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO);
        }

        public  int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO);
        }

        public  int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblLocation(Int32 idLocation)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation);
        }

        public  int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation, conn, tran);
        }

        #endregion
        
    }
}
