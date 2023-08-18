using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblVisitDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVisitDetailsTO> SelectAllTblVisitDetails()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitDetailsTO> visitDetailsList = ConvertDTToList(visitDetailsDT);
                if (visitDetailsList != null)
                    return visitDetailsList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblVisitDetailsTO SelectTblVisitDetails(int idVisit)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisit = " + idVisit + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitDetailsTO> visitDetailsList = ConvertDTToList(visitDetailsDT);
                if (visitDetailsList != null)
                    return visitDetailsList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblVisitDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectAllTblVisitDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        public static List<VisitFirmDetailsTO> SelectVisitFirmDetailsList()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                String sqlQuery = "";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public static Int32 SelectLastVisitId(SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();
            int result;

            try
            {
                //conn.Open();
                //cmdSelect.CommandText = " SELECT MAX(idVisit) FROM tblVisitDetails ";
                cmdSelect.CommandText = " SELECT IDENT_CURRENT('tblVisitDetails') AS 'idVisit' ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                result = Convert.ToInt32(cmdSelect.ExecuteScalar());

                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectLastVisitId");
                return -1;
            }
            finally
            {
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitDetailsTO> ConvertDTToList(SqlDataReader visitDetailsDT)
        {
            List<TblVisitDetailsTO> visitDetailsTOList = new List<TblVisitDetailsTO>();
            if (visitDetailsDT != null)
            {
                while (visitDetailsDT.Read())
                {
                    TblVisitDetailsTO visitDetailsTONew = new TblVisitDetailsTO();
                    if (visitDetailsDT["idVisit"] != DBNull.Value)
                        visitDetailsTONew.IdVisit = Convert.ToInt32(visitDetailsDT["idVisit"].ToString());
                    if (visitDetailsDT["visitTypeId"] != DBNull.Value)
                        visitDetailsTONew.VisitTypeId = Convert.ToInt32(visitDetailsDT["visitTypeId"]);
                    if (visitDetailsDT["siteName"] != DBNull.Value)
                        visitDetailsTONew.SiteName = visitDetailsDT["siteName"].ToString();
                    if (visitDetailsDT["siteOwnerTypeId"] != DBNull.Value)
                        visitDetailsTONew.SiteOwnerTypeId = Convert.ToInt32(visitDetailsDT["siteOwnerTypeId"]);
                    if (visitDetailsDT["siteOwnerId"] != DBNull.Value)
                        visitDetailsTONew.SiteOwnerId = Convert.ToInt32(visitDetailsDT["siteOwnerId"]);
                    if (visitDetailsDT["visitPurposeId"] != DBNull.Value)
                        visitDetailsTONew.VisitPurposeId = Convert.ToInt32(visitDetailsDT["visitPurposeId"]);

                    if (visitDetailsDT["siteTypeId"] != DBNull.Value)
                        visitDetailsTONew.SiteTypeId = Convert.ToInt32(visitDetailsDT["siteTypeId"]);
                    if (visitDetailsDT["sizeOfSite"] != DBNull.Value)
                        visitDetailsTONew.SizeOfSite = Convert.ToSingle(visitDetailsDT["sizeOfSite"]);
                    if (visitDetailsDT["siteSizeUnitId"] != DBNull.Value)
                        visitDetailsTONew.SiteSizeUnitId = Convert.ToInt32(visitDetailsDT["siteSizeUnitId"]);
                    if (visitDetailsDT["roadLanes"] != DBNull.Value)
                        visitDetailsTONew.RoadLanes = Convert.ToInt16(visitDetailsDT["roadLanes"]);
                    if (visitDetailsDT["siteCost"] != DBNull.Value)
                        visitDetailsTONew.SiteCost = Convert.ToSingle(visitDetailsDT["siteCost"]);
                    if (visitDetailsDT["SiteCostUnitId"] != DBNull.Value)
                        visitDetailsTONew.SiteCostUnitId = Convert.ToInt32(visitDetailsDT["SiteCostUnitId"]);

                    if (visitDetailsDT["siteStatusId"] != DBNull.Value)
                        visitDetailsTONew.SiteStatusId = Convert.ToInt32(visitDetailsDT["siteStatusId"]);
                    if (visitDetailsDT["paymentTermId"] != DBNull.Value)
                        visitDetailsTONew.PaymentTermId = Convert.ToInt32(visitDetailsDT["paymentTermId"]);
                    if (visitDetailsDT["siteArchitectId"] != DBNull.Value)
                        visitDetailsTONew.SiteArchitectId = Convert.ToInt32(visitDetailsDT["siteArchitectId"]);
                    if (visitDetailsDT["siteStructuralEnggId"] != DBNull.Value)
                        visitDetailsTONew.SiteStructuralEnggId = Convert.ToInt32(visitDetailsDT["siteStructuralEnggId"]);
                    if (visitDetailsDT["contractorId"] != DBNull.Value)
                        visitDetailsTONew.ContractorId = Convert.ToInt32(visitDetailsDT["contractorId"]);
                    if (visitDetailsDT["purchaseAuthorityPersonId"] != DBNull.Value)
                        visitDetailsTONew.PurchaseAuthorityPersonId = Convert.ToInt32(visitDetailsDT["purchaseAuthorityPersonId"]);

                    if (visitDetailsDT["dealerId"] != DBNull.Value)
                        visitDetailsTONew.DealerId = Convert.ToInt32(visitDetailsDT["dealerId"]);
                    if (visitDetailsDT["dealerMeetingWithId"] != DBNull.Value)
                        visitDetailsTONew.DealerMeetingWithId = Convert.ToInt32(visitDetailsDT["dealerMeetingWithId"]);
                    if (visitDetailsDT["dealerVisitAlongWithDesignationId"] != DBNull.Value)
                        visitDetailsTONew.DealerVisitAlongWithDesignationId = Convert.ToInt32(visitDetailsDT["dealerVisitAlongWithDesignationId"]);
                    if (visitDetailsDT["dealerVisitAlongWithId"] != DBNull.Value)
                        visitDetailsTONew.DealerVisitAlongWithId = Convert.ToInt32(visitDetailsDT["dealerVisitAlongWithId"]);

                    if (visitDetailsDT["notes"] != DBNull.Value)
                        visitDetailsTONew.Notes = visitDetailsDT["notes"].ToString();
                    if (visitDetailsDT["influencerVisitedBy"] != DBNull.Value)
                        visitDetailsTONew.InfluencerVisitedBy = Convert.ToInt32(visitDetailsDT["influencerVisitedBy"]);
                    if (visitDetailsDT["influencerRecommandedBy"] != DBNull.Value)
                        visitDetailsTONew.InfluencerRecommandedBy = Convert.ToInt32(visitDetailsDT["influencerRecommandedBy"]);
                    if (visitDetailsDT["firmId"] != DBNull.Value)
                        visitDetailsTONew.FirmId = Convert.ToInt32(visitDetailsDT["firmId"]);
                    if (visitDetailsDT["firmOwnerId"] != DBNull.Value)
                        visitDetailsTONew.FirmOwnerId = Convert.ToInt32(visitDetailsDT["firmOwnerId"]);
                    if (visitDetailsDT["visitePlace"] != DBNull.Value)
                        visitDetailsTONew.VisitePlace = visitDetailsDT["visitePlace"].ToString();
                    if (visitDetailsDT["visitDate"] != DBNull.Value)
                        visitDetailsTONew.VisitDate = Convert.ToDateTime(visitDetailsDT["visitDate"]);

                    if (visitDetailsDT["timeFrom"] != DBNull.Value)
                        visitDetailsTONew.TimeFrom = TimeSpan.Parse(visitDetailsDT["timeFrom"].ToString());
                    if (visitDetailsDT["timeTo"] != DBNull.Value)
                        visitDetailsTONew.TimeTo = TimeSpan.Parse(visitDetailsDT["timeTo"].ToString());

                    if (visitDetailsDT["createdBy"] != DBNull.Value)
                        visitDetailsTONew.CreatedBy = Convert.ToInt32(visitDetailsDT["createdBy"]);
                    if (visitDetailsDT["createdOn"] != DBNull.Value)
                        visitDetailsTONew.CreatedOn = Convert.ToDateTime(visitDetailsDT["createdOn"]);
                    if (visitDetailsDT["updatedBy"] != DBNull.Value)
                        visitDetailsTONew.UpdatedBy = Convert.ToInt32(visitDetailsDT["updatedBy"]);
                    if (visitDetailsDT["updatedOn"] != DBNull.Value)
                        visitDetailsTONew.UpdatedOn = Convert.ToDateTime(visitDetailsDT["updatedOn"]);

                    //Sudhir[06-DEC-2017] Added for Maintain Logitude and Lattitude.
                    if (visitDetailsDT["locLongitude"] != DBNull.Value)
                        visitDetailsTONew.LocLongitude = visitDetailsDT["locLongitude"].ToString();
                    if (visitDetailsDT["locLattitude"] != DBNull.Value)
                        visitDetailsTONew.LocLatitude = visitDetailsDT["locLattitude"].ToString();

                    visitDetailsTOList.Add(visitDetailsTONew);
                }
            }
            return visitDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                // return ExecuteInsertionCommand(tblVisitDetailsTO, cmdInsert);
                return 0;
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

        public static int InsertTblVisitDetails(ref TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(ref tblVisitDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(ref TblVisitDetailsTO tblVisitDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitDetails]( " +
             "  [visitTypeId]" +
             " ,[siteName]" +
             " ,[siteOwnerTypeId]" +
             " ,[siteOwnerId]" +
             " ,[visitPurposeId]" +
             " ,[siteTypeId]" +
             " ,[sizeOfSite]" +
             " ,[siteSizeUnitId]" +
             " ,[roadLanes]" +
             " ,[siteCost]" +
             " ,[siteStatusId]" +
             " ,[paymentTermId]" +
             " ,[siteArchitectId]" +
            " ,[siteStructuralEnggId]" +
            " ,[contractorId]" +
            " ,[purchaseAuthorityPersonId]" +
            " ,[dealerId]" +
            " ,[dealerMeetingWithId]" +
            " ,[dealerVisitAlongWithDesignationId]" +
            " ,[dealerVisitAlongWithId]" +
            " ,[notes]" +
            " ,[influencerVisitedBy]" +
            " ,[influencerRecommandedBy]" +
            " ,[firmId]" +
            " ,[firmOwnerId]" +
            " ,[visitePlace]" +
            " ,[visitDate]" +
            " ,[timeFrom]" +
            " ,[timeTo]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +

            " ,[SiteCostUnitId]" +
            " ,[locLongitude]" +
            " ,[locLattitude]" +

            " )" +
            " VALUES (" +
            "  @VisitTypeId " +
            " ,@SiteName " +
            " ,@SiteOwnerTypeId " +
            " ,@SiteOwnerId " +
            " ,@VisitPurposeId " +
            " ,@SiteTypeId " +
            " ,@SizeOfSite " +
            " ,@SiteSizeUnitId " +
            " ,@RoadLanes " +
            " ,@SiteCost " +
            " ,@SiteStatusId " +
            " ,@PaymentTermId " +
            " ,@SiteArchitectId " +
            " ,@SiteStructuralEnggId " +
            " ,@ContractorId " +
            " ,@PurchaseAuthorityPersonId " +
            " ,@DealerId " +
            " ,@DealerMeetingWithId " +
            " ,@DealerVisitAlongWithDesignationId " +
            " ,@DealerVisitAlongWithId " +
            " ,@Notes " +
            " ,@InfluencerVisitedBy " +
            " ,@InfluencerRecommandedBy " +
            " ,@FirmId " +
            " ,@FirmOwnerId " +
            " ,@VisitePlace " +
            " ,@VisitDate " +
            " ,@TimeFrom " +
            " ,@TimeTo " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@SiteCostUnitId " +
            " ,@LocLongitude " +
            " ,@LocLattitude " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVisit", System.Data.SqlDbType.Int).Value = tblVisitDetailsTO.IdVisit;
            cmdInsert.Parameters.Add("@VisitTypeId", System.Data.SqlDbType.Int).Value = tblVisitDetailsTO.VisitTypeId;
            cmdInsert.Parameters.Add("@SiteName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteName);
            cmdInsert.Parameters.Add("@SiteOwnerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteOwnerTypeId);
            cmdInsert.Parameters.Add("@SiteOwnerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteOwnerId);            
            cmdInsert.Parameters.Add("@VisitPurposeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitPurposeId);
            cmdInsert.Parameters.Add("@SiteTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteTypeId);

            cmdInsert.Parameters.Add("@SizeOfSite", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SizeOfSite);
            cmdInsert.Parameters.Add("@SiteSizeUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteSizeUnitId);
            cmdInsert.Parameters.Add("@RoadLanes", System.Data.SqlDbType.SmallInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.RoadLanes);
            cmdInsert.Parameters.Add("@SiteCost", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteCost);
            cmdInsert.Parameters.Add("@SiteCostUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteCostUnitId);

            cmdInsert.Parameters.Add("@SiteStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteStatusId);
            cmdInsert.Parameters.Add("@PaymentTermId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.PaymentTermId);

            cmdInsert.Parameters.Add("@SiteArchitectId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteArchitectId);
            cmdInsert.Parameters.Add("@SiteStructuralEnggId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteStructuralEnggId);
            cmdInsert.Parameters.Add("@ContractorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.ContractorId);
            cmdInsert.Parameters.Add("@PurchaseAuthorityPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.PurchaseAuthorityPersonId);
            cmdInsert.Parameters.Add("@DealerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerId);
            cmdInsert.Parameters.Add("@DealerMeetingWithId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerMeetingWithId);

            cmdInsert.Parameters.Add("@DealerVisitAlongWithDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerVisitAlongWithDesignationId);
            cmdInsert.Parameters.Add("@DealerVisitAlongWithId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerVisitAlongWithId);
            cmdInsert.Parameters.Add("@Notes", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.Notes);
            cmdInsert.Parameters.Add("@InfluencerVisitedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.InfluencerVisitedBy);
            cmdInsert.Parameters.Add("@InfluencerRecommandedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.InfluencerRecommandedBy);
            cmdInsert.Parameters.Add("@FirmId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.FirmId);
            cmdInsert.Parameters.Add("@FirmOwnerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.FirmOwnerId);

            cmdInsert.Parameters.Add("@VisitePlace", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitePlace);
            cmdInsert.Parameters.Add("@VisitDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitDate);

            cmdInsert.Parameters.Add("@TimeFrom", System.Data.SqlDbType.Time).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.TimeFrom);
            cmdInsert.Parameters.Add("@TimeTo", System.Data.SqlDbType.Time).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.TimeTo);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.UpdatedOn);

            //Sudhir[06-DEC-2017] Added for Maintain Logitude and Lattitude.
            cmdInsert.Parameters.Add("@LocLongitude", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.LocLongitude);
            cmdInsert.Parameters.Add("@LocLattitude", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.LocLatitude);

            //return cmdInsert.ExecuteNonQuery();

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblVisitDetailsTO.IdVisit = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return -1;
        }
        #endregion

        #region Updation
        //public static int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO)
        //{
        //    String sqlConnStr = Startup.ConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdUpdate.Connection = conn;
        //        return ExecuteUpdationCommand(tblVisitDetailsTO, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdUpdate.Dispose();
        //    }
        //}

        public static int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblVisitDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public static int ExecuteUpdationCommand(TblVisitDetailsTO tblVisitDetailsTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblVisitDetails] SET " +

           "  [siteName]= @SiteName " +
           " ,[siteOwnerTypeId]=@SiteOwnerTypeId " +
           " ,[siteOwnerId]=@SiteOwnerId " +
           " ,[visitPurposeId]=@VisitPurposeId " +
           " ,[siteTypeId]=@SiteTypeId " +
           " ,[sizeOfSite]=@SizeOfSite " +
           " ,[siteSizeUnitId]=@SiteSizeUnitId " +
           " ,[roadLanes]=@RoadLanes " +
           " ,[siteCost]=@SiteCost " +
           " ,[siteStatusId]=@SiteStatusId " +
           " ,[paymentTermId]=@PaymentTermId " +
           " ,[siteArchitectId]=@SiteArchitectId " +
           " ,[siteStructuralEnggId]=@SiteStructuralEnggId " +
           " ,[contractorId]=@ContractorId " +
           " ,[purchaseAuthorityPersonId]=@PurchaseAuthorityPersonId " +
           " ,[dealerId]=@DealerId " +
           " ,[dealerMeetingWithId]=@DealerMeetingWithId " +
           " ,[dealerVisitAlongWithDesignationId]=@DealerVisitAlongWithDesignationId " +
           " ,[dealerVisitAlongWithId]=@DealerVisitAlongWithId " +
           " ,[notes]=@Notes " +
           " ,[influencerVisitedBy]=@InfluencerVisitedBy " +
           " ,[influencerRecommandedBy]=@InfluencerRecommandedBy " +
           " ,[firmId]=@FirmId " +
           " ,[firmOwnerId]=@FirmOwnerId " +
           " ,[visitePlace]=@VisitePlace " +
           " ,[visitDate]=@VisitDate " +
           " ,[timeFrom]=@TimeFrom " +
           " ,[timeTo]=@TimeTo " +
           //" ,[createdBy=@CreatedBy " +
           " ,[updatedBy]=@UpdatedBy " +
           //" ,[createdOn]=@CreatedOn " +
           " ,[updatedOn]=@UpdatedOn " +
           " ,[SiteCostUnitId]=@SiteCostUnitId "+
            " WHERE [idVisit]= @IdVisit";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisit", System.Data.SqlDbType.Int).Value = tblVisitDetailsTO.IdVisit;
            cmdUpdate.Parameters.Add("@VisitTypeId", System.Data.SqlDbType.Int).Value = tblVisitDetailsTO.VisitTypeId;
            cmdUpdate.Parameters.Add("@SiteName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteName);
            cmdUpdate.Parameters.Add("@SiteOwnerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteOwnerTypeId);
            cmdUpdate.Parameters.Add("@SiteOwnerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteOwnerId);
            cmdUpdate.Parameters.Add("@VisitPurposeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitPurposeId);
            cmdUpdate.Parameters.Add("@SiteTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteTypeId);

            cmdUpdate.Parameters.Add("@SizeOfSite", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SizeOfSite);
            cmdUpdate.Parameters.Add("@SiteSizeUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteSizeUnitId);
            cmdUpdate.Parameters.Add("@RoadLanes", System.Data.SqlDbType.SmallInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.RoadLanes);
            cmdUpdate.Parameters.Add("@SiteCost", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteCost);
            cmdUpdate.Parameters.Add("@SiteCostUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteCostUnitId);

            cmdUpdate.Parameters.Add("@SiteStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteStatusId);
            cmdUpdate.Parameters.Add("@PaymentTermId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.PaymentTermId);

            cmdUpdate.Parameters.Add("@SiteArchitectId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteArchitectId);
            cmdUpdate.Parameters.Add("@SiteStructuralEnggId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.SiteStructuralEnggId);
            cmdUpdate.Parameters.Add("@ContractorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.ContractorId);
            cmdUpdate.Parameters.Add("@PurchaseAuthorityPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.PurchaseAuthorityPersonId);
            cmdUpdate.Parameters.Add("@DealerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerId);
            cmdUpdate.Parameters.Add("@DealerMeetingWithId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerMeetingWithId);

            cmdUpdate.Parameters.Add("@DealerVisitAlongWithDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerVisitAlongWithDesignationId);
            cmdUpdate.Parameters.Add("@DealerVisitAlongWithId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.DealerVisitAlongWithId);
            cmdUpdate.Parameters.Add("@Notes", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.Notes);
            cmdUpdate.Parameters.Add("@InfluencerVisitedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.InfluencerVisitedBy);
            cmdUpdate.Parameters.Add("@InfluencerRecommandedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.InfluencerRecommandedBy);
            cmdUpdate.Parameters.Add("@FirmId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.FirmId);
            cmdUpdate.Parameters.Add("@FirmOwnerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.FirmOwnerId);

            cmdUpdate.Parameters.Add("@VisitePlace", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitePlace);
            cmdUpdate.Parameters.Add("@VisitDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.VisitDate);

            cmdUpdate.Parameters.Add("@TimeFrom", System.Data.SqlDbType.Time).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.TimeFrom);
            cmdUpdate.Parameters.Add("@TimeTo", System.Data.SqlDbType.Time).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.TimeTo);
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.UpdatedBy);
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitDetailsTO.UpdatedOn);

           return cmdUpdate.ExecuteNonQuery();
        }
       
        #endregion

        #region Deletion
        public static int DeleteTblVisitDetails(int idVisit)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisit, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblVisitDetails(int idVisit, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisit, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(int idVisit, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitDetails] " +
            " WHERE idVisit = " + idVisit + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisit", System.Data.SqlDbType.Int).Value = tblVisitDetailsTO.IdVisit;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
