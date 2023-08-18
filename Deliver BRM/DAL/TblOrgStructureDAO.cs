using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblOrgStructureDAO
    {

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT role.idRole,role.roleTypeId,Levels.shortName,Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                            " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn," +
                            " OrgStructure.levelId,OrgStructure.isNewAdded FROM [tblOrgStructure] OrgStructure LEFT JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                            " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                            " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                            " LEFT JOIN tblRole role ON OrgStructure.idOrgStructure=role.orgStructureId ";

            // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType "+
            // " WHERE OrgStructure.isActive = 1";
            // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType ";
            return sqlSelectQry;
        }
        //Hrushikesh
        public static String SqlSelectQueryForChild()
        {
            String sqlSelectQry = "SELECT role.idRole,role.roleTypeId,Levels.shortName,Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                            " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,roleType.reportingTypeName,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn," +
                            " OrgStructure.levelId,OrgStructure.isNewAdded FROM [tblOrgStructure] OrgStructure LEFT JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                            " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                            " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                            " LEFT JOIN tblRole role ON OrgStructure.idOrgStructure=role.orgStructureId " +
                            " LEFT JOIN tblOrgStructureHierarchy Hierarchy ON Hierarchy.orgStructureId=OrgStructure.idOrgStructure" +
                            " LEFT JOIN dimReportingType roleType ON roleType.idReportingType = Hierarchy.reportingTypeId";
            // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType "+
            // " WHERE OrgStructure.isActive = 1";
            // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType ";
            return sqlSelectQry;
        }

        public static String SqlSelectQueryUserReportingBom()

        {
            String sqlSelectQry = " SELECT DISTINCT tbluser.idUser,tbluser.userDisplayName AS 'userName',tbl_user.idUser AS 'idReportingTo', " +
                          " tbl_user.userDisplayName AS 'reportingTo',userreportingdetails.remark, " +
                          " userreportingdetails.idUserReportingDetails,userreportingdetails.orgStructureId,userreportingdetails.isActive," +
                          " userreportingdetails.levelId,MstDesignation.designationDesc +'-'+MstDept.deptDisplayName AS positionName, " +
                          "userreportingdetails.createdBy,userreportingdetails.updatedBy,userreportingdetails.reportingTypeId,userreportingdetails.reportingTypeId,userreportingdetails.createdOn,userreportingdetails.updatedOn" +
                          " FROM tblUserReportingDetails userreportingdetails  INNER JOIN tblOrgStructure orgstructure " +
                          " ON userreportingdetails.orgStructureId = orgstructure.idOrgStructure  INNER JOIN tbluser tbluser " +
                          " ON tbluser.idUser = userreportingdetails.userId  " +
                          " LEFT JOIN dimMstDept MstDept ON orgstructure.deptId=MstDept.idDept " +
                          " LEFT JOIN dimMstDesignation MstDesignation ON MstDesignation.idDesignation = orgstructure.designationId " +
                          " LEFT JOIN tblUser tbl_user ON tbl_user.idUser = userreportingdetails.reportingTo ";

            return sqlSelectQry;
        }


        public static String SqlSelectQueryUserReporting()
        {
            String sqlSelectQry = " SELECT DISTINCT tbluser.idUser,tbluser.userDisplayName AS 'userName',tbl_user.idUser AS 'idReportingTo', " +
                           " tbl_user.userDisplayName AS 'reportingTo',userreportingdetails.remark, repType.reportingTypeName," +
                           " userreportingdetails.idUserReportingDetails,userreportingdetails.orgStructureId,userreportingdetails.isActive," +
                           " userreportingdetails.levelId,MstDesignation.designationDesc +'-'+MstDept.deptDisplayName AS positionName, " +
                           " userreportingdetails.createdBy,userreportingdetails.updatedBy,userreportingdetails.reportingTypeId,userreportingdetails.createdOn,userreportingdetails.updatedOn " +
                           " FROM tblUserReportingDetails userreportingdetails  INNER JOIN tblOrgStructure orgstructure " +
                           " ON userreportingdetails.orgStructureId = orgstructure.idOrgStructure  INNER JOIN tbluser tbluser " +
                           " ON tbluser.idUser = userreportingdetails.userId  " +
                           " INNER JOIN dimMstDept MstDept ON orgstructure.deptId=MstDept.idDept " +
                           " INNER JOIN dimMstDesignation MstDesignation ON MstDesignation.idDesignation = orgstructure.designationId " +
                           " LEFT JOIN tblUser tbl_user ON tbl_user.idUser = userreportingdetails.reportingTo " +
                           " LEFT JOIN tblOrgStructureHierarchy Hierarchy ON userreportingdetails.orgStructureId = Hierarchy.orgStructureId " +
                           " LEFT JOIN dimReportingType repType ON repType.idReportingType =  userreportingdetails.reportingTypeId";


            return sqlSelectQry;
        }

        #endregion

        #region Selection

        public static List<TblOrgStructureTO> SelectAllOrgStructureHierarchy(SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            SqlDataReader userDT = null;
            try
            {
                string queryStr;
                queryStr = SqlSelectQuery() + " WHERE OrgStructure.isActive = 1";
                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblOrgStructureTO> orgStructureList = ConvertDTToList(userDT);
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
                if (userDT != null)
                    userDT.Dispose();
                cmdSelect.Dispose();

            }
        }

        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetailsForBom(int orgStructureId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (orgStructureId == 0)
                {
                    queryStr = SqlSelectQueryUserReportingBom() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1";
                }
                else
                {
                    queryStr = SqlSelectQueryUserReportingBom() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 " +
                                " AND userreportingdetails.isActive = 1   and orgstructure.idOrgStructure =" + orgStructureId;
                }


                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetailsForBom(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

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

        public static TblUserReportingDetailsTO SelectUserReportingDetailsTOBom(int idUserReportingDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            SqlDataReader userDT = null;
            try
            {
                string queryStr;
                queryStr = SqlSelectQueryUserReportingBom() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1  AND userreportingdetails.isActive = 1 AND userreportingdetails.orgStructureId=1 AND userreportingdetails.idUserReportingDetails=" + idUserReportingDetails;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetailsForBom(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList[0];
                else
                    return new TblUserReportingDetailsTO();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (userDT != null)
                    userDT.Dispose();
                cmdSelect.Dispose();

            }
        }



        public static List<TblOrgStructureTO> SelectAllOrgStructureHierarchy()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string queryStr;
                queryStr = SqlSelectQuery() + " WHERE OrgStructure.isActive = 1";
                // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType "+


                //queryStr = "SELECT Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                //            " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn" +
                //            " FROM [tblOrgStructure] OrgStructure INNER JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                //            " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                //            " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                //            " WHERE OrgStructure.isActive = 1";
                conn.Open();
                cmdSelect.CommandText = queryStr;
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

        public static List<TblOrgStructureTO> SelectAllOrgStructureHierarchy(int reportingTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string queryStr;
                queryStr = SqlSelectQuery() + " INNER JOIN tblOrgStructureHierarchy Hierarchy ON OrgStructure.idOrgStructure = Hierarchy.orgStructureId " +
                            // " LEFT JOIN dimReportingType ReportingType ON OrgStructure.reportingTypeId = ReportingType.idReportingType "+
                            " WHERE OrgStructure.isActive = 1 AND Hierarchy.isActive=1 AND Hierarchy.reportingTypeId=" + reportingTypeId;

                //queryStr = "SELECT Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                //            " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn" +
                //            " FROM [tblOrgStructure] OrgStructure INNER JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                //            " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                //            " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                //            " WHERE OrgStructure.isActive = 1";
                conn.Open();
                cmdSelect.CommandText = queryStr;
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

        public static TblOrgStructureTO SelectTblOrgStructure(Int32 idOrgStructure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgStructure = " + idOrgStructure + " AND OrgStructure.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader orgStructureDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureTO> orgStructureList = ConvertDTToList(orgStructureDT);
                if (orgStructureList != null)
                    return orgStructureList[0];
                else
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

        public static TblOrgStructureTO SelectAllTblOrgStructure(SqlConnection conn, SqlTransaction tran)
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

        public static TblOrgStructureTO SelectBOMOrgStructure()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE OrgStructure.isActive = 1 AND  OrgStructure.idOrgStructure=" + 1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader orgStructureDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureTO> orgStructureList = ConvertDTToList(orgStructureDT);
                if (orgStructureList != null)
                    return orgStructureList[0];
                else
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

        public static List<TblUserReportingDetailsTO> SelectUserReportingListOnUserId(int userId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND Hierarchy.isActive=1  AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1 AND userreportingdetails.userId=" + userId;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return new List<TblUserReportingDetailsTO>();
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

        public static TblUserReportingDetailsTO SelectUserReportingDetailsTO(int idUserReportingDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 AND Hierarchy.isActive=1   AND userreportingdetails.isActive = 1 AND userreportingdetails.idUserReportingDetails=" + idUserReportingDetails;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList[0];
                else
                    return new TblUserReportingDetailsTO();
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
        public static TblUserReportingDetailsTO SelectUserReportingDetailsTO(int idUserReportingDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            SqlDataReader userDT = null;
            try
            {
                string queryStr;
                queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1  AND Hierarchy.isActive=1  AND userreportingdetails.isActive = 1 AND userreportingdetails.idUserReportingDetails=" + idUserReportingDetails;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList[0];
                else
                    return new TblUserReportingDetailsTO();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (userDT != null)
                    userDT.Dispose();
                cmdSelect.Dispose();
            }
        }


        public static TblOrgStructureHierarchyTO SelectTblOrgStructureHierarchyForDelink(int selfOrgStructureId, int parentOrgStrucureId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM [tblOrgStructureHierarchy] WHERE isActive=1 AND orgStructureId=" + selfOrgStructureId + " AND parentOrgStructId= " + parentOrgStrucureId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructHierarchyDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = ConvertDTToListOrgStructHierarchy(orgStructHierarchyDT);
                if (orgStructureHierarchyTOList != null)
                    return orgStructureHierarchyTOList[0];
                else
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

        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(string orgStructureId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (orgStructureId == String.Empty)
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND Hierarchy.isActive=1  AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1";
                }
                else
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 " +
                                " AND userreportingdetails.isActive = 1 AND Hierarchy.isActive=1 and orgstructure.idOrgStructure IN(" + orgStructureId + ")";
                }


                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

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



        public static List<TblUserReportingDetailsTO> SelectUserReportingOnuserIds(string userIds, int reportingTo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (userIds == String.Empty)
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1  AND Hierarchy.isActive=1 AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1";
                }
                else
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND Hierarchy.isActive=1  AND tbluser.isActive = 1 " +
                                " AND userreportingdetails.isActive = 1 and userreportingdetails.userId IN(" + userIds + ") AND userreportingdetails.reportingTo=" + reportingTo;
                }


                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

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


        #region HierarchyTable List Selection
        public static List<TblOrgStructureHierarchyTO> SelectAllTblOrgStructureHierarchy()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM [tblOrgStructureHierarchy] WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructHierarchyDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = ConvertDTToListOrgStructHierarchy(orgStructHierarchyDT);
                if (orgStructureHierarchyTOList != null)
                    return orgStructureHierarchyTOList;
                else
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


        public static TblOrgStructureHierarchyTO SelectAllTblOrgStructureHierarchy(int orgStrctureId, int parentOrgStrctureId, int reportingTypeId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader orgStructHierarchyDT = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = " SELECT * FROM [tblOrgStructureHierarchy] WHERE " +
                    "  reportingTypeId=" + reportingTypeId + " AND orgStructureId=" + orgStrctureId + " AND parentOrgStructId=" + parentOrgStrctureId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                orgStructHierarchyDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = ConvertDTToListOrgStructHierarchy(orgStructHierarchyDT);
                if (orgStructureHierarchyTOList != null && orgStructureHierarchyTOList.Count == 1)
                    return orgStructureHierarchyTOList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                orgStructHierarchyDT.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnReportingType(int reportingTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT* FROM[tblOrgStructureHierarchy] where isACtive=1 AND reportingTypeId=" + reportingTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructHierarchyDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = ConvertDTToListOrgStructHierarchy(orgStructHierarchyDT);
                if (orgStructureHierarchyTOList != null)
                    return orgStructureHierarchyTOList;
                else
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

        public static List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnOrgStructutreId(int orgStructutreId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT* FROM[tblOrgStructureHierarchy] where isActive=1 AND orgStructureId=" + orgStructutreId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructHierarchyDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = ConvertDTToListOrgStructHierarchy(orgStructHierarchyDT);
                if (orgStructureHierarchyTOList != null)
                    return orgStructureHierarchyTOList;
                else
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
        #endregion

        #region USerReportingDetailsList

        public static List<TblUserReportingDetailsTO> SelectAllUserReportingDetails()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;

                queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND Hierarchy.isActive=1  AND tbluser.isActive = 1 AND userreportingdetails.isActive = 1"; ;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

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

        public static List<TblUserReportingDetailsTO> SelectAllUserReportingDetails(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader userDT = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 AND Hierarchy.isActive=1   AND userreportingdetails.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                string queryStr;

                queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 AND Hierarchy.isActive=1   AND userreportingdetails.isActive = 1"; ;

                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectOrgStructureUserDetails");
                return null;
            }
            finally
            {
                //conn.Close();
                if (userDT != null)
                    userDT.Dispose();
                cmdSelect.Dispose();
            }
        }
        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (orgStructureId == 0)
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND Hierarchy.isActive=1  AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1";
                }
                else
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND  Hierarchy.isActive=1  AND tbluser.isActive = 1 " +
                                " AND userreportingdetails.isActive = 1 and orgstructure.idOrgStructure =" + orgStructureId;
                }


                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;

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

        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(int orgStructureId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string queryStr;
                if (orgStructureId == 0)
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1    AND userreportingdetails.isActive = 1";
                }
                else
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 " +
                                " AND userreportingdetails.isActive = 1 and orgstructure.idOrgStructure =" + orgStructureId;
                }
                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                {
                    userDT.Close();
                    return userReportingDetailsTOList;
                }
                else
                {
                    userDT.Close();
                    return null;
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectOrgStructureUserDetails");
                return null;
            }
            finally
            {
                //   conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Sudhir
        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetailsByReportingType(int orgStructureId, int reportingTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                string queryStr;
                if (orgStructureId == 0)
                {
                    //Hrushikesh  added to get users with reporting type on userReporting
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1 AND tbluser.isActive = 1 AND userreportingdetails.isActive=1  AND userreportingdetails.reportingTypeId = " + reportingTypeId;
                    //" WHERE orgstructure.isActive = 1 AND tbluser.isActive = 1  AND userreportingdetails.isActive = 1 ";//AND userreportingdetails.reportingTypeId="+ reportingTypeId;
                }
                else
                {
                    queryStr = SqlSelectQueryUserReporting() + " WHERE orgstructure.isActive = 1   AND tbluser.isActive = 1 " +
                        " AND userreportingdetails.isActive = 1 AND userreportingdetails.isActive=1 AND orgstructure.idOrgStructure =" + orgStructureId + " AND userreportingdetails.reportingTypeId = " + reportingTypeId; ;
                }


                cmdSelect.CommandText = queryStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader userDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserReportingDetailsTO> userReportingDetailsTOList = ConvertDTToListUserReportingDetails(userDT);
                if (userReportingDetailsTOList != null)
                    return userReportingDetailsTOList;
                else
                    return null;
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
        #endregion

        public static String SelectAllOrgStructureIdList(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
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

        public static List<DropDownTO> SelectReportingToUserList(string orgStructureId, int repTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                String sqlQUery;
                if (false)
                {
                    sqlQUery = " SELECT idUser,userDisplayName FROM tblUser  tbluser  " +
                               " WHERE tbluser.isActive=1 ";
                }
                else
                {
                    sqlQUery = "SELECT DISTINCT idUser, userDisplayName FROM tblUser  tbluser INNER JOIN tblUserReportingDetails tbluserreportingdetail" +
                                " ON tbluser.idUser = tbluserreportingdetail.userId " +
                                " LEFT JOIN tblOrgStructureHierarchy hierarchy ON hierarchy.orgStructureId = tbluserreportingdetail.orgStructureId " +
                                 " WHERE tbluserreportingdetail.isActive = 1 AND tbluser.isActive = 1 AND hierarchy.isActive=1 AND hierarchy.orgStructureId IN (" + orgStructureId + ")";



                    //sqlQUery = " SELECT idUser,userDisplayName FROM tblUser  tbluser INNER JOIN  " +
                    //                " tblUserReportingDetails tbluserreportingdetail ON tbluser.idUser = tbluserreportingdetail.userId " +
                    //                " INNER JOIN  tblOrgStructure tblorgstr ON tblorgstr.parentOrgStructureId = tbluserreportingdetail.orgStructureId " +
                    //                " WHERE tbluserreportingdetail.isActive=1 AND tbluser.isActive=1 AND tblorgstr.idOrgStructure = " + orgStructureId;
                }

                if (repTypeId > 0)
                {
                    sqlQUery = sqlQUery + "AND tbluserreportingdetail.reportingTypeId =" + repTypeId;
                }
                else
                {
                }

                //{
                //    sqlQUery = "SELECT idUser, userDisplayName FROM tblUser WHERE idUser IN(select DISTINCT userId from tblUserReportingDetails where orgStructureId in(" +
                //           " SELECT idOrgStructure FROM tblOrgStructure WHERE parentOrgStructureId IN(" +
                //            "( SELECT parentOrgStructureId from tblOrgStructure WHERE idOrgStructure = (SELECT parentOrgStructureId FROM tblOrgStructure WHERE idOrgStructure =" + orgStructureId + "))) AND isActive = 1" +
                //             ") AND tblUserReportingDetails.isactive = 1)";
                //}
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

        public static List<DimLevelsTO> SelectAllDimLevels()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM [dimLevels]";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader allLevelsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimLevelsTO> levelsList = ConvertDTToListLevels(allLevelsDT);
                if (levelsList != null)
                    return levelsList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimLevels");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgStructureTO> SelectChildPositionList(string deptIds, int idOrgStructure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = "SELECT role.idRole,Levels.shortName,Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                //            " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn," +
                //            " OrgStructure.levelId,OrgStructure.isNewAdded FROM [tblOrgStructure] OrgStructure LEFT JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                //            " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                //            " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                //            " LEFT JOIN tblRole role ON OrgStructure.idOrgStructure = role.orgStructureId " +
                //            " Where OrgStructure.deptId IN (" + deptIds + ") AND OrgStructure.idOrgStructure !=" + idOrgStructure + " AND OrgStructure.isActive=1";


                cmdSelect.CommandText = "SELECT role.idRole,role.roleTypeId,Levels.shortName,Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                           " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn," +
                           " OrgStructure.levelId,OrgStructure.isNewAdded FROM [tblOrgStructure] OrgStructure LEFT JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation" +
                           " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                           " LEFT JOIN dimLevels Levels ON OrgStructure.levelId=Levels.idLevel" +
                           " LEFT JOIN tblRole role ON OrgStructure.idOrgStructure = role.orgStructureId " +
                           " Where (OrgStructure.deptId IN (" + deptIds + ")   OR OrgStructure.deptId IN (SELECT idDept from dimMstDept WHERE  " +
                           " deptTypeId IN (SELECT deptTypeId FROM dimMstDept WHERE idDept IN (" + deptIds + "))))  " +
                           " AND OrgStructure.idOrgStructure !=" + idOrgStructure + " AND OrgStructure.isActive=1";

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
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgStructureTO> SelectAllNotLinkedPositionsList()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT Levels.shortName,role.roleTypeId,Designation.designationDesc,Dept.deptDisplayName,OrgStructure.idOrgStructure,OrgStructure.designationId,OrgStructure.deptId," +
                             " OrgStructure.parentOrgStructureId,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.createdBy,OrgStructure.createdOn,OrgStructure.updatedBy,OrgStructure.updatedOn" +
                             " ,OrgStructure.levelId,OrgStructure.isNewAdded,role.idRole FROM[tblOrgStructure] OrgStructure INNER JOIN dimMstDesignation Designation ON OrgStructure.designationId = Designation.idDesignation " +
                             " INNER JOIN dimMstDept Dept ON OrgStructure.deptId = Dept.idDept " +
                             " LEFT JOIN dimLevels Levels ON OrgStructure.levelId = Levels.idLevel " +
                             " LEFT JOIN tblRole role ON role.orgStructureId = OrgStructure.idOrgStructure " +
                             " WHERE OrgStructure.idOrgStructure NOT IN  (SELECT orgStructureId FROM tblOrgStructureHierarchy WHERE isActive=1) AND (parentOrgStructureId IS NULL OR parentOrgStructureId=0) ";
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
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblOrgStructureTO> SelectPositionLinkDetails(int idOrgStructure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT OrgStructureHierarchy.reportingTypeId,ReportingType.reportingTypeName,OrgStructureHierarchy.parentOrgStructId,OrgStructure.idOrgStructure,OrgStructure.orgStructureDesc,OrgStructure.isActive,OrgStructure.updatedOn " +
                    " FROM tblOrgStructureHierarchy OrgStructureHierarchy LEFT JOIN tblOrgStructure OrgStructure ON OrgStructure.idOrgStructure = OrgStructureHierarchy.orgStructureId " +
                    " LEFT JOIN dimReportingType ReportingType ON OrgStructureHierarchy.reportingTypeId = ReportingType.idReportingType WHERE OrgStructureHierarchy.orgStructureId = " + idOrgStructure + " AND OrgStructureHierarchy.isActive=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructureDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureTO> orgStructureList = new List<TblOrgStructureTO>();//ConvertDTToList(orgStructureDT);
                while (orgStructureDT.Read())
                {
                    TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
                    if (orgStructureDT["reportingTypeName"] != DBNull.Value)
                        orgStructureTO.ReportingName = Convert.ToString(orgStructureDT["reportingTypeName"].ToString());
                    if (orgStructureDT["idOrgStructure"] != DBNull.Value)
                        orgStructureTO.IdOrgStructure = Convert.ToInt32(orgStructureDT["idOrgStructure"].ToString());
                    if (orgStructureDT["orgStructureDesc"] != DBNull.Value)
                        orgStructureTO.OrgStructureDesc = Convert.ToString(orgStructureDT["orgStructureDesc"].ToString());
                    if (orgStructureDT["parentOrgStructId"] != DBNull.Value)
                        orgStructureTO.ParentOrgStructureId = Convert.ToInt32(orgStructureDT["parentOrgStructId"].ToString());
                    if (orgStructureDT["reportingTypeId"] != DBNull.Value)
                        orgStructureTO.ReportingTypeId = Convert.ToInt32(orgStructureDT["reportingTypeId"].ToString());

                    orgStructureList.Add(orgStructureTO);
                }

                return orgStructureList;
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

        public static List<TblOrgStructureTO> SelectChildPositionsDetails(int idOrgStructure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForChild() +
                    " WHERE Hierarchy.parentOrgStructId = " + idOrgStructure + "AND  Hierarchy.isActive=1 AND OrgStructure.isActive=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader orgStructureDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgStructureTO> orgStructureList = ConvertDTToListForChild(orgStructureDT);
                return orgStructureList;
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


        #endregion

        #region Insertion
        public static int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
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

        public static int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommandForOrgStructureHierarchy(tblOrgStructureHierarchyTO, cmdInsert);
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

        public static int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForOrgStructureHierarchy(tblOrgStructureHierarchyTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblOrgStructureHierarchy");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }


        public static int ExecuteInsertionCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdInsert)
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
            " ,[levelId]" +
            " ,[isNewAdded]" +
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
            " ,@LevelId" +
            " ,@IsNewAdded" +
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
            cmdInsert.Parameters.Add("@LevelId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.LevelId);
            cmdInsert.Parameters.Add("@IsNewAdded", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IsNewAdded;
            return cmdInsert.ExecuteNonQuery();
        }

        public static int ExecuteInsertionCommandForOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgStructureHierarchy]( " +
            //"  [idOrgHierarchy]" +
            " [orgStructureId]" +
            " ,[parentOrgStructId]" +
            " ,[reportingTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive] " +
            " )" +
            " VALUES (" +
            //"  @IdOrgHierarchy " +
            " @OrgStructureId " +
            " ,@ParentOrgStructId " +
            " ,@ReportingTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOrgHierarchy", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.IdOrgHierarchy;
            cmdInsert.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.OrgStructureId;
            cmdInsert.Parameters.Add("@ParentOrgStructId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.ParentOrgStructId;
            cmdInsert.Parameters.Add("@ReportingTypeId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.ReportingTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureHierarchyTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgStructureHierarchyTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureHierarchyTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureHierarchyTO.IsActive);

            return cmdInsert.ExecuteNonQuery();
        }

        // Vaibhav [27-Sep-2017] added to attach new employee to specific organization structure
        public static int InsertTblUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                if (tblUserReportingDetailsTO.ReportingTypeId == 0)
                    tblUserReportingDetailsTO.ReportingTypeId = (int)Constants.ReportingTypeE.ADMINISTRATIVE;
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

        public static int ExecuteInsertionCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserReportingDetails]( " +
            "  [isActive]" +
            " ,[userId]" +
            " ,[reportingTo]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[deActivatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[deActivatedOn]" +
            " ,[remark]" +
            " ,[orgStructureId]" +
            " ,[levelId]" +
             " ,[ReportingTypeId]" +
            " )" +
            " VALUES (" +
            "  @IsActive " +
            " ,@UserId " +
            " ,@ReportingTo " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@DeActivatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@DeActivatedOn " +
            " ,@Remark " +
            " ,@OrgStructureId" +
             ",@LevelId" +
             ",@ReportingType" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.SmallInt).Value = tblUserReportingDetailsTO.IsActive;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.UserId;
            cmdInsert.Parameters.Add("@ReportingTo", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTo;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@DeActivatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserReportingDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@DeActivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedOn);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.Remark);
            cmdInsert.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.OrgStructureId);
            cmdInsert.Parameters.Add("@LevelId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.LevelId);
            cmdInsert.Parameters.Add("@ReportingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.ReportingTypeId);
            return cmdInsert.ExecuteNonQuery();
        }

        #endregion

        #region Updation
        public static int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
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

        public static int UpdateChildTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteChildUpdationCommand(tblOrgStructureTO, cmdUpdate);
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

        //public static int ExecuteUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
        //{
        //    String sqlQuery = @" UPDATE [tblOrgStructure] SET " +
        //    "  [deptId]= @DeptId" +
        //    " ,[designationid]= @Designationid" +
        //    " ,[updatedBy]= @UpdatedBy" +
        //    " ,[updatedOn]= @UpdatedOn" +
        //    " ,[isActive]= @IsActive" +
        //    " ,[orgStructureDesc] = @OrgStructureDesc" +
        //    " ,[isNewAdded]=@IsNewAdded" +
        //    " WHERE [idOrgStructure] = @IdOrgStructure";

        //    cmdUpdate.CommandText = sqlQuery;
        //    cmdUpdate.CommandType = System.Data.CommandType.Text;

        //    cmdUpdate.Parameters.Add("@IdOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
        //    cmdUpdate.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DeptId;
        //    cmdUpdate.Parameters.Add("@Designationid", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DesignationId;
        //    cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.UpdatedBy;
        //    cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgStructureTO.UpdatedOn;
        //    cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblOrgStructureTO.IsActive;
        //    cmdUpdate.Parameters.Add("@OrgStructureDesc", System.Data.SqlDbType.NVarChar).Value = tblOrgStructureTO.OrgStructureDesc;
        //    cmdUpdate.Parameters.Add("@IsNewAdded", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IsNewAdded;
        //    return cmdUpdate.ExecuteNonQuery();
        //}

        public static int ExecuteUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgStructure] SET " +
            "  [parentOrgStructureId]= @ParentOrgStructureId" +
            " ,[deptId]= @DeptId" +
            " ,[designationId]= @DesignationId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[levelId]= @LevelId" +
            " ,[isNewAdded]= @IsNewAdded" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[orgStructureDesc] = @OrgStructureDesc" +
            " WHERE idOrgStructure = @IdOrgStructure ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            cmdUpdate.Parameters.Add("@ParentOrgStructureId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.ParentOrgStructureId;
            cmdUpdate.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DeptId;
            cmdUpdate.Parameters.Add("@DesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.DesignationId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IsActive;
            cmdUpdate.Parameters.Add("@LevelId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.LevelId);
            cmdUpdate.Parameters.Add("@IsNewAdded", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IsNewAdded;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@OrgStructureDesc", System.Data.SqlDbType.NVarChar).Value = tblOrgStructureTO.OrgStructureDesc;
            return cmdUpdate.ExecuteNonQuery();
        }

        public static int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommandForOrgHierarchy(tblOrgStructureHierarchyTO, cmdUpdate);
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

        public static int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForOrgHierarchy(tblOrgStructureHierarchyTO, cmdUpdate);
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

        public static int ExecuteUpdationCommandForOrgHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgStructureHierarchy] SET " +
            " [orgStructureId]= @OrgStructureId" +
            " ,[parentOrgStructId]= @ParentOrgStructId" +
            " ,[reportingTypeId]= @ReportingTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[isActive]=@IsActive" +
            " WHERE idOrgHierarchy = @IdOrgHierarchy ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgHierarchy", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.IdOrgHierarchy;
            cmdUpdate.Parameters.Add("@OrgStructureId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.OrgStructureId;
            cmdUpdate.Parameters.Add("@ParentOrgStructId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.ParentOrgStructId;
            cmdUpdate.Parameters.Add("@ReportingTypeId", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.ReportingTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureHierarchyTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgStructureHierarchyTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgStructureHierarchyTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }

        public static int ExecuteChildUpdationCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgStructure] SET " +
            "  [deptId]= @DeptId" +
            " ,[designationid]= @Designationid" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            // " ,[parentOrgStructureId]=@ParentOrgStructureId" +
            " ,[orgStructureDesc] = @OrgStructureDesc" +
            " WHERE [idOrgStructure] = @IdOrgStructure";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            //cmdUpdate.Parameters.Add("@ParentOrgStructureId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.ParentOrgStructureId;
            cmdUpdate.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DeptId;
            cmdUpdate.Parameters.Add("@Designationid", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.DesignationId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgStructureTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblOrgStructureTO.IsActive;
            cmdUpdate.Parameters.Add("@OrgStructureDesc", System.Data.SqlDbType.NVarChar).Value = tblOrgStructureTO.OrgStructureDesc;
            return cmdUpdate.ExecuteNonQuery();
        }

        // Vaibhav [27-Sep-2017] added to deactivate specific organization structure
        public static int DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteDeactivateOrgSTructureCommand(TblOrgStructureTO tblOrgStructureTO, SqlCommand cmdUpdate)
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
        public static int DeactivateOrgStructureEmployees(String orgStructureIdList, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteDeactivateOrgSTructureEmployeesCommand(String orgStructureIdList, SqlCommand cmdUpdate)
        {
            String sqlQuery = " UPDATE tblUserReportingDetails SET isActive = 0 WHERE orgStructureId IN(" + orgStructureIdList + " )";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            return cmdUpdate.ExecuteNonQuery();
        }

        public static int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdateUserReportingDetailsCommand(tblUserReportingDetailsTO, cmdUpdate);
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

        // Vaibhav [29-Sep-2017] added to update user reporting details
        public static int UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteUpdateUserReportingDetailsCommand(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = " UPDATE tblUserReportingDetails SET orgStructureId = @orgStructureId,userId = @userId,reportingTo = @reportingTo, " +
                              " updatedBy=@updatedBy,updatedOn=@updatedOn,ReportingTypeId=@ReportingType,isActive=@isActive,deActivatedBy=@deactivatedBy,deActivatedOn=@deactivatedOn" +
                              " WHERE idUserReportingDetails=@IdUserReportingDetails";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserReportingDetails", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.IdUserReportingDetails;
            cmdUpdate.Parameters.Add("@orgStructureId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.OrgStructureId;
            cmdUpdate.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.UserId;
            cmdUpdate.Parameters.Add("@reportingTo", System.Data.SqlDbType.Int).Value = tblUserReportingDetailsTO.ReportingTo;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = tblUserReportingDetailsTO.IsActive;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@deactivatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedBy);
            cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.DeActivatedOn);
            cmdUpdate.Parameters.Add("@ReportingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserReportingDetailsTO.ReportingTypeId);

            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

        #region Deletion
        public static int DeleteTblOrgStructure(Int32 idOrgStructure)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran)
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

        public static int ExecuteDeletionCommand(Int32 idOrgStructure, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgStructure] " +
            " WHERE idOrgStructure = " + idOrgStructure + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgStructure", System.Data.SqlDbType.Int).Value = tblOrgStructureTO.IdOrgStructure;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        #region Convert DT to List
        public static List<TblUserReportingDetailsTO> ConvertDTToListUserReportingDetails(SqlDataReader userDT)
        {
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
                    //if (userDT["idReportingType"] != DBNull.Value)
                    //    userReportingDetailsTONew.ReportingTypeId = Convert.ToInt32(userDT["idReportingType"].ToString());
                    if (userDT["positionName"] != DBNull.Value)
                        userReportingDetailsTONew.PositionName = userDT["positionName"].ToString();
                    if (userDT["remark"] != DBNull.Value)
                        userReportingDetailsTONew.Remark = userDT["remark"].ToString();
                    if (userDT["idUserReportingDetails"] != DBNull.Value)
                        userReportingDetailsTONew.IdUserReportingDetails = Convert.ToInt32(userDT["idUserReportingDetails"].ToString());
                    if (userDT["orgStructureId"] != DBNull.Value)
                        userReportingDetailsTONew.OrgStructureId = Convert.ToInt32(userDT["orgStructureId"].ToString());
                    if (userDT["isActive"] != DBNull.Value)
                        userReportingDetailsTONew.IsActive = Convert.ToInt16(userDT["isActive"].ToString());
                    if (userDT["levelId"] != DBNull.Value)
                        userReportingDetailsTONew.LevelId = Convert.ToInt32(userDT["levelId"].ToString());
                    if (userDT["createdBy"] != DBNull.Value)
                        userReportingDetailsTONew.CreatedBy = Convert.ToInt32(userDT["createdBy"].ToString());
                    if (userDT["updatedBy"] != DBNull.Value)
                        userReportingDetailsTONew.UpdatedBy = Convert.ToInt32(userDT["updatedBy"].ToString());
                    if (userDT["createdOn"] != DBNull.Value)
                        userReportingDetailsTONew.CreatedOn = Convert.ToDateTime(userDT["createdOn"].ToString());
                    if (userDT["reportingTypeId"] != DBNull.Value)
                        userReportingDetailsTONew.ReportingTypeId = Convert.ToInt32(userDT["reportingTypeId"].ToString());
                    if (userDT["reportingTypeName"] != DBNull.Value)
                        userReportingDetailsTONew.ReportingType = Convert.ToString(userDT["reportingTypeName"].ToString());

                    //if (userDT["shortName"] != DBNull.Value)
                    //    userReportingDetailsTONew.LevelId = Convert.ToInt32(userDT["levelId"].ToString());
                    userReportingDetailsTOList.Add(userReportingDetailsTONew);
                }
            }
            return userReportingDetailsTOList;
        }


        public static List<TblUserReportingDetailsTO> ConvertDTToListUserReportingDetailsForBom(SqlDataReader userDT)
        {
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
                    //if (userDT["idReportingType"] != DBNull.Value)
                    //    userReportingDetailsTONew.ReportingTypeId = Convert.ToInt32(userDT["idReportingType"].ToString());
                    if (userDT["positionName"] != DBNull.Value)
                        userReportingDetailsTONew.PositionName = userDT["positionName"].ToString();
                    if (userDT["remark"] != DBNull.Value)
                        userReportingDetailsTONew.Remark = userDT["remark"].ToString();
                    if (userDT["idUserReportingDetails"] != DBNull.Value)
                        userReportingDetailsTONew.IdUserReportingDetails = Convert.ToInt32(userDT["idUserReportingDetails"].ToString());
                    if (userDT["orgStructureId"] != DBNull.Value)
                        userReportingDetailsTONew.OrgStructureId = Convert.ToInt32(userDT["orgStructureId"].ToString());
                    if (userDT["isActive"] != DBNull.Value)
                        userReportingDetailsTONew.IsActive = Convert.ToInt16(userDT["isActive"].ToString());
                    if (userDT["levelId"] != DBNull.Value)
                        userReportingDetailsTONew.LevelId = Convert.ToInt32(userDT["levelId"].ToString());
                    if (userDT["createdBy"] != DBNull.Value)
                        userReportingDetailsTONew.CreatedBy = Convert.ToInt32(userDT["createdBy"].ToString());
                    if (userDT["updatedBy"] != DBNull.Value)
                        userReportingDetailsTONew.UpdatedBy = Convert.ToInt32(userDT["updatedBy"].ToString());
                    if (userDT["createdOn"] != DBNull.Value)
                        userReportingDetailsTONew.CreatedOn = Convert.ToDateTime(userDT["createdOn"].ToString());
                    if (userDT["reportingTypeId"] != DBNull.Value)
                        userReportingDetailsTONew.ReportingTypeId = Convert.ToInt32(userDT["reportingTypeId"].ToString());

                    //if (userDT["shortName"] != DBNull.Value)
                    //    userReportingDetailsTONew.LevelId = Convert.ToInt32(userDT["levelId"].ToString());
                    userReportingDetailsTOList.Add(userReportingDetailsTONew);
                }
            }
            return userReportingDetailsTOList;
        }

        public static List<DimLevelsTO> ConvertDTToListLevels(SqlDataReader levelsDT)
        {
            List<DimLevelsTO> levelToList = new List<DimLevelsTO>();
            if (levelToList != null)
            {
                while (levelsDT.Read())
                {
                    DimLevelsTO levelTONew = new DimLevelsTO();

                    if (levelsDT["idLevel"] != DBNull.Value)
                        levelTONew.IdLevel = Convert.ToInt32(levelsDT["idLevel"].ToString());
                    if (levelsDT["levelName"] != DBNull.Value)
                        levelTONew.LevelName = levelsDT["levelName"].ToString();
                    if (levelsDT["levelDesc"] != DBNull.Value)
                        levelTONew.LevelDesc = levelsDT["levelDesc"].ToString();
                    if (levelsDT["shortName"] != DBNull.Value)
                        levelTONew.ShortName = levelsDT["shortName"].ToString();
                    levelToList.Add(levelTONew);
                }
            }
            return levelToList;
        }

        public static List<TblOrgStructureHierarchyTO> ConvertDTToListOrgStructHierarchy(SqlDataReader tblOrgStructureHierarchyTODT)
        {
            List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = new List<TblOrgStructureHierarchyTO>();
            if (orgStructureHierarchyTOList != null)
            {
                while (tblOrgStructureHierarchyTODT.Read())
                {
                    TblOrgStructureHierarchyTO tblOrgStructureHierarchyTONew = new TblOrgStructureHierarchyTO();
                    if (tblOrgStructureHierarchyTODT["idOrgHierarchy"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.IdOrgHierarchy = Convert.ToInt32(tblOrgStructureHierarchyTODT["idOrgHierarchy"].ToString());
                    if (tblOrgStructureHierarchyTODT["orgStructureId"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.OrgStructureId = Convert.ToInt32(tblOrgStructureHierarchyTODT["orgStructureId"].ToString());
                    if (tblOrgStructureHierarchyTODT["parentOrgStructId"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.ParentOrgStructId = Convert.ToInt32(tblOrgStructureHierarchyTODT["parentOrgStructId"].ToString());
                    if (tblOrgStructureHierarchyTODT["reportingTypeId"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.ReportingTypeId = Convert.ToInt32(tblOrgStructureHierarchyTODT["reportingTypeId"].ToString());
                    if (tblOrgStructureHierarchyTODT["createdBy"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.CreatedBy = Convert.ToInt32(tblOrgStructureHierarchyTODT["createdBy"].ToString());
                    if (tblOrgStructureHierarchyTODT["updatedBy"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.UpdatedBy = Convert.ToInt32(tblOrgStructureHierarchyTODT["updatedBy"].ToString());
                    if (tblOrgStructureHierarchyTODT["createdOn"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.CreatedOn = Convert.ToDateTime(tblOrgStructureHierarchyTODT["createdOn"].ToString());
                    if (tblOrgStructureHierarchyTODT["updatedOn"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.UpdatedOn = Convert.ToDateTime(tblOrgStructureHierarchyTODT["updatedOn"].ToString());
                    if (tblOrgStructureHierarchyTODT["isActive"] != DBNull.Value)
                        tblOrgStructureHierarchyTONew.IsActive = Convert.ToInt32(tblOrgStructureHierarchyTODT["isActive"].ToString());

                    orgStructureHierarchyTOList.Add(tblOrgStructureHierarchyTONew);


                }
            }
            return orgStructureHierarchyTOList;
        }


        public static List<TblOrgStructureTO> ConvertDTToListForChild(SqlDataReader orgStructureDT)
        {
            List<TblOrgStructureTO> orgStructureTOList = new List<TblOrgStructureTO>();
            if (orgStructureDT != null)
            {
                while (orgStructureDT.Read())
                {
                    TblOrgStructureTO orgStructureTONew = new TblOrgStructureTO();

                    if (orgStructureDT["designationDesc"] != DBNull.Value)
                        orgStructureTONew.DesignationName = orgStructureDT["designationDesc"].ToString();
                    if (orgStructureDT["deptDisplayName"] != DBNull.Value)
                        orgStructureTONew.DepartmentName = orgStructureDT["deptDisplayName"].ToString();
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
                    if (orgStructureDT["levelId"] != DBNull.Value)
                        orgStructureTONew.LevelId = Convert.ToInt16(orgStructureDT["levelId"]);
                    if (orgStructureDT["shortName"] != DBNull.Value)
                        orgStructureTONew.LevelName = orgStructureDT["shortName"].ToString();
                    if (orgStructureDT["isNewAdded"] != DBNull.Value)
                        orgStructureTONew.IsNewAdded = Convert.ToInt16(orgStructureDT["isNewAdded"]);
                    if (orgStructureDT["idRole"] != DBNull.Value)
                        orgStructureTONew.RoleId = Convert.ToInt16(orgStructureDT["idRole"]);
                    if (orgStructureDT["roleTypeId"] != DBNull.Value)
                        orgStructureTONew.RoleTypeId = Convert.ToInt32(orgStructureDT["roleTypeId"].ToString());
                    if (orgStructureDT["reportingTypeName"] != DBNull.Value)
                        orgStructureTONew.ReportingName = Convert.ToString(orgStructureDT["reportingTypeName"].ToString());

                    orgStructureTONew.PositionName = orgStructureTONew.DesignationName + "-" + orgStructureTONew.DepartmentName;
                    orgStructureTOList.Add(orgStructureTONew);
                }
            }
            return orgStructureTOList;
        }

        public static List<TblOrgStructureTO> ConvertDTToList(SqlDataReader orgStructureDT)
        {
            List<TblOrgStructureTO> orgStructureTOList = new List<TblOrgStructureTO>();
            if (orgStructureDT != null)
            {
                while (orgStructureDT.Read())
                {
                    TblOrgStructureTO orgStructureTONew = new TblOrgStructureTO();

                    if (orgStructureDT["designationDesc"] != DBNull.Value)
                        orgStructureTONew.DesignationName = orgStructureDT["designationDesc"].ToString();
                    if (orgStructureDT["deptDisplayName"] != DBNull.Value)
                        orgStructureTONew.DepartmentName = orgStructureDT["deptDisplayName"].ToString();
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
                    if (orgStructureDT["levelId"] != DBNull.Value)
                        orgStructureTONew.LevelId = Convert.ToInt16(orgStructureDT["levelId"]);
                    if (orgStructureDT["shortName"] != DBNull.Value)
                        orgStructureTONew.LevelName = orgStructureDT["shortName"].ToString();
                    if (orgStructureDT["isNewAdded"] != DBNull.Value)
                        orgStructureTONew.IsNewAdded = Convert.ToInt16(orgStructureDT["isNewAdded"]);
                    if (orgStructureDT["idRole"] != DBNull.Value)
                        orgStructureTONew.RoleId = Convert.ToInt16(orgStructureDT["idRole"]);
                    if (orgStructureDT["roleTypeId"] != DBNull.Value)
                        orgStructureTONew.RoleTypeId = Convert.ToInt32(orgStructureDT["roleTypeId"].ToString());
                    // if (orgStructureDT["reportingTypeName"] != DBNull.Value)
                    //   orgStructureTONew.ReportingName = Convert.ToString(orgStructureDT["reportingTypeName"].ToString());

                    orgStructureTONew.PositionName = orgStructureTONew.DesignationName + "-" + orgStructureTONew.DepartmentName;
                    orgStructureTOList.Add(orgStructureTONew);
                }
            }
            return orgStructureTOList;
        }
        #endregion
    }
}
