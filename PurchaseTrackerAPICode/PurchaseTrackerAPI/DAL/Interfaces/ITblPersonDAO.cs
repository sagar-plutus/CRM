using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPersonDAO
    {
        String SqlSelectQuery();
        List<TblPersonTO> SelectAllTblPerson();
        List<TblPersonTO> SelectAllTblPersonByOrganization(Int32 organizationId);
        TblPersonTO SelectTblPerson(Int32 idPerson);
        List<TblPersonTO> ConvertDTToList(SqlDataReader tblPersonTODT);
        TblPersonTO SelectAllTblPersonByUser(Int32 userId);
        int InsertTblPerson(TblPersonTO tblPersonTO);
        int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPersonTO tblPersonTO, SqlCommand cmdInsert);
        int UpdateTblPerson(TblPersonTO tblPersonTO);
        int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPersonTO tblPersonTO, SqlCommand cmdUpdate);
        int DeleteTblPerson(Int32 idPerson);
        int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPerson, SqlCommand cmdDelete);

    }
}