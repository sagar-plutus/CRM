using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblGradeQtyDtlsDAO: ITblGradeQtyDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblGradeQtyDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGradeQtyDtls]"; 
            return sqlSelectQry;
        }

        public static String SqlSelectQueryForUnloadingQty()
        {
            String sqlSelectQueryForUnloadingQty = "SELECT ISNULL((SUM(purchaseGradingDtls.qtyMT)),0) AS qty,0 AS averageRate" +
                                                   " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                                                   " LEFT JOIN tblPurchaseEnquiry purchaseEnquiry ON purchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId" +
                                                   " LEFT JOIN tblPurchaseWeighingStageSummary purchaseWeighingStageSummary ON ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseWeighingStageSummary.purchaseScheduleSummaryId" +
                                                   " LEFT JOIN tblPurchaseGradingDtls purchaseGradingDtls ON purchaseWeighingStageSummary.idPurchaseWeighingStage = purchaseGradingDtls.purchaseWeighingStageId  AND ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseGradingDtls.purchaseScheduleSummaryId" +
                                                   " WHERE purchaseScheduleSummary.vehiclePhaseId = 4 AND" +
                                                   " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE) AND" +
                                                   " ISNULL(purchaseScheduleSummary.isCorrectionCompleted, 0) = 1";
            return sqlSelectQueryForUnloadingQty;

        }

        //BugId = 8931 Inventory Report - as per format Grade wise Average Rate column is displaying
        public static String SqlSelectQueryForUnloadingQtyDisplayAverageRate()
        {
            String sqlSelectQueryForUnloadingQty = "SELECT ISNULL((SUM(purchaseGradingDtls.qtyMT)),0) AS qty," +
                                                   " CAST(ROUND(ISNULL((SUM(purchaseGradingDtls.productAmount))/(SUM(purchaseGradingDtls.qtyMT)),0),2) AS NUMERIC(36,2)) AS averageRate " +
                                                   " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                                                   " LEFT JOIN tblPurchaseEnquiry purchaseEnquiry ON purchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId" +
                                                   " LEFT JOIN tblPurchaseWeighingStageSummary purchaseWeighingStageSummary ON ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseWeighingStageSummary.purchaseScheduleSummaryId" +
                                                   " LEFT JOIN tblPurchaseGradingDtls purchaseGradingDtls ON purchaseWeighingStageSummary.idPurchaseWeighingStage = purchaseGradingDtls.purchaseWeighingStageId  AND ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseGradingDtls.purchaseScheduleSummaryId" +
                                                   " WHERE purchaseScheduleSummary.vehiclePhaseId = 4 AND" +
                                                   " CAST(purchaseScheduleSummary.corretionCompletedOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE) AND" +
                                                   " ISNULL(purchaseScheduleSummary.isCorrectionCompleted, 0) = 1";
            return sqlSelectQueryForUnloadingQty;

        }
        #endregion

        #region Selection
        public List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls()
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

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeQtyDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public DataTable SelectGradeQtyDetails(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            DataTable dt;
            try
            {
                conn.Open();
                sqlQuery = "SELECT FORMAT(tblGradeQtyDtls.date,'dd/MM/yyyy') AS Date,tblGradeQtyDtls.prodClassId,prodClassification.prodClassDesc,tblGradeQtyDtls.prodItemId," +
                                        "proditem.itemName,tblGradeQtyDtls.supervisorId,userNM.userDisplayName ,ISNULL(tblGradeQtyDtls.qty, 0) AS qty," +
                                        "ISNULL(tblGradeQtyDtls.qty2, 0) AS qty2" +
                                        " FROM tblGradeQtyDtls tblGradeQtyDtls" +
                                        " LEFT JOIN tblProdClassification prodClassification ON tblGradeQtyDtls.prodClassId = prodClassification.idProdClass" +
                                        " LEFT JOIN tblProductItem proditem ON tblGradeQtyDtls.prodItemId = proditem.idProdItem" +
                                        " LEFT JOIN(" +
                                        " SELECT userId, userDisplayName" +
                                        " FROM tblUser" +
                                        " LEFT JOIN tblUserRole ON tblUser.iduser = tblUserRole.userId" +
                                        " LEFT JOIN tblRole ON  tblUserRole.roleId = tblRole.idRole" +
                                        " WHERE roleTypeId = 19 AND tblUserRole.isActive= 1 AND tblUser.isActive= 1" +
                                        " ) AS userNM" +
                                        " ON tblGradeQtyDtls.supervisorId = userNM.userId" +
                                        " WHERE tblGradeQtyDtls.qtyTypeId = 1 AND tblGradeQtyDtls.isActive = 1 AND" +
                                        " CAST(tblGradeQtyDtls.createdOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)";

                if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
                {
                    for (int i = 0; i < tblReportsTO.TblFilterReportTOList1.Count; i++)
                    {
                        TblFilterReportTO filterTO = tblReportsTO.TblFilterReportTOList1[i];

                        if (filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.IsOptional == 1)
                        {
                            sqlQuery += filterTO.WhereClause;
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;

                        }
                        else if(filterTO.OutputValue != null && filterTO.OutputValue != String.Empty && filterTO.SqlDbTypeValue == 33)
                        {
                            cmdSelect.Parameters.Add("@" + filterTO.SqlParameterName, (SqlDbType)filterTO.SqlDbTypeValue).Value = filterTO.OutputValue;
                        }


                    }
                }


                //if (prodClassId > 0)
                //{
                //    cmdSelect.CommandText += " AND ISNULL(tblGradeQtyDtls.prodClassId,0) =" + prodClassId;
                //}
                //if (prodItemId > 0)
                //{
                //    cmdSelect.CommandText += " AND ISNULL(tblGradeQtyDtls.prodItemId,0) =" + prodItemId;
                //}
                //if (supervisorId > 0)
                //{
                //    cmdSelect.CommandText += " AND ISNULL(tblGradeQtyDtls.supervisorId,0) =" + supervisorId;
                //}

                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                dt = new DataTable();
                dt.Load(reader);
                return dt;
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

        public DataTable GetUnloadingQty(DateTime fromDate, DateTime toDate,Int32 prodClassId,Int32 prodItemId,Int32 supervisorId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader;
            Double unloadingQty = 0;
            DataTable dt = new DataTable();
            try
            {

                conn.Open();
                if (prodItemId == 0)
                {
                    cmdSelect.CommandText = SqlSelectQueryForUnloadingQty();
                }
                else if (prodItemId > 0)
                {
                    cmdSelect.CommandText = SqlSelectQueryForUnloadingQtyDisplayAverageRate();
                }

                if (prodClassId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(purchaseEnquiry.prodClassId,0) =" + prodClassId;
                }
                if (prodItemId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(purchaseGradingDtls.prodItemId,0) =" + prodItemId;
                }
                if (supervisorId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(purchaseWeighingStageSummary.supervisorId,0) =" + supervisorId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;
                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    dt.Load(reader);
                    //while (reader.Read())
                    //{
                    //    unloadingQty = Convert.ToDouble(reader["qty"].ToString());
                    //}
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return dt;
        }
        public List<TblGradeQtyDtlsTO> SelectTblGradeQtyDtls(Int32 idGradeQtyDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGradeQtyDtls = " + idGradeQtyDtls +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeQtyDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeQtyDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
                return list;
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

        public List<TblGradeQtyDtlsTO> SelectExistingGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String whereCond = String.Empty;
            try
            {
                whereCond = " WHERE cast(date as date) = @date ";

                cmdSelect.CommandText = SqlSelectQuery() + whereCond;

                cmdSelect.CommandText += " AND ISNULL(prodClassId,0) = " + tblGradeQtyDtlsTO.ProdClassId;

                cmdSelect.CommandText += " AND ISNULL(prodItemId,0) = " + tblGradeQtyDtlsTO.ProdItemId;

                cmdSelect.CommandText += " AND ISNULL(supervisorId,0) = " + tblGradeQtyDtlsTO.SupervisorId;

                cmdSelect.CommandText += " AND ISNULL(qtyTypeId,0) = " + tblGradeQtyDtlsTO.QtyTypeId;

                //if (tblGradeQtyDtlsTO.ProdClassId > 0)
                //{
                //    cmdSelect.CommandText += " AND prodClassId =" + tblGradeQtyDtlsTO.ProdClassId + " AND prodItemId = " + tblGradeQtyDtlsTO.ProdItemId;
                //}

                //if(tblGradeQtyDtlsTO.ProdItemId > 0)
                //{
                //    cmdSelect.CommandText += " AND prodItemId =" + tblGradeQtyDtlsTO.ProdItemId;
                //}

                //if (tblGradeQtyDtlsTO.SupervisorId > 0)
                //{
                //    cmdSelect.CommandText += " AND supervisorId = " + tblGradeQtyDtlsTO.SupervisorId;
                //}

                //if (tblGradeQtyDtlsTO.QtyTypeId > 0)
                //{
                //    cmdSelect.CommandText += " AND qtyTypeId = " + tblGradeQtyDtlsTO.QtyTypeId;
                //}


                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@date", System.Data.SqlDbType.Date).Value = tblGradeQtyDtlsTO.Date;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeQtyDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeQtyDtlsTO> ConvertDTToList(SqlDataReader tblGradeQtyDtlsTODT)
        {
            List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList = new List<TblGradeQtyDtlsTO>();
            if (tblGradeQtyDtlsTODT != null)
            {
                while(tblGradeQtyDtlsTODT.Read())
                {
                    TblGradeQtyDtlsTO tblGradeQtyDtlsTONew = new TblGradeQtyDtlsTO();
                    if (tblGradeQtyDtlsTODT["idGradeQtyDtls"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.IdGradeQtyDtls = Convert.ToInt32(tblGradeQtyDtlsTODT["idGradeQtyDtls"].ToString());
                    if (tblGradeQtyDtlsTODT["prodClassId"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.ProdClassId = Convert.ToInt32(tblGradeQtyDtlsTODT["prodClassId"].ToString());
                    if (tblGradeQtyDtlsTODT["prodItemId"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.ProdItemId = Convert.ToInt32(tblGradeQtyDtlsTODT["prodItemId"].ToString());
                    if (tblGradeQtyDtlsTODT["supervisorId"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.SupervisorId = Convert.ToInt32(tblGradeQtyDtlsTODT["supervisorId"].ToString());
                    if (tblGradeQtyDtlsTODT["qtyTypeId"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.QtyTypeId = Convert.ToInt32(tblGradeQtyDtlsTODT["qtyTypeId"].ToString());
                    if (tblGradeQtyDtlsTODT["isActive"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.IsActive = Convert.ToInt32(tblGradeQtyDtlsTODT["isActive"].ToString());
                    if (tblGradeQtyDtlsTODT["createdBy"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.CreatedBy = Convert.ToInt32(tblGradeQtyDtlsTODT["createdBy"].ToString());
                    if (tblGradeQtyDtlsTODT["updatedBy"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.UpdatedBy = Convert.ToInt32(tblGradeQtyDtlsTODT["updatedBy"].ToString());
                    if (tblGradeQtyDtlsTODT["date"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.Date = Convert.ToDateTime(tblGradeQtyDtlsTODT["date"].ToString());
                    if (tblGradeQtyDtlsTODT["createdOn"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.CreatedOn = Convert.ToDateTime(tblGradeQtyDtlsTODT["createdOn"].ToString());
                    if (tblGradeQtyDtlsTODT["updatedOn"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.UpdatedOn = Convert.ToDateTime(tblGradeQtyDtlsTODT["updatedOn"].ToString());
                    if (tblGradeQtyDtlsTODT["qty"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.Qty = Convert.ToDouble(tblGradeQtyDtlsTODT["qty"].ToString());
                    if (tblGradeQtyDtlsTODT["qty2"] != DBNull.Value)
                        tblGradeQtyDtlsTONew.Qty2 = Convert.ToDouble(tblGradeQtyDtlsTODT["qty2"].ToString());
                    tblGradeQtyDtlsTOList.Add(tblGradeQtyDtlsTONew);
                }
            }
            return tblGradeQtyDtlsTOList;
        }

        //public List<TblGradeQtyDtlsTO> ConvertDTToListForRpt(SqlDataReader tblGradeQtyDtlsTODT)
        //{
        //    List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList = new List<TblGradeQtyDtlsTO>();
        //    if (tblGradeQtyDtlsTODT != null)
        //    {
        //        while (tblGradeQtyDtlsTODT.Read())
        //        {
        //            TblGradeQtyDtlsTO tblGradeQtyDtlsTONew = new TblGradeQtyDtlsTO();
        //            //if (tblGradeQtyDtlsTODT["idGradeQtyDtls"] != DBNull.Value)
        //            //   tblGradeQtyDtlsTONew.IdGradeQtyDtls = Convert.ToInt32(tblGradeQtyDtlsTODT["idGradeQtyDtls"].ToString());
        //            if (tblGradeQtyDtlsTODT["prodClassId"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.ProdClassId = Convert.ToInt32(tblGradeQtyDtlsTODT["prodClassId"].ToString());
        //            if (tblGradeQtyDtlsTODT["prodClassDesc"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.ProdClassDesc = Convert.ToString(tblGradeQtyDtlsTODT["prodClassDesc"].ToString());
        //            if (tblGradeQtyDtlsTODT["itemName"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.ItemName = Convert.ToString(tblGradeQtyDtlsTODT["itemName"].ToString());
        //            if (tblGradeQtyDtlsTODT["prodClassDesc"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.ProdClassDesc = Convert.ToString(tblGradeQtyDtlsTODT["prodClassDesc"].ToString());
        //            if (tblGradeQtyDtlsTODT["supervisorId"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.SupervisorId = Convert.ToInt32(tblGradeQtyDtlsTODT["supervisorId"].ToString());
        //            if (tblGradeQtyDtlsTODT["userDisplayName"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.UserDisplayName = Convert.ToString(tblGradeQtyDtlsTODT["userDisplayName"].ToString());
        //            //if (tblGradeQtyDtlsTODT["qtyTypeId"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.QtyTypeId = Convert.ToInt32(tblGradeQtyDtlsTODT["qtyTypeId"].ToString());
        //            //if (tblGradeQtyDtlsTODT["isActive"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.IsActive = Convert.ToInt32(tblGradeQtyDtlsTODT["isActive"].ToString());
        //            //if (tblGradeQtyDtlsTODT["createdBy"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.CreatedBy = Convert.ToInt32(tblGradeQtyDtlsTODT["createdBy"].ToString());
        //            //if (tblGradeQtyDtlsTODT["updatedBy"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.UpdatedBy = Convert.ToInt32(tblGradeQtyDtlsTODT["updatedBy"].ToString());
        //            if (tblGradeQtyDtlsTODT["date"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.Date = Convert.ToDateTime(tblGradeQtyDtlsTODT["date"].ToString());
        //            //if (tblGradeQtyDtlsTODT["createdOn"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.CreatedOn = Convert.ToDateTime(tblGradeQtyDtlsTODT["createdOn"].ToString());
        //            //if (tblGradeQtyDtlsTODT["updatedOn"] != DBNull.Value)
        //            //  tblGradeQtyDtlsTONew.UpdatedOn = Convert.ToDateTime(tblGradeQtyDtlsTODT["updatedOn"].ToString());
        //            if (tblGradeQtyDtlsTODT["qty"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.Qty = Convert.ToDouble(tblGradeQtyDtlsTODT["qty"].ToString());
        //            if (tblGradeQtyDtlsTODT["qty2"] != DBNull.Value)
        //                tblGradeQtyDtlsTONew.Qty2 = Convert.ToDouble(tblGradeQtyDtlsTODT["qty2"].ToString());
        //            tblGradeQtyDtlsTOList.Add(tblGradeQtyDtlsTONew);
        //        }
        //    }
        //    return tblGradeQtyDtlsTOList;
        //}


        #endregion

        #region Insertion
        public int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGradeQtyDtlsTO, cmdInsert);
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

        public int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGradeQtyDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGradeQtyDtls]( " + 
            //"  [idGradeQtyDtls]" +
            "  [prodClassId]" +
            " ,[prodItemId]" +
            " ,[supervisorId]" +
            " ,[qtyTypeId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[date]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[qty]" +
            " ,[qty2]" +
            " )" +
" VALUES (" +
            //"  @IdGradeQtyDtls " +
            "  @ProdClassId " +
            " ,@ProdItemId " +
            " ,@SupervisorId " +
            " ,@QtyTypeId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@Date " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Qty " +
            " ,@Qty2 " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.ProdClassId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.ProdItemId);
            cmdInsert.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.SupervisorId);
            cmdInsert.Parameters.Add("@QtyTypeId", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.QtyTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = tblGradeQtyDtlsTO.Date;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.Qty);
            cmdInsert.Parameters.Add("@Qty2", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.Qty2);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGradeQtyDtlsTO, cmdUpdate);
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

        public int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGradeQtyDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGradeQtyDtls] SET " + 
            //"  [idGradeQtyDtls] = @IdGradeQtyDtls" +
            "  [prodClassId]= @ProdClassId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[supervisorId]= @SupervisorId" +
            " ,[qtyTypeId]= @QtyTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[date]= @Date" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[qty] = @Qty" +
            " ,[qty2] = @Qty2" +
            " WHERE idGradeQtyDtls = @IdGradeQtyDtls "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.ProdClassId);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.ProdItemId);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.SupervisorId);
            cmdUpdate.Parameters.Add("@QtyTypeId", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.QtyTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@Date", System.Data.SqlDbType.DateTime).Value = tblGradeQtyDtlsTO.Date;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeQtyDtlsTO.Qty);
            cmdUpdate.Parameters.Add("@Qty2", System.Data.SqlDbType.Decimal).Value = tblGradeQtyDtlsTO.Qty2;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGradeQtyDtls, cmdDelete);
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

        public int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGradeQtyDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idGradeQtyDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGradeQtyDtls] " +
            " WHERE idGradeQtyDtls = " + idGradeQtyDtls +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGradeQtyDtls", System.Data.SqlDbType.Int).Value = tblGradeQtyDtlsTO.IdGradeQtyDtls;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
