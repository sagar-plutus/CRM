using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblpurchaseEnqShipmemtDtlsExtDAO
    {

        #region Selection
        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt();

        TblpurchaseEnqShipmemtDtlsExtTO SelectTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt);

        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt(SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Insertion
        int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO);

        int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran);

         #endregion

        #region Updation
        int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO);

        int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
         int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt);
        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(int shipmentDtlsId);
        int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt, SqlConnection conn, SqlTransaction tran);
        
        #endregion
    }
}
