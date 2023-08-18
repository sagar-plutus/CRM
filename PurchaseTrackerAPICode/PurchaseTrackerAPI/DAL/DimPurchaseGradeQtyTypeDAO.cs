using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class DimPurchaseGradeQtyTypeDAO : IDimPurchaseGradeQtyTypeDAO
    {
        private readonly IConnectionString _iConnectionString;

        public DimPurchaseGradeQtyTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimPurchaseGradeQtyType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimPurchaseGradeQtyTypeTO> SelectAllDimPurchaseGradeQtyType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPurchaseGradeQtyTypeTO> list = ConvertDTToList(rdr);
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

        public List<DimPurchaseGradeQtyTypeTO> SelectDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseGradeQtyType = " + idPurchaseGradeQtyType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimPurchaseGradeQtyTypeTO> list = ConvertDTToList(rdr);
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

        public DataTable SelectAllDimPurchaseGradeQtyType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
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

        public List<DimPurchaseGradeQtyTypeTO> ConvertDTToList(SqlDataReader dimPurchaseGradeQtyTypeTODT)
        {
            List<DimPurchaseGradeQtyTypeTO> dimPurchaseGradeQtyTypeTOList = new List<DimPurchaseGradeQtyTypeTO>();
            if (dimPurchaseGradeQtyTypeTODT != null)
            {
                while (dimPurchaseGradeQtyTypeTODT.Read())
                {
                    DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTONew = new DimPurchaseGradeQtyTypeTO();
                    if (dimPurchaseGradeQtyTypeTODT["idPurchaseGradeQtyType"] != DBNull.Value)
                        dimPurchaseGradeQtyTypeTONew.IdPurchaseGradeQtyType = Convert.ToInt32(dimPurchaseGradeQtyTypeTODT["idPurchaseGradeQtyType"].ToString());
                    if (dimPurchaseGradeQtyTypeTODT["isActive"] != DBNull.Value)
                        dimPurchaseGradeQtyTypeTONew.IsActive = Convert.ToInt32(dimPurchaseGradeQtyTypeTODT["isActive"].ToString());
                    if (dimPurchaseGradeQtyTypeTODT["purchaseGradeQtyType"] != DBNull.Value)
                        dimPurchaseGradeQtyTypeTONew.PurchaseGradeQtyType = Convert.ToString(dimPurchaseGradeQtyTypeTODT["purchaseGradeQtyType"].ToString());
                    dimPurchaseGradeQtyTypeTOList.Add(dimPurchaseGradeQtyTypeTONew);
                }
            }
            return dimPurchaseGradeQtyTypeTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimPurchaseGradeQtyTypeTO, cmdInsert);
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

        public int InsertDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimPurchaseGradeQtyTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimPurchaseGradeQtyType]( " + 
            "  [idPurchaseGradeQtyType]" +
            " ,[isActive]" +
            " ,[purchaseGradeQtyType]" +
            " )" +
" VALUES (" +
            "  @IdPurchaseGradeQtyType " +
            " ,@IsActive " +
            " ,@PurchaseGradeQtyType " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IsActive;
            cmdInsert.Parameters.Add("@PurchaseGradeQtyType", System.Data.SqlDbType.NVarChar).Value = dimPurchaseGradeQtyTypeTO.PurchaseGradeQtyType;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimPurchaseGradeQtyTypeTO, cmdUpdate);
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

        public int UpdateDimPurchaseGradeQtyType(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimPurchaseGradeQtyTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimPurchaseGradeQtyTypeTO dimPurchaseGradeQtyTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimPurchaseGradeQtyType] SET " + 
            "  [idPurchaseGradeQtyType] = @IdPurchaseGradeQtyType" +
            " ,[isActive]= @IsActive" +
            " ,[purchaseGradeQtyType] = @PurchaseGradeQtyType" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@PurchaseGradeQtyType", System.Data.SqlDbType.NVarChar).Value = dimPurchaseGradeQtyTypeTO.PurchaseGradeQtyType;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseGradeQtyType, cmdDelete);
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

        public int DeleteDimPurchaseGradeQtyType(Int32 idPurchaseGradeQtyType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseGradeQtyType, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseGradeQtyType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimPurchaseGradeQtyType] " +
            " WHERE idPurchaseGradeQtyType = " + idPurchaseGradeQtyType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseGradeQtyType", System.Data.SqlDbType.Int).Value = dimPurchaseGradeQtyTypeTO.IdPurchaseGradeQtyType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
