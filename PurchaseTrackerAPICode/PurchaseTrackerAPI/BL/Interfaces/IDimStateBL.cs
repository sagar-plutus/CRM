using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimStateBL
    {
        List<DimStateTO> SelectAllDimState();
        DimStateTO SelectDimStateTO(Int32 idState);
        List<DimStateTO> ConvertDTToList(DataTable dimStateTODT);
        int InsertDimState(DimStateTO dimStateTO);
        int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewState(DimStateTO dimStateTO);
        ResultMessage UpdateState(DimStateTO dimStateTO);
        int UpdateDimState(DimStateTO dimStateTO);
        int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimState(Int32 idState);
        int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran);

    }
}