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
    public class TblBookingQtyConsumptionDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblBookingQtyConsumption]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumption()
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

        public static List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumption(DateTime asOnDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
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
        public static List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumption(String bookingIdStr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId IN("+ bookingIdStr + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
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
        public static TblBookingQtyConsumptionTO SelectTblBookingQtyConsumption(Int32 idBookQtyConsuption)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBookQtyConsuption = " + idBookQtyConsuption +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingQtyConsumptionTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingQtyConsumptionTO> ConvertDTToList(SqlDataReader tblBookingQtyConsumptionTODT)
        {
            List<TblBookingQtyConsumptionTO> tblBookingQtyConsumptionTOList = new List<TblBookingQtyConsumptionTO>();
            if (tblBookingQtyConsumptionTODT != null)
            {
                while (tblBookingQtyConsumptionTODT.Read())
                {
                    TblBookingQtyConsumptionTO tblBookingQtyConsumptionTONew = new TblBookingQtyConsumptionTO();
                    if (tblBookingQtyConsumptionTODT["idBookQtyConsuption"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.IdBookQtyConsuption = Convert.ToInt32(tblBookingQtyConsumptionTODT["idBookQtyConsuption"].ToString());
                    if (tblBookingQtyConsumptionTODT["bookingId"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.BookingId = Convert.ToInt32(tblBookingQtyConsumptionTODT["bookingId"].ToString());
                    if (tblBookingQtyConsumptionTODT["statusId"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.StatusId = Convert.ToInt32(tblBookingQtyConsumptionTODT["statusId"].ToString());
                    if (tblBookingQtyConsumptionTODT["createdBy"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.CreatedBy = Convert.ToInt32(tblBookingQtyConsumptionTODT["createdBy"].ToString());
                    if (tblBookingQtyConsumptionTODT["createdOn"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.CreatedOn = Convert.ToDateTime(tblBookingQtyConsumptionTODT["createdOn"].ToString());
                    if (tblBookingQtyConsumptionTODT["consumptionQty"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.ConsumptionQty = Convert.ToDouble(tblBookingQtyConsumptionTODT["consumptionQty"].ToString());
                    if (tblBookingQtyConsumptionTODT["weightTolerance"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.WeightTolerance = Convert.ToString(tblBookingQtyConsumptionTODT["weightTolerance"].ToString());
                    if (tblBookingQtyConsumptionTODT["remark"] != DBNull.Value)
                        tblBookingQtyConsumptionTONew.Remark = Convert.ToString(tblBookingQtyConsumptionTODT["remark"].ToString());
                    tblBookingQtyConsumptionTOList.Add(tblBookingQtyConsumptionTONew);
                }
            }
            return tblBookingQtyConsumptionTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingQtyConsumptionTO, cmdInsert);
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

        public static int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingQtyConsumptionTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingQtyConsumption]( " + 
                                "  [bookingId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[consumptionQty]" +
                                " ,[weightTolerance]" +
                                " ,[remark]" +
                                " )" +
                    " VALUES (" +
                                "  @BookingId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@ConsumptionQty " +
                                " ,@WeightTolerance " +
                                " ,@Remark " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBookQtyConsuption", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.IdBookQtyConsuption;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.BookingId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingQtyConsumptionTO.CreatedOn;
            cmdInsert.Parameters.Add("@ConsumptionQty", System.Data.SqlDbType.NVarChar).Value = tblBookingQtyConsumptionTO.ConsumptionQty;
            cmdInsert.Parameters.Add("@WeightTolerance", System.Data.SqlDbType.NVarChar,50).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingQtyConsumptionTO.WeightTolerance);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar,50).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingQtyConsumptionTO.Remark);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingQtyConsumptionTO.IdBookQtyConsuption = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingQtyConsumptionTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return-1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingQtyConsumptionTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingQtyConsumption] SET " + 
                            "  [bookingId]= @BookingId" +
                            " ,[statusId]= @StatusId" +
                            " ,[consumptionQty]= @ConsumptionQty" +
                            " ,[weightTolerance]= @WeightTolerance" +
                            " ,[remark] = @Remark" +
                            " WHERE [idBookQtyConsuption] = @IdBookQtyConsuption"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookQtyConsuption", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.IdBookQtyConsuption;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.BookingId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.StatusId;
            cmdUpdate.Parameters.Add("@ConsumptionQty", System.Data.SqlDbType.NVarChar).Value = tblBookingQtyConsumptionTO.ConsumptionQty;
            cmdUpdate.Parameters.Add("@WeightTolerance", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingQtyConsumptionTO.WeightTolerance);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingQtyConsumptionTO.Remark);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookQtyConsuption, cmdDelete);
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

        public static int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookQtyConsuption, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBookQtyConsuption, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingQtyConsumption] " +
            " WHERE idBookQtyConsuption = " + idBookQtyConsuption +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookQtyConsuption", System.Data.SqlDbType.Int).Value = tblBookingQtyConsumptionTO.IdBookQtyConsuption;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
