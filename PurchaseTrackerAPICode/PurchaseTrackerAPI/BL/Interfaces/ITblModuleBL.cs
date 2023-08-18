using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblModuleBL
    {
        TblModuleTO SelectTblModuleTO(Int32 idModule);
        List<DropDownTO> SelectAllTblModuleList();
        List<TblModuleTO> SelectTblModuleList();
        TblModuleTO SelectTblModuleTO(Int32 idModule, SqlConnection conn, SqlTransaction tran);
        int InsertTblModule(TblModuleTO tblModuleTO);
        int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblModule(TblModuleTO tblModuleTO);
        int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblModule(Int32 idModule);
        int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran);

    }
}