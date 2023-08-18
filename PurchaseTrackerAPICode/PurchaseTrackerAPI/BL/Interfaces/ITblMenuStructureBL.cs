using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblMenuStructureBL
    {
        List<TblMenuStructureTO> SelectAllTblMenuStructureList();
        TblMenuStructureTO SelectTblMenuStructureTO(Int32 idMenu);
        int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO);
        int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO);
        int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblMenuStructure(Int32 idMenu);
        int DeleteTblMenuStructure(Int32 idMenu, SqlConnection conn, SqlTransaction tran);

    }
}