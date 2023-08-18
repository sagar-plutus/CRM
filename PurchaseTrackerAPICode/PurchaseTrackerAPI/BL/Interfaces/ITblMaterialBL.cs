using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblMaterialBL
    {
        List<TblMaterialTO> SelectAllTblMaterialList();
        List<DropDownTO> SelectAllMaterialListForDropDown();
        dynamic GetDynamicObject(Dictionary<string, object> properties);
        TblMaterialTO SelectTblMaterialTO(Int32 idMaterial);
        List<DropDownTO> SelectMaterialTypeDropDownList();
        int InsertTblMaterial(TblMaterialTO tblMaterialTO);
        int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblMaterial(TblMaterialTO tblMaterialTO);
        int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblMaterial(Int32 idMaterial);
        int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran);

    }
}