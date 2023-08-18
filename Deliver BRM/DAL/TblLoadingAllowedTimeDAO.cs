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
    public class TblLoadingAllowedTimeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblLoadingAllowedTime]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingAllowedTimeTO> SelectAllTblLoadingAllowedTime()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingAllowedTimeTO SelectTblLoadingAllowedTime(Int32 idLoadingTime)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLoadingTime = " + idLoadingTime + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingAllowedTimeTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblLoadingAllowedTimeTO SelectTblLoadingAllowedTime(DateTime date)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLoadingTime = (SELECT MAX(idLoadingTime) FROM tblLoadingAllowedTime WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingAllowedTimeTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblLoadingAllowedTimeTO> ConvertDTToList(SqlDataReader tblLoadingAllowedTimeTODT)
        {
            List<TblLoadingAllowedTimeTO> tblLoadingAllowedTimeTOList = new List<TblLoadingAllowedTimeTO>();
            if (tblLoadingAllowedTimeTODT != null)
            {
                while (tblLoadingAllowedTimeTODT.Read())
                {
                    TblLoadingAllowedTimeTO tblLoadingAllowedTimeTONew = new TblLoadingAllowedTimeTO();
                    if (tblLoadingAllowedTimeTODT["idLoadingTime"] != DBNull.Value)
                        tblLoadingAllowedTimeTONew.IdLoadingTime = Convert.ToInt32(tblLoadingAllowedTimeTODT["idLoadingTime"].ToString());
                    if (tblLoadingAllowedTimeTODT["createdBy"] != DBNull.Value)
                        tblLoadingAllowedTimeTONew.CreatedBy = Convert.ToInt32(tblLoadingAllowedTimeTODT["createdBy"].ToString());
                    if (tblLoadingAllowedTimeTODT["allowedLoadingTime"] != DBNull.Value)
                        tblLoadingAllowedTimeTONew.AllowedLoadingTime = Convert.ToDateTime(tblLoadingAllowedTimeTODT["allowedLoadingTime"].ToString());
                    if (tblLoadingAllowedTimeTODT["createdOn"] != DBNull.Value)
                        tblLoadingAllowedTimeTONew.CreatedOn = Convert.ToDateTime(tblLoadingAllowedTimeTODT["createdOn"].ToString());
                    tblLoadingAllowedTimeTOList.Add(tblLoadingAllowedTimeTONew);
                }
            }
            return tblLoadingAllowedTimeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingAllowedTimeTO, cmdInsert);
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

        public static int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingAllowedTimeTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingAllowedTime]( " + 
                                "  [createdBy]" +
                                " ,[allowedLoadingTime]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                "  @CreatedBy " +
                                " ,@AllowedLoadingTime " +
                                " ,@CreatedOn " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingTime", System.Data.SqlDbType.Int).Value = tblLoadingAllowedTimeTO.IdLoadingTime;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingAllowedTimeTO.CreatedBy;
            cmdInsert.Parameters.Add("@AllowedLoadingTime", System.Data.SqlDbType.DateTime).Value = tblLoadingAllowedTimeTO.AllowedLoadingTime;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingAllowedTimeTO.CreatedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingAllowedTimeTO.IdLoadingTime = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingAllowedTimeTO, cmdUpdate);
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

        public static int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingAllowedTimeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingAllowedTime] SET " + 
            "  [idLoadingTime] = @IdLoadingTime" +
            " ,[createdBy]= @CreatedBy" +
            " ,[allowedLoadingTime]= @AllowedLoadingTime" +
            " ,[createdOn] = @CreatedOn" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingTime", System.Data.SqlDbType.Int).Value = tblLoadingAllowedTimeTO.IdLoadingTime;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingAllowedTimeTO.CreatedBy;
            cmdUpdate.Parameters.Add("@AllowedLoadingTime", System.Data.SqlDbType.DateTime).Value = tblLoadingAllowedTimeTO.AllowedLoadingTime;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingAllowedTimeTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingAllowedTime(Int32 idLoadingTime)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingTime, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblLoadingAllowedTime(Int32 idLoadingTime, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingTime, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idLoadingTime, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tblLoadingAllowedTime] " +
                                    " WHERE idLoadingTime = " + idLoadingTime +"";

            cmdDelete.CommandType = System.Data.CommandType.Text;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion
        
    }
}
