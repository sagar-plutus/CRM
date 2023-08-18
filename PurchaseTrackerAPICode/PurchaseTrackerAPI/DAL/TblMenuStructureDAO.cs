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
    public class TblMenuStructureDAO : ITblMenuStructureDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblMenuStructureDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT menus.*,idSysElement FROM [tblMenuStructure] menus" +
                                  " LEFT JOIN tblSysElements sysElements ON sysElements.menuId=menus.idMenu";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblMenuStructureTO> SelectAllTblMenuStructure()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMenuStructureTO> list = ConvertDTToList(rdr);
                return list;
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

        public  TblMenuStructureTO SelectTblMenuStructure(Int32 idMenu)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idMenu = " + idMenu +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblMenuStructureTO> list = ConvertDTToList(rdr);
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

        public  List<TblMenuStructureTO> ConvertDTToList(SqlDataReader tblMenuStructureTODT)
        {
            List<TblMenuStructureTO> tblMenuStructureTOList = new List<TblMenuStructureTO>();
            if (tblMenuStructureTODT != null)
            {
                while (tblMenuStructureTODT.Read())
                {
                    TblMenuStructureTO tblMenuStructureTONew = new TblMenuStructureTO();
                    if (tblMenuStructureTODT["idMenu"] != DBNull.Value)
                        tblMenuStructureTONew.IdMenu = Convert.ToInt32(tblMenuStructureTODT["idMenu"].ToString());
                    if (tblMenuStructureTODT["parentMenuId"] != DBNull.Value)
                        tblMenuStructureTONew.ParentMenuId = Convert.ToInt32(tblMenuStructureTODT["parentMenuId"].ToString());
                    if (tblMenuStructureTODT["moduleId"] != DBNull.Value)
                        tblMenuStructureTONew.ModuleId = Convert.ToInt32(tblMenuStructureTODT["moduleId"].ToString());
                    if (tblMenuStructureTODT["serNo"] != DBNull.Value)
                        tblMenuStructureTONew.SerNo = Convert.ToInt32(tblMenuStructureTODT["serNo"].ToString());
                    if (tblMenuStructureTODT["createdOn"] != DBNull.Value)
                        tblMenuStructureTONew.CreatedOn = Convert.ToDateTime(tblMenuStructureTODT["createdOn"].ToString());
                    if (tblMenuStructureTODT["menuName"] != DBNull.Value)
                        tblMenuStructureTONew.MenuName = Convert.ToString(tblMenuStructureTODT["menuName"].ToString());
                    if (tblMenuStructureTODT["menuDesc"] != DBNull.Value)
                        tblMenuStructureTONew.MenuDesc = Convert.ToString(tblMenuStructureTODT["menuDesc"].ToString());
                    if (tblMenuStructureTODT["menuAction"] != DBNull.Value)
                        tblMenuStructureTONew.MenuAction = Convert.ToString(tblMenuStructureTODT["menuAction"].ToString());
                    if (tblMenuStructureTODT["menuItemIcon"] != DBNull.Value)
                        tblMenuStructureTONew.MenuItemIcon = Convert.ToString(tblMenuStructureTODT["menuItemIcon"].ToString());
                    if (tblMenuStructureTODT["menuShortCut"] != DBNull.Value)
                        tblMenuStructureTONew.MenuShortCut = Convert.ToString(tblMenuStructureTODT["menuShortCut"].ToString());
                    if (tblMenuStructureTODT["idSysElement"] != DBNull.Value)
                        tblMenuStructureTONew.SysElementId = Convert.ToInt32(tblMenuStructureTODT["idSysElement"].ToString());
                    tblMenuStructureTOList.Add(tblMenuStructureTONew);
                }
            }
            return tblMenuStructureTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblMenuStructureTO, cmdInsert);
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

        public  int InsertTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblMenuStructureTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblMenuStructureTO tblMenuStructureTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblMenuStructure]( " + 
            "  [idMenu]" +
            " ,[parentMenuId]" +
            " ,[moduleId]" +
            " ,[serNo]" +
            " ,[createdOn]" +
            " ,[menuName]" +
            " ,[menuDesc]" +
            " ,[menuAction]" +
            " ,[menuItemIcon]" +
            " ,[menuShortCut]" +
            " )" +
" VALUES (" +
            "  @IdMenu " +
            " ,@ParentMenuId " +
            " ,@ModuleId " +
            " ,@SerNo " +
            " ,@CreatedOn " +
            " ,@MenuName " +
            " ,@MenuDesc " +
            " ,@MenuAction " +
            " ,@MenuItemIcon " +
            " ,@MenuShortCut " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdMenu", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.IdMenu;
            cmdInsert.Parameters.Add("@ParentMenuId", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.ParentMenuId;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.ModuleId;
            cmdInsert.Parameters.Add("@SerNo", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.SerNo;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblMenuStructureTO.CreatedOn;
            cmdInsert.Parameters.Add("@MenuName", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuName;
            cmdInsert.Parameters.Add("@MenuDesc", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuDesc;
            cmdInsert.Parameters.Add("@MenuAction", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuAction;
            cmdInsert.Parameters.Add("@MenuItemIcon", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuItemIcon;
            cmdInsert.Parameters.Add("@MenuShortCut", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuShortCut;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblMenuStructureTO, cmdUpdate);
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

        public  int UpdateTblMenuStructure(TblMenuStructureTO tblMenuStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblMenuStructureTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblMenuStructureTO tblMenuStructureTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblMenuStructure] SET " + 
            "  [idMenu] = @IdMenu" +
            " ,[parentMenuId]= @ParentMenuId" +
            " ,[moduleId]= @ModuleId" +
            " ,[serNo]= @SerNo" +
            " ,[createdOn]= @CreatedOn" +
            " ,[menuName]= @MenuName" +
            " ,[menuDesc]= @MenuDesc" +
            " ,[menuAction]= @MenuAction" +
            " ,[menuItemIcon]= @MenuItemIcon" +
            " ,[menuShortCut] = @MenuShortCut" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdMenu", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.IdMenu;
            cmdUpdate.Parameters.Add("@ParentMenuId", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.ParentMenuId;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.ModuleId;
            cmdUpdate.Parameters.Add("@SerNo", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.SerNo;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblMenuStructureTO.CreatedOn;
            cmdUpdate.Parameters.Add("@MenuName", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuName;
            cmdUpdate.Parameters.Add("@MenuDesc", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuDesc;
            cmdUpdate.Parameters.Add("@MenuAction", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuAction;
            cmdUpdate.Parameters.Add("@MenuItemIcon", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuItemIcon;
            cmdUpdate.Parameters.Add("@MenuShortCut", System.Data.SqlDbType.NVarChar).Value = tblMenuStructureTO.MenuShortCut;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblMenuStructure(Int32 idMenu)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idMenu, cmdDelete);
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

        public  int DeleteTblMenuStructure(Int32 idMenu, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idMenu, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idMenu, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblMenuStructure] " +
            " WHERE idMenu = " + idMenu +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idMenu", System.Data.SqlDbType.Int).Value = tblMenuStructureTO.IdMenu;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
