using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI
{
    public class TblSpotEntryContainerDtlsBL : ITblSpotEntryContainerDtlsBL
    {
        private readonly ITblSpotEntryContainerDtlsDAO _iTblSpotEntryContainerDtlsDAO;
        public TblSpotEntryContainerDtlsBL(ITblSpotEntryContainerDtlsDAO iTblSpotEntryContainerDtlsDAO)
        {
            _iTblSpotEntryContainerDtlsDAO = iTblSpotEntryContainerDtlsDAO;
        }
        #region Selection
        public List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls()
        {
            return _iTblSpotEntryContainerDtlsDAO.SelectAllTblSpotEntryContainerDtls();
        }

        public  List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtlsList()
        {
            return _iTblSpotEntryContainerDtlsDAO.SelectAllTblSpotEntryContainerDtls();          
        }

        public  TblSpotEntryContainerDtlsTO SelectTblSpotEntryContainerDtlsTO(Int32 idSpotEntryContainerDtls)
        {
            return _iTblSpotEntryContainerDtlsDAO.SelectTblSpotEntryContainerDtls(idSpotEntryContainerDtls);
            
        }

        // Deepali Added for task no 1151
        public List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtlsList(int spotEntryId)
        {
            return _iTblSpotEntryContainerDtlsDAO.SelectAllTblSpotEntryContainerDtls(spotEntryId);
        }


        #endregion

        #region Insertion
        public int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO)
        {
            return _iTblSpotEntryContainerDtlsDAO.InsertTblSpotEntryContainerDtls(tblSpotEntryContainerDtlsTO);
        }

        public  int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotEntryContainerDtlsDAO.InsertTblSpotEntryContainerDtls(tblSpotEntryContainerDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO)
        {
            return _iTblSpotEntryContainerDtlsDAO.UpdateTblSpotEntryContainerDtls(tblSpotEntryContainerDtlsTO);
        }

        public  int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotEntryContainerDtlsDAO.UpdateTblSpotEntryContainerDtls(tblSpotEntryContainerDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls)
        {
            return _iTblSpotEntryContainerDtlsDAO.DeleteTblSpotEntryContainerDtls(idSpotEntryContainerDtls);
        }

        public  int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSpotEntryContainerDtlsDAO.DeleteTblSpotEntryContainerDtls(idSpotEntryContainerDtls, conn, tran);
        }

        #endregion
        
    }
}
