using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using static PurchaseTrackerAPI.StaticStuff.Constants;

namespace PurchaseTrackerAPI.DAL
{
    public class TblTRLoadingDAO : ITblTRLoadingDAO
    {
        private readonly IConnectionString _iConnectionString;

        public TblTRLoadingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
		
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblTRLoading.* ,materialType .value MaterialType, materialsSubType .value MaterialSubType, " +
                                 " fromLocation.value fromLocation, toLocation .value toLocation, unloadingPoint .value unloadingPoint," +
                                 "  dimStatus.statusName as 'Status',createdBy.userDisplayName 'CreatedByName',updatedBy.userDisplayName UpdatedByName " +
                                 "  FROM [tblTRLoading] " +
                                 " left  join dimGenericMaster materialType on tblTRLoading.materialTypeId = materialType.idGenericMaster " +
                                 " left  join dimGenericMaster materialsSubType on tblTRLoading.materialSubTypeId = materialsSubType.idGenericMaster " +
                                 " left  join dimGenericMaster fromLocation on tblTRLoading.fromLocationId = fromLocation.idGenericMaster " +
                                 " left join dimGenericMaster toLocation on tblTRLoading.toLocationId = toLocation.idGenericMaster " +
                                 " left  join dimGenericMaster unloadingPoint on tblTRLoading.unloadingPointId = unloadingPoint.idGenericMaster " +
                                 " left join dimStatus dimStatus on tblTRLoading.statusId = dimStatus.idStatus" +
                                 " left join tblUser createdBy  on tblTRLoading.createdBy   =createdBy .idUser   " +
                                 " left join tblUser updatedBy  on tblTRLoading.updatedBy    =updatedBy .idUser  ";
            return sqlSelectQry;
        }
		
        #endregion

        #region Selection
        public  DataTable SelectAllTblTRLoading()
        {
			
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblTRLoading.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public DataTable SelectAllTblTRLoading(int statusId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblTRLoading.isActive = 1 and tblTRLoading.statusId = " + statusId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
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

        public DataTable SelectAllTblTRLoading(String statusId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblTRLoading.isActive = 1 and tblTRLoading.statusId In( " + statusId + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
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

        public DataTable SelectAllTblTRLoading(String statusId, DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblTRLoading.isActive = 1 and tblTRLoading.statusId In( " + statusId + ") And tblTRLoading.createdOn > @FromDate And tblTRLoading.createdOn <= @ToDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = toDate;
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

        public DataTable SelectAllTblTRLoading( DateTime fromDate, DateTime toDate)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblTRLoading.isActive = 1  And tblTRLoading.createdOn > @FromDate And tblTRLoading.createdOn <= @ToDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = toDate;
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

        public DataTable SelectAllTblTRLoading(String statusId, DateTime fromDate, DateTime toDate, TRLoadingFilterE tRLoadingFilterE)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String query = SqlSelectQuery() + " Where tblTRLoading.isActive = 1 ";

                switch (tRLoadingFilterE)
                {
                    case TRLoadingFilterE.Status:

                        query += " And tblTRLoading.statusId In( " + statusId + ") ";
                        break;

                    case TRLoadingFilterE.Date:
                        query += " And tblTRLoading.createdOn > @FromDate And tblTRLoading.createdOn <= @ToDate ";
                        break;

                    case TRLoadingFilterE.StatusNDate:
                        query += " And statusId In( " + statusId + ") And createdOn > @FromDate And createdOn <= @ToDate ";
                        break;

                }


                cmdSelect.CommandText = query;// SqlSelectQuery() + " Where isActive = 1 and statusId In( " + statusId + ") And createdOn > @FromDate And createdOn <= @ToDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                cmdSelect.Parameters.Add("@FromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@ToDate", System.Data.SqlDbType.DateTime).Value = toDate;
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

        public DataTable SelectNextLoadingId()
        {
			
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT max(loadingSlipNo) as loadingSlipNo FROM [tblTRLoading] ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public  DataTable SelectTblTRLoading(Int32 idLoading)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE tblTRLoading.idLoading = " + idLoading + " And tblTRLoading.isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public DataTable SelectTblTRLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblTRLoading.idLoading = " + idLoading + " And tblTRLoading.isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
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

        public  DataTable SelectAllTblTRLoading(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() +" Where And isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        #endregion
        
        #region Insertion
        public  int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblTRLoadingTO, cmdInsert);
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

        public  int InsertTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblTRLoadingTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblTRLoading]( " + 
            //"  [idLoading]" +
            " [loadingSlipNo]" +
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
            //"  @IdLoading " +
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
            " ,@unloadingPointId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
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
            cmdInsert.Parameters.Add("@unloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            //return cmdInsert.ExecuteNonQuery();
			     tblTRLoadingTO.IdLoading =  (Int32)cmdInsert.ExecuteScalar();
            if (tblTRLoadingTO.IdLoading > 0)
                return 1;
            else
                return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblTRLoadingTO, cmdUpdate);
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

        public  int UpdateTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblTRLoadingTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblTRLoading] SET " + 
            " [loadingTypeId]= @LoadingTypeId" +
            " ,[transferRequestId]= @TransferRequestId" +
            " ,[fromLocationId]= @FromLocationId" +
            " ,[toLocationId]= @ToLocationId" +
            " ,[materialTypeId]= @MaterialTypeId" +
            " ,[materialSubTypeId]= @MaterialSubTypeId" +
            " ,[vehicleId]= @VehicleId" +
            " ,[statusId]= @StatusId" +            
            " ,[updatedBy]= @UpdatedBy" +
            " ,[statusBy]= @StatusBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[statusOn]= @StatusOn" +
            " ,[scheduleQty]= @ScheduleQty" +
            " ,[narration]= @Narration" +
            " ,[driverName] = @DriverName" +
            " ,[unloadingPointId] = @unloadingPointId" +
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
            cmdUpdate.Parameters.Add("@VehicleId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.VehicleId);
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.StatusOn;
            cmdUpdate.Parameters.Add("@ScheduleQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.ScheduleQty);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.Narration);
            cmdUpdate.Parameters.Add("@DriverName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.DriverName);
            cmdUpdate.Parameters.Add("@unloadingPointId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblTRLoadingTO.UnloadingPointId);
            return cmdUpdate.ExecuteNonQuery();
        }
		
		 public  int UpdateTblTRLoadingStatus(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                 String sqlQuery = @" UPDATE [tblTRLoading] SET " + 
                                    " [statusId]= @StatusId" +            
                                    " ,[statusBy]= @StatusBy" +
                                    " ,[statusOn]= @StatusOn" +
                                    " WHERE  [idLoading] = @IdLoading "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusId;
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.StatusBy;
            cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.StatusOn;
            
            return cmdUpdate.ExecuteNonQuery();

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
		
        #endregion
        
        #region Deletion
        public  int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(tblTRLoadingTO, cmdDelete);
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

        public  int DeleteTblTRLoading(TblTRLoadingTO tblTRLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(tblTRLoadingTO, cmdDelete);
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

        public  int ExecuteDeletionCommand(TblTRLoadingTO tblTRLoadingTO, SqlCommand cmdUpdate)
        {
          
			String sqlQuery = @" UPDATE [tblTRLoading] SET " + 
                                   " [updatedBy]= @UpdatedBy" +
                                    " ,[updatedOn]= @UpdatedOn" +
                                    " ,[isActive]= @IsActive" +
                                    " WHERE  [idLoading] = @IdLoading "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IdLoading;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblTRLoadingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblTRLoadingTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
			
           
        }
        #endregion
        
    }
}
