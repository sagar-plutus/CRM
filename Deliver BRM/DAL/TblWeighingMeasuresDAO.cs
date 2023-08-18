using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblWeighingMeasuresDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempWeighingMeasures]" +

                                 // Vaibhav [20-Nov-2017] Added to select from finalWeighingMeasures

                                 " UNION ALL " +
                                 " SELECT * FROM [finalWeighingMeasures]";


            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasures()
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
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
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

        /*GJ@20170810 : To get all weighing measures By Loading Id */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE 1 = 1" 
                    + " and loadingId = " + loadingId; 
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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

        public static Dictionary<Int32, Int32> SelectLoadingWiseRecordCountDCT()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT idLoading,COUNT(*) AS recordCount " +
                                        " FROM tempWeighingMeasures INNER JOIN tempLoading ON idLoading = loadingId " +
                                        " WHERE tempLoading.statusId = " + Constants.TranStatusE.LOADING_GATE_IN + " AND  weightMeasurTypeId !=" + Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT +
                                        " GROUP BY idLoading" +

                                        // Vaibhav [17-Jan-2018] added to select from final tables.
                                        " UNION ALL " +
                                        " SELECT idLoading,COUNT(*) AS recordCount " +
                                        " FROM finalWeighingMeasures INNER JOIN finalLoading ON idLoading = loadingId " +
                                        " WHERE finalLoading.statusId = " + Constants.TranStatusE.LOADING_GATE_IN + " AND  weightMeasurTypeId !=" + Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT +
                                        " GROUP BY idLoading";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                Dictionary<Int32, Int32> weightRecordsDCT = new Dictionary<int, int>();
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        Int32 loadingId = 0;
                        Int32 recordCount = 0;
                        if (sqlReader["idLoading"] != DBNull.Value)
                            loadingId = Convert.ToInt32(sqlReader["idLoading"].ToString());
                        if (sqlReader["recordCount"] != DBNull.Value)
                            recordCount = Convert.ToInt32(sqlReader["recordCount"].ToString());

                        weightRecordsDCT.Add(loadingId, recordCount);
                    }
                }

                return weightRecordsDCT;
            }
            catch (Exception ex)
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

        /*GJ@20170810 : To get all weighing measures By Loading Id */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        {           
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE 1 = 1"
                    + " and loadingId = " + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /*GJ@20170810 : To get all weighing measures By Loading Ids */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingIds(string loadingIds, Boolean isUnloading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string sqlQuery = string.Empty;
            try
            {
                sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE 1 = 1";
                if (!isUnloading)
                {
                    sqlQuery += " and loadingId IN ( " + loadingIds + ")";
                }
                else
                {
                    sqlQuery += " and unLoadingId IN ( " + loadingIds + ")";
                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        /*GJ@20170828 : To get all weighing measures By Vehicle No */
        public static List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT twm.* FROM tempWeighingMeasures twm " +
                                        " INNER JOIN tempLoading tl ON twm.loadingId = tl.idLoading " +
                                        " AND tl.statusId = " + Constants.TranStatusE.LOADING_COMPLETED +
                                        " AND tl.statusId NOT IN ("+ Constants.TranStatusE.LOADING_DELIVERED + ") " +
                                        " AND twm.vehicleNo = " + vehicleNo +

                                        // Vaibhav [20-Nov-2017] Added to select from finalLoading and finalWeighingMeasures

                                        " UNION ALL " +
                                        "SELECT twm.* FROM finalWeighingMeasures twm " +
                                        " INNER JOIN finalLoading tl ON twm.loadingId = tl.idLoading " +
                                        " AND tl.statusId = " + Constants.TranStatusE.LOADING_COMPLETED +
                                        " AND tl.statusId NOT IN (" + Constants.TranStatusE.LOADING_DELIVERED + ") " +
                                        " AND twm.vehicleNo = " + vehicleNo;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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

        public static TblWeighingMeasuresTO SelectTblWeighingMeasures(Int32 idWeightMeasure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idWeightMeasure = " + idWeightMeasure +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
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

        public static List<TblWeighingMeasuresTO> ConvertDTToList(SqlDataReader tblWeighingMeasuresTODT)
        {
            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            if (tblWeighingMeasuresTODT != null)
            {
                while (tblWeighingMeasuresTODT.Read())
                {
                    TblWeighingMeasuresTO tblWeighingMeasuresTONew = new TblWeighingMeasuresTO();
                    if (tblWeighingMeasuresTODT["idWeightMeasure"] != DBNull.Value)
                        tblWeighingMeasuresTONew.IdWeightMeasure = Convert.ToInt32(tblWeighingMeasuresTODT["idWeightMeasure"].ToString());
                    if (tblWeighingMeasuresTODT["weighingMachineId"] != DBNull.Value)
                        tblWeighingMeasuresTONew.WeighingMachineId = Convert.ToInt32(tblWeighingMeasuresTODT["weighingMachineId"].ToString());
                    if (tblWeighingMeasuresTODT["machineCalibrationId"] != DBNull.Value)
                        tblWeighingMeasuresTONew.MachineCalibrationId = Convert.ToInt32(tblWeighingMeasuresTODT["machineCalibrationId"].ToString());
                    if (tblWeighingMeasuresTODT["loadingId"] != DBNull.Value)
                        tblWeighingMeasuresTONew.LoadingId = Convert.ToInt32(tblWeighingMeasuresTODT["loadingId"].ToString());
                    if (tblWeighingMeasuresTODT["weightMeasurTypeId"] != DBNull.Value)
                        tblWeighingMeasuresTONew.WeightMeasurTypeId = Convert.ToInt32(tblWeighingMeasuresTODT["weightMeasurTypeId"].ToString());
                    if (tblWeighingMeasuresTODT["isConfirmed"] != DBNull.Value)
                        tblWeighingMeasuresTONew.IsConfirmed = Convert.ToInt32(tblWeighingMeasuresTODT["isConfirmed"].ToString());
                    if (tblWeighingMeasuresTODT["createdBy"] != DBNull.Value)
                        tblWeighingMeasuresTONew.CreatedBy = Convert.ToInt32(tblWeighingMeasuresTODT["createdBy"].ToString());
                    if (tblWeighingMeasuresTODT["updatedBy"] != DBNull.Value)
                        tblWeighingMeasuresTONew.UpdatedBy = Convert.ToInt32(tblWeighingMeasuresTODT["updatedBy"].ToString());
                    if (tblWeighingMeasuresTODT["createdOn"] != DBNull.Value)
                        tblWeighingMeasuresTONew.CreatedOn = Convert.ToDateTime(tblWeighingMeasuresTODT["createdOn"].ToString());
                    if (tblWeighingMeasuresTODT["updatedOn"] != DBNull.Value)
                        tblWeighingMeasuresTONew.UpdatedOn = Convert.ToDateTime(tblWeighingMeasuresTODT["updatedOn"].ToString());
                    if (tblWeighingMeasuresTODT["weightMT"] != DBNull.Value)
                        tblWeighingMeasuresTONew.WeightMT = Convert.ToDouble(tblWeighingMeasuresTODT["weightMT"].ToString());
                    if (tblWeighingMeasuresTODT["vehicleNo"] != DBNull.Value)
                        tblWeighingMeasuresTONew.VehicleNo = Convert.ToString(tblWeighingMeasuresTODT["vehicleNo"].ToString());
                    if (tblWeighingMeasuresTODT["unLoadingId"] != DBNull.Value)
                        tblWeighingMeasuresTONew.UnLoadingId = Convert.ToInt32(tblWeighingMeasuresTODT["unLoadingId"]);

                    tblWeighingMeasuresTOList.Add(tblWeighingMeasuresTONew);
                }
            }
            return tblWeighingMeasuresTOList;
        }


        public static List<TblWeghingMessureDtlsTO> ConvertDTToListDtls(SqlDataReader tblWeghingMessureDtlsTODT)
        {
            List<TblWeghingMessureDtlsTO> tblWeghingMessureDtlsTOList = new List<TblWeghingMessureDtlsTO>();
            if (tblWeghingMessureDtlsTODT != null)
            {
               while(tblWeghingMessureDtlsTODT.Read())
                { 
                    TblWeghingMessureDtlsTO tblWeghingMessureDtlsTONew = new TblWeghingMessureDtlsTO();
                    if (tblWeghingMessureDtlsTODT["idWeightMeasure"] != DBNull.Value)
                        tblWeghingMessureDtlsTONew.IdWeightMeasure = Convert.ToInt32(tblWeghingMessureDtlsTODT["idWeightMeasure"].ToString());
                    if (tblWeghingMessureDtlsTODT["weighingMachineId"] != DBNull.Value)
                        tblWeghingMessureDtlsTONew.WeighingMachineId = Convert.ToInt32(tblWeghingMessureDtlsTODT["weighingMachineId"].ToString());
                    if (tblWeghingMessureDtlsTODT["loadingId"] != DBNull.Value)
                        tblWeghingMessureDtlsTONew.LoadingId = Convert.ToInt32(tblWeghingMessureDtlsTODT["loadingId"].ToString());
                    if (tblWeghingMessureDtlsTODT["weightMeasurTypeId"] != DBNull.Value)
                        tblWeghingMessureDtlsTONew.WeightMeasurTypeId = Convert.ToInt32(tblWeghingMessureDtlsTODT["weightMeasurTypeId"].ToString());
                    if (tblWeghingMessureDtlsTODT["layerId"] != DBNull.Value)
                        tblWeghingMessureDtlsTONew.LayerId = Convert.ToInt32(tblWeghingMessureDtlsTODT["layerId"].ToString());
                    tblWeghingMessureDtlsTOList.Add(tblWeghingMessureDtlsTONew);
                }
            }
            return tblWeghingMessureDtlsTOList;
        }

        public static List<TblWeghingMessureDtlsTO> SelectAllTblWeghingMessureDtls()
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
                List<TblWeghingMessureDtlsTO> list = ConvertDTToListDtls(reader);
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
        public static List<TblWeghingMessureDtlsTO> SelectAllTblWeghingMessureDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeghingMessureDtlsTO> list = ConvertDTToListDtls(reader);
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

        public static List<TblWeighingMeasuresTO> SelectAllTempWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE 1 = 1"
                    + " and loadingId = " + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMeasuresTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        #endregion

        #region Insertion
        public static int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblWeighingMeasuresTO, cmdInsert);
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

        public static int InsertTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblWeighingMeasuresTO, cmdInsert);
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


        public static int InsertTblWeghingMessureDtls(TblWeghingMessureDtlsTO tblWeghingMessureDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = Startup.ConnectionString;
           // SqlConnection conn = conn;//new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                //conn.Open();
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForDtls(tblWeghingMessureDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                //conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommandForDtls(TblWeghingMessureDtlsTO tblWeghingMessureDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblWeghingMessureDtls]( " +
           // "  [idWeightMeasure]" +
            "[weighingMachineId]" +
            " ,[loadingId]" +
            " ,[weightMeasurTypeId]" +
            " ,[layerId]" +
            " )" +
" VALUES (" +
            //"  @IdWeightMeasure " +
            " @WeighingMachineId " +
            " ,@LoadingId " +
            " ,@WeightMeasurTypeId " +
            " ,@LayerId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdWeightMeasure", System.Data.SqlDbType.Int).Value = tblWeghingMessureDtlsTO.IdWeightMeasure;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblWeghingMessureDtlsTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblWeghingMessureDtlsTO.LoadingId;
            cmdInsert.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = tblWeghingMessureDtlsTO.WeightMeasurTypeId;
            cmdInsert.Parameters.Add("@LayerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeghingMessureDtlsTO.LayerId);
            return cmdInsert.ExecuteNonQuery();
        }

        public static int ExecuteInsertionCommand(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempWeighingMeasures]( " +
                               " [weighingMachineId]" +
                                " ,[loadingId]" +
                                " ,[weightMeasurTypeId]" +
                                " ,[isConfirmed]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[weightMT]" +
                                " ,[vehicleNo]" +
                                " ,[machineCalibrationId]"+
                                " ,[unLoadingId]" +
                                " )" +
                    " VALUES (" +
                               " @WeighingMachineId " +
                                " ,@LoadingId " +
                                " ,@WeightMeasurTypeId " +
                                " ,@IsConfirmed " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@WeightMT " +
                                " ,@VehicleNo " +
                                " ,@machineCalibrationId " +
                                " ,@unLoadingId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdWeightMeasure", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IdWeightMeasure;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.LoadingId);
            cmdInsert.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeightMeasurTypeId;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IsConfirmed;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(Constants.ServerDateTime);
            cmdInsert.Parameters.Add("@WeightMT", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.WeightMT;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.VehicleNo);
            cmdInsert.Parameters.Add("@machineCalibrationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.MachineCalibrationId);
            cmdInsert.Parameters.Add("@unLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UnLoadingId);

            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblWeighingMeasuresTO.IdWeightMeasure = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblWeighingMeasuresTO, cmdUpdate);
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

        public static int UpdateTblWeighingMeasures(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblWeighingMeasuresTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblWeighingMeasuresTO tblWeighingMeasuresTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempWeighingMeasures] SET " + 
                            "  [weighingMachineId]= @WeighingMachineId" +
                            " ,[loadingId]= @LoadingId" +
                            " ,[weightMeasurTypeId]= @WeightMeasurTypeId" +
                            " ,[isConfirmed]= @IsConfirmed" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[weightMT]= @WeightMT" +
                            " ,[vehicleNo] = @VehicleNo" +
                            " ,[machineCalibrationId]= @MachineCalibrationId" +
                            " ,[unLoadingId] = @unLoadingId " +
                            " WHERE [idWeightMeasure] = @IdWeightMeasure"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdWeightMeasure", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IdWeightMeasure;
            cmdUpdate.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeighingMachineId;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.LoadingId);
            cmdUpdate.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.WeightMeasurTypeId;
            cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblWeighingMeasuresTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@WeightMT", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.WeightMT;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.VehicleNo;
            cmdUpdate.Parameters.Add("@MachineCalibrationId", System.Data.SqlDbType.NVarChar).Value = tblWeighingMeasuresTO.MachineCalibrationId;
            cmdUpdate.Parameters.Add("@unLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMeasuresTO.UnLoadingId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblWeighingMeasures(Int32 idWeightMeasure)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idWeightMeasure, cmdDelete);
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

        public static int DeleteTblWeighingMeasures(Int32 idWeightMeasure, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idWeightMeasure, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idWeightMeasure, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempWeighingMeasures] " +
            " WHERE idWeightMeasure = " + idWeightMeasure +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeightMeasure", System.Data.SqlDbType.Int).Value = tblWeighingMeasuresTO.IdWeightMeasure;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
