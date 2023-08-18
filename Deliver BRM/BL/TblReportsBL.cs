using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblReportsBL
    {
        #region Selection
        //public static List<TblReportsTO> SelectAllTblReports()
        //{
        //    return TblReportsDAO.SelectAllTblReports();
        //}

        public static List<TblReportsTO> SelectAllTblReportsList()
        {
            List<TblReportsTO> tblReportsTODT = TblReportsDAO.SelectAllTblReports();
            Parallel.ForEach(tblReportsTODT, element =>
             {
                 element.SqlQuery = null;
             }
            );
            return tblReportsTODT;
        }

        public static DataTable GetMonthWiseBookingReport(TblReportsTO tblReportsTO)
        {
            DataTable monthWiseBookingDT = new DataTable();
            List<TblOrganizationTO> tblOrganizationTOList = TblOrganizationBL.SelectAllTblOrganizationWithDefaultAddress(Constants.OrgTypeE.DEALER);
            monthWiseBookingDT.Columns.Add("Sr No", typeof(Int32));
            monthWiseBookingDT.Columns.Add("Cnf Name", typeof(String));
            monthWiseBookingDT.Columns.Add("Dealer Name", typeof(String));
            monthWiseBookingDT.Columns.Add("Category", typeof(String));

            monthWiseBookingDT.Columns.Add("Location", typeof(String));
            monthWiseBookingDT.Columns.Add("Taluka", typeof(String));
            monthWiseBookingDT.Columns.Add("District", typeof(String));
            monthWiseBookingDT.Columns.Add("State", typeof(String));
            monthWiseBookingDT.Columns.Add("isTotalRow", typeof(Int32));
            
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            var fromDateTo = tblReportsTO.TblFilterReportTOList1.Where(w => w.FilterName == "From Date").FirstOrDefault();
            if (fromDateTo != null && !String.IsNullOrEmpty(fromDateTo.OutputValue))
            {
                fromDate = Convert.ToDateTime(fromDateTo.OutputValue);
            }
            var toDateTo = tblReportsTO.TblFilterReportTOList1.Where(w => w.FilterName == "To Date").FirstOrDefault();
            if (toDateTo != null && !String.IsNullOrEmpty(toDateTo.OutputValue))
            {
                toDate = Convert.ToDateTime(toDateTo.OutputValue);
            }

            if (fromDate != new DateTime() && toDate != new DateTime())
            {
                DateTime indexDate = new DateTime(fromDate.Year, fromDate.Month, 01);
                DateTime indexToDate = new DateTime(toDate.Year, toDate.Month, 01);

                if (indexDate.Year != indexToDate.Year || indexDate.Month != indexToDate.Month)
                {
                    while (indexDate < indexToDate)
                    {
                        String columnName = indexDate.ToString("MMM") + "-" + indexDate.Year;
                        monthWiseBookingDT.Columns.Add(columnName, typeof(Double));
                        indexDate = indexDate.AddMonths(1);
                    }
                }
                String columnName1 = toDate.ToString("MMM") + "-" + toDate.Year;
                monthWiseBookingDT.Columns.Add(columnName1, typeof(Double));
            }

            monthWiseBookingDT.Columns.Add("Total", typeof(Double));

            Dictionary<String, Double> totalBookingDct = new Dictionary<string, double>();

            Double allTotal = 0;

            List<TblBookingsTO> tblBookingsTOList = TblBookingsBL.SelectBookingList(0, 0, "", fromDate, toDate, null, 0,0,0);

            if (tblBookingsTOList != null && tblBookingsTOList.Count > 0)
            {
                List<TblBookingsTO> dealerCnfList = tblBookingsTOList.GroupBy(g => new { g.DealerOrgId, g.DealerName, g.CnfName, g.CnFOrgId }).Select(s => s.FirstOrDefault()).ToList();
                dealerCnfList = dealerCnfList.OrderBy(o => o.CnfName).ThenBy(ot => ot.DealerName).ToList();
                for (int i = 0; i < dealerCnfList.Count; i++)
                {
                    TblBookingsTO dealerTO = dealerCnfList[i];

                    monthWiseBookingDT.Rows.Add();
                    Int32 rowIndex = monthWiseBookingDT.Rows.Count - 1;

                    monthWiseBookingDT.Rows[rowIndex]["Sr No"] = (rowIndex + 1);
                    monthWiseBookingDT.Rows[rowIndex]["Cnf Name"] = dealerTO.CnfName;
                    monthWiseBookingDT.Rows[rowIndex]["Dealer Name"] = dealerTO.DealerName;
                    monthWiseBookingDT.Rows[rowIndex]["isTotalRow"] = 0;
                    
                    TblOrganizationTO dealerOrgTO = tblOrganizationTOList.Where(w => w.IdOrganization == dealerTO.DealerOrgId).FirstOrDefault();
                    if (dealerOrgTO != null)
                    {
                        monthWiseBookingDT.Rows[rowIndex]["Category"] = dealerOrgTO.ConsumerType;
                        if (dealerOrgTO.AddressList != null && dealerOrgTO.AddressList.Count > 0)
                        {
                            TblAddressTO tblAddressTO = dealerOrgTO.AddressList[0];
                            String location = String.Empty;
                            if (!String.IsNullOrEmpty(tblAddressTO.VillageName))
                            {
                                location += tblAddressTO.VillageName;
                            }
                            else if (!String.IsNullOrEmpty(tblAddressTO.AreaName))
                            {
                                location += tblAddressTO.AreaName;
                            }
                            else if (!String.IsNullOrEmpty(tblAddressTO.PlotNo))
                            {
                                location += tblAddressTO.PlotNo;
                            }

                            monthWiseBookingDT.Rows[rowIndex]["Location"] = location;


                            monthWiseBookingDT.Rows[rowIndex]["Taluka"] = tblAddressTO.TalukaName;
                            monthWiseBookingDT.Rows[rowIndex]["District"] = tblAddressTO.DistrictName;
                            monthWiseBookingDT.Rows[rowIndex]["State"] = tblAddressTO.StateName;
                        }
                    }
                    List<TblBookingsTO> dealerBookingList = tblBookingsTOList.Where(w => w.DealerOrgId == dealerTO.DealerOrgId && w.CnFOrgId == dealerTO.CnFOrgId).ToList();

                    Dictionary<String, Double> monthwiseBooking = new Dictionary<string, double>();
                    Double totalDealerBookingQty = 0; 
                    for (int k = 0; k < dealerBookingList.Count; k++)
                    {
                        TblBookingsTO dealerBookingTo = dealerBookingList[k];
                        String monthKey = dealerBookingTo.CreatedOn.ToString("MMM") + "-" + dealerBookingTo.CreatedOn.Year;

                        Double bookingQty = Math.Round(dealerBookingTo.BookingQty, 3);

                        totalDealerBookingQty += bookingQty;

                        if (monthwiseBooking.ContainsKey(monthKey))
                        {
                            monthwiseBooking[monthKey] += bookingQty;
                        }
                        else
                        {
                            monthwiseBooking.Add(monthKey, bookingQty);
                        }

                        if (totalBookingDct.ContainsKey(monthKey))
                        {
                            totalBookingDct[monthKey] += bookingQty;
                        }
                        else
                        {
                            totalBookingDct.Add(monthKey, bookingQty);
                        }
                        

                    }

                    if (monthwiseBooking.Count > 0)
                    {
                        foreach (KeyValuePair<string, Double> keyValueObj in monthwiseBooking)
                        {
                            monthWiseBookingDT.Rows[i][keyValueObj.Key] = Math.Round(keyValueObj.Value, 3);
                        }
                    }

                    monthWiseBookingDT.Rows[i]["Total"] = Math.Round(totalDealerBookingQty, 3);

                    allTotal += totalDealerBookingQty;

                }

                if (totalBookingDct.Count > 0)
                {
                    monthWiseBookingDT.Rows.Add();
                    Int32 ind = monthWiseBookingDT.Rows.Count - 1;
                    monthWiseBookingDT.Rows[ind]["Cnf Name"] = "Total";
                    monthWiseBookingDT.Rows[ind]["isTotalRow"] = 1;
                    foreach (KeyValuePair<string, Double> keyValueObj in totalBookingDct)
                    {
                        monthWiseBookingDT.Rows[ind][keyValueObj.Key] = Math.Round(keyValueObj.Value, 3);
                    }
                    monthWiseBookingDT.Rows[ind]["Total"] = Math.Round(allTotal, 3);
                }

            }
            return monthWiseBookingDT;
        }


        public static TblReportsTO SelectTblReportsTO(Int32 idReports)
        {
            TblReportsTO tblReportsTODT = TblReportsDAO.SelectTblReports(idReports);
            if (tblReportsTODT != null)
            {
                List<TblFilterReportTO> tblFilterReportList = TblFilterReportBL.SelectTblFilterReportList(idReports);
                if (tblFilterReportList != null && tblFilterReportList.Count > 0)
                {
                    tblReportsTODT.TblFilterReportTOList1 = tblFilterReportList;

                }
                return tblReportsTODT;
            }

            else
                return null;
        }

        #endregion

        #region Insertion
        public static int InsertTblReports(TblReportsTO tblReportsTO)
        {
            return TblReportsDAO.InsertTblReports(tblReportsTO);
        }

        public static int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsDAO.InsertTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblReports(TblReportsTO tblReportsTO)
        {
            return TblReportsDAO.UpdateTblReports(tblReportsTO);
        }

        public static int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsDAO.UpdateTblReports(tblReportsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblReports(Int32 idReports)
        {
            return TblReportsDAO.DeleteTblReports(idReports);
        }

        public static int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran)
        {
            return TblReportsDAO.DeleteTblReports(idReports, conn, tran);
        }

        #endregion

        //public static List<DynamicReportTO> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        //{
        //    try
        //    {
        //        List<DynamicReportTO> data = TblReportsDAO.GetDynamicSqlData(Startup.ConnectionString, "SELECT * FROM dimOrgType");
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        public static IEnumerable<dynamic> GetDynamicData(string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                IEnumerable<dynamic> dynamicList = DynamicReportModel.GetDynamicSqlData(Startup.ConnectionString, cmdText, commandParameters);
                if (dynamicList != null)
                {
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static IEnumerable<dynamic> CreateDynamicQuery(TblReportsTO tblReportsTO)
        {
            try
            {
                if (tblReportsTO != null )
                {
                    TblReportsTO temptblReportsTO = BL.TblReportsBL.SelectTblReportsTO(tblReportsTO.IdReports);
                    List<TblFilterReportTO> filterReportlist = new List<TblFilterReportTO>();
                    if (tblReportsTO.TblFilterReportTOList1 != null)
                    {
                        filterReportlist = tblReportsTO.TblFilterReportTOList1.OrderBy(element => element.OrderArguments).ToList();
                    }
                    String sqlQuery = temptblReportsTO.SqlQuery;
                    int count = filterReportlist.Count;
                    SqlParameter[] commandParameters = new SqlParameter[count];
                    for (int i = 0; i < filterReportlist.Count; i++)
                    {
                        TblFilterReportTO tblFilterReportTO = filterReportlist[i];
                        if(tblFilterReportTO.OutputValue != null && tblFilterReportTO.OutputValue != string.Empty && tblFilterReportTO.IsOptional==1)
                        {
                            sqlQuery += tblFilterReportTO.WhereClause;
                        }
                        if(tblFilterReportTO.IsRequired==0 && temptblReportsTO.WhereClause != String.Empty)
                        {
                            object listofUsers = TblOrgStructureBL.ChildUserListOnUserId(tblReportsTO.CreatedBy, 1, (int)Constants.ReportingTypeE.ADMINISTRATIVE);  //this method is call for get Child User Id's From Organzization Structure.
                            List<int> userIdList = new List<int>();
                            if (listofUsers != null)
                            {
                                userIdList=(List<int>)listofUsers;
                                userIdList.Add(tblReportsTO.CreatedBy);
                            }
                            else
                            {
                                userIdList.Add(tblReportsTO.CreatedBy);
                            }
                            string createdArr = string.Join<int>(",", userIdList);
                            temptblReportsTO.WhereClause = temptblReportsTO.WhereClause.Replace(tblFilterReportTO.SqlParameterName, createdArr);
                            sqlQuery += temptblReportsTO.WhereClause;
                        }
                        SqlDbType sqlDbType = (SqlDbType)tblFilterReportTO.SqlDbTypeValue;
                        commandParameters[i] = new SqlParameter("@" + tblFilterReportTO.SqlParameterName, sqlDbType);
                        commandParameters[i].Value = tblFilterReportTO.OutputValue;
                    }
                    IEnumerable<dynamic> dynamicList = GetDynamicData(sqlQuery, commandParameters);
                    return dynamicList;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
    }
}
