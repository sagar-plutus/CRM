using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblSysEleUserEntitlementsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSysEleUserEntitlements]"; 
            return sqlSelectQry;
        }
        #endregion



        #region Selection
        public static List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlements(int userId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userId=" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleUserEntitlementsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlements(int userId, int? moduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                string query = SqlSelectQuery();
                if (moduleId != null)
                    query = query + " LEFT JOIN tblSysElements sysele on tblSysEleUserEntitlements.sysEleId = sysele.idSysElement ";
                cmdSelect.CommandText = query + " WHERE userId=" + userId;
                if (moduleId != null)
                    cmdSelect.CommandText = cmdSelect.CommandText + " and sysele.moduleId=" + moduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleUserEntitlementsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idUserEntitlement = " + idUserEntitlement +" AND userId = " + userId +" AND sysEleId = " + sysEleId +" AND permission = " + permission +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleUserEntitlementsTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblSysEleUserEntitlementsTO SelectUserSysEleUserEntitlements(Int32 userId, Int32 sysEleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userId = " + userId + " AND sysEleId = " + sysEleId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleUserEntitlementsTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSysEleUserEntitlementsTO> ConvertDTToList(SqlDataReader tblSysEleUserEntitlementsTODT)
        {
            List<TblSysEleUserEntitlementsTO> tblSysEleUserEntitlementsTOList = new List<TblSysEleUserEntitlementsTO>();
            if (tblSysEleUserEntitlementsTODT != null)
            {
                while (tblSysEleUserEntitlementsTODT.Read())
                {
                    TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTONew = new TblSysEleUserEntitlementsTO();
                    if (tblSysEleUserEntitlementsTODT["idUserEntitlement"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.IdUserEntitlement = Convert.ToInt32(tblSysEleUserEntitlementsTODT["idUserEntitlement"].ToString());
                    if (tblSysEleUserEntitlementsTODT["userId"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.UserId = Convert.ToInt32(tblSysEleUserEntitlementsTODT["userId"].ToString());
                    if (tblSysEleUserEntitlementsTODT["sysEleId"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.SysEleId = Convert.ToInt32(tblSysEleUserEntitlementsTODT["sysEleId"].ToString());
                    if (tblSysEleUserEntitlementsTODT["createdBy"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.CreatedBy = Convert.ToInt32(tblSysEleUserEntitlementsTODT["createdBy"].ToString());
                    if (tblSysEleUserEntitlementsTODT["createdOn"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.CreatedOn = Convert.ToDateTime(tblSysEleUserEntitlementsTODT["createdOn"].ToString());
                    if (tblSysEleUserEntitlementsTODT["permission"] != DBNull.Value)
                        tblSysEleUserEntitlementsTONew.Permission = Convert.ToString(tblSysEleUserEntitlementsTODT["permission"].ToString());
                    tblSysEleUserEntitlementsTOList.Add(tblSysEleUserEntitlementsTONew);
                }
            }
            return tblSysEleUserEntitlementsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSysEleUserEntitlementsTO, cmdInsert);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSysEleUserEntitlementsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSysEleUserEntitlements]( " + 
                            "  [userId]" +
                            " ,[sysEleId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[permission]" +
                            " )" +
                " VALUES (" +
                            "  @UserId " +
                            " ,@SysEleId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Permission " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdUserEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.IdUserEntitlement;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.UserId;
            cmdInsert.Parameters.Add("@SysEleId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.SysEleId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSysEleUserEntitlementsTO.CreatedOn;
            cmdInsert.Parameters.Add("@Permission", System.Data.SqlDbType.Char).Value = tblSysEleUserEntitlementsTO.Permission;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSysEleUserEntitlementsTO.IdUserEntitlement = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSysEleUserEntitlementsTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSysEleUserEntitlementsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSysEleUserEntitlements] SET " + 
                            "  [userId]= @UserId" +
                            " ,[sysEleId]= @SysEleId" +
                            " ,[permission] = @Permission" +
                            " WHERE [idUserEntitlement] = @IdUserEntitlement "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.IdUserEntitlement;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.UserId;
            cmdUpdate.Parameters.Add("@SysEleId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.SysEleId;
            cmdUpdate.Parameters.Add("@Permission", System.Data.SqlDbType.Char).Value = tblSysEleUserEntitlementsTO.Permission;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUserEntitlement, userId, sysEleId, permission, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUserEntitlement, userId, sysEleId, permission, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSysEleUserEntitlements] " +
            " WHERE idUserEntitlement = " + idUserEntitlement +" AND userId = " + userId +" AND sysEleId = " + sysEleId +" AND permission = " + permission +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUserEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.IdUserEntitlement;
            //cmdDelete.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.UserId;
            //cmdDelete.Parameters.Add("@sysEleId", System.Data.SqlDbType.Int).Value = tblSysEleUserEntitlementsTO.SysEleId;
            //cmdDelete.Parameters.Add("@permission", System.Data.SqlDbType.Char).Value = tblSysEleUserEntitlementsTO.Permission;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
