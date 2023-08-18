using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseEnquiryHistoryDAO : ITblPurchaseEnquiryHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseEnquiryHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            //String sqlSelectQry = " SELECT * FROM [tblPurchaseEnquiryHistory]";
            String sqlSelectQry = " SELECT trailRecords.*,trailRecords.updatedOn as createdOn, userDtl.userDisplayName ,statusDtl.statusName,'' as cdValue, rate " +
                                  " FROM tblPurchaseEnquiry trailRecords " +
                                  " INNER JOIN tblGlobalRatePurchase " +
                                  " ON globalRatePurchaseId = idGlobalRatePurchase" +
                                  " LEFT JOIN tblUser userDtl " +
                                  " ON userDtl.idUser = trailRecords.createdBy " +
                                  " LEFT JOIN dimStatus statusDtl " +
                                  " ON statusDtl.idStatus = trailRecords.statusId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryHistoryTO> list = ConvertDTToList(reader);
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

        //Priyanka [10-01-2019]
        public List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBookingDetails(Int32 idPurchaseEnquiry)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT tblPurchaseEnquiryHistory.*, statusDtl.statusName FROM tblPurchaseEnquiryHistory " +
                                        " LEFT JOIN dimStatus statusDtl ON statusDtl.idStatus = tblPurchaseEnquiryHistory.statusId " +
                                        " WHERE idPurchaseEnquiry = " + idPurchaseEnquiry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryHistoryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseEnquiryHistoryTO> SelectTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseEnquiryHistory = " + idPurchaseEnquiryHistory + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryHistoryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseEnquiryHistoryTO> SelectAllTblPurchaseEnquiryHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryHistoryTO> list = ConvertDTToList(reader);
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
        public List<TblPurchaseEnquiryHistoryTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseEnquiry=" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnquiryHistoryTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseEnquiryHistoryTO> ConvertDTToList(SqlDataReader tblPurchaseEnquiryHistoryTODT)
        {
            List<TblPurchaseEnquiryHistoryTO> tblPurchaseEnquiryHistoryTOList = new List<TblPurchaseEnquiryHistoryTO>();
            if (tblPurchaseEnquiryHistoryTODT != null)
            {
                while (tblPurchaseEnquiryHistoryTODT.Read())
                {
                    TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTONew = new TblPurchaseEnquiryHistoryTO();
                    if (tblPurchaseEnquiryHistoryTODT["idPurchaseEnquiryHistory"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.IdPurchaseEnquiryHistory = Convert.ToInt32(tblPurchaseEnquiryHistoryTODT["idPurchaseEnquiryHistory"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseEnquiryHistoryTODT["idPurchaseEnquiry"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.CreatedBy = Convert.ToInt32(tblPurchaseEnquiryHistoryTODT["createdBy"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["statusId"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.StatusId = Convert.ToInt32(tblPurchaseEnquiryHistoryTODT["statusId"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseEnquiryHistoryTODT["createdOn"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.GlobalRatePurchaseId = Convert.ToDouble(tblPurchaseEnquiryHistoryTODT["globalRatePurchaseId"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["bookingQty"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.BookingQty = Convert.ToDouble(tblPurchaseEnquiryHistoryTODT["bookingQty"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.BookingRate = Convert.ToDouble(tblPurchaseEnquiryHistoryTODT["bookingRate"].ToString());
                    if (tblPurchaseEnquiryHistoryTODT["comments"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.Comments = Convert.ToString(tblPurchaseEnquiryHistoryTODT["comments"].ToString());

                    if (tblPurchaseEnquiryHistoryTODT["statusName"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.StatusName = Convert.ToString(tblPurchaseEnquiryHistoryTODT["statusName"].ToString());

                    if (tblPurchaseEnquiryHistoryTODT["wtActualRate"] != DBNull.Value)
                        tblPurchaseEnquiryHistoryTONew.WtActualRate = Convert.ToDouble(tblPurchaseEnquiryHistoryTODT["wtActualRate"].ToString());

                    tblPurchaseEnquiryHistoryTOList.Add(tblPurchaseEnquiryHistoryTONew);
                }
            }
            return tblPurchaseEnquiryHistoryTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseEnquiryHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseEnquiryHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseEnquiryHistory]( " +
            //"  [idPurchaseEnquiryHistory]" +
            "  [idPurchaseEnquiry]" +
            " ,[createdBy]" +
            " ,[statusId]" +
            " ,[createdOn]" +
            " ,[globalRatePurchaseId]" +
            " ,[bookingQty]" +
            " ,[bookingRate]" +
            " ,[comments]" +
            " ,[wtActualRate]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseEnquiryHistory " +
            "  @IdPurchaseEnquiry " +
            " ,@CreatedBy " +
            " ,@StatusId " +
            " ,@CreatedOn " +
            " ,@GlobalRatePurchaseId " +
            " ,@BookingQty " +
            " ,@BookingRate " +
            " ,@Comments " +
            " ,@WtActualRate " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
            cmdInsert.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiry;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.GlobalRatePurchaseId;
            cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.BookingQty;
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.BookingRate;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.VarChar).Value = tblPurchaseEnquiryHistoryTO.Comments;
            cmdInsert.Parameters.Add("@WtActualRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryHistoryTO.WtActualRate);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseEnquiryHistoryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPurchaseEnquiryHistory(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseEnquiryHistoryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseEnquiryHistoryTO tblPurchaseEnquiryHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseEnquiryHistory] SET " +
            //"  [idPurchaseEnquiryHistory] = @IdPurchaseEnquiryHistory" +
            "  [idPurchaseEnquiry]= @IdPurchaseEnquiry" +
            " ,[createdBy]= @CreatedBy" +
            " ,[statusId]= @StatusId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[globalRatePurchaseId]= @GlobalRatePurchaseId" +
            " ,[bookingQty]= @BookingQty" +
            " ,[bookingRate]= @BookingRate" +
            " ,[comments] = @Comments" +
            " ,[wtActualRate] = @WtActualRate" +
            " WHERE idPurchaseEnquiryHistory = @IdPurchaseEnquiryHistory ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
            cmdUpdate.Parameters.Add("@IdPurchaseEnquiry", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiry;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.StatusId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseEnquiryHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.GlobalRatePurchaseId;
            cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.BookingQty;
            cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnquiryHistoryTO.BookingRate;
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.VarChar).Value = tblPurchaseEnquiryHistoryTO.Comments;
            cmdUpdate.Parameters.Add("@WtActualRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseEnquiryHistoryTO.WtActualRate);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseEnquiryHistory, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPurchaseEnquiryHistory(Int32 idPurchaseEnquiryHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseEnquiryHistory, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPurchaseEnquiryHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseEnquiryHistory] " +
            " WHERE idPurchaseEnquiryHistory = " + idPurchaseEnquiryHistory + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseEnquiryHistory", System.Data.SqlDbType.Int).Value = tblPurchaseEnquiryHistoryTO.IdPurchaseEnquiryHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
