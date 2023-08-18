using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblWeighingMachineBL
    {
        List<TblWeighingMachineTO> SelectAllTblWeighingMachineList();
        List<DropDownTO> SelectTblWeighingMachineDropDownList();
        TblWeighingMachineTO SelectTblWeighingMachineTO(Int32 idWeighingMachine);
        TblWeighingMachineTO SelectTblWeighingMachineTO(string ipAddr);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO);
        int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine);
        int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran);
        TblWeighingMachineTO SelectTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran);
    }
}