using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseVehicleOtherEntryDAO
    {
        String SqlSelectQuery();
        int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO);
        int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO, SqlCommand cmdInsert);

    }
}