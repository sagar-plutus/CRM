using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblTransferRequestDAO : ITblTransferRequestDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _icommondao;
        public TblTransferRequestDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _icommondao = icommondao;
        }
        #region Methods
        String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblTransferRequest.* ,materialType .value MaterialType,materialsSubType .value MaterialSubType, "+
                                 " fromLocation.value fromLocation, toLocation .value toLocation, unloadingPoint .value unloadingPoint,"+
                                 "  dimStatus.statusName as 'Status',createdBy.userDisplayName 'CreatedByName',updatedBy.userDisplayName UpdatedByName " +
                                 " from tblTransferRequest tblTransferRequest "+
                                 " left  join dimGenericMaster materialType on tblTransferRequest.materialTypeId = materialType.idGenericMaster "+
                                 " left  join dimGenericMaster materialsSubType on tblTransferRequest.materialSubTypeId = materialsSubType.idGenericMaster "+
                                 " left  join dimGenericMaster fromLocation on tblTransferRequest.fromLocationId = fromLocation.idGenericMaster "+
                                 " left join dimGenericMaster toLocation on tblTransferRequest.toLocationId = toLocation.idGenericMaster "+
                                 " left  join dimGenericMaster unloadingPoint on tblTransferRequest.unloadingPointId = unloadingPoint.idGenericMaster "+
                                 " left join dimStatus dimStatus on tblTransferRequest.statusId = dimStatus.idStatus"+
                                 " left join tblUser createdBy  on tblTransferRequest.createdBy   =createdBy .idUser   "+
                                 " left join tblUser updatedBy  on tblTransferRequest.updatedBy    =updatedBy .idUser  ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblTransferRequestTO>  GetTransferRequestDtlList(InternalTransferFilterTO voucherFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where   tblTransferRequest.isActive =1";
                if (voucherFilterTO != null)
                {
                    if (voucherFilterTO.CreatedBy > 0)
                    {
                        cmdSelect.CommandText += " AND tblTransferRequest.createdBy = " + voucherFilterTO.CreatedBy;
                    } 
                    if (!String.IsNullOrEmpty(voucherFilterTO.StatusIdStr))
                    {
                        cmdSelect.CommandText += " AND tblTransferRequest.statusId IN(" + voucherFilterTO.StatusIdStr + ")";
                    }
                    if (voucherFilterTO.FromDate != DateTime.MinValue  && voucherFilterTO.ToDate !=DateTime .MinValue && voucherFilterTO.SkipDateFilter == false)
                    {
                        if (CheckDate(Convert.ToString(voucherFilterTO.FromDate)) == true && CheckDate(Convert.ToString(voucherFilterTO.ToDate)) == true)
                        {
                            cmdSelect.CommandText += " AND CAST(tblTransferRequest.createdOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                            cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = voucherFilterTO.FromDate ; 
                            cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = voucherFilterTO.ToDate  ;
                        }
                    } 
                    
                }
                else
                {
                    cmdSelect.CommandText += " AND tblTransferRequest.isActive = 1 ";
                }
                cmdSelect.CommandText += " ORDER BY tblTransferRequest.idTransferRequest DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect); 
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return ConvertDTToList(dt);
                }
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

        public TblTransferRequestTO  SelectTblTransferRequest(Int32 idTransferRequest)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            DataTable dt = new DataTable();
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTransferRequest = " + idTransferRequest + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTransferRequest", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.IdTransferRequest;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblTransferRequestTO> TblTransferRequestTOList = new List<TblTransferRequestTO>();
                    TblTransferRequestTOList = ConvertDTToList(dt);
                    if (TblTransferRequestTOList != null && TblTransferRequestTOList.Count > 0)
                        return TblTransferRequestTOList[0];
                    else
                        return null; 
                }
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
        public List<TblTransferRequestTO> SelectTblTransferRequest(String idTransferRequestStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            DataTable dt = new DataTable();
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTransferRequest IN(" + idTransferRequestStr + ") ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblTransferRequestTO> TblTransferRequestTOList = new List<TblTransferRequestTO>();
                    TblTransferRequestTOList = ConvertDTToList(dt);
                    if (TblTransferRequestTOList != null && TblTransferRequestTOList.Count > 0)
                        return TblTransferRequestTOList;
                    else
                        return null;
                }
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

        public TblTransferRequestTO  SelectTblTransferRequest(Int32 idTransferRequest, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTransferRequest = " + idTransferRequest + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect); 
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblTransferRequestTO> TblTransferRequestTOList = new List<TblTransferRequestTO>();
                    TblTransferRequestTOList = ConvertDTToList(dt);
                    if (TblTransferRequestTOList != null && TblTransferRequestTOList.Count > 0)
                        return TblTransferRequestTOList[0];
                    else
                        return null;
                }
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

        public DataTable SelectAllTblTransferRequest(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTransferRequest", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.IdTransferRequest;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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
        List<TblTransferRequestTO> ConvertDTToList(DataTable tblTransferRequestTODT)
        {
            List<TblTransferRequestTO> tblTransferRequestTOList = new List<TblTransferRequestTO>();
            if (tblTransferRequestTODT != null)
            {
                for (int rowCount = 0; rowCount < tblTransferRequestTODT.Rows.Count; rowCount++)
                {
                    TblTransferRequestTO tblTransferRequestTONew = new TblTransferRequestTO();
                    if (tblTransferRequestTODT.Rows[rowCount]["idTransferRequest"] != DBNull.Value)
                        tblTransferRequestTONew.IdTransferRequest = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["idTransferRequest"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialTypeId"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialTypeId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["materialTypeId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialSubTypeId"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialSubTypeId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["materialSubTypeId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["fromLocationId"] != DBNull.Value)
                        tblTransferRequestTONew.FromLocationId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["fromLocationId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["toLocationId"] != DBNull.Value)
                        tblTransferRequestTONew.ToLocationId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["toLocationId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["unloadingPointId"] != DBNull.Value)
                        tblTransferRequestTONew.UnloadingPointId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["unloadingPointId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["statusId"] != DBNull.Value)
                        tblTransferRequestTONew.StatusId = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["statusId"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedBy = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedBy = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedOn = Convert.ToDateTime(tblTransferRequestTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedOn = Convert.ToDateTime(tblTransferRequestTODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["isAutoCreated"] != DBNull.Value)
                        tblTransferRequestTONew.IsAutoCreated = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["isAutoCreated"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblTransferRequestTONew.IsActive = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["isActive"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["qty"] != DBNull.Value)
                        tblTransferRequestTONew.Qty = Convert.ToDouble(tblTransferRequestTODT.Rows[rowCount]["qty"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["scheduleqty"] != DBNull.Value)
                        tblTransferRequestTONew.Scheduleqty = Convert.ToDouble(tblTransferRequestTODT.Rows[rowCount]["scheduleqty"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["requestDisplayNo"] != DBNull.Value)
                        tblTransferRequestTONew.RequestDisplayNo = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["requestDisplayNo"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["narration"] != DBNull.Value)
                        tblTransferRequestTONew.Narration = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["narration"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["statusChangedBy"] != DBNull.Value)
                        tblTransferRequestTONew.StatusChangedBy = Convert.ToInt32(tblTransferRequestTODT.Rows[rowCount]["statusChangedBy"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["statusChangedOn"] != DBNull.Value)
                        tblTransferRequestTONew.StatusChangedOn = Convert.ToDateTime(tblTransferRequestTODT.Rows[rowCount]["statusChangedOn"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["UpdatedByName"] != DBNull.Value)
                        tblTransferRequestTONew.UpdatedByName = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["UpdatedByName"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["CreatedByName"] != DBNull.Value)
                        tblTransferRequestTONew.CreatedByName = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["CreatedByName"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["Status"] != DBNull.Value)
                        tblTransferRequestTONew.Status = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["Status"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["unloadingPoint"] != DBNull.Value)
                        tblTransferRequestTONew.UnloadingPoint = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["unloadingPoint"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["toLocation"] != DBNull.Value)
                        tblTransferRequestTONew.ToLocation = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["toLocation"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["fromLocation"] != DBNull.Value)
                        tblTransferRequestTONew.FromLocation = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["fromLocation"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["MaterialSubType"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialsSubType = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["MaterialSubType"].ToString());
                    if (tblTransferRequestTODT.Rows[rowCount]["materialType"] != DBNull.Value)
                        tblTransferRequestTONew.MaterialType = Convert.ToString(tblTransferRequestTODT.Rows[rowCount]["materialType"].ToString());
                    tblTransferRequestTOList.Add(tblTransferRequestTONew);
                }
            }
            return tblTransferRequestTOList;
        }
        #endregion

        #region Insertion
        public int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTransferRequestTO, cmdInsert);
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

        public int InsertTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTransferRequestTO, cmdInsert);
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

        int ExecuteInsertionCommand(TblTransferRequestTO tblTransferRequestTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTransferRequest]( " + 
            " [materialTypeId]" +
            " ,[materialSubTypeId]" +
            " ,[fromLocationId]" +
            " ,[toLocationId]" +
            " ,[unloadingPointId]" +
            " ,[statusId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" + 
            " ,[updatedOn]" +
            " ,[isAutoCreated]" +
            " ,[isActive]" +
            " ,[qty]" +
            " ,[scheduleqty]" +
            " ,[requestDisplayNo]" +
            " ,[narration]" +
            " , statusChangedBy "+
            " ,statusChangedOn" +
            " ,createdOn" +

            " )" +
" VALUES (" + 
            " @MaterialTypeId " +
            " ,@MaterialSubTypeId " +
            " ,@FromLocationId " +
            " ,@ToLocationId " +
            " ,@UnloadingPointId " +
            " ,@StatusId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " + 
            " ,@UpdatedOn " +
            " ,@IsAutoCreated " +
            " ,@IsActive " +
            " ,@Qty " +
            " ,@Scheduleqty " +
            " ,@RequestDisplayNo " +
            " ,@Narration " +
            " , @StatusChangedBy " +
            " ,@StatusChangedOn" +
            " ,@CreatedOn" +

            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.MaterialTypeId;
            cmdInsert.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.MaterialSubTypeId;
            cmdInsert.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.FromLocationId;
            cmdInsert.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.ToLocationId;
            cmdInsert.Parameters.Add("@UnloadingPointId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.UnloadingPointId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblTransferRequestTO.CreatedOn  ;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsAutoCreated", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.IsAutoCreated);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblTransferRequestTO.IsActive;
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblTransferRequestTO.Qty;
            cmdInsert.Parameters.Add("@Scheduleqty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.Scheduleqty);
            cmdInsert.Parameters.Add("@RequestDisplayNo", System.Data.SqlDbType.NVarChar).Value = tblTransferRequestTO.RequestDisplayNo;
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = tblTransferRequestTO.Narration;
            cmdInsert.Parameters.Add("@StatusChangedBy", System.Data.SqlDbType.Int ).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.StatusChangedBy) ;
            cmdInsert.Parameters.Add("@StatusChangedOn", System.Data.SqlDbType.DateTime ).Value = Constants.GetSqlDataValueNullForBaseValue(tblTransferRequestTO.StatusChangedOn) ;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblTransferRequestTO.IdTransferRequest  = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTransferRequestTO, cmdUpdate);
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

        public int UpdateTblTransferRequest(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTransferRequestTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblTransferRequestTO tblTransferRequestTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTransferRequest] SET " + 
            "  [materialTypeId]= @MaterialTypeId" +
            " ,[materialSubTypeId]= @MaterialSubTypeId" +
            " ,[fromLocationId]= @FromLocationId" +
            " ,[toLocationId]= @ToLocationId" +
            " ,[unloadingPointId]= @UnloadingPointId" +
            " ,[qty]= @Qty" +
            " ,[scheduleqty]= @Scheduleqty" +
            " ,[statusId]= @StatusId" +
            " ,[narration] = @Narration" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[statusChangedBy]= @StatusChangedBy" +
            " ,[statusChangedOn]= @StatusChangedOn" +
            " WHERE[idTransferRequest] = @IdTransferRequest ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdTransferRequest", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.IdTransferRequest;
            cmdUpdate.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.MaterialTypeId;
            cmdUpdate.Parameters.Add("@MaterialSubTypeId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.MaterialSubTypeId;
            cmdUpdate.Parameters.Add("@FromLocationId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.FromLocationId;
            cmdUpdate.Parameters.Add("@ToLocationId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.ToLocationId;
            cmdUpdate.Parameters.Add("@UnloadingPointId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.UnloadingPointId;
            cmdUpdate.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblTransferRequestTO.Qty;
            cmdUpdate.Parameters.Add("@Scheduleqty", System.Data.SqlDbType.Decimal).Value = tblTransferRequestTO.Scheduleqty;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.StatusId;
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = tblTransferRequestTO.Narration;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTransferRequestTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsAutoCreated", System.Data.SqlDbType.Bit).Value = tblTransferRequestTO.IsAutoCreated;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblTransferRequestTO.IsActive;
            cmdUpdate.Parameters.Add("@StatusChangedBy", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.StatusChangedBy;
            cmdUpdate.Parameters.Add("@StatusChangedOn", System.Data.SqlDbType.DateTime).Value = tblTransferRequestTO.StatusChangedOn;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateTblTransferRequestScheduleQtyNStatus(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblTransferRequest] SET " +
                           " [statusId]= @StatusId" +
                           " ,[scheduleqty] = @Scheduleqty" +
                           " ,statusChangedBy=@StatusChangedBy" +
                           " ,statusChangedOn=@StatusChangedOn" +
                     " WHERE[idTransferRequest] = @IdTransferRequest ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;


                cmdUpdate.Parameters.Add("@IdTransferRequest", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.IdTransferRequest;
                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusChangedBy", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.StatusChangedBy;
                cmdUpdate.Parameters.Add("@StatusChangedOn", System.Data.SqlDbType.DateTime).Value = tblTransferRequestTO.StatusChangedOn;
                cmdUpdate.Parameters.Add("@Scheduleqty", System.Data.SqlDbType.Decimal).Value = tblTransferRequestTO.Scheduleqty;
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

        public int UpdateTblTransferRequestScheduleQty(TblTransferRequestTO tblTransferRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblTransferRequest] SET " +
                           " [scheduleqty] += Scheduleqty" +

                     " WHERE[idTransferRequest] = @IdTransferRequest ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;


                cmdUpdate.Parameters.Add("@IdTransferRequest", System.Data.SqlDbType.Int).Value = tblTransferRequestTO.IdTransferRequest;
                cmdUpdate.Parameters.Add("@Scheduleqty", System.Data.SqlDbType.Decimal).Value = tblTransferRequestTO.Scheduleqty;
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


    }
}
