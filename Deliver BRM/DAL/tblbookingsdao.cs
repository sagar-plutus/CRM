using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using Microsoft.Extensions.Logging;


namespace SalesTrackerAPI.DAL
{
    public class TblBookingsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT bookings.*, orgCnf.firmName as cnfName,orgDealer.firmName as dealerName, dimStatus.statusName, " +
                                    " tblAddress.villageName AS dealerVillageName,dimTaluka.talukaName AS dealerTalukaName,dimDistrict.districtName AS dealerDistrictName,dimState.stateName AS dealerStateName,dimBookingType.BookingTypeName " +
                                    " FROM tblbookings bookings " +
                                    " LEFT JOIN tblOrganization orgCnf " +
                                    " ON bookings.cnfOrgId = orgCnf.idOrganization " +
                                    " LEFT JOIN tblOrganization orgDealer " +
                                    " ON bookings.dealerOrgId = orgDealer.idOrganization " +
                                    " LEFT JOIN tblAddress tblAddress on tblAddress.idAddr = orgDealer.addrId " +
                                    " LEFT JOIN dimTaluka dimTaluka on dimTaluka.idTaluka = tblAddress.talukaId " +
                                    " LEFT JOIN dimDistrict dimDistrict on dimDistrict.idDistrict = tblAddress.districtId " +
                                    " LEFT JOIN dimState dimState on dimState.idState = tblAddress.stateId " +
                                    " LEFT JOIN dimStatus ON dimStatus.idStatus=bookings.statusId " +
                                    "   left join dimBookingType on dimBookingType.idBookingType=bookings.bookingtypeid  ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblBookingsTO> SelectAllTblBookings()
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
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idBooking IN(SELECT DISTINCT bookingId FROM tempLoadingSlipDtl WHERE loadingSlipId=" + loadingSlipId + ")" +

