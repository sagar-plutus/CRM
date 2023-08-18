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
    public class TblPurchaseGradingDtlsDAO : ITblPurchaseGradingDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseGradingDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseGradingDtls.*,tblProductItem.isNonCommercialItem, tblPurchaseWeighingStageSummary.recoveryPer,tblVariables.variableDisplayname  FROM [tblPurchaseGradingDtls] tblPurchaseGradingDtls" +
                                    " LEFT JOIN tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary on tblPurchaseWeighingStageSummary.idPurchaseWeighingStage=tblPurchaseGradingDtls.purchaseWeighingStageId " +
                                    " LEFT JOIN tblProductItem tblProductItem on tblProductItem.idProdItem=tblPurchaseGradingDtls.prodItemId " +
                                    " LEFT JOIN tblVariables tblVariables ON tblVariables.idVariable =  tblPurchaseGradingDtls.processVarId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls()
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

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtls(Int32 idGradingDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGradingDtls = " + idGradingDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseWeighingStageId = " + weighingStageId + " ORDER BY tblProductItem.displaySequanceNo";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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
        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseGradingDtls.purchaseScheduleSummaryId IN  ( " + purchaseScheduleIds + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByScheduleId(string purchaseScheduleIds, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId IN  ( " + purchaseScheduleIds + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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
        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByWeighingId(Int32 weighingStageId, SqlConnection conn, SqlTransaction tran)
        {
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseWeighingStageId = " + weighingStageId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseGradingDtlsTO> SelectTblPurchaseGradingDtlsTOListByGradingDtlsId(Int32 gradingDtlsId, SqlConnection conn, SqlTransaction tran)
        {
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGradingDtls = " + gradingDtlsId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseGradingDtlsTO> SelectAllTblPurchaseGradingDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseGradingDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblPurchaseGradingDtlsTO> ConvertDTToList(SqlDataReader tblPurchaseGradingDtlsTODT)
        {
            List<TblPurchaseGradingDtlsTO> tblPurchaseGradingDtlsTOList = new List<TblPurchaseGradingDtlsTO>();
            if (tblPurchaseGradingDtlsTODT != null)
            {
                while (tblPurchaseGradingDtlsTODT.Read())
                {
                    TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTONew = new TblPurchaseGradingDtlsTO();
                    if (tblPurchaseGradingDtlsTODT["idGradingDtls"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.IdGradingDtls = Convert.ToInt32(tblPurchaseGradingDtlsTODT["idGradingDtls"].ToString());
                    if (tblPurchaseGradingDtlsTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseGradingDtlsTODT["purchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseGradingDtlsTODT["purchaseWeighingStageId"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.PurchaseWeighingStageId = Convert.ToInt32(tblPurchaseGradingDtlsTODT["purchaseWeighingStageId"].ToString());
                    if (tblPurchaseGradingDtlsTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.ProdItemId = Convert.ToInt32(tblPurchaseGradingDtlsTODT["prodItemId"].ToString());
                    if (tblPurchaseGradingDtlsTODT["createdBy"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.CreatedBy = Convert.ToInt32(tblPurchaseGradingDtlsTODT["createdBy"].ToString());
                    if (tblPurchaseGradingDtlsTODT["createdOn"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseGradingDtlsTODT["createdOn"].ToString());
                    if (tblPurchaseGradingDtlsTODT["qtyMT"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.QtyMT = Convert.ToDouble(tblPurchaseGradingDtlsTODT["qtyMT"].ToString());
                    if (tblPurchaseGradingDtlsTODT["rate"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.Rate = Convert.ToDouble(tblPurchaseGradingDtlsTODT["rate"].ToString());
                    if (tblPurchaseGradingDtlsTODT["productAmount"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.ProductAmount = Convert.ToDouble(tblPurchaseGradingDtlsTODT["productAmount"].ToString());
                    if (tblPurchaseGradingDtlsTODT["isConfirmGrading"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.IsConfirmGrading = Convert.ToInt32(tblPurchaseGradingDtlsTODT["isConfirmGrading"].ToString());

                    if (tblPurchaseGradingDtlsTODT["recoveryPer"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.RecoveryPer = Convert.ToDouble(tblPurchaseGradingDtlsTODT["recoveryPer"].ToString());

                    if (tblPurchaseGradingDtlsTODT["isNonCommercialItem"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.IsNonCommercialItem = Convert.ToInt32(tblPurchaseGradingDtlsTODT["isNonCommercialItem"].ToString());

                    if (tblPurchaseGradingDtlsTODT["itemBookingRate"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.ItemBookingRate = Convert.ToDouble(tblPurchaseGradingDtlsTODT["itemBookingRate"].ToString());

                    if (tblPurchaseGradingDtlsTODT["isGradeSelected"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.IsGradeSelected = Convert.ToInt32(tblPurchaseGradingDtlsTODT["isGradeSelected"].ToString());

                    if(tblPurchaseGradingDtlsTODT["processVarId"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.ProcessVarId = Convert.ToInt32(tblPurchaseGradingDtlsTODT["processVarId"].ToString());

                    if (tblPurchaseGradingDtlsTODT["variableDisplayname"] != DBNull.Value)
                        tblPurchaseGradingDtlsTONew.ProcessVarDesc = Convert.ToString(tblPurchaseGradingDtlsTODT["variableDisplayname"].ToString());

                    tblPurchaseGradingDtlsTOList.Add(tblPurchaseGradingDtlsTONew);
                }
            }
            return tblPurchaseGradingDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseGradingDtlsTO, cmdInsert);
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

        public int InsertTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseGradingDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseGradingDtls]( " +
            //"  [idGradingDtls]" +
            "  [purchaseScheduleSummaryId]" +
            " ,[purchaseWeighingStageId]" +
            " ,[prodItemId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[qtyMT]" +
            " ,[rate]" +
            " ,[productAmount]" +
            " ,[isConfirmGrading]" +
            " ,[itemBookingRate]" +
            " ,[isGradeSelected]" +
            " ,[processVarId]" +
            " )" +
" VALUES (" +
            //"  @IdGradingDtls " +
            "  @PurchaseScheduleSummaryId " +
            " ,@PurchaseWeighingStageId " +
            " ,@ProdItemId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@QtyMT " +
            " ,@Rate " +
            " ,@ProductAmount " +
            " ,@IsConfirmGrading " +
            " ,@ItemBookingRate " +
            " ,@IsGradeSelected " +
            " ,@ProcessVarId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@PurchaseWeighingStageId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.PurchaseWeighingStageId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.ProdItemId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.CreatedBy;
            // cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseGradingDtlsTO.CreatedOn.ToString("MM/dd/yyyy h:mm tt");
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseGradingDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@QtyMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.QtyMT;
            cmdInsert.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.Rate;
            cmdInsert.Parameters.Add("@ProductAmount", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.ProductAmount;
            cmdInsert.Parameters.Add("@IsConfirmGrading", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IsConfirmGrading;
            cmdInsert.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseGradingDtlsTO.ItemBookingRate);
            cmdInsert.Parameters.Add("@IsGradeSelected", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IsGradeSelected;
            cmdInsert.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseGradingDtlsTO.ProcessVarId);


            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseGradingDtlsTO, cmdUpdate);
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

        public int UpdateTblPurchaseGradingDtls(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseGradingDtlsTO, cmdUpdate);
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

        public int DeleteAllGradingDtls(Int32 scheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                SqlCommand cmdUpdate = new SqlCommand();

                String sqlQuery = @" DELETE FROM [tblPurchaseGradingDtls]  " +
                " WHERE purchaseScheduleSummaryId = @PurchaseScheduleSummaryId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = scheduleSummaryId;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return 0;
            }

        }


        public int ExecuteUpdationCommand(TblPurchaseGradingDtlsTO tblPurchaseGradingDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseGradingDtls] SET " +
            //"  [idGradingDtls] = @IdGradingDtls" +
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[purchaseWeighingStageId]= @PurchaseWeighingStageId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[qtyMT] = @QtyMT" +
            " ,[rate] = @Rate" +
            " ,[productAmount] = @ProductAmount" +
            " ,[isConfirmGrading] = @IsConfirmGrading" +
            " ,[itemBookingRate] = @ItemBookingRate" +
            " ,[isGradeSelected] = @IsGradeSelected" +
            " ,[processVarId] = @ProcessVarId" +
            " WHERE idGradingDtls = @IdGradingDtls ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@PurchaseWeighingStageId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.PurchaseWeighingStageId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.ProdItemId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseGradingDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@QtyMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.QtyMT;
            cmdUpdate.Parameters.Add("@Rate", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.Rate;
            cmdUpdate.Parameters.Add("@ProductAmount", System.Data.SqlDbType.NVarChar).Value = tblPurchaseGradingDtlsTO.ProductAmount;
            cmdUpdate.Parameters.Add("@IsConfirmGrading", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IsConfirmGrading;
            cmdUpdate.Parameters.Add("@ItemBookingRate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseGradingDtlsTO.ItemBookingRate);
            cmdUpdate.Parameters.Add("@IsGradeSelected", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IsGradeSelected;
            cmdUpdate.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseGradingDtlsTO.ProcessVarId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGradingDtls, cmdDelete);
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

        public int DeleteTblPurchaseGradingDtls(Int32 idGradingDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGradingDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idGradingDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseGradingDtls] " +
            " WHERE idGradingDtls = " + idGradingDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGradingDtls", System.Data.SqlDbType.Int).Value = tblPurchaseGradingDtlsTO.IdGradingDtls;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllGradingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseGradingDtls WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
