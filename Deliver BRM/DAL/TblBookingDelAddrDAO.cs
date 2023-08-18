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
    public class TblBookingDelAddrDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT *,tblBookingSchedule.loadingLayerId FROM [tblBookingDelAddr] tblBookingDelAddr" +
               "  LEFT JOIN[tblBookingSchedule] tblBookingSchedule ON tblBookingSchedule.idSchedule = tblBookingDelAddr.scheduleId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection

        /// <summary>
        /// Aditee [04-02-2020]: Select the address for booking from existing bookings of dealer.
        /// </summary>
        /// <param name="dealerOrgId"></param>
        /// <param name="txnAddrTypeId"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static List<TblBookingDelAddrTO> SelectTblBookingsByDealerOrgId(Int32 dealerOrgId, String txnAddrTypeIdtemp , Int32 topRecordcnt)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String addrTypeCon = null;
                if (!String.IsNullOrEmpty(txnAddrTypeIdtemp))
                {
                    addrTypeCon = " AND addr.txnAddrTypeId IN (" + txnAddrTypeIdtemp + ")";

                }
                //cmdSelect.CommandText = " SELECT * FROM tblBookingDelAddr bookingDelAddr" +
                //                        " LEFT JOIN tblbookings tblbooking on tblbooking.idBooking = bookingDelAddr.bookingId " +
                //                        " WHERE tblbooking.dealerOrgId = " + dealerOrgId + addrTypeCon + " ORDER BY tblbooking.idbooking DESC";

                cmdSelect.CommandText = "select billingOrgId,Pincode, addr.billingName,gstNo,contactNo,address,talukaName,districtName,state,stateId,country,villageName,panNo,aadharNo,idBookingDelAddr" +
                                           " from tblBookingDelAddr addr" +
                                            " LEFT JOIN tblbookings tblbooking on tblbooking.idBooking = addr.bookingId" +
                                            " where tblbooking.dealerOrgId = " + dealerOrgId + addrTypeCon +
                                            " group by billingOrgId,Pincode,addr.billingName,gstNo,contactNo,address,talukaName,districtName,state,stateId,country,villageName,panNo,aadharNo,idBookingDelAddr" +
                                            " order by idBookingDelAddr desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingDelAddrTO> list = BookingConvertDTToList(sqlReader);
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

        public static List<TblBookingDelAddrTO> BookingConvertDTToList(SqlDataReader tblBookingDelAddrTODT)
        {
            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
            if (tblBookingDelAddrTODT != null)
            {
                while (tblBookingDelAddrTODT.Read())
                {
                    TblBookingDelAddrTO tblBookingDelAddrTONew = new TblBookingDelAddrTO();
                    if (tblBookingDelAddrTODT["billingName"] != DBNull.Value)
                        tblBookingDelAddrTONew.BillingName = Convert.ToString(tblBookingDelAddrTODT["billingName"].ToString());
                    if (tblBookingDelAddrTODT["gstNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.GstNo = Convert.ToString(tblBookingDelAddrTODT["gstNo"].ToString());
                    if (tblBookingDelAddrTODT["contactNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.ContactNo = Convert.ToString(tblBookingDelAddrTODT["contactNo"].ToString());
                    if (tblBookingDelAddrTODT["address"] != DBNull.Value)
                        tblBookingDelAddrTONew.Address = Convert.ToString(tblBookingDelAddrTODT["address"].ToString());
                    if (tblBookingDelAddrTODT["talukaName"] != DBNull.Value)
                        tblBookingDelAddrTONew.TalukaName = Convert.ToString(tblBookingDelAddrTODT["talukaName"].ToString());
                    if (tblBookingDelAddrTODT["districtName"] != DBNull.Value)
                        tblBookingDelAddrTONew.DistrictName = Convert.ToString(tblBookingDelAddrTODT["districtName"].ToString());
                    if (tblBookingDelAddrTODT["state"] != DBNull.Value)
                        tblBookingDelAddrTONew.State = Convert.ToString(tblBookingDelAddrTODT["state"].ToString());
                    if (tblBookingDelAddrTODT["country"] != DBNull.Value)
                        tblBookingDelAddrTONew.Country = Convert.ToString(tblBookingDelAddrTODT["country"].ToString());
                    if (tblBookingDelAddrTODT["stateId"] != DBNull.Value)
                        tblBookingDelAddrTONew.StateId = Convert.ToInt32(tblBookingDelAddrTODT["stateId"].ToString());
                    if (tblBookingDelAddrTODT["villageName"] != DBNull.Value)
                        tblBookingDelAddrTONew.VillageName = Convert.ToString(tblBookingDelAddrTODT["villageName"].ToString());
                    if (tblBookingDelAddrTODT["Pincode"] != DBNull.Value)
                        tblBookingDelAddrTONew.Pincode = Convert.ToInt32(tblBookingDelAddrTODT["Pincode"].ToString());
                    if (tblBookingDelAddrTODT["panNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.PanNo = Convert.ToString(tblBookingDelAddrTODT["panNo"].ToString());
                    if (tblBookingDelAddrTODT["aadharNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.AadharNo = Convert.ToString(tblBookingDelAddrTODT["aadharNo"].ToString());
                    
                        if (tblBookingDelAddrTODT["billingOrgId"] != DBNull.Value)
                        tblBookingDelAddrTONew.BillingOrgId = Convert.ToInt32(tblBookingDelAddrTODT["billingOrgId"].ToString());
                    tblBookingDelAddrTOList.Add(tblBookingDelAddrTONew);
                }
            }
            return tblBookingDelAddrTOList;
        }

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE scheduleId=" + scheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingDelAddrTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddr()
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
                List<TblBookingDelAddrTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddr(int bookingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblBookingSchedule.bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingDelAddrTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingDelAddrTO> SelectAllTblBookingDelAddr(int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblBookingSchedule.bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingDelAddrTO> list = ConvertDTToList(sqlReader);
                return list;
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

        public static TblBookingDelAddrTO SelectTblBookingDelAddr(Int32 idBookingDelAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBookingDelAddr = " + idBookingDelAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingDelAddrTO> list = ConvertDTToList(reader);
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

        public static List<TblBookingDelAddrTO> ConvertDTToList(SqlDataReader tblBookingDelAddrTODT)
        {
            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
            if (tblBookingDelAddrTODT != null)
            {
                while (tblBookingDelAddrTODT.Read())
                {
                    TblBookingDelAddrTO tblBookingDelAddrTONew = new TblBookingDelAddrTO();
                    if (tblBookingDelAddrTODT["idBookingDelAddr"] != DBNull.Value)
                        tblBookingDelAddrTONew.IdBookingDelAddr = Convert.ToInt32(tblBookingDelAddrTODT["idBookingDelAddr"].ToString());
                    if (tblBookingDelAddrTODT["bookingId"] != DBNull.Value)
                        tblBookingDelAddrTONew.BookingId = Convert.ToInt32(tblBookingDelAddrTODT["bookingId"].ToString());
                    if (tblBookingDelAddrTODT["pincode"] != DBNull.Value)
                        tblBookingDelAddrTONew.Pincode = Convert.ToInt32(tblBookingDelAddrTODT["pincode"].ToString());
                    if (tblBookingDelAddrTODT["address"] != DBNull.Value)
                        tblBookingDelAddrTONew.Address = Convert.ToString(tblBookingDelAddrTODT["address"].ToString());
                    if (tblBookingDelAddrTODT["villageName"] != DBNull.Value)
                        tblBookingDelAddrTONew.VillageName = Convert.ToString(tblBookingDelAddrTODT["villageName"].ToString());
                    if (tblBookingDelAddrTODT["talukaName"] != DBNull.Value)
                        tblBookingDelAddrTONew.TalukaName = Convert.ToString(tblBookingDelAddrTODT["talukaName"].ToString());
                    if (tblBookingDelAddrTODT["districtName"] != DBNull.Value)
                        tblBookingDelAddrTONew.DistrictName = Convert.ToString(tblBookingDelAddrTODT["districtName"].ToString());
                    if (tblBookingDelAddrTODT["comment"] != DBNull.Value)
                        tblBookingDelAddrTONew.Comment = Convert.ToString(tblBookingDelAddrTODT["comment"].ToString());
                    if (tblBookingDelAddrTODT["state"] != DBNull.Value)
                        tblBookingDelAddrTONew.State = Convert.ToString(tblBookingDelAddrTODT["state"].ToString());
                    if (tblBookingDelAddrTODT["country"] != DBNull.Value)
                        tblBookingDelAddrTONew.Country = Convert.ToString(tblBookingDelAddrTODT["country"].ToString());
                    if (tblBookingDelAddrTODT["billingName"] != DBNull.Value)
                        tblBookingDelAddrTONew.BillingName = Convert.ToString(tblBookingDelAddrTODT["billingName"].ToString());
                    if (tblBookingDelAddrTODT["gstNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.GstNo = Convert.ToString(tblBookingDelAddrTODT["gstNo"].ToString());
                    if (tblBookingDelAddrTODT["contactNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.ContactNo = Convert.ToString(tblBookingDelAddrTODT["contactNo"].ToString());
                    if (tblBookingDelAddrTODT["stateId"] != DBNull.Value)
                        tblBookingDelAddrTONew.StateId = Convert.ToInt32(tblBookingDelAddrTODT["stateId"].ToString());
                    if (tblBookingDelAddrTODT["txnAddrTypeId"] != DBNull.Value)
                        tblBookingDelAddrTONew.TxnAddrTypeId = Convert.ToInt32(tblBookingDelAddrTODT["txnAddrTypeId"].ToString());
                    if (tblBookingDelAddrTODT["addrSourceTypeId"] != DBNull.Value)
                        tblBookingDelAddrTONew.AddrSourceTypeId = Convert.ToInt32(tblBookingDelAddrTODT["addrSourceTypeId"].ToString());
                    if (tblBookingDelAddrTODT["billingOrgId"] != DBNull.Value)
                        tblBookingDelAddrTONew.BillingOrgId = Convert.ToInt32(tblBookingDelAddrTODT["billingOrgId"].ToString());
                    if (tblBookingDelAddrTODT["panNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.PanNo = Convert.ToString(tblBookingDelAddrTODT["panNo"].ToString());
                    if (tblBookingDelAddrTODT["aadharNo"] != DBNull.Value)
                        tblBookingDelAddrTONew.AadharNo = Convert.ToString(tblBookingDelAddrTODT["aadharNo"].ToString());

                    tblBookingDelAddrTOList.Add(tblBookingDelAddrTONew);
                }
            }
            return tblBookingDelAddrTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingDelAddrTO, cmdInsert);
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

        public static int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingDelAddrTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingDelAddrTO tblBookingDelAddrTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingDelAddr]( " + 
                                "  [bookingId]" +
                                " ,[pincode]" +
                                " ,[address]" +
                                " ,[villageName]" +
                                " ,[talukaName]" +
                                " ,[districtName]" +
                                " ,[comment]" +
                                " ,[state]" +
                                " ,[country]" +
                                " ,[billingName]" +
                                " ,[gstNo]" +
                                " ,[contactNo]" +
                                " ,[stateId]" +
                                " ,[scheduleId]" +
                                " ,[txnAddrTypeId]" +
                                " ,[addrSourceTypeId]" +
                                " ,[billingOrgId]" +
                                " ,[panNo]" +
                                " ,[aadharNo]" +
                                " )" +
                    " VALUES (" +
                                "  @BookingId " +
                                " ,@Pincode " +
                                " ,@Address " +
                                " ,@VillageName " +
                                " ,@TalukaName " +
                                " ,@DistrictName " +
                                " ,@Comment " +
                                " ,@state " +
                                " ,@country " +
                                " ,@billingName " +
                                " ,@gstNo " +
                                " ,@contactNo " +
                                " ,@stateId " +
                                " ,@scheduleId " +
                                " ,@TxnAddrTypeId " +
                                " ,@AddrSourceTypeId " +
                                " ,@BillingOrgId " +
                                " ,@panNo " +
                                " ,@aadharNo " +

                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            //String sqlSelectIdentityQry = "Select @@Identity";

            //Commented as identity column
            //cmdInsert.Parameters.Add("@IdBookingDelAddr", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.IdBookingDelAddr;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.BookingId;
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.Pincode);
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = tblBookingDelAddrTO.Address;
            cmdInsert.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.VillageName);
            cmdInsert.Parameters.Add("@TalukaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.TalukaName);
            cmdInsert.Parameters.Add("@DistrictName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.DistrictName);
            cmdInsert.Parameters.Add("@Comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.Comment);
            cmdInsert.Parameters.Add("@state", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.State);
            cmdInsert.Parameters.Add("@country", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.Country);
            cmdInsert.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.BillingName);
            cmdInsert.Parameters.Add("@gstNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.GstNo);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.ContactNo);
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue( tblBookingDelAddrTO.StateId);
            cmdInsert.Parameters.Add("@scheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.ScheduleId);
            cmdInsert.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.TxnAddrTypeId);
            cmdInsert.Parameters.Add("@AddrSourceTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.AddrSourceTypeId);
            cmdInsert.Parameters.Add("@BillingOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.BillingOrgId);
            cmdInsert.Parameters.Add("@panNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.PanNo);
            cmdInsert.Parameters.Add("@aadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.AadharNo);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingDelAddrTO.IdBookingDelAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingDelAddrTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingDelAddrTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingDelAddrTO tblBookingDelAddrTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingDelAddr] SET " +
                            "  [bookingId]= @BookingId" +
                            " ,[pincode]= @Pincode" +
                            " ,[address]= @Address" +
                            " ,[villageName]= @VillageName" +
                            " ,[talukaName]= @TalukaName" +
                            " ,[districtName]= @DistrictName" +
                            " ,[comment] = @Comment" +
                            " ,[state] = @state" +
                            " ,[country] = @country" +
                            " ,[billingName] = @billingName" +
                            " ,[gstNo] = @gstNo" +
                            " ,[contactNo] = @contactNo" +
                            " ,[stateId] = @stateId" +
                            " ,[scheduleId] = @scheduleId" +
                            " WHERE [idBookingDelAddr] = @IdBookingDelAddr";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingDelAddr", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.IdBookingDelAddr;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.BookingId;
            cmdUpdate.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.Pincode);
            cmdUpdate.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = tblBookingDelAddrTO.Address;
            cmdUpdate.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.VillageName);
            cmdUpdate.Parameters.Add("@TalukaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.TalukaName);
            cmdUpdate.Parameters.Add("@DistrictName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.DistrictName);
            cmdUpdate.Parameters.Add("@Comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.Comment);
            cmdUpdate.Parameters.Add("@state", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.State);
            cmdUpdate.Parameters.Add("@country", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.Country);
            cmdUpdate.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.BillingName);
            cmdUpdate.Parameters.Add("@gstNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.GstNo);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.ContactNo);
            cmdUpdate.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.StateId);
            cmdUpdate.Parameters.Add("@scheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingDelAddrTO.ScheduleId);
            return cmdUpdate.ExecuteNonQuery();
        }
        public static int DeleteTblBookingDelAddrByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByScheduleId(scheduleId, cmdDelete);
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

        public static int ExecuteDeletionCommandByScheduleId(Int32 scheduleId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingDelAddr] " +
            " WHERE scheduleId = " + scheduleId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingDelAddr", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.IdBookingDelAddr;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

        #region Deletion
        public static int DeleteTblBookingDelAddr(Int32 idBookingDelAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingDelAddr, cmdDelete);
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

        public static int DeleteTblBookingDelAddr(Int32 idBookingDelAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingDelAddr, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBookingDelAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingDelAddr] " +
            " WHERE idBookingDelAddr = " + idBookingDelAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingDelAddr", System.Data.SqlDbType.Int).Value = tblBookingDelAddrTO.IdBookingDelAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
