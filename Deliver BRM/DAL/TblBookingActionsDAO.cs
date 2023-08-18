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
    public class TblBookingActionsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblBookingActions]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingActionsTO> SelectAllTblBookingActions()
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
                List<TblBookingActionsTO> list = ConvertDTToList(sqlReader);
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

        public static TblBookingActionsTO SelectTblBookingActions(Int32 idBookingAction)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBookingAction = " + idBookingAction + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingActionsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = "SELECT TOP 1 * FROM tblBookingActions ORDER BY statusDate DESC";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingActionsTO> list = ConvertDTToList(reader);
                reader.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingActionsTO> ConvertDTToList(SqlDataReader tblBookingActionsTODT)
        {
            List<TblBookingActionsTO> tblBookingActionsTOList = new List<TblBookingActionsTO>();
            if (tblBookingActionsTODT != null)
            {
                while (tblBookingActionsTODT.Read())
                {
                    TblBookingActionsTO tblBookingActionsTONew = new TblBookingActionsTO();
                    if (tblBookingActionsTODT["idBookingAction"] != DBNull.Value)
                        tblBookingActionsTONew.IdBookingAction = Convert.ToInt32(tblBookingActionsTODT["idBookingAction"].ToString());
                    if (tblBookingActionsTODT["isAuto"] != DBNull.Value)
                        tblBookingActionsTONew.IsAuto = Convert.ToInt32(tblBookingActionsTODT["isAuto"].ToString());
                    if (tblBookingActionsTODT["statusBy"] != DBNull.Value)
                        tblBookingActionsTONew.StatusBy = Convert.ToInt32(tblBookingActionsTODT["statusBy"].ToString());
                    if (tblBookingActionsTODT["statusDate"] != DBNull.Value)
                        tblBookingActionsTONew.StatusDate = Convert.ToDateTime(tblBookingActionsTODT["statusDate"].ToString());
                    if (tblBookingActionsTODT["bookingStatus"] != DBNull.Value)
                        tblBookingActionsTONew.BookingStatus = Convert.ToString(tblBookingActionsTODT["bookingStatus"].ToString());
                    tblBookingActionsTOList.Add(tblBookingActionsTONew);
                }
            }
            return tblBookingActionsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingActionsTO, cmdInsert);
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

        public static int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingActionsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingActions]( " + 
                                "  [isAuto]" +
                                " ,[statusBy]" +
                                " ,[statusDate]" +
                                " ,[bookingStatus]" +
                                " )" +
                    " VALUES (" +
                                "  @IsAuto " +
                                " ,@StatusBy " +
                                " ,@StatusDate " +
                                " ,@BookingStatus " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBookingAction", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IdBookingAction;
            cmdInsert.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IsAuto;
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.StatusBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingActionsTO.StatusDate;
            cmdInsert.Parameters.Add("@BookingStatus", System.Data.SqlDbType.VarChar).Value = tblBookingActionsTO.BookingStatus;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingActionsTO.IdBookingAction = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingActionsTO, cmdUpdate);
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

        public static int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingActionsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingActions] SET " + 
            "  [idBookingAction] = @IdBookingAction" +
            " ,[isAuto]= @IsAuto" +
            " ,[statusBy]= @StatusBy" +
            " ,[statusDate]= @StatusDate" +
            " ,[bookingStatus] = @BookingStatus" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingAction", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IdBookingAction;
            cmdUpdate.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IsAuto;
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.StatusBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingActionsTO.StatusDate;
            cmdUpdate.Parameters.Add("@BookingStatus", System.Data.SqlDbType.VarChar).Value = tblBookingActionsTO.BookingStatus;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblBookingActions(Int32 idBookingAction)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingAction, cmdDelete);
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

        public static int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingAction, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBookingAction, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingActions] " +
            " WHERE idBookingAction = " + idBookingAction +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingAction", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IdBookingAction;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
