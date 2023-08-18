using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
    public class DimVehicleTypeDAO : IDimVehicleTypeDAO
    {

        private readonly IConnectionString _iConnectionString;
        public DimVehicleTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimVehicleType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DimVehicleTypeTO> SelectAllDimVehicleType()
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                List<DimVehicleTypeTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  DimVehicleTypeTO SelectDimVehicleType(Int32 idVehicleType)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehicleType = " + idVehicleType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehicleTypeTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<DimVehicleTypeTO> ConvertDTToList(SqlDataReader dimVehicleTypeTODT)
        {
            List<DimVehicleTypeTO> dimVehicleTypeTOList = new List<DimVehicleTypeTO>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    DimVehicleTypeTO dimVehicleTypeTONew = new DimVehicleTypeTO();
                    if (dimVehicleTypeTODT["idVehicleType"] != DBNull.Value)
                        dimVehicleTypeTONew.IdVehicleType = Convert.ToInt32(dimVehicleTypeTODT["idVehicleType"].ToString());
                    if (dimVehicleTypeTODT["vehicleTypeDesc"] != DBNull.Value)
                        dimVehicleTypeTONew.VehicleTypeDesc = Convert.ToString(dimVehicleTypeTODT["vehicleTypeDesc"].ToString());
                    dimVehicleTypeTOList.Add(dimVehicleTypeTONew);
                }
            }
            return dimVehicleTypeTOList;
        }

        #endregion

        #region Insertion
        public  int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimVehicleTypeTO, cmdInsert);
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

        public  int InsertDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimVehicleTypeTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimVehicleTypeTO dimVehicleTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimVehicleType]( " + 
            "  [idVehicleType]" +
            " ,[vehicleTypeDesc]" +
            " )" +
" VALUES (" +
            "  @IdVehicleType " +
            " ,@VehicleTypeDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVehicleType", System.Data.SqlDbType.Int).Value = dimVehicleTypeTO.IdVehicleType;
            cmdInsert.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimVehicleTypeTO.VehicleTypeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimVehicleTypeTO, cmdUpdate);
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

        public  int UpdateDimVehicleType(DimVehicleTypeTO dimVehicleTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimVehicleTypeTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimVehicleTypeTO dimVehicleTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimVehicleType] SET " + 
            "  [idVehicleType] = @IdVehicleType" +
            " ,[vehicleTypeDesc] = @VehicleTypeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehicleType", System.Data.SqlDbType.Int).Value = dimVehicleTypeTO.IdVehicleType;
            cmdUpdate.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimVehicleTypeTO.VehicleTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimVehicleType(Int32 idVehicleType)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehicleType, cmdDelete);
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

        public  int DeleteDimVehicleType(Int32 idVehicleType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehicleType, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idVehicleType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimVehicleType] " +
            " WHERE idVehicleType = " + idVehicleType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehicleType", System.Data.SqlDbType.Int).Value = dimVehicleTypeTO.IdVehicleType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
