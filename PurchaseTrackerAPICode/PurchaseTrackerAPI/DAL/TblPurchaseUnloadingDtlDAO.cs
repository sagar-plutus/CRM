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
    public class TblPurchaseUnloadingDtlDAO : ITblPurchaseUnloadingDtlDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseUnloadingDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseUnloadingDtl.*,tblProductItem.itemName,tblProductItem.isNonCommercialItem,tblVariables.variableDisplayname FROM tblPurchaseUnloadingDtl tblPurchaseUnloadingDtl " +
                                  " LEFT JOIN tblProductItem tblProductItem ON tblProductItem.idProdItem=tblPurchaseUnloadingDtl.prodItemId " +
                                  " LEFT JOIN tblVariables tblVariables ON tblVariables.idVariable =  tblPurchaseUnloadingDtl.processVarId ";
                                  
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl()
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

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(Int32 purchaseWeighingStageId, Int32 isGradingBeforeUnld = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseWeighingStageId=" + purchaseWeighingStageId + " AND ISNULL(isGradingBeforeUnld,0) = " + isGradingBeforeUnld + " ORDER BY tblProductItem.displaySequanceNo";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId, Int32 isGradingBeforeUnloading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + " AND ISNULL(isGradingBeforeUnld,0)=" + isGradingBeforeUnloading;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtlListByScheduleId(Int32 purchaseScheduleId, Int32 isGradingBeforeUnloading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + " AND ISNULL(isGradingBeforeUnld,0)=" + isGradingBeforeUnloading;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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
        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(Int32 purchaseWeighingStageId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseWeighingStageId=" + purchaseWeighingStageId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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
        public List<TblPurchaseUnloadingDtlTO> SelectTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPurchaseUnloadingDtl = " + idPurchaseUnloadingDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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

        public List<TblPurchaseUnloadingDtlTO> ConvertDTToList(SqlDataReader tblPurchaseUnloadingDtlTODT)
        {
            List<TblPurchaseUnloadingDtlTO> tblPurchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
            if (tblPurchaseUnloadingDtlTODT != null)
            {
                while (tblPurchaseUnloadingDtlTODT.Read())
                {
                    TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTONew = new TblPurchaseUnloadingDtlTO();
                    if (tblPurchaseUnloadingDtlTODT["idPurchaseUnloadingDtl"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IdPurchaseUnloadingDtl = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["idPurchaseUnloadingDtl"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["purchaseWeighingStageId"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.PurchaseWeighingStageId = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["purchaseWeighingStageId"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.ProdItemId = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["prodItemId"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["createdBy"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.CreatedBy = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["createdBy"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["createdOn"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.CreatedOn = Convert.ToDateTime(tblPurchaseUnloadingDtlTODT["createdOn"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["qtyMT"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.QtyMT = Convert.ToDouble(tblPurchaseUnloadingDtlTODT["qtyMT"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["itemName"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.ItemName = Convert.ToString(tblPurchaseUnloadingDtlTODT["itemName"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["isConfirmUnloading"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IsConfirmUnloading = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["isConfirmUnloading"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["purchaseScheduleSummaryId"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["IsGradingBeforeUnld"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IsGradingBeforeUnld = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["IsGradingBeforeUnld"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["IsNextUnldGrade"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IsNextUnldGrade = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["IsNextUnldGrade"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["isNonCommercialItem"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IsNonCommercialItem = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["isNonCommercialItem"].ToString());
                    if (tblPurchaseUnloadingDtlTODT["recovery"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.Recovery = Convert.ToDouble(tblPurchaseUnloadingDtlTODT["recovery"].ToString());

                    if (tblPurchaseUnloadingDtlTODT["isGradeSelected"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.IsGradeSelected = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["isGradeSelected"].ToString());

                    if (tblPurchaseUnloadingDtlTODT["processVarId"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.ProcessVarId = Convert.ToInt32(tblPurchaseUnloadingDtlTODT["processVarId"].ToString());

                    if (tblPurchaseUnloadingDtlTODT["variableDisplayname"] != DBNull.Value)
                        tblPurchaseUnloadingDtlTONew.ProcessVarDesc = Convert.ToString(tblPurchaseUnloadingDtlTODT["variableDisplayname"].ToString());


                    tblPurchaseUnloadingDtlTOList.Add(tblPurchaseUnloadingDtlTONew);
                }
            }
            return tblPurchaseUnloadingDtlTOList;
        }

        public List<TblPurchaseUnloadingDtlTO> SelectAllTblPurchaseUnloadingDtl(string purchaseWeighingStageIdStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE purchaseWeighingStageId IN(" + purchaseWeighingStageIdStr +")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseUnloadingDtlTO> list = ConvertDTToList(sqlReader);
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
        #endregion

        #region Insertion
        public int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseUnloadingDtlTO, cmdInsert);
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

        public int InsertTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseUnloadingDtlTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseUnloadingDtl]( " +
            //"  [idPurchaseUnloadingDtl]" +
            "  [purchaseWeighingStageId]" +
            " ,[prodItemId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[qtyMT]" +
            " ,[isConfirmUnloading]" +
            " ,[purchaseScheduleSummaryId]" +
            " ,[isGradingBeforeUnld]" +
            " ,[isNextUnldGrade]" +
            " ,[recovery]" +
            " ,[isGradeSelected]" +
            " ,[processVarId]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseUnloadingDtl " +
            "  @PurchaseWeighingStageId " +
            " ,@ProdItemId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@QtyMT " +
            " ,@IsConfirmUnloading " +
            " ,@PurchaseScheduleSummaryId " +
            " ,@IsGradingBeforeUnld " +
            " ,@IsNextUnldGrade " +
            " ,@Recovery " +
            " ,@IsGradeSelected " +
            " ,@ProcessVarId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
            cmdInsert.Parameters.Add("@PurchaseWeighingStageId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.PurchaseWeighingStageId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.ProdItemId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseUnloadingDtlTO.CreatedOn;
            cmdInsert.Parameters.Add("@QtyMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseUnloadingDtlTO.QtyMT;
            cmdInsert.Parameters.Add("@IsConfirmUnloading", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsConfirmUnloading;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@IsGradingBeforeUnld", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsGradingBeforeUnld;
            cmdInsert.Parameters.Add("@IsNextUnldGrade", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsNextUnldGrade;
            cmdInsert.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseUnloadingDtlTO.Recovery);
            cmdInsert.Parameters.Add("@IsGradeSelected", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsGradeSelected;
            cmdInsert.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseUnloadingDtlTO.ProcessVarId);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseUnloadingDtlTO, cmdUpdate);
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

        public int UpdateTblPurchaseUnloadingDtl(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseUnloadingDtlTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPurchaseUnloadingDtlTO tblPurchaseUnloadingDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseUnloadingDtl] SET " +
            //"  [idPurchaseUnloadingDtl] = @IdPurchaseUnloadingDtl" +
            "  [purchaseWeighingStageId]= @PurchaseWeighingStageId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[qtyMT] = @QtyMT" +
            " ,[isConfirmUnloading] = @IsConfirmUnloading" +
            " ,[purchaseScheduleSummaryId] = @PurchaseScheduleSummaryId" +
            " ,[isGradingBeforeUnld] = @IsGradingBeforeUnld" +
            " ,[isNextUnldGrade ] = @IsNextUnldGrade " +
            " ,[recovery] = @Recovery " +
            " ,[isGradeSelected ] = @IsGradeSelected" +
            " ,[processVarId ] = @ProcessVarId" +

            " WHERE idPurchaseUnloadingDtl = @IdPurchaseUnloadingDtl ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
            cmdUpdate.Parameters.Add("@PurchaseWeighingStageId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.PurchaseWeighingStageId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.ProdItemId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseUnloadingDtlTO.CreatedOn;
            cmdUpdate.Parameters.Add("@QtyMT", System.Data.SqlDbType.NVarChar).Value = tblPurchaseUnloadingDtlTO.QtyMT;
            cmdUpdate.Parameters.Add("@IsConfirmUnloading", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsConfirmUnloading;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@IsGradingBeforeUnld", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsGradingBeforeUnld;
            cmdUpdate.Parameters.Add("@IsNextUnldGrade", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsNextUnldGrade;
            cmdUpdate.Parameters.Add("@Recovery", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseUnloadingDtlTO.Recovery);
            cmdUpdate.Parameters.Add("@IsGradeSelected", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IsGradeSelected;
            cmdUpdate.Parameters.Add("@ProcessVarId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseUnloadingDtlTO.ProcessVarId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseUnloadingDtl, cmdDelete);
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

        public int DeleteTblPurchaseUnloadingDtl(Int32 idPurchaseUnloadingDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseUnloadingDtl, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPurchaseUnloadingDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseUnloadingDtl] " +
            " WHERE idPurchaseUnloadingDtl = " + idPurchaseUnloadingDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseUnloadingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseUnloadingDtlTO.IdPurchaseUnloadingDtl;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllUnloadingDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseUnloadingDtl WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
