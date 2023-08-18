using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblOrgLicenseDtlDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT orgLicDtl.*, dimLic.licenseName FROM tblOrgLicenseDtl orgLicDtl " +
                                  " LEFT JOIN dimCommerLicenceInfo dimLic " +
                                  " ON orgLicDtl.licenseId = dimLic.idLicense"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId=" + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId=" + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, Int32 licenseId, String licenseVal)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId <> " + orgId + " AND orgLicDtl.licenseId=" + licenseId + " AND licenseValue='" + licenseVal + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblOrgLicenseDtlTO SelectTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgLicense = " + idOrgLicense + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgLicenseDtlTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgLicenseDtlTO> ConvertDTToList(SqlDataReader tblOrgLicenseDtlTODT)
        {
            List<TblOrgLicenseDtlTO> tblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
            if (tblOrgLicenseDtlTODT != null)
            {
                while (tblOrgLicenseDtlTODT.Read())
                {
                    TblOrgLicenseDtlTO tblOrgLicenseDtlTONew = new TblOrgLicenseDtlTO();
                    if (tblOrgLicenseDtlTODT["idOrgLicense"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.IdOrgLicense = Convert.ToInt32(tblOrgLicenseDtlTODT["idOrgLicense"].ToString());
                    if (tblOrgLicenseDtlTODT["organizationId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.OrganizationId = Convert.ToInt32(tblOrgLicenseDtlTODT["organizationId"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseId = Convert.ToInt32(tblOrgLicenseDtlTODT["licenseId"].ToString());
                    if (tblOrgLicenseDtlTODT["createdBy"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.CreatedBy = Convert.ToInt32(tblOrgLicenseDtlTODT["createdBy"].ToString());
                    if (tblOrgLicenseDtlTODT["createdOn"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.CreatedOn = Convert.ToDateTime(tblOrgLicenseDtlTODT["createdOn"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseValue"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseValue = Convert.ToString(tblOrgLicenseDtlTODT["licenseValue"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseName"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseName = Convert.ToString(tblOrgLicenseDtlTODT["licenseName"].ToString());
                    tblOrgLicenseDtlTOList.Add(tblOrgLicenseDtlTONew);
                }
            }
            return tblOrgLicenseDtlTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgLicenseDtlTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgLicenseDtlTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgLicenseDtl]( " +
                            "  [organizationId]" +
                            " ,[licenseId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[licenseValue]" +
                            " )" +
                " VALUES (" +
                            "  @OrganizationId " +
                            " ,@LicenseId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@LicenseValue " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.OrganizationId;
            cmdInsert.Parameters.Add("@LicenseId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.LicenseId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgLicenseDtlTO.CreatedOn;
            cmdInsert.Parameters.Add("@LicenseValue", System.Data.SqlDbType.NVarChar).Value = tblOrgLicenseDtlTO.LicenseValue;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = SalesTrackerAPI.StaticStuff.Constants.IdentityColumnQuery;
                tblOrgLicenseDtlTO.IdOrgLicense = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgLicenseDtlTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgLicenseDtlTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgLicenseDtl] SET " + 
                            "  [organizationId]= @OrganizationId" +
                            " ,[licenseId]= @LicenseId" +
                            " ,[licenseValue] = @LicenseValue" +
                            " WHERE [idOrgLicense] = @IdOrgLicense"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.OrganizationId;
            cmdUpdate.Parameters.Add("@LicenseId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.LicenseId;
            cmdUpdate.Parameters.Add("@LicenseValue", System.Data.SqlDbType.NVarChar).Value = tblOrgLicenseDtlTO.LicenseValue;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgLicense, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgLicense, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idOrgLicense, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgLicenseDtl] " +
            " WHERE idOrgLicense = " + idOrgLicense +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
