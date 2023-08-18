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
    public class TblUserVerDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUserVer]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblUserVerTO> SelectAllTblUserVer()
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
                SqlDataReader sdr = null;
                //cmdSelect.Parameters.Add("@idUserVer", System.Data.SqlDbType.Int).Value = tblUserVerTO.IdUserVer;
                //DataTable dt = new DataTable();
                //da.Fill(dt);
                List<TblUserVerTO> tblUserVerToList = new List <TblUserVerTO>();
                sdr.Dispose();
                return tblUserVerToList;
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

        public static List<TblUserVerTO> SelectTblUserVer(Int32 idUserVer)
        {
            String sqlConnStr = Startup.ConnectionString; 
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                TblUserVerTO tblUserVerTO = new TblUserVerTO();
                List<TblUserVerTO> tblUserVerTOList = new List<TblUserVerTO>();
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idUserVer = " + idUserVer +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sdr = null;
                cmdSelect.Parameters.Add("@idUserVer", System.Data.SqlDbType.Int).Value = tblUserVerTO.IdUserVer;
                sdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return tblUserVerTOList;
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

      

        #endregion
        
        #region Insertion
        public static int InsertTblUserVer(TblUserVerTO tblUserVerTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserVerTO, cmdInsert);
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

        public static int InsertTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserVerTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblUserVerTO tblUserVerTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserVer]( " + 
            //"  [idUserVer]" +
            " [versionId]" +
            " ,[userId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " )" +
" VALUES (" +
            //"  @IdUserVer " +
            " @VersionId " +
            " ,@UserId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdUserVer", System.Data.SqlDbType.Int).Value = tblUserVerTO.IdUserVer;
            cmdInsert.Parameters.Add("@VersionId", System.Data.SqlDbType.Int).Value = tblUserVerTO.VersionId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserVerTO.UserId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserVerTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserVerTO.CreatedOn;
            //Assif
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblUserVerTO.IdUserVer = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblUserVer(TblUserVerTO tblUserVerTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserVerTO, cmdUpdate);
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

        public static int UpdateTblUserVer(TblUserVerTO tblUserVerTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserVerTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblUserVerTO tblUserVerTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserVer] SET " + 
            "  [idUserVer] = @IdUserVer" +
            " ,[versionId]= @VersionId" +
            " ,[userId]= @UserId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn] = @CreatedOn" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserVer", System.Data.SqlDbType.Int).Value = tblUserVerTO.IdUserVer;
            cmdUpdate.Parameters.Add("@VersionId", System.Data.SqlDbType.Int).Value = tblUserVerTO.VersionId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserVerTO.UserId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserVerTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserVerTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblUserVer(Int32 idUserVer)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUserVer, cmdDelete);
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

        public static int DeleteTblUserVer(Int32 idUserVer, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUserVer, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idUserVer, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserVer] " +
            " WHERE idUserVer = " + idUserVer +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUserVer", System.Data.SqlDbType.Int).Value = tblUserVerTO.IdUserVer;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
