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
    public class TblExpressionDtlsDAO : ITblExpressionDtlsDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblExpressionDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblExpressionDtls] tblExpressionDtls";
            return sqlSelectQry;
        }
        #endregion

        #region Selection

        public List<TblExpressionDtlsTO> GetHistoryOfExpressionsbyUniqueNo(int uniqueTrackId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where uniqueTrackId = " + uniqueTrackId + " order by createdOn desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
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
        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isActive = 1 order by seqNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (isActive == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " order by seqNo asc";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblExpressionDtls.isActive=1 AND tblExpressionDtls.prodClassId=" + prodClassId + " order by seqNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(Int32 isActive, Int32 prodClassId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (isActive == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " order by seqNo asc";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblExpressionDtls.isActive=1 AND tblExpressionDtls.prodClassId=" + prodClassId + " order by seqNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
                reader.Dispose();

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
        public List<TblExpressionDtlsTO> SelectTblExpressionDtls(Int32 idExpDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idExpDtls = " + idExpDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblExpressionDtlsTO> SelectAllTblExpressionDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " order by seqNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblExpressionDtlsTO> list = ConvertDTToList(reader);
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

        public List<TblExpressionDtlsTO> ConvertDTToList(SqlDataReader tblExpressionDtlsTODT)
        {
            List<TblExpressionDtlsTO> tblExpressionDtlsTOList = new List<TblExpressionDtlsTO>();
            if (tblExpressionDtlsTODT != null)
            {
                while (tblExpressionDtlsTODT.Read())
                {
                    TblExpressionDtlsTO tblExpressionDtlsTONew = new TblExpressionDtlsTO();
                    if (tblExpressionDtlsTODT["idExpDtls"] != DBNull.Value)
                        tblExpressionDtlsTONew.IdExpDtls = Convert.ToInt32(tblExpressionDtlsTODT["idExpDtls"].ToString());
                    if (tblExpressionDtlsTODT["isActive"] != DBNull.Value)
                        tblExpressionDtlsTONew.IsActive = Convert.ToInt32(tblExpressionDtlsTODT["isActive"].ToString());
                    if (tblExpressionDtlsTODT["createdBy"] != DBNull.Value)
                        tblExpressionDtlsTONew.CreatedBy = Convert.ToInt32(tblExpressionDtlsTODT["createdBy"].ToString());
                    if (tblExpressionDtlsTODT["updatedBy"] != DBNull.Value)
                        tblExpressionDtlsTONew.UpdatedBy = Convert.ToInt32(tblExpressionDtlsTODT["updatedBy"].ToString());
                    if (tblExpressionDtlsTODT["createdOn"] != DBNull.Value)
                        tblExpressionDtlsTONew.CreatedOn = Convert.ToDateTime(tblExpressionDtlsTODT["createdOn"].ToString());
                    if (tblExpressionDtlsTODT["updatedOn"] != DBNull.Value)
                        tblExpressionDtlsTONew.UpdatedOn = Convert.ToDateTime(tblExpressionDtlsTODT["updatedOn"].ToString());
                    if (tblExpressionDtlsTODT["expFormula"] != DBNull.Value)
                        tblExpressionDtlsTONew.ExpFormula = Convert.ToString(tblExpressionDtlsTODT["expFormula"].ToString());
                    if (tblExpressionDtlsTODT["expCode"] != DBNull.Value)
                        tblExpressionDtlsTONew.ExpCode = Convert.ToString(tblExpressionDtlsTODT["expCode"].ToString());
                    if (tblExpressionDtlsTODT["expDesc"] != DBNull.Value)
                        tblExpressionDtlsTONew.ExpDesc = Convert.ToString(tblExpressionDtlsTODT["expDesc"].ToString());
                    if (tblExpressionDtlsTODT["seqNo"] != DBNull.Value)
                        tblExpressionDtlsTONew.SeqNo = Convert.ToInt32(tblExpressionDtlsTODT["seqNo"].ToString());

                    if (tblExpressionDtlsTODT["expDisplayName"] != DBNull.Value)
                        tblExpressionDtlsTONew.ExpDisplayName = Convert.ToString(tblExpressionDtlsTODT["expDisplayName"].ToString());

                    if (tblExpressionDtlsTODT["prodClassId"] != DBNull.Value)
                        tblExpressionDtlsTONew.ProdClassId = Convert.ToInt32(tblExpressionDtlsTODT["prodClassId"].ToString());

                    if (tblExpressionDtlsTODT["maxRecVal"] != DBNull.Value)
                        tblExpressionDtlsTONew.MaxRecVal = Convert.ToDouble(tblExpressionDtlsTODT["maxRecVal"].ToString());
                    if (tblExpressionDtlsTODT["includeInMetalCost"] != DBNull.Value)
                        tblExpressionDtlsTONew.IncludeInMetalCost = Convert.ToInt32(tblExpressionDtlsTODT["includeInMetalCost"].ToString());

                    if (tblExpressionDtlsTODT["cOrNcId"] != DBNull.Value)
                        tblExpressionDtlsTONew.COrNcId = Convert.ToInt32(tblExpressionDtlsTODT["cOrNcId"].ToString());

                    if (tblExpressionDtlsTODT["isRecValFrmVariables"] != DBNull.Value)
                        tblExpressionDtlsTONew.IsRecValFrmVariables = Convert.ToString(tblExpressionDtlsTODT["isRecValFrmVariables"].ToString());


                    if (tblExpressionDtlsTODT["uniqueTrackId"] != DBNull.Value)
                        tblExpressionDtlsTONew.UniqueTrackId = Convert.ToInt32(tblExpressionDtlsTODT["uniqueTrackId"].ToString());



                    tblExpressionDtlsTOList.Add(tblExpressionDtlsTONew);
                }
            }
            return tblExpressionDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblExpressionDtlsTO, cmdInsert);
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

        public int InsertTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblExpressionDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblExpressionDtlsTO tblExpressionDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblExpressionDtls]( " +
            //"  [idExpDtls]" +
            "  [isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[expFormula]" +
            " ,[expCode]" +
            " ,[expDesc]" +
            " ,[seqNo]" +
            " ,[expDisplayName]" +
            " ,[prodClassId]" +
            " ,[maxRecVal]" +
            " ,[includeInMetalCost]" +
            " ,[cOrNcId]" +
            " ,[uniqueTrackId]" +
            " )" +
" VALUES (" +
            //"  @IdExpDtls " +
            "  @IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@ExpFormula " +
            " ,@ExpCode " +
            " ,@ExpDesc " +
            " ,@SeqNo " +
            " ,@ExpDisplayName " +
            " ,@ProdClassId " +
            " ,@MaxRecVal " +
            " ,@IncludeInMetalCost " +
            " ,@COrNcId " +
            ",@UniqueTrackId " +
            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblExpressionDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblExpressionDtlsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@ExpFormula", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpFormula;
            cmdInsert.Parameters.Add("@ExpCode", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpCode;
            cmdInsert.Parameters.Add("@ExpDesc", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpDesc;
            cmdInsert.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.SeqNo;
            cmdInsert.Parameters.Add("@ExpDisplayName", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpDisplayName;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.ProdClassId;
            cmdInsert.Parameters.Add("@MaxRecVal", System.Data.SqlDbType.Decimal).Value = tblExpressionDtlsTO.MaxRecVal;
            cmdInsert.Parameters.Add("@IncludeInMetalCost", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IncludeInMetalCost;
            cmdInsert.Parameters.Add("@COrNcId", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.COrNcId;
            cmdInsert.Parameters.Add("@UniqueTrackId", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.UniqueTrackId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblExpressionDtlsTO, cmdUpdate);
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

        public int UpdateTblExpressionDtls(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblExpressionDtlsTO, cmdUpdate);
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

        public int UpdateTblExpressionDtlsEdit(TblExpressionDtlsTO tblExpressionDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandEdit(tblExpressionDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommandEdit(TblExpressionDtlsTO tblExpressionDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblExpressionDtls] SET " +
            //"  [idExpDtls] = @IdExpDtls" +
            "  [isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE idExpDtls = @IdExpDtls ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblExpressionDtlsTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }


        public int ExecuteUpdationCommand(TblExpressionDtlsTO tblExpressionDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblExpressionDtls] SET " +
            //"  [idExpDtls] = @IdExpDtls" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[expFormula]= @ExpFormula" +
            " ,[expCode]= @ExpCode" +
            " ,[expDesc] = @ExpDesc" +
            " ,[seqNo] = @SeqNo" +
            " ,[expDisplayName] = @ExpDisplayName" +
            " ,[prodClassId] = @ProdClassId" +
            " ,[maxRecVal] = @MaxRecVal" +
            " ,[includeInMetalCost] = @IncludeInMetalCost" +
            " ,[cOrNcId] = @COrNcId" +
            " WHERE idExpDtls = @IdExpDtls ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblExpressionDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblExpressionDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ExpFormula", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpFormula;
            cmdUpdate.Parameters.Add("@ExpCode", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpCode;
            cmdUpdate.Parameters.Add("@ExpDesc", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpDesc;
            cmdUpdate.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.SeqNo;
            cmdUpdate.Parameters.Add("@ExpDisplayName", System.Data.SqlDbType.VarChar).Value = tblExpressionDtlsTO.ExpDisplayName;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.ProdClassId;
            cmdUpdate.Parameters.Add("@MaxRecVal", System.Data.SqlDbType.Decimal).Value = tblExpressionDtlsTO.MaxRecVal;
            cmdUpdate.Parameters.Add("@IncludeInMetalCost", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IncludeInMetalCost;
            cmdUpdate.Parameters.Add("@COrNcId", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.COrNcId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblExpressionDtls(Int32 idExpDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idExpDtls, cmdDelete);
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

        public int DeleteTblExpressionDtls(Int32 idExpDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idExpDtls, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idExpDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblExpressionDtls] " +
            " WHERE idExpDtls = " + idExpDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idExpDtls", System.Data.SqlDbType.Int).Value = tblExpressionDtlsTO.IdExpDtls;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
