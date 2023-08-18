using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryDetailsDAO
    {
        String SqlSelectQuery();
         int UpdateEnquiryItemPendingQty(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        String SqlSelectQueryNew();
        List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsListByEnquiryId(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId);
        List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseEnquiryDetailsTO> ConvertDTToList(SqlDataReader tblEnquiryDetailsTODT);
        List<TblPurchaseEnquiryDetailsTO> ConvertDTToListNew(SqlDataReader tblEnquiryDetailsTODT);
        List<TblPurchaseEnquiryDetailsTO> SelectAllTblEnquiryDetailsList(Int32 purchaseEnquiryId, Int32 stateId);

        List<TblPurchaseEnquiryDetailsTO> SelectEnquiryDetailsListBySaudaIds(string saudaIds);
        int InsertTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlCommand cmdInsert);
        int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO);
        int UpdateTblPurchaseEnquiryDetails(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseEnquiryDetailsTO tblPurchaseEnquiryDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseEnquiryDetails(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);
          int ExecuteDeletionCommand(Int32 idPurchaseEnquiryDetails, SqlCommand cmdDelete);
          List<TblPurchaseEnquiryDetailsTO> SelectTblEnquiryDetailsList(Int32 purchaseEnquiryId,SqlConnection conn,SqlTransaction tran);
          int DeleteAllGradeDetailsForEnquiry(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran);

    }
}