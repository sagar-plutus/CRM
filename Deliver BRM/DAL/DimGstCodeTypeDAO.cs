using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class DimGstCodeTypeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimGstCodeType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimGstCodeTypeTO> SelectAllDimGstCodeType()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGstCodeTypeTO> list = ConvertDTToList(reader);
                return list;
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

        public static DimGstCodeTypeTO SelectDimGstCodeType(Int32 idCodeType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idCodeType = " + idCodeType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGstCodeTypeTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DimGstCodeTypeTO> ConvertDTToList(SqlDataReader dimGstCodeTypeTODT)
        {
            List<DimGstCodeTypeTO> dimGstCodeTypeTOList = new List<DimGstCodeTypeTO>();
            if (dimGstCodeTypeTODT != null)
            {
                while (dimGstCodeTypeTODT.Read())
                {
                    DimGstCodeTypeTO dimGstCodeTypeTONew = new DimGstCodeTypeTO();
                    if (dimGstCodeTypeTODT["idCodeType"] != DBNull.Value)
                        dimGstCodeTypeTONew.IdCodeType = Convert.ToInt32(dimGstCodeTypeTODT["idCodeType"].ToString());
                    if (dimGstCodeTypeTODT["createdOn"] != DBNull.Value)
                        dimGstCodeTypeTONew.CreatedOn = Convert.ToDateTime(dimGstCodeTypeTODT["createdOn"].ToString());
                    if (dimGstCodeTypeTODT["codeDesc"] != DBNull.Value)
                        dimGstCodeTypeTONew.CodeDesc = Convert.ToString(dimGstCodeTypeTODT["codeDesc"].ToString());
                    dimGstCodeTypeTOList.Add(dimGstCodeTypeTONew);
                }
            }
            return dimGstCodeTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimGstCodeTypeTO, cmdInsert);
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

        public static int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimGstCodeTypeTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(DimGstCodeTypeTO dimGstCodeTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimGstCodeType]( " + 
                            "  [idCodeType]" +
                            " ,[createdOn]" +
                            " ,[codeDesc]" +
                            " )" +
                " VALUES (" +
                            "  @IdCodeType " +
                            " ,@CreatedOn " +
                            " ,@CodeDesc " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdCodeType", System.Data.SqlDbType.Int).Value = dimGstCodeTypeTO.IdCodeType;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimGstCodeTypeTO.CreatedOn;
            cmdInsert.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = dimGstCodeTypeTO.CodeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimGstCodeTypeTO, cmdUpdate);
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

        public static int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimGstCodeTypeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimGstCodeTypeTO dimGstCodeTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimGstCodeType] SET " + 
            "  [idCodeType] = @IdCodeType" +
            " ,[createdOn]= @CreatedOn" +
            " ,[codeDesc] = @CodeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdCodeType", System.Data.SqlDbType.Int).Value = dimGstCodeTypeTO.IdCodeType;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimGstCodeTypeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = dimGstCodeTypeTO.CodeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimGstCodeType(Int32 idCodeType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idCodeType, cmdDelete);
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

        public static int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idCodeType, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idCodeType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimGstCodeType] " +
            " WHERE idCodeType = " + idCodeType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idCodeType", System.Data.SqlDbType.Int).Value = dimGstCodeTypeTO.IdCodeType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
