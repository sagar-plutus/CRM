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
    public class TblWeighingDAO : ITblWeighingDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblWeighingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblWeighing]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblWeighingTO> SelectAllTblWeighing()
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
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingTO> list = ConvertDTToList(reader);
                return list;
                //cmdSelect.Parameters.Add("@idWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblWeighingTO SelectTblWeighing(Int32 idWeighing)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);

            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idWeighing = " + idWeighing +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
                return null;
                //cmdSelect.Parameters.Add("@idWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;

            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblWeighingTO SelectTblWeighingByMachineIp(string ipAddr, DateTime timeStamp)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);

            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 1 * FROM tblWeighing WHERE machineIp = '" + ipAddr + "' " +
                                        //" AND timeStamp > " + timeStamp +
                                        " ORDER BY idWeighing Desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingTO> list = ConvertDTToList(reader);
                //cmdSelect.Parameters.Add("@idWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
                if(list != null && list.Count == 1)
                {
                    return list[0];
                }
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

        public  List<TblWeighingTO> SelectAllTblWeighing(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                
                
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public  int InsertTblWeighing(TblWeighingTO tblWeighingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblWeighingTO, cmdInsert);
            }
            catch(Exception ex)
            {                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblWeighingTO, cmdInsert);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblWeighingTO tblWeighingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblWeighing]( " + 
            //"  [idWeighing]" +
            " [timeStamp]" +
            " ,[measurement]" +
            " ,[machineIp]" +
            " )" +
" VALUES (" +
            //"  @IdWeighing " +
            " @TimeStamp " +
            " ,@Measurement " +
            " ,@MachineIp " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
            cmdInsert.Parameters.Add("@TimeStamp", System.Data.SqlDbType.DateTime).Value = tblWeighingTO.TimeStamp;
            cmdInsert.Parameters.Add("@Measurement", System.Data.SqlDbType.Decimal).Value = tblWeighingTO.Measurement;
            cmdInsert.Parameters.Add("@MachineIp", System.Data.SqlDbType.NVarChar).Value = tblWeighingTO.MachineIp;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblWeighing(TblWeighingTO tblWeighingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblWeighingTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblWeighing(TblWeighingTO tblWeighingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblWeighingTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblWeighingTO tblWeighingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblWeighing] SET " + 
            "  [idWeighing] = @IdWeighing" +
            " ,[timeStamp]= @TimeStamp" +
            " ,[measurement]= @Measurement" +
            " ,[machineIp] = @MachineIp" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
            cmdUpdate.Parameters.Add("@TimeStamp", System.Data.SqlDbType.DateTime).Value = tblWeighingTO.TimeStamp;
            cmdUpdate.Parameters.Add("@Measurement", System.Data.SqlDbType.Decimal).Value = tblWeighingTO.Measurement;
            cmdUpdate.Parameters.Add("@MachineIp", System.Data.SqlDbType.NVarChar).Value = tblWeighingTO.MachineIp;
            return cmdUpdate.ExecuteNonQuery();
        }

        public  List<TblWeighingTO> ConvertDTToList(SqlDataReader tblWeighingTODT)
        {
            List<TblWeighingTO> tblWeighingTOList = new List<TblWeighingTO>();
            if (tblWeighingTODT != null)
            {
                while(tblWeighingTODT.Read())
                {
                    TblWeighingTO tblWeighingTONew = new TblWeighingTO();
                    if (tblWeighingTODT["idWeighing"] != DBNull.Value)
                        tblWeighingTONew.IdWeighing = Convert.ToInt32(tblWeighingTODT["idWeighing"].ToString());
                    if (tblWeighingTODT["timeStamp"] != DBNull.Value)
                        tblWeighingTONew.TimeStamp = Convert.ToDateTime(tblWeighingTODT["timeStamp"].ToString());
                    if (tblWeighingTODT["measurement"] != DBNull.Value)
                        tblWeighingTONew.Measurement = Convert.ToString(tblWeighingTODT["measurement"].ToString());
                    if (tblWeighingTODT["machineIp"] != DBNull.Value)
                        tblWeighingTONew.MachineIp = Convert.ToString(tblWeighingTODT["machineIp"].ToString());
                    tblWeighingTOList.Add(tblWeighingTONew);
                }
            }
            return tblWeighingTOList;
        }
        #endregion

        #region Deletion
        public  int DeleteTblWeighing(Int32 idWeighing)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idWeighing, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblWeighingByByMachineIp(string ipAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommandByMachineIp(ipAddr, cmdDelete);
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
        public  int DeleteTblWeighing(Int32 idWeighing, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idWeighing, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int32 idWeighing, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblWeighing] " +
            " WHERE idWeighing = " + idWeighing +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
            return cmdDelete.ExecuteNonQuery();
        }
        public  int ExecuteDeletionCommandByMachineIp(string ipAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblWeighing] " +
            " WHERE machineIp = '" + ipAddr + "'";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeighing", System.Data.SqlDbType.Int).Value = tblWeighingTO.IdWeighing;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        //chetan[20-jan-2020] added for get data from IOT 
        public static List<TblMachineBackupTO> GetALLMachineData(int machinetype)
        {
            String sqlConnStr = Startup.IoTBackUpConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            var list = new List<TblMachineBackupTO>();
            //SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idmachineBackup,transactionId,machinedata, machinePortNumber, globleIP from tblMachineBackupRecycle where machinetype=" + machinetype;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader dr = cmdSelect.ExecuteReader();
                while (dr.Read())
                {
                    TblMachineBackupTO machineBackUpTo = new TblMachineBackupTO();
                    if (dr["idmachineBackup"] != DBNull.Value)
                        machineBackUpTo.IdMachineBackup = Convert.ToInt32(dr["idmachineBackup"].ToString());
                    if (dr["transactionId"] != DBNull.Value)
                        machineBackUpTo.TransactionId = Convert.ToInt32(dr["transactionId"].ToString());
                    if (dr["machinedata"] != DBNull.Value)
                        machineBackUpTo.Machinedata = (dr["machinedata"].ToString());
                    if (dr["machinePortNumber"] != DBNull.Value)
                        machineBackUpTo.MachinePortNumber = Convert.ToInt32(dr["machinePortNumber"].ToString());
                    if (dr["globleIP"] != DBNull.Value)
                        machineBackUpTo.GlobleIP = (dr["globleIP"].ToString());
                    list.Add(machineBackUpTo);

                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<String> SelectlayerWiseData(decimal transactionId, List<int> portList)
        {
            String sqlConnStr = Startup.IoTBackUpConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT machinedata,machinePortNumber FROM tblMachineBackupRecycle WHERE transactionId=" + transactionId + " AND machineType=2";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                dr = (cmdSelect.ExecuteReader());
                List<String> layerWiseData = new List<String>();
                while (dr.Read())
                {
                    layerWiseData.Add(Convert.ToString(dr["machinedata"].ToString()));
                    portList.Add(Convert.ToInt32(dr["machinePortNumber"].ToString()));
                }
                return layerWiseData;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
    }
}
