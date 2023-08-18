using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblEnquiryDtlDAO
    {
        String SqlSelectQuery();
        List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl();
        List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl(String dealerIds);
        List<TblEnquiryDtlTO> SelectEnquiryDtlList(Int32 organizationId);
        TblEnquiryDtlTO SelectTblEnquiryDtl(Int32 idEnquiryDtl);
        List<TblEnquiryDtlTO> ConvertDTToList(SqlDataReader tblEnquiryDtlTODT);
        TblEnquiryDtlTO SelectOrganizationEnquiryDtl(string enqRefId);
        int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO);
        int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateOrgEnqDeatls(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEnquiryDtlTO tblEnquiryDtlTO, SqlCommand cmdInsert);
        int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO);
        int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblEnquiryDtlTO tblEnquiryDtlTO, SqlCommand cmdUpdate);
        int DeleteTblEnquiryDtl(Int32 idEnquiryDtl);
        int DeleteTblEnquiryDtl(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idEnquiryDtl, SqlCommand cmdDelete);
        int DeleteTblEnquiryDtl(SqlConnection conn, SqlTransaction tran);

    }
}