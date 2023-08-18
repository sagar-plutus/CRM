using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using System.Configuration;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseBookingOpngBalDAO : ITblPurchaseBookingOpngBalDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseBookingOpngBalDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseBookingOpngBal]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBal()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isActive =1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseBookingOpngBalTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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


        public List<TblPurchaseBookingOpngBalTO> ConvertDTToList(SqlDataReader tblPurchaseBookingOpngBalTODT)
        {
            List<TblPurchaseBookingOpngBalTO> tblPurchaseBookingOpngBalTOList = new List<TblPurchaseBookingOpngBalTO>();
            if (tblPurchaseBookingOpngBalTODT != null)
            {
                while (tblPurchaseBookingOpngBalTODT.Read())
                {
                    TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTONew = new TblPurchaseBookingOpngBalTO();
                    if (tblPurchaseBookingOpngBalTODT["idPurchaseBookingOpngBal"] != DBNull.Value)
                        tblPurchaseBookingOpngBalTONew.IdPurchaseBookingOpngBal = Convert.ToInt32(tblPurchaseBookingOpngBalTODT["idPurchaseBookingOpngBal"].ToString());
                    if (tblPurchaseBookingOpngBalTODT["enquiryId"] != DBNull.Value)
                        tblPurchaseBookingOpngBalTONew.EnquiryId = Convert.ToInt32(tblPurchaseBookingOpngBalTODT["enquiryId"].ToString());
                    if (tblPurchaseBookingOpngBalTODT["balAsOnDate"] != DBNull.Value)
                        tblPurchaseBookingOpngBalTONew.BalAsOnDate = Convert.ToDateTime(tblPurchaseBookingOpngBalTODT["balAsOnDate"].ToString());
                    if (tblPurchaseBookingOpngBalTODT["openingBalQty"] != DBNull.Value)
                        tblPurchaseBookingOpngBalTONew.OpeningBalQty = Convert.ToDouble(tblPurchaseBookingOpngBalTODT["openingBalQty"].ToString());
                    if (tblPurchaseBookingOpngBalTODT["isActive"] != DBNull.Value)
                        tblPurchaseBookingOpngBalTONew.IsActive = Convert.ToInt32(tblPurchaseBookingOpngBalTODT["isActive"].ToString());

                    tblPurchaseBookingOpngBalTOList.Add(tblPurchaseBookingOpngBalTONew);
                }
            }
            return tblPurchaseBookingOpngBalTOList;
        }

        public TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBalLatest()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select top(1)* from tblPurchaseBookingOpngBal where isActive =1 order by idPurchaseBookingOpngBal desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseBookingOpngBalTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
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

        public Dictionary<Int32, Double> SelectBookingsPendingQtyDCT(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Dictionary<Int32, Double> pendingQtyDCT = new Dictionary<int, double>();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR + "," + (Int32)Constants.TranStatusE.BOOKING_APPROVED
                + "," + (Int32)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL;

            // String statusIds = (int)Constants.TranStatusE.BOOKING_NEW_B + "," + (int)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "," + (int)Constants.TranStatusE.COMPLETED + ","
            // + (int)Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTOR + "," + (int)Constants.TranStatusE.PENDING_FOR_PURCHASE_MANAGER_APPROVAL + "," + (int)Constants.TranStatusE.BOOKING_DELETE;
            try
            {
                cmdSelect.CommandText = " SELECT idPurchaseEnquiry,pendingBookingQty FROM tblPurchaseEnquiry " +
                                        " WHERE statusId IN (" + statusIds + " ) AND isnull(pendingBookingQty,0) > 0" +
                                        " AND ISNULL(isConvertToSauda,0) = 1";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 bookingId = 0;
                        Double pendingQty = 0;
                        if (reader["idPurchaseEnquiry"] != DBNull.Value)
                            bookingId = Convert.ToInt32(reader["idPurchaseEnquiry"].ToString());
                        if (reader["pendingBookingQty"] != DBNull.Value)
                            pendingQty = Convert.ToDouble(reader["pendingBookingQty"].ToString());

                        if (bookingId > 0 && pendingQty > 0)
                            pendingQtyDCT.Add(bookingId, pendingQty);
                    }
                }

                return pendingQtyDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public TblPurchaseBookingOpngBalTO SelectTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseBookingOpngBal = " + idPurchaseBookingOpngBal + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseBookingOpngBalTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
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

        public List<TblPurchaseBookingOpngBalTO> SelectAllTblPurchaseBookingOpngBal(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseBookingOpngBalTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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


        public Dictionary<int, double> SelectBookingWiseUnloadingQtyDCT(DateTime serverDate, bool flag)
        {
            Dictionary<int, Double> unloadingQtyDCT = new Dictionary<int, double>();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            // String statusIn = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_NOT_CONFIRM;
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_DIRECTOR + "," + (Int32)Constants.TranStatusE.BOOKING_APPROVED;

            // String delStatusIn = (int)Constants.TranStatusE.LOADING_CANCEL + "";

            try
            {
                conn.Open();
                if (!flag)
                {
                    cmdSelect.CommandText = @" select sum(tblPurchaseScheduleSummary.qty) as unloadingQty,idPurchaseEnquiry "
                    + " from tblPurchaseEnquiry tblPurchaseEnquiry left join tblPurchaseScheduleSummary tblPurchaseScheduleSummary "
                    + " on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId"
                    + " where isnull(tblPurchaseScheduleSummary.qty,0) !=0 and isnull(tblPurchaseScheduleSummary.statusId,0)= " + (Int32)Constants.TranStatusE.New 
                    //Prajakta [2020-29-07] Commented as u pending enquiry loading was not shown on UI.
                    //+ " and tblPurchaseEnquiry.pendingBookingQty >0 "
                    + " and tblPurchaseEnquiry.statusId in (" + statusIds + ") and tblPurchaseScheduleSummary.createdOn > @serverDate"
                    + " group by idPurchaseEnquiry";
                }
                else
                {
                    cmdSelect.CommandText = @" select sum(tblPurchaseUnloadingDtl.qtyMT) as unloadingQty,idPurchaseEnquiry "
                                        + " from tblPurchaseEnquiry tblPurchaseEnquiry left join tblPurchaseScheduleSummary tblPurchaseScheduleSummary "
                                        + " on tblPurchaseEnquiry.idPurchaseEnquiry = tblPurchaseScheduleSummary.purchaseEnquiryId"
                                        + " left join tblPurchaseUnloadingDtl tblPurchaseUnloadingDtl"
                                        + " on tblPurchaseScheduleSummary.idPurchaseScheduleSummary = tblPurchaseUnloadingDtl.purchaseScheduleSummaryId "
                                        + " and tblPurchaseScheduleSummary.statusId in (" + (int)Constants.TranStatusE.VEHICLE_REJECTED_AFTER_WEIGHING + ")"
                                        + " where isnull(tblPurchaseUnloadingDtl.qtyMT,0) !=0 group by idPurchaseEnquiry";

                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@serverDate", System.Data.SqlDbType.DateTime).Value = serverDate;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        int idPurchaseEnquiry = Convert.ToInt32(reader["idPurchaseEnquiry"].ToString());
                        Double unloadingQty = Convert.ToDouble(reader["unloadingQty"].ToString());
                        unloadingQtyDCT.Add(idPurchaseEnquiry, unloadingQty);
                    }
                }

                return unloadingQtyDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Dispose();
                }
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseBookingOpngBalTO, cmdInsert);
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

        public int InsertTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseBookingOpngBalTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseBookingOpngBal]( " +
            "  [enquiryId]" +
            " ,[balAsOnDate]" +
            " ,[openingBalQty]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
            " @EnquiryId " +
            " ,@BalAsOnDate " +
            " ,@OpeningBalQty " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
            cmdInsert.Parameters.Add("@EnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.EnquiryId;
            cmdInsert.Parameters.Add("@BalAsOnDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseBookingOpngBalTO.BalAsOnDate;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IsActive;
            cmdInsert.Parameters.Add("@OpeningBalQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseBookingOpngBalTO.OpeningBalQty;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation

        public int UpdateTblPurchaseBookingOpngBal(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandIsActive(cmdUpdate);
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

        public int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseBookingOpngBalTO, cmdUpdate);
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

        public int UpdateTblPurchaseBookingOpngBal(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseBookingOpngBalTO, cmdUpdate);
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

        public int ExecuteUpdationCommandIsActive(SqlCommand cmdUpdate)
        {
            String sqlQuery = @"UPDATE [tblPurchaseBookingOpngBal] SET [isActive] = 0" +
            " WHERE isActive = 1";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdationCommand(TblPurchaseBookingOpngBalTO tblPurchaseBookingOpngBalTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseBookingOpngBal] SET " +
            " [enquiryId]= @EnquiryId" +
            " ,[balAsOnDate]= @BalAsOnDate" +
            " ,[openingBalQty] = @OpeningBalQty" +
            " ,[isActive] = @IsActive" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
            cmdUpdate.Parameters.Add("@EnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.EnquiryId;
            cmdUpdate.Parameters.Add("@BalAsOnDate", System.Data.SqlDbType.DateTime).Value = tblPurchaseBookingOpngBalTO.BalAsOnDate;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IsActive;
            cmdUpdate.Parameters.Add("@OpeningBalQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseBookingOpngBalTO.OpeningBalQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseBookingOpngBal, cmdDelete);
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

        public int DeleteTblPurchaseBookingOpngBal(Int32 idPurchaseBookingOpngBal, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseBookingOpngBal, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseBookingOpngBal, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseBookingOpngBal] " +
            " WHERE idPurchaseBookingOpngBal = " + idPurchaseBookingOpngBal + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseBookingOpngBal", System.Data.SqlDbType.Int).Value = tblPurchaseBookingOpngBalTO.IdPurchaseBookingOpngBal;
            return cmdDelete.ExecuteNonQuery();
        }

        #endregion

    }
}
