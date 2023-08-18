using PurchaseTrackerAPI.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface ITblSpotEntryContainerDtlsDAO
    {
        #region Selection
        List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls();

        TblSpotEntryContainerDtlsTO SelectTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls);

        List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls(SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Insertion
        int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO);

        int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteInsertionCommand(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlCommand cmdInsert);
        #endregion

        #region Updation
        int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO);

        int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlCommand cmdUpdate);
        #endregion

        #region Deletion
        int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls);
        List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls(int spotEntryId);
        int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls, SqlConnection conn, SqlTransaction tran);

        int ExecuteDeletionCommand(Int32 idSpotEntryContainerDtls, SqlCommand cmdDelete);
        #endregion
    }
}
