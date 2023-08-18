using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimStatusBL
    {
        List<DimStatusTO> SelectAllDimStatusList();
        List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId);
        DimStatusTO SelectDimStatusTO(Int32 idStatus);
        int InsertDimStatus(DimStatusTO dimStatusTO);
        int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimStatus(DimStatusTO dimStatusTO);
        int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimStatus(Int32 idStatus);
        int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);

        DimStatusTO SelectDimStatusTOByIotStatusId(Int32 iotStatusId);

    }
}