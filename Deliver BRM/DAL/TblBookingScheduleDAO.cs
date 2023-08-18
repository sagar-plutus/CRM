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
    public class TblBookingScheduleDAO 
    {
    
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT bookingSchedule.*,loadingLayer.layerDesc as loadingLayerDesc" +
                " , tblOrganizationForCnf.firmName as cnfName,tblOrganizationForDealer.firmName as dealerName " +
                " ,tblbookings.bookingDisplayNo,tblOrganizationForDealer.idOrganization as dealerOrgId  FROM [tblBookingSchedule] bookingSchedule" +
                " Left join dimLoadingLayers loadingLayer on bookingSchedule.loadingLayerId= loadingLayer.idLoadingLayer"+
                " left join tblbookings tblbookings on tblbookings.idbooking=bookingSchedule.bookingid " +
                " left join tblOrganization tblOrganizationForCnf on tblOrganizationForCnf.idOrganization = tblBookings.cnFOrgId " +
                " left join tblOrganization tblOrganizationForDealer on tblOrganizationForDealer.idOrganization = tblBookings.dealerOrgId "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingScheduleTO> SelectAllTblBookingSchedule()
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

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public static TblBookingScheduleTO SelectTblBookingSchedule(Int32 idSchedule)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSchedule = " + idSchedule +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
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

        public static TblBookingScheduleTO SelectAllTblBookingSchedule(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
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
                cmdSelect.Dispose();
            }
        }


        public static List<TblBookingScheduleTO> ConvertDTToList(SqlDataReader tblBookingScheduleTODT)
        {
            List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO>();
            if (tblBookingScheduleTODT != null)
            {
                while (tblBookingScheduleTODT.Read())
                {
                    TblBookingScheduleTO tblBookingScheduleTONew = new TblBookingScheduleTO();
                    if (tblBookingScheduleTODT["idSchedule"] != DBNull.Value)
                        tblBookingScheduleTONew.IdSchedule = Convert.ToInt32(tblBookingScheduleTODT["idSchedule"].ToString());
                    if (tblBookingScheduleTODT["bookingId"] != DBNull.Value)
                        tblBookingScheduleTONew.BookingId = Convert.ToInt32(tblBookingScheduleTODT["bookingId"].ToString());
                    if (tblBookingScheduleTODT["createdBy"] != DBNull.Value)
                        tblBookingScheduleTONew.CreatedBy = Convert.ToInt32(tblBookingScheduleTODT["createdBy"].ToString());
                    if (tblBookingScheduleTODT["updatedBy"] != DBNull.Value)
                        tblBookingScheduleTONew.UpdatedBy = Convert.ToInt32(tblBookingScheduleTODT["updatedBy"].ToString());
                    if (tblBookingScheduleTODT["scheduleDate"] != DBNull.Value)
                        tblBookingScheduleTONew.ScheduleDate = Convert.ToDateTime(tblBookingScheduleTODT["scheduleDate"].ToString());
                    if (tblBookingScheduleTODT["createdOn"] != DBNull.Value)
                        tblBookingScheduleTONew.CreatedOn = Convert.ToDateTime(tblBookingScheduleTODT["createdOn"].ToString());
                    if (tblBookingScheduleTODT["updatedOn"] != DBNull.Value)
                        tblBookingScheduleTONew.UpdatedOn = Convert.ToDateTime(tblBookingScheduleTODT["updatedOn"].ToString());
                    if (tblBookingScheduleTODT["Qty"] != DBNull.Value)
                        tblBookingScheduleTONew.Qty = Convert.ToDouble(tblBookingScheduleTODT["Qty"].ToString());
                    if (tblBookingScheduleTODT["remark"] != DBNull.Value)
                        tblBookingScheduleTONew.Remark = Convert.ToString(tblBookingScheduleTODT["remark"].ToString());
                    if (tblBookingScheduleTODT["loadingLayerId"] != DBNull.Value)
                        tblBookingScheduleTONew.LoadingLayerId = Convert.ToInt32(tblBookingScheduleTODT["loadingLayerId"].ToString());
                    if (tblBookingScheduleTODT["loadingLayerDesc"] != DBNull.Value)
                        tblBookingScheduleTONew.LoadingLayerDesc = Convert.ToString(tblBookingScheduleTODT["loadingLayerDesc"].ToString());
                    if (tblBookingScheduleTODT["noOfLayers"] != DBNull.Value)
                        tblBookingScheduleTONew.NoOfLayers = Convert.ToInt32(tblBookingScheduleTODT["noOfLayers"].ToString());
                    if (tblBookingScheduleTODT["scheduleGroupId"] != DBNull.Value)
                        tblBookingScheduleTONew.ScheduleGroupId = Convert.ToInt32(tblBookingScheduleTODT["scheduleGroupId"].ToString());
                    //if (tblBookingScheduleTODT["isItemized"] != DBNull.Value)
                    //    tblBookingScheduleTONew.IsItemized = Convert.ToInt32(tblBookingScheduleTODT["isItemized"]);
                    tblBookingScheduleTONew.ScheduleDateStr = tblBookingScheduleTONew.ScheduleDate.ToString("dd/MMM/yyyy");

                    if (tblBookingScheduleTODT["cnfName"] != DBNull.Value)
                        tblBookingScheduleTONew.CnfName = Convert.ToString(tblBookingScheduleTODT["cnfName"].ToString());

                    if (tblBookingScheduleTODT["dealerName"] != DBNull.Value)
                        tblBookingScheduleTONew.DealerName = Convert.ToString(tblBookingScheduleTODT["dealerName"].ToString());

                    if (tblBookingScheduleTODT["bookingDisplayNo"] != DBNull.Value)
                        tblBookingScheduleTONew.BookingDisplayNo = Convert.ToString(tblBookingScheduleTODT["bookingDisplayNo"].ToString());

                    if (tblBookingScheduleTODT["dealerOrgId"] != DBNull.Value)
                        tblBookingScheduleTONew.DealerOrgId = Convert.ToInt32(tblBookingScheduleTODT["dealerOrgId"].ToString());

                    tblBookingScheduleTOList.Add(tblBookingScheduleTONew);
                }
            }
            return tblBookingScheduleTOList;
        }



        public static List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingSchedule.bookingId =" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public static List<TblBookingScheduleTO> GetAllBookingScheduleList(DateTime fromDate,DateTime toDate, Int32 cnfOrgId, Int32 dealerOrgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string bookingStatusIds = string.Empty;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE CONVERT (DATE,bookingSchedule.createdOn,103)  BETWEEN @fromDate AND @toDate" +
                    " AND ISNULL(tblbookings.pendingQty,0) > 0";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                if(cnfOrgId > 0)
                {
                    cmdSelect.CommandText += " AND tblbookings.cnFOrgId = " + cnfOrgId;
                }
                if (dealerOrgId > 0)
                {
                    cmdSelect.CommandText += " AND tblbookings.dealerOrgId = " + dealerOrgId;
                }

                TblConfigParamsTO bookingStatusIdsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.BOOKING_STATUS_ID_FOR_VIEW_SCHEDULE);
                if(bookingStatusIdsTO != null)
                {
                    bookingStatusIds = bookingStatusIdsTO.ConfigParamVal.ToString();
                }

                if(!String.IsNullOrEmpty(bookingStatusIds))
                {
                    cmdSelect.CommandText += " AND tblbookings.statusId IN (" + bookingStatusIds + ")";
                }

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat); ;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat); ;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
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


        public static List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId,SqlConnection conn , SqlTransaction tran)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
               
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingSchedule.bookingId =" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
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
        public static int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingScheduleTO, cmdInsert);
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

        public static int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingScheduleTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdInsert)
        {

            if (tblBookingScheduleTO.ScheduleGroupId == 0)
                tblBookingScheduleTO.ScheduleGroupId = 1;

            String sqlQuery = @" INSERT INTO [tblBookingSchedule]( " + 
            //"  [idSchedule]" +
            " [bookingId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[scheduleDate]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[qty]" +
            " ,[remark]" +
            " ,[loadingLayerId]" +
            " ,[noOfLayers]" +
            " ,[scheduleGroupId]" +
            " )" +
" VALUES (" +
            //"  @IdSchedule " +
            " @BookingId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@ScheduleDate " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Qty " +
            " ,@Remark " +
            " ,@LoadingLayerId " +
             ",@NoOfLayers " +
             ",@ScheduleGroupId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.BookingId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.UpdatedBy);
            cmdInsert.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.ScheduleDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.Qty);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.Remark);
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.LoadingLayerId);
            cmdInsert.Parameters.Add("@NoOfLayers", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.NoOfLayers);
            cmdInsert.Parameters.Add("@ScheduleGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.ScheduleGroupId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingScheduleTO.IdSchedule = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingScheduleTO, cmdUpdate);
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

        public static int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingScheduleTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingSchedule] SET " + 
            " [bookingId]= @BookingId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[scheduleDate]= @ScheduleDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[qty]= @Qty" +
            " ,[remark] = @Remark" +
             " [loadingLayerId]= @LoadingLayerId" +
             " [noOfLayers]= @NoOfLayers" +
             " [scheduleGroupId]= @ScheduleGroupId" +
            " WHERE   [idSchedule] = @IdSchedule";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.BookingId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@ScheduleDate", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.ScheduleDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingScheduleTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblBookingScheduleTO.Qty;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblBookingScheduleTO.Remark;
            cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.LoadingLayerId);
            cmdUpdate.Parameters.Add("@NoOfLayers", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.NoOfLayers);
            cmdUpdate.Parameters.Add("@ScheduleGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingScheduleTO.ScheduleGroupId);

            return cmdUpdate.ExecuteNonQuery();
        }

        /// <summary>
        /// AmolG[2020-Feb-25] This will update the qty
        /// </summary>
        /// <param name="tblBookingScheduleTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int UpdateTblBookingSchedulePendingQty (TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblBookingSchedule] SET " +
                                 " [qty] = @Qty" +
                                 " WHERE   [idSchedule] = @IdSchedule";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
                cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.NVarChar).Value = tblBookingScheduleTO.Qty;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblBookingSchedule(Int32 idSchedule)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSchedule, cmdDelete);
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

        public static int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSchedule, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSchedule, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookingSchedule] " +
            " WHERE idSchedule = " + idSchedule +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSchedule", System.Data.SqlDbType.Int).Value = tblBookingScheduleTO.IdSchedule;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
