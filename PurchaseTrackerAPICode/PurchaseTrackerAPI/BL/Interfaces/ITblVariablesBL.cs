using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblVariablesBL
    {
        List<TblVariablesTO> SelectAllTblVariables(Int32 isActive);
        List<TblVariablesTO> SelectAllTblVariablesList();
        TblVariablesTO SelectTblVariablesTO(Int32 idVariable);
        List<TblVariablesTO> SelectActiveVariablesList(SqlConnection conn, SqlTransaction tran);
        List<TblVariablesTO> SelectAllTblVariables(SqlConnection conn, SqlTransaction tran);
        int InsertTblVariables(TblVariablesTO tblVariablesTO);
        int InsertTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PostVariableDetails(List<TblVariablesTO> tblVariablesTOList, Int32 loginUserId);
        ResultMessage EditVariableDetails(TblVariablesTO tblVariablesTO, Int32 loginUserId);
        
        int UpdateTblVariables(TblVariablesTO tblVariablesTO);
        int UpdateTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVariables(Int32 idVariable);
        int DeleteTblVariables(Int32 idVariable, SqlConnection conn, SqlTransaction tran);
        List<TblVariablesTO> GetHistoryOfVariablesbyUniqueNo(int uniqueTrackId);
        List<DropDownTO> SelectVariableList(int isProcessVar);
        double GetProcessVariableValue(TblPurchaseVehicleDetailsTO tblPurchaseVehicleDetailsLocalTO, List<TblVariablesTO> variableTOList);

        List<TblVariablesTO> SelectVariableCodeDtls(String variableCode, DateTime fromDate, DateTime toDate);
    }
}