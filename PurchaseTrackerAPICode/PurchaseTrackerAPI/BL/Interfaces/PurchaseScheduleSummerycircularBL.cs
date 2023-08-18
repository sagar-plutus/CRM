using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IPurchaseScheduleSummerycircularBL
    {
        ResultMessage checkIfQtyGoesOutofBand(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew, TblPurchaseEnquiryTO enquiryTO, SqlConnection conn, SqlTransaction tran);

    }
}
