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
    public class TblPurchaseVehicleSpotEntryDAO : ITblPurchaseVehicleSpotEntryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleSpotEntryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblPurchaseVehicleSpotEntry.*,tblPurchaseEnquiry.isAutoSpotVehSauda,tblPurchaseEnquiry.enqDisplayNo, tblPurchaseEnquiry.idPurchaseEnquiry,tblPurchaseEnquiry.CreatedOn,tblPurchaseEnquiry.EnqDisplayNo,tblPurchaseEnquiry.saudaCreatedOn,tblProdClassification.prodClassDesc,tblOrganization.firmName from tblPurchaseVehicleSpotEntry tblPurchaseVehicleSpotEntry " +
                                 " left Join tblOrganization tblOrganization on tblOrganization.idOrganization=tblPurchaseVehicleSpotEntry.supplierId " +
                                 " left Join tblProdClassification tblProdClassification on tblProdClassification.idProdClass=tblPurchaseVehicleSpotEntry.prodClassId " +
                                 " left join tblPurchaseEnquiry tblPurchaseEnquiry  on tblPurchaseEnquiry .idPurchaseEnquiry=tblPurchaseVehicleSpotEntry.purchaseEnquiryId";

            //  " left Join dimVehicleType dimVehicleType on dimVehicleType.idVehicleType=tblPurchaseVehicleSpotEntry.vehicleTypeId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry()
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

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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


        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnquiryVehicleEntryTO(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                //   //  cmdSelect.CommandText=SELECT * FROM [TblPurchaseVehicleSpotEntryTO]"";
                //    String sqlSelectQry ="SELECT * FROM [TblPurchaseVehicleSpotEntryTO]";
                //      cmdSelect.CommandText=  sqlSelectQry;
                cmdSelect.CommandText += " where tblPurchaseVehicleSpotEntry.purchaseEnquiryId=" + purchaseEnquiryId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where tblPurchaseVehicleSpotEntry.purchaseEnquiryId=" + purchaseEnquiryId + " and ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 order by tblPurchaseVehicleSpotEntry.createdOn desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(sqlReader);
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
        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseEnqVehEntryTOList(Int32 purchaseEnquiryId, SqlConnection conn, SqlTransaction tran)
        {
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " where tblPurchaseVehicleSpotEntry.purchaseEnquiryId=" + purchaseEnquiryId + " and ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 order by tblPurchaseVehicleSpotEntry.createdOn desc";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(sqlReader);
                sqlReader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();

                cmdSelect.Dispose();
            }
        }








        // public  List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntryList(DateTime createdOn)
        // {
        //     String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //     SqlConnection conn = new SqlConnection(sqlConnStr);
        //     SqlCommand cmdSelect = new SqlCommand();
        //     try
        //     {
        //         conn.Open();
        //         cmdSelect.CommandText = SqlSelectQuery() + "Where tblPurchaseVehicleSpotEntry.statusId= " + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +" AND cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date order by tblPurchaseVehicleSpotEntry.createdOn desc";//New Status Vehicles
        //         cmdSelect.Connection = conn;
        //         cmdSelect.CommandType = System.Data.CommandType.Text;

        //         //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
        //         cmdSelect.CommandType = System.Data.CommandType.Text;
        //         cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = createdOn;
        //         SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //         List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
        //         reader.Dispose();
        //         return list;
        //     }
        //     catch(Exception ex)
        //     {
        //         return null;
        //     }
        //     finally
        //     {
        //         conn.Close();
        //         cmdSelect.Dispose();
        //     }
        // }

        //Prajakta[2019-03-11] Commented and added 
        //public  List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehicles(DateTime fromDate, DateTime toDate, Int32 loginUserId, Int32 id) //, SqlConnection conn, SqlTransaction tran)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();

        //        string whereStr = " and ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 ";
        //        string orderStr = " ORDER BY tblPurchaseVehicleSpotEntry.createdOn desc";
        //        string dateConditionStr = " WHERE cast(tblPurchaseVehicleSpotEntry.createdOn as date) BETWEEN @fromDate and @toDate ";

        //        if (loginUserId > 0)
        //        {
        //            cmdSelect.CommandText = SqlSelectQuery() + " where (supplierId in ( " +
        //                                 " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
        //                                 "  and ISNULL(isActive,0)=1 ) or ISNULL(supplierId,0)=0) " +
        //                                  //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
        //                                  dateConditionStr + " and tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +
        //                                 " " + whereStr;
        //        }
        //        else
        //        {
        //            //All records 
        //            if (id == 1)
        //            {
        //                cmdSelect.CommandText = SqlSelectQuery() + dateConditionStr + orderStr;
        //            }

        //            //Pending
        //            else if (id == 2)
        //            {
        //                cmdSelect.CommandText = SqlSelectQuery() + dateConditionStr + "AND ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) = 0 "
        //                    + whereStr + " AND  tblPurchaseVehicleSpotEntry.statusId != " 
        //                    + Convert.ToInt32(Stuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) + orderStr;
        //            }

        //            //Linked
        //            else if(id ==3)
        //            {
        //                cmdSelect.CommandText = SqlSelectQuery() + dateConditionStr + " AND  tblPurchaseVehicleSpotEntry.statusId != "
        //                    + Convert.ToInt32(Stuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED)  + " AND ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) > 0"
        //                    + orderStr;
        //            }

        //            //Rejected
        //            else
        //            {
        //                cmdSelect.CommandText = SqlSelectQuery() + dateConditionStr + "AND  tblPurchaseVehicleSpotEntry.statusId=" 
        //                    + Convert.ToInt32(Stuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) + orderStr;

        //            }
        //        }


        //        //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_PURCHASE_MANAGER_ROLE_ID.ToString());
        //        //if (tblConfigParamsTO != null)
        //        //{
        //        //    Int32 defaultRoleId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
        //        //    if (defaultRoleId == roleId)
        //        //    {
        //        //        cmdSelect.CommandText = SqlSelectQuery() + " where (supplierId in ( " +
        //        //                         " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
        //        //                         "  and ISNULL(isActive,0)=1 ) or ISNULL(supplierId,0)=0) " +
        //        //                          //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
        //        //                          dateConditionStr + " and tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +
        //        //                         " " + whereStr;
        //        //    }
        //        //    else
        //        //    {
        //        //        cmdSelect.CommandText = SqlSelectQuery() +
        //        //    "Where tblPurchaseVehicleSpotEntry.statusId= " + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +
        //        //     //" AND cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date " +
        //        //     dateConditionStr + whereStr;
        //        //    }
        //        //}


        //        // if (roleId == Convert.ToInt32(Constants.SystemRolesE.SYSTEM_ADMIN) || roleId == Convert.ToInt32(Constants.SystemRolesE.DIRECTOR))
        //        // {
        //        //     cmdSelect.CommandText = SqlSelectQuery() +
        //        //      "Where tblPurchaseVehicleSpotEntry.statusId= " + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +
        //        //       //" AND cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date " +
        //        //       dateConditionStr + whereStr;
        //        // }
        //        // else
        //        // {
        //        //     cmdSelect.CommandText = SqlSelectQuery() + " where (supplierId in ( " +
        //        //                            " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
        //        //                            "  and ISNULL(isActive,0)=1 ) or ISNULL(supplierId,0)=0) " +
        //        //                             //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
        //        //                             dateConditionStr + " and tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(Stuff.Constants.TranStatusE.New) +
        //        //                            " " + whereStr;

        //        // }


        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;
        //        cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
        //        cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
        //        SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
        //        reader.Dispose();
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        cmdSelect.Dispose();
        //        conn.Close();
        //    }
        //}

        public List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehicles(DateTime fromDate, DateTime toDate, String loginUserId, Int32 id, bool skipDatetime) //, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                string whereStr = " and ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 ";
                string orderStr = " ORDER BY tblPurchaseVehicleSpotEntry.createdOn desc";
                string dateConditionStr = "";
                if (!skipDatetime)
                {
                    dateConditionStr = "  cast(tblPurchaseVehicleSpotEntry.createdOn as date) BETWEEN @fromDate and @toDate ";
                }
                string loginUserCondition = "";

                if (!String.IsNullOrEmpty(loginUserId))
                {
                    loginUserCondition = "  (tblPurchaseVehicleSpotEntry.supplierId in ( " +
                                         " select organizationId from tblPurchaseManagerSupplier where userId IN (" + loginUserId + ") " +
                                         "  and ISNULL(tblPurchaseManagerSupplier.isActive,0)=1 ) or ISNULL(tblPurchaseVehicleSpotEntry.supplierId,0)=0)  ";

                }
                // if (loginUserId > 0)
                // {
                //     cmdSelect.CommandText = SqlSelectQuery() + dateConditionStr + " and (tblPurchaseVehicleSpotEntry.supplierId in ( " +
                //                          " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
                //                          "  and ISNULL(tblPurchaseManagerSupplier.isActive,0)=1 ) or ISNULL(tblPurchaseVehicleSpotEntry.supplierId,0)=0) AND" +
                //                           //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
                //                         " tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New) +
                //                          " " + whereStr;
                // }
                // else

                //All records 
                if (id == 1)
                {
                    cmdSelect.CommandText = SqlSelectQuery();
                    if (!skipDatetime || (loginUserCondition != null && loginUserCondition != ""))
                    {
                        cmdSelect.CommandText = SqlSelectQuery() + " where ";
                    }
                    if (!skipDatetime)
                    {
                        cmdSelect.CommandText += dateConditionStr;
                    }

                    if (!String.IsNullOrEmpty(loginUserCondition) && !String.IsNullOrEmpty(dateConditionStr))
                    {
                        cmdSelect.CommandText += " and ";
                    }
                    cmdSelect.CommandText += loginUserCondition + orderStr;
                }

                //Pending
                else if (id == 2)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " where ";
                    if (!skipDatetime)
                    {
                        cmdSelect.CommandText += dateConditionStr + " and ";
                    }
                    // cmdSelect.CommandText += loginUserCondition + " ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) = 0 "
                    //   + whereStr + " AND  tblPurchaseVehicleSpotEntry.statusId != "
                    //   + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) + orderStr;

                    string pendingCondition = getpendingSpotEntryList();

                    if (!String.IsNullOrEmpty(loginUserCondition))
                    {
                        loginUserCondition += " and ";
                        cmdSelect.CommandText += loginUserCondition + pendingCondition + orderStr;
                    }
                    else
                        cmdSelect.CommandText += pendingCondition + orderStr;
                }

                //Linked
                else if (id == 3)
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " where ";
                    if (!skipDatetime)
                    {
                        cmdSelect.CommandText += dateConditionStr + " and ";
                    }

                    string statusStr = "  tblPurchaseVehicleSpotEntry.statusId != "
                       + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) + " AND ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) > 0"
                       + orderStr;

                    if (!String.IsNullOrEmpty(loginUserCondition))
                        loginUserCondition += " and ";

                    cmdSelect.CommandText += loginUserCondition + statusStr;
                }

                //Rejected
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " where ";
                    if (!skipDatetime)
                    {
                        cmdSelect.CommandText += dateConditionStr + " and ";
                    }

                    string statusStr = " tblPurchaseVehicleSpotEntry.statusId="
                        + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) + orderStr;

                    if (!String.IsNullOrEmpty(loginUserCondition))
                        loginUserCondition += " and ";

                    cmdSelect.CommandText += loginUserCondition + statusStr;
                }



                //TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_PURCHASE_MANAGER_ROLE_ID.ToString());
                //if (tblConfigParamsTO != null)
                //{
                //    Int32 defaultRoleId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                //    if (defaultRoleId == roleId)
                //    {
                //        cmdSelect.CommandText = SqlSelectQuery() + " where (supplierId in ( " +
                //                         " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
                //                         "  and ISNULL(isActive,0)=1 ) or ISNULL(supplierId,0)=0) " +
                //                          //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
                //                          dateConditionStr + " and tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New) +
                //                         " " + whereStr;
                //    }
                //    else
                //    {
                //        cmdSelect.CommandText = SqlSelectQuery() +
                //    "Where tblPurchaseVehicleSpotEntry.statusId= " + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New) +
                //     //" AND cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date " +
                //     dateConditionStr + whereStr;
                //    }
                //}


                // if (roleId == Convert.ToInt32(Constants.SystemRolesE.SYSTEM_ADMIN) || roleId == Convert.ToInt32(Constants.SystemRolesE.DIRECTOR))
                // {
                //     cmdSelect.CommandText = SqlSelectQuery() +
                //      "Where tblPurchaseVehicleSpotEntry.statusId= " + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New) +
                //       //" AND cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date " +
                //       dateConditionStr + whereStr;
                // }
                // else
                // {
                //     cmdSelect.CommandText = SqlSelectQuery() + " where (supplierId in ( " +
                //                            " select organizationId from tblPurchaseManagerSupplier where userId=" + loginUserId +
                //                            "  and ISNULL(isActive,0)=1 ) or ISNULL(supplierId,0)=0) " +
                //                             //" cast(tblPurchaseVehicleSpotEntry.createdOn as date) = @date 
                //                             dateConditionStr + " and tblPurchaseVehicleSpotEntry.statusId=" + Convert.ToInt32(StaticStuff.Constants.TranStatusE.New) +
                //                            " " + whereStr;

                // }


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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
                conn.Close();
            }
        }

        private string getpendingSpotEntryList()
        {
            string whereStr = " ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 ";

            // return "  ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) = 0 "
            //           + whereStr + " AND  tblPurchaseVehicleSpotEntry.statusId != "
            //           + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED);


            return whereStr + " AND  tblPurchaseVehicleSpotEntry.statusId != "
                      + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED);
        }

        public DropDownTO SelectAllSpotEntryVehiclesCount(int pmId, int supplierId, int materialTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                //Pending spot entry list
                cmdSelect.CommandText = "select count(idVehicleSpotEntry) as noOfveh, SUM(spotVehicleQty) as qty from  tblPurchaseVehicleSpotEntry where ";

                // cmdSelect.CommandText += " ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId, 0) = 0 "
                //   + " AND  tblPurchaseVehicleSpotEntry.statusId != "
                //   + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED);

                cmdSelect.CommandText += getpendingSpotEntryList();
                if (supplierId > 0)
                {
                    cmdSelect.CommandText += " and supplierId = " + supplierId;
                }
                if (pmId > 0)
                {
                    cmdSelect.CommandText += " and (tblPurchaseVehicleSpotEntry.supplierId in ( " +
                                         " select organizationId from tblPurchaseManagerSupplier where userId IN (" + pmId + ") " +
                                         "  and ISNULL(tblPurchaseManagerSupplier.isActive,0)=1 ) or ISNULL(tblPurchaseVehicleSpotEntry.supplierId,0)=0)";
                }
                if (materialTypeId > 0)
                {
                    cmdSelect.CommandText += "and isnull(prodClassId,0) = " + materialTypeId;
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTONew = new DropDownTO();
                while (dateReader.Read())
                {
                    if (dateReader["noOfveh"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["noOfveh"].ToString());

                    if (dateReader["qty"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["qty"].ToString());

                    dropDownTONew.Text = "Spot Entry pending vehicles";
                }
                dateReader.Dispose();
                return dropDownTONew;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        public List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotVehCountForDashboard(String pmId, Int32 supplierId, Int32 materialTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                //Pending spot entry list
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ";

                cmdSelect.CommandText +=  getpendingSpotEntryList();

                if (supplierId > 0)
                {
                    cmdSelect.CommandText += " and supplierId = " + supplierId;
                }
                //if (pmId > 0)
                if (!String.IsNullOrEmpty(pmId))
                {
                    cmdSelect.CommandText += " and (tblPurchaseVehicleSpotEntry.supplierId in ( " +
                                         " select organizationId from tblPurchaseManagerSupplier where userId IN (" + pmId + ") " +
                                         "  and ISNULL(tblPurchaseManagerSupplier.isActive,0)=1 ) or ISNULL(tblPurchaseVehicleSpotEntry.supplierId,0)=0)";
                }
                if (materialTypeId > 0)
                {
                    cmdSelect.CommandText += "and isnull(tblPurchaseVehicleSpotEntry.prodClassId,0) = " + materialTypeId;
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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
                conn.Close();
            }
        }

        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVehicleSpotEntry = " + idVehicleSpotEntry + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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
        public TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTOByRootId(Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId = " + rootScheduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.purchaseScheduleSummaryId;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list[0];
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

        public TblPurchaseVehicleSpotEntryTO SelectTblPurchaseVehicleSpotEntryTOByRootId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId = " + rootScheduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.purchaseScheduleSummaryId;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                reader.Dispose();
            }
        }

        public List<TblPurchaseVehicleSpotEntryTO> SelectTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVehicleSpotEntry = " + idVehicleSpotEntry + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
                return list;
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

        public TblPurchaseVehicleSpotEntryTO SelectSpotVehicleAgainstScheduleId(Int32 purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId = " + purchaseScheduleSummaryId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list[0];
                else
                    return null;
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

        public List<TblPurchaseVehicleSpotEntryTO> SelectAllTblPurchaseVehicleSpotEntry(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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

        public List<VehicleNumber> SelectAllVehicles()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT distinct vehicleNo FROM tblPurchaseVehicleSpotEntry";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleNumber> list = new List<VehicleNumber>();

                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            vehicleNo = Convert.ToString(sqlReader["vehicleNo"].ToString());

                        if (!string.IsNullOrEmpty(vehicleNo))
                        {
                            String[] vehNoPart = vehicleNo.Split(' ');
                            if (vehNoPart.Length == 4)
                            {
                                VehicleNumber vehicleNumber = new VehicleNumber();
                                for (int i = 0; i < vehNoPart.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        vehicleNumber.StateCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 1)
                                    {
                                        vehicleNumber.DistrictCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 2)
                                    {
                                        vehicleNumber.UniqueLetters = vehNoPart[i];
                                        if (vehicleNumber.UniqueLetters == "undefined")
                                            vehicleNumber.UniqueLetters = "";
                                        else
                                            vehicleNumber.UniqueLetters = vehicleNumber.UniqueLetters.ToUpper();
                                    }
                                    if (i == 3)
                                    {
                                        if (Constants.IsInteger(vehNoPart[i]))
                                        {
                                            vehicleNumber.VehicleNo = Convert.ToInt32(vehNoPart[i]);
                                        }
                                        else break;
                                    }
                                }

                                if (vehicleNumber.VehicleNo > 0)
                                    list.Add(vehicleNumber);
                            }
                        }
                    }
                }

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

        public List<TblPurchaseVehicleSpotEntryTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleSpotEntryTODT)
        {
            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = new List<TblPurchaseVehicleSpotEntryTO>();
            if (tblPurchaseVehicleSpotEntryTODT != null)
            {
                while (tblPurchaseVehicleSpotEntryTODT.Read())
                {
                    TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTONew = new TblPurchaseVehicleSpotEntryTO();

                    if (tblPurchaseVehicleSpotEntryTODT["idVehicleSpotEntry"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.IdVehicleSpotEntry = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["idVehicleSpotEntry"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.SupplierId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["supplierId"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["vehicleTypeId"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["statusId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.StatusId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["statusId"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.CreatedBy = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["createdBy"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["statusDate"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.StatusDate = Convert.ToDateTime(tblPurchaseVehicleSpotEntryTODT["statusDate"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseVehicleSpotEntryTODT["createdOn"].ToString());
                    // if(tblPurchaseVehicleSpotEntryTODT["vehicleQtyMT"] != DBNull.Value)
                    // tblPurchaseVehicleSpotEntryTONew.VehicleQtyMT = Convert.ToDouble( tblPurchaseVehicleSpotEntryTODT["vehicleQtyMT"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["location"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.Location = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["location"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.VehicleNo = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["vehicleNo"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["driverName"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.DriverName = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["driverName"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["remark"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.Remark = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["remark"].ToString());
                    if (tblPurchaseVehicleSpotEntryTODT["locationId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.LocationId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["locationId"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["firmName"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.SupplierName = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["firmName"].ToString());

                    //    if(tblPurchaseVehicleSpotEntryTODT["vehicleTypeDesc"] != DBNull.Value)
                    //         tblPurchaseVehicleSpotEntryTONew.VehicleTypeDesc = Convert.ToString( tblPurchaseVehicleSpotEntryTODT["vehicleTypeDesc"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["stateId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.StateId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["stateId"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["driverContactNo"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.DriverContactNo = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["driverContactNo"].ToString());


                    if (tblPurchaseVehicleSpotEntryTODT["spotVehicleQty"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.SpotVehicleQty = Convert.ToDouble(tblPurchaseVehicleSpotEntryTODT["spotVehicleQty"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["prodClassId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.ProdClassId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["prodClassId"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["prodClassDesc"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.ProdClassType = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["prodClassDesc"].ToString());


                    if (tblPurchaseVehicleSpotEntryTODT["isLinkToExistingSauda"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.IsLinkToExistingSauda = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["isLinkToExistingSauda"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["purchaseScheduleSummaryId"].ToString());

                    if (tblPurchaseVehicleSpotEntryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseVehicleSpotEntryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["purchaseEnquiryId"].ToString());

                    //Prajakta[2019-03-08] Added
                    if (tblPurchaseVehicleSpotEntryTONew.PurchaseEnquiryId > 0)
                    {
                        tblPurchaseVehicleSpotEntryTONew.BookingTO = new TblPurchaseEnquiryTO();

                        if (tblPurchaseVehicleSpotEntryTODT["idPurchaseEnquiry"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.BookingTO.IdPurchaseEnquiry = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["idPurchaseEnquiry"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["CreatedOn"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.BookingTO.CreatedOn = Convert.ToDateTime(tblPurchaseVehicleSpotEntryTODT["CreatedOn"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["EnqDisplayNo"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.BookingTO.EnqDisplayNo = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["EnqDisplayNo"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["saudaCreatedOn"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.BookingTO.SaudaCreatedOn = Convert.ToDateTime(tblPurchaseVehicleSpotEntryTODT["saudaCreatedOn"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.BookingTO.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["isAutoSpotVehSauda"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["isAutoSpotVehSauda"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.IsAutoSpotVehSauda = Convert.ToInt32(tblPurchaseVehicleSpotEntryTODT["isAutoSpotVehSauda"].ToString());

                        if (tblPurchaseVehicleSpotEntryTODT["enqDisplayNo"] != DBNull.Value)
                            tblPurchaseVehicleSpotEntryTONew.EnqDisplayNo = Convert.ToString(tblPurchaseVehicleSpotEntryTODT["enqDisplayNo"].ToString());

                    }


                    tblPurchaseVehicleSpotEntryTOList.Add(tblPurchaseVehicleSpotEntryTONew);
                }
            }
            return tblPurchaseVehicleSpotEntryTOList;
        }

        //Priyanka [14-03-2019]
        public List<TblPurchaseVehicleSpotEntryTO> SelectAllSpotEntryVehiclesPending(string vehicleNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                string whereStr = " WHERE ISNULL(tblPurchaseVehicleSpotEntry.purchaseScheduleSummaryId,0)=0 ";
                string orderStr = " ORDER BY tblPurchaseVehicleSpotEntry.createdOn desc";

                //Pending

                cmdSelect.CommandText = SqlSelectQuery() + whereStr + " AND  tblPurchaseVehicleSpotEntry.statusId != "
                              + Convert.ToInt32(StaticStuff.Constants.TranStatusE.SPOT_ENTRY_VEHICLE_REJECTED) +
                              " AND tblPurchaseVehicleSpotEntry.vehicleNo ='" + vehicleNo + "'" + orderStr;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleSpotEntryTO> list = ConvertDTToList(reader);
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
                conn.Close();
            }
        }

        #endregion

        #region Insertion


        public int InsertTblRecyleDocuments(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommandDocs(tblRecycleDocumentTO, cmdInsert);
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


        public int ExecuteInsertionCommandDocs(TblRecycleDocumentTO tblRecycleDocumentsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRecycleDocument]( " +
            "  [txnId]" +
            "  ,[txnTypeId]" +
            " ,[documentId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[isActive]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +

            "  @txnId " +
            "  ,@txnTypeId " +
            " ,@DocumentId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@isActive " +
            " ,@UpdatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@txnTypeId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.TxnTypeId;
            cmdInsert.Parameters.Add("@txnId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.TxnId;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.IsActive;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.DocumentId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentsTO.UpdatedOn;
            return cmdInsert.ExecuteNonQuery();
        }


        public int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehicleSpotEntryTO, cmdInsert);
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

        public int InsertTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehicleSpotEntryTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlCommand cmdInsert)
        {
            Int32 vehicleSpotEntryId;
            String sqlQuery = @" INSERT INTO [tblPurchaseVehicleSpotEntry]( " +
            //"  [idVehicleSpotEntry]" +
            "  [supplierId]" +
            " ,[vehicleTypeId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[statusDate]" +
            " ,[createdOn]" +
            //" ,[vehicleQtyMT]" +
            " ,[location]" +
            " ,[vehicleNo]" +
            " ,[driverName]" +
            " ,[remark]" +
            " ,[locationId]" +
            " ,[purchaseEnquiryId]" +
            " ,[stateId]" +
            " ,[driverContactNo]" +
            " ,[spotVehicleQty]" +
             " ,[prodClassId]" +
             " ,[isLinkToExistingSauda]" +
             " ,[purchaseScheduleSummaryId]" +
            " )" +
" VALUES (" +
            //"  @IdVehicleSpotEntry " +
            "  @SupplierId " +
            " ,@VehicleTypeId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@StatusDate " +
            " ,@CreatedOn " +
            //" ,@vehicleQtyMT " +
            " ,@Location " +
            " ,@VehicleNo " +
            " ,@DriverName " +
            " ,@Remark " +
            " ,@locationId " +
            " ,@purchaseEnquiryId " +
            " ,@stateId " +
            " ,@driverContactNo " +
            ",@spotVehicleQty" +
             ",@prodClassId" +
             ",@isLinkToExistingSauda" +
             ",@PurchaseScheduleSummaryId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.SupplierId);
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.StatusId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.CreatedBy);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.StatusDate);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.CreatedOn);
            //cmdInsert.Parameters.Add("@VehicleQtyMT", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.VehicleQtyMT);
            cmdInsert.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.Location);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehicleSpotEntryTO.VehicleNo;
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.DriverName);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.Remark);
            cmdInsert.Parameters.Add("@locationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.LocationId);
            cmdInsert.Parameters.Add("@purchaseEnquiryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.PurchaseEnquiryId);
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.StateId);
            cmdInsert.Parameters.Add("@driverContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.DriverContactNo);
            cmdInsert.Parameters.Add("@spotVehicleQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.SpotVehicleQty);
            cmdInsert.Parameters.Add("@prodClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.ProdClassId);
            cmdInsert.Parameters.Add("@isLinkToExistingSauda", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.IsLinkToExistingSauda);
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.PurchaseScheduleSummaryId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                vehicleSpotEntryId = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return vehicleSpotEntryId;
                //return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehicleSpotEntryTO, cmdUpdate);
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

        public int UpdateTblPurchaseVehicleSpotEntry(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehicleSpotEntryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehicleSpotEntry] SET " +
            //"  [idVehicleSpotEntry] = @IdVehicleSpotEntry" +
            "  [supplierId]= @SupplierId" +
            " ,[location]= @Location" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            //" ,[vehicleQtyMT]= @VehicleQtyMT" +
            " ,[driverName]= @DriverName" +
            " ,[statusId]= @StatusId" +
            " ,[statusDate]= @StatusDate" +
            " ,[remark] = @Remark" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[locationId]= @LocationId" +
            " ,[purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[spotVehicleQty]= @spotVehicleQty" +
             " ,[prodClassId]= @ProdClassId" +
             " ,[isLinkToExistingSauda]= @IsLinkToExistingSauda" +
             " ,[purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " WHERE idVehicleSpotEntry = @IdVehicleSpotEntry ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.SupplierId);
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.VehicleTypeId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.StatusId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.CreatedBy);
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.StatusDate);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.CreatedOn);
            //cmdUpdate.Parameters.Add("@VehicleQtyMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehicleSpotEntryTO.VehicleQtyMT;
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.Location);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.VehicleNo);
            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.DriverName);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.Remark);
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.LocationId);
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.PurchaseEnquiryId);
            cmdUpdate.Parameters.Add("@spotVehicleQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.SpotVehicleQty);
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.ProdClassId);
            cmdUpdate.Parameters.Add("@IsLinkToExistingSauda", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.IsLinkToExistingSauda);
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehicleSpotEntryTO.PurchaseScheduleSummaryId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehicleSpotEntry, cmdDelete);
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

        public int DeleteTblPurchaseVehicleSpotEntry(Int32 idVehicleSpotEntry, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehicleSpotEntry, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVehicleSpotEntry, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseVehicleSpotEntry] " +
            " WHERE idVehicleSpotEntry = " + idVehicleSpotEntry + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehicleSpotEntry", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleSpotEntryTO.IdVehicleSpotEntry;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion



    }
}
