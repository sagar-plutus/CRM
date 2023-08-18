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
    public class TblBookingOpngBalDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblBookingOpngBal]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection


        public static List<TblBookingOpngBalTO> SelectAllTblBookingOpngBal(DateTime asOnDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingOpngBalTO> list = ConvertDTToList(sqlReader);
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

        public static TblBookingOpngBalTO SelectTblBookingOpngBal(Int32 idOpeningBal)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOpeningBal = " + idOpeningBal + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingOpngBalTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
                    return list[0];

                return null;
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

        public static List<TblBookingOpngBalTO> ConvertDTToList(SqlDataReader tblBookingOpngBalTODT)
        {
            List<TblBookingOpngBalTO> tblBookingOpngBalTOList = new List<TblBookingOpngBalTO>();
            if (tblBookingOpngBalTODT != null)
            {
                while (tblBookingOpngBalTODT.Read())
                {
                    TblBookingOpngBalTO tblBookingOpngBalTONew = new TblBookingOpngBalTO();
                    if (tblBookingOpngBalTODT["idOpeningBal"] != DBNull.Value)
                        tblBookingOpngBalTONew.IdOpeningBal = Convert.ToInt32(tblBookingOpngBalTODT["idOpeningBal"].ToString());
                    if (tblBookingOpngBalTODT["bookingId"] != DBNull.Value)
                        tblBookingOpngBalTONew.BookingId = Convert.ToInt32(tblBookingOpngBalTODT["bookingId"].ToString());
                    if (tblBookingOpngBalTODT["balAsOnDate"] != DBNull.Value)
                        tblBookingOpngBalTONew.BalAsOnDate = Convert.ToDateTime(tblBookingOpngBalTODT["balAsOnDate"].ToString());
                    if (tblBookingOpngBalTODT["openingBalQty"] != DBNull.Value)
                        tblBookingOpngBalTONew.OpeningBalQty = Convert.ToDouble(tblBookingOpngBalTODT["openingBalQty"].ToString());
                    tblBookingOpngBalTOList.Add(tblBookingOpngBalTONew);
                }
            }
            return tblBookingOpngBalTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingOpngBalTO, cmdInsert);
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

        public static int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingOpngBalTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingOpngBalTO tblBookingOpngBalTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingOpngBal]( " +
                                "  [bookingId]" +
                                " ,[balAsOnDate]" +
                                " ,[openingBalQty]" +
                                " )" +
                    " VALUES (" +
                                "  @BookingId " +
                                " ,@BalAsOnDate " +
                                " ,@OpeningBalQty " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOpeningBal", System.Data.SqlDbType.Int).Value = tblBookingOpngBalTO.IdOpeningBal;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingOpngBalTO.BookingId;
            cmdInsert.Parameters.Add("@BalAsOnDate", System.Data.SqlDbType.DateTime).Value = tblBookingOpngBalTO.BalAsOnDate;
            cmdInsert.Parameters.Add("@OpeningBalQty", System.Data.SqlDbType.NVarChar).Value = tblBookingOpngBalTO.OpeningBalQty;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingOpngBalTO.IdOpeningBal = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingOpngBalTO, cmdUpdate);
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

        public static int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingOpngBalTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingOpngBalTO tblBookingOpngBalTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingOpngBal] SET " +
                                "  [bookingId]= @BookingId" +
                                " ,[balAsOnDate]= @BalAsOnDate" +
                                " ,[openingBalQty] = @OpeningBalQty" +
                                " WHERE  [idOpeningBal] = @IdOpeningBal ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOpeningBal", System.Data.SqlDbType.Int).Value = tblBookingOpngBalTO.IdOpeningBal;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingOpngBalTO.BookingId;
            cmdUpdate.Parameters.Add("@BalAsOnDate", System.Data.SqlDbType.DateTime).Value = tblBookingOpngBalTO.BalAsOnDate;
            cmdUpdate.Parameters.Add("@OpeningBalQty", System.Data.SqlDbType.NVarChar).Value = tblBookingOpngBalTO.OpeningBalQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblBookingOpngBal(Int32 idOpeningBal)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOpeningBal, cmdDelete);
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

        public static int DeleteTblBookingOpngBal(Int32 idOpeningBal, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOpeningBal, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idOpeningBal, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingOpngBal] " +
            " WHERE idOpeningBal = " + idOpeningBal + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOpeningBal", System.Data.SqlDbType.Int).Value = tblBookingOpngBalTO.IdOpeningBal;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
