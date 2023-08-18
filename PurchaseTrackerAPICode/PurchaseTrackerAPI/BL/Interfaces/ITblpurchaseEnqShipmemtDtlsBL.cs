using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
   public interface ITblpurchaseEnqShipmemtDtlsBL
    {
        #region Selection
        List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtls();

        List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtlsList();

        TblpurchaseEnqShipmemtDtlsTO SelectTblpurchaseEnqShipmemtDtlsTO(Int32 idShipmentDtls);
        #endregion

        #region Insertion
        int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO);

        int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO);

        int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls);

        int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls, SqlConnection conn, SqlTransaction tran);
        List<TblpurchaseEnqShipmemtDtlsTO> SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(int idPurchaseEnquiry);

        #endregion

    }
}
