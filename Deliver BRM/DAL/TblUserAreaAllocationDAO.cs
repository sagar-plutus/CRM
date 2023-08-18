using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;


namespace SalesTrackerAPI.DAL
{
    public class TblUserAreaAllocationDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT userArea.*,org.firmName,userDtl.userDisplayName,district.districtName,createdUserDtl.userDisplayName as createdUserName " +
                                  " FROM tblUserAreaAllocation userArea " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = userArea.cnfOrgId " +
                                  " LEFT JOIN tblUser userDtl ON userArea.userId = userDtl.idUser " +
                                  " LEFT JOIN dimDistrict district ON district.idDistrict = userArea.districtId " +
                                  " LEFT JOIN tblUser createdUserDtl ON userArea.createdBy = createdUserDtl.idUser ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblUserAreaAllocationTO> SelectAllTblUserAreaAllocation(Int32 userId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userArea.userId=" + userId + " AND userArea.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<UserAreaCnfDealerDtlTO> SelectAllUserAreaCnfDealerList(Int32 userId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblUserAreaAllocationTODT = null;
            try
            {
                cmdSelect.CommandText = " SELECT areaConf.* , dealerOrg.idOrganization AS dealerOrgId , " +
                                        " tblUser.userDisplayName,addrDtl.districtName,cnfOrg.firmName as cnfOrgName, " +
                                        " dealerOrg.firmName AS dealerName FROM tblOrganization dealerOrg " +
                                        " INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                        " INNER JOIN tblOrganization cnfOrg ON tblCnfDealers.cnfOrgId = cnfOrg.idOrganization " +
                                        " INNER JOIN " +
                                        " ( " +
                                        "    SELECT tblAddress.*, districtName, organizationId FROM tblOrgAddress " +
                                        "    INNER JOIN tblAddress ON idAddr = addressId " +
                                        "    INNER JOIN dimDistrict ON idDistrict = districtId " +
                                        "    WHERE addrTypeId = 1 " +
                                        " ) addrDtl ON dealerOrg.idOrganization = organizationId " +
                                        " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                        " INNER JOIN tblUser ON idUser = areaConf.userId " +
                                        " WHERE dealerOrg.isActive = 1 AND tblCnfDealers.isActive = 1 " +
                                        " AND dealerOrg.orgTypeId =" + (int)Constants.OrgTypeE.DEALER +
                                        " AND areaConf.userId = " + userId +
                                        " AND areaConf.isActive = 1 ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblUserAreaAllocationTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<UserAreaCnfDealerDtlTO> userAreaAllocationTOList = new List<UserAreaCnfDealerDtlTO>();
                if (tblUserAreaAllocationTODT != null)
                {
                    while (tblUserAreaAllocationTODT.Read())
                    {
                        UserAreaCnfDealerDtlTO tblUserAreaAllocationTONew = new UserAreaCnfDealerDtlTO();
                        if (tblUserAreaAllocationTODT["userId"] != DBNull.Value)
                            tblUserAreaAllocationTONew.UserId = Convert.ToInt32(tblUserAreaAllocationTODT["userId"].ToString());
                        if (tblUserAreaAllocationTODT["cnfOrgId"] != DBNull.Value)
                            tblUserAreaAllocationTONew.CnfOrgId = Convert.ToInt32(tblUserAreaAllocationTODT["cnfOrgId"].ToString());
                        if (tblUserAreaAllocationTODT["districtId"] != DBNull.Value)
                            tblUserAreaAllocationTONew.DistrictId = Convert.ToInt32(tblUserAreaAllocationTODT["districtId"].ToString());
                        if (tblUserAreaAllocationTODT["dealerOrgId"] != DBNull.Value)
                            tblUserAreaAllocationTONew.DealerOrgId = Convert.ToInt32(tblUserAreaAllocationTODT["dealerOrgId"].ToString());
                        if (tblUserAreaAllocationTODT["isActive"] != DBNull.Value)
                            tblUserAreaAllocationTONew.IsActive = Convert.ToInt32(tblUserAreaAllocationTODT["isActive"].ToString());
                        if (tblUserAreaAllocationTODT["userDisplayName"] != DBNull.Value)
                            tblUserAreaAllocationTONew.UserName = Convert.ToString(tblUserAreaAllocationTODT["userDisplayName"].ToString());
                        if (tblUserAreaAllocationTODT["districtName"] != DBNull.Value)
                            tblUserAreaAllocationTONew.DistrictName = Convert.ToString(tblUserAreaAllocationTODT["districtName"].ToString());
                        if (tblUserAreaAllocationTODT["cnfOrgName"] != DBNull.Value)
                            tblUserAreaAllocationTONew.CnfOrgName = Convert.ToString(tblUserAreaAllocationTODT["cnfOrgName"].ToString());
                        if (tblUserAreaAllocationTODT["dealerName"] != DBNull.Value)
                            tblUserAreaAllocationTONew.DealerOrgName = Convert.ToString(tblUserAreaAllocationTODT["dealerName"].ToString());

                        userAreaAllocationTOList.Add(tblUserAreaAllocationTONew);
                    }
                }
                return userAreaAllocationTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblUserAreaAllocationTODT != null)
                    tblUserAreaAllocationTODT.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static TblUserAreaAllocationTO SelectTblUserAreaAllocation(Int32 idAreaAllocDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAreaAllocDtl = " + idAreaAllocDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List< TblUserAreaAllocationTO> list =ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblUserAreaAllocationTO> ConvertDTToList(SqlDataReader tblUserAreaAllocationTODT)
        {
            List<TblUserAreaAllocationTO> tblUserAreaAllocationTOList = new List<TblUserAreaAllocationTO>();
            if (tblUserAreaAllocationTODT != null)
            {
                while (tblUserAreaAllocationTODT.Read())
                {
                    TblUserAreaAllocationTO tblUserAreaAllocationTONew = new TblUserAreaAllocationTO();
                    if (tblUserAreaAllocationTODT["idAreaAllocDtl"] != DBNull.Value)
                        tblUserAreaAllocationTONew.IdAreaAllocDtl = Convert.ToInt32(tblUserAreaAllocationTODT["idAreaAllocDtl"].ToString());
                    if (tblUserAreaAllocationTODT["userId"] != DBNull.Value)
                        tblUserAreaAllocationTONew.UserId = Convert.ToInt32(tblUserAreaAllocationTODT["userId"].ToString());
                    if (tblUserAreaAllocationTODT["cnfOrgId"] != DBNull.Value)
                        tblUserAreaAllocationTONew.CnfOrgId = Convert.ToInt32(tblUserAreaAllocationTODT["cnfOrgId"].ToString());
                    if (tblUserAreaAllocationTODT["districtId"] != DBNull.Value)
                        tblUserAreaAllocationTONew.DistrictId = Convert.ToInt32(tblUserAreaAllocationTODT["districtId"].ToString());
                    if (tblUserAreaAllocationTODT["createdBy"] != DBNull.Value)
                        tblUserAreaAllocationTONew.CreatedBy = Convert.ToInt32(tblUserAreaAllocationTODT["createdBy"].ToString());
                    if (tblUserAreaAllocationTODT["isActive"] != DBNull.Value)
                        tblUserAreaAllocationTONew.IsActive = Convert.ToInt32(tblUserAreaAllocationTODT["isActive"].ToString());
                    if (tblUserAreaAllocationTODT["createdOn"] != DBNull.Value)
                        tblUserAreaAllocationTONew.CreatedOn = Convert.ToDateTime(tblUserAreaAllocationTODT["createdOn"].ToString());
                    if (tblUserAreaAllocationTODT["remark"] != DBNull.Value)
                        tblUserAreaAllocationTONew.Remark = Convert.ToString(tblUserAreaAllocationTODT["remark"].ToString());
                    if (tblUserAreaAllocationTODT["firmName"] != DBNull.Value)
                        tblUserAreaAllocationTONew.CnfOrgName = Convert.ToString(tblUserAreaAllocationTODT["firmName"].ToString());
                    if (tblUserAreaAllocationTODT["userDisplayName"] != DBNull.Value)
                        tblUserAreaAllocationTONew.UserName = Convert.ToString(tblUserAreaAllocationTODT["userDisplayName"].ToString());
                    if (tblUserAreaAllocationTODT["districtName"] != DBNull.Value)
                        tblUserAreaAllocationTONew.DistrictName = Convert.ToString(tblUserAreaAllocationTODT["districtName"].ToString());
                    if (tblUserAreaAllocationTODT["createdUserName"] != DBNull.Value)
                        tblUserAreaAllocationTONew.CreatedByUserName = Convert.ToString(tblUserAreaAllocationTODT["createdUserName"].ToString());

                    tblUserAreaAllocationTOList.Add(tblUserAreaAllocationTONew);
                }
            }
            return tblUserAreaAllocationTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserAreaAllocationTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserAreaAllocationTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserAreaAllocation]( " +
                                "  [userId]" +
                                " ,[cnfOrgId]" +
                                " ,[districtId]" +
                                " ,[createdBy]" +
                                " ,[isActive]" +
                                " ,[createdOn]" +
                                " ,[remark]" +
                                " )" +
                    " VALUES (" +
                                "  @UserId " +
                                " ,@CnfOrgId " +
                                " ,@DistrictId " +
                                " ,@CreatedBy " +
                                " ,@IsActive " +
                                " ,@CreatedOn " +
                                " ,@Remark " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAreaAllocDtl", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.IdAreaAllocDtl;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.UserId;
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.CnfOrgId;
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.DistrictId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserAreaAllocationTO.CreatedOn;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblUserAreaAllocationTO.Remark);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblUserAreaAllocationTO.IdAreaAllocDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserAreaAllocationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblUserAreaAllocation(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserAreaAllocationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblUserAreaAllocationTO tblUserAreaAllocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserAreaAllocation] SET " + 
                            "  [userId]= @UserId" +
                            " ,[cnfOrgId]= @CnfOrgId" +
                            " ,[districtId]= @DistrictId" +
                            " ,[isActive]= @IsActive" +
                            " ,[remark] = @Remark" +
                            " WHERE  [idAreaAllocDtl] = @IdAreaAllocDtl"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAreaAllocDtl", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.IdAreaAllocDtl;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.UserId;
            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.CnfOrgId;
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.DistrictId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.IsActive;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserAreaAllocationTO.Remark);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAreaAllocDtl, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblUserAreaAllocation(Int32 idAreaAllocDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAreaAllocDtl, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idAreaAllocDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserAreaAllocation] " +
            " WHERE idAreaAllocDtl = " + idAreaAllocDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAreaAllocDtl", System.Data.SqlDbType.Int).Value = tblUserAreaAllocationTO.IdAreaAllocDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
