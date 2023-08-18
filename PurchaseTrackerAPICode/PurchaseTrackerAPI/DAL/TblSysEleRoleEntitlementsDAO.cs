using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblSysEleRoleEntitlementsDAO : ITblSysEleRoleEntitlementsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblSysEleRoleEntitlementsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSysEleRoleEntitlements]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblSysEleRoleEntitlementsTO> SelectAllTblSysEleRoleEntitlements(int roleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE roleId=" + roleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleRoleEntitlementsTO> list = ConvertDTToList(rdr);
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

        public  Dictionary<int, String> SelectAllTblSysEleRoleEntitlementsDCT(Int32 roleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblSysEleRoleEntitlementsTODT = null;
            Dictionary<int, String> sysEleDCT = new Dictionary<int, string>();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE roleId=" + roleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblSysEleRoleEntitlementsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblSysEleRoleEntitlementsTODT.Read())
                {
                    TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTONew = new TblSysEleRoleEntitlementsTO();
                    int sysEleId = 0;
                    string permission = string.Empty;

                    if (tblSysEleRoleEntitlementsTODT["sysEleId"] != DBNull.Value)
                        sysEleId = Convert.ToInt32(tblSysEleRoleEntitlementsTODT["sysEleId"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["permission"] != DBNull.Value)
                        permission = Convert.ToString(tblSysEleRoleEntitlementsTODT["permission"].ToString());

                    if (sysEleId > 0 && !string.IsNullOrEmpty(permission))
                        sysEleDCT.Add(sysEleId, permission);
                }

                return sysEleDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblSysEleRoleEntitlementsTODT != null)
                    tblSysEleRoleEntitlementsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblSysEleRoleEntitlementsTO SelectTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idRoleEntitlement = " + idRoleEntitlement + " AND roleId = " + roleId + " AND sysEleId = " + sysEleId + " AND permission = " + permission + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleRoleEntitlementsTO> list = ConvertDTToList(rdr);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblSysEleRoleEntitlementsTO SelectRoleSysEleUserEntitlements(Int32 roleId, Int32 sysEleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE roleId = " + roleId + " AND sysEleId = " + sysEleId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysEleRoleEntitlementsTO> list = ConvertDTToList(rdr);
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

        public  List<TblSysEleRoleEntitlementsTO> ConvertDTToList(SqlDataReader tblSysEleRoleEntitlementsTODT)
        {
            List<TblSysEleRoleEntitlementsTO> tblSysEleRoleEntitlementsTOList = new List<TblSysEleRoleEntitlementsTO>();
            if (tblSysEleRoleEntitlementsTODT != null)
            {
                while (tblSysEleRoleEntitlementsTODT.Read())
                {
                    TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTONew = new TblSysEleRoleEntitlementsTO();
                    if (tblSysEleRoleEntitlementsTODT["idRoleEntitlement"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.IdRoleEntitlement = Convert.ToInt32(tblSysEleRoleEntitlementsTODT["idRoleEntitlement"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["roleId"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.RoleId = Convert.ToInt32(tblSysEleRoleEntitlementsTODT["roleId"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["sysEleId"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.SysEleId = Convert.ToInt32(tblSysEleRoleEntitlementsTODT["sysEleId"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["createdBy"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.CreatedBy = Convert.ToInt32(tblSysEleRoleEntitlementsTODT["createdBy"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["createdOn"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.CreatedOn = Convert.ToDateTime(tblSysEleRoleEntitlementsTODT["createdOn"].ToString());
                    if (tblSysEleRoleEntitlementsTODT["permission"] != DBNull.Value)
                        tblSysEleRoleEntitlementsTONew.Permission = Convert.ToString(tblSysEleRoleEntitlementsTODT["permission"].ToString());
                    tblSysEleRoleEntitlementsTOList.Add(tblSysEleRoleEntitlementsTONew);
                }
            }
            return tblSysEleRoleEntitlementsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSysEleRoleEntitlementsTO, cmdInsert);
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

        public  int InsertTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSysEleRoleEntitlementsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSysEleRoleEntitlements]( " + 
                            "  [roleId]" +
                            " ,[sysEleId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[permission]" +
                            " )" +
                " VALUES (" +
                            "  @RoleId " +
                            " ,@SysEleId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@Permission " + 
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdRoleEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.IdRoleEntitlement;
            cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.RoleId;
            cmdInsert.Parameters.Add("@SysEleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.SysEleId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSysEleRoleEntitlementsTO.CreatedOn;
            cmdInsert.Parameters.Add("@Permission", System.Data.SqlDbType.Char).Value = tblSysEleRoleEntitlementsTO.Permission;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSysEleRoleEntitlementsTO.IdRoleEntitlement = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSysEleRoleEntitlementsTO, cmdUpdate);
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

        public  int UpdateTblSysEleRoleEntitlements(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSysEleRoleEntitlementsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblSysEleRoleEntitlementsTO tblSysEleRoleEntitlementsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSysEleRoleEntitlements] SET " + 
                                "  [roleId]= @RoleId" +
                                " ,[sysEleId]= @SysEleId" +
                                " ,[createdBy]= @CreatedBy" +
                                " ,[createdOn]= @CreatedOn" +
                                " ,[permission] = @Permission" +
                                " WHERE [idRoleEntitlement] = @IdRoleEntitlement"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRoleEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.IdRoleEntitlement;
            cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.RoleId;
            cmdUpdate.Parameters.Add("@SysEleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.SysEleId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSysEleRoleEntitlementsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Permission", System.Data.SqlDbType.Char).Value = tblSysEleRoleEntitlementsTO.Permission;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRoleEntitlement, roleId, sysEleId, permission, cmdDelete);
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

        public  int DeleteTblSysEleRoleEntitlements(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRoleEntitlement, roleId, sysEleId, permission, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idRoleEntitlement, Int32 roleId, Int32 sysEleId, String permission, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSysEleRoleEntitlements] " +
            " WHERE idRoleEntitlement = " + idRoleEntitlement +" AND roleId = " + roleId +" AND sysEleId = " + sysEleId +" AND permission = " + permission +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRoleEntitlement", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.IdRoleEntitlement;
            //cmdDelete.Parameters.Add("@roleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.RoleId;
            //cmdDelete.Parameters.Add("@sysEleId", System.Data.SqlDbType.Int).Value = tblSysEleRoleEntitlementsTO.SysEleId;
            //cmdDelete.Parameters.Add("@permission", System.Data.SqlDbType.Char).Value = tblSysEleRoleEntitlementsTO.Permission;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
