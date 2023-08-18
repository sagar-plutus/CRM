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
    public class TblConfigParamsDAO : ITblConfigParamsDAO
    {



    private readonly IConnectionString _iConnectionString;
        private readonly Icommondao _iCommonDAO;
        private readonly ITblModuleDAO _iblModuleDAO;
        
        public TblConfigParamsDAO(IConnectionString iConnectionString,Icommondao icommondao, ITblModuleDAO itblModuleDAO)
        {
            _iCommonDAO = icommondao;
            _iConnectionString = iConnectionString;
            _iblModuleDAO = itblModuleDAO;
        }

        public TblConfigParamsDAO()
        {

        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblConfigParams]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblConfigParamsTO> SelectAllTblConfigParams()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        //@KKM added for IOT to check WEIGHING_MEASURE_SOURCE_ID settings

        public int IoTSetting()
        {
            int WeighingSrcConfig = (int)StaticStuff.Constants.WeighingDataSourceE.DB;
            TblModuleTO tblModuleTO = _iblModuleDAO.SelectTblModule((int)StaticStuff.Constants.DefaultModuleID);
            if(tblModuleTO != null && tblModuleTO.IotconfigSetting != 0)
            {
                WeighingSrcConfig = tblModuleTO.IotconfigSetting;
            }
            //TblConfigParamsTO tblConfigParamsTO = SelectTblConfigParamsValByName(StaticStuff.Constants.CP_WEIGHING_MEASURE_SOURCE_ID);
            //if (tblConfigParamsTO != null)
            //{
            //    WeighingSrcConfig = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            //}
            return WeighingSrcConfig;
        }

        public int IsSAPEnabled()
        {
            int value = 0;
            TblModuleTO tblModuleTO = _iblModuleDAO.SelectTblModule((int)StaticStuff.Constants.DefaultModuleID);
            if (tblModuleTO != null && tblModuleTO.IsSAPEnabled != 0)
            {
                value = tblModuleTO.IsSAPEnabled;
            }
            return value;
        }

        public TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE"
                    + " configParamName = '" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(sqlReader);
                TblConfigParamsTO tblConfigParamsTO = null;
                if (list.Count > 0)
                {
                    tblConfigParamsTO = new TblConfigParamsTO();
                    tblConfigParamsTO = list[0];
                }
                sqlReader.Dispose();
                return tblConfigParamsTO;
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

        //Gokul - To Access statically
        public static TblConfigParamsTO SelectTblConfigParamValByName(string configParamName)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM [tblConfigParams] WHERE 1=1 "
                    + "and configParamName = '" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertToList(sqlReader);
                TblConfigParamsTO tblConfigParamsTO = new TblConfigParamsTO();
                if (list.Count > 0)
                {
                    tblConfigParamsTO = list[0];
                }
                sqlReader.Dispose();
                return tblConfigParamsTO;
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



//Added By Gokul
        public static List<TblConfigParamsTO> ConvertToList(SqlDataReader tblConfigParamsTODT)
        {
            List<TblConfigParamsTO> tblConfigParamsTOList = new List<TblConfigParamsTO>();
            if (tblConfigParamsTODT != null)
            {
                while (tblConfigParamsTODT.Read())
                {
                    TblConfigParamsTO tblConfigParamsTONew = new TblConfigParamsTO();
                    if (tblConfigParamsTODT["idConfigParam"] != DBNull.Value)
                        tblConfigParamsTONew.IdConfigParam = Convert.ToInt32(tblConfigParamsTODT["idConfigParam"].ToString());
                    if (tblConfigParamsTODT["configParamValType"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamValType = Convert.ToInt32(tblConfigParamsTODT["configParamValType"].ToString());
                    if (tblConfigParamsTODT["createdOn"] != DBNull.Value)
                        tblConfigParamsTONew.CreatedOn = Convert.ToDateTime(tblConfigParamsTODT["createdOn"].ToString());
                    if (tblConfigParamsTODT["configParamName"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamName = Convert.ToString(tblConfigParamsTODT["configParamName"].ToString());
                    if (tblConfigParamsTODT["configParamVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamVal = Convert.ToString(tblConfigParamsTODT["configParamVal"].ToString());
                    if (tblConfigParamsTODT["moduleId"] != DBNull.Value)
                        tblConfigParamsTONew.ModuleId = Convert.ToInt32(tblConfigParamsTODT["moduleId"].ToString());
                    ///Hrishikesh[27 - 03 - 2018] Added
                    if (tblConfigParamsTODT["isActive"] != DBNull.Value)
                        tblConfigParamsTONew.IsActive = Convert.ToInt32(Convert.ToString(tblConfigParamsTODT["isActive"]));

                    if (tblConfigParamsTODT["configParamDisplayVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamDisplayVal = Convert.ToString(tblConfigParamsTODT["configParamDisplayVal"].ToString());

                    //if (tblConfigParamsTONew.ConfigParamVal == "1")
                    //{
                    //    tblConfigParamsTONew.BooleanFlag = "ON";
                    //    tblConfigParamsTONew.Slider = true;
                    //}
                    //else
                    //{
                    //    tblConfigParamsTONew.BooleanFlag = "OFF";
                    //    tblConfigParamsTONew.Slider = false;
                    //}
                    tblConfigParamsTOList.Add(tblConfigParamsTONew);
                }
            }
            return tblConfigParamsTOList;
        }




        public TblConfigParamsTO SelectTblConfigParams(Int32 idConfigParam)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idConfigParam = " + idConfigParam + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
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

        public  TblConfigParamsTO SelectTblConfigParams(String configParamName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE configParamName ='" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                reader.Close();
                if (list != null && list.Count == 1)
                    return list[0];
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

        public  List<TblConfigParamsTO> ConvertDTToList(SqlDataReader tblConfigParamsTODT)
        {
            List<TblConfigParamsTO> tblConfigParamsTOList = new List<TblConfigParamsTO>();
            if (tblConfigParamsTODT != null)
            {
                while (tblConfigParamsTODT.Read())
                {
                    TblConfigParamsTO tblConfigParamsTONew = new TblConfigParamsTO();
                    if (tblConfigParamsTODT["idConfigParam"] != DBNull.Value)
                        tblConfigParamsTONew.IdConfigParam = Convert.ToInt32(tblConfigParamsTODT["idConfigParam"].ToString());
                    if (tblConfigParamsTODT["configParamValType"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamValType = Convert.ToInt32(tblConfigParamsTODT["configParamValType"].ToString());
                    if (tblConfigParamsTODT["createdOn"] != DBNull.Value)
                        tblConfigParamsTONew.CreatedOn = Convert.ToDateTime(tblConfigParamsTODT["createdOn"].ToString());
                    if (tblConfigParamsTODT["configParamName"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamName = Convert.ToString(tblConfigParamsTODT["configParamName"].ToString());
                    if (tblConfigParamsTODT["configParamVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamVal = Convert.ToString(tblConfigParamsTODT["configParamVal"].ToString());
                    if (tblConfigParamsTODT["isActive"] != DBNull.Value)
                        tblConfigParamsTONew.IsActive = Convert.ToInt32(tblConfigParamsTODT["isActive"].ToString());
                    if (tblConfigParamsTODT["ConfigParamDisplayVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamDisplayVal = Convert.ToString(tblConfigParamsTODT["ConfigParamDisplayVal"].ToString());
                    tblConfigParamsTOList.Add(tblConfigParamsTONew);
                }
            }
            return tblConfigParamsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblConfigParamsTO, cmdInsert);
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

        public  int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblConfigParamsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblConfigParams]( " +
            "  [idConfigParam]" +
            " ,[configParamValType]" +
            " ,[createdOn]" +
            " ,[configParamName]" +
            " ,[configParamVal]" +
            " ,[isActive]" +
            " ,[configParamDisplayVal]" +
            " ,[moduleId]" +
            " ,[configDevDesc]" +
            " )" +
" VALUES (" +
            "  @IdConfigParam " +
            " ,@ConfigParamValType " +
            " ,@CreatedOn " +
            " ,@ConfigParamName " +
            " ,@ConfigParamVal " +
            " ,@IsActive " +
            " ,@ConfigParamDisplayVal " +
            " ,@ModuleId " +
            " ,@ConfigDevDesc" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            

            //String sqlSelectIdentityQry = "Select @@Identity";

            string cmdSelectMax = "select max(idConfigParam) from tblConfigParams";
            SqlCommand selectCmd = new SqlCommand();
            selectCmd.CommandText = cmdSelectMax;
            selectCmd.CommandType = System.Data.CommandType.Text;
            selectCmd.Connection = cmdInsert.Connection;
            selectCmd.Transaction = cmdInsert.Transaction;


            tblConfigParamsTO.IdConfigParam  = Convert.ToInt32(selectCmd.ExecuteScalar());
            tblConfigParamsTO.IdConfigParam = tblConfigParamsTO.IdConfigParam + 1;

            cmdInsert.Parameters.Add("@IdConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            cmdInsert.Parameters.Add("@ConfigParamValType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblConfigParamsTO.ConfigParamValType);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblConfigParamsTO.CreatedOn);
            cmdInsert.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamName;
            cmdInsert.Parameters.Add("@ConfigParamVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamVal;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IsActive;
            cmdInsert.Parameters.Add("@ConfigParamDisplayVal", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblConfigParamsTO.ConfigParamDisplayVal);
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.ModuleId;
            cmdInsert.Parameters.Add("@ConfigDevDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblConfigParamsTO.ConfigDevDesc);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                //cmdInsert.CommandText = sqlSelectIdentityQry;
                //tblConfigParamsTO.IdConfigParam = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public  int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblConfigParamsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblConfigParamsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblConfigParamsTO tblConfigParamsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblConfigParams] SET " +
                                "  [configParamValType]= @ConfigParamValType" +
                                " ,[createdOn]= @CreatedOn" +
                                " ,[configParamName]= @ConfigParamName" +
                                " ,[configParamVal] = @ConfigParamVal" +
                                " WHERE  [idConfigParam] = @IdConfigParam";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            cmdUpdate.Parameters.Add("@ConfigParamValType", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.ConfigParamValType;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value =  _iCommonDAO.ServerDateTime;
            cmdUpdate.Parameters.Add("@ConfigParamName", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamName;
            cmdUpdate.Parameters.Add("@ConfigParamVal", System.Data.SqlDbType.NVarChar).Value = tblConfigParamsTO.ConfigParamVal;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblConfigParams(Int32 idConfigParam)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idConfigParam, cmdDelete);
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

        public  int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idConfigParam, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idConfigParam, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblConfigParams] " +
            " WHERE idConfigParam = " + idConfigParam + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idConfigParam", System.Data.SqlDbType.Int).Value = tblConfigParamsTO.IdConfigParam;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
