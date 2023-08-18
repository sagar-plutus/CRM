using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRecyclePreferenceBL
    {
        List<TblRecyclePreferenceTO> SelectAllTblRecyclePreference();
        List<TblRecyclePreferenceTO> SelectAllTblRecyclePreferenceList();
        TblRecyclePreferenceTO SelectTblRecyclePreferenceTO(Int32 idPreference);
        TblRecyclePreferenceTO SelectTblRecyclePreferenceValByName(string settingKey);
        int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO);
        int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO);
        int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblRecyclePreference(Int32 idPreference);
        int DeleteTblRecyclePreference(Int32 idPreference, SqlConnection conn, SqlTransaction tran);

    }
}