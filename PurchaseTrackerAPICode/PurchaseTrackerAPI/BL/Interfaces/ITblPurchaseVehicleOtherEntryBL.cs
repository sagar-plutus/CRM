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
    public interface ITblPurchaseVehicleOtherEntryBL
    {
        List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry();
        List<VehicleNumber> SelectAllVehicles();
        TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTO(Int32 idVehicleSpotEntry);
        ResultMessage SaveVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleSpotEntryTO);
        int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO);
        int InsertTblPurchaseVehicleOtherEntry(TblPurchaseVehicleOthertEntryTO tblPurchaseVehicleOtherEntryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO);
        int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry);
        int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran);

    }
}