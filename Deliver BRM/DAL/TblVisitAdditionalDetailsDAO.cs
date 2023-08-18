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
    public class TblVisitAdditionalDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitAdditionalDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblVisitAdditionalDetails()
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

        public static DataTable SelectTblVisitAdditionalDetails(Int32 idVisitDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisitDetails = " + idVisitDetails + " ";
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

        public static DataTable SelectAllTblVisitAdditionalDetails(SqlConnection conn, SqlTransaction tran)
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

        public static TblVisitAdditionalDetailsTO SelectVisitAdditionalDetails(Int32 visitId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE visitId = " + visitId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitAdditionalDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitAdditionalDetailsTO> visitAdditionalDetailsTOList = ConvertDTToList(visitAdditionalDetailsDT);
                if (visitAdditionalDetailsTOList != null)
                    return visitAdditionalDetailsTOList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitAdditionalDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitAdditionalDetailsTO> ConvertDTToList(SqlDataReader visitAdditionalDetailsDT)
        {
            List<TblVisitAdditionalDetailsTO> visitDetailsTOList = new List<TblVisitAdditionalDetailsTO>();
            if (visitAdditionalDetailsDT != null)
            {
                while (visitAdditionalDetailsDT.Read())
                {
                    TblVisitAdditionalDetailsTO visitAdditionalDetailsTONew = new TblVisitAdditionalDetailsTO();
                    if (visitAdditionalDetailsDT["idVisitAdditionalDetails"] != DBNull.Value)
                        visitAdditionalDetailsTONew.IdVisitDetails = Convert.ToInt32(visitAdditionalDetailsDT["idVisitAdditionalDetails"].ToString());
                    if (visitAdditionalDetailsDT["visitId"] != DBNull.Value)
                        visitAdditionalDetailsTONew.VisitId = Convert.ToInt32(visitAdditionalDetailsDT["visitId"]);
                    if (visitAdditionalDetailsDT["siteComplaintReferredBy"] != DBNull.Value)
                        visitAdditionalDetailsTONew.SiteComplaintReferredBy = Convert.ToInt32(visitAdditionalDetailsDT["siteComplaintReferredBy"]);
                    if (visitAdditionalDetailsDT["communicationPersonId"] != DBNull.Value)
                        visitAdditionalDetailsTONew.CommunicationPersonId = Convert.ToInt32(visitAdditionalDetailsDT["communicationPersonId"]);
                    if (visitAdditionalDetailsDT["otherSiteNotes"] != DBNull.Value)
                        visitAdditionalDetailsTONew.OtherSiteNotes = visitAdditionalDetailsDT["otherSiteNotes"].ToString();
                    if (visitAdditionalDetailsDT["remindMeOn"] != DBNull.Value)
                        visitAdditionalDetailsTONew.RemindMeOn = Convert.ToDateTime(visitAdditionalDetailsDT["remindMeOn"]);

                    if (visitAdditionalDetailsDT["benifits"] != DBNull.Value)
                        visitAdditionalDetailsTONew.Benifits = Convert.ToInt16(visitAdditionalDetailsDT["benifits"]);
                    if (visitAdditionalDetailsDT["additionalFeatures"] != DBNull.Value)
                        visitAdditionalDetailsTONew.AdditionalFeatures = Convert.ToInt16(visitAdditionalDetailsDT["additionalFeatures"]);

                    if (visitAdditionalDetailsDT["orgAwareness"] != DBNull.Value)
                        visitAdditionalDetailsTONew.OrgAwareness =Convert.ToInt16(visitAdditionalDetailsDT["orgAwareness"]);
                    if (visitAdditionalDetailsDT["qualityAwareness"] != DBNull.Value)
                        visitAdditionalDetailsTONew.QualityAwareness = Convert.ToInt16(visitAdditionalDetailsDT["qualityAwareness"]);
                    if (visitAdditionalDetailsDT["visitedFactory"] != DBNull.Value)
                        visitAdditionalDetailsTONew.VisitedFactory = Convert.ToInt16(visitAdditionalDetailsDT["visitedFactory"]);
                    if (visitAdditionalDetailsDT["materialUsedBefore"] != DBNull.Value)
                        visitAdditionalDetailsTONew.MaterialUsedBefore = Convert.ToInt16(visitAdditionalDetailsDT["materialUsedBefore"]);
                    if (visitAdditionalDetailsDT["satesfactoryLevel"] != DBNull.Value)
                        visitAdditionalDetailsTONew.SatesfactoryLevel = Convert.ToInt16(visitAdditionalDetailsDT["satesfactoryLevel"]);
                    if (visitAdditionalDetailsDT["previousVisitedByRepresentative"] != DBNull.Value)
                        visitAdditionalDetailsTONew.PreviousVisitedByRepresentative = Convert.ToInt16(visitAdditionalDetailsDT["previousVisitedByRepresentative"]);

                    if (visitAdditionalDetailsDT["comments"] != DBNull.Value)
                        visitAdditionalDetailsTONew.Comments = visitAdditionalDetailsDT["comments"].ToString();
                    if (visitAdditionalDetailsDT["suggestionsForOrg"] != DBNull.Value)
                        visitAdditionalDetailsTONew.SuggestionsForOrg =visitAdditionalDetailsDT["suggestionsForOrg"].ToString();
                    if (visitAdditionalDetailsDT["giftId"] != DBNull.Value)
                        visitAdditionalDetailsTONew.GiftId = Convert.ToInt32(visitAdditionalDetailsDT["giftId"]);
                    if (visitAdditionalDetailsDT["designationId"] != DBNull.Value)
                        visitAdditionalDetailsTONew.DesignationId = Convert.ToInt32(visitAdditionalDetailsDT["designationId"]);
                    if (visitAdditionalDetailsDT["createdBy"] != DBNull.Value)
                        visitAdditionalDetailsTONew.CreatedBy = Convert.ToInt32(visitAdditionalDetailsDT["createdBy"]);
                    if (visitAdditionalDetailsDT["createdOn"] != DBNull.Value)
                        visitAdditionalDetailsTONew.CreatedOn = Convert.ToDateTime(visitAdditionalDetailsDT["createdOn"]);

                    if (visitAdditionalDetailsDT["updatedBy"] != DBNull.Value)
                        visitAdditionalDetailsTONew.UpdatedBy = Convert.ToInt32(visitAdditionalDetailsDT["updatedBy"]);
                    if (visitAdditionalDetailsDT["updatedOn"] != DBNull.Value)
                        visitAdditionalDetailsTONew.UpdatedOn = Convert.ToDateTime(visitAdditionalDetailsDT["updatedOn"]);


                    visitDetailsTOList.Add(visitAdditionalDetailsTONew);
                }
            }
            return visitDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitAdditionalDetailsTO, cmdInsert);
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

        public static int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitAdditionalDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitAdditionalDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitAdditionalDetails]( " +
            " [visitId]" +
            " ,[siteComplaintReferredBy]" +
            " ,[communicationPersonId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[remindMeOn]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[otherSiteNotes]" +
            " ,[benifits]" +
            " ,[additionalFeatures]" +
            " ,[Comments]" +

            " ,[orgAwareness]" +
            " ,[qualityAwareness]" +
            " ,[visitedFactory]" +
            " ,[materialUsedBefore]" +
            " ,[satesfactoryLevel]" +
            " ,[previousVisitedByRepresentative]" +
            " ,[suggestionsForOrg]" +
            " ,[giftId]" +
            " ,[designationId]" + 
            " )" +
            " VALUES (" +         
            " @VisitId " +
            " ,@SiteComplaintReferredBy " +
            " ,@CommunicationPersonId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@RemindMeOn " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@OtherSiteNotes " +
            " ,@Benifits " +
            " ,@AdditionalFeatures " +
            " ,@Comments " +

            " ,@OrgAwareness " +
            " ,@QualityAwareness " +
            " ,@VisitedFactory " +
            " ,@MaterialUsedBefore " +
            " ,@SatesfactoryLevel " +
            " ,@PreviousVisitedByRepresentative " +
             " ,@SuggestionsForOrg " +
             " ,@GiftId " +
             " ,@DesignationId " +
             
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.VisitId;
            cmdInsert.Parameters.Add("@SiteComplaintReferredBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SiteComplaintReferredBy);
            cmdInsert.Parameters.Add("@CommunicationPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.CommunicationPersonId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@RemindMeOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.RemindMeOn);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitAdditionalDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@OtherSiteNotes", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.OtherSiteNotes);
            cmdInsert.Parameters.Add("@Benifits", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.Benifits);
            cmdInsert.Parameters.Add("@AdditionalFeatures", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.AdditionalFeatures);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.Comments);

            cmdInsert.Parameters.Add("@OrgAwareness", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.OrgAwareness);
            cmdInsert.Parameters.Add("@QualityAwareness", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.QualityAwareness);
            cmdInsert.Parameters.Add("@VisitedFactory", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.VisitedFactory);
            cmdInsert.Parameters.Add("@MaterialUsedBefore", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.MaterialUsedBefore);
            cmdInsert.Parameters.Add("@SatesfactoryLevel", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SatesfactoryLevel);
            cmdInsert.Parameters.Add("@PreviousVisitedByRepresentative", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.PreviousVisitedByRepresentative);            
            cmdInsert.Parameters.Add("@SuggestionsForOrg", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SuggestionsForOrg);
            cmdInsert.Parameters.Add("@GiftId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.GiftId);
            cmdInsert.Parameters.Add("@DesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.DesignationId);

            return cmdInsert.ExecuteNonQuery(); 
        }

        public static int InsertTblVisitAdditionalDetailsAboutCompany(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                //return ExecuteInsertionVisitAdditionalDetailsAboutCompanyCommand(tblVisitAdditionalDetailsTO, cmdInsert);
                return 1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitAdditionalDetailsAboutCompany");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        //public static int ExecuteInsertionVisitAdditionalDetailsAboutCompanyCommand(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlCommand cmdInsert)
        //{
        //    String sqlQuery = @" INSERT INTO [tblVisitAdditionalDetails]( " +
        //    " ,[visitId]" +
        //    " ,[polaadInfoId]" +
        //    " ,[polaadInfoDetails]" +
        //    " )" +
        //    " VALUES (" +          
        //    " ,@VisitId " +
        //    " ,@PolaadInfoId " +
        //    " ,@PolaadInfoDetails " +
        //    " )";
        //    cmdInsert.CommandText = sqlQuery;
        //    cmdInsert.CommandType = System.Data.CommandType.Text;

        //    cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.VisitId;
        //    cmdInsert.Parameters.Add("@PolaadInfoId", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.PolaadInfoId;
        //    cmdInsert.Parameters.Add("@PolaadInfoDetails", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.PolaadInfoDetails;
        //    return cmdInsert.ExecuteNonQuery();
        //}


        #endregion

        #region Updation
        public static int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitAdditionalDetailsTO, cmdUpdate);
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

        public static int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitAdditionalDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblVisitAdditionalDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblVisitAdditionalDetails] SET " +
             " [siteComplaintReferredBy]=@SiteComplaintReferredBy " +
             " ,[communicationPersonId]=@CommunicationPersonId " +
             " ,[updatedBy]=@UpdatedBy " +
             " ,[remindMeOn]=@RemindMeOn " +
             " ,[updatedOn]=@UpdatedOn " +
             " ,[otherSiteNotes]=@OtherSiteNotes " +
             " ,[benifits]=@Benifits " +
             " ,[additionalFeatures]=@AdditionalFeatures " +
             " ,[Comments]=@Comments " +

             " ,[orgAwareness]=@OrgAwareness " +
             " ,[qualityAwareness]=@QualityAwareness " +
             " ,[visitedFactory]=@VisitedFactory " +
             " ,[materialUsedBefore]=@MaterialUsedBefore " +
             " ,[satesfactoryLevel]=@SatesfactoryLevel " +
             " ,[previousVisitedByRepresentative]=@PreviousVisitedByRepresentative " +
              " ,[suggestionsForOrg]=@SuggestionsForOrg " +
              " ,[giftId]=@GiftId " +
              " ,[designationId]=@DesignationId " +
            // "WHERE  [idVisitAdditionalDetails]=@IdVisitAdditionalDetails AND [visitId]=@VisitId ";
             "WHERE [visitId]=@VisitId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisitAdditionalDetails", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.IdVisitDetails);
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.VisitId);

            cmdUpdate.Parameters.Add("@SiteComplaintReferredBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SiteComplaintReferredBy);
            cmdUpdate.Parameters.Add("@CommunicationPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.CommunicationPersonId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@RemindMeOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.RemindMeOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@OtherSiteNotes", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.OtherSiteNotes);
            cmdUpdate.Parameters.Add("@Benifits", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.Benifits);
            cmdUpdate.Parameters.Add("@AdditionalFeatures", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.AdditionalFeatures);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.Comments);

            cmdUpdate.Parameters.Add("@OrgAwareness", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.OrgAwareness);
            cmdUpdate.Parameters.Add("@QualityAwareness", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.QualityAwareness);
            cmdUpdate.Parameters.Add("@VisitedFactory", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.VisitedFactory);
            cmdUpdate.Parameters.Add("@MaterialUsedBefore", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.MaterialUsedBefore);
            cmdUpdate.Parameters.Add("@SatesfactoryLevel", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SatesfactoryLevel);
            cmdUpdate.Parameters.Add("@PreviousVisitedByRepresentative", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.PreviousVisitedByRepresentative);
            cmdUpdate.Parameters.Add("@SuggestionsForOrg", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.SuggestionsForOrg);
            cmdUpdate.Parameters.Add("@GiftId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.GiftId);
            cmdUpdate.Parameters.Add("@DesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitAdditionalDetailsTO.DesignationId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisitDetails, cmdDelete);
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

        public static int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisitDetails, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idVisitDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitAdditionalDetails] " +
            " WHERE idVisitDetails = " + idVisitDetails + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisitDetails", System.Data.SqlDbType.Int).Value = tblVisitAdditionalDetailsTO.IdVisitDetails;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
