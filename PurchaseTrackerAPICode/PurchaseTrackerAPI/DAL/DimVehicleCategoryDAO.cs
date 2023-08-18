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
    public class DimVehicleCategoryDAO : IDimVehicleCategoryDAO
    {

        private readonly IConnectionString _iConnectionString;

        private readonly Icommondao _iCommon;
       
        public DimVehicleCategoryDAO(IConnectionString iConnectionString, Icommondao common)
        {
            _iConnectionString = iConnectionString;
            _iCommon = common;

        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimVehicleCategory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DimVehicleCategoryTO> SelectAllDimVehicleCategory()
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

                //cmdSelect.Parameters.Add("@idVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehicleCategoryTO> list = ConvertDTToList(rdr);
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

        public  DimVehicleCategoryTO SelectDimVehicleCategory(Int32 idVehicleCategory)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehicleCategory = " + idVehicleCategory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehicleCategoryTO> list = ConvertDTToList(rdr);
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

        public  List<DimVehicleCategoryTO> SelectAllDimVehicleCategory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehicleCategoryTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
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
        public  List<DimVehicleCategoryTO> ConvertDTToList(SqlDataReader dimVehicleCategoryTODT )
        {
            List<DimVehicleCategoryTO> dimVehicleCategoryTOList = new List<DimVehicleCategoryTO>();
            if (dimVehicleCategoryTODT != null)
            {
               while (dimVehicleCategoryTODT.Read())
                {
                    DimVehicleCategoryTO dimVehicleCategoryTONew = new DimVehicleCategoryTO();
                    if(dimVehicleCategoryTODT["idVehicleCategory"] != DBNull.Value)
                        dimVehicleCategoryTONew.IdVehicleCategory = Convert.ToInt32( dimVehicleCategoryTODT["idVehicleCategory"].ToString());
                    if(dimVehicleCategoryTODT["vehicleCatName"] != DBNull.Value)
                        dimVehicleCategoryTONew.VehicleCatName = Convert.ToString( dimVehicleCategoryTODT["vehicleCatName"].ToString());
                    dimVehicleCategoryTOList.Add(dimVehicleCategoryTONew);
                }
            }
            return dimVehicleCategoryTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimVehicleCategoryTO, cmdInsert);
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

        public  int InsertDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimVehicleCategoryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimVehicleCategoryTO dimVehicleCategoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimVehicleCategory]( " + 
            "  [idVehicleCategory]" +
            " ,[vehicleCatName]" +
            " )" +
" VALUES (" +
            "  @IdVehicleCategory " +
            " ,@VehicleCatName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
            cmdInsert.Parameters.Add("@VehicleCatName", System.Data.SqlDbType.VarChar).Value = dimVehicleCategoryTO.VehicleCatName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimVehicleCategoryTO, cmdUpdate);
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

        public  int UpdateDimVehicleCategory(DimVehicleCategoryTO dimVehicleCategoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimVehicleCategoryTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimVehicleCategoryTO dimVehicleCategoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimVehicleCategory] SET " + 
            "  [idVehicleCategory] = @IdVehicleCategory" +
            " ,[vehicleCatName] = @VehicleCatName" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
            cmdUpdate.Parameters.Add("@VehicleCatName", System.Data.SqlDbType.VarChar).Value = dimVehicleCategoryTO.VehicleCatName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimVehicleCategory(Int32 idVehicleCategory)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehicleCategory, cmdDelete);
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

        public  int DeleteDimVehicleCategory(Int32 idVehicleCategory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehicleCategory, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idVehicleCategory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimVehicleCategory] " +
            " WHERE idVehicleCategory = " + idVehicleCategory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehicleCategory", System.Data.SqlDbType.Int).Value = dimVehicleCategoryTO.IdVehicleCategory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
