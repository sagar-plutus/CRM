using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI
{
    public class TblSpotEntryContainerDtlsDAO: ITblSpotEntryContainerDtlsDAO
    {
        #region Methods
        private readonly IConnectionString _iConnectionString;
        public TblSpotEntryContainerDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblSpotEntryContainerDtls]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls()
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

                //cmdSelect.Parameters.Add("@idSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotEntryContainerDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public TblSpotEntryContainerDtlsTO SelectTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSpotEntryContainerDtls = " + idSpotEntryContainerDtls +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotEntryContainerDtlsTO> tblSpotEntryContainerDtlsTOList = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (tblSpotEntryContainerDtlsTOList != null && tblSpotEntryContainerDtlsTOList.Count == 1)
                    return tblSpotEntryContainerDtlsTOList[0];
                else
                    return null;
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

        public List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls(Int32 spotEntryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE vehicleSpotEntryId = " + spotEntryId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotEntryContainerDtlsTO> tblSpotEntryContainerDtlsTOList = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (tblSpotEntryContainerDtlsTOList != null && tblSpotEntryContainerDtlsTOList.Count >0)
                    return tblSpotEntryContainerDtlsTOList;
                else
                    return null;
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

        public List<TblSpotEntryContainerDtlsTO> SelectAllTblSpotEntryContainerDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSpotEntryContainerDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static List<TblSpotEntryContainerDtlsTO> ConvertDTToList(SqlDataReader tblSpotEntryContainerDtlsTODT)
        {
            List<TblSpotEntryContainerDtlsTO> tblSpotEntryContainerDtlsTOList = new List<TblSpotEntryContainerDtlsTO>();
            if (tblSpotEntryContainerDtlsTODT != null)
            {
                while (tblSpotEntryContainerDtlsTODT.Read())
                {
                    TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTONew = new TblSpotEntryContainerDtlsTO();
                    if (tblSpotEntryContainerDtlsTODT["idSpotEntryContainerDtls"] != DBNull.Value)
                        tblSpotEntryContainerDtlsTONew.IdSpotEntryContainerDtls = Convert.ToInt32(tblSpotEntryContainerDtlsTODT["idSpotEntryContainerDtls"].ToString());
                    if (tblSpotEntryContainerDtlsTODT["vehicleSpotEntryId"] != DBNull.Value)
                        tblSpotEntryContainerDtlsTONew.VehicleSpotEntryId = Convert.ToInt32(tblSpotEntryContainerDtlsTODT["vehicleSpotEntryId"].ToString());
                    if (tblSpotEntryContainerDtlsTODT["isActive"] != DBNull.Value)
                        tblSpotEntryContainerDtlsTONew.IsActive = Convert.ToInt32(tblSpotEntryContainerDtlsTODT["isActive"].ToString());
                    if (tblSpotEntryContainerDtlsTODT["containerNo"] != DBNull.Value)
                        tblSpotEntryContainerDtlsTONew.ContainerNo = Convert.ToString(tblSpotEntryContainerDtlsTODT["containerNo"].ToString());
                    tblSpotEntryContainerDtlsTOList.Add(tblSpotEntryContainerDtlsTONew);
                }
            }
            return tblSpotEntryContainerDtlsTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSpotEntryContainerDtlsTO, cmdInsert);
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

        public  int InsertTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSpotEntryContainerDtlsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSpotEntryContainerDtls]( " + 
            "  [vehicleSpotEntryId]" +
            " ,[isActive]" +
            " ,[containerNo]" +
            " )" +
" VALUES (" +
            "  @VehicleSpotEntryId " +
            " ,@IsActive " +
            " ,@ContainerNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
            cmdInsert.Parameters.Add("@VehicleSpotEntryId", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.VehicleSpotEntryId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = tblSpotEntryContainerDtlsTO.ContainerNo;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSpotEntryContainerDtlsTO, cmdUpdate);
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

        public  int UpdateTblSpotEntryContainerDtls(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSpotEntryContainerDtlsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblSpotEntryContainerDtlsTO tblSpotEntryContainerDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSpotEntryContainerDtls] SET " + 
            "  [vehicleSpotEntryId]= @VehicleSpotEntryId" +
            " ,[isActive]= @IsActive" +
            " ,[containerNo] = @ContainerNo" +
            " WHERE 1 = 1 " ; 

            cmdUpdate.CommandText = sqlQuery + "[idSpotEntryContainerDtls] = @IdSpotEntryContainerDtls";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
            cmdUpdate.Parameters.Add("@VehicleSpotEntryId", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.VehicleSpotEntryId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = tblSpotEntryContainerDtlsTO.ContainerNo;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSpotEntryContainerDtls, cmdDelete);
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

        public  int DeleteTblSpotEntryContainerDtls(Int32 idSpotEntryContainerDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSpotEntryContainerDtls, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idSpotEntryContainerDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSpotEntryContainerDtls] " +
            " WHERE idSpotEntryContainerDtls = " + idSpotEntryContainerDtls +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSpotEntryContainerDtls", System.Data.SqlDbType.Int).Value = tblSpotEntryContainerDtlsTO.IdSpotEntryContainerDtls;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
