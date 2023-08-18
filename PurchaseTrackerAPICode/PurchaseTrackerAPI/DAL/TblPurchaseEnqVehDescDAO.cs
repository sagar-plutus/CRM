using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseEnqVehDescDAO : ITblPurchaseEnqVehDescDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblPurchaseEnqVehDescDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseEnqVehDesc]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseEnqVehDescTO>  SelectAllTblPurchaseEnqVehDesc()
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

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnqVehDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnqVehDescTO> SelectTblPurchaseEnqVehDesc(Int32 idVehTypeDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehTypeDesc = " + idVehTypeDesc +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnqVehDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnqVehDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseEnqId = " + purchaseEnqId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnqVehDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseEnqVehDescTO> SelectAllTblPurchaseEnqVehDesc(Int32 purchaseEnqId,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseEnqId = " + purchaseEnqId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseEnqVehDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseEnqVehDescTO> ConvertDTToList(SqlDataReader tblPurchaseEnqVehDescTODT)
        {
            List<TblPurchaseEnqVehDescTO> tblPurchaseEnqVehDescTOList = new List<TblPurchaseEnqVehDescTO>();
            if (tblPurchaseEnqVehDescTODT != null)
            {
                while(tblPurchaseEnqVehDescTODT.Read())
                {
                    TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTONew = new TblPurchaseEnqVehDescTO();
                    if (tblPurchaseEnqVehDescTODT ["idVehTypeDesc"] != DBNull.Value)
                        tblPurchaseEnqVehDescTONew.IdVehTypeDesc = Convert.ToInt32(tblPurchaseEnqVehDescTODT ["idVehTypeDesc"].ToString());
                    if (tblPurchaseEnqVehDescTODT ["purchaseEnqId"] != DBNull.Value)
                        tblPurchaseEnqVehDescTONew.PurchaseEnqId = Convert.ToInt32(tblPurchaseEnqVehDescTODT ["purchaseEnqId"].ToString());
                    if (tblPurchaseEnqVehDescTODT ["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseEnqVehDescTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseEnqVehDescTODT ["vehicleTypeId"].ToString());
                    if (tblPurchaseEnqVehDescTODT ["vehicleTypeDesc"] != DBNull.Value)
                        tblPurchaseEnqVehDescTONew.VehicleTypeDesc = Convert.ToString(tblPurchaseEnqVehDescTODT ["vehicleTypeDesc"].ToString());
                    tblPurchaseEnqVehDescTOList.Add(tblPurchaseEnqVehDescTONew);
                }
            }
            return tblPurchaseEnqVehDescTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseEnqVehDescTO, cmdInsert);
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

        public int InsertTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseEnqVehDescTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseEnqVehDesc]( " + 
            //"  [idVehTypeDesc]" +
            "  [purchaseEnqId]" +
            " ,[vehicleTypeId]" +
            " ,[vehicleTypeDesc]" +
            " )" +
" VALUES (" +
            //"  @IdVehTypeDesc " +
            "  @PurchaseEnqId " +
            " ,@VehicleTypeId " +
            " ,@VehicleTypeDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVehTypeDesc", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.IdVehTypeDesc;
            cmdInsert.Parameters.Add("@PurchaseEnqId", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.PurchaseEnqId;
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.VehicleTypeId;
            cmdInsert.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnqVehDescTO.VehicleTypeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseEnqVehDescTO, cmdUpdate);
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

        public int UpdateTblPurchaseEnqVehDesc(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseEnqVehDescTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseEnqVehDescTO tblPurchaseEnqVehDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseEnqVehDesc] SET " + 
            //"  [idVehTypeDesc] = @IdVehTypeDesc" +
            "  [purchaseEnqId]= @PurchaseEnqId" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            " ,[vehicleTypeDesc] = @VehicleTypeDesc" +
            " WHERE idVehTypeDesc = @IdVehTypeDesc "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehTypeDesc", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.IdVehTypeDesc;
            cmdUpdate.Parameters.Add("@PurchaseEnqId", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.PurchaseEnqId;
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.VehicleTypeId;
            cmdUpdate.Parameters.Add("@VehicleTypeDesc", System.Data.SqlDbType.NVarChar).Value = tblPurchaseEnqVehDescTO.VehicleTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehTypeDesc, cmdDelete);
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

        public int DeleteTblPurchaseEnqVehDesc(Int32 idVehTypeDesc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehTypeDesc, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVehTypeDesc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseEnqVehDesc] " +
            " WHERE idVehTypeDesc = " + idVehTypeDesc +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehTypeDesc", System.Data.SqlDbType.Int).Value = tblPurchaseEnqVehDescTO.IdVehTypeDesc;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeletePurchVehDesc(Int32 purchaseEnqId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseEnqVehDesc WHERE purchaseEnqId=" + purchaseEnqId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        #endregion

    }
}
