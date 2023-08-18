using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblVersionDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVersion]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVersionTO> SelectAllTblVersion()
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

                //cmdSelect.Parameters.Add("@idVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
                sqlReader  = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVersionTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if(sqlReader != null)
                {
                    sqlReader.Dispose();
                }
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblVersionTO SelectTblVersion(Int32 idVersion)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVersion = " + idVersion +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = null;
                //cmdSelect.Parameters.Add("@idVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVersionTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
                

                //return list;
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
        public static TblVersionTO SelectLatestVersionTO()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 1 * FROM tblVersion ORDER BY createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = null;
                //cmdSelect.Parameters.Add("@idVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVersionTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;


                //return list;
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
        public static List<TblVersionTO> SelectAllTblVersion(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = null;

                //cmdSelect.Parameters.Add("@idVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
                sqlReader =cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVersionTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public static List<TblVersionTO> ConvertDTToList(SqlDataReader tblUserRoleTODT)
        {
            List<TblVersionTO> tblUserRoleTOList = new List<TblVersionTO>();
            if (tblUserRoleTODT != null)
            {
                while (tblUserRoleTODT.Read())
                {
                    TblVersionTO tblUserRoleTONew = new TblVersionTO();
                    if (tblUserRoleTODT["idVersion"] != DBNull.Value)
                        tblUserRoleTONew.IdVersion = Convert.ToInt32(tblUserRoleTODT["idVersion"].ToString());
                    if (tblUserRoleTODT["versionNo"] != DBNull.Value)
                        tblUserRoleTONew.VersionNo = Convert.ToString(tblUserRoleTODT["versionNo"].ToString());
                    if (tblUserRoleTODT["urlPath"] != DBNull.Value)
                        tblUserRoleTONew.UrlPath = Convert.ToString(tblUserRoleTODT["urlPath"].ToString());                  
                    if (tblUserRoleTODT["createdBy"] != DBNull.Value)
                        tblUserRoleTONew.CreatedBy = Convert.ToInt32(tblUserRoleTODT["createdBy"].ToString());
                    if (tblUserRoleTODT["createdOn"] != DBNull.Value)
                        tblUserRoleTONew.CreatedOn = Convert.ToDateTime(tblUserRoleTODT["createdOn"].ToString());
                    if (tblUserRoleTODT["verDesc"] != DBNull.Value)
                        tblUserRoleTONew.VerDesc = Convert.ToString(tblUserRoleTODT["verDesc"].ToString());                   

                    tblUserRoleTOList.Add(tblUserRoleTONew);
                }
            }
            return tblUserRoleTOList;
        }


        #endregion

        #region Insertion
        public static int InsertTblVersion(TblVersionTO tblVersionTO)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVersionTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVersionTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblVersionTO tblVersionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVersion]( " + 
            "  [idVersion]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[versionNo]" +
            " ,[verDesc]" +
            " ,[urlPath]" +
            " )" +
" VALUES (" +
            "  @IdVersion " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@VersionNo " +
            " ,@VerDesc " +
            " ,@UrlPath " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVersionTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVersionTO.CreatedOn;
            cmdInsert.Parameters.Add("@VersionNo", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.VersionNo;
            cmdInsert.Parameters.Add("@VerDesc", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.VerDesc;
            cmdInsert.Parameters.Add("@UrlPath", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.UrlPath;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblVersion(TblVersionTO tblVersionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVersionTO, cmdUpdate);
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

        public static int UpdateTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVersionTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblVersionTO tblVersionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVersion] SET " + 
            "  [idVersion] = @IdVersion" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[versionNo]= @VersionNo" +
            " ,[verDesc]= @VerDesc" +
            " ,[urlPath] = @UrlPath" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVersionTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVersionTO.CreatedOn;
            cmdUpdate.Parameters.Add("@VersionNo", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.VersionNo;
            cmdUpdate.Parameters.Add("@VerDesc", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.VerDesc;
            cmdUpdate.Parameters.Add("@UrlPath", System.Data.SqlDbType.NVarChar).Value = tblVersionTO.UrlPath;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblVersion(Int32 idVersion)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVersion, cmdDelete);
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

        public static int DeleteTblVersion(Int32 idVersion, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVersion, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idVersion, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVersion] " +
            " WHERE idVersion = " + idVersion +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVersion", System.Data.SqlDbType.Int).Value = tblVersionTO.IdVersion;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
