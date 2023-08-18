using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimStateDAO
    {
        String SqlSelectQuery();
        List<DimStateTO> SelectAllDimState();
        DimStateTO SelectDimState(Int32 idState);
        List<DimStateTO> SelectAllDimState(SqlConnection conn, SqlTransaction tran);
        List<DimStateTO> ConvertDTToList(SqlDataReader statesDT);
        int InsertDimState(DimStateTO dimStateTO);
        int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimStateTO dimStateTO, SqlCommand cmdInsert);
        int UpdateDimState(DimStateTO dimStateTO);
        int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimStateTO dimStateTO, SqlCommand cmdUpdate);
        int DeleteDimState(Int32 idState);
        int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idState, SqlCommand cmdDelete);

    }
}