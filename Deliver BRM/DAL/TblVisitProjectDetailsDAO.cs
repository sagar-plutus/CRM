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
    public class TblVisitProjectDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitProjectDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVisitProjectDetailsTO> SelectAllTblVisitProjectDetails(Int32 visitId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                String sqlQuery= " SELECT idProject,visitId,projectTypeId, projectName,projectAddress,person.idPerson, " +
                                 " CONCAT(person.firstName, person.midName, person.lastName) as 'contactPersonName', "+
                                 " ISNULL(mobileNo, alternateMobNo) as 'mobileNo',ISNULL(primaryEmail, alternateEmail) as 'emailId', "+
                                 " completionYear,visitprojectdtl.createdBy,visitprojectdtl.createdOn,updatedBy,updatedOn "+
                                 " FROM tblVisitProjectDetails visitprojectdtl "+
                                 " LEFT JOIN tblPerson person On person.idPerson = visitprojectdtl.contactPersonId"+
                                 " WHERE visitprojectdtl.visitId = " +visitId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader projectDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitProjectDetailsTO> projectDetailsList = ConvertDTToList(projectDetailsDT);
                if (projectDetailsList != null)
                    return projectDetailsList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblProjectDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectTblVisitProjectDetails(Int32 idProject)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProject = " + idProject + " ";
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

        public static DataTable SelectAllTblVisitProjectDetails(SqlConnection conn, SqlTransaction tran)
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

        public static List<TblVisitProjectDetailsTO> ConvertDTToList(SqlDataReader projectDetailsDT)
        {
            List<TblVisitProjectDetailsTO> projectDetailsTOList = new List<TblVisitProjectDetailsTO>();
            if (projectDetailsDT != null)
            {
                while (projectDetailsDT.Read())
                {
                    TblVisitProjectDetailsTO projectDetailsTONew = new TblVisitProjectDetailsTO();
                    if (projectDetailsDT["idProject"] != DBNull.Value)
                        projectDetailsTONew.IdProject = Convert.ToInt32(projectDetailsDT["idProject"].ToString());
                    if (projectDetailsDT["visitId"] != DBNull.Value)
                        projectDetailsTONew.VisitId = Convert.ToInt32(projectDetailsDT["visitId"]);
                    if (projectDetailsDT["projectTypeId"] != DBNull.Value)
                        projectDetailsTONew.ProjectTypeId = Convert.ToInt32(projectDetailsDT["projectTypeId"]);
                    if (projectDetailsDT["projectName"] != DBNull.Value)
                        projectDetailsTONew.ProjectName = projectDetailsDT["projectName"].ToString();
                    if (projectDetailsDT["projectAddress"] != DBNull.Value)
                        projectDetailsTONew.ProjectAddress = projectDetailsDT["projectAddress"].ToString();
                    if (projectDetailsDT["idPerson"] != DBNull.Value)
                        projectDetailsTONew.ContactPersonId = Convert.ToInt32(projectDetailsDT["idPerson"]);
                    if (projectDetailsDT["contactPersonName"] != DBNull.Value)
                        projectDetailsTONew.ContactPersonName = projectDetailsDT["contactPersonName"].ToString().Trim('-');
                    if (projectDetailsDT["mobileNo"] != DBNull.Value)
                        projectDetailsTONew.ContactNo = projectDetailsDT["mobileNo"].ToString();
                    if (projectDetailsDT["emailId"] != DBNull.Value)
                        projectDetailsTONew.EmailId = projectDetailsDT["emailId"].ToString();
                    if (projectDetailsDT["completionYear"] != DBNull.Value)
                        projectDetailsTONew.CompletionYear = projectDetailsDT["completionYear"].ToString();
                    if (projectDetailsDT["createdBy"] != DBNull.Value)
                        projectDetailsTONew.CreatedBy = Convert.ToInt32(projectDetailsDT["createdBy"]);
                    if (projectDetailsDT["createdOn"] != DBNull.Value)
                        projectDetailsTONew.CreatedOn = Convert.ToDateTime(projectDetailsDT["createdOn"]);
                    if (projectDetailsDT["updatedBy"] != DBNull.Value)
                        projectDetailsTONew.UpdatedBy = Convert.ToInt32(projectDetailsDT["updatedBy"]);
                    if (projectDetailsDT["updatedOn"] != DBNull.Value)
                        projectDetailsTONew.UpdatedOn = Convert.ToDateTime(projectDetailsDT["updatedOn"]);

                    projectDetailsTOList.Add(projectDetailsTONew);
                }
            }
            return projectDetailsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString; ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitProjectDetailsTO, cmdInsert);
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

        public static int InsertTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitProjectDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitProjectDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitProjectDetails]( " +
            " [visitId]" +
            " ,[projectTypeId]" +
            " ,[contactPersonId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[projectName]" +
            " ,[projectAddress]" +
            " ,[completionYear]" +
            " )" +
            " VALUES (" +
            " @VisitId " +
            " ,@ProjectTypeId " +
            " ,@ContactPersonId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@ProjectName " +
            " ,@ProjectAddress " +
            " ,@CompletionYear " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProject", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.IdProject;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.VisitId;
            cmdInsert.Parameters.Add("@ProjectTypeId", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.ProjectTypeId;
            cmdInsert.Parameters.Add("@ContactPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitProjectDetailsTO.ContactPersonId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitProjectDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitProjectDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitProjectDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ProjectName", System.Data.SqlDbType.NVarChar).Value = tblVisitProjectDetailsTO.ProjectName;
            cmdInsert.Parameters.Add("@ProjectAddress", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitProjectDetailsTO.ProjectAddress);
            cmdInsert.Parameters.Add("@CompletionYear", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVisitProjectDetailsTO.CompletionYear);

           // return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblVisitProjectDetailsTO.IdProject = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString; 
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitProjectDetailsTO, cmdUpdate);
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

        public static int UpdateTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitProjectDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblVisitProjectDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlCommand cmdUpdate)
        {

          String sqlQuery = @" UPDATE [tblVisitProjectDetails] SET " +

            " [projectTypeId]= @ProjectTypeId" +
            " ,[contactPersonId]= @ContactPersonId" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[projectName]= @ProjectName" +
            " ,[projectAddress]= @ProjectAddress" +
            " ,[completionYear] = @CompletionYear" +
            " WHERE [idProject]=@IdProject AND [visitId]=@VisitId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProject", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.IdProject;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.VisitId;
            cmdUpdate.Parameters.Add("@ProjectTypeId", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.ProjectTypeId;
            cmdUpdate.Parameters.Add("@ContactPersonId", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.ContactPersonId;          
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitProjectDetailsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ProjectName", System.Data.SqlDbType.NVarChar).Value = tblVisitProjectDetailsTO.ProjectName;
            cmdUpdate.Parameters.Add("@ProjectAddress", System.Data.SqlDbType.NVarChar).Value = tblVisitProjectDetailsTO.ProjectAddress;
            cmdUpdate.Parameters.Add("@CompletionYear", System.Data.SqlDbType.NVarChar).Value = tblVisitProjectDetailsTO.CompletionYear;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitProjectDetails(Int32 idProject)
        {
            String sqlConnStr = Startup.ConnectionString; ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProject, cmdDelete);
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

        public static int DeleteTblVisitProjectDetails(Int32 idProject, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProject, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idProject, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitProjectDetails] " +
            " WHERE idProject = " + idProject + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProject", System.Data.SqlDbType.Int).Value = tblVisitProjectDetailsTO.IdProject;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
    }
}
