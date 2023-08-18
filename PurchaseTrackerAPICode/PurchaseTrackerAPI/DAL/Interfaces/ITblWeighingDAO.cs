using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblWeighingDAO
    {
        String SqlSelectQuery();
        List<TblWeighingTO> SelectAllTblWeighing();
        TblWeighingTO SelectTblWeighing(Int32 idWeighing);
        TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr, DateTime timeStamp);
        List<TblWeighingTO> SelectAllTblWeighing(SqlConnection conn, SqlTransaction tran);
        int InsertTblWeighing(TblWeighingTO tblWeighingTO);
        int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblWeighingTO tblWeighingTO, SqlCommand cmdInsert);
        int UpdateTblWeighing(TblWeighingTO tblWeighingTO);
        int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblWeighingTO tblWeighingTO, SqlCommand cmdUpdate);
        List<TblWeighingTO> ConvertDTToList(SqlDataReader tblWeighingTODT);
        int DeleteTblWeighing(Int32 idWeighing);
        int DeleteTblWeighingByByMachineIp(string ipAddr);
        int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idWeighing, SqlCommand cmdDelete);
        int ExecuteDeletionCommandByMachineIp(string ipAddr, SqlCommand cmdDelete);

    }
}