using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblUserRoleDAO : ITblUserRoleDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUserRoleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT userRole.*, role.roleDesc,role.enableAreaAlloc FROM [tblUserRole] userRole" +
                                  " LEFT JOIN tblRole role ON role.idRole=userRole.roleId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblUserRoleTO> SelectAllTblUserRole()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idUserRole", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IdUserRole;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserRoleTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblUserRoleTO> SelectAllActiveUserRole(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userId=" + userId + " AND userRole.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserRoleTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblUserRoleTO SelectTblUserRole(Int32 idUserRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUserRole = " + idUserRole + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserRoleTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  Int32 IsAreaAllocatedUser(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT ISNULL(tblRole.enableAreaAlloc,0)  AS enableAreaAlloc FROM tblUserRole " +
                                        " INNER JOIN tblRole ON idRole = roleId WHERE tblRole.isActive = 1 AND tblUserRole.isActive = 1" +
                                        " AND tblUserRole.userId=" + userId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Int32 isAreaEnable = 0;
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        TblUserRoleTO tblUserRoleTONew = new TblUserRoleTO();
                        if (sqlReader["enableAreaAlloc"] != DBNull.Value)
                        {
                            isAreaEnable = Convert.ToInt32(sqlReader["enableAreaAlloc"]);
                        }
                    }
                }

                return isAreaEnable;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                // String aqlQuery = " SELECT idUser,userDisplayName FROM tblUser " +
                //                   " INNER JOIN tblUserRole ON userId = idUser " +
                //                   " INNER JOIN tblUserExt UserExt ON UserExt.userId=tblUser.idUser " +
                //                   " WHERE tblUser.isActive=1 AND roleId=" + roleId;

                String aqlQuery = " SELECT idUser,userDisplayName FROM tblUser " +
                                                " INNER JOIN tblUserRole ON userId = idUser " +
                                                " INNER JOIN tblUserExt UserExt ON UserExt.userId=idUser " + //Sudhir[30-7-2018]  Added for Get PersonId
                                                " WHERE tblUser.isActive=1 AND tblUserRole.isActive=1 AND  roleId=" + roleId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<TblUserRoleTO> ConvertDTToList(SqlDataReader tblUserRoleTODT)
        {
            List<TblUserRoleTO> tblUserRoleTOList = new List<TblUserRoleTO>();
            if (tblUserRoleTODT != null)
            {
                while (tblUserRoleTODT.Read())
                {
                    TblUserRoleTO tblUserRoleTONew = new TblUserRoleTO();
                    if (tblUserRoleTODT["idUserRole"] != DBNull.Value)
                        tblUserRoleTONew.IdUserRole = Convert.ToInt32(tblUserRoleTODT["idUserRole"].ToString());
                    if (tblUserRoleTODT["userId"] != DBNull.Value)
                        tblUserRoleTONew.UserId = Convert.ToInt32(tblUserRoleTODT["userId"].ToString());
                    if (tblUserRoleTODT["roleId"] != DBNull.Value)
                        tblUserRoleTONew.RoleId = Convert.ToInt32(tblUserRoleTODT["roleId"].ToString());
                    if (tblUserRoleTODT["isActive"] != DBNull.Value)
                        tblUserRoleTONew.IsActive = Convert.ToInt32(tblUserRoleTODT["isActive"].ToString());
                    if (tblUserRoleTODT["createdBy"] != DBNull.Value)
                        tblUserRoleTONew.CreatedBy = Convert.ToInt32(tblUserRoleTODT["createdBy"].ToString());
                    if (tblUserRoleTODT["createdOn"] != DBNull.Value)
                        tblUserRoleTONew.CreatedOn = Convert.ToDateTime(tblUserRoleTODT["createdOn"].ToString());
                    if (tblUserRoleTODT["roleDesc"] != DBNull.Value)
                        tblUserRoleTONew.RoleDesc = Convert.ToString(tblUserRoleTODT["roleDesc"].ToString());
                    if (tblUserRoleTODT["enableAreaAlloc"] != DBNull.Value)
                        tblUserRoleTONew.EnableAreaAlloc = Convert.ToInt32(tblUserRoleTODT["enableAreaAlloc"].ToString());

                    tblUserRoleTOList.Add(tblUserRoleTONew);
                }
            }
            return tblUserRoleTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserRoleTO, cmdInsert);
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

        public  int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserRoleTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblUserRoleTO tblUserRoleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserRole]( " +
                            "  [userId]" +
                            " ,[roleId]" +
                            " ,[isActive]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " )" +
                " VALUES (" +
                            "  @UserId " +
                            " ,@RoleId " +
                            " ,@IsActive " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdUserRole", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IdUserRole;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserRoleTO.UserId;
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblUserRoleTO.RoleId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserRoleTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserRoleTO.CreatedOn;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblUserRoleTO.IdUserRole = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public  int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserRoleTO, cmdUpdate);
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

        public  int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserRoleTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblUserRoleTO tblUserRoleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserRole] SET " +
                            "  [userId]= @UserId" +
                            " ,[roleId]= @RoleId" +
                            " ,[isActive]= @IsActive" +
                            " WHERE [idUserRole] = @IdUserRole";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserRole", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IdUserRole;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserRoleTO.UserId;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblUserRoleTO.RoleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblUserRole(Int32 idUserRole)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUserRole, cmdDelete);
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

        public  int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUserRole, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idUserRole, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserRole] " +
            " WHERE idUserRole = " + idUserRole + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUserRole", System.Data.SqlDbType.Int).Value = tblUserRoleTO.IdUserRole;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
