using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblGateDAO
    {
        List<TblGateTO> SelectAllTblGate(StaticStuff.Constants.ActiveSelectionTypeE ActiveSelectionTypeE);
        TblGateTO SelectTblGate(Int32 idGate);
        List<TblGateTO> SelectAllTblGate(StaticStuff.Constants.ActiveSelectionTypeE ActiveSelectionTypeE, SqlConnection conn, SqlTransaction tran);
        int InsertTblGate(TblGateTO tblGateTO);
        int InsertTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblGate(TblGateTO tblGateTO);
        int UpdateTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGate(Int32 idGate);
        int DeleteTblGate(Int32 idGate, SqlConnection conn, SqlTransaction tran);
    }
}
