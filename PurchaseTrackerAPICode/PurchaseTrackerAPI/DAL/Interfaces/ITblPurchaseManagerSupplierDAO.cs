using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPurchaseManagerSupplierDAO
    {
        List<TblPurchaseManagerSupplierTO> SelectAllPurchaseManagerSupplier();
        String SqlSelectQueryToGetSupplier();
        String SqlSelectQueryTogetPMSupplier();
        List<DropDownTO> SelectPurchaseFromRoleForDropDown();
        List<DropDownTO> SelectPurchaseFromRoleForDropDown(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetSupplierByPMDropDownList(string userId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, int> SelectPurchaseManagerWithSupplierDCT(Int32 userId);
        Int32 GetSupplierStateId(Int32 supplierID);
        List<TblPurchaseManagerSupplierTO> ConvertDTToList(SqlDataReader tblPurchaseManagerSupplierTODT);
        int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO);
        int ExecuteInsertionCommand(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO, SqlCommand cmdInsert);

    }
}