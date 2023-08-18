using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblRecyclePreferenceBL : ITblRecyclePreferenceBL
    {
        private readonly ITblRecyclePreferenceDAO _iTblRecyclePreferenceDAO;
        public TblRecyclePreferenceBL(ITblRecyclePreferenceDAO iTblRecyclePreferenceDAO
            )
        {
            _iTblRecyclePreferenceDAO = iTblRecyclePreferenceDAO;
        }
        #region Selection
        public  List<TblRecyclePreferenceTO> SelectAllTblRecyclePreference()
        {
            return _iTblRecyclePreferenceDAO.SelectAllTblRecyclePreference();
        }

        public  List<TblRecyclePreferenceTO> SelectAllTblRecyclePreferenceList()
        {
            return _iTblRecyclePreferenceDAO.SelectAllTblRecyclePreference();
            //return ConvertDTToList(tblRecyclePreferenceTODT);
        }

        public  TblRecyclePreferenceTO SelectTblRecyclePreferenceTO(Int32 idPreference)
        {
            return _iTblRecyclePreferenceDAO.SelectTblRecyclePreference(idPreference);
            // List<TblRecyclePreferenceTO> tblRecyclePreferenceTOList = ConvertDTToList(tblRecyclePreferenceTODT);
            // if(tblRecyclePreferenceTOList != null && tblRecyclePreferenceTOList.Count == 1)
            //     return tblRecyclePreferenceTOList[0];
            // else
            //     return null;
        }

         public  TblRecyclePreferenceTO SelectTblRecyclePreferenceValByName(string settingKey)
        {
            return _iTblRecyclePreferenceDAO.SelectTblRecyclePreferenceValByName(settingKey);
        }

        #endregion
        
        #region Insertion
        public  int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO)
        {
            return _iTblRecyclePreferenceDAO.InsertTblRecyclePreference(tblRecyclePreferenceTO);
        }

        public  int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecyclePreferenceDAO.InsertTblRecyclePreference(tblRecyclePreferenceTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO)
        {
            return _iTblRecyclePreferenceDAO.UpdateTblRecyclePreference(tblRecyclePreferenceTO);
        }

        public  int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecyclePreferenceDAO.UpdateTblRecyclePreference(tblRecyclePreferenceTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblRecyclePreference(Int32 idPreference)
        {
            return _iTblRecyclePreferenceDAO.DeleteTblRecyclePreference(idPreference);
        }

        public  int DeleteTblRecyclePreference(Int32 idPreference, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRecyclePreferenceDAO.DeleteTblRecyclePreference(idPreference, conn, tran);
        }

        #endregion
        
    }
}
