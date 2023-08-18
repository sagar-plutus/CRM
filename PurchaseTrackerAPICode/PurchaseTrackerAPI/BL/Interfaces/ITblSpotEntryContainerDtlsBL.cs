using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL.Interfaces
{
    public interface ITblSpotEntryContainerDtlsBL
    {
        #region Selection
         List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls();

         List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtlsList();

         TblSpotEntryContainerDtlsTO SelectTblSpotEntryContainerDtlsTO(Int32 idSpotEntryContainerDtls);

        #endregion

        #region Insertion
         int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO);

         int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
         int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO);

         int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
         int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls);

         int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls, SqlConnection conn, SqlTransaction tran);
        List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtlsList(int spotEntryId);

        #endregion
    }
}
