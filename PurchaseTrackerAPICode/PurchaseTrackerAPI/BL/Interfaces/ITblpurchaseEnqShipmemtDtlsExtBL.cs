using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
   public interface ITblpurchaseEnqShipmemtDtlsExtBL
    {
        #region Selection
        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt();

        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtList();

        TblpurchaseEnqShipmemtDtlsExtTO SelectTblpurchaseEnqShipmemtDtlsExtTO(Int32 idShipmentDtlsExt);

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

         int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt, SqlConnection conn, SqlTransaction tran);
        List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(int idShipmentDtls);

        #endregion
    }
}
