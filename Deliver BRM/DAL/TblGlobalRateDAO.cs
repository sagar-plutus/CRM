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
    public class TblGlobalRateDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT rate.*,bookings.*,reasonDtl.reasonDesc FROM tblGlobalRate rate " +
                                  " LEFT JOIN " +
                                  " ( " +
                                  "     SELECT globalRateId, SUM(bookingQty)qty, (SUM(bookingQty * bookingRate) / SUM(bookingQty)) avgPrice " +
                                  "      FROM tblbookings WHERE statusId IN(2,3,9,11) " +
                                  "      GROUP BY globalRateId " +
                                  "  ) bookings " +
                                  "  ON rate.idGlobalRate = bookings.globalRateId " +
                                  " LEFT JOIN tblRateDeclareReasons reasonDtl" +
                                  " ON rate.rateReasonId=reasonDtl.idRateReason";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
       
        public static TblGlobalRateTO SelectTblGlobalRate(Int32 idGlobalRate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGlobalRate = " + idGlobalRate +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblGlobalRateTO> list = ConvertDTToList(tblGlobalRateTODT);
                tblGlobalRateTODT.Dispose();
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

        public static TblGlobalRateTO SelectTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGlobalRate = " + idGlobalRate + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblGlobalRateTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static TblGlobalRateTO SelectLatestTblGlobalRateTO(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGlobalRateTODT = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGlobalRate IN(SELECT TOP 1 idGlobalRate FROM [tblGlobalRate] ORDER BY createdOn DESC) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRateTO> list = ConvertDTToList(tblGlobalRateTODT);
                tblGlobalRateTODT.Dispose();
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
                if (tblGlobalRateTODT != null)
                    tblGlobalRateTODT.Dispose();
                cmdSelect.Dispose();
            }
        }
        //Aniket
        public static List<GlobalRateTOFroGraph> SelectRateForGraph(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                
               cmdSelect.CommandText = "select rate,createdOn,'' as firmName from tblGlobalRate "+
                    " where cast(createdOn as date) between @fromDate and @toDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;

                SqlDataReader sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<GlobalRateTOFroGraph> list = ConvertDTToListForGraph(sqlDataReader);
                sqlDataReader.Dispose();
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

        public static List<TblGlobalRateTO> SelectLatestTblGlobalRateTOList(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE CONVERT (DATE,rate.createdOn,103)   BETWEEN @fromDate AND @toDate ORDER BY rate.createdOn DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date.ToString(Constants.AzureDateFormat); 

                SqlDataReader sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGlobalRateTO> list= ConvertDTToList(sqlDataReader);
                sqlDataReader.Dispose();
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

        public static Boolean IsRateAlreadyDeclaredForTheDate(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGlobalRateTODT = null;
            try
            {
                cmdSelect.CommandText = "SELECT COUNT(*) AS todayCount FROM tblGlobalRate " + " WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (tblGlobalRateTODT != null)
                {
                    while (tblGlobalRateTODT.Read())
                    {
                        TblGlobalRateTO tblGlobalRateTONew = new TblGlobalRateTO();
                        if (tblGlobalRateTODT[0] != DBNull.Value)
                        {
                            if (Convert.ToInt32(tblGlobalRateTODT[0]) > 0)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
            finally
            {
                if (tblGlobalRateTODT != null)
                    tblGlobalRateTODT.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static SqlDataReader SelectAllTblGlobalRate(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return rdr;

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
        public static List<GlobalRateTOFroGraph> ConvertDTToListForGraph(SqlDataReader tblGlobalRateTODT)
        {
            List<GlobalRateTOFroGraph> tblGlobalRateTOList = new List<GlobalRateTOFroGraph>();
            if (tblGlobalRateTODT != null)
            {
                while (tblGlobalRateTODT.Read())
                {
                    GlobalRateTOFroGraph tblGlobalRateTONew = new GlobalRateTOFroGraph();
                    if (tblGlobalRateTODT["createdOn"] != DBNull.Value)
                        tblGlobalRateTONew.CreatedOn = Convert.ToDateTime(tblGlobalRateTODT["createdOn"]);
                    tblGlobalRateTONew.CreatedOn = Convert.ToDateTime(tblGlobalRateTODT["createdOn"]);
                    if (tblGlobalRateTODT["rate"] != DBNull.Value)
                        tblGlobalRateTONew.Rate = Convert.ToDouble(tblGlobalRateTODT["rate"]);
                    if (tblGlobalRateTODT["firmName"] != DBNull.Value)
                        tblGlobalRateTONew.FirmName = Convert.ToString(tblGlobalRateTODT["firmName"]);


                    tblGlobalRateTOList.Add(tblGlobalRateTONew);
                }
            }
            return tblGlobalRateTOList;
        }
            public static List<TblGlobalRateTO> ConvertDTToList(SqlDataReader tblGlobalRateTODT)
           {
            List<TblGlobalRateTO> tblGlobalRateTOList = new List<TblGlobalRateTO>();
            if (tblGlobalRateTODT != null)
            {
                while (tblGlobalRateTODT.Read())
                {
                    TblGlobalRateTO tblGlobalRateTONew = new TblGlobalRateTO();
                    if (tblGlobalRateTODT["idGlobalRate"] != DBNull.Value)
                        tblGlobalRateTONew.IdGlobalRate = Convert.ToInt32(tblGlobalRateTODT["idGlobalRate"]);
                    if (tblGlobalRateTODT["createdBy"] != DBNull.Value)
                        tblGlobalRateTONew.CreatedBy = Convert.ToInt32(tblGlobalRateTODT["createdBy"]);
                    if (tblGlobalRateTODT["createdOn"] != DBNull.Value)
                        tblGlobalRateTONew.CreatedOn = Convert.ToDateTime(tblGlobalRateTODT["createdOn"]);
                    if (tblGlobalRateTODT["rate"] != DBNull.Value)
                        tblGlobalRateTONew.Rate = Convert.ToDouble(tblGlobalRateTODT["rate"]);
                    if (tblGlobalRateTODT["comments"] != DBNull.Value)
                        tblGlobalRateTONew.Comments = Convert.ToString(tblGlobalRateTODT["comments"]);

                    if (tblGlobalRateTODT["qty"] != DBNull.Value)
                        tblGlobalRateTONew.Quantity = Convert.ToDouble(tblGlobalRateTODT["qty"]);
                    if (tblGlobalRateTODT["avgPrice"] != DBNull.Value)
                        tblGlobalRateTONew.AvgPrice = Convert.ToDouble(tblGlobalRateTODT["avgPrice"]);

                    if (tblGlobalRateTODT["rateReasonId"] != DBNull.Value)
                        tblGlobalRateTONew.RateReasonId = Convert.ToInt32(tblGlobalRateTODT["rateReasonId"]);
                    if (tblGlobalRateTODT["reasonDesc"] != DBNull.Value)
                        tblGlobalRateTONew.RateReasonDesc = Convert.ToString(tblGlobalRateTODT["reasonDesc"]);
                    tblGlobalRateTOList.Add(tblGlobalRateTONew);
                }
            }
            return tblGlobalRateTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGlobalRateTO, cmdInsert);
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

        public static int InsertTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGlobalRateTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblGlobalRateTO tblGlobalRateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGlobalRate]( " +
                            "  [createdBy]" +
                            " ,[createdOn]" +
                            " ,[rate]" +
                            " ,[comments]" +
                            " ,[rateReasonId]" +
                            " )" +
                " VALUES (" +
                            "  @CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Rate " +
                            " ,@Comments " +
                            " ,@rateReasonId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGlobalRate", System.Data.SqlDbType.Int).Value = tblGlobalRateTO.IdGlobalRate;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGlobalRateTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGlobalRateTO.CreatedOn;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblGlobalRateTO.Rate;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblGlobalRateTO.Comments);
            cmdInsert.Parameters.Add("@rateReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGlobalRateTO.RateReasonId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblGlobalRateTO.IdGlobalRate = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGlobalRateTO, cmdUpdate);
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

        public static int UpdateTblGlobalRate(TblGlobalRateTO tblGlobalRateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGlobalRateTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblGlobalRateTO tblGlobalRateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGlobalRate] SET " + 
                                "  [idGlobalRate] = @IdGlobalRate" +
                                " ,[createdBy]= @CreatedBy" +
                                " ,[createdOn]= @CreatedOn" +
                                " ,[rate] = @Rate" +
                                " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGlobalRate", System.Data.SqlDbType.Int).Value = tblGlobalRateTO.IdGlobalRate;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGlobalRateTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGlobalRateTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblGlobalRateTO.Rate;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblGlobalRate(Int32 idGlobalRate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGlobalRate, cmdDelete);
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

        public static int DeleteTblGlobalRate(Int32 idGlobalRate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGlobalRate, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idGlobalRate, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGlobalRate] " +
            " WHERE idGlobalRate = " + idGlobalRate +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGlobalRate", System.Data.SqlDbType.Int).Value = tblGlobalRateTO.IdGlobalRate;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
