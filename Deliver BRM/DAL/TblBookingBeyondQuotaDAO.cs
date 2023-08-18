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
    public class TblBookingBeyondQuotaDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT trailRecords.*,userDtl.userDisplayName ,statusDtl.statusName,dimCdStructure.cdValue " +
                                  " FROM tblBookingBeyondQuota trailRecords " +
                                  " LEFT JOIN tblUser userDtl " +
                                  " ON userDtl.idUser = trailRecords.createdBy " +
                                  " LEFT JOIN dimStatus statusDtl " +
                                  " ON statusDtl.idStatus = trailRecords.statusId" +
                                  " LEFT JOIN dimCdStructure ON idCdStructure=cdStructureId";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuota()
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
                List<TblBookingBeyondQuotaTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingBeyondQuotaTO> list = ConvertDTToList(sqlReader);
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

        public static TblBookingBeyondQuotaTO SelectTblBookingBeyondQuota(Int32 idBookingAuth)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBookingAuth = " + idBookingAuth +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingBeyondQuotaTO> list = ConvertDTToList(reader);
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

        public static List<TblBookingBeyondQuotaTO> ConvertDTToList(SqlDataReader tblBookingBeyondQuotaTODT)
        {
            List<TblBookingBeyondQuotaTO> tblBookingBeyondQuotaTOList = new List<TblBookingBeyondQuotaTO>();
            if (tblBookingBeyondQuotaTODT != null)
            {
                while (tblBookingBeyondQuotaTODT.Read())
                {
                    TblBookingBeyondQuotaTO tblBookingBeyondQuotaTONew = new TblBookingBeyondQuotaTO();
                    if (tblBookingBeyondQuotaTODT["idBookingAuth"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.IdBookingAuth = Convert.ToInt32(tblBookingBeyondQuotaTODT["idBookingAuth"].ToString());
                    if (tblBookingBeyondQuotaTODT["bookingId"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.BookingId = Convert.ToInt32(tblBookingBeyondQuotaTODT["bookingId"].ToString());
                    if (tblBookingBeyondQuotaTODT["statusId"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusId = Convert.ToInt32(tblBookingBeyondQuotaTODT["statusId"].ToString());
                    if (tblBookingBeyondQuotaTODT["statusDate"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusDate = Convert.ToDateTime(tblBookingBeyondQuotaTODT["statusDate"].ToString());
                    if (tblBookingBeyondQuotaTODT["rate"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.Rate = Convert.ToDouble(tblBookingBeyondQuotaTODT["rate"].ToString());
                    if (tblBookingBeyondQuotaTODT["quantity"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.Quantity = Convert.ToDouble(tblBookingBeyondQuotaTODT["quantity"].ToString());
                    if (tblBookingBeyondQuotaTODT["deliveryPeriod"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.DeliveryPeriod = Convert.ToDouble(tblBookingBeyondQuotaTODT["deliveryPeriod"].ToString());
                    if (tblBookingBeyondQuotaTODT["createdBy"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CreatedBy = Convert.ToInt32(tblBookingBeyondQuotaTODT["createdBy"].ToString());
                    if (tblBookingBeyondQuotaTODT["createdOn"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CreatedOn = Convert.ToDateTime(tblBookingBeyondQuotaTODT["createdOn"].ToString());
                    if (tblBookingBeyondQuotaTODT["userDisplayName"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CreatedUserName = Convert.ToString(tblBookingBeyondQuotaTODT["userDisplayName"].ToString());
                    if (tblBookingBeyondQuotaTODT["statusName"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusDesc = Convert.ToString(tblBookingBeyondQuotaTODT["statusName"].ToString());

                    if (tblBookingBeyondQuotaTODT["orcAmt"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.OrcAmt = Convert.ToDouble(tblBookingBeyondQuotaTODT["orcAmt"].ToString());
                    if (tblBookingBeyondQuotaTODT["cdStructureId"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CdStructureId = Convert.ToInt32(tblBookingBeyondQuotaTODT["cdStructureId"].ToString());
                    if (tblBookingBeyondQuotaTODT["cdValue"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CdStructure = Convert.ToDouble(tblBookingBeyondQuotaTODT["cdValue"].ToString());
                    if (tblBookingBeyondQuotaTODT["statusRemark"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusRemark = Convert.ToString(tblBookingBeyondQuotaTODT["statusRemark"].ToString());
                    tblBookingBeyondQuotaTOList.Add(tblBookingBeyondQuotaTONew);
                }
            }
            return tblBookingBeyondQuotaTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingBeyondQuotaTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1 ;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingBeyondQuotaTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingBeyondQuota]( " +
                            "  [bookingId]" +
                            " ,[statusId]" +
                            " ,[statusDate]" +
                            " ,[rate]" +
                            " ,[quantity]" +
                            " ,[deliveryPeriod]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[orcAmt]" +
                            " ,[cdStructureId]" +
                            " ,[remark]" +
                            " ,[statusRemark]" +
                            " )" +
                " VALUES (" +
                            "  @BookingId " +
                            " ,@StatusId " +
                            " ,@StatusDate " +
                            " ,@Rate " +
                            " ,@Quantity " +
                            " ,@DeliveryPeriod " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@orcAmt " +
                            " ,@cdStructureId " +
                            " ,@remark " +
                            " ,@statusRemark " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBookingAuth", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.IdBookingAuth;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.BookingId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.StatusId;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingBeyondQuotaTO.StatusDate;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.Rate;
            cmdInsert.Parameters.Add("@Quantity", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.Quantity;
            cmdInsert.Parameters.Add("@DeliveryPeriod", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.DeliveryPeriod;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingBeyondQuotaTO.CreatedOn;
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.OrcAmt;
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.CdStructureId;
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingBeyondQuotaTO.Remark);
            cmdInsert.Parameters.Add("@statusRemark", System.Data.SqlDbType.NVarChar,128).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingBeyondQuotaTO.StatusRemark);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingBeyondQuotaTO.IdBookingAuth = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingBeyondQuotaTO, cmdUpdate);
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

        public static int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingBeyondQuotaTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookingBeyondQuota] SET " + 
                                "  [bookingId]= @BookingId" +
                                " ,[statusId]= @StatusId" +
                                " ,[statusDate]= @StatusDate" +
                                " ,[rate]= @Rate" +
                                " ,[quantity]= @Quantity" +
                                " ,[deliveryPeriod] = @DeliveryPeriod" +
                                " WHERE [idBookingAuth] = @IdBookingAuth "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBookingAuth", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.IdBookingAuth;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.BookingId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.StatusId;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.StatusDate;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.Rate;
            cmdUpdate.Parameters.Add("@Quantity", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.Quantity;
            cmdUpdate.Parameters.Add("@DeliveryPeriod", System.Data.SqlDbType.NVarChar).Value = tblBookingBeyondQuotaTO.DeliveryPeriod;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblBookingBeyondQuota(Int32 idBookingAuth)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBookingAuth, cmdDelete);
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

        public static int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBookingAuth, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBookingAuth, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tblBookingBeyondQuota] " +
                                    " WHERE idBookingAuth = " + idBookingAuth +"";
                                    cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBookingAuth", System.Data.SqlDbType.Int).Value = tblBookingBeyondQuotaTO.IdBookingAuth;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
