using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.BL;

namespace SalesTrackerAPI.DAL
{
    public class TblEInvoiceApiDAO
    {
        public TblEInvoiceApiDAO()
        {

        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT idApi, apiName, apiMethod, apiBaseUri, apiFunctionName, headerParam, bodyParam, createdBy, createdOn, isActive, updatedBy, updatedOn, isSession, accessToken, sessionExpiresAt " +
                                  " FROM tblEInvoiceApi";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblEInvoiceApiTO> SelectAllTblEInvoiceApi()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblEInvoiceApiTO> SelectAllTblEInvoiceApi(Int32 idApi)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idApi=" + idApi;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblEInvoiceApiTO> SelectTblEInvoiceApi(string apiName, int OrgId = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE apiName='" + apiName + "' AND ISNULL(OrgId, 0) = " + OrgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceApiTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblEInvoiceApiTO> ConvertDTToList(SqlDataReader tblEInvoiceApiTODT)
        {
            List<TblEInvoiceApiTO> tblEInvoiceApiTOList = new List<TblEInvoiceApiTO>();
            if (tblEInvoiceApiTODT != null)
            {
                while(tblEInvoiceApiTODT.Read())
                {
                    TblEInvoiceApiTO tblEInvoiceApiTONew = new TblEInvoiceApiTO();
                    if (tblEInvoiceApiTODT["idApi"] != DBNull.Value)
                        tblEInvoiceApiTONew.IdApi= Convert.ToInt32(tblEInvoiceApiTODT["idApi"].ToString());
                    if (tblEInvoiceApiTODT["apiName"] != DBNull.Value)
                        tblEInvoiceApiTONew.ApiName = tblEInvoiceApiTODT["apiName"].ToString();
                    if (tblEInvoiceApiTODT["apiMethod"] != DBNull.Value)
                        tblEInvoiceApiTONew.ApiMethod = tblEInvoiceApiTODT["apiMethod"].ToString();
                    if (tblEInvoiceApiTODT["apiBaseUri"] != DBNull.Value)
                        tblEInvoiceApiTONew.ApiBaseUri = tblEInvoiceApiTODT["apiBaseUri"].ToString();
                    if (tblEInvoiceApiTODT["apiFunctionName"] != DBNull.Value)
                        tblEInvoiceApiTONew.ApiFunctionName = tblEInvoiceApiTODT["apiFunctionName"].ToString();
                    if (tblEInvoiceApiTODT["headerParam"] != DBNull.Value)
                        tblEInvoiceApiTONew.HeaderParam = tblEInvoiceApiTODT["headerParam"].ToString();
                    if (tblEInvoiceApiTODT["bodyParam"] != DBNull.Value)
                        tblEInvoiceApiTONew.BodyParam = tblEInvoiceApiTODT["bodyParam"].ToString();
                    if (tblEInvoiceApiTODT["createdBy"] != DBNull.Value)
                        tblEInvoiceApiTONew.CreatedBy = Convert.ToInt32(tblEInvoiceApiTODT["createdBy"].ToString());
                    if (tblEInvoiceApiTODT["updatedBy"] != DBNull.Value)
                        tblEInvoiceApiTONew.UpdatedBy = Convert.ToInt32(tblEInvoiceApiTODT["updatedBy"].ToString());
                    if (tblEInvoiceApiTODT["createdOn"] != DBNull.Value)
                        tblEInvoiceApiTONew.CreatedOn = Convert.ToDateTime(tblEInvoiceApiTODT["createdOn"].ToString());
                    if (tblEInvoiceApiTODT["updatedOn"] != DBNull.Value)
                        tblEInvoiceApiTONew.UpdatedOn = Convert.ToDateTime(tblEInvoiceApiTODT["updatedOn"].ToString());
                    if (tblEInvoiceApiTODT["isActive"] != DBNull.Value)
                        tblEInvoiceApiTONew.IsActive = Convert.ToInt32(tblEInvoiceApiTODT["isActive"].ToString());
                    if (tblEInvoiceApiTODT["isSession"] != DBNull.Value)
                        tblEInvoiceApiTONew.IsSession = Convert.ToInt32(tblEInvoiceApiTODT["isSession"].ToString());
                    if (tblEInvoiceApiTODT["accessToken"] != DBNull.Value)
                        tblEInvoiceApiTONew.AccessToken = tblEInvoiceApiTODT["accessToken"].ToString();
                    if (tblEInvoiceApiTODT["sessionExpiresAt"] != DBNull.Value)
                        tblEInvoiceApiTONew.SessionExpiresAt = Convert.ToDateTime(tblEInvoiceApiTODT["sessionExpiresAt"].ToString());
                    tblEInvoiceApiTOList.Add(tblEInvoiceApiTONew);
                }
            }
            return tblEInvoiceApiTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblEInvoiceApiTO, cmdInsert);
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

        public static int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblEInvoiceApiTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblEInvoiceApi]( " +
                                "  [apiName]" +
                                " ,[apiMethod]" +
                                " ,[apiBaseUri]" +
                                " ,[apiFunctionName]" +
                                " ,[headerParam]" +
                                " ,[bodyParam]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[isActive]" +
                                " ,[isSession]" +
                                " ,[accessToken]" +
                                " ,[sessionExpiresAt]" +
                                " )" +
                    " VALUES (" +
                                "  @ApiName " +
                                " ,@ApiMethod " +
                                " ,@ApiBaseUri " +
                                " ,@ApiFunctionName " +
                                " ,@HeaderParam " +
                                " ,@BodyParam " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@IsActive " +
                                " ,@IsSession " +
                                " ,@AccessToken " +
                                " ,@SessionExpiresAt " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdApi", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IdApi;
            cmdInsert.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.ApiName);
            cmdInsert.Parameters.Add("@ApiMethod", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.ApiMethod);
            cmdInsert.Parameters.Add("@ApiBaseUri", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.ApiBaseUri);
            cmdInsert.Parameters.Add("@ApiFunctionName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.ApiFunctionName);
            cmdInsert.Parameters.Add("@HeaderParam", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.HeaderParam);
            cmdInsert.Parameters.Add("@BodyParam", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.BodyParam);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IsActive;
            cmdInsert.Parameters.Add("@IsSession", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IsSession;
            cmdInsert.Parameters.Add("@AccessToken", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEInvoiceApiTO.AccessToken);
            cmdInsert.Parameters.Add("@SessionExpiresAt", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.SessionExpiresAt;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblEInvoiceApiTO.IdApi = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblEInvoiceApiTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblEInvoiceApiTO, cmdUpdate);
            }
            catch(Exception ex)
            {  
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEInvoiceApi] SET " +
            "  [apiName] = @ApiName" +
            " ,[apiMethod]= @ApiMethod" +
            " ,[apiBaseUri]= @ApiBaseUri" +
            " ,[apiFunctionName]= @ApiFunctionName" +
            " ,[headerParam]= @HeaderParam" +
            " ,[bodyParam]= @BodyParam" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive] = @IsActive" +
            " ,[isSession] = @IsSession" +
            " ,[accessToken] = @AccessToken" +
            " ,[sessionExpiresAt] = @SessionExpiresAt" +
            " WHERE [idApi] = @idApi "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idApi", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IdApi;
            cmdUpdate.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.ApiName;
            cmdUpdate.Parameters.Add("@ApiMethod", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.ApiMethod;
            cmdUpdate.Parameters.Add("@ApiBaseUri", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.ApiBaseUri;
            cmdUpdate.Parameters.Add("@ApiFunctionName", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.ApiFunctionName;
            cmdUpdate.Parameters.Add("@HeaderParam", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.HeaderParam;
            cmdUpdate.Parameters.Add("@BodyParam", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.BodyParam;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IsActive;
            cmdUpdate.Parameters.Add("@IsSession", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IsSession;
            cmdUpdate.Parameters.Add("@AccessToken", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.AccessToken;
            cmdUpdate.Parameters.Add("@SessionExpiresAt", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.SessionExpiresAt;
            return cmdUpdate.ExecuteNonQuery();
        }
        public static int UpdateTblEInvoiceApiSession(TblEInvoiceApiTO tblEInvoiceApiTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteSessionUpdationCommand(tblEInvoiceApiTO, cmdUpdate);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblEInvoiceApiSession(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteSessionUpdationCommand(tblEInvoiceApiTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteSessionUpdationCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEInvoiceApi] SET " +
            " [isSession] = @IsSession" +
            " ,[accessToken] = @AccessToken" +
            " ,[sessionExpiresAt] = @SessionExpiresAt" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE [idApi] = @idApi ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idApi", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IdApi;
            cmdUpdate.Parameters.Add("@IsSession", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.IsSession;
            cmdUpdate.Parameters.Add("@AccessToken", System.Data.SqlDbType.NVarChar).Value = tblEInvoiceApiTO.AccessToken;
            cmdUpdate.Parameters.Add("@SessionExpiresAt", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.SessionExpiresAt;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblEInvoiceApiTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblEInvoiceApiTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblEInvoiceApi(Int32 idLocation)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public static int DeleteTblEInvoiceApi(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idApi, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblEInvoiceApi] " +
            " WHERE idApi = " + idApi + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
