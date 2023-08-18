using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseEnquiryQtyConsumptionDAO
    {

         
        String SqlSelectQuery();
        List<TblPurchaseEnquiryQtyConsumptionTO>  SelectAllTblPurchaseEnquiryQtyConsumption();
        List<TblPurchaseEnquiryQtyConsumptionTO> SelectTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons);
        List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO);
        int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlCommand cmdInsert);
        List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(DateTime serverDate);
        int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO);
        int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlCommand cmdUpdate);
        int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons);
        int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurEnqQtyCons, SqlCommand cmdDelete);
        
    }
}