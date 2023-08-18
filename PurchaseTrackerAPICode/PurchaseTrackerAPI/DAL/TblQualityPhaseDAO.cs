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
    public class TblQualityPhaseDAO : ITblQualityPhaseDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblQualityPhaseDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblQualityPhase.*,dimVehiclePhase.phaseName,dimVehiclePhase.sequanceNo FROM [tblQualityPhase] tblQualityPhase " +
                                  " LEFT JOIN dimVehiclePhase dimVehiclePhase on dimVehiclePhase.idVehiclePhase=tblQualityPhase.vehiclePhaseId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblQualityPhaseTO> SelectAllTblQualityPhase(int PurchaseScheduleSummaryId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblQualityPhase.isActive = 1"
                                                           + "and PurchaseScheduleSummaryId =" + PurchaseScheduleSummaryId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseTO> list = ConvertDTToList(reader);
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


        public  List<TblQualityPhaseTO> SelectAllTblQualityPhase()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where tblQualityPhase.isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseTO> list = ConvertDTToList(reader);
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
        public  List<DropDownTO> GetFlagType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimFlagType where isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idFlagType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idFlagType"].ToString());
                        if (reader["flagType"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["flagType"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                reader.Dispose();
                return dropDownTOList;
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


        public  List<DropDownTO> GetAllIdsForSampleType(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId,int QualitySampleTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblQualityPhaseDtls where tblQualityPhaseId in " +
                                        "(select idTblQualityPhase from tblQualityPhase where PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId +
                                        " and flagTypeId =" + flagTypeId +
                                         " and vehiclePhaseId = " + vehiclePhaseId + ") and qualitySampleTypeId ="+QualitySampleTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idTblQualityPhasedtls"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idTblQualityPhasedtls"].ToString());
                        if (reader["tblQualityPhaseId"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["tblQualityPhaseId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                reader.Dispose();
                return dropDownTOList;
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




        public  List<TblQualityPhaseTO> SelectTblQualityPhase(Int32 idTblQualityPhase)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTblQualityPhase = " + idTblQualityPhase + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseTO> list = ConvertDTToList(reader);
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

        public  List<TblQualityPhaseTO> SelectTblQualityPhaseList(Int32 purchaseScheduleSummaryId, Int32 isActive)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where purchaseScheduleSummaryId=" + purchaseScheduleSummaryId + " and tblQualityPhase.isActive=" + isActive;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idQualitySampleType", System.Data.SqlDbType.Int).Value = dimQualitySampleTypeTO.IdQualitySampleType;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public  List<TblQualityPhaseTO> SelectAllTblQualityPhase(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQualityPhaseTO> list = ConvertDTToList(reader);
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

        public  List<TblQualityPhaseTO> ConvertDTToList(SqlDataReader tblQualityPhaseTODT)
        {
            List<TblQualityPhaseTO> tblQualityPhaseTOList = new List<TblQualityPhaseTO>();
            if (tblQualityPhaseTODT != null)
            {
                while (tblQualityPhaseTODT.Read())
                {
                    TblQualityPhaseTO tblQualityPhaseTONew = new TblQualityPhaseTO();
                    if (tblQualityPhaseTODT["idTblQualityPhase"] != DBNull.Value)
                        tblQualityPhaseTONew.IdTblQualityPhase = Convert.ToInt32(tblQualityPhaseTODT["idTblQualityPhase"].ToString());
                    if (tblQualityPhaseTODT["purchaseScheduleSummaryId"] != DBNull.Value)
                        tblQualityPhaseTONew.PurchaseScheduleSummaryId = Convert.ToInt32(tblQualityPhaseTODT["purchaseScheduleSummaryId"].ToString());
                    if (tblQualityPhaseTODT["vehiclePhaseId"] != DBNull.Value)
                        tblQualityPhaseTONew.VehiclePhaseId = Convert.ToInt32(tblQualityPhaseTODT["vehiclePhaseId"].ToString());
                    if (tblQualityPhaseTODT["isActive"] != DBNull.Value)
                        tblQualityPhaseTONew.IsActive = Convert.ToInt32(tblQualityPhaseTODT["isActive"].ToString());
                    if (tblQualityPhaseTODT["createdBy"] != DBNull.Value)
                        tblQualityPhaseTONew.CreatedBy = Convert.ToInt32(tblQualityPhaseTODT["createdBy"].ToString());
                    if (tblQualityPhaseTODT["updatedBy"] != DBNull.Value)
                        tblQualityPhaseTONew.UpdatedBy = Convert.ToInt32(tblQualityPhaseTODT["updatedBy"].ToString());
                    if (tblQualityPhaseTODT["flagStatusId"] != DBNull.Value)
                        tblQualityPhaseTONew.FlagStatusId = Convert.ToInt32(tblQualityPhaseTODT["flagStatusId"].ToString());
                    if (tblQualityPhaseTODT["createdOn"] != DBNull.Value)
                        tblQualityPhaseTONew.CreatedOn = Convert.ToDateTime(tblQualityPhaseTODT["createdOn"].ToString());
                    if (tblQualityPhaseTODT["updatedOn"] != DBNull.Value)
                        tblQualityPhaseTONew.UpdatedOn = Convert.ToDateTime(tblQualityPhaseTODT["updatedOn"].ToString());
                    if (tblQualityPhaseTODT["phaseName"] != DBNull.Value)
                        tblQualityPhaseTONew.VehiclePhaseName = Convert.ToString(tblQualityPhaseTODT["phaseName"].ToString());
                    if (tblQualityPhaseTODT["sequanceNo"] != DBNull.Value)
                        tblQualityPhaseTONew.VehiclePhaseSequanceNo = Convert.ToInt32(tblQualityPhaseTODT["sequanceNo"].ToString());
                    if (tblQualityPhaseTODT["flagTypeId"] != DBNull.Value)
                        tblQualityPhaseTONew.FlagTypeId = Convert.ToInt32(tblQualityPhaseTODT["flagTypeId"].ToString());

                    tblQualityPhaseTOList.Add(tblQualityPhaseTONew);
                }
            }
            return tblQualityPhaseTOList;
        }

        public  int SelectFromDtlsAndQuality(int purchaseScheduleSummaryId, int vehiclePhaseId, int flagTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                int res = 0;
                cmdSelect.CommandText = "select* from tblQualityPhase tqp left join tblQualityPhaseDtls tqpdls "
                                       + "on tqp.idTblQualityPhase = tqpdls.tblQualityPhaseId where "
                                       + "PurchaseScheduleSummaryId = " + purchaseScheduleSummaryId
                                       + " and FlagTypeId = " + flagTypeId
                                       + " and isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader.HasRows)
                {
                    res = 1;
                }
                reader.Dispose();
                return res;
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

        #endregion

        #region Insertion
        public  int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblQualityPhaseTO, cmdInsert);
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

        public  int InsertTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblQualityPhaseTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblQualityPhase]( " +
            //"  [idTblQualityPhase]" +
            "  [purchaseScheduleSummaryId]" +
            " ,[vehiclePhaseId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[flagStatusId]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[flagTypeId]" +
            " )" +
" VALUES (" +
            //"  @IdTblQualityPhase " +
            "  @PurchaseScheduleSummaryId " +
            " ,@VehiclePhaseId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@FlagStatusId " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@FlagTypeId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
            cmdInsert.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.PurchaseScheduleSummaryId;
            cmdInsert.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.VehiclePhaseId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.UpdatedBy);
            cmdInsert.Parameters.Add("@FlagStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.FlagStatusId);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.UpdatedOn);
            cmdInsert.Parameters.Add("@FlagTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.FlagTypeId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblQualityPhaseTO.IdTblQualityPhase = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public  int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblQualityPhaseTO, cmdUpdate);
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

        public  int UpdateTblQualityPhase(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblQualityPhaseTO, cmdUpdate);
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


        public  int UpdateTblQualityPhaseDeact(TblQualityPhaseTO tblQualityPhaseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandDeAct(tblQualityPhaseTO, cmdUpdate);
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
        public  int ExecuteUpdationCommand(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblQualityPhase] SET " +
            //"  [idTblQualityPhase] = @IdTblQualityPhase" +
            "  [purchaseScheduleSummaryId]= @PurchaseScheduleSummaryId" +
            " ,[vehiclePhaseId]= @VehiclePhaseId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[flagStatusId]= @FlagStatusId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[flagTypeId]= @FlagTypeId" +
            " WHERE idTblQualityPhase = @IdTblQualityPhase ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@VehiclePhaseId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.VehiclePhaseId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@FlagStatusId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.FlagStatusId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblQualityPhaseTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@FlagTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.FlagTypeId);
            return cmdUpdate.ExecuteNonQuery();
        }

        public  int ExecuteUpdationCommandDeAct(TblQualityPhaseTO tblQualityPhaseTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblQualityPhase] SET " +
            "  [isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE purchaseScheduleSummaryId = @PurchaseScheduleSummaryId and flagTypeId = @FlagTypeId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PurchaseScheduleSummaryId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.PurchaseScheduleSummaryId;
            cmdUpdate.Parameters.Add("@FlagTypeId", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.FlagTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQualityPhaseTO.UpdatedOn);
            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

        #region Deletion
        public  int DeleteTblQualityPhase(Int32 idTblQualityPhase)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblQualityPhase, cmdDelete);
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

        public  int DeleteTblQualityPhase(Int32 idTblQualityPhase, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblQualityPhase, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idTblQualityPhase, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblQualityPhase] " +
            " WHERE idTblQualityPhase = " + idTblQualityPhase + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblQualityPhase", System.Data.SqlDbType.Int).Value = tblQualityPhaseTO.IdTblQualityPhase;
            return cmdDelete.ExecuteNonQuery();
        }

         public  int DeleteAllQualityPhaseAgainstVehSchedule(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblQualityPhase WHERE PurchaseScheduleSummaryId=" + purchaseScheduleId + "";
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
