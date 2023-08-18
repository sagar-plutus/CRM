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
    public class LoadingDAO : ILoadingDAO
    {
        #region Declaration & Constructor
        private readonly IConnectionString _iConnectionString;
        public LoadingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #endregion
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQuery = @"select tblTRLoading.*,tblVehicle.idVehicle, tblVehicle.vehicleNo, tblVehicle.vehicleStatusId, tblTransferRequest.requestDisplayNo, trTblUser.userDisplayName as RequestUserName,tblTransferRequest.createdOn AS RequestCreatedOn,
            materialType.value as MaterialType, materialSubType.value as MaterialsSubType, fromLocation.value as FromLocation, tblUnloadingSLA.idSLA,
            toLocation.value as ToLocation, unloadingPoint.value as UnloadingPoint, dimStatus.statusDesc, dimVehicleStatus.statusName AS VehicleStatus, tblUser.userDisplayName as CreatedByName, unMaterialType.value as UnloadingMaterialType, unMaterialSubType.value as UnloadingMaterialsSubType
            from tblTRLoading tblTRLoading
            LEFT JOIN tblTransferRequest tblTransferRequest on tblTransferRequest.idTransferRequest = tblTRLoading.transferRequestId
            LEFT JOIN tblUnloadingSLA tblUnloadingSLA on tblUnloadingSLA.loadingId = tblTRLoading.idLoading
            LEFT JOIN tblVehicle tblVehicle on tblVehicle.idVehicle = tblTRLoading.vehicleId
            LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
            LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId
            LEFT JOIN dimGenericMaster fromLocation on fromLocation.idGenericMaster = tblTRLoading.fromLocationId
            LEFT JOIN dimGenericMaster toLocation on toLocation.idGenericMaster = tblTRLoading.toLocationId
            LEFT JOIN dimGenericMaster unloadingPoint on unloadingPoint.idGenericMaster = tblTRLoading.unloadingPointId
            LEFT JOIN dimGenericMaster unMaterialType on unMaterialType.idGenericMaster = tblTRLoading.unMaterialTypeId
            LEFT JOIN dimGenericMaster unMaterialSubType on unMaterialSubType.idGenericMaster = tblTRLoading.unMaterialSubTypeId
            LEFT JOIN dimStatus dimStatus on dimStatus.idStatus = tblTRLoading.statusId 
            LEFT JOIN dimStatus dimVehicleStatus on dimVehicleStatus.idStatus = tblVehicle.vehicleStatusId 
            LEFT JOIN tblUser tblUser on tblUser.idUser = tblTRLoading.createdBy
            LEFT JOIN tblUser trTblUser on trTblUser.idUser = tblTransferRequest.createdBy
            WHERE tblTRLoading.isActive = 1";
            return sqlSelectQuery;
        }
        public String SqlSelectQueryVehicleWiseLoading()
        {
            String sqlSelectQuery = @"select tblTRLoading.*,tblVehicle.idVehicle,tblVehicle.vehicleNo, tblVehicle.vehicleStatusId, tblTransferRequest.requestDisplayNo, trTblUser.userDisplayName as RequestUserName,tblTransferRequest.createdOn AS RequestCreatedOn,
            materialType.value as MaterialType, materialSubType.value as MaterialsSubType, fromLocation.value as FromLocation, tblUnloadingSLA.idSLA,
            toLocation.value as ToLocation, unloadingPoint.value as UnloadingPoint, dimStatus.statusDesc, dimVehicleStatus.statusName AS VehicleStatus, tblUser.userDisplayName as CreatedByName, unMaterialType.value as UnloadingMaterialType, unMaterialSubType.value as UnloadingMaterialsSubType
            from tblVehicle tblVehicle
            LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.vehicleId = tblVehicle.idVehicle and tblTRLoading.statusId NOT IN(@LOADING_STATUS_IDS) and tblTRLoading.isActive = 1
            LEFT JOIN tblTransferRequest tblTransferRequest on tblTransferRequest.idTransferRequest = tblTRLoading.transferRequestId
            LEFT JOIN tblUnloadingSLA tblUnloadingSLA on tblUnloadingSLA.loadingId = tblTRLoading.idLoading            
            LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
            LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId
            LEFT JOIN dimGenericMaster fromLocation on fromLocation.idGenericMaster = tblTRLoading.fromLocationId
            LEFT JOIN dimGenericMaster toLocation on toLocation.idGenericMaster = tblTRLoading.toLocationId
            LEFT JOIN dimGenericMaster unloadingPoint on unloadingPoint.idGenericMaster = tblTRLoading.unloadingPointId
            LEFT JOIN dimGenericMaster unMaterialType on unMaterialType.idGenericMaster = tblTRLoading.unMaterialTypeId
            LEFT JOIN dimGenericMaster unMaterialSubType on unMaterialSubType.idGenericMaster = tblTRLoading.unMaterialSubTypeId
            LEFT JOIN dimStatus dimStatus on dimStatus.idStatus = tblTRLoading.statusId 
            LEFT JOIN dimStatus dimVehicleStatus on dimVehicleStatus.idStatus = tblVehicle.vehicleStatusId 
            LEFT JOIN tblUser tblUser on tblUser.idUser = tblTRLoading.createdBy
            LEFT JOIN tblUser trTblUser on trTblUser.idUser = tblTransferRequest.createdBy
            WHERE tblVehicle.vehicleStatusId IN(@VEHICLE_STATUS_IDS)";
            String VehicleStatusIdStr = (Int32)Constants.InternalTransferRequestVehicalStatusE.IN_PROCESS + "," + (Int32)Constants.InternalTransferRequestVehicalStatusE.BREAKDOWN;
            String LoadingStatusIdStr = (Int32)Constants.LoadingStatusE.COMPLETED + "," + (Int32)Constants.LoadingStatusE.REJECTED;
            sqlSelectQuery = sqlSelectQuery.Replace("@VEHICLE_STATUS_IDS", VehicleStatusIdStr);
            sqlSelectQuery = sqlSelectQuery.Replace("@LOADING_STATUS_IDS", LoadingStatusIdStr);
            return sqlSelectQuery;
        }
        public String SqlSelectSLAQuery()
        {
            String sqlSelectQuery = @"SELECT tblUnloadingSLA.*, materialType.value as MaterialType, tblVehicle.vehicleNo,
            slaUnloading.value as slaUnloadingDesc, mixMaterial.value as mixMaterialDesc,
            offChemistry.value as offChemistryDesc, tblUser.userDisplayName as CreatedByName, tblTRLoading.loadingSlipNo,tblTransferRequest.requestDisplayNo
            FROM tblUnloadingSLA tblUnloadingSLA
            LEFT JOIN tblVehicle tblVehicle on tblVehicle.idVehicle = tblUnloadingSLA.vehicleId
            LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.idLoading = tblUnloadingSLA.loadingId
            LEFT JOIN tblTransferRequest tblTransferRequest on tblTransferRequest.idTransferRequest = tblTRLoading.transferRequestId
            LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblUnloadingSLA.materialTypeId
            LEFT JOIN dimGenericMaster slaUnloading on slaUnloading.idGenericMaster = tblUnloadingSLA.slaUnloadingId
            LEFT JOIN dimGenericMaster mixMaterial on mixMaterial.idGenericMaster = tblUnloadingSLA.mixMaterialId
            LEFT JOIN dimGenericMaster offChemistry on offChemistry.idGenericMaster = tblUnloadingSLA.offChemistryId
            LEFT JOIN tblUser tblUser on tblUser.idUser = tblUnloadingSLA.createdBy
            WHERE tblUnloadingSLA.isActive = 1";
            return sqlSelectQuery;
        }
        #endregion
        #region Get
        public TblTRLoadingTO GetLoadingDetailsTO(Int32 idLoading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND tblTRLoading.idLoading = " + idLoading;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = ConvertDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list[0];
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
        public TblTRLoadingTO GetLoadingDetailsTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND tblTRLoading.idLoading = " + idLoading;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = ConvertDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list[0];
                else
                    return null;
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
        public List<TblTRLoadingTO> GetLoadingDetailsTOList(LoadingFilterTO loadingFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                if (loadingFilterTO != null)
                {
                    if (!String.IsNullOrEmpty(loadingFilterTO.StatusIdStr))
                    {
                        cmdSelect.CommandText += " AND tblTRLoading.statusId IN(" + loadingFilterTO.StatusIdStr + ")";
                    }
                    if (loadingFilterTO.FromDate != DateTime.MinValue && loadingFilterTO.ToDate != DateTime.MinValue && loadingFilterTO.SkipDateFilter == false)
                    {
                        if (CheckDate(Convert.ToString(loadingFilterTO.FromDate)) == true && CheckDate(Convert.ToString(loadingFilterTO.ToDate)) == true)
                        {
                            cmdSelect.CommandText += " AND CAST(tblTRLoading.statusOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                            cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.FromDate;
                            cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.ToDate;
                        }
                    }
                    if (loadingFilterTO.MigrationDate != DateTime.MinValue)
                    {
                        if (CheckDate(Convert.ToString(loadingFilterTO.MigrationDate)) == true)
                        {
                            cmdSelect.CommandText += " AND CAST(tblTRLoading.statusOn as date) <= @MigrationDate";
                            cmdSelect.Parameters.Add("@MigrationDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.MigrationDate;
                        }
                    }
                    if (loadingFilterTO.IsReviewUnloading == 1)
                    {
                        cmdSelect.CommandText += " AND tblTRLoading.IsReviewUnloading = 1";
                    }
                }
                cmdSelect.CommandText += " ORDER BY tblTRLoading.idLoading DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = ConvertDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblUnloadingSLATO> GetUnloadingSLADetailsList(UnloadingSLAFilterTO unloadingSLAFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectSLAQuery();
                if (unloadingSLAFilterTO != null)
                {
                    if (unloadingSLAFilterTO.FromDate != DateTime.MinValue && unloadingSLAFilterTO.ToDate != DateTime.MinValue && unloadingSLAFilterTO.SkipDateFilter == false)
                    {
                        if (CheckDate(Convert.ToString(unloadingSLAFilterTO.FromDate)) == true && CheckDate(Convert.ToString(unloadingSLAFilterTO.ToDate)) == true)
                        {
                            cmdSelect.CommandText += " AND CAST(tblUnloadingSLA.createdOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                            cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.FromDate;
                            cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.ToDate;
                        }
                    }
                }
                cmdSelect.CommandText += " ORDER BY tblUnloadingSLA.idSLA DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingSLATO> list = ConvertTblUnloadingSLADTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingTO> GetVehicleWiseLoadingDetailsTOList(LoadingFilterTO loadingFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryVehicleWiseLoading();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = ConvertDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, Int32 LoadingTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT tblTRLoadingWeighing.*, tblWeighingMachine.machineName FROM tblTRLoadingWeighing tblTRLoadingWeighing
                LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tblTRLoadingWeighing.weighingMachineId
                WHERE tblTRLoadingWeighing.loadingId = " + LoadingId + " AND tblTRLoadingWeighing.LoadingTypeId = " + LoadingTypeId + " AND tblTRLoadingWeighing.isActive = 1 ORDER BY tblTRLoadingWeighing.idWeighing ASC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingWeighingTO> list = ConvertTblTRLoadingWeighingDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(String LoadingIdStr, String LoadingTypeIdStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT tblTRLoadingWeighing.*, tblWeighingMachine.machineName FROM tblTRLoadingWeighing tblTRLoadingWeighing
                LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tblTRLoadingWeighing.weighingMachineId
                WHERE tblTRLoadingWeighing.loadingId IN("+ LoadingIdStr + ") AND tblTRLoadingWeighing.LoadingTypeId IN("+ LoadingTypeIdStr + ") AND tblTRLoadingWeighing.isActive = 1 ORDER BY tblTRLoadingWeighing.idWeighing ASC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingWeighingTO> list = ConvertTblTRLoadingWeighingDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingTO> GetTRLoadingHistoryDetails(String statusIdStr, string loadingIdStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT loadingId, statusId, statusOn FROM tblTRLoadingHistory WHERE loadingId IN(" + loadingIdStr + ") " +
                " AND statusId IN("+ statusIdStr +")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = new List<TblTRLoadingTO>();
                if (tblTRLoadingTODT != null)
                {
                    while (tblTRLoadingTODT.Read())
                    {
                        TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                        if (tblTRLoadingTODT["loadingId"] != DBNull.Value)
                            tblTRLoadingTO.IdLoading = Convert.ToInt32(tblTRLoadingTODT["loadingId"].ToString());
                        if (tblTRLoadingTODT["statusId"] != DBNull.Value)
                            tblTRLoadingTO.StatusId = Convert.ToInt32(tblTRLoadingTODT["statusId"].ToString());
                        if (tblTRLoadingTODT["statusOn"] != DBNull.Value)
                            tblTRLoadingTO.StatusOn = Convert.ToDateTime(tblTRLoadingTODT["statusOn"].ToString());
                        tblTRLoadingTO.StatusOnDateStr = tblTRLoadingTO.StatusOn.ToString("dd/MM/yyyy");
                        tblTRLoadingTO.StatusOnTimeStr = tblTRLoadingTO.StatusOn.ToString("hh:mm tt");
                        list.Add(tblTRLoadingTO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingWeighingTO> GetWeighingNetWeightDetails(String LoadingIdStr, Int32 LoadingTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT loadingId, SUM(netWeight) as TotalNetWeight FROM tblTRLoadingWeighing WHERE loadingTypeId = @LoadingTypeId " +
                " AND tblTRLoadingWeighing.isActive = 1 AND loadingId IN("+ LoadingIdStr + ") " +
                " GROUP BY loadingId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@LoadingTypeId", System.Data.SqlDbType.Int).Value = LoadingTypeId;
                SqlDataReader tblTRLoadingWeighingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingWeighingTO> list = new List<TblTRLoadingWeighingTO>();
                if (tblTRLoadingWeighingTODT != null)
                {
                    while (tblTRLoadingWeighingTODT.Read())
                    {
                        TblTRLoadingWeighingTO tblTRLoadingWeighingTO = new TblTRLoadingWeighingTO();
                        if (tblTRLoadingWeighingTODT["loadingId"] != DBNull.Value)
                            tblTRLoadingWeighingTO.LoadingId = Convert.ToInt32(tblTRLoadingWeighingTODT["loadingId"].ToString());
                        if (tblTRLoadingWeighingTODT["TotalNetWeight"] != DBNull.Value)
                            tblTRLoadingWeighingTO.TotalNetWeight = Convert.ToDecimal(tblTRLoadingWeighingTODT["TotalNetWeight"].ToString());
                        list.Add(tblTRLoadingWeighingTO);
                    }
                }
                if (tblTRLoadingWeighingTODT != null)
                    tblTRLoadingWeighingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingTO> GetVehicleLoadingHistory(Int32 IdVehicle)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT tblTRLoadingHistory.statusId, tblTRLoadingHistory.statusOn, tblTRLoadingHistory.loadingId, tblVehicle.vehicleNo, dimStatus.statusName, tblUser.userDisplayName, tblTRLoading.loadingSlipNo
                FROM tblTRLoadingHistory tblTRLoadingHistory 
                LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.idLoading = tblTRLoadingHistory.loadingId 
                LEFT JOIN tblVehicle tblVehicle on tblVehicle.idVehicle = tblTRLoadingHistory.vehicleId
                LEFT JOIN dimStatus dimStatus on dimStatus.idStatus = tblTRLoadingHistory.statusId
                LEFT JOIN tblUser tblUser on tblUser.idUser = tblTRLoadingHistory.statusBy
                WHERE tblTRLoadingHistory.loadingId IN(SELECT MAX(loadingId) FROM tblTRLoadingHistory WHERE vehicleId = @IdVehicle) 
                group by tblTRLoadingHistory.statusId, tblTRLoadingHistory.statusOn, 
                tblTRLoadingHistory.loadingId, tblVehicle.vehicleNo, dimStatus.statusName, tblUser.userDisplayName, tblTRLoading.loadingSlipNo
                ORDER BY tblTRLoadingHistory.statusOn";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@IdVehicle", System.Data.SqlDbType.Int).Value = IdVehicle;
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = new List<TblTRLoadingTO>();
                if (tblTRLoadingTODT != null)
                {
                    while (tblTRLoadingTODT.Read())
                    {
                        TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                        if (tblTRLoadingTODT["statusId"] != DBNull.Value)
                            tblTRLoadingTO.StatusId = Convert.ToInt32(tblTRLoadingTODT["statusId"].ToString());
                        if (tblTRLoadingTODT["statusOn"] != DBNull.Value)
                            tblTRLoadingTO.StatusOn = Convert.ToDateTime(tblTRLoadingTODT["statusOn"].ToString());
                        if (tblTRLoadingTODT["loadingId"] != DBNull.Value)
                            tblTRLoadingTO.IdLoading = Convert.ToInt32(tblTRLoadingTODT["loadingId"].ToString());
                        if (tblTRLoadingTODT["vehicleNo"] != DBNull.Value)
                            tblTRLoadingTO.VehicleNo = Convert.ToString(tblTRLoadingTODT["vehicleNo"].ToString());
                        if (tblTRLoadingTODT["statusName"] != DBNull.Value)
                            tblTRLoadingTO.StatusDesc = Convert.ToString(tblTRLoadingTODT["statusName"].ToString());
                        if (tblTRLoadingTODT["userDisplayName"] != DBNull.Value)
                            tblTRLoadingTO.StatusByUserName = Convert.ToString(tblTRLoadingTODT["userDisplayName"].ToString());
                        if (tblTRLoadingTODT["loadingSlipNo"] != DBNull.Value)
                            tblTRLoadingTO.LoadingSlipNo = Convert.ToString(tblTRLoadingTODT["loadingSlipNo"].ToString());
                        list.Add(tblTRLoadingTO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingTO> GetVehicleWiseLoadingReportDetails(LoadingFilterTO loadingFilterTO, String WhereClause = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand(); 
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT tblTRLoading.idLoading, tblTRLoading.loadingSlipNo, tblTRLoading.createdOn, tblTRLoading.statusOn, tblTRLoading.createdBy, tblUser.userDisplayName as createdByName, 
                tblTRLoadingWeighing.vehicleNo, (SUM(tblTRLoadingWeighing.netWeight)*0.001) as LoadingNetWeight, (MIN(tblTRLoadingWeighing.grossWeight)*0.001) as LoadingTareWeight, (MAX(tblTRLoadingWeighing.grossWeight)*0.001) as LoadingGrossWeight, fromLocation.value AS FromLocation,toLocation.value AS ToLocation
                , tblTRLoading.unloadingNarration, tblTRLoading.weighingRemark, tblTRLoading.transferRequestId, materialType.value as MaterialType, materialSubType.value as MaterialsSubType, tblTRLoading.materialTypeId, tblTRLoading.materialSubTypeId, tblTRLoading.fromLocationId, tblTRLoading.toLocationId, tblTRLoading.scheduleQty
                FROM tblTRLoading tblTRLoading
                LEFT JOIN tblTRLoadingWeighing tblTRLoadingWeighing ON tblTRLoading.idLoading = tblTRLoadingWeighing.loadingId
                LEFT JOIN dimGenericMaster fromLocation ON fromLocation.idGenericMaster = tblTRLoading.fromLocationId
                LEFT JOIN dimGenericMaster toLocation ON toLocation.idGenericMaster = tblTRLoading.toLocationId
                LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
                LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId
                LEFT JOIN tblUser tblUser ON tblUser.idUser = tblTRLoading.createdBy
                WHERE tblTRLoadingWeighing.loadingTypeId = @LoadingTypeId AND tblTRLoading.statusId IN(@StatusIdStr) AND tblTRLoadingWeighing.isActive = 1 AND tblTRLoading.isActive = 1 ";
                if (loadingFilterTO.FromDate != DateTime.MinValue && loadingFilterTO.ToDate != DateTime.MinValue)
                {
                    if (CheckDate(Convert.ToString(loadingFilterTO.FromDate)) == true && CheckDate(Convert.ToString(loadingFilterTO.ToDate)) == true)
                    {
                        cmdSelect.CommandText += " AND CAST(tblTRLoading.statusOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                        cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.FromDate;
                        cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.ToDate;
                    }
                }
                if (!String.IsNullOrEmpty(WhereClause))
                {
                    cmdSelect.CommandText += WhereClause;
                }
                cmdSelect.CommandText += @" GROUP BY tblTRLoading.idLoading, tblTRLoading.loadingSlipNo, tblTRLoading.createdOn,tblTRLoading.statusOn, tblTRLoading.createdBy, tblUser.userDisplayName ,tblTRLoadingWeighing.vehicleNo,fromLocation.value,toLocation.value, tblTRLoading.unloadingNarration, tblTRLoading.weighingRemark, tblTRLoading.transferRequestId, materialType.value, materialSubType.value, tblTRLoading.materialTypeId, tblTRLoading.materialSubTypeId, tblTRLoading.fromLocationId, tblTRLoading.toLocationId, tblTRLoading.scheduleQty
                ORDER BY tblTRLoadingWeighing.vehicleNo";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@LoadingTypeId", System.Data.SqlDbType.Int).Value = (Int32)Constants.LoadingTypeIdE.LOADING;
                cmdSelect.Parameters.Add("@StatusIdStr", System.Data.SqlDbType.NVarChar).Value = loadingFilterTO.StatusIdStr;
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = new List<TblTRLoadingTO>();
                if (tblTRLoadingTODT != null)
                {
                    Int32 i = 0;
                    while (tblTRLoadingTODT.Read())
                    {
                        TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                        tblTRLoadingTO.SrNo = ++i;
                        if (tblTRLoadingTODT["idLoading"] != DBNull.Value)
                            tblTRLoadingTO.IdLoading = Convert.ToInt32(tblTRLoadingTODT["idLoading"].ToString());
                        if (tblTRLoadingTODT["loadingSlipNo"] != DBNull.Value)
                            tblTRLoadingTO.LoadingSlipNo = Convert.ToString(tblTRLoadingTODT["loadingSlipNo"].ToString());
                        if (tblTRLoadingTODT["createdOn"] != DBNull.Value)
                            tblTRLoadingTO.CreatedOn = Convert.ToDateTime(tblTRLoadingTODT["createdOn"].ToString());
                        if (tblTRLoadingTODT["statusOn"] != DBNull.Value)
                            tblTRLoadingTO.StatusOn = Convert.ToDateTime(tblTRLoadingTODT["statusOn"].ToString());
                        if (tblTRLoadingTODT["createdBy"] != DBNull.Value)
                            tblTRLoadingTO.CreatedBy = Convert.ToInt32(tblTRLoadingTODT["createdBy"].ToString());
                        if (tblTRLoadingTODT["createdByName"] != DBNull.Value)
                            tblTRLoadingTO.CreatedByName = Convert.ToString(tblTRLoadingTODT["createdByName"].ToString());
                        if (tblTRLoadingTODT["vehicleNo"] != DBNull.Value)
                            tblTRLoadingTO.VehicleNo = Convert.ToString(tblTRLoadingTODT["vehicleNo"].ToString());
                        if (tblTRLoadingTODT["FromLocation"] != DBNull.Value)
                            tblTRLoadingTO.FromLocation = Convert.ToString(tblTRLoadingTODT["FromLocation"].ToString());
                        if (tblTRLoadingTODT["ToLocation"] != DBNull.Value)
                            tblTRLoadingTO.ToLocation = Convert.ToString(tblTRLoadingTODT["ToLocation"].ToString());
                        if (tblTRLoadingTODT["LoadingNetWeight"] != DBNull.Value)
                            tblTRLoadingTO.LoadingNetWeight = Convert.ToDecimal(tblTRLoadingTODT["LoadingNetWeight"].ToString());
                        if (tblTRLoadingTODT["LoadingTareWeight"] != DBNull.Value)
                            tblTRLoadingTO.LoadingTareWeight = Convert.ToDecimal(tblTRLoadingTODT["LoadingTareWeight"].ToString());
                        if (tblTRLoadingTODT["LoadingGrossWeight"] != DBNull.Value)
                            tblTRLoadingTO.LoadingGrossWeight = Convert.ToDecimal(tblTRLoadingTODT["LoadingGrossWeight"].ToString());
                        if (tblTRLoadingTODT["unloadingNarration"] != DBNull.Value)
                            tblTRLoadingTO.UnloadingNarration = Convert.ToString(tblTRLoadingTODT["unloadingNarration"].ToString());
                        if (tblTRLoadingTODT["weighingRemark"] != DBNull.Value)
                            tblTRLoadingTO.WeighingRemark = Convert.ToString(tblTRLoadingTODT["weighingRemark"].ToString());
                        if (tblTRLoadingTODT["transferRequestId"] != DBNull.Value)
                            tblTRLoadingTO.TransferRequestId = Convert.ToInt32(tblTRLoadingTODT["transferRequestId"].ToString());
                        if (tblTRLoadingTODT["MaterialType"] != DBNull.Value)
                            tblTRLoadingTO.MaterialType = Convert.ToString(tblTRLoadingTODT["MaterialType"].ToString());
                        if (tblTRLoadingTODT["MaterialsSubType"] != DBNull.Value)
                            tblTRLoadingTO.MaterialsSubType = Convert.ToString(tblTRLoadingTODT["MaterialsSubType"].ToString());
                        if (tblTRLoadingTODT["materialTypeId"] != DBNull.Value)
                            tblTRLoadingTO.MaterialTypeId = Convert.ToInt32(tblTRLoadingTODT["materialTypeId"].ToString());
                        if (tblTRLoadingTODT["materialSubTypeId"] != DBNull.Value)
                            tblTRLoadingTO.MaterialSubTypeId = Convert.ToInt32(tblTRLoadingTODT["materialSubTypeId"].ToString());
                        if (tblTRLoadingTODT["fromLocationId"] != DBNull.Value)
                            tblTRLoadingTO.FromLocationId = Convert.ToInt32(tblTRLoadingTODT["fromLocationId"].ToString());
                        if (tblTRLoadingTODT["toLocationId"] != DBNull.Value)
                            tblTRLoadingTO.ToLocationId = Convert.ToInt32(tblTRLoadingTODT["toLocationId"].ToString());
                        if (tblTRLoadingTODT["scheduleQty"] != DBNull.Value)
                            tblTRLoadingTO.ScheduleQty = Convert.ToDouble(tblTRLoadingTODT["scheduleQty"].ToString());
                        tblTRLoadingTO.CreatedOnDateStr = tblTRLoadingTO.CreatedOn.ToString("dd/MM/yyyy");
                        tblTRLoadingTO.StatusOnDateStr = tblTRLoadingTO.StatusOn.ToString("dd/MM/yyyy");
                        tblTRLoadingTO.StatusOnTimeStr = tblTRLoadingTO.StatusOn.ToString("hh:mm tt");
                        tblTRLoadingTO.CreatedOnTimeStr = tblTRLoadingTO.CreatedOn.ToString("hh:mm tt");
                        
                        list.Add(tblTRLoadingTO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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

        public List<TblUnloadingSLATO > GetUnloadingSLAReportDetails(UnloadingSLAFilterTO unloadingSLAFilterTO, String WhereClause = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT concat( materialType.value,  '\ ' ,materialSubType.value ) Name,SUM(tblTransferRequest.qty)  Required,
                SUM(tblTRLoading.scheduleQty) Supply,(SUM(tblTRLoading.scheduleQty) /  SUM(tblTransferRequest.qty))*100 'SLA %' 
                FROM tblUnloadingSLA  tblUnloadingSLA
                LEFT JOIN tblTRLoading tblTRLoading on tblUnloadingSLA .loadingId =tblTRLoading.idLoading 
                LEFT JOIN tblTransferRequest tblTransferRequest on tblTRLoading.transferRequestId =tblTransferRequest.idTransferRequest 
                LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
                LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId  
                WHERE     tblUnloadingSLA.isActive = 1 AND tblTRLoading.isActive = 1 AND tblTransferRequest.isActive = 1   ";
                if (unloadingSLAFilterTO.FromDate != DateTime.MinValue && unloadingSLAFilterTO.ToDate != DateTime.MinValue)
                {
                    if (CheckDate(Convert.ToString(unloadingSLAFilterTO.FromDate)) == true && CheckDate(Convert.ToString(unloadingSLAFilterTO.ToDate)) == true)
                    {
                        cmdSelect.CommandText += " AND CAST(tblUnloadingSLA.createdOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                        cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.FromDate;
                        cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.ToDate;
                    }
                }
                if (!String.IsNullOrEmpty(WhereClause))
                {
                    cmdSelect.CommandText += WhereClause;
                }
                cmdSelect.CommandText += @" GROUP BY materialType.value, materialSubType.value ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text; 
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingSLATO> list = new List<TblUnloadingSLATO>();
                if (tblTRLoadingTODT != null)
                {
                    Int32 i = 0;
                    while (tblTRLoadingTODT.Read())
                    {
                        TblUnloadingSLATO tblUnloadingSLATO  = new TblUnloadingSLATO();
                        tblUnloadingSLATO.SrNo  = ++i;
                        if (tblTRLoadingTODT["Name"] != DBNull.Value)
                            tblUnloadingSLATO.ItemName  = Convert.ToString(tblTRLoadingTODT["Name"].ToString());
                        if (tblTRLoadingTODT["Required"] != DBNull.Value)
                            tblUnloadingSLATO.Required = Convert.ToDouble(tblTRLoadingTODT["Required"].ToString());
                        if (tblTRLoadingTODT["Supply"] != DBNull.Value)
                            tblUnloadingSLATO.Supply = Convert.ToDouble(tblTRLoadingTODT["Supply"].ToString());
                        if (tblTRLoadingTODT["SLA %"] != DBNull.Value)
                            tblUnloadingSLATO.SLAPer  = Convert.ToDouble(tblTRLoadingTODT["SLA %"].ToString());
                          
                        list.Add(tblUnloadingSLATO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblUnloadingSLATO> GetUnloadingSLAFurnaceReportDetails(UnloadingSLAFilterTO unloadingSLAFilterTO, String WhereClause = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"SELECT unloadingPoint.idGenericMaster , concat( materialType.value,  '\ ' ,materialSubType.value ) Name,SUM(tblTransferRequest.qty)  Required,
                                        SUM(tblTRLoading.scheduleQty) Supply,(SUM(tblTRLoading.scheduleQty) /  SUM(tblTransferRequest.qty))*100 'SLA %' 
                                        ,unloadingPoint.value  
                                        FROM tblUnloadingSLA  tblUnloadingSLA
                                        LEFT JOIN tblTRLoading tblTRLoading on tblUnloadingSLA .loadingId =tblTRLoading.idLoading 
                                        LEFT JOIN tblTransferRequest tblTransferRequest on tblTRLoading.transferRequestId =tblTransferRequest.idTransferRequest 
                                        LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
                                        LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId
                                        LEFT JOIN dimGenericMaster unloadingPoint on unloadingPoint.idGenericMaster = tblTRLoading.unloadingPointId 
                                        WHERE     tblUnloadingSLA.isActive = 1 AND tblTRLoading.isActive = 1 AND tblTransferRequest.isActive = 1 ";
                if (unloadingSLAFilterTO.FromDate != DateTime.MinValue && unloadingSLAFilterTO.ToDate != DateTime.MinValue)
                {
                    if (CheckDate(Convert.ToString(unloadingSLAFilterTO.FromDate)) == true && CheckDate(Convert.ToString(unloadingSLAFilterTO.ToDate)) == true)
                    {
                        cmdSelect.CommandText += " AND CAST(tblUnloadingSLA.createdOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                        cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.FromDate;
                        cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = unloadingSLAFilterTO.ToDate;
                    }
                }
                if (!String.IsNullOrEmpty(WhereClause))
                {
                    cmdSelect.CommandText += WhereClause;
                }
                cmdSelect.CommandText += @" Group By materialType.value, materialSubType.value,unloadingPoint.value ,unloadingPoint.idGenericMaster ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingSLATO> list = new List<TblUnloadingSLATO>();
                if (tblTRLoadingTODT != null)
                {
                    Int32 i = 0;
                    while (tblTRLoadingTODT.Read())
                    {
                        TblUnloadingSLATO tblUnloadingSLATO = new TblUnloadingSLATO();
                        tblUnloadingSLATO.SrNo = ++i;
                        if (tblTRLoadingTODT["Name"] != DBNull.Value)
                            tblUnloadingSLATO.ItemName = Convert.ToString(tblTRLoadingTODT["Name"].ToString());
                        if (tblTRLoadingTODT["Required"] != DBNull.Value)
                            tblUnloadingSLATO.Required = Convert.ToDouble(tblTRLoadingTODT["Required"].ToString());
                        if (tblTRLoadingTODT["Supply"] != DBNull.Value)
                            tblUnloadingSLATO.Supply = Convert.ToDouble(tblTRLoadingTODT["Supply"].ToString());
                        if (tblTRLoadingTODT["SLA %"] != DBNull.Value)
                            tblUnloadingSLATO.SLAPer  = Convert.ToDouble(tblTRLoadingTODT["SLA %"].ToString());
                        if (tblTRLoadingTODT["idGenericMaster"] != DBNull.Value)
                            tblUnloadingSLATO.IdGenericMaster  = Convert.ToInt16(tblTRLoadingTODT["idGenericMaster"].ToString()); 
                        if (tblTRLoadingTODT["value"] != DBNull.Value)
                            tblUnloadingSLATO.UnloadingPointName = Convert.ToString(tblTRLoadingTODT["value"].ToString());
                        list.Add(tblUnloadingSLATO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingTO> GetMaterialAndLocationWiseLoadingDtls(LoadingFilterTO loadingFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"select fromLocationId, toLocationId, materialTypeId, materialSubTypeId, fromLocation.value AS FromLocation,toLocation.value AS ToLocation, materialType.value as MaterialType, materialSubType.value as MaterialsSubType, SUM(tblTRLoading.scheduleQty) AS TotalScheduleQty, COUNT(*) AS TotalTransactionCnt
                FROM tblTRLoading 
                LEFT JOIN dimGenericMaster fromLocation ON fromLocation.idGenericMaster = tblTRLoading.fromLocationId
                LEFT JOIN dimGenericMaster toLocation ON toLocation.idGenericMaster = tblTRLoading.toLocationId
                LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId
                LEFT JOIN dimGenericMaster materialSubType on materialSubType.idGenericMaster = tblTRLoading.materialSubTypeId
                WHERE tblTRLoading.isActive = 1 AND tblTRLoading.statusId IN(@StatusIdStr) ";
                if (loadingFilterTO.FromDate != DateTime.MinValue && loadingFilterTO.ToDate != DateTime.MinValue)
                {
                    if (CheckDate(Convert.ToString(loadingFilterTO.FromDate)) == true && CheckDate(Convert.ToString(loadingFilterTO.ToDate)) == true)
                    {
                        cmdSelect.CommandText += " AND CAST(tblTRLoading.statusOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                        cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.FromDate;
                        cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = loadingFilterTO.ToDate;
                    }
                }
                cmdSelect.CommandText += @" GROUP BY fromLocationId, toLocationId, materialTypeId, materialSubTypeId, fromLocation.value,toLocation.value, materialType.value, materialSubType.value ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@StatusIdStr", System.Data.SqlDbType.NVarChar).Value = loadingFilterTO.StatusIdStr;
                SqlDataReader tblTRLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingTO> list = new List<TblTRLoadingTO>();
                if (tblTRLoadingTODT != null)
                {
                    Int32 i = 0;
                    while (tblTRLoadingTODT.Read())
                    {
                        TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                        tblTRLoadingTO.SrNo = ++i;
                        if (tblTRLoadingTODT["FromLocation"] != DBNull.Value)
                            tblTRLoadingTO.FromLocation = Convert.ToString(tblTRLoadingTODT["FromLocation"].ToString());
                        if (tblTRLoadingTODT["ToLocation"] != DBNull.Value)
                            tblTRLoadingTO.ToLocation = Convert.ToString(tblTRLoadingTODT["ToLocation"].ToString());
                        if (tblTRLoadingTODT["MaterialType"] != DBNull.Value)
                            tblTRLoadingTO.MaterialType = Convert.ToString(tblTRLoadingTODT["MaterialType"].ToString());
                        if (tblTRLoadingTODT["MaterialsSubType"] != DBNull.Value)
                            tblTRLoadingTO.MaterialsSubType = Convert.ToString(tblTRLoadingTODT["MaterialsSubType"].ToString());
                        if (tblTRLoadingTODT["materialTypeId"] != DBNull.Value)
                            tblTRLoadingTO.MaterialTypeId = Convert.ToInt32(tblTRLoadingTODT["materialTypeId"].ToString());
                        if (tblTRLoadingTODT["materialSubTypeId"] != DBNull.Value)
                            tblTRLoadingTO.MaterialSubTypeId = Convert.ToInt32(tblTRLoadingTODT["materialSubTypeId"].ToString());
                        if (tblTRLoadingTODT["fromLocationId"] != DBNull.Value)
                            tblTRLoadingTO.FromLocationId = Convert.ToInt32(tblTRLoadingTODT["fromLocationId"].ToString());
                        if (tblTRLoadingTODT["toLocationId"] != DBNull.Value)
                            tblTRLoadingTO.ToLocationId = Convert.ToInt32(tblTRLoadingTODT["toLocationId"].ToString());
                        if (tblTRLoadingTODT["TotalScheduleQty"] != DBNull.Value)
                            tblTRLoadingTO.TotalScheduleQty = Convert.ToDecimal(tblTRLoadingTODT["TotalScheduleQty"].ToString());
                        if (tblTRLoadingTODT["TotalTransactionCnt"] != DBNull.Value)
                            tblTRLoadingTO.TotalTransactionCnt = Convert.ToInt32(tblTRLoadingTODT["TotalTransactionCnt"].ToString());  
                        list.Add(tblTRLoadingTO);
                    }
                }
                if (tblTRLoadingTODT != null)
                    tblTRLoadingTODT.Dispose();
                if (list != null && list.Count > 0)
                    return list;
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
        public List<TblTRLoadingWeighingTO> GetWeighingDetails(Int32 LoadingId, String LoadingTypeIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandText = @"SELECT tblTRLoadingWeighing.*, tblWeighingMachine.machineName FROM tblTRLoadingWeighing tblTRLoadingWeighing
                LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tblTRLoadingWeighing.weighingMachineId
                WHERE tblTRLoadingWeighing.loadingId = " + LoadingId + " AND tblTRLoadingWeighing.LoadingTypeId IN(" + LoadingTypeIdStr + ") AND tblTRLoadingWeighing.isActive = 1 ORDER BY tblTRLoadingWeighing.idWeighing ASC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblTRLoadingWeighingTO> list = ConvertTblTRLoadingWeighingDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
                else
                    return null;
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
        public TblUnloadingSLATO GetUnloadingSLADetailsTO(Int32 idSLA)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectSLAQuery() + " AND tblUnloadingSLA.idSLA = " + idSLA;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingSLATO> list = ConvertTblUnloadingSLADTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list[0];
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

        public List<TblMaterialTypeReport> GetMaterialTypeReport()
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();
                cmdSelect.Connection = conn;
                cmdSelect.CommandText = "CREATE TABLE #finalResult (MaterialType nvarchar(50), FromLocation nvarchar(50), ToLocation nvarchar(50), CompletedVehicleCount Int default 0, CompletedQty int default 0, PendingVehicleCount Int default 0, PendingQty int default 0) " +
                    " " +
                    "select tblTransferRequest.idTransferRequest, tblTransferRequest.requestDisplayNo, materialType.value as MaterialType, fromLocation.value as FromLocation, tblTRLoading.statusId, vehicleStatusId as VehicleStatusId, " +
                    "toLocation.value as ToLocation, tblTRLoading.scheduleQty, tblTRLoadingWeighing.netWeight as NetWeight, tblVehicle.idVehicle,tblVehicle.vehicleNo, " +
                    "tblTRLoading.createdOn as CreatedOn, tblTRLoading.updatedOn as StatusChangedOn, tblTRLoading.idLoading " +
                    "INTO #tempLoadingData from tblVehicle tblVehicle " + 
                    "LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.vehicleId = tblVehicle.idVehicle and tblTRLoading.isActive = 1 " +
                    "LEFT JOIN tblTransferRequest tblTransferRequest on tblTransferRequest.idTransferRequest = tblTRLoading.transferRequestId " +
                    "LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.materialTypeId " +
                    "LEFT JOIN dimGenericMaster fromLocation on fromLocation.idGenericMaster = tblTRLoading.fromLocationId " +
                    "LEFT JOIN dimGenericMaster toLocation on toLocation.idGenericMaster = tblTRLoading.toLocationId " +
                    "LEFT JOIN tblTRLoadingWeighing tblTRLoadingWeighing on tblTRLoadingWeighing.loadingId = tblTRLoading.idLoading and tblTRLoadingWeighing.netWeight != 0 and tblTRLoadingWeighing.loadingTypeId = 2 " +
                    "where materialType.value is not null " +

                    " " +

                    "select tblTransferRequest.idTransferRequest, tblTransferRequest.requestDisplayNo, materialType.value as MaterialType, fromLocation.value as FromLocation, tblTRLoading.statusId, vehicleStatusId as VehicleStatusId," +
                    "toLocation.value as ToLocation, tblTRLoading.scheduleQty, tblTRLoadingWeighing.netWeight as NetWeight, tblVehicle.idVehicle,tblVehicle.vehicleNo, " +
                    "tblTRLoading.createdOn as CreatedOn, tblTRLoading.updatedOn as StatusChangedOn, tblTRLoading.idLoading " +
                    "INTO #tempUnloadingData " +
                    "from tblVehicle tblVehicle " +
                    "LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.vehicleId = tblVehicle.idVehicle and tblTRLoading.isActive = 1 " +
                    "LEFT JOIN tblTransferRequest tblTransferRequest on tblTransferRequest.idTransferRequest = tblTRLoading.transferRequestId " +
                    "LEFT JOIN dimGenericMaster materialType on materialType.idGenericMaster = tblTRLoading.unMaterialTypeId " +
                    "LEFT JOIN dimGenericMaster fromLocation on fromLocation.idGenericMaster = tblTRLoading.fromLocationId " +
                    "LEFT JOIN dimGenericMaster toLocation on toLocation.idGenericMaster = tblTRLoading.toLocationId " +
                    "LEFT JOIN tblTRLoadingWeighing tblTRLoadingWeighing on tblTRLoadingWeighing.loadingId = tblTRLoading.idLoading and tblTRLoadingWeighing.netWeight != 0 and tblTRLoadingWeighing.loadingTypeId = 2 " +
                    "where materialType.value is not null " +
                    " " +

                    "select a.idTransferRequest, a.requestDisplayNo, COALESCE(b.MaterialType, a.MaterialType) as MaterialType, a.FromLocation, a.statusId, a.VehicleStatusId, a.ToLocation, a.scheduleQty, a.NetWeight, a.idVehicle, a.vehicleNo, a.CreatedOn, a.StatusChangedOn, a.idLoading into #tempData from #tempLoadingData a left join #tempUnloadingData b ON a.idLoading = b.idLoading order by idLoading desc " +
                    " " +

                    "INSERT into #finalResult (MaterialType,FromLocation,ToLocation) " +
                    " " +
                    "Select distinct MaterialType,FromLocation,ToLocation from #tempData " +
                    " " +

                    "Update #finalResult Set PendingVehicleCount = A.pendingVechicleCount, PendingQty = A.PendingQty FROM #finalResult t " +
                    "inner join (select MaterialType, FromLocation, ToLocation, Count(idVehicle) AS pendingVechicleCount, round(sum(scheduleQty),0) as PendingQty " +
                    "from #tempData where statusId NOT IN (2312,2313) and VehicleStatusId IN (2209,2314) group by MaterialType,FromLocation,ToLocation) A " +
                    "ON A.FromLocation = t.FromLocation and A.ToLocation = t.ToLocation and A.MaterialType = t.MaterialType " +
                    " " +
                    "Update #finalResult Set CompletedVehicleCount = A.completedCount, CompletedQty = A.CompletedQty FROM #finalResult t " +
                    "inner join (select MaterialType, FromLocation, ToLocation, Count(idVehicle) as completedCount, round(sum(NetWeight)/1000,0) as CompletedQty " +
                    "from #tempData where statusId = 2312 and StatusChangedOn BETWEEN CAST(GETDATE() AS date) and DATEADD(day, +1, CAST(GETDATE() AS date)) group by MaterialType,FromLocation,ToLocation) A " +
                    "ON A.FromLocation = t.FromLocation and A.ToLocation = t.ToLocation and A.MaterialType = t.MaterialType " +
                    " " +
                    "delete from #finalResult where CompletedVehicleCount = 0 and PendingVehicleCount = 0 " +
                    " " +
                    "select * from #finalResult " +
                    " " +
                    "Drop table #finalResult " +
                    " " +
                    "Drop table #tempLoadingData " +
                    "Drop table #tempUnloadingData " +
                    "Drop table #tempData ";

                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMaterialTypeReport> list = ConvertTblMaterialTypeReportDTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list;
                else
                    return null;
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

        public TblUnloadingSLATO GetUnloadingSLADetailsTOByLoadingId(Int32 loadingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectSLAQuery() + " AND tblUnloadingSLA.loadingId = " + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUnloadingSLATO> list = ConvertTblUnloadingSLADTToList(rdr);
                if (rdr != null)
                    rdr.Dispose();
                if (list != null && list.Count > 0)
                    return list[0];
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
        
        #endregion
        #region Insert
        public int InsertLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTRLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int InsertFinalLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteFinalInsertionCommand(tblTRLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int InsertSLA(TblUnloadingSLATO tblUnloadingSLATO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @" INSERT INTO [tblUnloadingSLA]( " +
                "  [displayNo]" +
                " ,[loadingId]" +
                " ,[vehicleId]" +
                " ,[materialTypeId]" +
                " ,[slaUnloadingId]" +
                " ,[mixMaterialId]" +
                " ,[overSizePer]" +
                " ,[waste]" +
                " ,[offChemistryId]" +
                " ,[density]" +
                " ,[createdBy]" +
                " ,[createdOn]" +
                " ,[isActive]" +
                " )" +
                " VALUES (" +
                " @displayNo " +
                " ,@loadingId " +
                " ,@vehicleId " +
                " ,@materialTypeId " +
                " ,@slaUnloadingId " +
                " ,@mixMaterialId " +
                " ,@overSizePer " +
                " ,@waste " +
                " ,@offChemistryId " +
                " ,@density " +
                " ,@createdBy " +
                " ,@createdOn " +
                " ,@isActive " +
                " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";
                cmdInsert.Parameters.Add("@displayNo", System.Data.SqlDbType.NVarChar).Value = tblUnloadingSLATO.DisplayNo;
                cmdInsert.Parameters.Add("@loadingId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.LoadingId);
                cmdInsert.Parameters.Add("@vehicleId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.VehicleId);
                cmdInsert.Parameters.Add("@materialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.MaterialTypeId);
                cmdInsert.Parameters.Add("@slaUnloadingId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.SlaUnloadingId);
                cmdInsert.Parameters.Add("@mixMaterialId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.MixMaterialId);
                cmdInsert.Parameters.Add("@overSizePer", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.OverSizePer);
                cmdInsert.Parameters.Add("@waste", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.Waste);
                cmdInsert.Parameters.Add("@offChemistryId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.OffChemistryId);
                cmdInsert.Parameters.Add("@density", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.Density);
                cmdInsert.Parameters.Add("@createdBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.CreatedBy);
                cmdInsert.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.CreatedOn);
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblUnloadingSLATO.IsActive);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    tblUnloadingSLATO.IdSLA = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int InsertWeighing(TblTRLoadingWeighingTO tblTRLoadingWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @" INSERT INTO [tblTRLoadingWeighing]( " +
                "  [vehicleNo]" +
                " ,[loadingId]" +
                " ,[loadingTypeId]" +
                " ,[grossWeight]" +
                " ,[actualWeight]" +
                " ,[netWeight]" +
                " ,[rstNumber]" +
                " ,[weighingStageId]" +
                " ,[weighingMachineId]" +
                " ,[weighingMeasureTypeId]" +
                " ,[createdBy]" +
                " ,[createdOn]" +
                " ,[isActive]" +
                " )" +
                " VALUES (" +
                " @vehicleNo " +
                " ,@loadingId " +
                " ,@loadingTypeId " +
                " ,@grossWeight " +
                " ,@actualWeight " +
                " ,@netWeight " +
                " ,@rstNumber " +
                " ,@weighingStageId " +
                " ,@weighingMachineId " +
                " ,@weighingMeasureTypeId " +
                " ,@createdBy " +
                " ,@createdOn " +
                " ,@isActive " +
                " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";
                cmdInsert.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = tblTRLoadingWeighingTO.VehicleNo;
                cmdInsert.Parameters.Add("@loadingId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.LoadingId);
                cmdInsert.Parameters.Add("@loadingTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.LoadingTypeId);
                cmdInsert.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = tblTRLoadingWeighingTO.GrossWeight;
                cmdInsert.Parameters.Add("@actualWeight", System.Data.SqlDbType.Decimal).Value = tblTRLoadingWeighingTO.ActualWeight;
                cmdInsert.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = tblTRLoadingWeighingTO.NetWeight;
                cmdInsert.Parameters.Add("@rstNumber", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.RstNumber);
                cmdInsert.Parameters.Add("@weighingStageId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.WeighingStageId);
                cmdInsert.Parameters.Add("@weighingMachineId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.WeighingMachineId);
                cmdInsert.Parameters.Add("@weighingMeasureTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.WeighingMeasureTypeId);
                cmdInsert.Parameters.Add("@createdBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.CreatedBy);
                cmdInsert.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.CreatedOn);
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingWeighingTO.IsActive);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    tblTRLoadingWeighingTO.IdWeighing = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int InsertFinalWeighing(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @"INSERT INTO finalTRLoadingWeighing (weighingId, vehicleNo, loadingId, loadingTypeId,	grossWeight, actualWeight, netWeight, rstNumber, weighingStageId, weighingMachineId, weighingMeasureTypeId,	createdBy, createdOn, updatedBy, updatedOn, isActive)
                SELECT idWeighing, vehicleNo, loadingId, loadingTypeId,	grossWeight, actualWeight, netWeight, rstNumber, weighingStageId, weighingMachineId, weighingMeasureTypeId,	createdBy, createdOn, updatedBy, updatedOn, isActive
                FROM tblTRLoadingWeighing
                WHERE tblTRLoadingWeighing.loadingId IN("+ loadingIdStr + ")";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                if (cmdInsert.ExecuteNonQuery() > 0)
                {
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int InsertFinalUnloadingSLA(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @"INSERT INTO finalUnloadingSLA (idSLA, displayNo, loadingId,	vehicleId, materialTypeId, slaUnloadingId, mixMaterialId, overSizePer, waste, offChemistryId, density, createdBy, createdOn, isActive)
                SELECT idSLA, displayNo, loadingId,	vehicleId, materialTypeId, slaUnloadingId, mixMaterialId, overSizePer, waste, offChemistryId, density, createdBy, createdOn, isActive
                FROM tblUnloadingSLA
                WHERE tblUnloadingSLA.loadingId IN(" + loadingIdStr + ")";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                if (cmdInsert.ExecuteNonQuery() > 0)
                {
                    return 1;
                }
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteFinalInsertionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [finalTRLoading]( " +
            "  [loadingId]" +
            " ,[loadingSlipNo]" +
            " ,[loadingTypeId]" +
            " ,[transferRequestId]" +
            " ,[requestDisplayNo]" +
            " ,[fromLocationId]" +
            " ,[fromLocation]" +
            " ,[toLocationId]" +
            " ,[toLocation]" +
            " ,[materialTypeId]" +
            " ,[materialType]" +
            " ,[materialSubTypeId]" +
            " ,[materialsSubType]" +
            " ,[vehicleId]" +
            " ,[vehicleNo]" +
            " ,[scheduleQty]" +
            " ,[narration]" +
            " ,[driverName]" +
            " ,[statusId]" +
            " ,[statusDesc]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[statusBy]" +
            " ,[statusOn]" +
            " ,[isActive]" +
            " ,[unloadingPointId]" +
            " ,[createdByName]" +
            " ,[unloadingPoint]" +
            " ,[transactionCloseRemark]" +
            " ,[unMaterialTypeId]" +
            " ,[unMaterialSubTypeId]" +
            " ,[isReviewUnloading]" +
            " ,[weighingRemark]" +
            " ,[unloadingNarration]" +
            " )" +
            " VALUES (" +
            "  @LoadingId " +
            " ,@LoadingSlipNo " +
            " ,@LoadingTypeId " +
            " ,@TransferRequestId " +
            " ,@RequestDisplayNo " +
            " ,@FromLocationId " +
            " ,@FromLocation " +
            " ,@ToLocationId " +
            " ,@ToLocation " +
            " ,@MaterialTypeId " +
            " ,@MaterialType " +
            " ,@MaterialSubTypeId " +
            " ,@MaterialsSubType " +
            " ,@VehicleId " +
            " ,@VehicleNo " +
            " ,@ScheduleQty " +
            " ,@Narration " +
            " ,@DriverName " +
            " ,@StatusId " +
            " ,@StatusDesc " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@StatusBy " +
            " ,@StatusOn " +
            " ,@IsActive " +
            " ,@UnloadingPointId " +
            " ,@createdByName " +
            " ,@unloadingPoint " +
            " ,@transactionCloseRemark " +
            " ,@unMaterialTypeId " +
            " ,@unMaterialSubTypeId " +
            " ,@isReviewUnloading " +
            " ,@weighingRemark " +
            " ,@unloadingNarration " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
            cmdInsert.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.NVarChar).Value = tblTRLoadingTO.LoadingSlipNo;
            cmdInsert.Parameters.Add("@LoadingTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.LoadingTypeId);
            cmdInsert.Parameters.Add("@TransferRequestId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.TransferRequestId);
            cmdInsert.Parameters.Add("@RequestDisplayNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.RequestDisplayNo);
            cmdInsert.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.FromLocationId);
            cmdInsert.Parameters.Add("@FromLocation", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.FromLocation);
            cmdInsert.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ToLocationId);
            cmdInsert.Parameters.Add("@ToLocation", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ToLocation);
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialTypeId);
            cmdInsert.Parameters.Add("@MaterialType", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialType);
            cmdInsert.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialSubTypeId);
            cmdInsert.Parameters.Add("@MaterialsSubType", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialSubType);
            cmdInsert.Parameters.Add("@VehicleId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.VehicleId);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.VehicleNo);
            cmdInsert.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ScheduleQty);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.Narration);
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusId);
            cmdInsert.Parameters.Add("@StatusDesc", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusDesc);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedOn);
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusBy);
            cmdInsert.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsActive);
            cmdInsert.Parameters.Add("@UnloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            cmdInsert.Parameters.Add("@createdByName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedByName);
            cmdInsert.Parameters.Add("@unloadingPoint", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPoint);
            cmdInsert.Parameters.Add("@transactionCloseRemark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.TransactionCloseRemark);
            cmdInsert.Parameters.Add("@unMaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialTypeId);
            cmdInsert.Parameters.Add("@unMaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialSubTypeId);
            cmdInsert.Parameters.Add("@isReviewUnloading", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsReviewUnloading);
            cmdInsert.Parameters.Add("@weighingRemark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.WeighingRemark);
            cmdInsert.Parameters.Add("@unloadingNarration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingNarration);
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@updatedByName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UpdatedByName);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                return 1;
            }
            else return 0;
        }
        public int ExecuteInsertionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTRLoading]( " +
            "  [loadingSlipNo]" +
            " ,[loadingTypeId]" +
            " ,[transferRequestId]" +
            " ,[fromLocationId]" +
            " ,[toLocationId]" +
            " ,[materialTypeId]" +
            " ,[materialSubTypeId]" +
            " ,[vehicleId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[statusBy]" +
            " ,[createdOn]" +
            " ,[statusOn]" +
            " ,[isActive]" +
            " ,[scheduleQty]" +
            " ,[narration]" +
            " ,[driverName]" +
            " ,[unloadingPointId]" +
            " )" +
            " Output Inserted.idLoading " +
            " VALUES (" +
            " @LoadingSlipNo " +
            " ,@LoadingTypeId " +
            " ,@TransferRequestId " +
            " ,@FromLocationId " +
            " ,@ToLocationId " +
            " ,@MaterialTypeId " +
            " ,@MaterialSubTypeId " +
            " ,@VehicleId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@StatusBy " +
            " ,@CreatedOn " +
            " ,@StatusOn " +
            " ,@IsActive " +
            " ,@ScheduleQty " +
            " ,@Narration " +
            " ,@DriverName " +
            " ,@UnloadingPointId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.NVarChar).Value = tblTRLoadingTO.LoadingSlipNo;
            cmdInsert.Parameters.Add("@LoadingTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.LoadingTypeId);
            cmdInsert.Parameters.Add("@TransferRequestId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.TransferRequestId);
            cmdInsert.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.FromLocationId);
            cmdInsert.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ToLocationId);
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialTypeId);
            cmdInsert.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialSubTypeId);
            cmdInsert.Parameters.Add("@VehicleId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.VehicleId);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedBy);
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedOn);
            cmdInsert.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsActive);
            cmdInsert.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ScheduleQty);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.Narration);
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@UnloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblTRLoadingTO.IdLoading = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        public int InsertLoadingHistory(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteLodingHistoryInsertionCommand(tblTRLoadingTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteLodingHistoryInsertionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTRLoadingHistory]( " +
            "  [loadingId]" +
            " ,[fromLocationId]" +
            " ,[toLocationId]" +
            " ,[materialTypeId]" +
            " ,[materialSubTypeId]" +
            " ,[vehicleId]" +
            " ,[scheduleQty]" +
            " ,[narration]" +
            " ,[driverName]" +
            " ,[statusId]" +
            " ,[statusBy]" +
            " ,[statusOn]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[unloadingPointId]" +
            " ,[unloadingNarration]" +
            " ,[unMaterialTypeId]" +
            " ,[unMaterialSubTypeId]" +
            " ,[isReviewUnloading]" +
            " )" +
            " VALUES (" +
            " @LoadingId " +
            " ,@FromLocationId " +
            " ,@ToLocationId " +
            " ,@MaterialTypeId " +
            " ,@MaterialSubTypeId " +
            " ,@VehicleId " +
            " ,@ScheduleQty " +
            " ,@Narration " +
            " ,@DriverName " +
            " ,@StatusId " +
            " ,@StatusBy " +
            " ,@StatusOn " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedBy " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@UnloadingPointId " +
            " ,@UnloadingNarration " +
            " ,@UnMaterialTypeId " +
            " ,@UnMaterialSubTypeId " +
            " ,@IsReviewUnloading " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
            cmdInsert.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.FromLocationId);
            cmdInsert.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ToLocationId);
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialTypeId);
            cmdInsert.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialSubTypeId);
            cmdInsert.Parameters.Add("@VehicleId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.VehicleId);
            cmdInsert.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ScheduleQty);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.Narration);
            cmdInsert.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusId);
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusBy);
            cmdInsert.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.StatusOn);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsActive);
            cmdInsert.Parameters.Add("@UnloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            cmdInsert.Parameters.Add("@UnMaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialTypeId);
            cmdInsert.Parameters.Add("@UnMaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialSubTypeId);
            cmdInsert.Parameters.Add("@UnloadingNarration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingNarration);
            cmdInsert.Parameters.Add("@IsReviewUnloading", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsReviewUnloading);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                return 1;
            }
            else return 0;
        }
        public int InsertFinalLoadingHistory(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteFinalLodingHistoryInsertionCommand(loadingIdStr, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteFinalLodingHistoryInsertionCommand(String loadingIdStr, SqlCommand cmdInsert)
        {
            String sqlQuery = @"INSERT INTO finalTRLoadingHistory (loadingHistoryId,loadingId, fromLocationId, toLocationId, materialTypeId, materialSubTypeId, vehicleId,	scheduleQty, narration,	driverName,	statusId, createdBy, createdOn, updatedBy, updatedOn, isActive, statusOn, statusBy, unloadingPointId, unloadingNarration, unMaterialTypeId, unMaterialSubTypeId, isReviewUnloading ,statusDesc,statusByName)
            SELECT idLoadingHistory,loadingId, fromLocationId, toLocationId, materialTypeId, materialSubTypeId, vehicleId,	scheduleQty, narration,	driverName,	statusId, createdBy, createdOn, updatedBy, updatedOn, tblTRLoadingHistory.isActive, statusOn, statusBy, unloadingPointId, unloadingNarration, unMaterialTypeId, unMaterialSubTypeId, isReviewUnloading ,statusDesc,userDisplayName
            FROM tblTRLoadingHistory tblTRLoadingHistory
            LEFT JOIN dimStatus dimStatus on dimStatus.idStatus = tblTRLoadingHistory.statusId
            LEFT JOIN tblUser tblUser on tblUser.idUser = tblTRLoadingHistory.statusBy
            WHERE tblTRLoadingHistory.loadingId IN("+ loadingIdStr + ")";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            if (cmdInsert.ExecuteNonQuery() > 0)
            {
                return 1;
            }
            else return 0;
        }
        #endregion
        #region Update
        public int UpdateLoadingStatus(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblTRLoading] SET " +
                "  [statusId] = @StatusId" +
                " ,[statusBy]= @StatusBy" +
                " ,[statusOn] = @StatusOn";
                if(tblTRLoadingTO.StatusId == (Int32)Constants.LoadingStatusE.REJECTED)
                {
                    sqlQuery += " ,[transactionCloseRemark] = @TransactionCloseRemark";
                    cmdUpdate.Parameters.Add("@TransactionCloseRemark", System.Data.SqlDbType.NVarChar).Value = tblTRLoadingTO.TransactionCloseRemark;
                }
                sqlQuery += " WHERE [idLoading] = @IdLoading ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusBy;
                cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.StatusOn;
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
        public int UpdateLoadingVehicle(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblTRLoading] SET " +
                 " [vehicleId]= @VehicleId" +
                 " ,[updatedBy]= @updatedBy" +
                 " ,[updatedOn]= @UpdatedOn";
                sqlQuery += " WHERE [idLoading] = @IdLoading ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@VehicleId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.VehicleId;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.UpdatedOn;
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
        public int UpdateWeighingRemark(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblTRLoading] SET " +
                 " [weighingRemark]= @WeighingRemark";
                sqlQuery += " WHERE [idLoading] = @IdLoading ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@WeighingRemark", System.Data.SqlDbType.NVarChar).Value = tblTRLoadingTO.WeighingRemark;
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
        public int UpdateLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTRLoadingTO, cmdUpdate);
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
        public int ExecuteUpdationCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTRLoading] SET " +
            "  [loadingTypeId]= @LoadingTypeId" +
            " ,[transferRequestId]= @TransferRequestId" +
            " ,[fromLocationId]= @FromLocationId" +
            " ,[toLocationId]= @ToLocationId" +
            " ,[materialTypeId]= @MaterialTypeId" +
            " ,[materialSubTypeId]= @MaterialSubTypeId" +
            " ,[statusId]= @StatusId" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[statusBy]= @StatusBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[statusOn]= @StatusOn" +
            " ,[scheduleQty]= @ScheduleQty" +
            " ,[narration]= @Narration" +
            " ,[driverName] = @DriverName" +
            " ,[unloadingPointId] = @unloadingPointId" +
            " ,[unMaterialTypeId] = @unMaterialTypeId" +
            " ,[unMaterialSubTypeId] = @unMaterialSubTypeId" +
            " ,[unloadingNarration] = @unloadingNarration" +
            " ,[isReviewUnloading] = @isReviewUnloading" +
            " WHERE  [idLoading] = @IdLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
            cmdUpdate.Parameters.Add("@LoadingTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.LoadingTypeId);
            cmdUpdate.Parameters.Add("@TransferRequestId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.TransferRequestId);
            cmdUpdate.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.FromLocationId);
            cmdUpdate.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ToLocationId);
            cmdUpdate.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialTypeId);
            cmdUpdate.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.MaterialSubTypeId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.StatusOn;
            cmdUpdate.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ScheduleQty);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.Narration);
            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.DriverName);
            cmdUpdate.Parameters.Add("@unloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            cmdUpdate.Parameters.Add("@unMaterialTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialTypeId);
            cmdUpdate.Parameters.Add("@unMaterialSubTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnMaterialSubTypeId);
            cmdUpdate.Parameters.Add("@unloadingNarration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingNarration);
            cmdUpdate.Parameters.Add("@isReviewUnloading", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.IsReviewUnloading);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        #region ConvertDT
        public List<TblTRLoadingTO> ConvertDTToList(SqlDataReader tblTRLoadingTODT)
        {
            List<TblTRLoadingTO> tblTRLoadingTOList = new List<TblTRLoadingTO>();
            if (tblTRLoadingTODT != null)
            {
                Int32 i = 0;
                while (tblTRLoadingTODT.Read())
                {
                    TblTRLoadingTO tblTRLoadingTO = new TblTRLoadingTO();
                    tblTRLoadingTO.SrNo = ++i;
                    if (tblTRLoadingTODT["idLoading"] != DBNull.Value)
                        tblTRLoadingTO.IdLoading = Convert.ToInt32(tblTRLoadingTODT["idLoading"].ToString());
                    if (tblTRLoadingTODT["loadingSlipNo"] != DBNull.Value)
                        tblTRLoadingTO.LoadingSlipNo = Convert.ToString(tblTRLoadingTODT["loadingSlipNo"].ToString());
                    if (tblTRLoadingTODT["loadingTypeId"] != DBNull.Value)
                        tblTRLoadingTO.LoadingTypeId = Convert.ToInt32(tblTRLoadingTODT["loadingTypeId"].ToString());
                    if (tblTRLoadingTODT["transferRequestId"] != DBNull.Value)
                        tblTRLoadingTO.TransferRequestId = Convert.ToInt32(tblTRLoadingTODT["transferRequestId"].ToString());
                    if (tblTRLoadingTODT["fromLocationId"] != DBNull.Value)
                        tblTRLoadingTO.FromLocationId = Convert.ToInt32(tblTRLoadingTODT["fromLocationId"].ToString());
                    if (tblTRLoadingTODT["toLocationId"] != DBNull.Value)
                        tblTRLoadingTO.ToLocationId = Convert.ToInt32(tblTRLoadingTODT["toLocationId"].ToString());
                    if (tblTRLoadingTODT["materialTypeId"] != DBNull.Value)
                        tblTRLoadingTO.MaterialTypeId = Convert.ToInt32(tblTRLoadingTODT["materialTypeId"].ToString());
                    if (tblTRLoadingTODT["materialSubTypeId"] != DBNull.Value)
                        tblTRLoadingTO.MaterialSubTypeId = Convert.ToInt32(tblTRLoadingTODT["materialSubTypeId"].ToString());
                    if (tblTRLoadingTODT["vehicleId"] != DBNull.Value)
                        tblTRLoadingTO.VehicleId = Convert.ToInt32(tblTRLoadingTODT["vehicleId"].ToString());
                    if (tblTRLoadingTODT["scheduleQty"] != DBNull.Value)
                        tblTRLoadingTO.ScheduleQty = Convert.ToDouble(tblTRLoadingTODT["scheduleQty"].ToString());
                    if (tblTRLoadingTODT["narration"] != DBNull.Value)
                        tblTRLoadingTO.Narration = Convert.ToString(tblTRLoadingTODT["narration"].ToString());
                    if (tblTRLoadingTODT["driverName"] != DBNull.Value)
                        tblTRLoadingTO.DriverName = Convert.ToString(tblTRLoadingTODT["driverName"].ToString());
                    if (tblTRLoadingTODT["statusId"] != DBNull.Value)
                        tblTRLoadingTO.StatusId = Convert.ToInt32(tblTRLoadingTODT["statusId"].ToString());
                    if (tblTRLoadingTODT["createdBy"] != DBNull.Value)
                        tblTRLoadingTO.CreatedBy = Convert.ToInt32(tblTRLoadingTODT["createdBy"].ToString());
                    if (tblTRLoadingTODT["createdOn"] != DBNull.Value)
                        tblTRLoadingTO.CreatedOn = Convert.ToDateTime(tblTRLoadingTODT["createdOn"].ToString());
                    if (tblTRLoadingTODT["updatedBy"] != DBNull.Value)
                        tblTRLoadingTO.UpdatedBy = Convert.ToInt32(tblTRLoadingTODT["updatedBy"].ToString());
                    if (tblTRLoadingTODT["updatedOn"] != DBNull.Value)
                        tblTRLoadingTO.UpdatedOn = Convert.ToDateTime(tblTRLoadingTODT["updatedOn"].ToString());
                    if (tblTRLoadingTODT["isActive"] != DBNull.Value)
                        tblTRLoadingTO.IsActive = Convert.ToInt32(tblTRLoadingTODT["isActive"].ToString());
                    if (tblTRLoadingTODT["statusBy"] != DBNull.Value)
                        tblTRLoadingTO.StatusBy = Convert.ToInt32(tblTRLoadingTODT["statusBy"].ToString());
                    if (tblTRLoadingTODT["statusOn"] != DBNull.Value)
                        tblTRLoadingTO.StatusOn = Convert.ToDateTime(tblTRLoadingTODT["statusOn"].ToString());
                    if (tblTRLoadingTODT["unloadingPointId"] != DBNull.Value)
                        tblTRLoadingTO.UnloadingPointId = Convert.ToInt32(tblTRLoadingTODT["unloadingPointId"].ToString());
                    if (tblTRLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblTRLoadingTO.VehicleNo = Convert.ToString(tblTRLoadingTODT["vehicleNo"].ToString());
                    if (tblTRLoadingTODT["vehicleStatusId"] != DBNull.Value)
                        tblTRLoadingTO.VehicleStatusId = Convert.ToInt32(tblTRLoadingTODT["vehicleStatusId"].ToString());
                    if (tblTRLoadingTODT["requestDisplayNo"] != DBNull.Value)
                        tblTRLoadingTO.RequestDisplayNo = Convert.ToString(tblTRLoadingTODT["requestDisplayNo"].ToString());
                    if (tblTRLoadingTODT["MaterialType"] != DBNull.Value)
                        tblTRLoadingTO.MaterialType = Convert.ToString(tblTRLoadingTODT["MaterialType"].ToString());
                    if (tblTRLoadingTODT["MaterialsSubType"] != DBNull.Value)
                        tblTRLoadingTO.MaterialsSubType = Convert.ToString(tblTRLoadingTODT["MaterialsSubType"].ToString());
                    if (tblTRLoadingTODT["FromLocation"] != DBNull.Value)
                        tblTRLoadingTO.FromLocation = Convert.ToString(tblTRLoadingTODT["FromLocation"].ToString());
                    if (tblTRLoadingTODT["ToLocation"] != DBNull.Value)
                        tblTRLoadingTO.ToLocation = Convert.ToString(tblTRLoadingTODT["ToLocation"].ToString());
                    if (tblTRLoadingTODT["UnloadingPoint"] != DBNull.Value)
                        tblTRLoadingTO.UnloadingPoint = Convert.ToString(tblTRLoadingTODT["UnloadingPoint"].ToString());
                    if (tblTRLoadingTODT["statusDesc"] != DBNull.Value)
                        tblTRLoadingTO.StatusDesc = Convert.ToString(tblTRLoadingTODT["statusDesc"].ToString());
                    if (tblTRLoadingTODT["CreatedByName"] != DBNull.Value)
                        tblTRLoadingTO.CreatedByName = Convert.ToString(tblTRLoadingTODT["CreatedByName"].ToString());
                    if (tblTRLoadingTODT["transactionCloseRemark"] != DBNull.Value)
                        tblTRLoadingTO.TransactionCloseRemark = Convert.ToString(tblTRLoadingTODT["transactionCloseRemark"].ToString());
                    if (tblTRLoadingTODT["RequestUserName"] != DBNull.Value)
                        tblTRLoadingTO.RequestUserName = Convert.ToString(tblTRLoadingTODT["RequestUserName"].ToString());
                    if (tblTRLoadingTODT["RequestCreatedOn"] != DBNull.Value)
                        tblTRLoadingTO.RequestCreatedOn = Convert.ToDateTime(tblTRLoadingTODT["RequestCreatedOn"].ToString());
                    if (tblTRLoadingTODT["idVehicle"] != DBNull.Value)
                        tblTRLoadingTO.IdVehicle = Convert.ToInt32(tblTRLoadingTODT["idVehicle"].ToString());
                    if (tblTRLoadingTODT["VehicleStatus"] != DBNull.Value)
                        tblTRLoadingTO.VehicleStatus = Convert.ToString(tblTRLoadingTODT["VehicleStatus"].ToString());
                    if (tblTRLoadingTODT["unloadingNarration"] != DBNull.Value)
                        tblTRLoadingTO.UnloadingNarration = Convert.ToString(tblTRLoadingTODT["unloadingNarration"].ToString());
                    if (tblTRLoadingTODT["unMaterialTypeId"] != DBNull.Value)
                        tblTRLoadingTO.UnMaterialTypeId = Convert.ToInt32(tblTRLoadingTODT["unMaterialTypeId"].ToString());
                    if (tblTRLoadingTODT["unMaterialSubTypeId"] != DBNull.Value)
                        tblTRLoadingTO.UnMaterialSubTypeId = Convert.ToInt32(tblTRLoadingTODT["unMaterialSubTypeId"].ToString());
                    if (tblTRLoadingTODT["isReviewUnloading"] != DBNull.Value)
                        tblTRLoadingTO.IsReviewUnloading = Convert.ToInt32(tblTRLoadingTODT["isReviewUnloading"].ToString());
                    if (tblTRLoadingTODT["idSLA"] != DBNull.Value)
                        tblTRLoadingTO.IdSLA = Convert.ToInt32(tblTRLoadingTODT["idSLA"].ToString());
                    if (tblTRLoadingTODT["UnloadingMaterialType"] != DBNull.Value)
                        tblTRLoadingTO.UnloadingMaterialType = Convert.ToString(tblTRLoadingTODT["UnloadingMaterialType"].ToString());
                    if (tblTRLoadingTODT["UnloadingMaterialsSubType"] != DBNull.Value)
                        tblTRLoadingTO.UnloadingMaterialsSubType = Convert.ToString(tblTRLoadingTODT["UnloadingMaterialsSubType"].ToString());
                    tblTRLoadingTOList.Add(tblTRLoadingTO);
                }
            }
            return tblTRLoadingTOList;
        }

        public List<TblMaterialTypeReport> ConvertTblMaterialTypeReportDTToList(SqlDataReader tblTRLoadingWeighingTODT)
        {
            List<TblMaterialTypeReport> tblMaterialReportTOList = new List<TblMaterialTypeReport>();
            if (tblTRLoadingWeighingTODT != null)
            {
                while (tblTRLoadingWeighingTODT.Read())
                {
                    TblMaterialTypeReport tblMaterialTypeReport = new TblMaterialTypeReport();
                    if (tblTRLoadingWeighingTODT["MaterialType"] != DBNull.Value)
                        tblMaterialTypeReport.MaterialType = Convert.ToString(tblTRLoadingWeighingTODT["MaterialType"].ToString());
                    if (tblTRLoadingWeighingTODT["FromLocation"] != DBNull.Value)
                        tblMaterialTypeReport.FromLocation = Convert.ToString(tblTRLoadingWeighingTODT["FromLocation"].ToString());
                    if (tblTRLoadingWeighingTODT["ToLocation"] != DBNull.Value)
                        tblMaterialTypeReport.ToLocation = Convert.ToString(tblTRLoadingWeighingTODT["ToLocation"].ToString());
                    if (tblTRLoadingWeighingTODT["CompletedVehicleCount"] != DBNull.Value)
                        tblMaterialTypeReport.CompletedVehicleCount = Convert.ToInt32(tblTRLoadingWeighingTODT["CompletedVehicleCount"].ToString());
                    if (tblTRLoadingWeighingTODT["CompletedQty"] != DBNull.Value)
                        tblMaterialTypeReport.CompletedQty = Convert.ToDecimal(tblTRLoadingWeighingTODT["CompletedQty"].ToString());
                    if (tblTRLoadingWeighingTODT["PendingVehicleCount"] != DBNull.Value)
                        tblMaterialTypeReport.PendingVehicleCount = Convert.ToInt32(tblTRLoadingWeighingTODT["PendingVehicleCount"].ToString());
                    if (tblTRLoadingWeighingTODT["PendingQty"] != DBNull.Value)
                        tblMaterialTypeReport.PendingQty = Convert.ToDecimal(tblTRLoadingWeighingTODT["PendingQty"].ToString());
                    tblMaterialReportTOList.Add(tblMaterialTypeReport);
                }
            }
            return tblMaterialReportTOList;
        }

        public List<TblTRLoadingWeighingTO> ConvertTblTRLoadingWeighingDTToList(SqlDataReader tblTRLoadingWeighingTODT)
        {
            List<TblTRLoadingWeighingTO> tblTRLoadingWeighingTOList = new List<TblTRLoadingWeighingTO>();
            if (tblTRLoadingWeighingTODT != null)
            {
                Int32 i = 0;
                while (tblTRLoadingWeighingTODT.Read())
                {
                    TblTRLoadingWeighingTO tblTRLoadingWeighingTO = new TblTRLoadingWeighingTO();
                    tblTRLoadingWeighingTO.SrNo = ++i;
                    if (tblTRLoadingWeighingTODT["idWeighing"] != DBNull.Value)
                        tblTRLoadingWeighingTO.IdWeighing = Convert.ToInt32(tblTRLoadingWeighingTODT["idWeighing"].ToString());
                    if (tblTRLoadingWeighingTODT["vehicleNo"] != DBNull.Value)
                        tblTRLoadingWeighingTO.VehicleNo = Convert.ToString(tblTRLoadingWeighingTODT["vehicleNo"].ToString());
                    if (tblTRLoadingWeighingTODT["loadingId"] != DBNull.Value)
                        tblTRLoadingWeighingTO.LoadingId = Convert.ToInt32(tblTRLoadingWeighingTODT["loadingId"].ToString());
                    if (tblTRLoadingWeighingTODT["loadingTypeId"] != DBNull.Value)
                        tblTRLoadingWeighingTO.LoadingTypeId = Convert.ToInt32(tblTRLoadingWeighingTODT["loadingTypeId"].ToString());
                    if (tblTRLoadingWeighingTODT["grossWeight"] != DBNull.Value)
                        tblTRLoadingWeighingTO.GrossWeight = Convert.ToDecimal(tblTRLoadingWeighingTODT["grossWeight"].ToString());
                    if (tblTRLoadingWeighingTODT["actualWeight"] != DBNull.Value)
                        tblTRLoadingWeighingTO.ActualWeight = Convert.ToDecimal(tblTRLoadingWeighingTODT["actualWeight"].ToString());
                    if (tblTRLoadingWeighingTODT["netWeight"] != DBNull.Value)
                        tblTRLoadingWeighingTO.NetWeight = Convert.ToDecimal(tblTRLoadingWeighingTODT["netWeight"].ToString());
                    if (tblTRLoadingWeighingTODT["rstNumber"] != DBNull.Value)
                        tblTRLoadingWeighingTO.RstNumber = Convert.ToString(tblTRLoadingWeighingTODT["rstNumber"].ToString());
                    if (tblTRLoadingWeighingTODT["weighingStageId"] != DBNull.Value)
                        tblTRLoadingWeighingTO.WeighingStageId = Convert.ToInt32(tblTRLoadingWeighingTODT["weighingStageId"].ToString());
                    if (tblTRLoadingWeighingTODT["weighingMachineId"] != DBNull.Value)
                        tblTRLoadingWeighingTO.WeighingMachineId = Convert.ToInt32(tblTRLoadingWeighingTODT["weighingMachineId"].ToString());
                    if (tblTRLoadingWeighingTODT["weighingMeasureTypeId"] != DBNull.Value)
                        tblTRLoadingWeighingTO.WeighingMeasureTypeId = Convert.ToInt32(tblTRLoadingWeighingTODT["weighingMeasureTypeId"].ToString());
                    if (tblTRLoadingWeighingTODT["createdBy"] != DBNull.Value)
                        tblTRLoadingWeighingTO.CreatedBy = Convert.ToInt32(tblTRLoadingWeighingTODT["createdBy"].ToString());
                    if (tblTRLoadingWeighingTODT["createdOn"] != DBNull.Value)
                        tblTRLoadingWeighingTO.CreatedOn = Convert.ToDateTime(tblTRLoadingWeighingTODT["createdOn"].ToString());
                    if (tblTRLoadingWeighingTODT["updatedBy"] != DBNull.Value)
                        tblTRLoadingWeighingTO.UpdatedBy = Convert.ToInt32(tblTRLoadingWeighingTODT["updatedBy"].ToString());
                    if (tblTRLoadingWeighingTODT["updatedOn"] != DBNull.Value)
                        tblTRLoadingWeighingTO.UpdatedOn = Convert.ToDateTime(tblTRLoadingWeighingTODT["updatedOn"].ToString());
                    if (tblTRLoadingWeighingTODT["isActive"] != DBNull.Value)
                        tblTRLoadingWeighingTO.IsActive = Convert.ToInt32(tblTRLoadingWeighingTODT["isActive"].ToString());
                    if (tblTRLoadingWeighingTODT["machineName"] != DBNull.Value)
                        tblTRLoadingWeighingTO.MachineName = Convert.ToString(tblTRLoadingWeighingTODT["machineName"].ToString());
                    tblTRLoadingWeighingTOList.Add(tblTRLoadingWeighingTO);
                }
            }
            return tblTRLoadingWeighingTOList;
        }
        public List<TblUnloadingSLATO> ConvertTblUnloadingSLADTToList(SqlDataReader tblUnloadingSLATODT)
        {
            List<TblUnloadingSLATO> tblUnloadingSLATOList = new List<TblUnloadingSLATO>();
            if (tblUnloadingSLATODT != null)
            {
                Int32 i = 0;
                while (tblUnloadingSLATODT.Read())
                {
                    TblUnloadingSLATO tblUnloadingSLATO = new TblUnloadingSLATO();
                    tblUnloadingSLATO.SrNo = ++i;
                    if (tblUnloadingSLATODT["idSLA"] != DBNull.Value)
                        tblUnloadingSLATO.IdSLA = Convert.ToInt32(tblUnloadingSLATODT["idSLA"].ToString());
                    if (tblUnloadingSLATODT["displayNo"] != DBNull.Value)
                        tblUnloadingSLATO.DisplayNo = Convert.ToString(tblUnloadingSLATODT["displayNo"].ToString());
                    if (tblUnloadingSLATODT["loadingId"] != DBNull.Value)
                        tblUnloadingSLATO.LoadingId = Convert.ToInt32(tblUnloadingSLATODT["loadingId"].ToString());
                    if (tblUnloadingSLATODT["vehicleId"] != DBNull.Value)
                        tblUnloadingSLATO.VehicleId = Convert.ToInt32(tblUnloadingSLATODT["vehicleId"].ToString());
                    if (tblUnloadingSLATODT["materialTypeId"] != DBNull.Value)
                        tblUnloadingSLATO.MaterialTypeId = Convert.ToInt32(tblUnloadingSLATODT["materialTypeId"].ToString());
                    if (tblUnloadingSLATODT["slaUnloadingId"] != DBNull.Value)
                        tblUnloadingSLATO.SlaUnloadingId = Convert.ToInt32(tblUnloadingSLATODT["slaUnloadingId"].ToString());
                    if (tblUnloadingSLATODT["mixMaterialId"] != DBNull.Value)
                        tblUnloadingSLATO.MixMaterialId = Convert.ToInt32(tblUnloadingSLATODT["mixMaterialId"].ToString());
                    if (tblUnloadingSLATODT["overSizePer"] != DBNull.Value)
                        tblUnloadingSLATO.OverSizePer = Convert.ToDecimal(tblUnloadingSLATODT["overSizePer"].ToString());
                    if (tblUnloadingSLATODT["waste"] != DBNull.Value)
                        tblUnloadingSLATO.Waste = Convert.ToString(tblUnloadingSLATODT["waste"].ToString());
                    if (tblUnloadingSLATODT["offChemistryId"] != DBNull.Value)
                        tblUnloadingSLATO.OffChemistryId = Convert.ToInt32(tblUnloadingSLATODT["offChemistryId"].ToString());
                    if (tblUnloadingSLATODT["density"] != DBNull.Value)
                        tblUnloadingSLATO.Density = Convert.ToString(tblUnloadingSLATODT["density"].ToString());
                    if (tblUnloadingSLATODT["createdBy"] != DBNull.Value)
                        tblUnloadingSLATO.CreatedBy = Convert.ToInt32(tblUnloadingSLATODT["createdBy"].ToString());
                    if (tblUnloadingSLATODT["createdOn"] != DBNull.Value)
                        tblUnloadingSLATO.CreatedOn = Convert.ToDateTime(tblUnloadingSLATODT["createdOn"].ToString());
                    if (tblUnloadingSLATODT["isActive"] != DBNull.Value)
                        tblUnloadingSLATO.IsActive = Convert.ToInt32(tblUnloadingSLATODT["isActive"].ToString());
                    if (tblUnloadingSLATODT["MaterialType"] != DBNull.Value)
                        tblUnloadingSLATO.MaterialType = Convert.ToString(tblUnloadingSLATODT["MaterialType"].ToString());
                    if (tblUnloadingSLATODT["vehicleNo"] != DBNull.Value)
                        tblUnloadingSLATO.VehicleNo = Convert.ToString(tblUnloadingSLATODT["vehicleNo"].ToString());
                    if (tblUnloadingSLATODT["slaUnloadingDesc"] != DBNull.Value)
                        tblUnloadingSLATO.SlaUnloadingDesc = Convert.ToString(tblUnloadingSLATODT["slaUnloadingDesc"].ToString());
                    if (tblUnloadingSLATODT["mixMaterialDesc"] != DBNull.Value)
                        tblUnloadingSLATO.MixMaterialDesc = Convert.ToString(tblUnloadingSLATODT["mixMaterialDesc"].ToString());
                    if (tblUnloadingSLATODT["offChemistryDesc"] != DBNull.Value)
                        tblUnloadingSLATO.OffChemistryDesc = Convert.ToString(tblUnloadingSLATODT["offChemistryDesc"].ToString());
                    if (tblUnloadingSLATODT["CreatedByName"] != DBNull.Value)
                        tblUnloadingSLATO.CreatedByName = Convert.ToString(tblUnloadingSLATODT["CreatedByName"].ToString());
                    if (tblUnloadingSLATODT["loadingSlipNo"] != DBNull.Value)
                        tblUnloadingSLATO.LoadingSlipNo = Convert.ToString(tblUnloadingSLATODT["loadingSlipNo"].ToString());
                    if (tblUnloadingSLATODT["requestDisplayNo"] != DBNull.Value)
                        tblUnloadingSLATO.RequestDisplayNo = Convert.ToString(tblUnloadingSLATODT["requestDisplayNo"].ToString());
                    tblUnloadingSLATOList.Add(tblUnloadingSLATO);
                }
            }
            return tblUnloadingSLATOList;
        }
        protected bool CheckDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Delete
        public int DeleteTRLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = @"DELETE FROM tblTRLoading WHERE idLoading = " + idLoading + "";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTRLoading");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int DeleteTRLoading(String idLoadingStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = @"DELETE FROM tblTRLoading WHERE idLoading IN(" + idLoadingStr + ")";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTRLoading");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int DeleteTRLoadingWeighing(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = @"DELETE FROM tblTRLoadingWeighing WHERE loadingId IN(" + loadingIdStr + ")";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTRLoadingWeighing");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int DeleteTRLoadingHistory(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = @"DELETE FROM tblTRLoadingHistory WHERE loadingId IN(" + loadingIdStr + ")";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTRLoadingHistory");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int DeleteTransferRequest(String idTransferRequestStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = "DELETE FROM tblTransferRequest WHERE idTransferRequest IN( " +
                    " SELECT tblTransferRequest.idTransferRequest from tblTransferRequest tblTransferRequest " +
                    " LEFT JOIN tblTRLoading tblTRLoading on tblTRLoading.transferRequestId = tblTransferRequest.idTransferRequest " +
                    " LEFT JOIN tblUnloadingSLA tblUnloadingSLA on tblUnloadingSLA.loadingId = tblTRLoading.idLoading " +
                    " WHERE tblTransferRequest.statusId = "+(Int32)Constants.InternalTransferRequestStatusE.CLOSE+" and tblTransferRequest.idTransferRequest IN("+ idTransferRequestStr + ") AND tblTRLoading.statusId NOT IN("+(Int32)Constants.LoadingStatusE.REJECTED+") " +
                    " GROUP BY tblTransferRequest.idTransferRequest " +
                    " HAVING COUNT(tblUnloadingSLA.idSLA) = COUNT(tblTransferRequest.idTransferRequest))";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTransferRequest");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int DeleteUnloadingSLA(String loadingIdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                String sqlQuery = @"DELETE FROM tblUnloadingSLA WHERE loadingId IN(" + loadingIdStr + ")";
                cmdDelete.CommandText = sqlQuery;
                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteUnloadingSLA");
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
