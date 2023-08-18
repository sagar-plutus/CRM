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
    public class TblPurchaseVehFreightDtlsDAO : ITblPurchaseVehFreightDtlsDAO
    { 

        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehFreightDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseVehFreightDtls] tblPurchaseVehFreightDtls"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseVehFreightDtlsTO> SlectAllTblPurchaseVehFreightDtls()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehFreightDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblPurchaseVehFreightDtlsTO> SelectTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseVehFreightDtls = " + idPurchaseVehFreightDtls +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehFreightDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblPurchaseVehFreightDtlsTO> SelectAllTblPurchaseVehFreightDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehFreightDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " where tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = " + purchaseScheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehFreightDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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
        public List<TblPurchaseVehFreightDtlsTO> SelectFreightDtlsByPurchaseScheduleId(Int32 purchaseScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where tblPurchaseVehFreightDtls.purchaseScheduleSummaryId = " + purchaseScheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehFreightDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
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


        public List<TblPurchaseVehFreightDtlsTO> ConvertDTToList(SqlDataReader tblPurchaseVehFreightDtlsTODT)
        {
            List<TblPurchaseVehFreightDtlsTO> tblPurchaseVehFreightDtlsTOList = new List<TblPurchaseVehFreightDtlsTO>();
            if (tblPurchaseVehFreightDtlsTODT != null)
            {
                while(tblPurchaseVehFreightDtlsTODT.Read())
                {
                    TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTONew = new TblPurchaseVehFreightDtlsTO();
                    if (tblPurchaseVehFreightDtlsTODT ["idPurchaseVehFreightDtls"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.IdPurchaseVehFreightDtls = Convert.ToInt32(tblPurchaseVehFreightDtlsTODT ["idPurchaseVehFreightDtls"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseVehFreightDtlsTODT ["purchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["isValidInfo"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.IsValidInfo = Convert.ToInt32(tblPurchaseVehFreightDtlsTODT ["isValidInfo"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["isStorageExcess"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.IsStorageExcess = Convert.ToInt32(tblPurchaseVehFreightDtlsTODT ["isStorageExcess"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["createdBy"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.CreatedBy = Convert.ToInt32(tblPurchaseVehFreightDtlsTODT ["createdBy"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["createdOn"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseVehFreightDtlsTODT ["createdOn"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["freight"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.Freight = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT ["freight"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["advance"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.Advance = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT ["advance"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["unloadingQty"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.UnloadingQty = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT ["unloadingQty"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT ["shortage"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.Shortage = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT ["shortage"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT["amount"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.Amount = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT["amount"].ToString());
                    if (tblPurchaseVehFreightDtlsTODT["unloadingCharge"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.UnloadingCharge = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT["unloadingCharge"].ToString());
                    //chetan[03-March-2020] added
                    if (tblPurchaseVehFreightDtlsTODT["unloadingQtyAmt"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.UnloadingQtyAmt = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT["unloadingQtyAmt"]);
                    if (tblPurchaseVehFreightDtlsTODT["bookingRate"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.BookingRate = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT["bookingRate"]);
                    if (tblPurchaseVehFreightDtlsTODT["actulWt"] != DBNull.Value)
                        tblPurchaseVehFreightDtlsTONew.ActulWt = Convert.ToDouble(tblPurchaseVehFreightDtlsTODT["actulWt"]);


                    tblPurchaseVehFreightDtlsTOList.Add(tblPurchaseVehFreightDtlsTONew);
                }
            }
            return tblPurchaseVehFreightDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehFreightDtlsTO, cmdInsert);
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

        public int InsertTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehFreightDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseVehFreightDtls]( " + 
            //"  [idPurchaseVehFreightDtls]" +
            "  [purchaseScheduleSummaryId]" +
            " ,[isValidInfo]" +
            " ,[isStorageExcess]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[freight]" +
            " ,[advance]" +
            " ,[unloadingQty]" +
            " ,[shortage]" +
            " ,[amount]" +
            " ,[unloadingCharge]" +
            " ,[unloadingQtyAmt]" +
            " ,[bookingRate]" +
            " ,[actulWt]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseVehFreightDtls " +
            "  @PurchaseScheduleSummaryId " +
            " ,@IsValidInfo " +
            " ,@IsStorageExcess " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@Freight " +
            " ,@Advance " +
            " ,@UnloadingQty " +
            " ,@Shortage " +
            " ,@Amount " +
            " ,@UnloadingCharge " +
            " ,@unloadingQtyAmt " +
            " ,@bookingRate " +
            " ,@actulWt " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseVehFreightDtls", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IdPurchaseVehFreightDtls;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@IsValidInfo", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IsValidInfo;
            cmdInsert.Parameters.Add("@IsStorageExcess", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IsStorageExcess;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.CreatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.CreatedOn);
            cmdInsert.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Freight;
            cmdInsert.Parameters.Add("@Advance", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Advance;
            cmdInsert.Parameters.Add("@UnloadingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.UnloadingQty;
            cmdInsert.Parameters.Add("@Shortage", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Shortage;
            cmdInsert.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Amount;
            //chetan[03-March-2020] added
            cmdInsert.Parameters.Add("@UnloadingCharge", System.Data.SqlDbType.Decimal).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.UnloadingCharge);
            cmdInsert.Parameters.Add("@unloadingQtyAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.UnloadingQtyAmt);
            cmdInsert.Parameters.Add("@bookingRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.BookingRate);
            cmdInsert.Parameters.Add("@actulWt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.ActulWt);


            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehFreightDtlsTO, cmdUpdate);
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

        public int UpdateTblPurchaseVehFreightDtls(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehFreightDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseVehFreightDtlsTO tblPurchaseVehFreightDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehFreightDtls] SET " + 
            //"  [idPurchaseVehFreightDtls] = @IdPurchaseVehFreightDtls" +
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[isValidInfo]= @IsValidInfo" +
            " ,[isStorageExcess]= @IsStorageExcess" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[freight]= @Freight" +
            " ,[advance]= @Advance" +
            " ,[unloadingQty]= @UnloadingQty" +
            " ,[shortage] = @Shortage" +
            " ,[amount] = @Amount" +
            " ,[unloadingCharge] = @UnloadingCharge" +
            " ,[unloadingQtyAmt] = @unloadingQtyAmt " +
            " ,[bookingRate] = @bookingRate " +
            " ,[actulWt] = @actulWt " +
            " WHERE idPurchaseVehFreightDtls = @IdPurchaseVehFreightDtls "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseVehFreightDtls", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IdPurchaseVehFreightDtls;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@IsValidInfo", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IsValidInfo;
            cmdUpdate.Parameters.Add("@IsStorageExcess", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IsStorageExcess;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehFreightDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Freight;
            cmdUpdate.Parameters.Add("@Advance", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Advance;
            cmdUpdate.Parameters.Add("@UnloadingQty", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.UnloadingQty;
            cmdUpdate.Parameters.Add("@Shortage", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Shortage;
            cmdUpdate.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.Amount;
            cmdUpdate.Parameters.Add("@UnloadingCharge", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseVehFreightDtlsTO.UnloadingCharge);
            cmdUpdate.Parameters.Add("@unloadingQtyAmt", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.UnloadingQtyAmt;
            cmdUpdate.Parameters.Add("@bookingRate", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.BookingRate;
            cmdUpdate.Parameters.Add("@actulWt", System.Data.SqlDbType.Decimal).Value = tblPurchaseVehFreightDtlsTO.ActulWt;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseVehFreightDtls, cmdDelete);
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

        public int DeleteTblPurchaseVehFreightDtls(Int32 idPurchaseVehFreightDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseVehFreightDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseVehFreightDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseVehFreightDtls] " +
            " WHERE idPurchaseVehFreightDtls = " + idPurchaseVehFreightDtls +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseVehFreightDtls", System.Data.SqlDbType.Int).Value = tblPurchaseVehFreightDtlsTO.IdPurchaseVehFreightDtls;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeletePurchaseVehFreightDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseVehFreightDtls WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
