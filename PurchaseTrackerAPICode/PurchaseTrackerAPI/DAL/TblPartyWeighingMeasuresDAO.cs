using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPartyWeighingMeasuresDAO : ITblPartyWeighingMeasuresDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblPartyWeighingMeasuresDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPartyWeighingMeasures]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                List<TblPartyWeighingMeasuresTO> list = ConvertDTToList(reader);
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

        public  TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPartyWeighingMeasures = " + idPartyWeighingMeasures +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPartyWeighingMeasuresTO> list = ConvertDTToList(reader);
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

        //Priyanka [12-02-2019]
        public  TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId = " + purchaseScheduleSummaryId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPartyWeighingMeasuresTO> list = ConvertDTToList(reader);
                if (list != null && list.Count>0)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  TblPartyWeighingMeasuresTO SelectTblPartyWeighingMeasuresTOByPurSchedSummaryId(Int32 purchaseScheduleSummaryId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId = " + purchaseScheduleSummaryId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPartyWeighingMeasuresTO> list = ConvertDTToList(reader);
                if (list != null && list.Count>0)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPartyWeighingMeasuresTO> SelectAllTblPartyWeighingMeasures(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPartyWeighingMeasures", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.IdPartyWeighingMeasures;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPartyWeighingMeasuresTO> list = ConvertDTToList(reader);
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

        public  List<TblPartyWeighingMeasuresTO> ConvertDTToList(SqlDataReader tblPartyWeighingMeasuresTODT)
        {
            List<TblPartyWeighingMeasuresTO> tblPartyWeighingMeasuresTOList = new List<TblPartyWeighingMeasuresTO>();
            if (tblPartyWeighingMeasuresTODT != null)
            {
                while (tblPartyWeighingMeasuresTODT.Read())
                {
                    TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTONew = new TblPartyWeighingMeasuresTO();
                    if (tblPartyWeighingMeasuresTODT["idPartyWeighingMeasures"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.IdPartyWeighingMeasures = Convert.ToInt32(tblPartyWeighingMeasuresTODT["idPartyWeighingMeasures"].ToString());
                    if (tblPartyWeighingMeasuresTODT["weighingMachineId"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.WeighingMachineId = Convert.ToInt32(tblPartyWeighingMeasuresTODT["weighingMachineId"].ToString());
                    if (tblPartyWeighingMeasuresTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPartyWeighingMeasuresTODT["purchaseScheduleSummaryId"].ToString());
                    if (tblPartyWeighingMeasuresTODT["weighingMeasureTypeId"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.WeighingMeasureTypeId = Convert.ToInt32(tblPartyWeighingMeasuresTODT["weighingMeasureTypeId"].ToString());
                    if (tblPartyWeighingMeasuresTODT["prodClassId"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.ProdClassId = Convert.ToInt32(tblPartyWeighingMeasuresTODT["prodClassId"].ToString());
                    if (tblPartyWeighingMeasuresTODT["vehicleTypeId"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.VehicleTypeId = Convert.ToInt32(tblPartyWeighingMeasuresTODT["vehicleTypeId"].ToString());
                    if (tblPartyWeighingMeasuresTODT["createdBy"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.CreatedBy = Convert.ToInt32(tblPartyWeighingMeasuresTODT["createdBy"].ToString());
                    if (tblPartyWeighingMeasuresTODT["updatedBy"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.UpdatedBy = Convert.ToInt32(tblPartyWeighingMeasuresTODT["updatedBy"].ToString());
                    if (tblPartyWeighingMeasuresTODT["createdOn"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.CreatedOn = Convert.ToDateTime(tblPartyWeighingMeasuresTODT["createdOn"].ToString());
                    if (tblPartyWeighingMeasuresTODT["updatedOn"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.UpdatedOn = Convert.ToDateTime(tblPartyWeighingMeasuresTODT["updatedOn"].ToString());
                    if (tblPartyWeighingMeasuresTODT["tareWt"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.TareWt = Convert.ToInt64(tblPartyWeighingMeasuresTODT["tareWt"].ToString());
                    if (tblPartyWeighingMeasuresTODT["netWt"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.NetWt = Convert.ToInt64(tblPartyWeighingMeasuresTODT["netWt"].ToString());
                    if (tblPartyWeighingMeasuresTODT["grossWt"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.GrossWt = Convert.ToInt64(tblPartyWeighingMeasuresTODT["grossWt"].ToString());
                    if (tblPartyWeighingMeasuresTODT["vehicleNo"] != DBNull.Value)
                        tblPartyWeighingMeasuresTONew.VehicleNo = Convert.ToString(tblPartyWeighingMeasuresTODT["vehicleNo"].ToString());
                    tblPartyWeighingMeasuresTOList.Add(tblPartyWeighingMeasuresTONew);
                }
            }
            return tblPartyWeighingMeasuresTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPartyWeighingMeasuresTO, cmdInsert);
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

        public  int InsertTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPartyWeighingMeasuresTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPartyWeighingMeasures]( " + 
            //"  [idPartyWeighingMeasures]" +
            "  [weighingMachineId]" +
            " ,[purchaseScheduleSummaryId]" +
            " ,[weighingMeasureTypeId]" +
            " ,[prodClassId]" +
            " ,[vehicleTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[tareWt]" +
            " ,[netWt]" +
            " ,[grossWt]" +
            " ,[vehicleNo]" +
            " )" +
" VALUES (" +
           // "  @IdPartyWeighingMeasures " +
            "  @WeighingMachineId " +
            " ,@PurchaseScheduleSummaryId " +
            " ,@WeighingMeasureTypeId " +
            " ,@ProdClassId " +
            " ,@VehicleTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@TareWt " +
            " ,@NetWt " +
            " ,@GrossWt " +
            " ,@VehicleNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdPartyWeighingMeasures", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.IdPartyWeighingMeasures;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.WeighingMachineId);
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@WeighingMeasureTypeId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.WeighingMeasureTypeId);
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.ProdClassId);
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.VehicleTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPartyWeighingMeasuresTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.UpdatedOn);
            cmdInsert.Parameters.Add("@TareWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.TareWt;
            cmdInsert.Parameters.Add("@NetWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.NetWt;
            cmdInsert.Parameters.Add("@GrossWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.GrossWt;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPartyWeighingMeasuresTO.VehicleNo;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPartyWeighingMeasuresTO, cmdUpdate);
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

        public  int UpdateTblPartyWeighingMeasures(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPartyWeighingMeasuresTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPartyWeighingMeasuresTO tblPartyWeighingMeasuresTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPartyWeighingMeasures] SET " + 
            //"  [idPartyWeighingMeasures] = @IdPartyWeighingMeasures" +
            " [weighingMachineId]= @WeighingMachineId" +
            " ,[purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[weighingMeasureTypeId]= @WeighingMeasureTypeId" +
            " ,[prodClassId]= @ProdClassId" +
            " ,[vehicleTypeId]= @VehicleTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[tareWt]= @TareWt" +
            " ,[netWt]= @NetWt" +
            " ,[grossWt]= @GrossWt" +
            " ,[vehicleNo] = @VehicleNo" +
            " WHERE  [idPartyWeighingMeasures] = @IdPartyWeighingMeasures "; 

            cmdUpdate.CommandText = sqlQuery;//
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPartyWeighingMeasures", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.IdPartyWeighingMeasures;
            cmdUpdate.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.WeighingMachineId);
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@WeighingMeasureTypeId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.WeighingMeasureTypeId);
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.ProdClassId);
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPartyWeighingMeasuresTO.VehicleTypeId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPartyWeighingMeasuresTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPartyWeighingMeasuresTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@TareWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.TareWt;
            cmdUpdate.Parameters.Add("@NetWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.NetWt;
            cmdUpdate.Parameters.Add("@GrossWt", System.Data.SqlDbType.BigInt).Value = tblPartyWeighingMeasuresTO.GrossWt;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblPartyWeighingMeasuresTO.VehicleNo;     
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPartyWeighingMeasures, cmdDelete);
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

        public  int DeleteTblPartyWeighingMeasures(Int32 idPartyWeighingMeasures, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPartyWeighingMeasures, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPartyWeighingMeasures, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPartyWeighingMeasures] " +
            " WHERE idPartyWeighingMeasures = " + idPartyWeighingMeasures +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPartyWeighingMeasures", System.Data.SqlDbType.Int).Value = tblPartyWeighingMeasuresTO.IdPartyWeighingMeasures;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllPartyWeighingDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPartyWeighingMeasures WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
