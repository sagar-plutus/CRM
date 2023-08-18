using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SalesTrackerAPI.DAL
{
    public class DimOrgTypeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimOrgType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimOrgTypeTO> SelectAllDimOrgType()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if(rdr!=null) rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimOrgTypeTO SelectDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgType = " + idOrgType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null) rdr.Dispose(); 
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectAllDimOrgType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public static List<DimOrgTypeTO> ConvertDTToList(SqlDataReader dimOrgTypeTODT)
        {
            List<DimOrgTypeTO> dimOrgTypeTOList = new List<DimOrgTypeTO>();
            if (dimOrgTypeTODT != null)
            {
                while (dimOrgTypeTODT.Read())
                {
                    DimOrgTypeTO dimOrgTypeTONew = new DimOrgTypeTO();
                    if (dimOrgTypeTODT["idOrgType"] != DBNull.Value)
                        dimOrgTypeTONew.IdOrgType = Convert.ToInt32(dimOrgTypeTODT["idOrgType"].ToString());
                    if (dimOrgTypeTODT["isSystem"] != DBNull.Value)
                        dimOrgTypeTONew.IsSystem = Convert.ToInt32(dimOrgTypeTODT["isSystem"].ToString());
                    if (dimOrgTypeTODT["isActive"] != DBNull.Value)
                        dimOrgTypeTONew.IsActive = Convert.ToInt32(dimOrgTypeTODT["isActive"].ToString());
                    if (dimOrgTypeTODT["createUserYn"] != DBNull.Value)
                        dimOrgTypeTONew.CreateUserYn = Convert.ToInt32(dimOrgTypeTODT["createUserYn"].ToString());
                    if (dimOrgTypeTODT["defaultRoleId"] != DBNull.Value)
                        dimOrgTypeTONew.DefaultRoleId = Convert.ToInt32(dimOrgTypeTODT["defaultRoleId"].ToString());
                    if (dimOrgTypeTODT["OrgType"] != DBNull.Value)
                        dimOrgTypeTONew.OrgType = Convert.ToString(dimOrgTypeTODT["OrgType"].ToString());
                    dimOrgTypeTOList.Add(dimOrgTypeTONew);
                }
            }
            return dimOrgTypeTOList;
        }
        #endregion

        #region Insertion
        public static int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimOrgTypeTO, cmdInsert);
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

        public static int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimOrgTypeTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimOrgType]( " + 
                                "  [idOrgType]" +
                                " ,[isSystem]" +
                                " ,[isActive]" +
                                " ,[createUserYn]" +
                                " ,[defaultRoleId]" +
                                " ,[OrgType]" +
                                " )" +
                    " VALUES (" +
                                "  @IdOrgType " +
                                " ,@IsSystem " +
                                " ,@IsActive " +
                                " ,@CreateUserYn " +
                                " ,@DefaultRoleId " +
                                " ,@OrgType " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            cmdInsert.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsSystem;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsActive;
            cmdInsert.Parameters.Add("@CreateUserYn", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.CreateUserYn;
            cmdInsert.Parameters.Add("@DefaultRoleId", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.DefaultRoleId;
            cmdInsert.Parameters.Add("@OrgType", System.Data.SqlDbType.NVarChar).Value = dimOrgTypeTO.OrgType;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimOrgTypeTO, cmdUpdate);
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

        public static int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimOrgTypeTO, cmdUpdate);
            }
            catch(Exception ex)
            {
               
                
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimOrgType] SET " + 
            "  [idOrgType] = @IdOrgType" +
            " ,[isSystem]= @IsSystem" +
            " ,[isActive]= @IsActive" +
            " ,[createUserYn]= @CreateUserYn" +
            " ,[defaultRoleId]= @DefaultRoleId" +
            " ,[OrgType] = @OrgType" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            cmdUpdate.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsSystem;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@CreateUserYn", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.CreateUserYn;
            cmdUpdate.Parameters.Add("@DefaultRoleId", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.DefaultRoleId;
            cmdUpdate.Parameters.Add("@OrgType", System.Data.SqlDbType.NVarChar).Value = dimOrgTypeTO.OrgType;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimOrgType(Int32 idOrgType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgType, cmdDelete);
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

        public static int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgType, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idOrgType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimOrgType] " +
            " WHERE idOrgType = " + idOrgType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
