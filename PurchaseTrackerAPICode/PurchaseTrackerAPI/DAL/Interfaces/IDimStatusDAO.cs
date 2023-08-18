using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface IDimStatusDAO
    {
        String SqlSelectQuery();
        List<DimStatusTO> SelectAllDimStatus();
        List<DimStatusTO> SelectAllDimStatus(int txnTypeId);
        DimStatusTO SelectDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);
        DimStatusTO SelectDimStatus(Int32 idStatus);
        List<DimStatusTO> ConvertDTToList(SqlDataReader dimStatusTODT);
        int InsertDimStatus(DimStatusTO dimStatusTO);
        int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimStatusTO dimStatusTO, SqlCommand cmdInsert);
        int UpdateDimStatus(DimStatusTO dimStatusTO);
        int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimStatusTO dimStatusTO, SqlCommand cmdUpdate);
        int DeleteDimStatus(Int32 idStatus);
        int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStatus, SqlCommand cmdDelete);

        DimStatusTO SelectDimStatusTOByIotStatusId(Int32 iotStatusId, SqlConnection conn, SqlTransaction tran);

    }
}