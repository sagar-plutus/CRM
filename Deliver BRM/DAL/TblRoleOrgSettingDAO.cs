using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblRoleOrgSettingDAO
    {
        #region Insertion
        internal static int SaveRolesAndOrg(RoleOrgTO roleOrgTO, SqlConnection conn)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;


                String sqlQuery = @" INSERT INTO [tblRoleOrgSetting]( " +
                            "  [visitTypeID]" +
                            " ,[personTypeID]" +
                            " ,[roleID]" +
                            " ,[orgID]" +
                            " ,[isActive]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " )" +
                " VALUES (" +
                            "  @visitTypeID " +
                            " ,@personTypeID " +
                            " ,@roleID " +
                            " ,@orgID " +
                            " ,@isActive " +
                            " ,@createdBy " +
                            " ,@createdOn " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@visitTypeID", System.Data.SqlDbType.Int).Value = roleOrgTO.VisitTypeId;
                cmdInsert.Parameters.Add("@personTypeID", System.Data.SqlDbType.Int).Value = roleOrgTO.PersonTypeId;
                cmdInsert.Parameters.Add("@roleID", System.Data.SqlDbType.Int).Value = roleOrgTO.RoleId;
                cmdInsert.Parameters.Add("@orgID", System.Data.SqlDbType.Int).Value = roleOrgTO.OrgId;
                cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = roleOrgTO.CreatedBy;
                cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = roleOrgTO.CreatedBy;

                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = roleOrgTO.Status;
                cmdInsert.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = roleOrgTO.CreatedOn;
                cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = roleOrgTO.CreatedOn;
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    roleOrgTO.IdRoleOrg = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
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
        #endregion

        #region  Select 
        public static int CheckIfExists(RoleOrgTO roleOrgTO)
        {
            {
                String sqlConnStr = Startup.ConnectionString;
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    int res = 0;
                    String aqlQuery = "select 1 as res from tblRoleOrgSetting" +
                                       " where personTypeID = " + roleOrgTO.PersonTypeId +
                                        " and visitTypeID =" + roleOrgTO.VisitTypeId +
                                        " and roleID =" + roleOrgTO.RoleId +
                                        " and orgID =" + roleOrgTO.OrgId;

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    while (dateReader.Read())
                    {
                        if (dateReader["res"] != DBNull.Value)
                        {
                            res = Convert.ToInt32(dateReader["res"].ToString());
                        }
                    }

                    return res;
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    dateReader.Dispose();
                    conn.Close();
                    cmdSelect.Dispose();
                }

            }
        }

        public static List<DropDownTO> SelectSavedRoles(int visitTypeId, int personTypeId)
        {
            {

                String sqlConnStr = Startup.ConnectionString;
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    String aqlQuery = "select roleID,idRoleOrg,isActive from tblRoleOrgSetting" +
                                       " where roleID not in (0) and personTypeID = " + personTypeId +
                                        " and visitTypeID =" + visitTypeId;

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                    while (dateReader.Read())
                    {
                        DropDownTO dropDownTONew = new DropDownTO();
                        if (dateReader["roleID"] != DBNull.Value)
                        {
                            dropDownTONew.Value = Convert.ToInt32(dateReader["roleID"].ToString());
                        }
                        if (dateReader["idRoleOrg"] != DBNull.Value)
                            dropDownTONew.Text = Convert.ToString(dateReader["idRoleOrg"].ToString());

                        if (dateReader["isActive"] != DBNull.Value)
                            dropDownTONew.Tag = Convert.ToString(dateReader["isActive"].ToString());

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
        }

        internal static List<DropDownTO> SelectSavedOrg(int visitTypeId, int personTypeId)
        {
            {

                String sqlConnStr = Startup.ConnectionString;
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    String aqlQuery = "select orgID,idRoleOrg,isActive from tblRoleOrgSetting" +
                                       " where orgID not in (0) and  personTypeID = " + personTypeId +
                                        " and visitTypeID =" + visitTypeId;

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                    while (dateReader.Read())
                    {
                        DropDownTO dropDownTONew = new DropDownTO();
                        if (dateReader["orgID"] != DBNull.Value)
                        {
                            dropDownTONew.Value = Convert.ToInt32(dateReader["orgID"].ToString());
                        }
                        if (dateReader["idRoleOrg"] != DBNull.Value)
                            dropDownTONew.Text = Convert.ToString(dateReader["idRoleOrg"].ToString());

                        if (dateReader["isActive"] != DBNull.Value)
                            dropDownTONew.Tag = Convert.ToString(dateReader["isActive"].ToString());

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
        }

        internal static List<DropDownTO> SelectAllSystemRoleOrgListForDropDown(int visitTypeId, int personTypeId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select idRoleOrg, ISNULL(tblRole.roleDesc,dimOrgType.OrgType) AS disp, roleID,orgID from tblRoleOrgSetting" +
                                   " LEFT JOIN tblRole ON tblRole.idRole = tblRoleOrgSetting.roleID" +
                                   " LEFT JOIN dimOrgType ON dimOrgType.idOrgType = tblRoleOrgSetting.orgID" +
                                   " where tblRoleOrgSetting.isActive =1 and tblRoleOrgSetting.personTypeID = " + personTypeId +
                                    " and tblRoleOrgSetting.visitTypeID =" + visitTypeId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["roleID"] != DBNull.Value)
                    {
                        if (Convert.ToInt32(dateReader["roleID"].ToString()) == 0)
                        {

                            if (dateReader["orgID"] != DBNull.Value)
                            {
                                dropDownTONew.Tag = 1;
                                dropDownTONew.Value = Convert.ToInt32(dateReader["orgID"].ToString());
                            }
                        }
                        else
                        {
                            dropDownTONew.Tag = 0;
                            dropDownTONew.Value = Convert.ToInt32(dateReader["roleID"].ToString());
                        }
                    }
                    if (dateReader["disp"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["disp"].ToString());

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

        #endregion

        #region Update
        internal static int UpdateRolesAndOrg(RoleOrgTO roleOrgTO, SqlConnection conn)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;


                String sqlQuery = @" UPDATE [tblRoleOrgSetting] set " +
                            "  [visitTypeID] =@visitTypeID" +
                            " ,[personTypeID] =@personTypeID" +
                            " ,[roleID] =@roleID" +
                            " ,[orgID] =@orgID" +
                            " ,[isActive] =@isActive" +
                            " ,[updatedBy]= @updatedBy" +
                            " ,[updatedOn] =@updatedOn" +
                             " where personTypeID = " + roleOrgTO.PersonTypeId +
                                        " and visitTypeID =" + roleOrgTO.VisitTypeId +
                                        " and roleID =" + roleOrgTO.RoleId +
                                        " and orgID =" + roleOrgTO.OrgId;


                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@visitTypeID", System.Data.SqlDbType.Int).Value = roleOrgTO.VisitTypeId;
                cmdInsert.Parameters.Add("@personTypeID", System.Data.SqlDbType.Int).Value = roleOrgTO.PersonTypeId;
                cmdInsert.Parameters.Add("@roleID", System.Data.SqlDbType.Int).Value = roleOrgTO.RoleId;
                cmdInsert.Parameters.Add("@orgID", System.Data.SqlDbType.Int).Value = roleOrgTO.OrgId;
                cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = roleOrgTO.CreatedBy;
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = roleOrgTO.Status;
                cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = roleOrgTO.CreatedOn;
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    return 1;
                }
                else return 0;
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

        #endregion

    }
}
