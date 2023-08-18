using System;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using System.Collections.Generic;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblGateDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGate] tblGate"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblGateTO> SelectAllTblGate(Constants.ActiveSelectionTypeE ActiveSelectionTypeE)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();

                if (ActiveSelectionTypeE == Constants.ActiveSelectionTypeE.Active)
                {
                    cmdSelect.CommandText += " WHERE ISNULL(tblGate.isActive,0) = 1 AND moduleId = " + StaticStuff.Constants.DefaultModuleID;
                }
                else if (ActiveSelectionTypeE == Constants.ActiveSelectionTypeE.NonActive)
                {
                    cmdSelect.CommandText += " WHERE ISNULL(tblGate.isActive,0) = 0 AND moduleId = " + StaticStuff.Constants.DefaultModuleID; 
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGate", System.Data.SqlDbType.Int).Value = tblGateTO.IdGate;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
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

        public static TblGateTO SelectTblGate(Int32 idGate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGate = " + idGate +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGateTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public static List<TblGateTO> SelectAllTblGate(Constants.ActiveSelectionTypeE ActiveSelectionTypeE, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery();

                if (ActiveSelectionTypeE == Constants.ActiveSelectionTypeE.Active)
                {
                    cmdSelect.CommandText += " WHERE ISNULL(tblGate.isActive,0) = 1 AND moduleId = " + StaticStuff.Constants.DefaultModuleID;
                }
                else if (ActiveSelectionTypeE == Constants.ActiveSelectionTypeE.NonActive)
                {
                    cmdSelect.CommandText += " WHERE ISNULL(tblGate.isActive,0) = 0 AND moduleId = " + StaticStuff.Constants.DefaultModuleID;
                }


                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblGateTO> ConvertDTToList(SqlDataReader tblGateTODT)
        {
            List<TblGateTO> tblGateTOList = new List<TblGateTO>();
            if (tblGateTODT != null)
            {
                while (tblGateTODT.Read())
                {
                    TblGateTO tblGateTONew = new TblGateTO();
                    if (tblGateTODT["idGate"] != DBNull.Value)
                        tblGateTONew.IdGate = Convert.ToInt32(tblGateTODT["idGate"].ToString());
                    if (tblGateTODT["isActive"] != DBNull.Value)
                        tblGateTONew.IsActive = Convert.ToInt32(tblGateTODT["isActive"].ToString());
                    if (tblGateTODT["isDefault"] != DBNull.Value)
                        tblGateTONew.IsDefault = Convert.ToInt32(tblGateTODT["isDefault"].ToString());
                    if (tblGateTODT["createdBy"] != DBNull.Value)
                        tblGateTONew.CreatedBy = Convert.ToInt32(tblGateTODT["createdBy"].ToString());
                    if (tblGateTODT["updatedBy"] != DBNull.Value)
                        tblGateTONew.UpdatedBy = Convert.ToInt32(tblGateTODT["updatedBy"].ToString());
                    if (tblGateTODT["createdOn"] != DBNull.Value)
                        tblGateTONew.CreatedOn = Convert.ToDateTime(tblGateTODT["createdOn"].ToString());
                    if (tblGateTODT["updatedOn"] != DBNull.Value)
                        tblGateTONew.UpdatedOn = Convert.ToDateTime(tblGateTODT["updatedOn"].ToString());
                    if (tblGateTODT["gateName"] != DBNull.Value)
                        tblGateTONew.GateName = Convert.ToString(tblGateTODT["gateName"].ToString());
                    if (tblGateTODT["gateDesc"] != DBNull.Value)
                        tblGateTONew.GateDesc = Convert.ToString(tblGateTODT["gateDesc"].ToString());
                    if (tblGateTODT["portNumber"] != DBNull.Value)
                        tblGateTONew.PortNumber = Convert.ToString(tblGateTODT["portNumber"].ToString());
                    if (tblGateTODT["ioTUrl"] != DBNull.Value)
                        tblGateTONew.IoTUrl = Convert.ToString(tblGateTODT["ioTUrl"].ToString());
                    if (tblGateTODT["machineIP"] != DBNull.Value)
                        tblGateTONew.MachineIP = Convert.ToString(tblGateTODT["machineIP"].ToString());
                    if (tblGateTODT["moduleId"] != DBNull.Value)
                        tblGateTONew.ModuleId = Convert.ToInt32(tblGateTODT["moduleId"].ToString());
                    tblGateTOList.Add(tblGateTONew);
                }
            }
            return tblGateTOList;
        }



        #endregion

        #region Insertion
        public static int InsertTblGate(TblGateTO tblGateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGateTO, cmdInsert);
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

        public static int InsertTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGateTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblGateTO tblGateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGate]( " + 
            "  [idGate]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[gateName]" +
            " ,[gateDesc]" +
            " ,[portNumber]" +
            " ,[ioTUrl]" +
            " ,[isDefault]" +
            " ,[machineIP]" +
            " )" +
" VALUES (" +
            "  @IdGate " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@GateName " +
            " ,@GateDesc " +
            " ,@PortNumber " +
            " ,@IoTUrl " +
            " ,@IsDefault " +
            " ,@MachineIP " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdGate", System.Data.SqlDbType.Int).Value = tblGateTO.IdGate;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGateTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGateTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGateTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGateTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGateTO.UpdatedOn;
            cmdInsert.Parameters.Add("@GateName", System.Data.SqlDbType.NVarChar).Value = tblGateTO.GateName;
            cmdInsert.Parameters.Add("@GateDesc", System.Data.SqlDbType.NVarChar).Value = tblGateTO.GateDesc;
            cmdInsert.Parameters.Add("@PortNumber", System.Data.SqlDbType.NVarChar).Value = tblGateTO.PortNumber;
            cmdInsert.Parameters.Add("@IoTUrl", System.Data.SqlDbType.NVarChar).Value = tblGateTO.IoTUrl;
            cmdInsert.Parameters.Add("@MachineIP", System.Data.SqlDbType.NVarChar).Value = tblGateTO.MachineIP;
            cmdInsert.Parameters.Add("@IsDefault", System.Data.SqlDbType.NVarChar).Value = tblGateTO.IsDefault;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblGate(TblGateTO tblGateTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGateTO, cmdUpdate);
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

        public static int UpdateTblGate(TblGateTO tblGateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGateTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblGateTO tblGateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGate] SET " + 
            "  [idGate] = @IdGate" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[gateName]= @GateName" +
            " ,[gateDesc]= @GateDesc" +
            " ,[portNumber]= @PortNumber" +
            " ,[ioTUrl] = @IoTUrl" +
            " ,[isDefault] = @IsDefault" +
            " ,[machineIP] = @MachineIP" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGate", System.Data.SqlDbType.Int).Value = tblGateTO.IdGate;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGateTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGateTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGateTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGateTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGateTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@GateName", System.Data.SqlDbType.NVarChar).Value = tblGateTO.GateName;
            cmdUpdate.Parameters.Add("@GateDesc", System.Data.SqlDbType.NVarChar).Value = tblGateTO.GateDesc;
            cmdUpdate.Parameters.Add("@PortNumber", System.Data.SqlDbType.NVarChar).Value = tblGateTO.PortNumber;
            cmdUpdate.Parameters.Add("@IoTUrl", System.Data.SqlDbType.NVarChar).Value = tblGateTO.IoTUrl;
            cmdUpdate.Parameters.Add("@IsDefault", System.Data.SqlDbType.NVarChar).Value = tblGateTO.IsDefault;
            cmdUpdate.Parameters.Add("@MachineIP", System.Data.SqlDbType.NVarChar).Value = tblGateTO.MachineIP;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblGate(Int32 idGate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGate, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblGate(Int32 idGate, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGate, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idGate, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGate] " +
            " WHERE idGate = " + idGate +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGate", System.Data.SqlDbType.Int).Value = tblGateTO.IdGate;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
