using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblCnfDealersDAO
    {
        String SqlSelectQuery();
        List<TblCnfDealersTO> SelectAllTblCnfDealers();
        TblCnfDealersTO SelectTblCnfDealers(Int32 idCnfDealerId);
        TblCnfDealersTO SelectTblCnfDealers(Int32 cnfOrgId, Int32 dealerOrgId);
        List<TblCnfDealersTO> SelectAllTblCnfDealers(Int32 dealerId, Boolean isSpecialOnly, SqlConnection conn, SqlTransaction tran);
        List<TblCnfDealersTO> ConvertDTToList(SqlDataReader tblCnfDealersTODT);
        int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO);
        int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblCnfDealersTO tblCnfDealersTO, SqlCommand cmdInsert);
        int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO);
        int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblCnfDealersTO tblCnfDealersTO, SqlCommand cmdUpdate);
        int DeleteTblCnfDealers(Int32 idCnfDealerId);
        int DeleteTblCnfDealers(Int32 idCnfDealerId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idCnfDealerId, SqlCommand cmdDelete);

    }
}