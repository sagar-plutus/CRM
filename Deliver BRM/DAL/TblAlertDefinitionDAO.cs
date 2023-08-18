using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblAlertDefinitionDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT alertDef.*,tblmodule.moduleDesc FROM [tblAlertDefinition] alertDef " +
                                " LEFT JOIN tblModule tblmodule ON tblmodule.idModule = alertDef.moduleId " +
                                " WHERE alertDef.isActive = 1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblAlertDefinitionTO> SelectAllTblAlertDefinition()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery();
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

        public static TblAlertDefinitionTO SelectTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND idAlertDef = " + idAlertDef + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertDefinitionTO> list = ConvertDTToList(sqlReader);
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
                cmdSelect.Dispose();
            }
        }

        public static List<TblAlertDefinitionTO> ConvertDTToList(SqlDataReader tblAlertDefinitionTODT)
        {
            List<TblAlertDefinitionTO> tblAlertDefinitionTOList = new List<TblAlertDefinitionTO>();
            if (tblAlertDefinitionTODT != null)
            {
                while (tblAlertDefinitionTODT.Read())
                {
                    TblAlertDefinitionTO tblAlertDefinitionTONew = new TblAlertDefinitionTO();
                    if (tblAlertDefinitionTODT["idAlertDef"] != DBNull.Value)
                        tblAlertDefinitionTONew.IdAlertDef = Convert.ToInt32(tblAlertDefinitionTODT["idAlertDef"].ToString());
                    if (tblAlertDefinitionTODT["isAutoReset"] != DBNull.Value)
                        tblAlertDefinitionTONew.IsAutoReset = Convert.ToInt32(tblAlertDefinitionTODT["isAutoReset"].ToString());
                    if (tblAlertDefinitionTODT["isSysGenerated"] != DBNull.Value)
                        tblAlertDefinitionTONew.IsSysGenerated = Convert.ToInt32(tblAlertDefinitionTODT["isSysGenerated"].ToString());
                    if (tblAlertDefinitionTODT["createdBy"] != DBNull.Value)
                        tblAlertDefinitionTONew.CreatedBy = Convert.ToInt32(tblAlertDefinitionTODT["createdBy"].ToString());
                    if (tblAlertDefinitionTODT["updatedBy"] != DBNull.Value)
                        tblAlertDefinitionTONew.UpdatedBy = Convert.ToInt32(tblAlertDefinitionTODT["updatedBy"].ToString());
                    if (tblAlertDefinitionTODT["createdOn"] != DBNull.Value)
                        tblAlertDefinitionTONew.CreatedOn = Convert.ToDateTime(tblAlertDefinitionTODT["createdOn"].ToString());
                    if (tblAlertDefinitionTODT["updatedOn"] != DBNull.Value)
                        tblAlertDefinitionTONew.UpdatedOn = Convert.ToDateTime(tblAlertDefinitionTODT["updatedOn"].ToString());
                    if (tblAlertDefinitionTODT["alertDefDesc"] != DBNull.Value)
                        tblAlertDefinitionTONew.AlertDefDesc = Convert.ToString(tblAlertDefinitionTODT["alertDefDesc"].ToString());
                    if (tblAlertDefinitionTODT["defaultAlertTxt"] != DBNull.Value)
                        tblAlertDefinitionTONew.DefaultAlertTxt = Convert.ToString(tblAlertDefinitionTODT["defaultAlertTxt"].ToString());
                    if (tblAlertDefinitionTODT["navigationUrl"] != DBNull.Value)
                        tblAlertDefinitionTONew.NavigationUrl = Convert.ToString(tblAlertDefinitionTODT["navigationUrl"].ToString());
                    
                    //Priyanka [20-09-18] Added
                    if (tblAlertDefinitionTODT["moduleId"] != DBNull.Value)
                        tblAlertDefinitionTONew.ModuleId = Convert.ToInt32(tblAlertDefinitionTODT["moduleId"].ToString());
                    if (tblAlertDefinitionTODT["isActive"] != DBNull.Value)
                        tblAlertDefinitionTONew.IsActive = Convert.ToInt32(tblAlertDefinitionTODT["isActive"].ToString());
                    if (tblAlertDefinitionTODT["moduleDesc"] != DBNull.Value)
                        tblAlertDefinitionTONew.ModuleDesc = Convert.ToString(tblAlertDefinitionTODT["moduleDesc"].ToString());
                    if (tblAlertDefinitionTODT["isTxnAlert"] != DBNull.Value)
                        tblAlertDefinitionTONew.IsTxnAlert = Convert.ToInt32(tblAlertDefinitionTODT["isTxnAlert"].ToString());
                    if (tblAlertDefinitionTODT["alertDescription"] != DBNull.Value)
                        tblAlertDefinitionTONew.AlertDescription = Convert.ToString(tblAlertDefinitionTODT["alertDescription"].ToString());
                    tblAlertDefinitionTOList.Add(tblAlertDefinitionTONew);
                }
            }
            return tblAlertDefinitionTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertDefinitionTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertDefinitionTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblAlertDefinitionTO tblAlertDefinitionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertDefinition]( " +
                                "  [idAlertDef]" +
                                " ,[isAutoReset]" +
                                " ,[isSysGenerated]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[alertDefDesc]" +
                                " ,[defaultAlertTxt]" +
                                " ,[navigationUrl]" +
                                " ,[moduleId]" +
                                " ,[isActive] " +
                                " )" +
                    " VALUES (" +
                                "  @IdAlertDef " +
                                " ,@IsAutoReset " +
                                " ,@IsSysGenerated " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@AlertDefDesc " +
                                " ,@DefaultAlertTxt " +
                                " ,@navigationUrl " +
                                " ,@ModuleId "+
                                " ,@IsActive "+
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdAlertDef", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IdAlertDef;
            cmdInsert.Parameters.Add("@IsAutoReset", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IsAutoReset;
            cmdInsert.Parameters.Add("@IsSysGenerated", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IsSysGenerated;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertDefinitionTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertDefinitionTO.UpdatedOn;
            cmdInsert.Parameters.Add("@AlertDefDesc", System.Data.SqlDbType.NVarChar).Value = tblAlertDefinitionTO.AlertDefDesc;
            cmdInsert.Parameters.Add("@DefaultAlertTxt", System.Data.SqlDbType.NVarChar).Value = tblAlertDefinitionTO.DefaultAlertTxt;
            cmdInsert.Parameters.Add("@navigationUrl", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.NavigationUrl);

            //Priyanka [27-09-2018]
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.ModuleId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.IsActive);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertDefinitionTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertDefinitionTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblAlertDefinitionTO tblAlertDefinitionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertDefinition] SET " +
                                "  [isAutoReset]= @IsAutoReset" +
                                " ,[isSysGenerated]= @IsSysGenerated" +
                                " ,[createdBy]= @CreatedBy" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[createdOn]= @CreatedOn" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[alertDefDesc]= @AlertDefDesc" +
                                " ,[defaultAlertTxt] = @DefaultAlertTxt" +
                                " ,[navigationUrl] = @navigationUrl" +
                                " ,[moduleId] = @ModuleId" +
                                " ,[isActive] = @IsActive "+
                                " WHERE [idAlertDef] = @IdAlertDef ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAlertDef", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IdAlertDef;
            cmdUpdate.Parameters.Add("@IsAutoReset", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IsAutoReset;
            cmdUpdate.Parameters.Add("@IsSysGenerated", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IsSysGenerated;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertDefinitionTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertDefinitionTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@AlertDefDesc", System.Data.SqlDbType.NVarChar).Value = tblAlertDefinitionTO.AlertDefDesc;
            cmdUpdate.Parameters.Add("@DefaultAlertTxt", System.Data.SqlDbType.NVarChar).Value = tblAlertDefinitionTO.DefaultAlertTxt;
            cmdUpdate.Parameters.Add("@navigationUrl", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.NavigationUrl);

            //Priyanka [27-09-2018]
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.ModuleId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertDefinitionTO.IsActive);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblAlertDefinition(Int32 idAlertDef)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAlertDef, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAlertDef, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idAlertDef, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertDefinition] " +
            " WHERE idAlertDef = " + idAlertDef + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAlertDef", System.Data.SqlDbType.Int).Value = tblAlertDefinitionTO.IdAlertDef;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
