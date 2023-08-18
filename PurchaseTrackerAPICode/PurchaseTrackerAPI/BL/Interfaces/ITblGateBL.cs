using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblGateBL
    {
        List<TblGateTO> SelectAllTblGateList(StaticStuff.Constants.ActiveSelectionTypeE ActiveSelectionTypeE);
        TblGateTO SelectTblGateTO(Int32 idGate);
        TblGateTO GetDefaultTblGateTO();
        int InsertTblGate(TblGateTO tblGateTO);
        int InsertTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGate(TblGateTO tblGateTO);
        int UpdateTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGate(Int32 idGate);
        int DeleteTblGate(Int32 idGate, SqlConnection conn, SqlTransaction tran);
    }
}
