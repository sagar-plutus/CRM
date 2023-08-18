using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblRateBandDeclarationPurchaseDAO : ITblRateBandDeclarationPurchaseDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        public TblRateBandDeclarationPurchaseDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
        }

        #region Methods

        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * " +
                                  " FROM [tblRateBandDeclarationPurchase] rateBand" +
                                  " LEFT JOIN [tblGlobalRatePurchase] ratePurchase ON ratePurchase.idGlobalRatePurchase=rateBand.globalRatePurchaseId";

            return sqlSelectQry;
        }

        public  String SqlSelectQueryForPurchase()
        {
            String sqlSelectQry = " SELECT * " +
                                  " FROM tblUser userDtl " +
                                  " LEFT JOIN tblUserRole userRole " +
                                  " ON userDtl.idUser = userRole.userId AND userDtl.isActive=1 AND userRole.isActive = 1 " +
                                  " LEFT JOIN tblRole Role ON Role.idRole=userRole.roleId" +
                                  " WHERE Role.roleTypeId = " + Convert.ToInt32(Constants.SystemRoleTypeE.PURCHASE_MANAGER);

                                  //" WHERE Role.roleDesc='Purchase Manager'";
                                  //" WHERE userRole.roleId IN ( select configParamVal from tblConfigParams where configParamName = '" + Constants.CP_PURCHASE_MANAGER_ROLE_ID + "' )";

            return sqlSelectQry;
        }
        
        #endregion

        #region Selection

        /// <summary>
        /// swati Pisal
        /// </summary>       
        /// <returns></returns>
        public  List<TblRateBandDeclarationPurchaseTO> SelectAllTblUserOfPurchase()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblPuchaseManagerTODT = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForPurchase();// + " WHERE globalRateId=" + globalRateId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblPuchaseManagerTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToPurchaseList(tblPuchaseManagerTODT);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblPuchaseManagerTODT != null) tblPuchaseManagerTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblRateBandDeclarationPurchaseTO> SelectAllTblRateBandDeclarationPurchase(Int32 globalRatePurchaseId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGlobalRatePurchaseTODT = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE globalRatePurchaseId=" + globalRatePurchaseId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(tblGlobalRatePurchaseTODT);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblGlobalRatePurchaseTODT != null) tblGlobalRatePurchaseTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// added by swati pisal to get purchase list from tblUser table
        /// </summary>
        /// <param name="TblRateBandDeclarationPurchaseTODT"></param>
        /// <returns></returns>
        public  List<TblRateBandDeclarationPurchaseTO> ConvertDTToPurchaseList(SqlDataReader TblRateBandDeclarationPurchaseTODT)
        {
            List<TblRateBandDeclarationPurchaseTO> TblRateBandDeclarationPurchaseTOList = new List<TblRateBandDeclarationPurchaseTO>();
            if (TblRateBandDeclarationPurchaseTODT != null)
            {
                while (TblRateBandDeclarationPurchaseTODT.Read())
                {
                    TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTONew = new TblRateBandDeclarationPurchaseTO();
                    if (TblRateBandDeclarationPurchaseTODT["UserId"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.UserId = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["UserId"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["UserDisplayName"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.PurchaseManager = TblRateBandDeclarationPurchaseTODT["UserDisplayName"].ToString();
                    TblRateBandDeclarationPurchaseTOList.Add(TblRateBandDeclarationPurchaseTONew);
                }
            }
            return TblRateBandDeclarationPurchaseTOList;
        }

        public  List<TblRateBandDeclarationPurchaseTO> ConvertDTToList(SqlDataReader TblRateBandDeclarationPurchaseTODT)
        {
            List<TblRateBandDeclarationPurchaseTO> TblRateBandDeclarationPurchaseTOList = new List<TblRateBandDeclarationPurchaseTO>();
            if (TblRateBandDeclarationPurchaseTODT != null)
            {
                while (TblRateBandDeclarationPurchaseTODT.Read())
                {
                    TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTONew = new TblRateBandDeclarationPurchaseTO();
                    if (TblRateBandDeclarationPurchaseTODT["IdRateBandDeclarationPurchase"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.IdRateBandDeclarationPurchase = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["IdRateBandDeclarationPurchase"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["UserId"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.UserId = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["UserId"].ToString());

                    if (TblRateBandDeclarationPurchaseTODT["GlobalRatePurchaseId"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.GlobalRatePurchaseId = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["GlobalRatePurchaseId"].ToString());

                    if (TblRateBandDeclarationPurchaseTODT["Rate"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.DeclaredRate = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["Rate"].ToString());

                    if (TblRateBandDeclarationPurchaseTODT["CreatedBy"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.CreatedBy = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["CreatedBy"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["CreatedOn"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.CreatedOn = Convert.ToDateTime(TblRateBandDeclarationPurchaseTODT["CreatedOn"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["Rate_Band_Costing"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.RateBandCosting = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["Rate_Band_Costing"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["Rate_Band_Correction"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.RateBandCorrection = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["Rate_Band_Correction"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["ValidUpto"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.ValidUpto = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["ValidUpto"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["CalculatedRate_Costing"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.CalculatedRateCosting = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["CalculatedRate_Costing"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["rate"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.DeclaredRate = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["rate"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["CalculatedRate_Correction"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.CalculatedRateCorrection = Convert.ToDouble(TblRateBandDeclarationPurchaseTODT["CalculatedRate_Correction"].ToString());

                    if (TblRateBandDeclarationPurchaseTODT["IsActive"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.IsActive = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["IsActive"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["UpdatedBy"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.UpdatedBy = Convert.ToInt32(TblRateBandDeclarationPurchaseTODT["UpdatedBy"].ToString());
                    if (TblRateBandDeclarationPurchaseTODT["UpdatedOn"] != DBNull.Value)
                        TblRateBandDeclarationPurchaseTONew.UpdatedOn = Convert.ToDateTime(TblRateBandDeclarationPurchaseTODT["UpdatedOn"].ToString());
                    TblRateBandDeclarationPurchaseTOList.Add(TblRateBandDeclarationPurchaseTONew);
                }
            }
            return TblRateBandDeclarationPurchaseTOList;
        }

        //public  TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    SqlDataReader sqlReader = null;
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRateBandDeclarationPurchase = " + idRateBandDeclaration + " ";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(sqlReader);
        //        if (list != null && list.Count == 1)
        //            return list[0];
        //        else return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        sqlReader.Dispose();
        //        cmdSelect.Dispose();
        //    }
        //}

        public  PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo SelectDashboardQuotaAndRateInfo(Int32 roleId, Int32 orgId, DateTime sysDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            try
            {
                if (roleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                {
                    whereCond = " AND orgId=" + orgId;
                }
                conn.Open();
                cmdSelect.CommandText = " SELECT MAX(declaredRate.rate) latestRate,SUM(alloc_qty) AS totalQuota ,AVG(alloc_qty) AS avgRateBand" +
                                        " FROM tblRateBandDeclarationPurchase quota " +
                                        " INNER JOIN tblGlobalRate declaredRate " +
                                        " ON quota.globalRateId = declaredRate.idGlobalRate " +
                                        " WHERE declaredRate.idGlobalRate = (SELECT MAX(idGlobalRate) idGlobalRate " +
                                        " FROM tblGlobalRate WHERE DAY(createdOn) = " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + ")" + whereCond;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo quotaAndRateInfoNew = new PurchaseTrackerAPI.DashboardModels.QuotaAndRateInfo();
                    if (tblLoadingTODT["totalQuota"] != DBNull.Value)
                        quotaAndRateInfoNew.TotalQuota = Convert.ToDouble(tblLoadingTODT["totalQuota"].ToString());
                    if (tblLoadingTODT["latestRate"] != DBNull.Value)
                        quotaAndRateInfoNew.DeclaredRate = Convert.ToDouble(tblLoadingTODT["latestRate"].ToString());
                    if (tblLoadingTODT["avgRateBand"] != DBNull.Value)
                        quotaAndRateInfoNew.AvgRateBand = Convert.ToDouble(tblLoadingTODT["avgRateBand"].ToString());

                    return quotaAndRateInfoNew;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRateBandDeclarationPurchase = " + idRateBandDeclaration + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public  TblRateBandDeclarationPurchaseTO SelectTblRateBandDeclaration(Int32 idRateBandDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRateBandDeclarationPurchase = " + idRateBandDeclaration + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        //public  List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 userId, DateTime date)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = "SELECT top 2 * FROM tblRateBandDeclarationPurchase  as RateBand" +
        //                                " LEFT JOIN tblGlobalRatePurchase as GlobalRate" +
        //                                " ON GlobalRate.idGlobalRatePurchase = RateBand.globalRatePurchaseId" +
        //                                " WHERE DAY(RateBand.createdOn)= " + date.Day + " AND MONTH(RateBand.createdOn)= " + date.Month + "" +
        //                                " AND YEAR(RateBand.createdOn)= " + date.Year + " AND userId = " + userId + "" +
        //                                //" AND isActive = 1 " +
        //                                " ORDER BY RateBand.createdOn DESC";

        //            //" SELECT quota.*,latestRateInfo.rate as declaredRate,latestRateInfo.brandName,latestRateInfo.idBrand as brandId  FROM tblQuotaDeclaration quota " +
        //            //                     " LEFT JOIN (SELECT rateDtl.*,brand.brandName,brand.idBrand FROM tblGlobalRate rateDtl " +
        //            //                     " INNER JOIN(SELECT brandId, MAX(idGlobalRate) idGlobalRate  FROM tblGlobalRate GROUP BY brandId) AS latestRate " +
        //            //                     " ON rateDtl.brandId = latestRate.brandId AND rateDtl.idGlobalRate = latestRate.idGlobalRate 	LEFT JOIN dimBrand brand on brand.idBrand=latestRate.brandId) AS latestRateInfo ON latestRateInfo.idGlobalRate = quota.globalRateId " +
        //            //                     " WHERE DAY(quota.createdOn)=" + date.Day + " AND MONTH(quota.createdOn)=" + date.Month + " AND YEAR(quota.createdOn)= " + date.Year +
        //            //                     " AND orgId=" + orgId + " AND isActive=1 ORDER BY quotaAllocDate DESC";

        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(tblGlobalRateTODT);
        //        if (tblGlobalRateTODT != null)
        //            tblGlobalRateTODT.Dispose();

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        public List<TblRateBandDeclarationPurchaseTO> SelectLatestRateBandDeclarationPurchaseTOList(Int32 userId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT top 2 * FROM tblRateBandDeclarationPurchase  as RateBand" +
                                        " LEFT JOIN tblGlobalRatePurchase as GlobalRate" +
                                        " ON GlobalRate.idGlobalRatePurchase = RateBand.globalRatePurchaseId" +
                                        " WHERE DAY(RateBand.createdOn)= " + date.Day + " AND MONTH(RateBand.createdOn)= " + date.Month + "" +
                                        " AND YEAR(RateBand.createdOn)= " + date.Year + " AND userId = " + userId + "" +
                                        //" AND isActive = 1 " +
                                        " ORDER BY RateBand.createdOn DESC";

                if (date == DateTime.MinValue)
                {
                    cmdSelect.CommandText = " SELECT top 2 * FROM tblRateBandDeclarationPurchase  as RateBand " +
                                            " LEFT JOIN tblGlobalRatePurchase as GlobalRate " +
                                            " ON GlobalRate.idGlobalRatePurchase = RateBand.globalRatePurchaseId " +
                                            " WHERE RateBand.globalRatePurchaseId = (select max(idGlobalRatePurchase) from tblGlobalRatePurchase) AND userId = " + userId +
                                            " ORDER BY RateBand.createdOn DESC";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(tblGlobalRateTODT);
                if (tblGlobalRateTODT != null)
                    tblGlobalRateTODT.Dispose();

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

        public  TblRateBandDeclarationPurchaseTO SelectOldRateTOList(Int32 userId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select top(1)* from  [dbo].[tblGlobalRatePurchase] a inner join[dbo].[tblRateBandDeclarationPurchase] b " +
                    "on a.idGlobalRatePurchase=b.globalRatePurchaseId where cast(b.[createdOn] as date)=cast(GETDATE() as date) and[idGlobalRatePurchase]=" +
                    "(select max([idGlobalRatePurchase]-1) from[dbo].[tblGlobalRatePurchase]) And b.validUpto >=DATEDIFF(MINUTE, cast(b.[createdOn] as time)," +
                    "cast(GETDATE() as time)) AND b.userId=" + userId + "" +
                    " order by b.[createdOn] desc";
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                
                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblGlobalRateTODT != null)
                {
                    while (tblGlobalRateTODT.Read())
                    {
                        TblRateBandDeclarationPurchaseTO tblBookingsTONew = new TblRateBandDeclarationPurchaseTO();
                        if (tblGlobalRateTODT["rate"] != DBNull.Value)
                            tblBookingsTONew.DeclaredRate = Convert.ToDouble(tblGlobalRateTODT["rate"].ToString());
                        if (tblGlobalRateTODT["rate_band_costing"] != DBNull.Value)
                            tblBookingsTONew.RateBandCosting = Convert.ToDouble(tblGlobalRateTODT["rate_band_costing"].ToString());
                        //
                        if (tblGlobalRateTODT["idRateBandDeclarationPurchase"] != DBNull.Value)
                            tblBookingsTONew.IdRateBandDeclarationPurchase = Convert.ToInt32(tblGlobalRateTODT["idRateBandDeclarationPurchase"].ToString());
                        if (tblGlobalRateTODT["idGlobalRatePurchase"] != DBNull.Value)
                            tblBookingsTONew.GlobalRatePurchaseId = Convert.ToInt32(tblGlobalRateTODT["idGlobalRatePurchase"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblRateBandDeclarationPurchaseTO SelectLatestRateTOList(Int32 userId, DateTime date)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select top(1)DATEDIFF(MINUTE, cast(b.[createdOn] as time),cast(GETDATE() as time)) as timedef,* from  [dbo].[tblGlobalRatePurchase] a inner join[dbo].[tblRateBandDeclarationPurchase] b " +
                    "on a.idGlobalRatePurchase=b.globalRatePurchaseId where cast(b.[createdOn] as date)=cast(GETDATE() as date) and[idGlobalRatePurchase]=" +
                    "(select max([idGlobalRatePurchase]) from[dbo].[tblGlobalRatePurchase]) And b.validUpto >=DATEDIFF(MINUTE, cast(b.[createdOn] as time)," +
                    "cast(GETDATE() as time)) AND b.userId=" + userId + "" +
                    " order by b.[createdOn] desc";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (tblGlobalRateTODT != null)
                {
                    while (tblGlobalRateTODT.Read())
                    {
                        TblRateBandDeclarationPurchaseTO tblBookingsTONew = new TblRateBandDeclarationPurchaseTO();
                        if (tblGlobalRateTODT["rate"] != DBNull.Value)
                            tblBookingsTONew.DeclaredRate = Convert.ToDouble(tblGlobalRateTODT["rate"].ToString());
                        if (tblGlobalRateTODT["rate_band_costing"] != DBNull.Value)
                            tblBookingsTONew.RateBandCosting = Convert.ToDouble(tblGlobalRateTODT["rate_band_costing"].ToString());
                        if (tblGlobalRateTODT["timedef"] != DBNull.Value)
                            tblBookingsTONew.NewValidUpto = Convert.ToInt32(tblGlobalRateTODT["timedef"].ToString());
                        if (tblGlobalRateTODT["validUpto"] != DBNull.Value)
                            tblBookingsTONew.ValidUpto = Convert.ToInt32(tblGlobalRateTODT["validUpto"].ToString());

                        if (tblGlobalRateTODT["idRateBandDeclarationPurchase"] != DBNull.Value)
                            tblBookingsTONew.IdRateBandDeclarationPurchase = Convert.ToInt32(tblGlobalRateTODT["idRateBandDeclarationPurchase"].ToString());
                        if (tblGlobalRateTODT["idGlobalRatePurchase"] != DBNull.Value)
                            tblBookingsTONew.GlobalRatePurchaseId = Convert.ToInt32(tblGlobalRateTODT["idGlobalRatePurchase"].ToString());
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblRateBandDeclarationPurchaseTO SelectPreviousTblRateDeclarationTO(Int32 idRateBandDeclarationPurchase, Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRateBandDeclarationPurchase IN(SELECT TOP 1 idRateBandDeclarationPurchase  FROM tblRateBandDeclarationPurchase rateBand " +
                                        " WHERE userId = " + userId + " AND idRateBandDeclarationPurchase NOT IN(" + idRateBandDeclarationPurchase + ") ORDER BY idRateBandDeclarationPurchase DESC)";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRateBandDeclarationPurchaseTO> list = ConvertDTToList(tblGlobalRateTODT);
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

        public  int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblRateBandDeclarationPurchase] SET " +
                                  "  [isActive] = @isActive " +
                                  " ,[updatedBy] = @updatedBy " +
                                  " ,[validUpto] = @validUpto " +
                                  " ,[updatedOn] = @updatedOn ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value =  _iCommonDAO.ServerDateTime;

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
        #endregion

        #region Insertion

        public  int InsertTblRateBandDeclarationPurchase(TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(TblRateBandDeclarationPurchaseTO, cmdInsert);
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

        /// <summary>
        /// modified by swati
        /// </summary>
        /// <param name="TblRateBandDeclarationPurchaseTO"></param>
        /// <param name="cmdInsert"></param>
        /// <returns></returns>
        public  int ExecuteInsertionCommand(TblRateBandDeclarationPurchaseTO TblRateBandDeclarationPurchaseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRateBandDeclarationPurchase]( " +
                            " [userId]" +
                            " ,[GlobalRatePurchaseId]" +
                            " ,[rate_band_costing]" +
                            " ,[rate_band_correction]" +
                            " ,[ValidUpto]" +
                            " ,[CreatedBy]" +
                            " ,[CreatedOn]" +
                            " ,[calculatedRate_costing]" +
                            " ,[calculatedRate_correction]" +
                            " ,[isActive]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " )" +
                " VALUES (" +
                            " @UserId" +
                            " ,@GlobalRatePurchaseId " +
                            " ,@RateBandCosting " +
                            " ,@RateBandCorrection " +
                            " ,@ValidUpto " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@CalculatedRateCosting " +
                            " ,@CalculatedRateCorrection " +
                            " ,@isActive " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = TblRateBandDeclarationPurchaseTO.UserId;
            cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = TblRateBandDeclarationPurchaseTO.GlobalRatePurchaseId;
            cmdInsert.Parameters.Add("@RateBandCosting", System.Data.SqlDbType.NVarChar).Value = TblRateBandDeclarationPurchaseTO.RateBandCosting;
            cmdInsert.Parameters.Add("@RateBandCorrection", System.Data.SqlDbType.NVarChar).Value = TblRateBandDeclarationPurchaseTO.RateBandCorrection;
            cmdInsert.Parameters.Add("@ValidUpto", System.Data.SqlDbType.Int).Value = TblRateBandDeclarationPurchaseTO.ValidUpto;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = TblRateBandDeclarationPurchaseTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = TblRateBandDeclarationPurchaseTO.CreatedOn;
            cmdInsert.Parameters.Add("@CalculatedRateCosting", System.Data.SqlDbType.NVarChar).Value = TblRateBandDeclarationPurchaseTO.CalculatedRateCosting;
            cmdInsert.Parameters.Add("@CalculatedRateCorrection", System.Data.SqlDbType.NVarChar).Value = TblRateBandDeclarationPurchaseTO.CalculatedRateCorrection;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = TblRateBandDeclarationPurchaseTO.IsActive;
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblRateBandDeclarationPurchaseTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(TblRateBandDeclarationPurchaseTO.UpdatedOn);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                TblRateBandDeclarationPurchaseTO.IdRateBandDeclarationPurchase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        
        #endregion

        #region Updation

        // public  int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        // {
        //     SqlCommand cmdUpdate = new SqlCommand();
        //     try
        //     {
        //         cmdUpdate.Connection = conn;
        //         cmdUpdate.Transaction = tran;

        //         String sqlQuery = @" UPDATE [tblRateBandDeclarationPurchase] SET " +
        //                           "  [isActive] = @isActive " +
        //                           " ,[updatedBy] = @updatedBy " +
        //                           //" ,[validUpto] = @validUpto " +
        //                           " ,[updatedOn] = @updatedOn " +
        //                           "Where[isActive]='1'";
        //         cmdUpdate.CommandText = sqlQuery;
        //         cmdUpdate.CommandType = System.Data.CommandType.Text;

        //         cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
        //         //cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = 0;
        //         cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
        //         cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value =  _iCommonDAO.ServerDateTime;

        //         return cmdUpdate.ExecuteNonQuery();
        //     }
        //     catch (Exception ex)
        //     {
        //         return -1;
        //     }
        //     finally
        //     {
        //         cmdUpdate.Dispose();
        //     }
        // }

        public  int UpdateTblRateDeclaration(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRateBandDeclarationPurchaseTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblRateBandDeclarationPurchaseTO tblRateBandDeclarationPurchaseTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRateBandDeclarationPurchase] SET " +
                            "  [userId]= @UserId" +
                            " ,[globalRatePurchaseId]= @GlobalRatePurchaseId" +
                            " ,[rate_band_costing]= @RateBandCosting" +
                            " ,[rate_band_correction]= @RateBandCorrection" +
                            " ,[calculatedRate_costing]= @CalculatedRateCosting" +
                            " ,[calculatedRate_correction]= @CalculatedRateCorrection" +                            
                            ", [validUpto] = @validUpto" +
                            ", [isActive] = @isActive" +
                            ", [updatedBy] = @updatedBy" +
                            ", [updatedOn] = @updatedOn" +
                            " WHERE[idRateBandDeclarationPurchase] = @IdRateBandDeclarationPurchase";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRateBandDeclarationPurchase", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.IdRateBandDeclarationPurchase;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.UserId;
            cmdUpdate.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.GlobalRatePurchaseId;
            cmdUpdate.Parameters.Add("@RateBandCosting", System.Data.SqlDbType.NVarChar).Value = tblRateBandDeclarationPurchaseTO.RateBandCosting;
            cmdUpdate.Parameters.Add("@RateBandCorrection", System.Data.SqlDbType.NVarChar).Value = tblRateBandDeclarationPurchaseTO.RateBandCorrection;
            cmdUpdate.Parameters.Add("@CalculatedRateCosting", System.Data.SqlDbType.NVarChar).Value = tblRateBandDeclarationPurchaseTO.CalculatedRateCosting;
            cmdUpdate.Parameters.Add("@CalculatedRateCorrection", System.Data.SqlDbType.NVarChar).Value = tblRateBandDeclarationPurchaseTO.CalculatedRateCorrection;
            
            cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.ValidUpto;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.IsActive;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblRateBandDeclarationPurchaseTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblRateBandDeclarationPurchaseTO.UpdatedOn;

            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

    }
}