                                       // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipDtl
                                       " UNION ALL " +
                                       " SELECT * FROM ("+ SqlSelectQuery() + ")sq2 WHERE idBooking IN(SELECT DISTINCT bookingId FROM finalLoadingSlipDtl WHERE loadingSlipId=" + loadingSlipId + ")";
                
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        
        //Priyanka [28-03-2019]
        public static List<TblBookingsTO> SelectAllBookingsListFromBookingDisplayNo(Int32 BookingRefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE bookingRefId=" + BookingRefId ;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingsTO> SelectAllBookingsListForApproval()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_NEW + "";

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS + "," +(Int32)Constants.TranStatusE.PENDING_BOOKING_FOR_CNF_APPROVAL;
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();

                if (isConfEn == 1)
                {

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization ";
                }

                if (cnfId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")" + " AND bookings.cnFOrgId=" + cnfId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblBookingsTO> SelectAllPendingBookingsList(Int32 cnfId, Int32 dealerId, DateTime asOnDate,string dateCondOper, Boolean onlyPendingYn,Int32 StateId,Int32 DistId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
            String whereCond = string.Empty;
            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    whereCond = " AND bookings.cnfOrgId= " + cnfId;
                }

                if (dealerId > 0)
                {
                    whereCond += " AND dealerOrgId=" + dealerId;
                }
                if (StateId  > 0)
                {
                    whereCond = " AND tblAddress.stateId= " + StateId;
                }

                if (DistId > 0)
                {
                    whereCond += " AND tblAddress.districtId=" + DistId;
                }



                if (onlyPendingYn)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE)" + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.pendingQty > 0 " + whereCond;


                    //if (cnfId > 0)
                    //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE)" + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.pendingQty > 0 AND bookings.cnFOrgId=" + cnfId;
                    //else
                    //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.pendingQty > 0  ";
                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + whereCond;

                    //if (cnfId > 0)
                    //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.cnFOrgId=" + cnfId;
                    //else
                    //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@asOnDate", System.Data.SqlDbType.Date).Value = asOnDate.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblBookingsTO> SelectTodayLoadedAndDeletedBookingsList(Int32 cnfId, DateTime asOnDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;

            try
            {
                conn.Open();


                if (cnfId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM tempLoadingSlipExt LEFT JOIN tempLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) " +

                                            //Vaibhav [20-Nov-2017]Added to select from finalLoadingSlip and finalLoadingSlipExt

                                            " UNION ALL " +
                                            SqlSelectQuery() + " WHERE bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM finalLoadingSlipExt LEFT JOIN finalLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) ";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookings.cnfOrgId=" + cnfId + " AND bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM tempLoadingSlipExt LEFT JOIN tempLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) " +

                                            //Vaibhav [20-Nov-2017]Added to select from finalLoadingSlip and finalLoadingSlipExt

                                            " UNION ALL " +
                                            SqlSelectQuery() + " WHERE bookings.cnfOrgId=" + cnfId + " AND bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM finalLoadingSlipExt LEFT JOIN finalLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblBookingsTO> SelectAllTodaysBookingsWithOpeningBalance(Int32 cnfId, Int32 dealerId, DateTime asOnDate,Int32 StateId,Int32 DistId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String whereCond = string.Empty;
            try
            {
                conn.Open();
                if (cnfId > 0)
                {
                    whereCond = " AND bookings.cnfOrgId= " + cnfId;
                }
                if (dealerId > 0)
                {
                    whereCond += " AND dealerOrgId=" + dealerId;
                }
                if (StateId > 0)
                {
                    whereCond = " AND tblAddress.stateId= " + StateId;
                }

                if (DistId > 0)
                {
                    whereCond += " AND tblAddress.districtId=" + DistId;
                }


                cmdSelect.CommandText = SqlSelectQuery() + " INNER JOIN tblBookingOpngBal ON bookings.idBooking = tblBookingOpngBal.bookingId " +
                                           "WHERE DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year + whereCond;

                //if (cnfId == 0)
                //    cmdSelect.CommandText = SqlSelectQuery() + " INNER JOIN tblBookingOpngBal ON bookings.idBooking = tblBookingOpngBal.bookingId " +
                //                            "WHERE DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year ;

                //else
                //    cmdSelect.CommandText = SqlSelectQuery() + " INNER JOIN tblBookingOpngBal ON bookings.idBooking = tblBookingOpngBal.bookingId " +
                //                             "WHERE bookings.cnfOrgId=" + cnfId + " AND DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year;



                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords,Boolean pendingYn)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            String statusIds = (int)Constants.TranStatusE.BOOKING_APPROVED + "," + (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
            if (pendingYn)
                whereCond = " AND pendingQty > 0";
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP " + lastNRecords + " bookings.*, orgCnf.firmName as cnfName,orgDealer.firmName as dealerName ,dimStatus.statusName, " +
                                        " tblAddress.villageName AS dealerVillageName,dimTaluka.talukaName AS dealerTalukaName,dimDistrict.districtName AS dealerDistrictName,dimState.stateName AS dealerStateName  ,dimBookingType.BookingTypeName BookingTypeName  " +
                                        " FROM tblbookings bookings " +
                                        " LEFT JOIN tblOrganization orgCnf " +
                                        " ON bookings.cnfOrgId = orgCnf.idOrganization " +
                                        " LEFT JOIN tblOrganization orgDealer " +
                                        " ON bookings.dealerOrgId = orgDealer.idOrganization " +
                                        " LEFT JOIN tblAddress tblAddress on tblAddress.idAddr = orgDealer.addrId " +
                                        " LEFT JOIN dimTaluka dimTaluka on dimTaluka.idTaluka = tblAddress.talukaId " +
                                        " LEFT JOIN dimDistrict dimDistrict on dimDistrict.idDistrict = tblAddress.districtId " +
                                        " LEFT JOIN dimState dimState on dimState.idState = tblAddress.stateId " +
                                        " LEFT JOIN dimStatus ON dimStatus.idStatus=bookings.statusId" +
                                        "     left join dimBookingType on bookings.bookingTypeId =dimBookingType.idBookingType  " +
                                        " WHERE dealerOrgId=" + dealerId +
                                        " AND statusId IN(" + statusIds + ") " + whereCond +
                                        " ORDER BY bookings.createdOn DESC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (isConfEn == 1)
                {

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization ";
                }

                if (cnfId == 0)
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";
                else
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public static List<TblBookingsTO> GetBulkBookingHistoryBookingId(Int32 bookingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
           
            try
            {
                conn.Open();
                sqlQuery = "select  case when pendingQty =0 then 'Close' else  statusName end statusName,idBooking  ,bookingDisplayNo ,dealerOrgId,firmName  ,bookingQty " +
                    " from tblbookings  left join tblOrganization on tblbookings.dealerOrgId =tblOrganization.idOrganization " +
                    "  left join dimStatus  on tblbookings.statusId  =dimStatus.idStatus  " +
                    " where bookingRefId =" + bookingId + "";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = new List<TblBookingsTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                        if (sqlReader["statusName"] != DBNull.Value)
                            tblBookingsTONew.Status = Convert.ToString(sqlReader["statusName"].ToString());
                        if (sqlReader["idBooking"] != DBNull.Value)
                            tblBookingsTONew.IdBooking = Convert.ToInt32(sqlReader["idBooking"].ToString());
                        if (sqlReader["bookingDisplayNo"] != DBNull.Value)
                            tblBookingsTONew.BookingDisplayNo = Convert.ToString(sqlReader["bookingDisplayNo"].ToString());
                        if (sqlReader["dealerOrgId"] != DBNull.Value)
                            tblBookingsTONew.DealerOrgId = Convert.ToInt32(sqlReader["dealerOrgId"].ToString());
                        if (sqlReader["firmName"] != DBNull.Value)
                            tblBookingsTONew.DealerName = Convert.ToString(sqlReader["firmName"].ToString());
                        if (sqlReader["bookingQty"] != DBNull.Value)
                            tblBookingsTONew.BookingQty = Convert.ToDouble(sqlReader["bookingQty"].ToString());

                        list.Add(tblBookingsTONew);
                    }
                }

                if (list != null && list.Count > 0)
                    return list;
                else
                    return null;
                    //return list;
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

        public static List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, string statusId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO,Int32 BookingId,Int32 bookingTypeId,int skipDateFilter)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String areConfJoin = String.Empty;
            String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (isConfEn == 1)
                {
                    //areConfJoin += " AND bookings.dealerOrgId IN(SELECT distinct idOrganization " +
                    //            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                    //            " INNER JOIN " +
                    //            " ( " +
                    //            "    SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                    //            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                    //            " ) addrDtl " +
                    //            " ON idOrganization = organizationId " +
                    //            " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                    //            " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                    //            " WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ) ";


                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization";
                }

                //if (cnfId == 0 && dealerId == 0 && statusId == 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId NOT IN(" + notDelStatus + ")";
                //else if (cnfId == 0 && dealerId == 0 && statusId > 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId IN(" + statusId + ")";
                //else if (cnfId == 0 && dealerId > 0 && statusId > 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.dealerOrgId=" + dealerId + "AND bookings.statusId IN(" + statusId + ")";
                //else if (cnfId == 0 && dealerId > 0 && statusId == 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.dealerOrgId=" + dealerId + "AND bookings.statusId NOT IN(" + notDelStatus + ")";
                //else if (cnfId > 0 && dealerId == 0 && statusId == 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + "AND bookings.statusId NOT IN(" + notDelStatus + ")";
                //else if (cnfId > 0 && dealerId > 0 && statusId == 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                //else if (cnfId > 0 && dealerId == 0 && statusId > 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.statusId IN(" + statusId + ")";
                //else if (cnfId > 0 && dealerId > 0 && statusId > 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")";

                //Reshma Commented for add skip date filter for bulk booking
                if (cnfId == 0 && dealerId == 0 && String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId == 0 && dealerId == 0 && !String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId == 0 && dealerId > 0 && !String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId == 0 && dealerId > 0 && String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId == 0 && String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId > 0 && String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId == 0 && !String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId > 0 && dealerId > 0 && !String.IsNullOrEmpty(statusId))
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")";

                if (BookingId > 0)
                {
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId NOT IN(" + notDelStatus + ")" + "AND  bookings.bookingDisplayNo like '%" + BookingId + "%'";

                }
                if (bookingTypeId ==2)
                {
                    if (skipDateFilter == 0)
                        sqlQuery = sqlQuery + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                    else
                    {
                        if (cnfId == 0 && dealerId == 0 && String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.statusId NOT IN(" + notDelStatus + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId == 0 && dealerId == 0 && !String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE   bookings.statusId IN(" + statusId + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId == 0 && dealerId > 0 && !String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId == 0 && dealerId > 0 && String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId > 0 && dealerId == 0 && String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.cnFOrgId=" + cnfId + " AND bookings.statusId NOT IN(" + notDelStatus + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId > 0 && dealerId > 0 && String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId > 0 && dealerId == 0 && !String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE   bookings.cnFOrgId=" + cnfId + " AND bookings.statusId IN(" + statusId + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";
                        else if (cnfId > 0 && dealerId > 0 && !String.IsNullOrEmpty(statusId))
                            sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE  bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")" + " And  bookings.bookingTypeId =  " + bookingTypeId + "";

                    }
                }
                cmdSelect.CommandText = sqlQuery + " ORDER BY ISNULL(bookings.bookingDisplayNo,bookings.bookingRefId) ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        /// <summary>
        /// Priyanka [14-03-2018] : Added for booking summary report.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="typeId"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>

        public static List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO,Int32 cnfId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;


             string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF + "," + (Int32)Constants.TranStatusE.BOOKING_DELETE;



            String distStateJoin = String.Empty;
            String selectSqlQuery = String.Empty;
            String dateJoin = String.Empty;
            String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                    " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                 " INNER JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId " +
                               " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }


                if (isConfEn == 1)
                {

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization ";
                }

                conn.Open();
                distStateJoin = " LEFT JOIN tblOrganization orgCnf on orgCnf.idOrganization = bookings.cnFOrgId " +
                            " LEFT JOIN tblOrganization orgDealer on orgDealer.idOrganization = bookings.dealerOrgId " +
                            " LEFT JOIN tblAddress tblAddress on tblAddress.idAddr = orgDealer.addrId " +
                            " LEFT join dimDistrict dist on dist.idDistrict = tblAddress.districtId " +
                            " LEFT join dimState statelist  on statelist.idState = tblAddress.stateId " +
                            " where CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId IN (" + statusIds + ") ";

                String commonSelect = " bookings.bookingQty,bookings.createdOn as timeView from tblbookings bookings " + areConfJoin + distStateJoin;

                if (typeId == (int)Constants.SelectTypeE.DISTRICT)
                {
                    // selectSqlQuery = " select bookings.idBooking,dist.districtName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + sqlQuery + ")  AND tblAddress.districtId IN (" + masterId + ")";
                    selectSqlQuery = " select bookings.idBooking,orgDealer.firmName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + sqlQuery + ")  AND tblAddress.districtId IN (" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.STATE)
                {
                    selectSqlQuery = " select bookings.idBooking,statelist.stateName as displayName, " + commonSelect  + " AND bookings.dealerOrgId IN (" + sqlQuery  + ")  AND tblAddress.stateId IN(" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.CNF)
                {
                    selectSqlQuery = " select bookings.idBooking,orgCnf.firmName as displayName, " + commonSelect +  " AND bookings.cnFOrgId IN (" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.DEALER)
                {
                    selectSqlQuery = " select bookings.idBooking,orgDealer.firmName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + masterId + ")";

                }
                if (!string.IsNullOrEmpty(selectSqlQuery))
                    selectSqlQuery = selectSqlQuery + " And  bookings.BookingTypeId not in ("+ (int)Constants.BookingTypeE.Bulk+")" ;
                cmdSelect.CommandText = selectSqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingSummaryTO> list = ConvertDTToListForBookingSummaryRpt(sqlReader);
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








        public static TblBookingsTO SelectTblBookings(Int32 idBooking)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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

        public static TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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

        public static TblBookingsTO SelectTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingsTO> SelectTblBookingsRef(Int32 BookingRefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingRefId = " + BookingRefId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list;
                else return null;
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

        public static SalesTrackerAPI.DashboardModels.BookingInfo SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, int orgId, Int32 dealerId, DateTime date)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            string areConfJoin = string.Empty;
            SqlDataReader tblBookingsTODT = null;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();
                //if (tblUserRoleTO.RoleId == (int)Constants.SystemRoleTypeE.C_AND_F_AGENT)
                if (orgId > 0)
                {
                    whereCond = " AND cnFOrgId=" + orgId;
                }

                if (dealerId > 0)
                {
                    whereCond += " AND dealerOrgId=" + dealerId;
                }


                if (isConfEn == 1)
                {
                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = tblBookings.cnFOrgid AND tblBookings.dealerOrgId = userAreaDealer.idOrganization ";
                }


                cmdSelect.CommandText = " SELECT SUM(bookingQty) bookingQty, sum(COST) totalCost ,sum(COST)/SUM(bookingQty) avgPrice " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT bookingQty, bookingRate, (bookingQty * bookingRate) AS cost FROM tblBookings " + areConfJoin +
                                        " WHERE statusId IN(2,3,9,11) AND DAY(bookingDatetime) = " + date.Day + " AND MONTH(bookingDatetime) = " + date.Month + " AND YEAR(bookingDatetime) = " + date.Year + whereCond +
                                        " AND globalRateId = (SELECT TOP 1 idGlobalRate FROM tblGlobalRate ORDER BY createdOn DESC )" +
                                        " ) AS qryRes";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        SalesTrackerAPI.DashboardModels.BookingInfo tblBookingsTONew = new SalesTrackerAPI.DashboardModels.BookingInfo();
                        if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                            tblBookingsTONew.BookedQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                        if (tblBookingsTODT["avgPrice"] != DBNull.Value)
                            tblBookingsTONew.AvgPrice = Convert.ToDouble(tblBookingsTODT["avgPrice"].ToString());

                        return tblBookingsTONew;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblBookingsTO> ConvertDTToList(SqlDataReader tblBookingsTODT)
        {
            List<TblBookingsTO> tblBookingsTOList = new List<TblBookingsTO>();
            if (tblBookingsTODT != null)
            {
                while (tblBookingsTODT.Read())
                {
                    TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                    if (tblBookingsTODT["idBooking"] != DBNull.Value)
                        tblBookingsTONew.IdBooking = Convert.ToInt32(tblBookingsTODT["idBooking"].ToString());
                    if (tblBookingsTODT["cnFOrgId"] != DBNull.Value)
                        tblBookingsTONew.CnFOrgId = Convert.ToInt32(tblBookingsTODT["cnFOrgId"].ToString());
                    if (tblBookingsTODT["dealerOrgId"] != DBNull.Value)
                        tblBookingsTONew.DealerOrgId = Convert.ToInt32(tblBookingsTODT["dealerOrgId"].ToString());
                    if (tblBookingsTODT["deliveryDays"] != DBNull.Value)
                        tblBookingsTONew.DeliveryDays = Convert.ToInt32(tblBookingsTODT["deliveryDays"].ToString());
                    if (tblBookingsTODT["noOfDeliveries"] != DBNull.Value)
                        tblBookingsTONew.NoOfDeliveries = Convert.ToInt32(tblBookingsTODT["noOfDeliveries"].ToString());
                    if (tblBookingsTODT["isConfirmed"] != DBNull.Value)
                        tblBookingsTONew.IsConfirmed = Convert.ToInt32(tblBookingsTODT["isConfirmed"].ToString());
                    if (tblBookingsTODT["isJointDelivery"] != DBNull.Value)
                        tblBookingsTONew.IsJointDelivery = Convert.ToInt32(tblBookingsTODT["isJointDelivery"].ToString());
                    if (tblBookingsTODT["isSpecialRequirement"] != DBNull.Value)
                        tblBookingsTONew.IsSpecialRequirement = Convert.ToInt32(tblBookingsTODT["isSpecialRequirement"].ToString());
                    if (tblBookingsTODT["cdStructure"] != DBNull.Value)
                        tblBookingsTONew.CdStructure = Convert.ToDouble(tblBookingsTODT["cdStructure"].ToString());
                    if (tblBookingsTODT["statusId"] != DBNull.Value)
                        tblBookingsTONew.StatusId = Convert.ToInt32(tblBookingsTODT["statusId"].ToString());
                    if (tblBookingsTODT["isWithinQuotaLimit"] != DBNull.Value)
                        tblBookingsTONew.IsWithinQuotaLimit = Convert.ToInt32(tblBookingsTODT["isWithinQuotaLimit"].ToString());
                    if (tblBookingsTODT["globalRateId"] != DBNull.Value)
                        tblBookingsTONew.GlobalRateId = Convert.ToInt32(tblBookingsTODT["globalRateId"].ToString());
                    if (tblBookingsTODT["quotaDeclarationId"] != DBNull.Value)
                        tblBookingsTONew.QuotaDeclarationId = Convert.ToInt32(tblBookingsTODT["quotaDeclarationId"].ToString());
                    if (tblBookingsTODT["quotaQtyBforBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyBforBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyBforBooking"].ToString());
                    if (tblBookingsTODT["quotaQtyAftBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyAftBooking"].ToString());
                    if (tblBookingsTODT["createdBy"] != DBNull.Value)
                        tblBookingsTONew.CreatedBy = Convert.ToInt32(tblBookingsTODT["createdBy"].ToString());
                    if (tblBookingsTODT["createdOn"] != DBNull.Value)
                        tblBookingsTONew.CreatedOn = Convert.ToDateTime(tblBookingsTODT["createdOn"].ToString());
                    if (tblBookingsTODT["updatedBy"] != DBNull.Value)
                        tblBookingsTONew.UpdatedBy = Convert.ToInt32(tblBookingsTODT["updatedBy"].ToString());
                    if (tblBookingsTODT["bookingDatetime"] != DBNull.Value)
                        tblBookingsTONew.BookingDatetime = Convert.ToDateTime(tblBookingsTODT["bookingDatetime"].ToString());
                    if (tblBookingsTODT["statusDate"] != DBNull.Value)
                        tblBookingsTONew.StatusDate = Convert.ToDateTime(tblBookingsTODT["statusDate"].ToString());
                    if (tblBookingsTODT["updatedOn"] != DBNull.Value)
                        tblBookingsTONew.UpdatedOn = Convert.ToDateTime(tblBookingsTODT["updatedOn"].ToString());
                    if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                        tblBookingsTONew.BookingQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                    if (tblBookingsTODT["bookingRate"] != DBNull.Value)
                        tblBookingsTONew.BookingRate = Convert.ToDouble(tblBookingsTODT["bookingRate"].ToString());
                    if (tblBookingsTODT["comments"] != DBNull.Value)
                        tblBookingsTONew.Comments = Convert.ToString(tblBookingsTODT["comments"].ToString());

                    if (tblBookingsTODT["cnfName"] != DBNull.Value)
                        tblBookingsTONew.CnfName = Convert.ToString(tblBookingsTODT["cnfName"].ToString());
                    if (tblBookingsTODT["dealerName"] != DBNull.Value)
                        tblBookingsTONew.DealerName = Convert.ToString(tblBookingsTODT["dealerName"].ToString());

                    if (tblBookingsTODT["statusName"] != DBNull.Value)
                        tblBookingsTONew.Status = Convert.ToString(tblBookingsTODT["statusName"].ToString());

                    if (tblBookingsTODT["pendingQty"] != DBNull.Value)
                        tblBookingsTONew.PendingQty = Convert.ToDouble(tblBookingsTODT["pendingQty"].ToString());

                    if (tblBookingsTODT["authReasons"] != DBNull.Value)
                        tblBookingsTONew.AuthReasons = Convert.ToString(tblBookingsTODT["authReasons"].ToString());
                    if (tblBookingsTODT["cdStructureId"] != DBNull.Value)
                        tblBookingsTONew.CdStructureId = Convert.ToInt32(tblBookingsTODT["cdStructureId"].ToString());

                    if (tblBookingsTODT["parityId"] != DBNull.Value)
                        tblBookingsTONew.ParityId = Convert.ToInt32(tblBookingsTODT["parityId"].ToString());
                    //CommonDAO.SetDateStandards(tblBookingsTONew);

                    if (tblBookingsTODT["orcAmt"] != DBNull.Value)
                        tblBookingsTONew.OrcAmt = Convert.ToDouble(tblBookingsTODT["orcAmt"].ToString());
                    if (tblBookingsTODT["orcMeasure"] != DBNull.Value)
                        tblBookingsTONew.OrcMeasure = Convert.ToString(tblBookingsTODT["orcMeasure"].ToString());
                    if (tblBookingsTODT["billingName"] != DBNull.Value)
                        tblBookingsTONew.BillingName = Convert.ToString(tblBookingsTODT["billingName"].ToString());

                    //Sanjay [2017-06-06]
                    if (tblBookingsTODT["poNo"] != DBNull.Value)
                        tblBookingsTONew.PoNo = Convert.ToString(tblBookingsTODT["poNo"].ToString());

                    //Saket [2018-12-07] Added
                    if (tblBookingsTODT["bookingRefId"] != DBNull.Value)
                        tblBookingsTONew.BookingRefId = Convert.ToInt32(tblBookingsTODT["bookingRefId"].ToString());

                    if (tblBookingsTODT["bookingDisplayNo"] != DBNull.Value)
                        tblBookingsTONew.BookingDisplayNo = Convert.ToString(tblBookingsTODT["bookingDisplayNo"].ToString());

                    if (tblBookingsTODT["sizesQty"] != DBNull.Value)
                        tblBookingsTONew.SizesQty = Convert.ToDouble(tblBookingsTODT["sizesQty"].ToString());

                    if (tblBookingsTODT["dealerVillageName"] != DBNull.Value)
                        tblBookingsTONew.DealerVillageName = Convert.ToString(tblBookingsTODT["dealerVillageName"].ToString());
                    if (tblBookingsTODT["dealerTalukaName"] != DBNull.Value)
                        tblBookingsTONew.DealerTalukaName = Convert.ToString(tblBookingsTODT["dealerTalukaName"].ToString());
                    if (tblBookingsTODT["dealerDistrictName"] != DBNull.Value)
                        tblBookingsTONew.DealerDistrictName = Convert.ToString(tblBookingsTODT["dealerDistrictName"].ToString());
                    if (tblBookingsTODT["dealerStateName"] != DBNull.Value)
                        tblBookingsTONew.DealerStateName = Convert.ToString(tblBookingsTODT["dealerStateName"].ToString());

                    if (tblBookingsTODT["bookingTypeId"] != DBNull.Value)
                        tblBookingsTONew.BookingTypeId = Convert.ToInt32(tblBookingsTODT["bookingTypeId"].ToString());
                    if (tblBookingsTODT["BookingTypeName"] != DBNull.Value)
                        tblBookingsTONew.BookingTypeName  = Convert.ToString(tblBookingsTODT["BookingTypeName"].ToString());

                    tblBookingsTOList.Add(tblBookingsTONew);
                }
            }
            return tblBookingsTOList;
        }


        /// <summary>
        /// Priyanka [21-03-2018]: Added ConvertDTTOList For Booking Summary Report
        /// </summary>
        /// <param name="tblBookingsSummaryTODT"></param>
        /// <returns></returns>
        public static List<TblBookingSummaryTO> ConvertDTToListForBookingSummaryRpt(SqlDataReader tblBookingsSummaryTODT)
        {
            List<TblBookingSummaryTO> tblBookingsSummaryTOList = new List<TblBookingSummaryTO>();
            if (tblBookingsSummaryTODT != null)
            {
                while (tblBookingsSummaryTODT.Read())
                {
                    TblBookingSummaryTO tblBookingsSummaryTONew = new TblBookingSummaryTO();

                    if (tblBookingsSummaryTODT["idBooking"] != DBNull.Value)
                        tblBookingsSummaryTONew.IdBooking = Convert.ToInt32(tblBookingsSummaryTODT["idBooking"].ToString());

                    if (tblBookingsSummaryTODT["displayName"] != DBNull.Value)
                        tblBookingsSummaryTONew.DisplayName = Convert.ToString(tblBookingsSummaryTODT["displayName"].ToString());

                    if (tblBookingsSummaryTODT["bookingQty"] != DBNull.Value)
                        tblBookingsSummaryTONew.BookingQty = Convert.ToDouble(tblBookingsSummaryTODT["bookingQty"].ToString());

                    if (tblBookingsSummaryTODT["timeView"] != DBNull.Value)
                        tblBookingsSummaryTONew.TimeView = Convert.ToDateTime(tblBookingsSummaryTODT["timeView"].ToString());

                    tblBookingsSummaryTOList.Add(tblBookingsSummaryTONew);
                }
            }
            return tblBookingsSummaryTOList;
        }








        public static Dictionary<Int32, Double> SelectBookingsPendingQtyDCT(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Dictionary<Int32, Double> pendingQtyDCT = new Dictionary<int, double>();
            String statusIds = (int)Constants.TranStatusE.BOOKING_APPROVED + "," + (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
            try
            {
                cmdSelect.CommandText = " SELECT idBooking,pendingQty FROM tblBookings " +
                                        " WHERE statusId IN(" + statusIds + " ) AND pendingQty > 0";

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
                        if (reader["idBooking"] != DBNull.Value)
                            bookingId = Convert.ToInt32(reader["idBooking"].ToString());
                        if (reader["pendingQty"] != DBNull.Value)
                            pendingQty = Convert.ToDouble(reader["pendingQty"].ToString());

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

        /// <summary>
        /// Vijaymala [2017-09-11] added to get booking list to generate booking graph
        /// </summary>
        public static List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, TblUserRoleTO tblUserRoleTO, Int32 dealerId)
        {
         
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String SelectQuery= String.Empty;
            string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_CANDF;
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            DateTime sysDate = Constants.ServerDateTime;
            string whereCond = string.Empty;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();
                SelectQuery = " Select bookings.cnFOrgId,sum(bookings.bookingQty)bookingQty,org.firmName cnfName,  " +
                "  sum(bookings.bookingQty * bookings.bookingRate) / sum(bookings.bookingQty)avgPrice from tblBookings bookings  " +
                "  Inner Join tblOrganization org On bookings.cnFOrgId = org.idOrganization  " +
                "  Inner Join dimStatus ON dimStatus.idStatus = bookings.statusId  ";


                if (isConfEn == 1)
                {

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization ";
                }

                //if (tblUserRoleTO.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                if (OrganizationId > 0 && tblUserRoleTO.RoleTypeId == (int)Constants.SystemRoleTypeE.C_AND_F_AGENT)
                {
                    whereCond = " AND bookings.cnFOrgId=" + OrganizationId;
                }
                if (dealerId > 0)
                {
                    whereCond += " AND bookings.dealerOrgId=" + dealerId;

                }
                sqlQuery = SelectQuery + areConfJoin + "  WHERE  bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + whereCond + " group by bookings.cnFOrgId,org.firmName";

                //if (tblUserRoleTO.RoleId != (int)Constants.SystemRoleTypeE.C_AND_F_AGENT)
                //{
                //    sqlQuery = SelectQuery + areConfJoin + "  WHERE  bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + " group by bookings.cnFOrgId,org.firmName";
                //}
                //else
                //{
                //    sqlQuery = SelectQuery + areConfJoin + " WHERE bookings.cnFOrgId=" + OrganizationId + " AND  bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + " group by bookings.cnFOrgId,org.firmName";
                //}
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<BookingGraphRptTO> list = ConvertDTToListForGraph(sqlReader);
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
        /// <summary>
        /// Vijaymala [2017-09-11] added to convert dt to list to generate booking graph
        /// </summary>
        public static List<BookingGraphRptTO> ConvertDTToListForGraph(SqlDataReader tblBookingsGraphRptTODT)
        {
            List<BookingGraphRptTO> bookingGraphRptTOList = new List<BookingGraphRptTO>();
            if (tblBookingsGraphRptTODT != null)
            {
                while (tblBookingsGraphRptTODT.Read())
                {
                    BookingGraphRptTO tblBookingsGraphRptTONew = new BookingGraphRptTO();
                    
                    //if (tblBookingsGraphRptTODT["bookingId"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.BookingId = Convert.ToInt32(tblBookingsGraphRptTODT["bookingId"].ToString());
                    if (tblBookingsGraphRptTODT["cnFOrgId"] != DBNull.Value)
                        tblBookingsGraphRptTONew.CnFOrgId = Convert.ToInt32(tblBookingsGraphRptTODT["cnFOrgId"].ToString());
                    //if (tblBookingsGraphRptTODT["dealerOrgId"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.DealerOrgId = Convert.ToInt32(tblBookingsGraphRptTODT["dealerOrgId"].ToString());
                    if (tblBookingsGraphRptTODT["cnfName"] != DBNull.Value)
                        tblBookingsGraphRptTONew.CnfName = Convert.ToString(tblBookingsGraphRptTODT["cnfName"].ToString());
                    //if (tblBookingsGraphRptTODT["dealerName"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.DealerName = Convert.ToString(tblBookingsGraphRptTODT["dealerName"].ToString());
                    if (tblBookingsGraphRptTODT["bookingQty"] != DBNull.Value)
                        tblBookingsGraphRptTONew.BookingQty = Convert.ToDouble(tblBookingsGraphRptTODT["bookingQty"].ToString());
                    //if (tblBookingsGraphRptTODT["bookingRate"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.BookingRate = Convert.ToDouble(tblBookingsGraphRptTODT["bookingRate"].ToString());
                    if (tblBookingsGraphRptTODT["avgPrice"] != DBNull.Value)
                        tblBookingsGraphRptTONew.AvgPrice = Convert.ToDouble(tblBookingsGraphRptTODT["avgPrice"].ToString());
                    bookingGraphRptTOList.Add(tblBookingsGraphRptTONew);
                }
            }
            return bookingGraphRptTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblBookings(TblBookingsTO tblBookingsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingsTO, cmdInsert);
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

        public static int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookings]( " +
                            "  [cnFOrgId]" +
                            " ,[dealerOrgId]" +
                            " ,[deliveryDays]" +
                            " ,[noOfDeliveries]" +
                            " ,[isConfirmed]" +
                            " ,[isJointDelivery]" +
                            " ,[isSpecialRequirement]" +
                            " ,[cdStructure]" +
                            " ,[statusId]" +
                            " ,[isWithinQuotaLimit]" +
                            " ,[globalRateId]" +
                            " ,[quotaDeclarationId]" +
                            " ,[quotaQtyBforBooking]" +
                            " ,[quotaQtyAftBooking]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[updatedBy]" +
                            " ,[bookingDatetime]" +
                            " ,[statusDate]" +
                            " ,[updatedOn]" +
                            " ,[bookingQty]" +
                            " ,[bookingRate]" +
                            " ,[comments]" +
                            " ,[pendingQty]" +
                            " ,[authReasons]" +
                            " ,[cdStructureId]" +
                            " ,[parityId]" +
                            " ,[orcAmt]" +
                            " ,[orcMeasure]" +
                            " ,[billingName]" +
                            " ,[poNo]" +
                            " ,[bookingRefId]" +
                            " ,[bookingDisplayNo]" +
                            " ,[sizesQty]," +
                            "   [BookingTypeId] " +
                            " )" +
                " VALUES (" +
                            "  @CnFOrgId " +
                            " ,@DealerOrgId " +
                            " ,@DeliveryDays " +
                            " ,@NoOfDeliveries " +
                            " ,@IsConfirmed " +
                            " ,@IsJointDelivery " +
                            " ,@IsSpecialRequirement " +
                            " ,@CdStructure " +
                            " ,@StatusId " +
                            " ,@IsWithinQuotaLimit " +
                            " ,@GlobalRateId " +
                            " ,@QuotaDeclarationId " +
                            " ,@QuotaQtyBforBooking " +
                            " ,@QuotaQtyAftBooking " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedBy " +
                            " ,@BookingDatetime " +
                            " ,@StatusDate " +
                            " ,@UpdatedOn " +
                            " ,@BookingQty " +
                            " ,@BookingRate " +
                            " ,@Comments " +
                            " ,@PendingQty " +
                            " ,@AuthReasons " +
                            " ,@cdStructureId " +
                            " ,@parityId " +
                            " ,@orcAmt " +
                            " ,@orcMeasure " +
                            " ,@billingName " +
                            " ,@poNo " +
                            " ,@BookingRefId " +
                            " ,@BookingDisplayNo " +
                            " ,@SizesQty," +
                            "  @BookingTypeId " + 
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            cmdInsert.Parameters.Add("@CnFOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CnFOrgId);
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.DealerOrgId);
            cmdInsert.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = tblBookingsTO.DeliveryDays;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblBookingsTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsConfirmed;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@IsSpecialRequirement", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsSpecialRequirement;
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.Decimal).Value = tblBookingsTO.CdStructure;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingsTO.StatusId;
            cmdInsert.Parameters.Add("@IsWithinQuotaLimit", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsWithinQuotaLimit;
            cmdInsert.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.GlobalRateId);
            cmdInsert.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaDeclarationId);
            cmdInsert.Parameters.Add("@QuotaQtyBforBooking", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyBforBooking);
            cmdInsert.Parameters.Add("@QuotaQtyAftBooking", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyAftBooking);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@BookingDatetime", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.BookingDatetime;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.StatusDate;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingQty;
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingRate;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.Comments);
            cmdInsert.Parameters.Add("@PendingQty", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PendingQty);
            cmdInsert.Parameters.Add("@AuthReasons", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.AuthReasons);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CdStructureId);
            cmdInsert.Parameters.Add("@parityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ParityId);
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcAmt);
            cmdInsert.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcMeasure);
            cmdInsert.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BillingName);
            cmdInsert.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoNo);
            cmdInsert.Parameters.Add("@BookingRefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingRefId);
            cmdInsert.Parameters.Add("@BookingDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingDisplayNo);
            cmdInsert.Parameters.Add("@SizesQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SizesQty);    //PRiyanka [21-06-2018] Added for SHIVANGI. 
            cmdInsert.Parameters.Add("@BookingTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingTypeId);//Reshma Added For Bulk Booking

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingsTO.IdBooking = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblBookings(TblBookingsTO tblBookingsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public static int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public static int UpdateSizeQuantity(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblBookings] SET " +
                                " [sizesQty] = @SizeQty " +
                                " WHERE idBooking = @IdBooking ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
                cmdUpdate.Parameters.Add("@SizeQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.SizesQty;

                return cmdUpdate.ExecuteNonQuery();
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

        public static int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblBookings] SET " +
                                "  [updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[pendingQty] = @PendingQty" +
                                " WHERE idBooking = @IdBooking ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.PendingQty;
                return cmdUpdate.ExecuteNonQuery();
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

        public static int ExecuteUpdationCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookings] SET " +
                            "  [cnFOrgId]= @CnFOrgId" +
                            " ,[dealerOrgId]= @DealerOrgId" +
                            " ,[deliveryDays]= @DeliveryDays" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[isConfirmed]= @IsConfirmed" +
                            " ,[isJointDelivery]= @IsJointDelivery" +
                            " ,[isSpecialRequirement]= @IsSpecialRequirement" +
                            " ,[cdStructure]= @CdStructure" +
                            " ,[statusId]= @StatusId" +
                            " ,[isWithinQuotaLimit]= @IsWithinQuotaLimit" +
                            " ,[globalRateId]= @GlobalRateId" +
                            " ,[quotaDeclarationId]= @QuotaDeclarationId" +
                            " ,[quotaQtyBforBooking]= @QuotaQtyBforBooking" +
                            " ,[quotaQtyAftBooking]= @QuotaQtyAftBooking" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[bookingDatetime]= @BookingDatetime" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[bookingQty]= @BookingQty" +
                            " ,[bookingRate]= @BookingRate" +
                            " ,[comments] = @Comments" +
                            " ,[pendingQty] = @pendingQty" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[parityId] = @parityId" +
                            " ,[orcAmt] = @orcAmt" +
                            " ,[orcMeasure] = @orcMeasure" +
                            " ,[billingName] = @billingName" +
                            " ,[poNo] = @poNo" +
                            " ,[authReasons] =@AuthReasons" +                               //Priyanka [15-04-2019]
                            " ,[sizesQty]= @SizesQty," +
                            " BookingTypeId=@BookingTypeId " +   
                            " WHERE  [idBooking] = @IdBooking";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            cmdUpdate.Parameters.Add("@CnFOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CnFOrgId);
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblBookingsTO.DealerOrgId;
            cmdUpdate.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = tblBookingsTO.DeliveryDays;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblBookingsTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@IsSpecialRequirement", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsSpecialRequirement;
            cmdUpdate.Parameters.Add("@CdStructure", System.Data.SqlDbType.Decimal).Value = tblBookingsTO.CdStructure;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingsTO.StatusId;
            cmdUpdate.Parameters.Add("@IsWithinQuotaLimit", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsWithinQuotaLimit;
            cmdUpdate.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = tblBookingsTO.GlobalRateId;
            cmdUpdate.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = tblBookingsTO.QuotaDeclarationId;
            cmdUpdate.Parameters.Add("@QuotaQtyBforBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.QuotaQtyBforBooking;
            cmdUpdate.Parameters.Add("@QuotaQtyAftBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.QuotaQtyAftBooking;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@BookingDatetime", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.BookingDatetime;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.StatusDate;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingQty;
            cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingRate;
            cmdUpdate.Parameters.Add("@pendingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.PendingQty;
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.Comments);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CdStructureId);
            cmdUpdate.Parameters.Add("@parityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ParityId);
            cmdUpdate.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcAmt);
            cmdUpdate.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcMeasure);
            cmdUpdate.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BillingName);
            cmdUpdate.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoNo);
            cmdUpdate.Parameters.Add("@authReasons", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.AuthReasons);  //Priyanka [15-04-2019]
            cmdUpdate.Parameters.Add("@SizesQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SizesQty);    //Priyanka [21-06-2018]
            cmdUpdate.Parameters.Add("@BookingTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingTypeId);    //Priyanka [21-06-2018]

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblBookings(Int32 idBooking)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBooking, cmdDelete);
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

        public static int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBooking, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBooking, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookings] " +
            " WHERE idBooking = " + idBooking + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        //Sudhir[30-April-2018] Added for Update ParityId is NULL.
        public static int UpdateParityIdNull(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblBookings] SET " +
                                "  [parityId]= @ParityId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@ParityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(0);
                return cmdUpdate.ExecuteNonQuery();
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
    }
}
