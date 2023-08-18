using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblVariablesDAO : ITblVariablesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblVariablesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "SELECT tblVariables.*,tblUser.userDisplayName as createdByName FROM [tblVariables] tblVariables left join tblUser tblUser on tblUser.idUser = tblVariables.createdBy ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblVariablesTO> SelectAllTblVariables(Int32 isActive)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (isActive == 0)
                    cmdSelect.CommandText = SqlSelectQuery();
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " where tblVariables.isActive=1 order by sequanceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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
        public List<TblVariablesTO> GetHistoryOfVariablesbyUniqueNo(int uniqueTrackId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where uniqueTrackId = " + uniqueTrackId + " order by createdOn desc ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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

        public List<DropDownTO> SelectVariableList(Int32 isProcessVar)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();

                string isActiveCond = " AND tblVariables.isActive = 1";
                string orderByStr = " ORDER BY sequanceNo ASC";

                if (isProcessVar == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblVariables.isProcessVar = 0"; 
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblVariables.isProcessVar = 1";

                cmdSelect.CommandText += isActiveCond + orderByStr;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idVariable"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idVariable"].ToString());
                    if (dateReader["variableDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["variableDisplayName"].ToString());
                    if (dateReader["variableCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["variableCode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

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

        public int UpdateTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandEdit(tblVariablesTO, cmdUpdate);
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

        public List<TblVariablesTO> SelectAllTblVariables()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " order by sequanceNo asc ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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


        public List<TblVariablesTO> SelectTblVariables(Int32 idVariable)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVariable = " + idVariable + " order by sequanceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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

        public List<TblVariablesTO> SelectVariableCodeDtls(String variableCode,DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE variableCode = " + "'" + variableCode + "'" + "";

                if (fromDate != new DateTime())
                {
                    cmdSelect.CommandText += " AND createdOn BETWEEN @fromDate AND @toDate ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate.Date;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate.Date;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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

        public List<TblVariablesTO> SelectAllTblVariables(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " order by sequanceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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

        public List<TblVariablesTO> SelectActiveVariablesList(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " where tblVariables.isActive=1 order by sequanceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVariablesTO> list = ConvertDTToList(reader);
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

        public List<TblVariablesTO> ConvertDTToList(SqlDataReader tblVariablesTODT)
        {
            List<TblVariablesTO> tblVariablesTOList = new List<TblVariablesTO>();
            if (tblVariablesTODT != null)
            {
                while (tblVariablesTODT.Read())
                {
                    TblVariablesTO tblVariablesTONew = new TblVariablesTO();
                    if (tblVariablesTODT["idVariable"] != DBNull.Value)
                        tblVariablesTONew.IdVariable = Convert.ToInt32(tblVariablesTODT["idVariable"].ToString());
                    if (tblVariablesTODT["isPerc"] != DBNull.Value)
                        tblVariablesTONew.IsPerc = Convert.ToInt32(tblVariablesTODT["isPerc"].ToString());
                    if (tblVariablesTODT["isActive"] != DBNull.Value)
                        tblVariablesTONew.IsActive = Convert.ToInt32(tblVariablesTODT["isActive"].ToString());
                    if (tblVariablesTODT["createdBy"] != DBNull.Value)
                        tblVariablesTONew.CreatedBy = Convert.ToInt32(tblVariablesTODT["createdBy"].ToString());
                    if (tblVariablesTODT["updatedBy"] != DBNull.Value)
                        tblVariablesTONew.UpdatedBy = Convert.ToInt32(tblVariablesTODT["updatedBy"].ToString());
                    if (tblVariablesTODT["createdOn"] != DBNull.Value)
                        tblVariablesTONew.CreatedOn = Convert.ToDateTime(tblVariablesTODT["createdOn"].ToString());
                    if (tblVariablesTODT["updatedOn"] != DBNull.Value)
                        tblVariablesTONew.UpdatedOn = Convert.ToDateTime(tblVariablesTODT["updatedOn"].ToString());
                    if (tblVariablesTODT["value"] != DBNull.Value)
                        tblVariablesTONew.VariableValue = Convert.ToDouble(tblVariablesTODT["value"].ToString());
                    if (tblVariablesTODT["variableDisplayName"] != DBNull.Value)
                        tblVariablesTONew.VariableDisplayName = Convert.ToString(tblVariablesTODT["variableDisplayName"].ToString());
                    if (tblVariablesTODT["variableCode"] != DBNull.Value)
                        tblVariablesTONew.VariableCode = Convert.ToString(tblVariablesTODT["variableCode"].ToString());
                    if (tblVariablesTODT["createdByName"] != DBNull.Value)
                        tblVariablesTONew.CreatedByName = Convert.ToString(tblVariablesTODT["createdByName"].ToString());
                    if (tblVariablesTODT["sequanceNo"] != DBNull.Value)
                        tblVariablesTONew.SequanceNo = Convert.ToInt32(tblVariablesTODT["sequanceNo"].ToString());
                    if (tblVariablesTODT["uniqueTrackId"] != DBNull.Value)
                        tblVariablesTONew.UniqueTrackId = Convert.ToInt32(tblVariablesTODT["uniqueTrackId"].ToString());
                    if (tblVariablesTODT["isProcessVar"] != DBNull.Value)
                        tblVariablesTONew.IsProcessVar = Convert.ToInt32(tblVariablesTODT["isProcessVar"].ToString());

                    tblVariablesTOList.Add(tblVariablesTONew);
                }
            }
            return tblVariablesTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblVariables(TblVariablesTO tblVariablesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVariablesTO, cmdInsert);
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

        public int InsertTblVariablesEdit(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandEdit(tblVariablesTO, cmdInsert);
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

        public int InsertTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVariablesTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblVariablesTO tblVariablesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVariables]( " +
            //"  [idVariable]" +
            "  [isPerc]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[value]" +
            " ,[variableDisplayName]" +
            " ,[variableCode]" +
            " ,[sequanceNo]" +
            " ,[uniqueTrackId]" +
            " ,[isProcessVar]" +
            " )" +
" VALUES (" +
            //"  @IdVariable " +
            "  @IsPerc " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VariableValue " +
            " ,@VariableDisplayName " +
            " ,@VariableCode " +
            " ,@SequanceNo " +
            " ,isnull((select max(isnull(uniqueTrackId,0)) from tblVariables where isActive = 1),0) +1" +
            " ,@IsProcessVar " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
            cmdInsert.Parameters.Add("@IsPerc", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsPerc;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.CreatedBy;
            cmdInsert.Parameters.Add("@SequanceNo", System.Data.SqlDbType.Int).Value = tblVariablesTO.SequanceNo;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.UpdatedOn;
            cmdInsert.Parameters.Add("@VariableValue", System.Data.SqlDbType.Decimal).Value = tblVariablesTO.VariableValue;
            cmdInsert.Parameters.Add("@VariableDisplayName", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableDisplayName;
            cmdInsert.Parameters.Add("@VariableCode", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableCode;
            cmdInsert.Parameters.Add("@IsProcessVar", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsProcessVar;
            return cmdInsert.ExecuteNonQuery();
        }

        public int ExecuteInsertionCommandEdit(TblVariablesTO tblVariablesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVariables]( " +
            //"  [idVariable]" +
            "  [isPerc]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[value]" +
            " ,[variableDisplayName]" +
            " ,[variableCode]" +
            " ,[sequanceNo]" +
            " ,[uniqueTrackId]" +
            " ,[isProcessVar]" +
            " )" +
" VALUES (" +
            //"  @IdVariable " +
            "  @IsPerc " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VariableValue " +
            " ,@VariableDisplayName " +
            " ,@VariableCode " +
            " ,@SequanceNo " +
            " ,@UniqueTrackId" +
            " ,@IsProcessVar" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
            cmdInsert.Parameters.Add("@IsPerc", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsPerc;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.CreatedBy;
            cmdInsert.Parameters.Add("@SequanceNo", System.Data.SqlDbType.Int).Value = tblVariablesTO.SequanceNo;
            cmdInsert.Parameters.Add("@UniqueTrackId", System.Data.SqlDbType.Int).Value = tblVariablesTO.UniqueTrackId;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.UpdatedOn;
            cmdInsert.Parameters.Add("@VariableValue", System.Data.SqlDbType.Decimal).Value = tblVariablesTO.VariableValue;
            cmdInsert.Parameters.Add("@VariableDisplayName", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableDisplayName;
            cmdInsert.Parameters.Add("@VariableCode", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableCode;
            cmdInsert.Parameters.Add("@IsProcessVar", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsProcessVar;
            return cmdInsert.ExecuteNonQuery();
        }




        #endregion

        #region Updation
        public int UpdateTblVariables(TblVariablesTO tblVariablesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVariablesTO, cmdUpdate);
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

        public int UpdateTblVariables(TblVariablesTO tblVariablesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVariablesTO, cmdUpdate);
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

        public int ExecuteUpdationCommandEdit(TblVariablesTO tblVariablesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVariables] SET " +
             " [isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE idVariable = @IdVariable ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        public int ExecuteUpdationCommand(TblVariablesTO tblVariablesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVariables] SET " +
            //"  [idVariable] = @IdVariable" +
            "  [isPerc]= @IsPerc" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[value]= @VariableValue" +
            " ,[variableDisplayName]= @VariableDisplayName" +
            " ,[variableCode] = @VariableCode" +
            " ,[sequanceNo]= @SequanceNo" +
            " ,[uniquetrackId] = @UniquetrackId" +
            " ,[isProcessVar] = @IsProcessVar" +
            " WHERE idVariable = @IdVariable ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
            cmdUpdate.Parameters.Add("@IsPerc", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsPerc;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@SequanceNo", System.Data.SqlDbType.Int).Value = tblVariablesTO.SequanceNo;
            cmdUpdate.Parameters.Add("@UniqueTrackId", System.Data.SqlDbType.Int).Value = tblVariablesTO.UniqueTrackId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVariablesTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVariablesTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@VariableValue", System.Data.SqlDbType.Decimal).Value = tblVariablesTO.VariableValue;
            cmdUpdate.Parameters.Add("@VariableDisplayName", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableDisplayName;
            cmdUpdate.Parameters.Add("@VariableCode", System.Data.SqlDbType.VarChar).Value = tblVariablesTO.VariableCode;
            cmdUpdate.Parameters.Add("@IsProcessVar", System.Data.SqlDbType.Int).Value = tblVariablesTO.IsProcessVar;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblVariables(Int32 idVariable)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVariable, cmdDelete);
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

        public int DeleteTblVariables(Int32 idVariable, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVariable, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVariable, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVariables] " +
            " WHERE idVariable = " + idVariable + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVariable", System.Data.SqlDbType.Int).Value = tblVariablesTO.IdVariable;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
