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
    public interface ITblEnquiryDtlBL
    {
        List<TblEnquiryDtlTO> SelectAllTblEnquiryDtlList();
        List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl(String dealerIds);
        List<TblEnquiryDtlTO> SelectEnquiryDtlList(Int32 dealerId);
        TblEnquiryDtlTO SelectTblEnquiryDtl(Int32 idEnquiryDtl);
        int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO);
        int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveOrgEnquiryDtl(List<TblEnquiryDtlTO> tblEnquiryDtlTOList, Int32 loginUserId);
        int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO);
        int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEnquiryDtl(Int32 idEnquiryDtl);
        int DeleteTblEnquiryDtl(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEnquiryDtl(SqlConnection conn, SqlTransaction tran);

    }
}