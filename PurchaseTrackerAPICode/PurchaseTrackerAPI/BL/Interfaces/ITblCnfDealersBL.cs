using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblCnfDealersBL
    {
        List<TblCnfDealersTO> SelectAllTblCnfDealersList();
        TblCnfDealersTO SelectTblCnfDealersTO(Int32 idCnfDealerId);
        List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId, Boolean isSpecialOnly);
        List<TblCnfDealersTO> SelectAllActiveCnfDealersList(Int32 dealerId, Boolean isSpecialOnly, SqlConnection conn, SqlTransaction tran);
        int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO);
          int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran);
         // void TransferDealerToCnfDealerReleationship(); //Hrushikesh commented DI circular (it was one time utility function)

        int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO);
        int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblCnfDealers(Int32 idCnfDealerId);
        int DeleteTblCnfDealers(Int32 idCnfDealerId, SqlConnection conn, SqlTransaction tran);
    }
}