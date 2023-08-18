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
    public class TblWeighingMachineDAO : ITblWeighingMachineDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblWeighingMachineDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblWeighingMachine]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblWeighingMachineTO> SelectAllTblWeighingMachine()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMachineTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<DropDownTO> SelectTblWeighingMachineDropDownList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string sqlQuery = null;
            try
            {
                sqlQuery = "select idWeighingMachine, machineName, machineIP from tblWeighingMachine where isnull(isActive,0)=1";
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idWeighingMachine"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idWeighingMachine"].ToString());
                        if (reader["machineName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["machineName"].ToString());
                        if (reader["machineIP"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(reader["machineIP"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblWeighingMachineTO SelectTblWeighingMachine(Int32 idWeighingMachine)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idWeighingMachine = " + idWeighingMachine +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMachineTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
         public  TblWeighingMachineTO SelectTblWeighingMachine(string ipAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE machineIP = '" + ipAddr +"' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMachineTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblWeighingMachineTO> ConvertDTToList(SqlDataReader tblWeighingMachineTODT)
        {
            List<TblWeighingMachineTO> tblWeighingMachineTOList = new List<TblWeighingMachineTO>();
            if (tblWeighingMachineTODT != null)
            {
                while (tblWeighingMachineTODT.Read())
                {
                    TblWeighingMachineTO tblWeighingMachineTONew = new TblWeighingMachineTO();
                    if (tblWeighingMachineTODT["idWeighingMachine"] != DBNull.Value)
                        tblWeighingMachineTONew.IdWeighingMachine = Convert.ToInt32(tblWeighingMachineTODT["idWeighingMachine"].ToString());
                    if (tblWeighingMachineTODT["createdBy"] != DBNull.Value)
                        tblWeighingMachineTONew.CreatedBy = Convert.ToInt32(tblWeighingMachineTODT["createdBy"].ToString());
                    if (tblWeighingMachineTODT["updatedBy"] != DBNull.Value)
                        tblWeighingMachineTONew.UpdatedBy = Convert.ToInt32(tblWeighingMachineTODT["updatedBy"].ToString());
                    if (tblWeighingMachineTODT["createdOn"] != DBNull.Value)
                        tblWeighingMachineTONew.CreatedOn = Convert.ToDateTime(tblWeighingMachineTODT["createdOn"].ToString());
                    if (tblWeighingMachineTODT["updatedOn"] != DBNull.Value)
                        tblWeighingMachineTONew.UpdatedOn = Convert.ToDateTime(tblWeighingMachineTODT["updatedOn"].ToString());
                    if (tblWeighingMachineTODT["weighingCapMT"] != DBNull.Value)
                        tblWeighingMachineTONew.WeighingCapMT = Convert.ToDouble(tblWeighingMachineTODT["weighingCapMT"].ToString());
                    if (tblWeighingMachineTODT["machineName"] != DBNull.Value)
                        tblWeighingMachineTONew.MachineName = Convert.ToString(tblWeighingMachineTODT["machineName"].ToString());
                    if (tblWeighingMachineTODT["codeNumber"] != DBNull.Value)
                        tblWeighingMachineTONew.CodeNumber = Convert.ToString(tblWeighingMachineTODT["codeNumber"].ToString());
                    if (tblWeighingMachineTODT["machineDesc"] != DBNull.Value)
                        tblWeighingMachineTONew.MachineDesc = Convert.ToString(tblWeighingMachineTODT["machineDesc"].ToString());
                    if (tblWeighingMachineTODT["location"] != DBNull.Value)
                        tblWeighingMachineTONew.Location = Convert.ToString(tblWeighingMachineTODT["location"].ToString());
                    if (tblWeighingMachineTODT["deviceId"] != DBNull.Value)
                        tblWeighingMachineTONew.DeviceId = Convert.ToString(tblWeighingMachineTODT["deviceId"].ToString());
                    if (tblWeighingMachineTODT["machineIP"] != DBNull.Value)
                        tblWeighingMachineTONew.MachineIP = Convert.ToString(tblWeighingMachineTODT["machineIP"].ToString());
                    if (tblWeighingMachineTODT["portNumber"] != DBNull.Value)
                        tblWeighingMachineTONew.PortNumber = Convert.ToString(tblWeighingMachineTODT["portNumber"].ToString());
                    if (tblWeighingMachineTODT["IoTUrl"] != DBNull.Value)
                        tblWeighingMachineTONew.IoTUrl = Convert.ToString(tblWeighingMachineTODT["IoTUrl"].ToString());
                    if (tblWeighingMachineTODT["isActive"] != DBNull.Value)
                        tblWeighingMachineTONew.IsActive = Convert.ToInt32(tblWeighingMachineTODT["isActive"].ToString());
                    if (tblWeighingMachineTODT["userIds"] != DBNull.Value)
                        tblWeighingMachineTONew.UserIds = Convert.ToString(tblWeighingMachineTODT["userIds"].ToString());
                    tblWeighingMachineTOList.Add(tblWeighingMachineTONew);
                }
            }
            return tblWeighingMachineTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblWeighingMachineTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblWeighingMachineTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblWeighingMachineTO tblWeighingMachineTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblWeighingMachine]( " + 
                                "  [createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[weighingCapMT]" +
                                " ,[machineName]" +
                                " ,[codeNumber]" +
                                " ,[machineDesc]" +
                                " ,[location]" +
                                " ,[deviceId]" +
                                " ,[machineIP]" +
                                " )" +
                    " VALUES (" +
                                "  @CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@WeighingCapMT " +
                                " ,@MachineName " +
                                " ,@CodeNumber " +
                                " ,@MachineDesc " +
                                " ,@Location " +
                                " ,@DeviceId " +
                                " ,@MachineIP " + 
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdWeighingMachine", System.Data.SqlDbType.Int).Value = tblWeighingMachineTO.IdWeighingMachine;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblWeighingMachineTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblWeighingMachineTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue( tblWeighingMachineTO.UpdatedOn);
            cmdInsert.Parameters.Add("@WeighingCapMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.WeighingCapMT);
            cmdInsert.Parameters.Add("@MachineName", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.MachineName;
            cmdInsert.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.CodeNumber;
            cmdInsert.Parameters.Add("@MachineDesc", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.MachineDesc;
            cmdInsert.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.Location);
            cmdInsert.Parameters.Add("@DeviceId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.DeviceId);
            cmdInsert.Parameters.Add("@MachineIP", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.MachineIP);
            if( cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblWeighingMachineTO.IdWeighingMachine = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblWeighingMachineTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblWeighingMachine(TblWeighingMachineTO tblWeighingMachineTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblWeighingMachineTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblWeighingMachineTO tblWeighingMachineTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblWeighingMachine] SET " + 
                            "  [updatedBy]= @UpdatedBy" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[weighingCapMT]= @WeighingCapMT" +
                            " ,[machineName]= @MachineName" +
                            " ,[codeNumber]= @CodeNumber" +
                            " ,[machineDesc]= @MachineDesc" +
                            " ,[location]= @Location" +
                            " ,[deviceId]= @DeviceId" +
                            " ,[machineIP] = @MachineIP" +
                            " WHERE [idWeighingMachine] = @IdWeighingMachine"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdWeighingMachine", System.Data.SqlDbType.Int).Value = tblWeighingMachineTO.IdWeighingMachine;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblWeighingMachineTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblWeighingMachineTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@WeighingCapMT", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.WeighingCapMT);
            cmdUpdate.Parameters.Add("@MachineName", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.MachineName;
            cmdUpdate.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.CodeNumber;
            cmdUpdate.Parameters.Add("@MachineDesc", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.MachineDesc;
            cmdUpdate.Parameters.Add("@Location", System.Data.SqlDbType.NVarChar).Value = tblWeighingMachineTO.Location;
            cmdUpdate.Parameters.Add("@DeviceId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.DeviceId);
            cmdUpdate.Parameters.Add("@MachineIP", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblWeighingMachineTO.MachineIP);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblWeighingMachine(Int32 idWeighingMachine)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idWeighingMachine, cmdDelete);
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

        public  int DeleteTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idWeighingMachine, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idWeighingMachine, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblWeighingMachine] " +
            " WHERE idWeighingMachine = " + idWeighingMachine +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeighingMachine", System.Data.SqlDbType.Int).Value = tblWeighingMachineTO.IdWeighingMachine;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public TblWeighingMachineTO SelectTblWeighingMachine(Int32 idWeighingMachine, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idWeighingMachine = " + idWeighingMachine + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWeighingMachineTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
    }
}
