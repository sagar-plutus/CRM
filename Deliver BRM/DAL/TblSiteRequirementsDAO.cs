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
    public class TblSiteRequirementsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSiteRequirements]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblSiteRequirements()
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

        public static DataTable SelectTblSiteRequirements(Int32 idSiteRequirement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idSiteRequirement = " + idSiteRequirement + " ";
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

        public static DataTable SelectAllTblSiteRequirements(SqlConnection conn, SqlTransaction tran)
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


        public static TblSiteRequirementsTO SelectSiteRequirements(Int32 visitId)
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

                SqlDataReader siteRequirementsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSiteRequirementsTO> siteRequirementsTOList = ConvertDTToList(siteRequirementsDT);
                if (siteRequirementsTOList != null)
                    return siteRequirementsTOList[0];
                else
                return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblSiteRequirements");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSiteRequirementsTO> ConvertDTToList(SqlDataReader siteRequirementsDT)
        {
            List<TblSiteRequirementsTO> visitDetailsTOList = new List<TblSiteRequirementsTO>();
            if (siteRequirementsDT != null)
            {
                while (siteRequirementsDT.Read())
                {
                    TblSiteRequirementsTO siteRequirementTONew = new TblSiteRequirementsTO();
                    if (siteRequirementsDT["idSiteRequirement"] != DBNull.Value)
                        siteRequirementTONew.IdSiteRequirement = Convert.ToInt32(siteRequirementsDT["idSiteRequirement"].ToString());
                    if (siteRequirementsDT["visitId"] != DBNull.Value)
                        siteRequirementTONew.VisitId = Convert.ToInt32(siteRequirementsDT["visitId"]);
                    if (siteRequirementsDT["steelReqForTotalProject"] != DBNull.Value)
                        siteRequirementTONew.SteelReqForTotalProject = Convert.ToSingle(siteRequirementsDT["steelReqForTotalProject"]);
                    if (siteRequirementsDT["boughtSoFor"] != DBNull.Value)
                        siteRequirementTONew.BoughtSoFor = Convert.ToSingle(siteRequirementsDT["boughtSoFor"]);
                    if (siteRequirementsDT["boughtFrom"] != DBNull.Value)
                        siteRequirementTONew.BoughtFrom = Convert.ToInt32(siteRequirementsDT["boughtFrom"]);
                    if (siteRequirementsDT["brandId"] != DBNull.Value)
                        siteRequirementTONew.BrandId = Convert.ToInt32(siteRequirementsDT["brandId"]);

                    if (siteRequirementsDT["immediateReq"] != DBNull.Value)
                        siteRequirementTONew.ImmediateReq = Convert.ToSingle(siteRequirementsDT["immediateReq"]);
                    if (siteRequirementsDT["createdBy"] != DBNull.Value)
                        siteRequirementTONew.CreatedBy = Convert.ToInt32(siteRequirementsDT["createdBy"]);
                    if (siteRequirementsDT["createdOn"] != DBNull.Value)
                        siteRequirementTONew.CreatedOn = Convert.ToDateTime(siteRequirementsDT["createdOn"]);

                    if (siteRequirementsDT["updatedBy"] != DBNull.Value)
                        siteRequirementTONew.UpdatedBy = Convert.ToInt32(siteRequirementsDT["updatedBy"]);
                    if (siteRequirementsDT["updatedOn"] != DBNull.Value)
                        siteRequirementTONew.UpdatedOn = Convert.ToDateTime(siteRequirementsDT["updatedOn"]);


                    visitDetailsTOList.Add(siteRequirementTONew);
                }
            }
            return visitDetailsTOList;
        }


        #endregion

        #region Insertion
        public static int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSiteRequirementsTO, cmdInsert);
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

        public static int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSiteRequirementsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblSiteRequirements");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblSiteRequirementsTO tblSiteRequirementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSiteRequirements]( " +
           // "  [idSiteRequirement]" +
            "  [visitId]" +
            " ,[boughtFrom]" +
            " ,[brandId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[steelReqForTotalProject]" +
            " ,[boughtSoFor]" +
            " ,[immediateReq]" +
            " )" +
            " VALUES (" +
            //"  @IdSiteRequirement " +
            " @VisitId " +
            " ,@BoughtFrom " +
            " ,@BrandId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@SteelReqForTotalProject " +
            " ,@BoughtSoFor " +
            " ,@ImmediateReq " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSiteRequirement", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.IdSiteRequirement;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.VisitId;
            cmdInsert.Parameters.Add("@BoughtFrom", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.BoughtFrom);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.BrandId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteRequirementsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@SteelReqForTotalProject", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.SteelReqForTotalProject);
            cmdInsert.Parameters.Add("@BoughtSoFor", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.BoughtSoFor);
            cmdInsert.Parameters.Add("@ImmediateReq", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblSiteRequirementsTO.ImmediateReq);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSiteRequirementsTO, cmdUpdate);
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

        public static int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSiteRequirementsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblSiteRequirements");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblSiteRequirementsTO tblSiteRequirementsTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblSiteRequirements] SET " +
            " [boughtFrom]= @BoughtFrom" +
            " ,[brandId]= @BrandId" +
            //" ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            //" ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[steelReqForTotalProject]= @SteelReqForTotalProject" +
            " ,[boughtSoFor]= @BoughtSoFor" +
            " ,[immediateReq] = @ImmediateReq" +
           // " WHERE [idSiteRequirement]=@IdSiteRequirement AND [visitId]= @VisitId "; //Sudhir[16-NOV-2017] Update Record on VisitId
           " WHERE [visitId]= @VisitId "; 
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSiteRequirement", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.IdSiteRequirement;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.VisitId;
            cmdUpdate.Parameters.Add("@BoughtFrom", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.BoughtFrom;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.BrandId;
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.UpdatedBy;
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteRequirementsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblSiteRequirementsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@SteelReqForTotalProject", System.Data.SqlDbType.NVarChar).Value = tblSiteRequirementsTO.SteelReqForTotalProject;
            cmdUpdate.Parameters.Add("@BoughtSoFor", System.Data.SqlDbType.NVarChar).Value = tblSiteRequirementsTO.BoughtSoFor;
            cmdUpdate.Parameters.Add("@ImmediateReq", System.Data.SqlDbType.NVarChar).Value = tblSiteRequirementsTO.ImmediateReq;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblSiteRequirements(Int32 idSiteRequirement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSiteRequirement, cmdDelete);
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

        public static int DeleteTblSiteRequirements(Int32 idSiteRequirement, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSiteRequirement, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSiteRequirement, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSiteRequirements] " +
            " WHERE idSiteRequirement = " + idSiteRequirement + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSiteRequirement", System.Data.SqlDbType.Int).Value = tblSiteRequirementsTO.IdSiteRequirement;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
