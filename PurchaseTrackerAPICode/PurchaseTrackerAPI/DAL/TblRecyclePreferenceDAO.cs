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
    public class TblRecyclePreferenceDAO : ITblRecyclePreferenceDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRecyclePreferenceDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblRecyclePreference]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblRecyclePreferenceTO> SelectAllTblRecyclePreference()
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

                //cmdSelect.Parameters.Add("@idPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecyclePreferenceTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                // String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                // String userName = System.Windows.Forms.SystemInformation.UserName;
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblRecyclePreferenceTO SelectTblRecyclePreference(Int32 idPreference)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPreference = " + idPreference + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecyclePreferenceTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;

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

    public  TblRecyclePreferenceTO SelectTblRecyclePreferenceValByName(string settingKey)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE"
                    + " settingKey = '" + settingKey + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecyclePreferenceTO> list = ConvertDTToList(sqlReader);
                TblRecyclePreferenceTO tblRecyclePreferenceTO = new TblRecyclePreferenceTO();
                if (list.Count > 0)
                {
                    tblRecyclePreferenceTO = list[0];
                }
                sqlReader.Dispose();
                return tblRecyclePreferenceTO;
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
        public  TblRecyclePreferenceTO SelectAllTblRecyclePreference(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecyclePreferenceTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;


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

        public  List<TblRecyclePreferenceTO> ConvertDTToList(SqlDataReader tblRecyclePreferenceTODT)
        {
            List<TblRecyclePreferenceTO> tblRecyclePreferenceTOList = new List<TblRecyclePreferenceTO>();
            if (tblRecyclePreferenceTODT != null)
            {
                while (tblRecyclePreferenceTODT.Read())
                {
                    TblRecyclePreferenceTO tblRecyclePreferenceTONew = new TblRecyclePreferenceTO();
                    if (tblRecyclePreferenceTODT["idPreference"] != DBNull.Value)
                        tblRecyclePreferenceTONew.IdPreference = Convert.ToInt32(tblRecyclePreferenceTODT["idPreference"].ToString());
                    if (tblRecyclePreferenceTODT["settingKey"] != DBNull.Value)
                        tblRecyclePreferenceTONew.SettingKey = Convert.ToString(tblRecyclePreferenceTODT["settingKey"].ToString());
                    if (tblRecyclePreferenceTODT["settingValue"] != DBNull.Value)
                        tblRecyclePreferenceTONew.SettingValue = Convert.ToString(tblRecyclePreferenceTODT["settingValue"].ToString());
                    tblRecyclePreferenceTOList.Add(tblRecyclePreferenceTONew);
                }
            }
            return tblRecyclePreferenceTOList;
        }
        #endregion

        #region Insertion
        public  int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRecyclePreferenceTO, cmdInsert);
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

        public  int InsertTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRecyclePreferenceTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRecyclePreference]( " +
            //"  [idPreference]" +
            "  [settingKey]" +
            " ,[settingValue]" +
            " )" +
" VALUES (" +
            //"  @IdPreference " +
            "  @SettingKey " +
            " ,@SettingValue " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
            cmdInsert.Parameters.Add("@SettingKey", System.Data.SqlDbType.NVarChar).Value = tblRecyclePreferenceTO.SettingKey;
            cmdInsert.Parameters.Add("@SettingValue", System.Data.SqlDbType.NVarChar).Value = tblRecyclePreferenceTO.SettingValue;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRecyclePreferenceTO, cmdUpdate);
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

        public  int UpdateTblRecyclePreference(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRecyclePreferenceTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblRecyclePreferenceTO tblRecyclePreferenceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRecyclePreference] SET " +
            "  [idPreference] = @IdPreference" +
            " ,[settingKey]= @SettingKey" +
            " ,[settingValue] = @SettingValue" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
            cmdUpdate.Parameters.Add("@SettingKey", System.Data.SqlDbType.NVarChar).Value = tblRecyclePreferenceTO.SettingKey;
            cmdUpdate.Parameters.Add("@SettingValue", System.Data.SqlDbType.NVarChar).Value = tblRecyclePreferenceTO.SettingValue;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblRecyclePreference(Int32 idPreference)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPreference, cmdDelete);
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

        public  int DeleteTblRecyclePreference(Int32 idPreference, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPreference, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPreference, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRecyclePreference] " +
            " WHERE idPreference = " + idPreference + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPreference", System.Data.SqlDbType.Int).Value = tblRecyclePreferenceTO.IdPreference;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
