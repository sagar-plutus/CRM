using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblWeighingMachineDAO
    {
        String SqlSelectQuery();
        List<TblWeighingMachineTO> SelectAllTblWeighingMachine();
        List<DropDownTO> SelectTblWeighingMachineDropDownList();
        TblWeighingMachineTO SelectTblWeighingMachine(Int32 idWeighingMachine);
        TblWeighingMachineTO SelectTblWeighingMachine(string ipAddr);
        List<TblWeighingMachineTO> ConvertDTToList(SqlDataReader tblWeighingMachineTODT);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblWeighingMachineTO tblWeighingMachineTO, SqlCommand cmdInsert);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblWeighingMachineTO tblWeighingMachineTO, SqlCommand cmdUpdate);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idWeighingMachine, SqlCommand cmdDelete);
        TblWeighingMachineTO SelectTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran);

    }
}