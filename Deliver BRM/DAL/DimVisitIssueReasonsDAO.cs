using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class DimVisitIssueReasonsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimVisitIssueReasons]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllDimVisitIssueReasons()
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

                return null;
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

        public static DataTable SelectDimVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisitIssueReasons = " + idVisitIssueReasons + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        public static DataTable SelectAllDimVisitIssueReasons(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;         
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimVisitIssueReasonsTO, cmdInsert);
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

        public static int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimVisitIssueReasonsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimVisitIssueReasons]( " +
            "  [idVisitIssueReasons]" +
            " ,[isActive]" +
            " ,[visitIssueReasonName]" +
            " ,[visitIssueReasonDesc]" +
            " )" +
" VALUES (" +
            "  @IdVisitIssueReasons " +
            " ,@IsActive " +
            " ,@VisitIssueReasonName " +
            " ,@VisitIssueReasonDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVisitIssueReasons", System.Data.SqlDbType.Int).Value = dimVisitIssueReasonsTO.IdVisitIssueReasons;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimVisitIssueReasonsTO.IsActive;
            cmdInsert.Parameters.Add("@VisitIssueReasonName", System.Data.SqlDbType.NVarChar).Value = dimVisitIssueReasonsTO.VisitIssueReasonName;
            cmdInsert.Parameters.Add("@VisitIssueReasonDesc", System.Data.SqlDbType.NVarChar).Value = dimVisitIssueReasonsTO.VisitIssueReasonDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimVisitIssueReasonsTO, cmdUpdate);
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

        public static int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimVisitIssueReasonsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimVisitIssueReasons] SET " +
            "  [idVisitIssueReasons] = @IdVisitIssueReasons" +
            " ,[isActive]= @IsActive" +
            " ,[visitIssueReasonName]= @VisitIssueReasonName" +
            " ,[visitIssueReasonDesc] = @VisitIssueReasonDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisitIssueReasons", System.Data.SqlDbType.Int).Value = dimVisitIssueReasonsTO.IdVisitIssueReasons;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimVisitIssueReasonsTO.IsActive;
            cmdUpdate.Parameters.Add("@VisitIssueReasonName", System.Data.SqlDbType.NVarChar).Value = dimVisitIssueReasonsTO.VisitIssueReasonName;
            cmdUpdate.Parameters.Add("@VisitIssueReasonDesc", System.Data.SqlDbType.NVarChar).Value = dimVisitIssueReasonsTO.VisitIssueReasonDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisitIssueReasons, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisitIssueReasons, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idVisitIssueReasons, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimVisitIssueReasons] " +
            " WHERE idVisitIssueReasons = " + idVisitIssueReasons + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisitIssueReasons", System.Data.SqlDbType.Int).Value = dimVisitIssueReasonsTO.IdVisitIssueReasons;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
