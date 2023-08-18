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
    public class DimTaxTypeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimTaxType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimTaxTypeTO> SelectAllDimTaxType()
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
                List<DimTaxTypeTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DimTaxTypeTO SelectDimTaxType(Int32 idTaxType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idTaxType = " + idTaxType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimTaxTypeTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DimTaxTypeTO> ConvertDTToList(SqlDataReader dimTaxTypeTODT)
        {
            List<DimTaxTypeTO> dimTaxTypeTOList = new List<DimTaxTypeTO>();
            if (dimTaxTypeTODT != null)
            {
                while (dimTaxTypeTODT.Read())
                {
                    DimTaxTypeTO dimTaxTypeTONew = new DimTaxTypeTO();
                    if (dimTaxTypeTODT["idTaxType"] != DBNull.Value)
                        dimTaxTypeTONew.IdTaxType = Convert.ToInt32(dimTaxTypeTODT["idTaxType"].ToString());
                    if (dimTaxTypeTODT["isActive"] != DBNull.Value)
                        dimTaxTypeTONew.IsActive = Convert.ToInt32(dimTaxTypeTODT["isActive"].ToString());
                    if (dimTaxTypeTODT["createdOn"] != DBNull.Value)
                        dimTaxTypeTONew.CreatedOn = Convert.ToDateTime(dimTaxTypeTODT["createdOn"].ToString());
                    if (dimTaxTypeTODT["taxTypeDesc"] != DBNull.Value)
                        dimTaxTypeTONew.TaxTypeDesc = Convert.ToString(dimTaxTypeTODT["taxTypeDesc"].ToString());
                    dimTaxTypeTOList.Add(dimTaxTypeTONew);
                }
            }
            return dimTaxTypeTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimTaxTypeTO, cmdInsert);
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

        public static int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimTaxTypeTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimTaxTypeTO dimTaxTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimTaxType]( " + 
                            "  [idTaxType]" +
                            " ,[isActive]" +
                            " ,[createdOn]" +
                            " ,[taxTypeDesc]" +
                            " )" +
                " VALUES (" +
                            "  @IdTaxType " +
                            " ,@IsActive " +
                            " ,@CreatedOn " +
                            " ,@TaxTypeDesc " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdTaxType", System.Data.SqlDbType.Int).Value = dimTaxTypeTO.IdTaxType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimTaxTypeTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimTaxTypeTO.CreatedOn;
            cmdInsert.Parameters.Add("@TaxTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimTaxTypeTO.TaxTypeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimTaxTypeTO, cmdUpdate);
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

        public static int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimTaxTypeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimTaxTypeTO dimTaxTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimTaxType] SET " + 
            "  [idTaxType] = @IdTaxType" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[taxTypeDesc] = @TaxTypeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTaxType", System.Data.SqlDbType.Int).Value = dimTaxTypeTO.IdTaxType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimTaxTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimTaxTypeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@TaxTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimTaxTypeTO.TaxTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimTaxType(Int32 idTaxType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTaxType, cmdDelete);
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

        public static int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTaxType, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idTaxType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimTaxType] " +
            " WHERE idTaxType = " + idTaxType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTaxType", System.Data.SqlDbType.Int).Value = dimTaxTypeTO.IdTaxType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
