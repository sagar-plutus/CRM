using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblOrgAddressDAO
    {
        String SqlSelectQuery();
        List<TblOrgAddressTO> SelectAllTblOrgAddress();
        TblOrgAddressTO SelectTblOrgAddress(Int32 idOrgAddr);
        List<TblOrgAddressTO> ConvertDTToList(SqlDataReader tblOrgAddressTODT);
        int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO);
        int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgAddressTO tblOrgAddressTO, SqlCommand cmdInsert);
        int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO);
        int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgAddressTO tblOrgAddressTO, SqlCommand cmdUpdate);
        int DeleteTblOrgAddress(Int32 idOrgAddr);
        int DeleteTblOrgAddress(Int32 idOrgAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgAddr, SqlCommand cmdDelete);

    }
}