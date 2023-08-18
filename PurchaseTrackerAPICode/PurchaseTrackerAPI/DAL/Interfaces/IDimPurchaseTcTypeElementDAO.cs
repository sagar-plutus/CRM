using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimPurchaseTcTypeElementDAO
    {
        String SqlSelectQuery();
        List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement();
        List<DimPurchaseTcTypeElementTO> SelectDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement);
        List<DimPurchaseTcTypeElementTO> SelectAllDimPurchaseTcTypeElement(SqlConnection conn, SqlTransaction tran);
        int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO);
        int InsertDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlCommand cmdInsert);
        int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO);
        int UpdateDimPurchaseTcTypeElement(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimPurchaseTcTypeElementTO dimPurchaseTcTypeElementTO, SqlCommand cmdUpdate);
        int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement);
        int DeleteDimPurchaseTcTypeElement(Int32 idPurchasseTcTypeElement, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPurchasseTcTypeElement, SqlCommand cmdDelete);
    }
}
