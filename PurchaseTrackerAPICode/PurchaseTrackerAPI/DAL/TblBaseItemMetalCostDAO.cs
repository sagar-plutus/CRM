using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblBaseItemMetalCostDAO : ITblBaseItemMetalCostDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblBaseItemMetalCostDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblBaseItemMetalCost]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost()
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

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public List<TblBaseItemMetalCostTO> SelectTblBaseItemMetalCost(Int32 idBaseItemMetalCost)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBaseItemMetalCost = " + idBaseItemMetalCost + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE globalRatePurchaseId = " + globalRatePurchaseId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public List<TblBaseItemMetalCostTO> SelectBaseItemMetalCostByGlobalRateId(Int32 globalRatePurchaseId, Int32 cOrNcId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE globalRatePurchaseId = " + globalRatePurchaseId + " AND cOrNcId = " + cOrNcId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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
        public List<TblBaseItemMetalCostTO> SelectLatestBaseItemMetalCost(Int32 globalRatePurchaseId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE globalRatePurchaseId = " + globalRatePurchaseId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblBaseItemMetalCostTO> SelectAllTblBaseItemMetalCost(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBaseItemMetalCostTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public List<TblBaseItemMetalCostTO> ConvertDTToList(SqlDataReader tblBaseItemMetalCostTODT)
        {
            List<TblBaseItemMetalCostTO> tblBaseItemMetalCostTOList = new List<TblBaseItemMetalCostTO>();
            if (tblBaseItemMetalCostTODT != null)
            {
                while (tblBaseItemMetalCostTODT.Read())
                {
                    TblBaseItemMetalCostTO tblBaseItemMetalCostTONew = new TblBaseItemMetalCostTO();
                    if (tblBaseItemMetalCostTODT["idBaseItemMetalCost"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.IdBaseItemMetalCost = Convert.ToInt32(tblBaseItemMetalCostTODT["idBaseItemMetalCost"].ToString());
                    if (tblBaseItemMetalCostTODT["globalRatePurchaseId"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.GlobalRatePurchaseId = Convert.ToInt32(tblBaseItemMetalCostTODT["globalRatePurchaseId"].ToString());
                    if (tblBaseItemMetalCostTODT["createdBy"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.CreatedBy = Convert.ToInt32(tblBaseItemMetalCostTODT["createdBy"].ToString());
                    if (tblBaseItemMetalCostTODT["updatedBy"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.UpdatedBy = Convert.ToInt32(tblBaseItemMetalCostTODT["updatedBy"].ToString());
                    if (tblBaseItemMetalCostTODT["createdOn"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.CreatedOn = Convert.ToDateTime(tblBaseItemMetalCostTODT["createdOn"].ToString());
                    if (tblBaseItemMetalCostTODT["updatedOn"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.UpdatedOn = Convert.ToDateTime(tblBaseItemMetalCostTODT["updatedOn"].ToString());
                    if (tblBaseItemMetalCostTODT["baseMetalCostForC"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.BaseMetalCostForC = Convert.ToDouble(tblBaseItemMetalCostTODT["baseMetalCostForC"].ToString());
                    if (tblBaseItemMetalCostTODT["baseMetalCostForNC"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.BaseMetalCostForNC = Convert.ToDouble(tblBaseItemMetalCostTODT["baseMetalCostForNC"].ToString());
                    if (tblBaseItemMetalCostTODT["baseRecovery"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.BaseRecovery = Convert.ToDouble(tblBaseItemMetalCostTODT["baseRecovery"].ToString());

                    if (tblBaseItemMetalCostTODT["cOrNcId"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.COrNcId = Convert.ToInt32(tblBaseItemMetalCostTODT["cOrNcId"].ToString());

                    if (tblBaseItemMetalCostTODT["baseRate"] != DBNull.Value)
                        tblBaseItemMetalCostTONew.BaseRate = Convert.ToDouble(tblBaseItemMetalCostTODT["baseRate"].ToString());


                    tblBaseItemMetalCostTOList.Add(tblBaseItemMetalCostTONew);
                }
            }
            return tblBaseItemMetalCostTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBaseItemMetalCostTO, cmdInsert);
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

        public int InsertTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBaseItemMetalCostTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBaseItemMetalCost]( " +
            //"  [idBaseItemMetalCost]" +
            "  [globalRatePurchaseId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[baseMetalCostForC]" +
            " ,[baseMetalCostForNC]" +
            " ,[baseRecovery]" +
            " ,[cOrNcId]" +
            " ,[baseRate]" +
            " )" +
" VALUES (" +
            //"  @IdBaseItemMetalCost " +
            "  @GlobalRatePurchaseId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@BaseMetalCostForC " +
            " ,@BaseMetalCostForNC " +
            " ,@BaseRecovery " +
            " ,@COrNcId " +
            " ,@BaseRate " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
            cmdInsert.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.GlobalRatePurchaseId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBaseItemMetalCostTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBaseItemMetalCostTO.UpdatedOn;
            cmdInsert.Parameters.Add("@BaseMetalCostForC", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseMetalCostForC;
            cmdInsert.Parameters.Add("@BaseMetalCostForNC", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseMetalCostForNC;
            cmdInsert.Parameters.Add("@BaseRecovery", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseRecovery;
            cmdInsert.Parameters.Add("@COrNcId", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.COrNcId;
            cmdInsert.Parameters.Add("@BaseRate", System.Data.SqlDbType.Decimal).Value = tblBaseItemMetalCostTO.BaseRate;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBaseItemMetalCostTO.IdBaseItemMetalCost = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBaseItemMetalCostTO, cmdUpdate);
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

        public int UpdateTblBaseItemMetalCost(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBaseItemMetalCostTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblBaseItemMetalCostTO tblBaseItemMetalCostTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBaseItemMetalCost] SET " +
            //"  [idBaseItemMetalCost] = @IdBaseItemMetalCost" +
            "  [globalRatePurchaseId]= @GlobalRatePurchaseId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[baseMetalCostForC]= @BaseMetalCostForC" +
            " ,[baseMetalCostForNC]= @BaseMetalCostForNC" +
            " ,[baseRecovery] = @BaseRecovery" +
            " ,[cOrNcId] = @COrNcId" +
            " ,[baseRate] = @BaseRate" +
            " WHERE idBaseItemMetalCost = @IdBaseItemMetalCost ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
            cmdUpdate.Parameters.Add("@GlobalRatePurchaseId", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.GlobalRatePurchaseId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBaseItemMetalCostTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBaseItemMetalCostTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@BaseMetalCostForC", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseMetalCostForC;
            cmdUpdate.Parameters.Add("@BaseMetalCostForNC", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseMetalCostForNC;
            cmdUpdate.Parameters.Add("@BaseRecovery", System.Data.SqlDbType.NVarChar).Value = tblBaseItemMetalCostTO.BaseRecovery;
            cmdUpdate.Parameters.Add("@COrNcId", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.COrNcId;
            cmdUpdate.Parameters.Add("@BaseRate", System.Data.SqlDbType.Decimal).Value = tblBaseItemMetalCostTO.BaseRate;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBaseItemMetalCost, cmdDelete);
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

        public int DeleteTblBaseItemMetalCost(Int32 idBaseItemMetalCost, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBaseItemMetalCost, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idBaseItemMetalCost, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBaseItemMetalCost] " +
            " WHERE idBaseItemMetalCost = " + idBaseItemMetalCost + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBaseItemMetalCost", System.Data.SqlDbType.Int).Value = tblBaseItemMetalCostTO.IdBaseItemMetalCost;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
