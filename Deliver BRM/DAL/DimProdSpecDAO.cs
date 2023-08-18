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
    public class DimProdSpecDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimProdSpec]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<DimProdSpecTO> SelectAllDimProdSpec()
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
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

        public static DimProdSpecTO SelectDimProdSpec(Int32 idProdSpec)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdSpec = " + idProdSpec +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DimProdSpecTO> ConvertDTToList(SqlDataReader dimProdSpecTODT)
        {
            List<DimProdSpecTO> dimProdSpecTOList = new List<DimProdSpecTO>();
            if (dimProdSpecTODT != null)
            {
                while (dimProdSpecTODT.Read())
                {
                    DimProdSpecTO dimProdSpecTONew = new DimProdSpecTO();
                    if (dimProdSpecTODT["idProdSpec"] != DBNull.Value)
                        dimProdSpecTONew.IdProdSpec = Convert.ToInt32(dimProdSpecTODT["idProdSpec"].ToString());
                    if (dimProdSpecTODT["isActive"] != DBNull.Value)
                        dimProdSpecTONew.IsActive = Convert.ToInt32(dimProdSpecTODT["isActive"].ToString());
                    if (dimProdSpecTODT["prodSpecDesc"] != DBNull.Value)
                        dimProdSpecTONew.ProdSpecDesc = Convert.ToString(dimProdSpecTODT["prodSpecDesc"].ToString());
                    dimProdSpecTOList.Add(dimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }

        #endregion

        #region Insertion
        public static int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimProdSpecTO, cmdInsert);
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

        public static int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimProdSpecTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(DimProdSpecTO dimProdSpecTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimProdSpec]( " + 
            "  [idProdSpec]" +
            " ,[isActive]" +
            " ,[prodSpecDesc]" +
            " )" +
" VALUES (" +
            "  @IdProdSpec " +
            " ,@IsActive " +
            " ,@ProdSpecDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdProdSpec", System.Data.SqlDbType.Int).Value = dimProdSpecTO.IdProdSpec;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimProdSpecTO.IsActive;
            cmdInsert.Parameters.Add("@ProdSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecTO.ProdSpecDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimProdSpecTO, cmdUpdate);
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

        public static int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimProdSpecTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(DimProdSpecTO dimProdSpecTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdSpec] SET " + 
            "  [idProdSpec] = @IdProdSpec" +
            " ,[isActive]= @IsActive" +
            " ,[prodSpecDesc] = @ProdSpecDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdSpec", System.Data.SqlDbType.Int).Value = dimProdSpecTO.IdProdSpec;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimProdSpecTO.IsActive;
            cmdUpdate.Parameters.Add("@ProdSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecTO.ProdSpecDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteDimProdSpec(Int32 idProdSpec)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdSpec, cmdDelete);
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

        public static int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdSpec, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idProdSpec, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimProdSpec] " +
            " WHERE idProdSpec = " + idProdSpec +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdSpec", System.Data.SqlDbType.Int).Value = dimProdSpecTO.IdProdSpec;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
