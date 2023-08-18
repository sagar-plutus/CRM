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
    public class TblPurchaseWeighingStageSummaryDAO : ITblPurchaseWeighingStageSummaryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseWeighingStageSummaryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseWeighingStageSummary.*, userDisplayName as unloadingConfirmedByUser FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +                                 
                                  " left join tblWeighingMachine tblWeighingMachine on tblWeighingMachine.idWeighingMachine=tblPurchaseWeighingStageSummary.weighingMachineId " +
                                  " left join dimWeightMeasrTypes dimWeightMeasrTypes on dimWeightMeasrTypes.idWeightMeasurType=tblPurchaseWeighingStageSummary.weightMeasurTypeId " +
                                  " left join tblMachineCalibration tblMachineCalibration on tblMachineCalibration.idMachineCalibration=tblPurchaseWeighingStageSummary.machineCalibrationId " +
                                  " left join tblUser on tblUser.idUser = tblPurchaseWeighingStageSummary.unloadingConfirmedBy ";
            return sqlSelectQry;
        }
        //Added by minal 21 April 2021 Bug Id 8471 For On Comparison on view weighing show unloading point as well
        public String SqlSelectQueryForViewWeighingReport()
        {
            //String sqlSelectQry = " SELECT tblPurchaseWeighingStageSummary.*,userNM.userDisplayName AS supervisor FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
            //                      " LEFT JOIN ( SELECT userId,userDisplayName  FROM tblUser " +
            //                      " LEFT JOIN tblUserRole ON tblUser.iduser = tblUserRole.userId " +
            //                      " LEFT JOIN tblRole ON  tblUserRole.roleId = tblRole.idRole  " +
            //                      " WHERE roleTypeId = 19 AND tblUserRole.isActive=1 AND tblUser.isActive=1   " +
            //                      " ) AS userNM ON tblPurchaseWeighingStageSummary.supervisorId = userNM.userId " +
            //                      " left join tblWeighingMachine tblWeighingMachine on tblWeighingMachine.idWeighingMachine=tblPurchaseWeighingStageSummary.weighingMachineId " +
            //                      " left join dimWeightMeasrTypes dimWeightMeasrTypes on dimWeightMeasrTypes.idWeightMeasurType=tblPurchaseWeighingStageSummary.weightMeasurTypeId " +
            //                      " left join tblMachineCalibration tblMachineCalibration on tblMachineCalibration.idMachineCalibration=tblPurchaseWeighingStageSummary.machineCalibrationId ";

            //Prajakta[2021-05-20] Added to get unloading completed by user name if supervisor name is not there
            String sqlSelectQry = " SELECT tblPurchaseWeighingStageSummary.*,ISNULL(userNM.userDisplayName,unloadingCompleUser.userDisplayName) AS supervisor FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                                  " LEFT JOIN ( SELECT userId,userDisplayName  FROM tblUser " +
                                  " LEFT JOIN tblUserRole ON tblUser.iduser = tblUserRole.userId " +
                                  " LEFT JOIN tblRole ON  tblUserRole.roleId = tblRole.idRole  " +
                                  " WHERE roleTypeId = 19 AND tblUserRole.isActive=1 AND tblUser.isActive=1   " +
                                  " ) AS userNM ON tblPurchaseWeighingStageSummary.unloadingConfirmedBy = userNM.userId " +
                                  " left join tblWeighingMachine tblWeighingMachine on tblWeighingMachine.idWeighingMachine=tblPurchaseWeighingStageSummary.weighingMachineId " +
                                  " left join dimWeightMeasrTypes dimWeightMeasrTypes on dimWeightMeasrTypes.idWeightMeasurType=tblPurchaseWeighingStageSummary.weightMeasurTypeId " +
                                  " left join tblMachineCalibration tblMachineCalibration on tblMachineCalibration.idMachineCalibration=tblPurchaseWeighingStageSummary.machineCalibrationId " +
                                  " left join tblPurchaseScheduleSummary unloadingComplete on tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId " +
                                  " = unloadingComplete.rootScheduleId and unloadingComplete.isUnloadingCompleted = 1 and unloadingComplete.vehiclePhaseId =  " + (Int32)Constants.PurchaseVehiclePhasesE.UNLOADING_COMPLETED +
                                  " left join tbluser unloadingCompleUser on unloadingCompleUser.idUser = unloadingComplete.updatedBy ";


            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary()
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

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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

        public TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseWeighingStage = " + idPurchaseWeighingStage + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
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
                cmdSelect.Dispose();
            }
        }
        public TblPurchaseWeighingStageSummaryTO SelectTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseWeighingStage = " + idPurchaseWeighingStage + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;

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
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleId(Int32 purchaseScheduleId, bool fromWeighing)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  purchaseScheduleSummaryId = " + purchaseScheduleId + " ";
                if (fromWeighing == false)
                {
                    // cmdSelect.CommandText += " and isnull(isValid,0) =0";
                }
                cmdSelect.CommandText += " order by weightStageId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetailsBySchduleIdForWeighingReport(Int32 purchaseScheduleId, bool fromWeighing)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForViewWeighingReport() + " WHERE  purchaseScheduleSummaryId = " + purchaseScheduleId + " ";
                if (fromWeighing == false)
                {
                    // cmdSelect.CommandText += " and isnull(isValid,0) =0";
                }
                cmdSelect.CommandText += " order by weightStageId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToListForWeighingReport(reader);
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

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, SqlConnection conn, SqlTransaction tran, Boolean isGetAllWeighingDtls)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                // conn.Open();
                cmdSelect.CommandText =
                                        " select * from tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                                        " inner join  tblPurchaseScheduleSummary tblPurchaseScheduleSummary on " +
                                        " tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId=tblPurchaseScheduleSummary.idPurchaseScheduleSummary " +
                                        " where  tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId = " + purchaseScheduleId;
                                        //" and tblPurchaseWeighingStageSummary.weightMeasurTypeId= " + weightTypeId + ""
                                        ;

                if (!String.IsNullOrEmpty(weightTypeId))
                {
                    cmdSelect.CommandText += "and tblPurchaseWeighingStageSummary.weightMeasurTypeId= " + weightTypeId + "";
                }


                if (!isGetAllWeighingDtls)
                    cmdSelect.CommandText += " and isnull(tblPurchaseWeighingStageSummary.isValid,0) = 0";


                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, string weightTypeId, string wtMachineIds, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // conn.Open();
                cmdSelect.CommandText =
                                        " select * from tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                                        " inner join  tblPurchaseScheduleSummary tblPurchaseScheduleSummary on " +
                                        " tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId=tblPurchaseScheduleSummary.idPurchaseScheduleSummary " +
                                        " where  tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId = " + purchaseScheduleId + " and tblPurchaseWeighingStageSummary.weightMeasurTypeId= " + weightTypeId + "" +
                                        " and tblPurchaseWeighingStageSummary.weighingMachineId in ( " + wtMachineIds + " )";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeightDetails(Int32 purchaseScheduleId, string weightTypeId, Boolean isGetAllWeighingDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText =
                                        " select * from tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                                        " inner join  tblPurchaseScheduleSummary tblPurchaseScheduleSummary on " +
                                        " tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId=tblPurchaseScheduleSummary.idPurchaseScheduleSummary " +
                                        " where  tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId = " + purchaseScheduleId;


                if(!String.IsNullOrEmpty(weightTypeId))
                {
                    cmdSelect.CommandText += "and tblPurchaseWeighingStageSummary.weightMeasurTypeId= " + weightTypeId + "";
                }

                if (!isGetAllWeighingDtls)
                {
                    cmdSelect.CommandText += " and isnull(tblPurchaseWeighingStageSummary.isValid,0) = 0";
                }



                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseWeighingStageSummaryTO> SelectAllTblPurchaseWeighingStageSummary(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseWeighingStageSummaryTO> ConvertDTToList(SqlDataReader tblPurchaseWeighingStageSummaryTODT)
        {
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = new List<TblPurchaseWeighingStageSummaryTO>();
            if (tblPurchaseWeighingStageSummaryTODT != null)
            {
                while (tblPurchaseWeighingStageSummaryTODT.Read())
                {
                    TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTONew = new TblPurchaseWeighingStageSummaryTO();
                    if (tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IdPurchaseWeighingStage = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["weighingMachineId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeighingMachineId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weighingMachineId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["grossWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GrossWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["grossWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["actualWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.ActualWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["actualWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["netWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.NetWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["netWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["rstNumber"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RstNumber = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["rstNumber"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightMeasurTypeId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"].ToString());

                    //if (tblPurchaseWeighingStageSummaryTODT["weighingMachineId"] != DBNull.Value)
                    //    tblPurchaseWeighingStageSummaryTONew.WeighingMachineId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weighingMachineId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.MachineCalibrationId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"].ToString());


                    if (tblPurchaseWeighingStageSummaryTODT["weightStageId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightStageId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightStageId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["recoveryPer"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryPer = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["recoveryPer"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["recoveryBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["recoveryOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["isRecConfirm"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsRecConfirm = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isRecConfirm"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["isValid"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsValid = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isValid"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["supervisorId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["intervalTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IntervalTime = Convert.ToInt64(tblPurchaseWeighingStageSummaryTODT["intervalTime"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["gradingStartTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GradingStartTime = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["gradingStartTime"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["gradingEndTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GradingEndTime = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["gradingEndTime"].ToString());

                    //[2021-10-04] Dhananjay Added
                    try
                    {
                        if (tblPurchaseWeighingStageSummaryTODT["unloadingConfirmedBy"] != DBNull.Value)
                            tblPurchaseWeighingStageSummaryTONew.UnloadingConfirmedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["unloadingConfirmedBy"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        if (tblPurchaseWeighingStageSummaryTODT["unloadingConfirmedByUser"] != DBNull.Value)
                            tblPurchaseWeighingStageSummaryTONew.UnloadingConfirmedByUser = tblPurchaseWeighingStageSummaryTODT["unloadingConfirmedByUser"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }

                    tblPurchaseWeighingStageSummaryTOList.Add(tblPurchaseWeighingStageSummaryTONew);
                }
            }

            if(tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
            {
                tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();
            }
            return tblPurchaseWeighingStageSummaryTOList;
        }


        public List<TblPurchaseWeighingStageSummaryTO> ConvertDTToListForWeighingReport(SqlDataReader tblPurchaseWeighingStageSummaryTODT)
        {
            List<TblPurchaseWeighingStageSummaryTO> tblPurchaseWeighingStageSummaryTOList = new List<TblPurchaseWeighingStageSummaryTO>();
            if (tblPurchaseWeighingStageSummaryTODT != null)
            {
                while (tblPurchaseWeighingStageSummaryTODT.Read())
                {
                    TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTONew = new TblPurchaseWeighingStageSummaryTO();
                    if (tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IdPurchaseWeighingStage = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["idPurchaseWeighingStage"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["weighingMachineId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeighingMachineId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weighingMachineId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["grossWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GrossWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["grossWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["actualWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.ActualWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["actualWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["netWeightMT"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.NetWeightMT = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["netWeightMT"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["rstNumber"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RstNumber = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["rstNumber"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["purchaseScheduleSummaryId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightMeasurTypeId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightMeasurTypeId"].ToString());

                    //if (tblPurchaseWeighingStageSummaryTODT["weighingMachineId"] != DBNull.Value)
                    //    tblPurchaseWeighingStageSummaryTONew.WeighingMachineId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weighingMachineId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.MachineCalibrationId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["machineCalibrationId"].ToString());


                    if (tblPurchaseWeighingStageSummaryTODT["weightStageId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.WeightStageId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["weightStageId"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["recoveryPer"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryPer = Convert.ToDouble(tblPurchaseWeighingStageSummaryTODT["recoveryPer"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["recoveryBy"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["recoveryOn"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["isRecConfirm"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsRecConfirm = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isRecConfirm"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["isValid"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IsValid = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["isValid"].ToString());

                    if (tblPurchaseWeighingStageSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseWeighingStageSummaryTODT["supervisorId"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["intervalTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.IntervalTime = Convert.ToInt64(tblPurchaseWeighingStageSummaryTODT["intervalTime"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["gradingStartTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GradingStartTime = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["gradingStartTime"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["gradingEndTime"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.GradingEndTime = Convert.ToDateTime(tblPurchaseWeighingStageSummaryTODT["gradingEndTime"].ToString());
                    if (tblPurchaseWeighingStageSummaryTODT["supervisor"] != DBNull.Value)
                        tblPurchaseWeighingStageSummaryTONew.Supervisor = Convert.ToString(tblPurchaseWeighingStageSummaryTODT["supervisor"].ToString());

                    tblPurchaseWeighingStageSummaryTOList.Add(tblPurchaseWeighingStageSummaryTONew);
                }
            }

            if (tblPurchaseWeighingStageSummaryTOList != null && tblPurchaseWeighingStageSummaryTOList.Count > 0)
            {
                tblPurchaseWeighingStageSummaryTOList = tblPurchaseWeighingStageSummaryTOList.OrderBy(a => a.IdPurchaseWeighingStage).ToList();
            }
            return tblPurchaseWeighingStageSummaryTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseWeighingStageSummaryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseWeighingStageSummaryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int updateIsValidFlagToInvalid(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
                           " [isValid] = @IsValid " +
                           " WHERE idPurchaseWeighingStage = @IdPurchaseWeighingStage ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdUpdate.Parameters.Add("@IsValid", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IsValid;

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }

        }

        public int ExecuteInsertionCommand(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseWeighingStageSummary]( " +
            //"  [idPurchaseWeighingStage]" +
            "  [weighingMachineId]" +
            " ,[supplierId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[grossWeightMT]" +
            " ,[actualWeightMT]" +
            " ,[netWeightMT]" +
            " ,[rstNumber]" +
            " ,[vehicleNo]" +
            " ,[purchaseScheduleSummaryId]" +
            " ,[weightMeasurTypeId]" +
            " ,[weightStageId]" +
            //" ,[weighingMachineId]" +
            " ,[machineCalibrationId]" +
            " ,[supervisorId]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseWeighingStage " +
            "  @WeighingMachineId " +
            " ,@SupplierId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@GrossWeightMT " +
            " ,@ActualWeightMT " +
            " ,@NetWeightMT " +
            " ,@RstNumber " +
            " ,@VehicleNo " +
            " ,@PurchaseScheduleSummaryId" +
            " ,@WeightMeasurTypeId" +
            " ,@WeightStageId" +
            //" ,@WeighingMachineId" +supervisorId
            " ,@MachineCalibrationId" +
            " ,@supervisorId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";


            //cmdInsert.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
            //Saket [2018-10-13] Commented.
            //cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.SupplierId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseWeighingStageSummaryTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseWeighingStageSummaryTO.UpdatedOn;
            cmdInsert.Parameters.Add("@GrossWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.GrossWeightMT;
            cmdInsert.Parameters.Add("@ActualWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.ActualWeightMT;
            cmdInsert.Parameters.Add("@NetWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.NetWeightMT;
            cmdInsert.Parameters.Add("@RstNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.RstNumber);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NChar).Value = tblPurchaseWeighingStageSummaryTO.VehicleNo;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId);
            cmdInsert.Parameters.Add("@WeightStageId", System.Data.SqlDbType.VarChar).Value = tblPurchaseWeighingStageSummaryTO.WeightStageId;
            cmdInsert.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.WeighingMachineId;
            cmdInsert.Parameters.Add("@MachineCalibrationId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.MachineCalibrationId;
            cmdInsert.Parameters.Add("@supervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.SupervisorId);
            
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public int UpdateRecoveryEngineerId(int loginUserId, int purchaseScheduleSummaryId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                "  [engineerId]= @engineerId" +
                " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @idPurchaseScheduleSummary ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@engineerId", System.Data.SqlDbType.Int).Value = loginUserId;
                cmdUpdate.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = purchaseScheduleSummaryId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int PostUpdatePhotographerId(int loginUserId, int purchaseScheduleSummaryId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                "  [photographerId]= @photographerId" +
                " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @idPurchaseScheduleSummary ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@photographerId", System.Data.SqlDbType.Int).Value = loginUserId;
                cmdUpdate.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = purchaseScheduleSummaryId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }


        public int UpdateGraderId(int loginUserId, int purchaseScheduleSummaryId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                "  [graderId]= @graderId" +
                " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @idPurchaseScheduleSummary ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@graderId", System.Data.SqlDbType.Int).Value = loginUserId;
                cmdUpdate.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = purchaseScheduleSummaryId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }
        public int UpdateSupervisorId(TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTO)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tblPurchaseScheduleSummary] SET " +
                "  [supervisorId]= @supervisorId" +
                " WHERE isnull(rootScheduleId,idPurchaseScheduleSummary) = @idPurchaseScheduleSummary ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@supervisorId", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.SupervisorId;
                cmdUpdate.Parameters.Add("@idPurchaseScheduleSummary", System.Data.SqlDbType.Int).Value = tblPurchaseScheduleSummaryTO.ActualRootScheduleId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseWeighingStageSummaryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }


        public int UpdateTblPurchaseWeighingStageSummaryForIsValid(int rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForValid(rootScheduleId, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        private int ExecuteUpdationCommandForValid(int rootScheduleId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
           " [isValid] = null" +
           " WHERE purchaseScheduleSummaryId = @rootScheduleId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@rootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateTblPurchaseWeighingStageSummary(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseWeighingStageSummaryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
            // "  [idPurchaseWeighingStage] = @IdPurchaseWeighingStage" +
            "  [weighingMachineId]= @WeighingMachineId" +
            " ,[supplierId]= @SupplierId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[grossWeightMT]= @GrossWeightMT" +
            " ,[actualWeightMT]= @ActualWeightMT" +
            " ,[netWeightMT]= @NetWeightMT" +
            " ,[rstNumber]= @RstNumber" +
            " ,[vehicleNo] = @VehicleNo" +
            " ,[purchaseScheduleSummaryId] = @PurchaseScheduleSummaryId" +
            " ,[weightMeasurTypeId] = @WeightMeasurTypeId" +
            " ,[weightStageId] = @WeightStageId" +
            //" ,[weighingMachineId] = @WeighingMachineId" +
            " ,[machineCalibrationId] = @MachineCalibrationId" +
            " WHERE idPurchaseWeighingStage = @IdPurchaseWeighingStage ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
            //Saket [2018-10-13] Added.
            //cmdUpdate.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.WeighingMachineId;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.SupplierId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@GrossWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.GrossWeightMT;
            cmdUpdate.Parameters.Add("@ActualWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.ActualWeightMT;
            cmdUpdate.Parameters.Add("@NetWeightMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseWeighingStageSummaryTO.NetWeightMT;
            cmdUpdate.Parameters.Add("@RstNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.RstNumber);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NChar).Value = tblPurchaseWeighingStageSummaryTO.VehicleNo;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@WeightMeasurTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.WeightMeasurTypeId;
            cmdUpdate.Parameters.Add("@WeightStageId", System.Data.SqlDbType.VarChar).Value = tblPurchaseWeighingStageSummaryTO.WeightStageId;
            cmdUpdate.Parameters.Add("@WeighingMachineId", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.WeighingMachineId;
            cmdUpdate.Parameters.Add("@MachineCalibrationId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.MachineCalibrationId);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateTblPurchaseWeighingStageSummaryRecoveryDtls(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
                           " [recoveryPer]= @RecoveryPer" +
                           " ,[recoveryBy]= @RecoveryBy" +
                           " ,[recoveryOn] = @RecoveryOn " +
                           " ,[isRecConfirm] = @IsRecConfirm " +
                           " WHERE idPurchaseWeighingStage = @IdPurchaseWeighingStage ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdUpdate.Parameters.Add("@RecoveryPer", System.Data.SqlDbType.Decimal).Value = tblPurchaseWeighingStageSummaryTO.RecoveryPer;
                cmdUpdate.Parameters.Add("@RecoveryBy", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.RecoveryBy;
                cmdUpdate.Parameters.Add("@IsRecConfirm", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IsRecConfirm;
                cmdUpdate.Parameters.Add("@RecoveryOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseWeighingStageSummaryTO.RecoveryOn;

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseWeighingStage, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPurchaseWeighingStageSummary(Int32 idPurchaseWeighingStage, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseWeighingStage, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idPurchaseWeighingStage, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseWeighingStageSummary] " +
            " WHERE idPurchaseWeighingStage = " + idPurchaseWeighingStage + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllWeighingStageAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseWeighingStageSummary WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
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

        //chetan [16-March-2020] added for migration

        public List<TblPurchaseWeighingStageSummaryTO> GetVehWtDetailsForWeighingMachine(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // conn.Open();
                cmdSelect.CommandText =
                                        " select * from tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                                        " inner join  tblPurchaseScheduleSummary tblPurchaseScheduleSummary on " +
                                        " tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId=tblPurchaseScheduleSummary.idPurchaseScheduleSummary " +
                                        " where  tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId = " + purchaseScheduleId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
        //chetan[09-April-2020] added update Unloding time update updateIsValidFlagToInvalid
        public int UpdateUnlodingEndTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
                           " [intervalTime] = @intervalTime " +
                           " ,[gradingEndTime] = @gradingEndTime " +
                           " ,[unloadingConfirmedBy] = @unloadingConfirmedBy " +
                           " WHERE idPurchaseWeighingStage = @IdPurchaseWeighingStage ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdUpdate.Parameters.Add("@intervalTime", System.Data.SqlDbType.BigInt).Value = tblPurchaseWeighingStageSummaryTO.IntervalTime;
                cmdUpdate.Parameters.Add("@gradingEndTime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.GradingEndTime);
                cmdUpdate.Parameters.Add("@unloadingConfirmedBy", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.UnloadingConfirmedBy;

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }

        }

        public int UpdateUnlodingStartTime(TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                //cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblPurchaseWeighingStageSummary] SET " +
                           " [intervalTime] = @intervalTime " +
                            " ,[gradingStartTime] = @gradingStartTime " +
                           " WHERE idPurchaseWeighingStage = @IdPurchaseWeighingStage ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdUpdate.Parameters.Add("@intervalTime", System.Data.SqlDbType.BigInt).Value = tblPurchaseWeighingStageSummaryTO.IntervalTime;
                cmdUpdate.Parameters.Add("@gradingStartTime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseWeighingStageSummaryTO.GradingStartTime);

                return cmdUpdate.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }

        }

        //chetan[09-April-2020] added 

        public List<TblPurchaseWeighingStageSummaryTO> SelectTblPurchaseWeighingStageSummary(DateTime fromDate,DateTime toDate,String purchaseManagerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " Where tblPurchaseWeighingStageSummary.createdOn BETWEEN @fromDate And  @toDate And weightMeasurTypeId=" +(int)StaticStuff.Constants.TransMeasureTypeE.INTERMEDIATE_WEIGHT;
                if (!String.IsNullOrEmpty(purchaseManagerIds))
                {
                    cmdSelect.CommandText += " AND tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId IN (SELECT ISNULL(tblPurchaseScheduleSummary.rootScheduleId,tblPurchaseScheduleSummary.idPurchaseScheduleSummary) " +
                                             " FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                                             " WHERE tblPurchaseScheduleSummary.purchaseEnquiryId IN (SELECT tblPurchaseEnquiry.idPurchaseEnquiry " +
                                             "                                                        FROM tblPurchaseEnquiry tblPurchaseEnquiry" +
                                             "                                                        WHERE tblPurchaseEnquiry.userId IN (" + purchaseManagerIds + ")))";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = Constants.GetStartDateTime(fromDate);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = Constants.GetEndDateTime(toDate);

                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseWeighingStageSummaryTO> GetVehicleWeighingDetails(Int32 purchaseScheduleId,Int32 idPurchaseWeighingStage)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  purchaseScheduleSummaryId = " + purchaseScheduleId + " And idPurchaseWeighingStage < "+idPurchaseWeighingStage + " order by idPurchaseWeighingStage desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseWeighingStage", System.Data.SqlDbType.Int).Value = tblPurchaseWeighingStageSummaryTO.IdPurchaseWeighingStage;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseWeighingStageSummaryTO> list = ConvertDTToList(reader);
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
    }
}
