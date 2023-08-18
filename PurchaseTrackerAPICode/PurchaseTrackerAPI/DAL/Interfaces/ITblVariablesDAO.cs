using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblVariablesDAO
    {
        String SqlSelectQuery();
        List<TblVariablesTO> SelectAllTblVariables(Int32 isActive);
        List<TblVariablesTO> SelectAllTblVariables();
        List<TblVariablesTO> SelectTblVariables(Int32 idVariable);
        List<TblVariablesTO> SelectAllTblVariables(SqlConnection conn, SqlTransaction tran);
        List<TblVariablesTO> SelectActiveVariablesList(SqlConnection conn, SqlTransaction tran);
        List<TblVariablesTO> ConvertDTToList(SqlDataReader tblVariablesTODT);
        int InsertTblVariables(TblVariablesTO tblVariablesTO);
        int InsertTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);

        int InsertTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblVariablesTO tblVariablesTO, SqlCommand cmdInsert);
        int UpdateTblVariables(TblVariablesTO tblVariablesTO);
        int UpdateTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVariablesTO tblVariablesTO, SqlCommand cmdUpdate);
        int DeleteTblVariables(Int32 idVariable);
        int DeleteTblVariables(Int32 idVariable, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVariable, SqlCommand cmdDelete);
        List<TblVariablesTO> GetHistoryOfVariablesbyUniqueNo(int uniqueTrackId);
        int UpdateTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);

        List<DropDownTO> SelectVariableList(int isProcessVar);

        List<TblVariablesTO>  SelectVariableCodeDtls(String variableCode, DateTime fromDate, DateTime toDate);
    }
}