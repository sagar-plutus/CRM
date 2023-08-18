using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblMenuStructureDAO
    {
        String SqlSelectQuery();
        List<TblMenuStructureTO> SelectAllTblMenuStructure();
        TblMenuStructureTO SelectTblMenuStructure(Int32 idMenu);
        List<TblMenuStructureTO> ConvertDTToList(SqlDataReader tblMenuStructureTODT);
        int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO);
        int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran);
          int ExecuteInsertionCommand(TblMenuStructureTO tblMenuStructureTO, SqlCommand cmdInsert);
        int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO);
        int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblMenuStructureTO tblMenuStructureTO, SqlCommand cmdUpdate);
        int DeleteTblMenuStructure(Int32 idMenu);
        int DeleteTblMenuStructure(Int32 idMenu, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idMenu, SqlCommand cmdDelete);

    }
}