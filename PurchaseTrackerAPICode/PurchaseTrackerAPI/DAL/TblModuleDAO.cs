using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
   public class TblModuleDAO : ITblModuleDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblModuleDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery() 
        {
            String sqlSelectQry = "SELECT module.*,idSysElement FROM [tblModule] module LEFT JOIN tblSysElements sysElements ON sysElements.moduleId = module.idModule AND sysElements.type = 'M' "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<DropDownTO> SelectAllTblModule()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                // List<TblModuleTO> list = ConvertDTToList(rdr);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idModule"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idModule"].ToString());
                    if (dateReader["moduleName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["moduleName"].ToString());
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
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblModuleTO> SelectTblModuleList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader dateReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(dateReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblModuleTO SelectTblModule(Int32 idModule)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idModule = " + idModule +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public  TblModuleTO SelectTblModule(Int32 idModule, SqlConnection conn, SqlTransaction transaction)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idModule = " + idModule + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = transaction;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModuleTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public  List<TblModuleTO> ConvertDTToList(SqlDataReader tblModuleTODT)
        {
            List<TblModuleTO> tblModuleTOList = new List<TblModuleTO>();
            if (tblModuleTODT != null)
            {
                while (tblModuleTODT.Read())
                {
                    TblModuleTO tblModuleTONew = new TblModuleTO();
                    if (tblModuleTODT["idModule"] != DBNull.Value)
                        tblModuleTONew.IdModule = Convert.ToInt32(tblModuleTODT["idModule"].ToString());
                    if (tblModuleTODT["createdOn"] != DBNull.Value)
                        tblModuleTONew.CreatedOn = Convert.ToDateTime(tblModuleTODT["createdOn"].ToString());
                    if (tblModuleTODT["moduleName"] != DBNull.Value)
                        tblModuleTONew.ModuleName = Convert.ToString(tblModuleTODT["moduleName"].ToString());
                    if (tblModuleTODT["moduleDesc"] != DBNull.Value)
                        tblModuleTONew.ModuleDesc = Convert.ToString(tblModuleTODT["moduleDesc"].ToString());
                    if (tblModuleTODT["navigateUrl"] != DBNull.Value)
                       tblModuleTONew.NavigateUrl = Convert.ToString(tblModuleTODT["navigateUrl"].ToString());
                    if (tblModuleTODT["isActive"] != DBNull.Value)
                        tblModuleTONew.IsActive = Convert.ToInt16(tblModuleTODT["isActive"].ToString());
                    if (tblModuleTODT["logoUrl"] != DBNull.Value)
                       tblModuleTONew.LogoUrl = Convert.ToString(tblModuleTODT["logoUrl"].ToString());
                    if (tblModuleTODT["idSysElement"] != DBNull.Value)
                      tblModuleTONew.SysElementId = Convert.ToInt32(tblModuleTODT["idSysElement"].ToString());
                    if (tblModuleTODT["androidUrl"] != DBNull.Value)
                        tblModuleTONew.AndroidUrl = Convert.ToString(tblModuleTODT["androidUrl"].ToString());
                    if (tblModuleTODT["isSubscribe"] != DBNull.Value)
                        tblModuleTONew.IsSubscribe = Convert.ToInt16(tblModuleTODT["isSubscribe"].ToString());
                    if (tblModuleTODT["containerName"] != DBNull.Value)
                        tblModuleTONew.ContainerName = Convert.ToString(tblModuleTODT["containerName"].ToString());
                    if (tblModuleTODT["isExternal"] != DBNull.Value)
                        tblModuleTONew.IsExternal = Convert.ToInt16(tblModuleTODT["isExternal"].ToString());

                    if (tblModuleTODT["iotconfigSetting"] != DBNull.Value)
                        tblModuleTONew.IotconfigSetting = Convert.ToInt16(tblModuleTODT["iotconfigSetting"].ToString());

                    if (tblModuleTODT["isSAPEnabled"] != DBNull.Value)
                        tblModuleTONew.IsSAPEnabled = Convert.ToInt16(tblModuleTODT["isSAPEnabled"].ToString());


                    tblModuleTOList.Add(tblModuleTONew);
                }
            }
            return tblModuleTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblModule(TblModuleTO tblModuleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblModuleTO, cmdInsert);
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

        public  int InsertTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblModuleTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblModuleTO tblModuleTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblModule]( " + 
            "  [idModule]" +
            " ,[createdOn]" +
            " ,[moduleName]" +
            " ,[moduleDesc]" +
            " ,[androidUrl ]"+
            " ,[isSubscribe] "+
            " ,[containerName]" +
            " )" +
" VALUES (" +
            "  @IdModule " +
            " ,@CreatedOn " +
            " ,@ModuleName " +
            " ,@ModuleDesc " +
            " ,@AndroidUrl " +
            " ,@IsSubscribe " +
            " ,@containerName" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleTO.CreatedOn;
            cmdInsert.Parameters.Add("@ModuleName", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleName;
            cmdInsert.Parameters.Add("@ModuleDesc", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleDesc;
            cmdInsert.Parameters.Add("@AndroidUrl", System.Data.SqlDbType.VarChar).Value = tblModuleTO.AndroidUrl;
            cmdInsert.Parameters.Add("@IsSubscribe", System.Data.SqlDbType.Int).Value = tblModuleTO.IsSubscribe;
            cmdInsert.Parameters.Add("@containerName", System.Data.SqlDbType.NVarChar).Value = tblModuleTO.ContainerName; //Sudhir[11-OCT-2018] Added for Azure Container Name.
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblModule(TblModuleTO tblModuleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblModuleTO, cmdUpdate);
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

        public  int UpdateTblModule(TblModuleTO tblModuleTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblModuleTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblModuleTO tblModuleTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblModule] SET " + 
            "  [idModule] = @IdModule" +
            " ,[createdOn]= @CreatedOn" +
            " ,[moduleName]= @ModuleName" +
            " ,[moduleDesc] = @ModuleDesc" +
            " ,[isSubscribe]=@IsSubscribe "+
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblModuleTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ModuleName", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleName;
            cmdUpdate.Parameters.Add("@ModuleDesc", System.Data.SqlDbType.VarChar).Value = tblModuleTO.ModuleDesc;
            cmdUpdate.Parameters.Add("@IsSubscribe", System.Data.SqlDbType.Int).Value = tblModuleTO.IsSubscribe;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblModule(Int32 idModule)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idModule, cmdDelete);
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

        public  int DeleteTblModule(Int32 idModule, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idModule, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idModule, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblModule] " +
            " WHERE idModule = " + idModule +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idModule", System.Data.SqlDbType.Int).Value = tblModuleTO.IdModule;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
