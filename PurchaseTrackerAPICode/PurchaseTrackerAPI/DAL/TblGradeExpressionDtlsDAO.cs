using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblGradeExpressionDtlsDAO : ITblGradeExpressionDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblGradeExpressionDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblGradeExpressionDtls.*,tblExpressionDtls.expCode,tblExpressionDtls.expDisplayName,tblExpressionDtls.seqNo,tblExpressionDtls.includeInMetalCost FROM tblGradeExpressionDtls tblGradeExpressionDtls " +
                                  " LEFT JOIN tblExpressionDtls tblExpressionDtls on tblExpressionDtls.idExpDtls=tblGradeExpressionDtls.expressionDtlsId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls()
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

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> SelectTblGradeExpressionDtls(Int32 idGradeExpressionDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.idGradeExpressionDtls = " + idGradeExpressionDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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
        public List<TblGradeExpressionDtlsTO> SelectGradeExpreDtlsByBaseMetalId(Int32 baseItemMetalCostId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.baseItemMetalCostId = " + baseItemMetalCostId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.purchaseEnquiryDtlsId IN (" + enquiryDetailsId + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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


        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtls(string enquiryDetailsId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.purchaseEnquiryDtlsId IN ( " + enquiryDetailsId + " )";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.purchaseScheduleDtlsId  IN (" + scheduleDtlsId + " )";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> SelectGradeExpressionDtlsByScheduleId(string scheduleDtlsId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.purchaseScheduleDtlsId  IN (" + scheduleDtlsId + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpressionDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> SelectAllTblGradeExpDtlsByGlobalRateId(string globleRatePurchaseIds,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGradeExpressionDtls.globleRatePurchaseId  IN (" + globleRatePurchaseIds + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public List<TblGradeExpressionDtlsTO> ConvertDTToList(SqlDataReader tblGradeExpressionDtlsTODT)
        {
            List<TblGradeExpressionDtlsTO> tblGradeExpressionDtlsTOList = new List<TblGradeExpressionDtlsTO>();
            if (tblGradeExpressionDtlsTODT != null)
            {
                while (tblGradeExpressionDtlsTODT.Read())
                {
                    TblGradeExpressionDtlsTO tblGradeExpressionDtlsTONew = new TblGradeExpressionDtlsTO();
                    if (tblGradeExpressionDtlsTODT["idGradeExpressionDtls"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.IdGradeExpressionDtls = Convert.ToInt32(tblGradeExpressionDtlsTODT["idGradeExpressionDtls"].ToString());
                    if (tblGradeExpressionDtlsTODT["purchaseEnquiryDtlsId"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.PurchaseEnquiryDtlsId = Convert.ToInt32(tblGradeExpressionDtlsTODT["purchaseEnquiryDtlsId"].ToString());
                    if (tblGradeExpressionDtlsTODT["purchaseScheduleDtlsId"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.PurchaseScheduleDtlsId = Convert.ToInt32(tblGradeExpressionDtlsTODT["purchaseScheduleDtlsId"].ToString());
                    if (tblGradeExpressionDtlsTODT["expressionDtlsId"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.ExpressionDtlsId = Convert.ToInt32(tblGradeExpressionDtlsTODT["expressionDtlsId"].ToString());
                    if (tblGradeExpressionDtlsTODT["gradeValue"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.GradeValue = Convert.ToDouble(tblGradeExpressionDtlsTODT["gradeValue"].ToString());
                    if (tblGradeExpressionDtlsTODT["expCode"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.ExpCode = Convert.ToString(tblGradeExpressionDtlsTODT["expCode"].ToString());
                    if (tblGradeExpressionDtlsTODT["expDisplayName"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.ExpDisplayName = Convert.ToString(tblGradeExpressionDtlsTODT["expDisplayName"].ToString());
                    if (tblGradeExpressionDtlsTODT["seqNo"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.SeqNo = Convert.ToInt32(tblGradeExpressionDtlsTODT["seqNo"].ToString());
                    if (tblGradeExpressionDtlsTODT["globleRatePurchaseId"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.GlobleRatePurchaseId = Convert.ToInt32(tblGradeExpressionDtlsTODT["globleRatePurchaseId"].ToString());
                    if (tblGradeExpressionDtlsTODT["includeInMetalCost"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.IncludeInMetalCost = Convert.ToInt32(tblGradeExpressionDtlsTODT["includeInMetalCost"].ToString());

                    if (tblGradeExpressionDtlsTODT["baseItemMetalCostId"] != DBNull.Value)
                        tblGradeExpressionDtlsTONew.BaseItemMetalCostId = Convert.ToInt32(tblGradeExpressionDtlsTODT["baseItemMetalCostId"].ToString());



                    tblGradeExpressionDtlsTOList.Add(tblGradeExpressionDtlsTONew);
                }
            }
            return tblGradeExpressionDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGradeExpressionDtlsTO, cmdInsert);
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

        public int InsertTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGradeExpressionDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGradeExpressionDtls]( " +
            //"  [idGradeExpressionDtls]" +
            "  [purchaseEnquiryDtlsId]" +
            " ,[purchaseScheduleDtlsId]" +
            " ,[expressionDtlsId]" +
            " ,[gradeValue]" +
            " ,[globleRatePurchaseId]" +
            " ,[baseItemMetalCostId]" +
            " )" +
" VALUES (" +
            //"  @IdGradeExpressionDtls " +
            "  @PurchaseEnquiryDtlsId " +
            " ,@PurchaseScheduleDtlsId " +
            " ,@ExpressionDtlsId " +
            " ,@GradeValue " +
            " ,@GlobleRatePurchaseId " +
            " ,@BaseItemMetalCostId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
            cmdInsert.Parameters.Add("@PurchaseEnquiryDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.PurchaseEnquiryDtlsId);
            cmdInsert.Parameters.Add("@PurchaseScheduleDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.PurchaseScheduleDtlsId);
            cmdInsert.Parameters.Add("@ExpressionDtlsId", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.ExpressionDtlsId;
            cmdInsert.Parameters.Add("@GradeValue", System.Data.SqlDbType.NVarChar).Value = tblGradeExpressionDtlsTO.GradeValue;
            cmdInsert.Parameters.Add("@GlobleRatePurchaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.GlobleRatePurchaseId);
            cmdInsert.Parameters.Add("@BaseItemMetalCostId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.BaseItemMetalCostId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblGradeExpressionDtlsTO.IdGradeExpressionDtls = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGradeExpressionDtlsTO, cmdUpdate);
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

        public int UpdateTblGradeExpressionDtls(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGradeExpressionDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblGradeExpressionDtlsTO tblGradeExpressionDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGradeExpressionDtls] SET " +
            //"  [idGradeExpressionDtls] = @IdGradeExpressionDtls" +
            "  [purchaseEnquiryDtlsId]= @PurchaseEnquiryDtlsId" +
            " ,[purchaseScheduleDtlsId]= @PurchaseScheduleDtlsId" +
            " ,[expressionDtlsId]= @ExpressionDtlsId" +
            " ,[gradeValue] = @GradeValue" +
            " ,[globleRatePurchaseId] = @GlobleRatePurchaseId" +
            " ,[baseItemMetalCostId] = @BaseItemMetalCostId" +
            " WHERE idGradeExpressionDtls = @IdGradeExpressionDtls ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.PurchaseEnquiryDtlsId);
            cmdUpdate.Parameters.Add("@PurchaseScheduleDtlsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.PurchaseScheduleDtlsId);
            cmdUpdate.Parameters.Add("@ExpressionDtlsId", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.ExpressionDtlsId;
            cmdUpdate.Parameters.Add("@GradeValue", System.Data.SqlDbType.NVarChar).Value = tblGradeExpressionDtlsTO.GradeValue;
            cmdUpdate.Parameters.Add("@GlobleRatePurchaseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.GlobleRatePurchaseId);
            cmdUpdate.Parameters.Add("@BaseItemMetalCostId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGradeExpressionDtlsTO.BaseItemMetalCostId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGradeExpressionDtls, cmdDelete);
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

        public int DeleteTblGradeExpressionDtls(Int32 idGradeExpressionDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGradeExpressionDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idGradeExpressionDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGradeExpressionDtls] " +
            " WHERE idGradeExpressionDtls = " + idGradeExpressionDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGradeExpressionDtls", System.Data.SqlDbType.Int).Value = tblGradeExpressionDtlsTO.IdGradeExpressionDtls;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeleteAllGradeExpDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblGradeExpressionDtls WHERE purchaseScheduleDtlsId IN (SELECT idPurchaseScheduleDetails FROM tblPurchaseScheduleDetails WHERE purchaseScheduleSummaryId=" + purchaseScheduleId + ")";
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

    }
}
