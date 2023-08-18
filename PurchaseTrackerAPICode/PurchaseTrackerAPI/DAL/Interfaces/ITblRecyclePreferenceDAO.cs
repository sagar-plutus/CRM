using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblRecyclePreferenceDAO
    {
        String SqlSelectQuery();
        List<TblRecyclePreferenceTO> SelectAllTblRecyclePreference();
        TblRecyclePreferenceTO SelectTblRecyclePreference(Int32 idPreference);
        TblRecyclePreferenceTO SelectTblRecyclePreferenceValByName(string settingKey);
        TblRecyclePreferenceTO SelectAllTblRecyclePreference(SqlConnection conn, SqlTransaction tran);
        List<TblRecyclePreferenceTO> ConvertDTToList(SqlDataReader tblRecyclePreferenceTODT);
        int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO);
        int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlCommand cmdInsert);
        int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO);
        int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlCommand cmdUpdate);
        int DeleteTblRecyclePreference(Int32 idPreference);
        int DeleteTblRecyclePreference(Int32 idPreference, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPreference, SqlCommand cmdDelete);

    }
}