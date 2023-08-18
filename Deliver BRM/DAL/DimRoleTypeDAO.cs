using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class DimRoleTypeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimRoleType] dimRoleType"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimRoleTypeTO> SelectAllDimRoleTypeList()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader da = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE dimRoleType.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                da = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return  ConvertDTToList(da);
                
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (da != null)
                    da.Close();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimRoleTypeTO SelectDimRoleType(Int32 idRoleType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader da = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idRoleType = " + idRoleType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                da = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimRoleTypeTO> roleTypeList= ConvertDTToList(da);
                if (roleTypeList != null && roleTypeList.Count == 1)
                    return roleTypeList[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public static List<DimRoleTypeTO> ConvertDTToList(SqlDataReader dimRoleTypeTODT)
        {
            List<DimRoleTypeTO> dimRoleTypeTOList = new List<DimRoleTypeTO>();
            if (dimRoleTypeTODT != null)
            {
                while (dimRoleTypeTODT.Read())
                {
                    DimRoleTypeTO dimRoleTypeTONew = new DimRoleTypeTO();
                    if (dimRoleTypeTODT["idRoleType"] != DBNull.Value)
                        dimRoleTypeTONew.IdRoleType = Convert.ToInt32(dimRoleTypeTODT["idRoleType"].ToString());
                    if (dimRoleTypeTODT["roleTypeDesc"] != DBNull.Value)
                        dimRoleTypeTONew.RoleTypeDesc = Convert.ToString(dimRoleTypeTODT["roleTypeDesc"].ToString());
                    //if (dimRoleTypeTODT["roleId"] != DBNull.Value)
                    //    dimRoleTypeTONew.RoleId = Convert.ToString(dimRoleTypeTODT["roleId"].ToString());
                    if (dimRoleTypeTODT["isActive"] != DBNull.Value)
                        dimRoleTypeTONew.IsActive = Convert.ToInt32(dimRoleTypeTODT["isActive"].ToString());
                    dimRoleTypeTOList.Add(dimRoleTypeTONew);
                }
            }
            return dimRoleTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimRoleTypeTO, cmdInsert);
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

        public static int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimRoleTypeTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimRoleTypeTO dimRoleTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimRoleType]( " + 
                                "  [idRoleType]" +
                                " ,[roleTypeDesc]" +
                                //" ,[roleId]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +
                                "  @IdRoleType " +
                                " ,@RoleTypeDesc " +
                                //" ,@RoleId " +
                                " ,@IsActive " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdRoleType", System.Data.SqlDbType.Int).Value = dimRoleTypeTO.IdRoleType;
            cmdInsert.Parameters.Add("@RoleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimRoleTypeTO.RoleTypeDesc;
            //cmdInsert.Parameters.Add("@RoleId", System.Data.SqlDbType.NVarChar).Value = dimRoleTypeTO.RoleId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimRoleTypeTO.IsActive;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimRoleTypeTO, cmdUpdate);
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

        public static int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimRoleTypeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimRoleTypeTO dimRoleTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimRoleType] SET " + 
            "  [idRoleType] = @IdRoleType" +
            " ,[roleTypeDesc]= @RoleTypeDesc" +
            //" ,[roleId]= @RoleId" +
            " ,[isActive] = @IsActive" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRoleType", System.Data.SqlDbType.Int).Value = dimRoleTypeTO.IdRoleType;
            cmdUpdate.Parameters.Add("@RoleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimRoleTypeTO.RoleTypeDesc;
           // cmdUpdate.Parameters.Add("@RoleId", System.Data.SqlDbType.NVarChar).Value = dimRoleTypeTO.RoleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimRoleTypeTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimRoleType(Int32 idRoleType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRoleType, cmdDelete);
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

        public static int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRoleType, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idRoleType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [dimRoleType] " +
                                    " WHERE idRoleType = " + idRoleType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRoleType", System.Data.SqlDbType.Int).Value = dimRoleTypeTO.IdRoleType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
