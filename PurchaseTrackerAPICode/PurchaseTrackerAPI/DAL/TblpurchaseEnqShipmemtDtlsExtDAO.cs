using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using System.Globalization;

namespace PurchaseTrackerAPI.DAL
{
    public class TblpurchaseEnqShipmemtDtlsExtDAO: ITblpurchaseEnqShipmemtDtlsExtDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblpurchaseEnqShipmemtDtlsExtDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblpurchaseEnqShipmemtDtlsExt]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt()
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

                //cmdSelect.Parameters.Add("@idShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsExtTO> list = ConvertDTToList(reader);
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


        public List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExtByShipmentDtlsId(Int32 shipmentDtlsId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + "WHERE isActive = 1 AND shipmentDtlsId = " + shipmentDtlsId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsExtTO> list = ConvertDTToList(reader);
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

        public TblpurchaseEnqShipmemtDtlsExtTO SelectTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idShipmentDtlsExt = " + idShipmentDtlsExt +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsExtTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
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

        public List<TblpurchaseEnqShipmemtDtlsExtTO> SelectAllTblpurchaseEnqShipmemtDtlsExt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsExtTO> list = ConvertDTToList(reader);
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

        public List<TblpurchaseEnqShipmemtDtlsExtTO> ConvertDTToList(SqlDataReader tblpurchaseEnqShipmemtDtlsExtTODT)
        {
            List<TblpurchaseEnqShipmemtDtlsExtTO> tblpurchaseEnqShipmemtDtlsExtTOList = new List<TblpurchaseEnqShipmemtDtlsExtTO>();
            if (tblpurchaseEnqShipmemtDtlsExtTODT != null)
            {
                while (tblpurchaseEnqShipmemtDtlsExtTODT.Read())
                {
                    TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTONew = new TblpurchaseEnqShipmemtDtlsExtTO();
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["idShipmentDtlsExt"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.IdShipmentDtlsExt = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsExtTODT["idShipmentDtlsExt"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["shipmentDtlsId"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.ShipmentDtlsId = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsExtTODT["shipmentDtlsId"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["createdBy"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.CreatedBy = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsExtTODT["createdBy"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["updatedBy"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.UpdatedBy = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsExtTODT["updatedBy"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["isActive"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.IsActive = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsExtTODT["isActive"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["createdOn"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.CreatedOn = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsExtTODT["createdOn"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["updatedOn"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.UpdatedOn = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsExtTODT["updatedOn"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["etaPortDate"] != DBNull.Value)
                    {
                        tblpurchaseEnqShipmemtDtlsExtTONew.EtaPortDate = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsExtTODT["etaPortDate"].ToString());
                        tblpurchaseEnqShipmemtDtlsExtTONew.EtaPortDateStr = tblpurchaseEnqShipmemtDtlsExtTONew.EtaPortDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        if (tblpurchaseEnqShipmemtDtlsExtTONew.EtaPortDateStr == "31-01-1999")
                        {
                            tblpurchaseEnqShipmemtDtlsExtTONew.EtaPortDateStr = "";
                        }

                    }
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["etaIcdDate"] != DBNull.Value)
                    {
                        tblpurchaseEnqShipmemtDtlsExtTONew.EtaIcdDate = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsExtTODT["etaIcdDate"].ToString());
                        tblpurchaseEnqShipmemtDtlsExtTONew.EtaIcdDateStr = tblpurchaseEnqShipmemtDtlsExtTONew.EtaIcdDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        if (tblpurchaseEnqShipmemtDtlsExtTONew.EtaIcdDateStr == "31-01-1999")
                        {
                            tblpurchaseEnqShipmemtDtlsExtTONew.EtaIcdDateStr = "";
                        }
                    }
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["validTill"] != DBNull.Value)
                    {
                        tblpurchaseEnqShipmemtDtlsExtTONew.ValidTill = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsExtTODT["validTill"].ToString());
                        tblpurchaseEnqShipmemtDtlsExtTONew.ValidTillStr = tblpurchaseEnqShipmemtDtlsExtTONew.ValidTill.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                        if (tblpurchaseEnqShipmemtDtlsExtTONew.ValidTillStr == "31-01-1999")
                        {
                            tblpurchaseEnqShipmemtDtlsExtTONew.ValidTillStr = "";
                        }
                    }
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["netWt"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.NetWt = Convert.ToDouble(tblpurchaseEnqShipmemtDtlsExtTODT["netWt"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["grossWt"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.GrossWt = Convert.ToDouble(tblpurchaseEnqShipmemtDtlsExtTODT["grossWt"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["containerNo"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.ContainerNo = Convert.ToString(tblpurchaseEnqShipmemtDtlsExtTODT["containerNo"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["sealNo"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.SealNo = Convert.ToString(tblpurchaseEnqShipmemtDtlsExtTODT["sealNo"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsExtTODT["doNumber"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsExtTONew.DoNumber = Convert.ToString(tblpurchaseEnqShipmemtDtlsExtTODT["doNumber"].ToString());

                    tblpurchaseEnqShipmemtDtlsExtTOList.Add(tblpurchaseEnqShipmemtDtlsExtTONew);
                }
            }
            return tblpurchaseEnqShipmemtDtlsExtTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblpurchaseEnqShipmemtDtlsExtTO, cmdInsert);
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

        public  int InsertTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblpurchaseEnqShipmemtDtlsExtTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblpurchaseEnqShipmemtDtlsExt]( " + 
            "  [shipmentDtlsId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[etaPortDate]" +
            " ,[etaIcdDate]" +
            " ,[netWt]" +
            " ,[grossWt]" +
            " ,[containerNo]" +
            " ,[sealNo]" +
            " ,[doNumber]" +
            " ,[validTill]" +
            " )" +
" VALUES (" +
            "  @ShipmentDtlsId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@EtaPortDate " +
            " ,@EtaIcdDate " +
            " ,@NetWt " +
            " ,@GrossWt " +
            " ,@ContainerNo " +
            " ,@SealNo " +
            " ,@DoNumber " +
            " ,@ValidTill " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;
            cmdInsert.Parameters.Add("@ShipmentDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ShipmentDtlsId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@EtaPortDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.EtaPortDate);
            cmdInsert.Parameters.Add("@EtaIcdDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.EtaIcdDate);
            cmdInsert.Parameters.Add("@NetWt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.NetWt);
            cmdInsert.Parameters.Add("@GrossWt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.GrossWt);
            cmdInsert.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ContainerNo);
            cmdInsert.Parameters.Add("@SealNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.SealNo);
            cmdInsert.Parameters.Add("@DoNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.DoNumber);
            cmdInsert.Parameters.Add("@ValidTill", System.Data.SqlDbType.Date).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ValidTill);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblpurchaseEnqShipmemtDtlsExtTO, cmdUpdate);
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

        public  int UpdateTblpurchaseEnqShipmemtDtlsExt(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblpurchaseEnqShipmemtDtlsExtTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblpurchaseEnqShipmemtDtlsExtTO tblpurchaseEnqShipmemtDtlsExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblpurchaseEnqShipmemtDtlsExt] SET " + 
            "  [shipmentDtlsId]= @ShipmentDtlsId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[etaPortDate]= @EtaPortDate" +
            " ,[etaIcdDate]= @EtaIcdDate" +
            " ,[netWt]= @NetWt" +
            " ,[grossWt]= @GrossWt" +
            " ,[containerNo]= @ContainerNo" +
            " ,[sealNo] = @SealNo" +
            " ,[doNumber] = @DoNumber" +
            " ,[validTill] = @ValidTill" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery + " AND idShipmentDtlsExt = @IdShipmentDtlsExt";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;
            cmdUpdate.Parameters.Add("@ShipmentDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ShipmentDtlsId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@EtaPortDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.EtaPortDate);
            cmdUpdate.Parameters.Add("@EtaIcdDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.EtaIcdDate);
            cmdUpdate.Parameters.Add("@NetWt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.NetWt);
            cmdUpdate.Parameters.Add("@GrossWt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.GrossWt);
            cmdUpdate.Parameters.Add("@ContainerNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ContainerNo);
            cmdUpdate.Parameters.Add("@SealNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.SealNo);
            cmdUpdate.Parameters.Add("@DoNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.DoNumber);
            cmdUpdate.Parameters.Add("@ValidTill", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsExtTO.ValidTill);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idShipmentDtlsExt, cmdDelete);
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

        public  int DeleteTblpurchaseEnqShipmemtDtlsExt(Int32 idShipmentDtlsExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idShipmentDtlsExt, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idShipmentDtlsExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblpurchaseEnqShipmemtDtlsExt] " +
            " WHERE idShipmentDtlsExt = " + idShipmentDtlsExt +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idShipmentDtlsExt", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsExtTO.IdShipmentDtlsExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
