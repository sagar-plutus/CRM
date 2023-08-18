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
    public class TblStatusReasonDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblStatusReason]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblStatusReasonTO> SelectAllTblStatusReason()
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
                List<TblStatusReasonTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblStatusReasonTO> SelectAllTblStatusReason(int statusId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (statusId == 0)
                    cmdSelect.CommandText = SqlSelectQuery();
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE statusId=" + statusId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStatusReasonTO> list = ConvertDTToList(sqlReader);
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

        public static TblStatusReasonTO SelectTblStatusReason(Int32 idStatusReason)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStatusReason = " + idStatusReason +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStatusReasonTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblStatusReasonTO> ConvertDTToList(SqlDataReader tblStatusReasonTODT)
        {
            List<TblStatusReasonTO> tblStatusReasonTOList = new List<TblStatusReasonTO>();
            if (tblStatusReasonTODT != null)
            {
                while(tblStatusReasonTODT.Read())
                {
                    TblStatusReasonTO tblStatusReasonTONew = new TblStatusReasonTO();
                    if (tblStatusReasonTODT["idStatusReason"] != DBNull.Value)
                        tblStatusReasonTONew.IdStatusReason = Convert.ToInt32(tblStatusReasonTODT["idStatusReason"].ToString());
                    if (tblStatusReasonTODT["statusId"] != DBNull.Value)
                        tblStatusReasonTONew.StatusId = Convert.ToInt32(tblStatusReasonTODT["statusId"].ToString());
                    if (tblStatusReasonTODT["createdOn"] != DBNull.Value)
                        tblStatusReasonTONew.CreatedOn = Convert.ToDateTime(tblStatusReasonTODT["createdOn"].ToString());
                    if (tblStatusReasonTODT["reasonDesc"] != DBNull.Value)
                        tblStatusReasonTONew.ReasonDesc = Convert.ToString(tblStatusReasonTODT["reasonDesc"].ToString());
                    tblStatusReasonTOList.Add(tblStatusReasonTONew);
                }
            }
            return tblStatusReasonTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStatusReasonTO, cmdInsert);
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

        public static int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStatusReasonTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblStatusReasonTO tblStatusReasonTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStatusReason]( " +
                            "  [statusId]" +
                            " ,[createdOn]" +
                            " ,[reasonDesc]" +
                            " )" +
                " VALUES (" +
                            "  @StatusId " +
                            " ,@CreatedOn " +
                            " ,@ReasonDesc " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStatusReason", System.Data.SqlDbType.Int).Value = tblStatusReasonTO.IdStatusReason;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblStatusReasonTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStatusReasonTO.CreatedOn;
            cmdInsert.Parameters.Add("@ReasonDesc", System.Data.SqlDbType.VarChar).Value = tblStatusReasonTO.ReasonDesc;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblStatusReasonTO.IdStatusReason = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStatusReasonTO, cmdUpdate);
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

        public static int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStatusReasonTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblStatusReasonTO tblStatusReasonTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStatusReason] SET " + 
            "  [idStatusReason] = @IdStatusReason" +
            " ,[statusId]= @StatusId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[reasonDesc] = @ReasonDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStatusReason", System.Data.SqlDbType.Int).Value = tblStatusReasonTO.IdStatusReason;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblStatusReasonTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStatusReasonTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ReasonDesc", System.Data.SqlDbType.VarChar).Value = tblStatusReasonTO.ReasonDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblStatusReason(Int32 idStatusReason)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStatusReason, cmdDelete);
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

        public static int DeleteTblStatusReason(Int32 idStatusReason, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStatusReason, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idStatusReason, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStatusReason] " +
            " WHERE idStatusReason = " + idStatusReason +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStatusReason", System.Data.SqlDbType.Int).Value = tblStatusReasonTO.IdStatusReason;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
