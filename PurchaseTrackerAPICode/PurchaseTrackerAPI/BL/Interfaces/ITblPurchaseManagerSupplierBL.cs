using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseManagerSupplierBL
    {

        List<DropDownTO> SelectPurchaseFromRoleForDropDown();
        List<DropDownTO> SelectPurchaseFromRoleForDropDown(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetSupplierByPMDropDownList(string userId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId, SqlConnection conn, SqlTransaction tran);
        Int32 GetSupplierStateId(int supplierID);
        List<TblPurchaseManagerSupplierTO> SelectAllActivePurchaseManagerSupplierList(Int32 userId);
        int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO);

    }
}