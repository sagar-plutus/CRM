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
    public class TblPurchaseVehicleStageCntDAO : ITblPurchaseVehicleStageCntDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehicleStageCntDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseVehicleStageCnt]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt()
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

                //cmdSelect.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStageCntTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseVehicleStageCntTO> SelectTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE IdPurchaseVehicleStageCnt = " + idPurchaseVehicleStageCnt + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStageCntTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseVehicleStageCntTO> SelectPurchaseVehicleStageCntByRootScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE rootScheduleId = " + rootScheduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStageCntTO> list = ConvertDTToList(reader);
                reader.Close();
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

        public  List<TblPurchaseVehicleStageCntTO> ConvertDTToList(SqlDataReader tblPurchaseVehicleStageCntTODT)
        {
            List<TblPurchaseVehicleStageCntTO> tblPurchaseVehicleStageCntTOList = new List<TblPurchaseVehicleStageCntTO>();
            if (tblPurchaseVehicleStageCntTODT != null)
            {
                while (tblPurchaseVehicleStageCntTODT.Read())
                {
                    TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTONew = new TblPurchaseVehicleStageCntTO();
                    if (tblPurchaseVehicleStageCntTODT["IdPurchaseVehicleStageCnt"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.IdPurchaseVehicleStageCnt = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["IdPurchaseVehicleStageCnt"].ToString());
                    if (tblPurchaseVehicleStageCntTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.RootScheduleId = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["rootScheduleId"].ToString());
                    if (tblPurchaseVehicleStageCntTODT["wtStageCompCnt"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.WtStageCompCnt = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["wtStageCompCnt"].ToString());
                    if (tblPurchaseVehicleStageCntTODT["unloadingCompCnt"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.UnloadingCompCnt = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["unloadingCompCnt"].ToString());
                    if (tblPurchaseVehicleStageCntTODT["gradingCompCnt"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.GradingCompCnt = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["gradingCompCnt"].ToString());
                    if (tblPurchaseVehicleStageCntTODT["recoveryCompCnt"] != DBNull.Value)
                        tblPurchaseVehicleStageCntTONew.RecoveryCompCnt = Convert.ToInt32(tblPurchaseVehicleStageCntTODT["recoveryCompCnt"].ToString());
                    tblPurchaseVehicleStageCntTOList.Add(tblPurchaseVehicleStageCntTONew);
                }
            }
            return tblPurchaseVehicleStageCntTOList;
        }

        public  List<TblPurchaseVehicleStageCntTO> SelectAllTblPurchaseVehicleStageCnt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehicleStageCntTO> list = ConvertDTToList(reader);
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

        #endregion

        #region Insertion
        public  int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehicleStageCntTO, cmdInsert);
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

        public  int InsertTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehicleStageCntTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseVehicleStageCnt]( " +
            //"  [IdPurchaseVehicleStageCnt]" +
            "  [rootScheduleId]" +
            " ,[wtStageCompCnt]" +
            " ,[unloadingCompCnt]" +
            " ,[gradingCompCnt]" +
            " ,[recoveryCompCnt]" +
            " )" +
" VALUES (" +
            //"  @IdPurchaseVehicleStageCnt " +
            "  @RootScheduleId " +
            " ,@WtStageCompCnt " +
            " ,@UnloadingCompCnt " +
            " ,@GradingCompCnt " +
            " ,@RecoveryCompCnt " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
            cmdInsert.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.RootScheduleId;
            cmdInsert.Parameters.Add("@WtStageCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.WtStageCompCnt;
            cmdInsert.Parameters.Add("@UnloadingCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.UnloadingCompCnt;
            cmdInsert.Parameters.Add("@GradingCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.GradingCompCnt;
            cmdInsert.Parameters.Add("@RecoveryCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.RecoveryCompCnt;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehicleStageCntTO, cmdUpdate);
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

        public  int UpdateTblPurchaseVehicleStageCnt(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehicleStageCntTO, cmdUpdate);
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


        public  int UpdateTblPurchaseVehicleStageCntForWeighing(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForWeighing(rootScheduleId, cmdUpdate);
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


        public  int ExecuteUpdationCommandForWeighing(Int32 rootScheduleId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehicleStageCnt] SET " +
            " [wtStageCompCnt]= [wtStageCompCnt] " +
            " WHERE rootScheduleId = @RootScheduleId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = rootScheduleId;
            return cmdUpdate.ExecuteNonQuery();
        }

        public  int ExecuteUpdationCommand(TblPurchaseVehicleStageCntTO tblPurchaseVehicleStageCntTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehicleStageCnt] SET " +
            " [rootScheduleId]= @RootScheduleId" +
            " ,[wtStageCompCnt]= @WtStageCompCnt" +
            " ,[unloadingCompCnt]= @UnloadingCompCnt" +
            " ,[gradingCompCnt]= @GradingCompCnt" +
            " ,[recoveryCompCnt] = @RecoveryCompCnt" +
            " WHERE IdPurchaseVehicleStageCnt = @IdPurchaseVehicleStageCnt and rootScheduleId = @RootScheduleId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.RootScheduleId;
            cmdUpdate.Parameters.Add("@WtStageCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.WtStageCompCnt;
            cmdUpdate.Parameters.Add("@UnloadingCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.UnloadingCompCnt;
            cmdUpdate.Parameters.Add("@GradingCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.GradingCompCnt;
            cmdUpdate.Parameters.Add("@RecoveryCompCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.RecoveryCompCnt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseVehicleStageCnt, cmdDelete);
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

        public  int DeleteTblPurchaseVehicleStageCnt(Int32 idPurchaseVehicleStageCnt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseVehicleStageCnt, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPurchaseVehicleStageCnt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseVehicleStageCnt] " +
            " WHERE IdPurchaseVehicleStageCnt = " + idPurchaseVehicleStageCnt + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@IdPurchaseVehicleStageCnt", System.Data.SqlDbType.Int).Value = tblPurchaseVehicleStageCntTO.IdPurchaseVehicleStageCnt;
            return cmdDelete.ExecuteNonQuery();
        }

        public  int DeleteAllStageCntAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseVehicleStageCnt WHERE rootScheduleId=" + purchaseScheduleId + "";
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
