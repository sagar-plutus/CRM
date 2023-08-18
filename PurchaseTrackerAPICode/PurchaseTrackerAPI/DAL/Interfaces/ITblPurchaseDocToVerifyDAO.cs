using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseDocToVerifyDAO
    {
        String SqlSelectQuery();
        List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify();
        TblPurchaseDocToVerifyTO SelectTblPurchaseDocToVerify(Int32 idPurchaseDocType);
        List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify(SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseDocToVerifyTO> ConvertDTToList(SqlDataReader tblPurchaseDocToVerifyTODT);
        int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO);
        int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO);
        int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType);
        int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchaseDocType, SqlCommand cmdDelete);

    }
}