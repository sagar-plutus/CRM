using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseDocToVerifyBL
    {
        List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerify();
        List<TblPurchaseDocToVerifyTO> SelectAllTblPurchaseDocToVerifyList();
        TblPurchaseDocToVerifyTO SelectTblPurchaseDocToVerifyTO(Int32 idPurchaseDocType);
        int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO);
        int InsertTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO);
        int UpdateTblPurchaseDocToVerify(TblPurchaseDocToVerifyTO tblPurchaseDocToVerifyTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType);
        int DeleteTblPurchaseDocToVerify(Int32 idPurchaseDocType, SqlConnection conn, SqlTransaction tran);

    }
}