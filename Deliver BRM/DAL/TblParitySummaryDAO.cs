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
    public class TblParitySummaryDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblParitySummary.*,stateName FROM [tblParitySummary] tblParitySummary" +
                                  " LEFT JOIN dimState ON idState=stateId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblParitySummaryTO> SelectAllTblParitySummary()
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
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
                return list;
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

        public static List<TblParitySummaryTO> SelectAllTblParitySummary(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count != 0)
                    return list;
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

        public static TblParitySummaryTO SelectTblParitySummary(Int32 idParity,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idParity = " + idParity +" ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
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

        public static TblParitySummaryTO SelectParitySummaryFromParityDtlId(Int32 parityDtlId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idParity IN(SELECT parityId FROM tblParityDetails WHERE idParityDtl=" + parityDtlId + ")";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
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

        public static TblParitySummaryTO SelectStatesActiveParitySummary(Int32 stateId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE stateId = " + stateId + " AND isActive=1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
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

        public static TblParitySummaryTO SelectActiveParitySummaryTO(int dealerId, SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1" +
                                        " AND stateId IN(SELECT stateId FROM tblOrganization " +
                                        " INNER JOIN " +
                                        " ( " +
                                        " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                        " INNER JOIN tblAddress ON idAddr = addressId " +
                                        " WHERE addrTypeId = 1 AND organizationId =" + dealerId +
                                        " ) addrDtl " +
                                        " ON idOrganization = organizationId WHERE tblOrganization.isActive = 1 AND idOrganization=" + dealerId + " )";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblParitySummaryTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblParitySummaryTO> ConvertDTToList(SqlDataReader tblParitySummaryTODT)
        {
            List<TblParitySummaryTO> tblParitySummaryTOList = new List<TblParitySummaryTO>();
            if (tblParitySummaryTODT != null)
            {
                while (tblParitySummaryTODT.Read())
                {
                    TblParitySummaryTO tblParitySummaryTONew = new TblParitySummaryTO();
                    if (tblParitySummaryTODT["idParity"] != DBNull.Value)
                        tblParitySummaryTONew.IdParity = Convert.ToInt32(tblParitySummaryTODT["idParity"].ToString());
                    if (tblParitySummaryTODT["createdBy"] != DBNull.Value)
                        tblParitySummaryTONew.CreatedBy = Convert.ToInt32(tblParitySummaryTODT["createdBy"].ToString());
                    if (tblParitySummaryTODT["isActive"] != DBNull.Value)
                        tblParitySummaryTONew.IsActive = Convert.ToInt32(tblParitySummaryTODT["isActive"].ToString());
                    if (tblParitySummaryTODT["createdOn"] != DBNull.Value)
                        tblParitySummaryTONew.CreatedOn = Convert.ToDateTime(tblParitySummaryTODT["createdOn"].ToString());
                    if (tblParitySummaryTODT["remark"] != DBNull.Value)
                        tblParitySummaryTONew.Remark = Convert.ToString(tblParitySummaryTODT["remark"].ToString());
                    if (tblParitySummaryTODT["stateId"] != DBNull.Value)
                        tblParitySummaryTONew.StateId = Convert.ToInt32(tblParitySummaryTODT["stateId"].ToString());
                    if (tblParitySummaryTODT["stateName"] != DBNull.Value)
                        tblParitySummaryTONew.StateName = Convert.ToString(tblParitySummaryTODT["stateName"].ToString());
                    if (tblParitySummaryTODT["baseValCorAmt"] != DBNull.Value)
                        tblParitySummaryTONew.BaseValCorAmt = Convert.ToDouble(tblParitySummaryTODT["baseValCorAmt"].ToString());
                    if (tblParitySummaryTODT["freightAmt"] != DBNull.Value)
                        tblParitySummaryTONew.FreightAmt = Convert.ToDouble(tblParitySummaryTODT["freightAmt"].ToString());
                    if (tblParitySummaryTODT["expenseAmt"] != DBNull.Value)
                        tblParitySummaryTONew.ExpenseAmt = Convert.ToDouble(tblParitySummaryTODT["expenseAmt"].ToString());
                    if (tblParitySummaryTODT["otherAmt"] != DBNull.Value)
                        tblParitySummaryTONew.OtherAmt = Convert.ToDouble(tblParitySummaryTODT["otherAmt"].ToString());

                    tblParitySummaryTOList.Add(tblParitySummaryTONew);
                }
            }
            return tblParitySummaryTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblParitySummaryTO, cmdInsert);
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

        public static int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblParitySummaryTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblParitySummaryTO tblParitySummaryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblParitySummary]( " + 
                            "  [createdBy]" +
                            " ,[isActive]" +
                            " ,[createdOn]" +
                            " ,[remark]" +
                            " ,[stateId]" +
                            " ,[baseValCorAmt]" +
                            " ,[freightAmt]" +
                            " ,[expenseAmt]" +
                            " ,[otherAmt]" +
                            " )" +
                " VALUES (" +
                            "  @CreatedBy " +
                            " ,@IsActive " +
                            " ,@CreatedOn " +
                            " ,@Remark " +
                            " ,@stateId " +
                            " ,@baseValCorAmt " +
                            " ,@freightAmt " +
                            " ,@expenseAmt " +
                            " ,@otherAmt " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdParity", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IdParity;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParitySummaryTO.CreatedOn;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.Remark);
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.StateId);
            cmdInsert.Parameters.Add("@baseValCorAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.BaseValCorAmt);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.FreightAmt);
            cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ExpenseAmt);
            cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.OtherAmt);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblParitySummaryTO.IdParity = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblParitySummaryTO, cmdUpdate);
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

        public static int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblParitySummaryTO, cmdUpdate);
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

        public static int DeactivateAllParitySummary(int stateId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblParitySummary] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE stateId=@StateId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = stateId;
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

        public static int ExecuteUpdationCommand(TblParitySummaryTO tblParitySummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblParitySummary] SET " + 
            "  [createdBy]= @CreatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[remark] = @Remark" +
            " ,[stateId] = @stateId" +
            " ,[baseValCorAmt] = @baseValCorAmt" +
            " ,[freightAmt] = @freightAmt" +
            " ,[expenseAmt] = @expenseAmt" +
            " ,[otherAmt] = @otherAmt" +
            " WHERE [idParity] = @IdParity"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdParity", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IdParity;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblParitySummaryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.Remark);
            cmdUpdate.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.StateId);
            cmdUpdate.Parameters.Add("@baseValCorAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.BaseValCorAmt);
            cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.FreightAmt);
            cmdUpdate.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.ExpenseAmt);
            cmdUpdate.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblParitySummaryTO.OtherAmt);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblParitySummary(Int32 idParity)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idParity, cmdDelete);
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

        public static int DeleteTblParitySummary(Int32 idParity, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idParity, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idParity, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblParitySummary] " +
            " WHERE idParity = " + idParity +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idParity", System.Data.SqlDbType.Int).Value = tblParitySummaryTO.IdParity;
            return cmdDelete.ExecuteNonQuery();
        }

        public static int DeleteAllTblParitySummary(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteAllDeletionCommand(cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteAllDeletionCommand(SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblParitySummary] ";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
