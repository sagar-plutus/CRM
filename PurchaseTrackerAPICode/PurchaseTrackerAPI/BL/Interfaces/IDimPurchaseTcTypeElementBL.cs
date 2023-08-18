using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface IDimPurchaseTcTypeElementBL
    {
        List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement();
        List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElementList();
        DimPurchaseTcTypeElementTO SelectDimPurchaseTcTypeElementTO(Int32 idPurchasseTcTypeElement);
        int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO);
        int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO);
        int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement);
        int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement, SqlConnection conn, SqlTransaction tran);

        

    }
}
