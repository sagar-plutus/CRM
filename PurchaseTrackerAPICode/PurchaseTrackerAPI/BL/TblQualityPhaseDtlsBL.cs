using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblQualityPhaseDtlsBL : ITblQualityPhaseDtlsBL
    {
        private readonly ITblQualityPhaseDtlsDAO _iTblQualityPhaseDtlsDAO;
        public TblQualityPhaseDtlsBL(ITblQualityPhaseDtlsDAO iTblQualityPhaseDtlsDAO)
        {
            _iTblQualityPhaseDtlsDAO = iTblQualityPhaseDtlsDAO;
        }
        #region Selection
        public List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls()
        {
            return _iTblQualityPhaseDtlsDAO.SelectAllTblQualityPhaseDtls();
        }

        public  List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtlsList()
        {
            return _iTblQualityPhaseDtlsDAO.SelectAllTblQualityPhaseDtls();
            //return ConvertDTToList(tblQualityPhaseDtlsTODT);
        }
         public  List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtlsList(Int32 tblQualityPhaseId)
        {
            return _iTblQualityPhaseDtlsDAO.SelectAllTblQualityPhaseDtls(tblQualityPhaseId);
            //return ConvertDTToList(tblQualityPhaseDtlsTODT);
        }

        public  TblQualityPhaseDtlsTO SelectTblQualityPhaseDtlsTO(Int32 idTblQualityPhaseDtls)
        {
             List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList  = _iTblQualityPhaseDtlsDAO.SelectTblQualityPhaseDtls(idTblQualityPhaseDtls);
           // List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList = ConvertDTToList(tblQualityPhaseDtlsTODT);
            if(tblQualityPhaseDtlsTOList != null && tblQualityPhaseDtlsTOList.Count == 1)
                return tblQualityPhaseDtlsTOList[0];
            else
                return null;
        }

        

        #endregion
        
        #region Insertion
        public  int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO)
        {
            return _iTblQualityPhaseDtlsDAO.InsertTblQualityPhaseDtls(tblQualityPhaseDtlsTO);
        }

        public  int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.InsertTblQualityPhaseDtls(tblQualityPhaseDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO)
        {
            return _iTblQualityPhaseDtlsDAO.UpdateTblQualityPhaseDtls(tblQualityPhaseDtlsTO);
        }

        public  int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.UpdateTblQualityPhaseDtls(tblQualityPhaseDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls)
        {
            return _iTblQualityPhaseDtlsDAO.DeleteTblQualityPhaseDtls(idTblQualityPhaseDtls);
        }

        public  int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.DeleteTblQualityPhaseDtls(idTblQualityPhaseDtls, conn, tran);
        }
        
        public int DeleteAllQualityPhaseDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQualityPhaseDtlsDAO.DeleteAllQualityPhaseDtlsAgainstVehSchedule(purchaseScheduleId, conn, tran);
        }

        #endregion
        
    }
}
