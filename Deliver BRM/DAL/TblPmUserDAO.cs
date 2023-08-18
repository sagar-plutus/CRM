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
    public class TblPmUserDAO
    {
        #region Methods
         public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblUser.userDisplayName,* FROM [tblPmUser] tblPmUser  " +
                                  " LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPmUser.pmId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblPmUserTO> SelectAllTblPmUser()
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

                //cmdSelect.Parameters.Add("@idPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPmUserTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblPmUserTO> SelectTblPmUser(Int32 idPmUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPmUser = " + idPmUser + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPmUserTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblPmUserTO> SelectAllPMForUser(Int32 loginUserId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPmUser.userId = " + loginUserId + " AND ISNULL(tblPmUser.isActive,0)=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPmUserTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblPmUserTO> SelectAllTblPmUser(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPmUserTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblPmUserTO> ConvertDTToList(SqlDataReader tblPmUserTODT)
        {
            List<TblPmUserTO> tblPmUserTOList = new List<TblPmUserTO>();
            if (tblPmUserTODT != null)
            {
                while (tblPmUserTODT.Read())
                {
                    TblPmUserTO tblPmUserTONew = new TblPmUserTO();
                    if (tblPmUserTODT["idPmUser"] != DBNull.Value)
                        tblPmUserTONew.IdPmUser = Convert.ToInt32(tblPmUserTODT["idPmUser"].ToString());
                    if (tblPmUserTODT["pmId"] != DBNull.Value)
                        tblPmUserTONew.PmId = Convert.ToInt32(tblPmUserTODT["pmId"].ToString());
                    if (tblPmUserTODT["userId"] != DBNull.Value)
                        tblPmUserTONew.UserId = Convert.ToInt32(tblPmUserTODT["userId"].ToString());
                    if (tblPmUserTODT["isActive"] != DBNull.Value)
                        tblPmUserTONew.IsActive = Convert.ToInt32(tblPmUserTODT["isActive"].ToString());
                    if (tblPmUserTODT["createdBy"] != DBNull.Value)
                        tblPmUserTONew.CreatedBy = Convert.ToInt32(tblPmUserTODT["createdBy"].ToString());
                    if (tblPmUserTODT["createdOn"] != DBNull.Value)
                        tblPmUserTONew.CreatedOn = Convert.ToDateTime(tblPmUserTODT["createdOn"].ToString());
                    if (tblPmUserTODT["updatedBy"] != DBNull.Value)
                        tblPmUserTONew.UpdatedBy = Convert.ToInt32(tblPmUserTODT["updatedBy"].ToString());
                    if (tblPmUserTODT["updatedOn"] != DBNull.Value)
                        tblPmUserTONew.UpdatedOn = Convert.ToDateTime(tblPmUserTODT["updatedOn"].ToString());
                    if (tblPmUserTODT["userDisplayName"] != DBNull.Value)
                        tblPmUserTONew.UserDisplayName = Convert.ToString(tblPmUserTODT["userDisplayName"].ToString());

                    tblPmUserTOList.Add(tblPmUserTONew);
                }
            }
            return tblPmUserTOList;
        }


        #endregion

        #region Insertion
        public static int InsertTblPmUser(TblPmUserTO tblPmUserTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPmUserTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPmUserTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblPmUserTO tblPmUserTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPmUser]( " +
            //"  [idPmUser]" +
            "  [pmId]" +
            " ,[userId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            //"  @IdPmUser " +
            "  @PmId " +
            " ,@UserId " +
            " ,@IsActive " +
            " ,@UpdatedBy " +
            " ,@UpdatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
            cmdInsert.Parameters.Add("@PmId", System.Data.SqlDbType.Int).Value = tblPmUserTO.PmId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPmUserTO.UserId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPmUserTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPmUserTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPmUserTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPmUserTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPmUserTO.UpdatedOn);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblPmUser(TblPmUserTO tblPmUserTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPmUserTO, cmdUpdate);
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

        public static int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPmUserTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblPmUserTO tblPmUserTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPmUser] SET " +
            //"  [idPmUser] = @IdPmUser" +
            " ,[pmId]= @PmId" +
            " ,[userId]= @UserId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn] = @CreatedOn" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE idPmUser = @IdPmUser ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
            cmdUpdate.Parameters.Add("@PmId", System.Data.SqlDbType.Int).Value = tblPmUserTO.PmId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPmUserTO.UserId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPmUserTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPmUserTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPmUserTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPmUserTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPmUserTO.UpdatedOn);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblPmUser(Int32 idPmUser)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPmUser, cmdDelete);
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

        public static int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPmUser, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idPmUser, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPmUser] " +
            " WHERE idPmUser = " + idPmUser + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPmUser", System.Data.SqlDbType.Int).Value = tblPmUserTO.IdPmUser;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
