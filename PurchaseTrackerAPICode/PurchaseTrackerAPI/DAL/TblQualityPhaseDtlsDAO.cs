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
    public class TblQualityPhaseDtlsDAO : ITblQualityPhaseDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblQualityPhaseDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblQualityPhaseDtls] tblQualityPhaseDtls " +
                                  " LEFT JOIN dimQualitySampleType dimQualitySampleType on dimQualitySampleType.idQualitySampleType=tblQualityPhaseDtls.qualitySampleTypeId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls()
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

                //cmdSelect.Parameters.Add("@idTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseDtlsTO> list = ConvertDTToList(reader);
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
        public  List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls(Int32 tblQualityPhaseId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblQualityPhaseId=" + tblQualityPhaseId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseDtlsTO> list = ConvertDTToList(reader);
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
        public  List<TblQualityPhaseDtlsTO> SelectTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTblQualityPhaseDtls = " + idTblQualityPhaseDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseDtlsTO> list = ConvertDTToList(reader);
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

        public  string GetVehicleNameForNotification(int purchaseScheduleSummaryId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                string name = "";
                cmdSelect.CommandText = "select vehicleNo from tblPurchaseScheduleSummary where idPurchaseScheduleSummary = " + purchaseScheduleSummaryId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    if (reader["vehicleNo"] != DBNull.Value)
                        name = (reader["vehicleNo"].ToString());
                }
                reader.Dispose();
                return name;
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

        public  int SelectAlertDefinationID(int qualitySampleTypeId, int flag, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                int defId = 0;
                cmdSelect.CommandText = "select alertDefId from tblSampleTypeAlertDef where qualitySampleTypeId =" + qualitySampleTypeId + "and statusId =" + flag;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    if (reader["alertDefId"] != DBNull.Value)
                        defId = Convert.ToInt32(reader["alertDefId"].ToString());
                }
                reader.Dispose();
                return defId;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdSelect.Dispose();
            }

        }
        public  List<TblQualityPhaseDtlsTO> SelectAllTblQualityPhaseDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseDtlsTO> list = ConvertDTToList(reader);
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

        public  List<TblQualityPhaseDtlsTO> ConvertDTToList(SqlDataReader tblQualityPhaseDtlsTODT)
        {
            List<TblQualityPhaseDtlsTO> tblQualityPhaseDtlsTOList = new List<TblQualityPhaseDtlsTO>();
            if (tblQualityPhaseDtlsTODT != null)
            {
                while (tblQualityPhaseDtlsTODT.Read())
                {
                    TblQualityPhaseDtlsTO tblQualityPhaseDtlsTONew = new TblQualityPhaseDtlsTO();
                    if (tblQualityPhaseDtlsTODT["idTblQualityPhaseDtls"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.IdTblQualityPhaseDtls = Convert.ToInt32(tblQualityPhaseDtlsTODT["idTblQualityPhaseDtls"].ToString());
                    if (tblQualityPhaseDtlsTODT["tblQualityPhaseId"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.TblQualityPhaseId = Convert.ToInt32(tblQualityPhaseDtlsTODT["tblQualityPhaseId"].ToString());
                    if (tblQualityPhaseDtlsTODT["qualitySampleTypeId"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.QualitySampleTypeId = Convert.ToInt32(tblQualityPhaseDtlsTODT["qualitySampleTypeId"].ToString());
                    if (tblQualityPhaseDtlsTODT["flagStatusId"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.FlagStatusId = Convert.ToInt32(tblQualityPhaseDtlsTODT["flagStatusId"].ToString());

                    if (tblQualityPhaseDtlsTODT["sampleTypeName"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.SampleTypeName = Convert.ToString(tblQualityPhaseDtlsTODT["sampleTypeName"].ToString());


                    if (tblQualityPhaseDtlsTODT["remark"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.Remark = Convert.ToString(tblQualityPhaseDtlsTODT["remark"].ToString());

                    if (tblQualityPhaseDtlsTODT["createdBy"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.CreatedBy = Convert.ToInt32(tblQualityPhaseDtlsTODT["createdBy"].ToString());
                    if (tblQualityPhaseDtlsTODT["updatedBy"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.UpdatedBy = Convert.ToInt32(tblQualityPhaseDtlsTODT["updatedBy"].ToString());
                    if (tblQualityPhaseDtlsTODT["createdOn"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.CreatedOn = Convert.ToDateTime(tblQualityPhaseDtlsTODT["createdOn"].ToString());
                    if (tblQualityPhaseDtlsTODT["updatedOn"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.UpdatedOn = Convert.ToDateTime(tblQualityPhaseDtlsTODT["updatedOn"].ToString());

                    if (tblQualityPhaseDtlsTODT["statusBy"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.StatusBy = Convert.ToInt32(tblQualityPhaseDtlsTODT["statusBy"].ToString());
                    if (tblQualityPhaseDtlsTODT["statusOn"] != DBNull.Value)
                        tblQualityPhaseDtlsTONew.StatusOn = Convert.ToDateTime(tblQualityPhaseDtlsTODT["statusOn"].ToString());

                    tblQualityPhaseDtlsTOList.Add(tblQualityPhaseDtlsTONew);
                }
            }
            return tblQualityPhaseDtlsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblQualityPhaseDtlsTO, cmdInsert);
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

        public  int InsertTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblQualityPhaseDtlsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblQualityPhaseDtls]( " +
            //"  [idTblQualityPhaseDtls]" +
            "  [tblQualityPhaseId]" +
            " ,[qualitySampleTypeId]" +
            " ,[flagStatusId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[statusOn]" +
            " ,[statusBy]" +
             " ,[remark]" +
            " )" +
" VALUES (" +
            //"  @IdTblQualityPhaseDtls " +
            "  @TblQualityPhaseId " +
            " ,@QualitySampleTypeId " +
            " ,@FlagStatusId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@StatusOn " +
            " ,@StatusBy " +
            " ,@Remark " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            String sqlSelectIdentityQry = "Select @@Identity";


            //cmdInsert.Parameters.Add("@IdTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
            cmdInsert.Parameters.Add("@TblQualityPhaseId", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.TblQualityPhaseId;
            cmdInsert.Parameters.Add("@QualitySampleTypeId", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.QualitySampleTypeId;
            cmdInsert.Parameters.Add("@FlagStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.FlagStatusId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.StatusOn);
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.StatusBy);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.Remark);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region Updation
        public  int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblQualityPhaseDtlsTO, cmdUpdate);
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

        public  int UpdateTblQualityPhaseDtls(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblQualityPhaseDtlsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblQualityPhaseDtlsTO tblQualityPhaseDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblQualityPhaseDtls] SET " +
            // "  [idTblQualityPhaseDtls] = @IdTblQualityPhaseDtls" +
            " [tblQualityPhaseId]= @TblQualityPhaseId" +
            " ,[qualitySampleTypeId]= @QualitySampleTypeId" +
            " ,[flagStatusId]= @FlagStatusId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[statusOn]= @StatusOn" +
            " ,[remark]= @Remark" +
            " ,[statusBy] = @StatusBy" +
            " WHERE idTblQualityPhaseDtls = @IdTblQualityPhaseDtls ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
            cmdUpdate.Parameters.Add("@TblQualityPhaseId", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.TblQualityPhaseId;
            cmdUpdate.Parameters.Add("@QualitySampleTypeId", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.QualitySampleTypeId;
            cmdUpdate.Parameters.Add("@FlagStatusId", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.FlagStatusId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@StatusOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.StatusOn);
            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.StatusBy);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseDtlsTO.Remark);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblQualityPhaseDtls, cmdDelete);
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

        public  int DeleteTblQualityPhaseDtls(Int32 idTblQualityPhaseDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblQualityPhaseDtls, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idTblQualityPhaseDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblQualityPhaseDtls] " +
            " WHERE idTblQualityPhaseDtls = " + idTblQualityPhaseDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblQualityPhaseDtls", System.Data.SqlDbType.Int).Value = tblQualityPhaseDtlsTO.IdTblQualityPhaseDtls;
            return cmdDelete.ExecuteNonQuery();
        }


        public  int DeleteAllQualityPhaseDtlsAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblQualityPhaseDtls WHERE tblQualityPhaseId in (SELECT idTblQualityPhase FROM tblQualityPhase WHERE PurchaseScheduleSummaryId=" + purchaseScheduleId + ")";
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
