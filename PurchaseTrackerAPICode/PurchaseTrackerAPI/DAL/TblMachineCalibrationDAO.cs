using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblMachineCalibrationDAO : ITblMachineCalibrationDAO
    {

        private readonly Icommondao _iCommonDAO;
        private readonly IConnectionString _iConnectionString;
        public TblMachineCalibrationDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblMachineCalibration]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblMachineCalibrationTO> SelectAllTblMachineCalibration()
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

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMachineCalibrationTO> list = ConvertDTToList(rdr);
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

        public TblMachineCalibrationTO SelectTblMachineCalibration(Int32 idMachineCalibration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idMachineCalibration = " + idMachineCalibration +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMachineCalibrationTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                if (list != null && list.Count ==1)
                    return list[0];
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

        public List<TblMachineCalibrationTO> SelectAllTblMachineCalibration(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMachineCalibrationTO> list = ConvertDTToList(rdr);
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

        public TblMachineCalibrationTO SelectTblMachineCalibrationByWeighingMachineId(Int32 weighingMachineId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE weighingMachineId = " + weighingMachineId + " ORDER BY idMachineCalibration desc ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMachineCalibrationTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                if (list != null && list.Count > 0)
                {
                   
                    return list[0];
                }
                    
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

        public List<TblMachineCalibrationTO> ConvertDTToList(SqlDataReader tblMachineCalibrationTODT)
        {
            List<TblMachineCalibrationTO> tblMachineCalibrationTOList = new List<TblMachineCalibrationTO>();
            if (tblMachineCalibrationTODT != null)
            {
                while (tblMachineCalibrationTODT.Read())
                {
                    TblMachineCalibrationTO tblMachineCalibrationTONew = new TblMachineCalibrationTO();
                    if (tblMachineCalibrationTODT["idMachineCalibration"] != DBNull.Value)
                        tblMachineCalibrationTONew.IdMachineCalibration = Convert.ToInt32(tblMachineCalibrationTODT["idMachineCalibration"].ToString());
                    if (tblMachineCalibrationTODT["weighingMachineId"] != DBNull.Value)
                        tblMachineCalibrationTONew.WeighingMachineId = Convert.ToInt32(tblMachineCalibrationTODT["weighingMachineId"].ToString());
                    if (tblMachineCalibrationTODT["calibrationValue"] != DBNull.Value)
                        tblMachineCalibrationTONew.CalibrationValue = Convert.ToDouble(tblMachineCalibrationTODT["calibrationValue"].ToString());
                    if (tblMachineCalibrationTODT["createdOn"] != DBNull.Value)
                        tblMachineCalibrationTONew.CreatedOn = Convert.ToDateTime(tblMachineCalibrationTODT["createdOn"].ToString());
                    if (tblMachineCalibrationTODT["createdBy"] != DBNull.Value)
                        tblMachineCalibrationTONew.CreatedBy = Convert.ToInt32(tblMachineCalibrationTODT["createdBy"].ToString());
                    if (tblMachineCalibrationTODT["updatedOn"] != DBNull.Value)
                        tblMachineCalibrationTONew.UpdatedOn = Convert.ToDateTime(tblMachineCalibrationTODT["updatedOn"].ToString());
                    if (tblMachineCalibrationTODT["updatedBy"] != DBNull.Value)
                        tblMachineCalibrationTONew.UpdatedBy = Convert.ToInt32(tblMachineCalibrationTODT["updatedBy"].ToString());
                    tblMachineCalibrationTOList.Add(tblMachineCalibrationTONew);
                }
            }
            return tblMachineCalibrationTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblMachineCalibrationTO, cmdInsert);
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

        public  int InsertTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblMachineCalibrationTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblMachineCalibrationTO tblMachineCalibrationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblMachineCalibration]( " + 
                            "  [weighingMachineId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[calibrationValue]" +
                            " )" +
                " VALUES (" +
                            " @WeighingMachineId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@CalibrationValue " + 
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdMachineCalibration", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.IdMachineCalibration;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblMachineCalibrationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value =  _iCommonDAO.ServerDateTime;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =  _iCommonDAO.ServerDateTime;
            cmdInsert.Parameters.Add("@CalibrationValue", System.Data.SqlDbType.Decimal).Value = tblMachineCalibrationTO.CalibrationValue;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblMachineCalibrationTO.IdMachineCalibration = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblMachineCalibrationTO, cmdUpdate);
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

        public  int UpdateTblMachineCalibration(TblMachineCalibrationTO tblMachineCalibrationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblMachineCalibrationTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblMachineCalibrationTO tblMachineCalibrationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblMachineCalibration] SET " + 
                            " [weighingMachineId]= @WeighingMachineId" +
                            " ,[createdBy]= @CreatedBy" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[createdOn]= @CreatedOn" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[calibrationValue] = @CalibrationValue" +
                            " WHERE  [idMachineCalibration]  = @IdMachineCalibration "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdMachineCalibration", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.IdMachineCalibration;
            cmdUpdate.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.WeighingMachineId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblMachineCalibrationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblMachineCalibrationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CalibrationValue", System.Data.SqlDbType.Decimal).Value = tblMachineCalibrationTO.CalibrationValue;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblMachineCalibration(Int32 idMachineCalibration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idMachineCalibration, cmdDelete);
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

        public  int DeleteTblMachineCalibration(Int32 idMachineCalibration, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idMachineCalibration, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idMachineCalibration, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblMachineCalibration] " +
            " WHERE idMachineCalibration = " + idMachineCalibration +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idMachineCalibration", System.Data.SqlDbType.Int).Value = tblMachineCalibrationTO.IdMachineCalibration;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
