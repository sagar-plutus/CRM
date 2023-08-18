using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using PurchaseTrackerAPI.Models;
using System.Data;
namespace PurchaseTrackerAPI.DAL.Interfaces
{
    public interface Itblorglicensedtldao
    {
        String SqlSelectQuery();
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, Int32 licenseId, String licenseVal);
        TblOrgLicenseDtlTO SelectTblOrgLicenseDtl(Int32 idOrgLicense);
        List<TblOrgLicenseDtlTO> ConvertDTToList(SqlDataReader tblOrgLicenseDtlTODT);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdInsert);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdUpdate);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgLicense, SqlCommand cmdDelete);

    }
}