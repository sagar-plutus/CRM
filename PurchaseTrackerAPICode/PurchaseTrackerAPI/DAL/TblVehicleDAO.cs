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
    public class TblVehicleDAO : ITblVehicleDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _icommondao;
        public TblVehicleDAO(IConnectionString iConnectionString, Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _icommondao = icommondao;
        }
        #region Methods
        String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblVehicle.*,vehicalTypeName as 'Vehical Type',approvalUser.userDisplayName as 'Approval user', " + 
                                " dimVehicalStatus.statusName 'Vehical Status Name',createdName.userDisplayName as 'Created By Name' " +
                                " from tblVehicle tblVehicle  left join dimVehicalType  on tblVehicle.vehicleTypeId = dimVehicalType.idVehicalType  " +
                                " left join tblUser approvalUser on tblVehicle.approvalUserId = approvalUser.idUser " +
                                " left join dimStatus dimVehicalStatus on tblVehicle.vehicleStatusId = dimVehicalStatus.idStatus " + 
                                " left  join tblUser createdName on tblVehicle.createdBy = createdName.idUser "  ;
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllTblVehicle()
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

                //cmdSelect.Parameters.Add("@idVehicle", System.Data.SqlDbType.Int).Value = tblVehicleTO.IdVehicle;
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblVehicleTO SelectTblVehicle(Int32 idVehicle)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable(); 
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVehicle = " + idVehicle + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text; 
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblVehicleTO> tblVehicleTOList = ConvertDTToList(dt);
                    if (tblVehicleTOList != null && tblVehicleTOList.Count == 1)
                        return tblVehicleTOList[0];
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
        public TblVehicleTO SelectTblVehicle(Int32 idVehicle,SqlConnection conn,SqlTransaction tran)
        { 
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVehicle = " + idVehicle + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblVehicleTO> tblVehicleTOList = ConvertDTToList(dt);
                    if (tblVehicleTOList != null && tblVehicleTOList.Count == 1)
                        return tblVehicleTOList[0];
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
        List<TblVehicleTO> ConvertDTToList(DataTable tblVehicleTODT)
        {
            List<TblVehicleTO> tblVehicleTOList = new List<TblVehicleTO>();
            if (tblVehicleTODT != null)
            {
                for (int rowCount = 0; rowCount < tblVehicleTODT.Rows.Count; rowCount++)
                {
                    TblVehicleTO tblVehicleTONew = new TblVehicleTO();
                    if (tblVehicleTODT.Rows[rowCount]["idVehicle"] != DBNull.Value)
                        tblVehicleTONew.IdVehicle = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["idVehicle"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleTypeId"] != DBNull.Value)
                        tblVehicleTONew.VehicleTypeId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["vehicleTypeId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvalUserId"] != DBNull.Value)
                        tblVehicleTONew.ApprovalUserId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvalUserId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvalStatusId"] != DBNull.Value)
                        tblVehicleTONew.ApprovalStatusId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvalStatusId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvedBy"] != DBNull.Value)
                        tblVehicleTONew.ApprovedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["approvedBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleStatusId"] != DBNull.Value)
                        tblVehicleTONew.VehicleStatusId = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["vehicleStatusId"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        tblVehicleTONew.CreatedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["createdBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["updatedBy"] != DBNull.Value)
                        tblVehicleTONew.UpdatedBy = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["updatedBy"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["approvedOn"] != DBNull.Value)
                        tblVehicleTONew.ApprovedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["approvedOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["createdOn"] != DBNull.Value)
                        tblVehicleTONew.CreatedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["createdOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["updatedOn"] != DBNull.Value)
                        tblVehicleTONew.UpdatedOn = Convert.ToDateTime(tblVehicleTODT.Rows[rowCount]["updatedOn"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        tblVehicleTONew.IsActive = Convert.ToInt32(tblVehicleTODT.Rows[rowCount]["isActive"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["vehicleNo"] != DBNull.Value)
                        tblVehicleTONew.VehicleNo = Convert.ToString(tblVehicleTODT.Rows[rowCount]["vehicleNo"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["narration"] != DBNull.Value)
                        tblVehicleTONew.Narration = Convert.ToString(tblVehicleTODT.Rows[rowCount]["narration"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Vehical Type"] != DBNull.Value)
                        tblVehicleTONew.VehicalType = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Vehical Type"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Approval user"] != DBNull.Value)
                        tblVehicleTONew.ApprovalUser = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Approval user"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Vehical Status Name"] != DBNull.Value)
                        tblVehicleTONew.VehicalStatusName = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Vehical Status Name"].ToString());
                    if (tblVehicleTODT.Rows[rowCount]["Created By Name"] != DBNull.Value)
                        tblVehicleTONew.CreatedByName = Convert.ToString(tblVehicleTODT.Rows[rowCount]["Created By Name"].ToString());
                    tblVehicleTOList.Add(tblVehicleTONew);
                }
            }
            return tblVehicleTOList;
        }
        public DataTable SelectAllTblVehicle(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVehicle", System.Data.SqlDbType.Int).Value = tblVehicleTO.IdVehicle;
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
        public List<TblVehicleTO>  GetAllInternalTranferVehical(InternalTransferFilterTO voucherFilterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where   tblVehicle.isActive =1";
                if (voucherFilterTO != null)
                {

                    if (voucherFilterTO.CreatedBy > 0)
                    {
                        cmdSelect.CommandText += " AND tblVehicle.createdBy = " + voucherFilterTO.CreatedBy;
                    }
                    if (voucherFilterTO.ApprovalUserId > 0)
                    {
                        cmdSelect.CommandText += " AND tblVehicle.approvalUserId = " + voucherFilterTO.ApprovalUserId;
                    }
                    if (!String.IsNullOrEmpty(voucherFilterTO.StatusIdStr))
                    {
                        cmdSelect.CommandText += " AND tblVehicle.vehicleStatusId IN(" + voucherFilterTO.StatusIdStr + ")";
                    }
                    if (voucherFilterTO.FromDate != DateTime.MinValue && voucherFilterTO.ToDate != DateTime.MinValue)
                    {
                        if (CheckDate(Convert.ToString(voucherFilterTO.FromDate)) == true && CheckDate(Convert.ToString(voucherFilterTO.ToDate)) == true)
                        {
                            cmdSelect.CommandText += " AND CAST(tblVehicle.createdOn as date) BETWEEN CAST(@FromDate as date) AND CAST(@ToDate as date)";
                            cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = voucherFilterTO.FromDate;
                            cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = voucherFilterTO.ToDate;
                        }
                    }

                }
                else
                {
                    //cmdSelect.CommandText += " AND tblVehicle.isActive = 'True' ";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<TblVehicleTO> tblVehicleTOList = new List<TblVehicleTO>();
                    tblVehicleTOList= ConvertDTToList(dt);
                    return tblVehicleTOList;
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
        #endregion

        #region Insertion
        public int InsertTblVehicle(TblVehicleTO tblVehicleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVehicleTO, cmdInsert);
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

        public int InsertTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVehicleTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblVehicleTO tblVehicleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVehicle]( " +
            "  [vehicleTypeId]" +
            " ,[approvalUserId]" +
            " ,[approvalStatusId]" +
            " ,[vehicleStatusId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[isActive]" +
            " ,[vehicleNo]" +
            " ,[narration]" +
            " )" +
" VALUES (" + 
            " @VehicleTypeId " +
            " ,@ApprovalUserId " +
            " ,@ApprovalStatusId " +
            " ,@VehicleStatusId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@IsActive " +
            " ,@VehicleNo " +
            " ,@Narration " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleTypeId;
            cmdInsert.Parameters.Add("@ApprovalUserId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleTO.ApprovalUserId);
            cmdInsert.Parameters.Add("@ApprovalStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleTO.ApprovalStatusId);
            cmdInsert.Parameters.Add("@VehicleStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleStatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.CreatedOn ;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVehicleTO.IsActive;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblVehicleTO.VehicleNo;
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = tblVehicleTO.Narration;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblVehicleTO.IdVehicle  = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblVehicle(TblVehicleTO tblVehicleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVehicleTO, cmdUpdate);
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

        public int UpdateTblVehicle(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVehicleTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblVehicleTO tblVehicleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVehicle] SET " +
            "" +
            "  [vehicleTypeId]= @VehicleTypeId" +
            " ,[approvalUserId]= @ApprovalUserId" +
            " ,[approvalStatusId]= @ApprovalStatusId" +
            " ,[approvedBy]= @ApprovedBy" +
            " ,[vehicleStatusId]= @VehicleStatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[approvedOn]= @ApprovedOn" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[narration] = @Narration" +
            " WHERE  [idVehicle] = @IdVehicle";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehicle", System.Data.SqlDbType.Int).Value = tblVehicleTO.IdVehicle;
            cmdUpdate.Parameters.Add("@VehicleTypeId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleTypeId;
            cmdUpdate.Parameters.Add("@ApprovalUserId", System.Data.SqlDbType.Int).Value = tblVehicleTO.ApprovalUserId;
            cmdUpdate.Parameters.Add("@ApprovalStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.ApprovalStatusId;
            cmdUpdate.Parameters.Add("@ApprovedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.ApprovedBy;
            cmdUpdate.Parameters.Add("@VehicleStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleStatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@ApprovedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.ApprovedOn;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblVehicleTO.IsActive;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = tblVehicleTO.VehicleNo;
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = tblVehicleTO.Narration;
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateVehicalApprovalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlQuery = @" UPDATE [tblVehicle] SET " + 
           " [approvalStatusId]= @ApprovalStatusId" +
           " ,[approvedBy]= @ApprovedBy" +
           " ,[vehicleStatusId]= @VehicleStatusId" + 
           " ,[updatedBy]= @UpdatedBy" +
           " ,[approvedOn]= @ApprovedOn" + 
           " ,[updatedOn]= @UpdatedOn" +
           " ,[isActive]= @IsActive" +  
           " WHERE  [idVehicle] = @IdVehicle";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                 
                cmdUpdate.Parameters.Add("@ApprovalStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.ApprovalStatusId;
                cmdUpdate.Parameters.Add("@ApprovedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.ApprovedBy;
                cmdUpdate.Parameters.Add("@VehicleStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleStatusId; 
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@ApprovedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.ApprovedOn; 
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblVehicleTO.IsActive;
                cmdUpdate.Parameters.Add("@IdVehicle", System.Data.SqlDbType.Int).Value = tblVehicleTO.IdVehicle;

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


        /// <summary>
        /// AmolG[2022-Feb-04]
        /// </summary>
        /// <param name="tblVehicleTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateVehicalStatus(TblVehicleTO tblVehicleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlQuery = @" UPDATE [tblVehicle] SET " +
                                   " [vehicleStatusId]= @VehicleStatusId" +
                                   " ,[updatedBy]= @UpdatedBy" +
                                   " ,[updatedOn]= @UpdatedOn" +
                                   " WHERE  [idVehicle] = @IdVehicle";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.Parameters.Add("@VehicleStatusId", System.Data.SqlDbType.Int).Value = tblVehicleTO.VehicleStatusId;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVehicleTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVehicleTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@IdVehicle", System.Data.SqlDbType.Int).Value = tblVehicleTO.IdVehicle;

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

        public Boolean  checkAlreadyVehicalByName(TblVehicleTO tblVehicleTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblVehicle  where  isActive =1 AND vehicleStatusId NOT IN("+(Int32)Constants.InternalTransferRequestVehicalStatusE.REJECTED+","+ (Int32)Constants.InternalTransferRequestVehicalStatusE.CLOSE + ") And VehicleNo ='" + tblVehicleTO.VehicleNo  + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }

        public Boolean checkAlreadyVehicalById(TblVehicleTO tblVehicleTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblVehicle  where  isActive =1  And idVehicle =" + tblVehicleTO.IdVehicle  + "";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }
    }
}
