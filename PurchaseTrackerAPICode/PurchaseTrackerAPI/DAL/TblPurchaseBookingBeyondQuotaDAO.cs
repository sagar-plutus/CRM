using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseBookingBeyondQuotaDAO : ITblPurchaseBookingBeyondQuotaDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseBookingBeyondQuotaDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public  String SqlSelectQuery()
        {
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

        public  List<TblPurchaseBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
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
                List<TblPurchaseBookingBeyondQuotaTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblPurchaseBookingBeyondQuotaTO> SelectAllPurchaseEnquiryHistory(Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select top(1)*,enquiry.enqDisplayNo from [dbo].[tblPurchaseEnquiryHistory] pech " +
                                        " LEFT JOIN dimStatus statusDtl  ON statusDtl.idStatus = pech.statusId " +
                                        " left join tblPurchaseEnquiry enquiry on enquiry.idPurchaseEnquiry = pech.idPurchaseEnquiry " +
                                        " INNER JOIN tblGlobalRatePurchase  ON pech.globalRatePurchaseId = idGlobalRatePurchase " +
                                        " WHERE pech.idPurchaseEnquiry=" + bookingId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseBookingBeyondQuotaTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblPurchaseBookingBeyondQuotaTO> ConvertDTToList(SqlDataReader tblBookingBeyondQuotaTODT)
        {
            List<TblPurchaseBookingBeyondQuotaTO> tblBookingBeyondQuotaTOList = new List<TblPurchaseBookingBeyondQuotaTO>();
            if (tblBookingBeyondQuotaTODT != null)
            {
                while (tblBookingBeyondQuotaTODT.Read())
                {
                    TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTONew = new TblPurchaseBookingBeyondQuotaTO();
                    if (tblBookingBeyondQuotaTODT["idPurchaseEnquiry"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.BookingId = Convert.ToInt32(tblBookingBeyondQuotaTODT["idPurchaseEnquiry"].ToString());
                    if (tblBookingBeyondQuotaTODT["statusId"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusId = Convert.ToInt32(tblBookingBeyondQuotaTODT["statusId"].ToString());
                    if (tblBookingBeyondQuotaTODT["bookingRate"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.Rate = Convert.ToDouble(tblBookingBeyondQuotaTODT["bookingRate"].ToString());
                    if (tblBookingBeyondQuotaTODT["bookingQty"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.Quantity = Convert.ToDouble(tblBookingBeyondQuotaTODT["bookingQty"].ToString());
                    tblBookingBeyondQuotaTONew.DeliveryPeriod = 0;
                    if (tblBookingBeyondQuotaTODT["createdBy"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CreatedBy = Convert.ToInt32(tblBookingBeyondQuotaTODT["createdBy"].ToString());
                    if (tblBookingBeyondQuotaTODT["createdOn"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.CreatedOn = Convert.ToDateTime(tblBookingBeyondQuotaTODT["createdOn"].ToString());


                    if (tblBookingBeyondQuotaTODT["enqDisplayNo"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.EnqDisplayNo = Convert.ToString(tblBookingBeyondQuotaTODT["enqDisplayNo"].ToString());


                    if (tblBookingBeyondQuotaTODT["statusName"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.StatusDesc = Convert.ToString(tblBookingBeyondQuotaTODT["statusName"].ToString());
                    if (tblBookingBeyondQuotaTODT["rate"] != DBNull.Value)
                        tblBookingBeyondQuotaTONew.Declaredrate = Convert.ToDouble(tblBookingBeyondQuotaTODT["rate"].ToString());

                    try
                    {

                        if (tblBookingBeyondQuotaTODT["authReasons"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.AuthReason = Convert.ToString(tblBookingBeyondQuotaTODT["authReasons"].ToString());


                        if (tblBookingBeyondQuotaTODT["userDisplayName"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.CreatedUserName = Convert.ToString(tblBookingBeyondQuotaTODT["userDisplayName"].ToString());
                        if (tblBookingBeyondQuotaTODT["calculatedMetalCost"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.CalculatedMetalCost = Convert.ToDouble(tblBookingBeyondQuotaTODT["calculatedMetalCost"].ToString());
                        if (tblBookingBeyondQuotaTODT["baseMetalCost"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.BaseMetalCost = Convert.ToDouble(tblBookingBeyondQuotaTODT["baseMetalCost"].ToString());
                        if (tblBookingBeyondQuotaTODT["padta"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.Padta = Convert.ToDouble(tblBookingBeyondQuotaTODT["padta"].ToString());
                        if (tblBookingBeyondQuotaTODT["userId"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.UserId = Convert.ToInt32(tblBookingBeyondQuotaTODT["userId"].ToString());

                    }
                    catch (Exception ex)
                    {

                    }

                    try
                    {
                        if (tblBookingBeyondQuotaTODT["comments"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.Remark = tblBookingBeyondQuotaTODT["comments"].ToString();
                        //
                        if (tblBookingBeyondQuotaTODT["bookingPmRate"] != DBNull.Value)
                            tblBookingBeyondQuotaTONew.BookingPmRate = Convert.ToDouble(tblBookingBeyondQuotaTODT["bookingPmRate"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }


                    tblBookingBeyondQuotaTOList.Add(tblBookingBeyondQuotaTONew);
                }
            }
            return tblBookingBeyondQuotaTOList;
        }

        #endregion

        #region Insertion
        /*
        public  int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingBeyondQuotaTO, cmdInsert);
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

        public  int InsertTblBookingBeyondQuota(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingBeyondQuotaTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlCommand cmdInsert)
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
            cmdInsert.Parameters.Add("@statusRemark", System.Data.SqlDbType.NVarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingBeyondQuotaTO.StatusRemark);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingBeyondQuotaTO.IdBookingAuth = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        */
        #endregion

    }
}
