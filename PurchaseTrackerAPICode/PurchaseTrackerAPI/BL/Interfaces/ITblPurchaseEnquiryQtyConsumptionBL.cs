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
    public interface ITblPurchaseEnquiryQtyConsumptionBL
    {

        List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption();
        List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumptionList();
        TblPurchaseEnquiryQtyConsumptionTO SelectTblPurchaseEnquiryQtyConsumptionTO(Int32 idPurEnqQtyCons);
        int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO);
        int InsertTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO);
        int UpdateTblPurchaseEnquiryQtyConsumption(TblPurchaseEnquiryQtyConsumptionTO tblPurchaseEnquiryQtyConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons);
        int DeleteTblPurchaseEnquiryQtyConsumption(Int32 idPurEnqQtyCons, SqlConnection conn, SqlTransaction tran);

        ResultMessage SaveConsumptionQtyAgainstBooking(TblPurchaseEnquiryTO enquiryTO,double consumptionQty,Int32 isAuto,Int32 loginUserId,SqlConnection conn,SqlTransaction tran);
        List<TblPurchaseEnquiryQtyConsumptionTO> SelectAllTblPurchaseEnquiryQtyConsumption(DateTime serverDate);
    }
}