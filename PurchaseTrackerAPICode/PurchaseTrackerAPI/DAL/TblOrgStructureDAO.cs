using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblOrgStructureDAO: ITblOrgStructureDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblOrgStructureDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOrgStructure]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblOrgStructureTO> SelectAllOrgStructureHierarchy()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructureDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureTO> orgStructureList = ConvertDTToList(orgStructureDT);
                if (orgStructureList != null)
                    return orgStructureList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllOrgStructureHierarchy");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblOrgStructureTO SelectTblOrgStructure(Int32 idOrgStructure)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgStructure = " + idOrgStructure + " ";
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

        public  TblOrgStructureTO SelectAllTblOrgStructure(SqlConnection conn, SqlTransaction tran)
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

        public  List<TblOrgStructureTO> ConvertDTToList(SqlDataReader orgStructureDT)
        {
            List<TblOrgStructureTO> orgStructureTOList = new List<TblOrgStructureTO>();
            if (orgStructureDT != null)
            {
                while (orgStructureDT.Read())
                {
                    TblOrgStructureTO orgStructureTONew = new TblOrgStructureTO();
                    if (orgStructureDT["idOrgStructure"] != DBNull.Value)
                        orgStructureTONew.IdOrgStructure = Convert.ToInt32(orgStructureDT["idOrgStructure"].ToString());
                    if (orgStructureDT["parentOrgStructureId"] != DBNull.Value)
                        orgStructureTONew.ParentOrgStructureId = Convert.ToInt32(orgStructureDT["parentOrgStructureId"]);
                    if (orgStructureDT["deptId"] != DBNull.Value)
                        orgStructureTONew.DeptId = Convert.ToInt32(orgStructureDT["deptId"]);
                    if (orgStructureDT["designationId"] != DBNull.Value)
                        orgStructureTONew.DesignationId = Convert.ToInt32(orgStructureDT["designationId"]);
                    if (orgStructureDT["orgStructureDesc"] != DBNull.Value)
                        orgStructureTONew.OrgStructureDesc = orgStructureDT["orgStructureDesc"].ToString();
                    if (orgStructureDT["createdBy"] != DBNull.Value)
                        orgStructureTONew.CreatedBy = Convert.ToInt32(orgStructureDT["createdBy"]);
                    if (orgStructureDT["createdOn"] != DBNull.Value)
                        orgStructureTONew.CreatedOn = Convert.ToDateTime(orgStructureDT["createdOn"]);
                    if (orgStructureDT["updatedBy"] != DBNull.Value)
                        orgStructureTONew.UpdatedBy = Convert.ToInt32(orgStructureDT["updatedBy"]);
                    if (orgStructureDT["updatedOn"] != DBNull.Value)
                        orgStructureTONew.UpdatedOn = Convert.ToDateTime(orgStructureDT["updatedOn"]);
                    if (orgStructureDT["isActive"] != DBNull.Value)
                        orgStructureTONew.IsActive = Convert.ToInt16(orgStructureDT["isActive"]);

                    orgStructureTOList.Add(orgStructureTONew);
                }
            }
            return orgStructureTOList;
        }

        public  List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (orgStructureId == 0)
                {
                    queryStr = " SELECT tbluser.idUser,tbluser.userDisplayName AS 'userName',tbluser.idUser AS 'idReportingTo' ," +
                               " tbluser.userDisplayName AS 'reportingTo',reportingtype.idReportingType, reportingtype.reportingTypeName,remark , " +
                               " userreportingdetails.idUserReportingDetails,userreportingdetails.orgStructureId,userreportingdetails.isActive " +
                               " FROM tblUserReportingDetails userreportingdetails  INNER JOIN tblOrgStructure orgstructure ON " +
                               " userreportingdetails.orgStructureId = orgstructure.idOrgStructure  LEFT JOIN tbluser tbluser ON " +
                               " tbluser.idUser = userreportingdetails.userId  INNER JOIN dimReportingType reportingtype " +
                               " ON reportingtype.idReportingType = userreportingdetails.reportingTypeId " +
                               " WHERE orgstructure.isActive = 1 AND tbluser.isActive = 1  AND userreportingdetails.isActive = 1 ";
                }
                else
                {
                    queryStr = " SELECT tbluser.idUser,tbluser.userDisplayName AS 'userName',tbl_user.idUser AS 'idReportingTo', " +
                               " tbl_user.userDisplayName AS 'reportingTo',reportingtype.idReportingType, reportingtype.reportingTypeName,remark,  " +
                               " userreportingdetails.idUserReportingDetails,userreportingdetails.orgStructureId,userreportingdetails.isActive " +
                               " FROM tblUserReportingDetails userreportingdetails  INNER JOIN tblOrgStructure orgstructure " +
                               " ON userreportingdetails.orgStructureId = orgstructure.idOrgStructure  INNER JOIN tbluser tbluser " +
                               " ON tbluser.idUser = userreportingdetails.userId  LEFT JOIN tblUser tbl_user ON tbl_user.idUser = userreportingdetails.reportingTo " +
                               " INNER JOIN dimReportingType reportingtype ON reportingtype.idReportingType = userreportingdetails.reportingTypeId " +
                               " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1 and orgstructure.idOrgStructure ="+ orgStructureId;
                }
                

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblUserReportingDetailsTO> userReportingDetailsTOList = new List<TblUserReportingDetailsTO>();
                if (userDT != null)
                {
                    while (userDT.Read())
                    {
                        TblUserReportingDetailsTO userReportingDetailsTONew = new TblUserReportingDetailsTO();
                        if (userDT["idUser"] != DBNull.Value)
                            userReportingDetailsTONew.UserId = Convert.ToInt32(userDT["idUser"].ToString());
                        if (userDT["userName"] != DBNull.Value)
                            userReportingDetailsTONew.UserName = userDT["userName"].ToString();
                        if (userDT["idReportingTo"] != DBNull.Value)
                            userReportingDetailsTONew.ReportingTo = Convert.ToInt32(userDT["idReportingTo"].ToString());
                        if (userDT["reportingTo"] != DBNull.Value)
                            userReportingDetailsTONew.ReportingToName = userDT["reportingTo"].ToString();
                        if (userDT["idReportingType"] != DBNull.Value)
                            userReportingDetailsTONew.ReportingTypeId = Convert.ToInt32(userDT["idReportingType"].ToString());
                        if (userDT["reportingTypeName"] != DBNull.Value)
                            userReportingDetailsTONew.ReportingType = userDT["reportingTypeName"].ToString();
                        if (userDT["remark"] != DBNull.Value)
                            userReportingDetailsTONew.Remark = userDT["remark"].ToString();
                        if (userDT["idUserReportingDetails"] != DBNull.Value)
                            userReportingDetailsTONew.IdUserReportingDetails = Convert.ToInt32(userDT["idUserReportingDetails"].ToString());
                        if (userDT["orgStructureId"] != DBNull.Value)
                            userReportingDetailsTONew.OrgStructureId = Convert.ToInt32(userDT["orgStructureId"].ToString());
                        if (userDT["isActive"] != DBNull.Value)
                            userReportingDetailsTONew.IsActive = Convert.ToInt16(userDT["isActive"].ToString());

                        userReportingDetailsTOList.Add(userReportingDetailsTONew);
                    }
                }
                return userReportingDetailsTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectOrgStructureUserDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public  String SelectAllOrgStructureIdList(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                String sqlQuery = " SELECT idOrgStructure FROM tblOrgStructure WHERE idOrgStructure= " + tblOrgStructureTO.IdOrgStructure +
                                  " OR parentOrgStructureId in ( " + tblOrgStructureTO.IdOrgStructure + " )";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;



                SqlDataReader orgStructureIdDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                String orgStructureIdList = "";
                if (orgStructureIdDT != null)
                {
                    while (orgStructureIdDT.Read())
                    {
                        orgStructureIdList += orgStructureIdDT["idOrgStructure"] + ",";
                    }
                }
                orgStructureIdDT.Dispose();
                return orgStructureIdList.Trim(',');
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllOrgStructureIdList");
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public  List<DropDownTO> SelectReportingToUserList(Int32 orgStructureId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                String sqlQUery;
                if (orgStructureId == 1)
                {
                    sqlQUery = " SELECT idUser,userDisplayName FROM tblUser  tbluser  " +
                               " WHERE tbluser.isActive=1 ";
                }
                else
                {
                     sqlQUery = " SELECT idUser,userDisplayName FROM tblUser  tbluser INNER JOIN  " +
                                     " tblUserReportingDetails tbluserreportingdetail ON tbluser.idUser = tbluserreportingdetail.userId " +
                                     " INNER JOIN  tblOrgStructure tblorgstr ON tblorgstr.parentOrgStructureId = tbluserreportingdetail.orgStructureId " +
                                     " WHERE tbluserreportingdetail.isActive=1 AND tbluser.isActive=1 AND tblorgstr.idOrgStructure = " + orgStructureId;
                }

                cmdSelect.CommandText = sqlQUery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (departmentTODT["idUser"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idUser"].ToString());
                        if (departmentTODT["userDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["userDisplayName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingToUserList");
                return null;
            }
        }

        #endregion

        #region Insertion
        public  int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgStructureTO, cmdInsert);
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

        public  int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgStructureTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblOrgStructure");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgStructure]( " +
            " [parentOrgStructureId]" +
            " ,[deptId]" +
            " ,[designationid]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[orgStructureDesc]" +
            " )" +
            " VALUES (" +
            " @ParentOrgStructureId " +
            " ,@DeptId " +
            " ,@Designationid " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@OrgStructureDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@ParentOrgStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.ParentOrgStructureId);
            cmdInsert.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DeptId;
            cmdInsert.Parameters.Add("@Designationid", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DesignationId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblOrgStructureTO.IsActive;
            cmdInsert.Parameters.Add("@OrgStructureDesc", System.Data.SqlDbType.NVarChar).Value = tblOrgStructureTO.OrgStructureDesc;
            return cmdInsert.ExecuteNonQuery();
        }


        // Vaibhav [27-Sep-2017] added to attach new employee to specific organization structure
        public  int InsertTblUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserReportingDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblUserReportingDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserReportingDetails]( " +
            "  [isActive]" +
            " ,[userId]" +
            " ,[reportingTo]" +
            " ,[reportingTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[deActivatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[deActivatedOn]" +
            " ,[remark]" +
            " ,[orgStructureId]" +
            " )" +
            " VALUES (" +
            "  @IsActive " +
            " ,@UserId " +
            " ,@ReportingTo " +
            " ,@ReportingTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@DeActivatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@DeActivatedOn " +
            " ,@Remark " +
            " ,@OrgStructureId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.SmallInt).Value = tblUserReportingDetailsTO.IsActive;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.UserId;
            cmdInsert.Parameters.Add("@ReportingTo", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTo;
            cmdInsert.Parameters.Add("@ReportingTypeId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@DeActivatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserReportingDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@DeActivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedOn);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.Remark);
            cmdInsert.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.NVarChar).Value = tblUserReportingDetailsTO.OrgStructureId;
            return cmdInsert.ExecuteNonQuery();
        }

        #endregion

        #region Updation
        public  int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgStructureTO, cmdUpdate);
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

        public  int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgStructureTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgStructure] SET " +
            "  [deptId]= @DeptId" +
            " ,[designationid]= @Designationid" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[orgStructureDesc] = @OrgStructureDesc" +
            " WHERE [idOrgStructure] = @IdOrgStructure";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            cmdUpdate.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DeptId;
            cmdUpdate.Parameters.Add("@Designationid", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DesignationId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgStructureTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblOrgStructureTO.IsActive;
            cmdUpdate.Parameters.Add("@OrgStructureDesc", System.Data.SqlDbType.NVarChar).Value = tblOrgStructureTO.OrgStructureDesc;
            return cmdUpdate.ExecuteNonQuery();
        }


        // Vaibhav [27-Sep-2017] added to deactivate specific organization structure
        public  int DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteDeactivateOrgSTructureCommand(tblOrgStructureTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateOrgStructure");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteDeactivateOrgSTructureCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgStructure] SET " +
            " [isActive]= @IsActive" +
            " WHERE idOrgStructure = @IdOrgStructure OR parentOrgStructureId = @IdOrgStructure";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblOrgStructureTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }

        // Vaibhav [4-Oct-2017] added to deactivate specific organization structure employees
        public  int DeactivateOrgStructureEmployees(String orgStructureIdList, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteDeactivateOrgSTructureEmployeesCommand(orgStructureIdList, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateOrgStructureEmployees");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteDeactivateOrgSTructureEmployeesCommand(String orgStructureIdList, SqlCommand cmdUpdate)
        {
            String sqlQuery = " UPDATE tblUserReportingDetails SET isActive = 0 WHERE orgStructureId IN(" + orgStructureIdList + " )";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            return cmdUpdate.ExecuteNonQuery();
        }

        // Vaibhav [29-Sep-2017] added to update user reporting details
        public  int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateUserReportingDetailsCommand(tblUserReportingDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactivateUserForOrgStructure");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdateUserReportingDetailsCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = " UPDATE tblUserReportingDetails SET reportingTo= @reportingTo, reportingTypeId= @reportingTypeId, " +
                              " updatedBy=@updatedBy,updatedOn=@updatedOn,isActive=@isActive,deActivatedBy=@deactivatedBy,deActivatedOn=@deactivatedOn" +
                              " WHERE orgStructureId = @orgStructureId AND userId = @userId ANd reportingTypeId = @reportingTypeId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@orgStructureId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.OrgStructureId;
            cmdUpdate.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.UserId;
            cmdUpdate.Parameters.Add("@reportingTypeId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTypeId;
            cmdUpdate.Parameters.Add("@reportingTo", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTo;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = tblUserReportingDetailsTO.IsActive;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@deactivatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedBy);
            cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedOn);

            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

        #region Deletion
        public  int DeleteTblOrgStructure(Int32 idOrgStructure)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgStructure, cmdDelete);
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

        public  int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgStructure, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idOrgStructure, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgStructure] " +
            " WHERE idOrgStructure = " + idOrgStructure + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
