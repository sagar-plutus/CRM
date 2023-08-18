using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblModuleDAO
    {
        String SqlSelectQuery();
        List<DropDownTO> SelectAllTblModule();
        List<TblModuleTO> SelectTblModuleList();
        TblModuleTO SelectTblModule(Int32 idModule);
        TblModuleTO SelectTblModule(Int32 idModule, SqlConnection conn, SqlTransaction transaction);
        List<TblModuleTO> ConvertDTToList(SqlDataReader tblModuleTODT);
        int InsertTblModule(TblModuleTO tblModuleTO);
        int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblModuleTO tblModuleTO, SqlCommand cmdInsert);
        int UpdateTblModule(TblModuleTO tblModuleTO);
        int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblModuleTO tblModuleTO, SqlCommand cmdUpdate);
        int DeleteTblModule(Int32 idModule);
        int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idModule, SqlCommand cmdDelete);

    }
}