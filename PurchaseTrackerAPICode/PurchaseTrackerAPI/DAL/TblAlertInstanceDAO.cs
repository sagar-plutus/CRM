using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblAlertInstanceDAO : ITblAlertInstanceDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAlertInstanceDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT alert.*  , userDtl.userDisplayName " +
                                  " FROM tblAlertInstance alert " +
                                  " LEFT JOIN tblUser userDtl " +
                                  " ON alert.raisedBy = userDtl.idUser ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblAlertInstanceTO> SelectAllTblAlertInstance()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblAlertInstanceTO> SelectAllTblAlertInstance(Int32 userId, Int32 roleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblAlertInstanceTO SelectTblAlertInstance(Int32 idAlertInstance)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idAlertInstance = " + idAlertInstance + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertInstanceTO> list = ConvertDTToList(sqlReader);
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
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblAlertInstanceTO> ConvertDTToList(SqlDataReader tblAlertInstanceTODT)
        {
            List<TblAlertInstanceTO> tblAlertInstanceTOList = new List<TblAlertInstanceTO>();
            if (tblAlertInstanceTODT != null)
            {
                while (tblAlertInstanceTODT.Read())
                {
                    TblAlertInstanceTO tblAlertInstanceTONew = new TblAlertInstanceTO();
                    if (tblAlertInstanceTODT["idAlertInstance"] != DBNull.Value)
                        tblAlertInstanceTONew.IdAlertInstance = Convert.ToInt32(tblAlertInstanceTODT["idAlertInstance"].ToString());
                    if (tblAlertInstanceTODT["raisedBy"] != DBNull.Value)
                        tblAlertInstanceTONew.RaisedBy = Convert.ToInt32(tblAlertInstanceTODT["raisedBy"].ToString());
                    if (tblAlertInstanceTODT["sourceEntityId"] != DBNull.Value)
                        tblAlertInstanceTONew.SourceEntityId = Convert.ToInt32(tblAlertInstanceTODT["sourceEntityId"].ToString());
                    if (tblAlertInstanceTODT["isAutoReset"] != DBNull.Value)
                        tblAlertInstanceTONew.IsAutoReset = Convert.ToInt32(tblAlertInstanceTODT["isAutoReset"].ToString());
                    if (tblAlertInstanceTODT["alertDefinitionId"] != DBNull.Value)
                        tblAlertInstanceTONew.AlertDefinitionId = Convert.ToInt32(tblAlertInstanceTODT["alertDefinitionId"].ToString());
                    if (tblAlertInstanceTODT["emailId"] != DBNull.Value)
                        tblAlertInstanceTONew.EmailId = Convert.ToInt32(tblAlertInstanceTODT["emailId"].ToString());
                    if (tblAlertInstanceTODT["smsId"] != DBNull.Value)
                        tblAlertInstanceTONew.SmsId = Convert.ToInt32(tblAlertInstanceTODT["smsId"].ToString());
                    if (tblAlertInstanceTODT["parentAlertId"] != DBNull.Value)
                        tblAlertInstanceTONew.ParentAlertId = Convert.ToInt32(tblAlertInstanceTODT["parentAlertId"].ToString());
                    if (tblAlertInstanceTODT["isActive"] != DBNull.Value)
                        tblAlertInstanceTONew.IsActive = Convert.ToInt32(tblAlertInstanceTODT["isActive"].ToString());
                    if (tblAlertInstanceTODT["effectiveFromDate"] != DBNull.Value)
                        tblAlertInstanceTONew.EffectiveFromDate = Convert.ToDateTime(tblAlertInstanceTODT["effectiveFromDate"].ToString());
                    if (tblAlertInstanceTODT["effectiveToDate"] != DBNull.Value)
                        tblAlertInstanceTONew.EffectiveToDate = Convert.ToDateTime(tblAlertInstanceTODT["effectiveToDate"].ToString());
                    if (tblAlertInstanceTODT["raisedOn"] != DBNull.Value)
                        tblAlertInstanceTONew.RaisedOn = Convert.ToDateTime(tblAlertInstanceTODT["raisedOn"].ToString());
                    //if (tblAlertInstanceTODT["escalationOn"] != DBNull.Value)
                    //    tblAlertInstanceTONew.EscalationOn = Convert.ToDateTime(tblAlertInstanceTODT["escalationOn"].ToString());
                    if (tblAlertInstanceTODT["alertComment"] != DBNull.Value)
                        tblAlertInstanceTONew.AlertComment = Convert.ToString(tblAlertInstanceTODT["alertComment"].ToString());
                    if (tblAlertInstanceTODT["sourceDisplayId"] != DBNull.Value)
                        tblAlertInstanceTONew.SourceDisplayId = Convert.ToString(tblAlertInstanceTODT["sourceDisplayId"].ToString());
                    if (tblAlertInstanceTODT["alertAction"] != DBNull.Value)
                        tblAlertInstanceTONew.AlertAction = Convert.ToString(tblAlertInstanceTODT["alertAction"].ToString());
                    if (tblAlertInstanceTODT["userDisplayName"] != DBNull.Value)
                        tblAlertInstanceTONew.RaisedByUserName = Convert.ToString(tblAlertInstanceTODT["userDisplayName"].ToString());
                    tblAlertInstanceTOList.Add(tblAlertInstanceTONew);
                }
            }
            return tblAlertInstanceTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertInstanceTO, cmdInsert);
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

        public  int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertInstanceTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblAlertInstanceTO tblAlertInstanceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertInstance]( " +
                            "  [raisedBy]" +
                            " ,[sourceEntityId]" +
                            " ,[isAutoReset]" +
                            " ,[alertDefinitionId]" +
                            " ,[emailId]" +
                            " ,[smsId]" +
                            " ,[parentAlertId]" +
                            " ,[isActive]" +
                            " ,[effectiveFromDate]" +
                            " ,[effectiveToDate]" +
                            " ,[raisedOn]" +
                            //" ,[escalationOn]" +
                            " ,[alertComment]" +
                            " ,[sourceDisplayId]" +
                            " ,[alertAction]" +
                            " )" +
                " VALUES (" +
                            "  @RaisedBy " +
                            " ,@SourceEntityId " +
                            " ,@IsAutoReset " +
                            " ,@AlertDefinitionId " +
                            " ,@EmailId " +
                            " ,@SmsId " +
                            " ,@ParentAlertId " +
                            " ,@IsActive " +
                            " ,@EffectiveFromDate " +
                            " ,@EffectiveToDate " +
                            " ,@RaisedOn " +
                            //" ,@EscalationOn " +
                            " ,@AlertComment " +
                            " ,@SourceDisplayId " +
                            " ,@AlertAction " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAlertInstance", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IdAlertInstance;
            cmdInsert.Parameters.Add("@RaisedBy", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.RaisedBy;
            cmdInsert.Parameters.Add("@SourceEntityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SourceEntityId);
            cmdInsert.Parameters.Add("@IsAutoReset", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IsAutoReset;
            cmdInsert.Parameters.Add("@AlertDefinitionId", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.AlertDefinitionId;
            cmdInsert.Parameters.Add("@EmailId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.EmailId);
            cmdInsert.Parameters.Add("@SmsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SmsId);
            cmdInsert.Parameters.Add("@ParentAlertId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.ParentAlertId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IsActive;
            cmdInsert.Parameters.Add("@EffectiveFromDate", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.EffectiveFromDate;
            cmdInsert.Parameters.Add("@EffectiveToDate", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.EffectiveToDate;
            cmdInsert.Parameters.Add("@RaisedOn", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.RaisedOn;
            //cmdInsert.Parameters.Add("@EscalationOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.EscalationOn);
            cmdInsert.Parameters.Add("@AlertComment", System.Data.SqlDbType.NVarChar).Value = tblAlertInstanceTO.AlertComment;
            cmdInsert.Parameters.Add("@SourceDisplayId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SourceDisplayId);
            cmdInsert.Parameters.Add("@AlertAction", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.AlertAction);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertInstanceTO.IdAlertInstance = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Updation
        public  int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertInstanceTO, cmdUpdate);
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

        public  int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertInstanceTO, cmdUpdate);
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

        public  int ResetAlertInstanceByDef(String alertDefIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblAlertInstance] SET " +
                                   " [isActive]= 0" +
                                   " WHERE [alertDefinitionId] IN (" + alertDefIds + ") AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
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

        public  int ResetAutoResetAlertInstances(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblAlertInstance] SET " +
                                   " [isActive]= 0" +
                                   " WHERE [alertDefinitionId] IN ( SELECT idAlertDef FROM tblAlertDefinition WHERE isAutoReset=1) AND isActive=1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
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

        public  int ResetAlertInstance(int alertDefId,int sourceEntityId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblAlertInstance] SET " +
                                   " [isActive]= 0" +
                                   " WHERE [alertDefinitionId] = @alertDefinitionId AND sourceEntityId=@sourceEntityId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@SourceEntityId", System.Data.SqlDbType.Int).Value = sourceEntityId;
                cmdUpdate.Parameters.Add("@AlertDefinitionId", System.Data.SqlDbType.Int).Value = alertDefId;
                return cmdUpdate.ExecuteNonQuery();
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

        public  int ExecuteUpdationCommand(TblAlertInstanceTO tblAlertInstanceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertInstance] SET " +
                            "  [raisedBy]= @RaisedBy" +
                            " ,[sourceEntityId]= @SourceEntityId" +
                            " ,[isAutoReset]= @IsAutoReset" +
                            " ,[alertDefinitionId]= @AlertDefinitionId" +
                            " ,[emailId]= @EmailId" +
                            " ,[smsId]= @SmsId" +
                            " ,[parentAlertId]= @ParentAlertId" +
                            " ,[isActive]= @IsActive" +
                            " ,[effectiveFromDate]= @EffectiveFromDate" +
                            " ,[effectiveToDate]= @EffectiveToDate" +
                            " ,[raisedOn]= @RaisedOn" +
                            //" ,[escalationOn]= @EscalationOn" +
                            " ,[alertComment]= @AlertComment" +
                            " ,[sourceDisplayId]= @SourceDisplayId" +
                            " ,[alertAction] = @AlertAction" +
                            " WHERE [idAlertInstance] = @IdAlertInstance ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAlertInstance", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IdAlertInstance;
            cmdUpdate.Parameters.Add("@RaisedBy", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.RaisedBy;
            cmdUpdate.Parameters.Add("@SourceEntityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SourceEntityId);
            cmdUpdate.Parameters.Add("@IsAutoReset", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IsAutoReset;
            cmdUpdate.Parameters.Add("@AlertDefinitionId", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.AlertDefinitionId;
            cmdUpdate.Parameters.Add("@EmailId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.EmailId);
            cmdUpdate.Parameters.Add("@SmsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SmsId);
            cmdUpdate.Parameters.Add("@ParentAlertId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.ParentAlertId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IsActive;
            cmdUpdate.Parameters.Add("@EffectiveFromDate", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.EffectiveFromDate;
            cmdUpdate.Parameters.Add("@EffectiveToDate", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.EffectiveToDate;
            cmdUpdate.Parameters.Add("@RaisedOn", System.Data.SqlDbType.DateTime).Value = tblAlertInstanceTO.RaisedOn;
            //cmdUpdate.Parameters.Add("@EscalationOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.EscalationOn);
            cmdUpdate.Parameters.Add("@AlertComment", System.Data.SqlDbType.NVarChar).Value = tblAlertInstanceTO.AlertComment;
            cmdUpdate.Parameters.Add("@SourceDisplayId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.SourceDisplayId);
            cmdUpdate.Parameters.Add("@AlertAction", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertInstanceTO.AlertAction);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblAlertInstance(Int32 idAlertInstance)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAlertInstance, cmdDelete);
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

        public  int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAlertInstance, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idAlertInstance, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertInstance] " +
            " WHERE idAlertInstance = " + idAlertInstance + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAlertInstance", System.Data.SqlDbType.Int).Value = tblAlertInstanceTO.IdAlertInstance;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
