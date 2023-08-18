using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblPersonBL
    {
        List<TblPersonTO> SelectAllTblPersonList();
        TblPersonTO SelectTblPersonTO(Int32 idPerson);
        List<TblPersonTO> SelectAllPersonListByOrganization(int organizationId);
        TblPersonTO SelectAllPersonListByUser(int userId);
        int InsertTblPerson(TblPersonTO tblPersonTO);
        int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPerson(TblPersonTO tblPersonTO);
        int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPerson(Int32 idPerson);
        int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran);

    }
}