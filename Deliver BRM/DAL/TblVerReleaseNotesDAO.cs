using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using TO;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblVerReleaseNotesDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVerReleaseNotes]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes()
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
                //cmdSelect.Parameters.Add("@idReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVerReleaseNotesTO> tblUserVerTOList = new List<TblVerReleaseNotesTO>();
                tblUserVerTOList = ConvertDTToList(sqlReader);
                return tblUserVerTOList;
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

        public static List<TblVerReleaseNotesTO> SelectTblVerReleaseNotes(Int32 idReleaseNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReleaseNote = " + idReleaseNote +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //cmdSelect.Parameters.Add("@idReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
                List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = new List<TblVerReleaseNotesTO>();
                tblVerReleaseNotesTOList = ConvertDTToList(sqlReader);
                return tblVerReleaseNotesTOList;
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

        public static List<TblVerReleaseNotesTO> SelectTblVerReleaseNotesByVerId(Int32 idVersion)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE versionId = " + idVersion + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //cmdSelect.Parameters.Add("@idReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
                List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = new List<TblVerReleaseNotesTO>();
                tblVerReleaseNotesTOList = ConvertDTToList(sqlReader);
                return tblVerReleaseNotesTOList;
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

        public static List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = null;
                //cmdSelect.Parameters.Add("@idReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
                List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = new List<TblVerReleaseNotesTO>();
                return tblVerReleaseNotesTOList;
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
        public static List<TblVerReleaseNotesTO> ConvertDTToList(SqlDataReader tblVerReleaseNotesTODT)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = new List<TblVerReleaseNotesTO>();
            if (tblVerReleaseNotesTODT != null)
            {
                while (tblVerReleaseNotesTODT.Read())
                {
                    TblVerReleaseNotesTO tblVerReleaseNotesTONew = new TblVerReleaseNotesTO();
                    if (tblVerReleaseNotesTODT["idReleaseNote"] != DBNull.Value)
                        tblVerReleaseNotesTONew.IdReleaseNote = Convert.ToInt32(tblVerReleaseNotesTODT["idReleaseNote"].ToString());
                    if (tblVerReleaseNotesTODT["versionId"] != DBNull.Value)
                        tblVerReleaseNotesTONew.VersionId = Convert.ToInt32(tblVerReleaseNotesTODT["versionId"].ToString());
                    if (tblVerReleaseNotesTODT["createdBy"] != DBNull.Value)
                        tblVerReleaseNotesTONew.CreatedBy = Convert.ToInt32(tblVerReleaseNotesTODT["createdBy"].ToString());
                    if (tblVerReleaseNotesTODT["createdOn"] != DBNull.Value)
                        tblVerReleaseNotesTONew.CreatedOn = Convert.ToDateTime(tblVerReleaseNotesTODT["createdOn"].ToString());
                    if (tblVerReleaseNotesTODT["noteDesc"] != DBNull.Value)
                        tblVerReleaseNotesTONew.NoteDesc = Convert.ToString(tblVerReleaseNotesTODT["noteDesc"].ToString());
                    tblVerReleaseNotesTOList.Add(tblVerReleaseNotesTONew);
                }
            }
            return tblVerReleaseNotesTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVerReleaseNotesTO, cmdInsert);
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

        public static int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVerReleaseNotesTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVerReleaseNotes]( " + 
            "  [idReleaseNote]" +
            " ,[versionId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[noteDesc]" +
            " )" +
" VALUES (" +
            "  @IdReleaseNote " +
            " ,@VersionId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@NoteDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
            cmdInsert.Parameters.Add("@VersionId", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.VersionId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVerReleaseNotesTO.CreatedOn;
            cmdInsert.Parameters.Add("@NoteDesc", System.Data.SqlDbType.NVarChar).Value = tblVerReleaseNotesTO.NoteDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVerReleaseNotesTO, cmdUpdate);
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

        public static int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVerReleaseNotesTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVerReleaseNotes] SET " + 
            "  [idReleaseNote] = @IdReleaseNote" +
            " ,[versionId]= @VersionId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[noteDesc] = @NoteDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
            cmdUpdate.Parameters.Add("@VersionId", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.VersionId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVerReleaseNotesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@NoteDesc", System.Data.SqlDbType.NVarChar).Value = tblVerReleaseNotesTO.NoteDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblVerReleaseNotes(Int32 idReleaseNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReleaseNote, cmdDelete);
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

        public static int DeleteTblVerReleaseNotes(Int32 idReleaseNote, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReleaseNote, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idReleaseNote, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVerReleaseNotes] " +
            " WHERE idReleaseNote = " + idReleaseNote +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReleaseNote", System.Data.SqlDbType.Int).Value = tblVerReleaseNotesTO.IdReleaseNote;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
