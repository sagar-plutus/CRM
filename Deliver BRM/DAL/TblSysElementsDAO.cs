using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI.DAL
{
    public class TblSysElementsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT sysElement.* , " +
                                  "CASE WHEN sysElement.menuId IS NULL THEN  CASE WHEN pgElement.pageEleTypeId is null THEN module.moduleName " +
                                  "ELSE pgElement.elementDisplayName END ELSE menus.menuName END AS elementName," +
                                  " CASE WHEN sysElement.menuId IS NULL THEN CASE WHEN pgElement.pageEleTypeId is null THEN module.moduleDesc " +
                                  "ELSE pgElement.elementDesc END ELSE menus.menuDesc END AS elementDesc " +
                                  " FROM tblSysElements sysElement " +
                                  " LEFT JOIN tblPageElements pgElement ON sysElement.pageElementId = pgElement.idPageElement " +
                                  " LEFT JOIN tblMenuStructure menus ON sysElement.menuId = menus.idMenu " +
                                  " LEFT JOIN tblModule module ON sysElement.moduleId = module.idModule ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblSysElementsTO> SelectAllTblSysElements(int menuPageId,string type,int moduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (menuPageId == 0)
                {
                    if(menuPageId == 0 && moduleId != 0)
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE sysElement.type=" + "'" + type + "' AND menus.moduleId = " + moduleId;
                    else
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE sysElement.type=" + "'" + type + "'";
                }  
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE pgElement.pageId=" + menuPageId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #region user subscription
// user Tracking
 public static int SelectIsImportantPerson(int userId,int sysEleID)
        {
            int impPerson=0;
            String sqlConnStr =  Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
               
               cmdSelect.CommandText = "select isImpPerson from tblSysEleUserEntitlements where sysEleId="+sysEleID+" and userId="+userId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while(rdr.Read())
                {
                    impPerson=Convert.ToInt32(rdr["isImpPerson"].ToString());
                }
                
              
                return impPerson;
            }
            catch(Exception ex)
            {
                return impPerson;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
//END
#endregion

        public static TblSysElementsTO SelectTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSysElement = " + idSysElement +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToList(rdr);
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
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSysElementsTO> ConvertDTToList(SqlDataReader tblSysElementsTODT)
        {
            List<TblSysElementsTO> tblSysElementsTOList = new List<TblSysElementsTO>();
            if (tblSysElementsTODT != null)
            {
                while (tblSysElementsTODT.Read())
                {
                    TblSysElementsTO tblSysElementsTONew = new TblSysElementsTO();
                    if (tblSysElementsTODT["idSysElement"] != DBNull.Value)
                        tblSysElementsTONew.IdSysElement = Convert.ToInt32(tblSysElementsTODT["idSysElement"].ToString());
                    if (tblSysElementsTODT["pageElementId"] != DBNull.Value)
                        tblSysElementsTONew.PageElementId = Convert.ToInt32(tblSysElementsTODT["pageElementId"].ToString());
                    if (tblSysElementsTODT["menuId"] != DBNull.Value)
                        tblSysElementsTONew.MenuId = Convert.ToInt32(tblSysElementsTODT["menuId"].ToString());
                    if (tblSysElementsTODT["type"] != DBNull.Value)
                        tblSysElementsTONew.Type = Convert.ToString(tblSysElementsTODT["type"].ToString());
                    if (tblSysElementsTODT["elementName"] != DBNull.Value)
                        tblSysElementsTONew.ElementName = Convert.ToString(tblSysElementsTODT["elementName"].ToString());
                    if (tblSysElementsTODT["elementDesc"] != DBNull.Value)
                        tblSysElementsTONew.ElementDesc = Convert.ToString(tblSysElementsTODT["elementDesc"].ToString());

                    tblSysElementsTOList.Add(tblSysElementsTONew);
                }
            }
            return tblSysElementsTOList;
        }

        public static List<TblSysElementsTO> SelectTblSysElementsByModulId(Int32 moduleId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM tblSysElements WHERE isnull(moduleId,1) = " + moduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSysElementsTO> list = ConvertDTToListbyModuleId(rdr);
                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }



        public static List<TblSysElementsTO> ConvertDTToListbyModuleId(SqlDataReader tblSysElementsTODT)
        {
            List<TblSysElementsTO> tblSysElementsTOList = new List<TblSysElementsTO>();
            if (tblSysElementsTODT != null)
            {
                while (tblSysElementsTODT.Read())
                {
                    TblSysElementsTO tblSysElementsTONew = new TblSysElementsTO();
                    if (tblSysElementsTODT["idSysElement"] != DBNull.Value)
                        tblSysElementsTONew.IdSysElement = Convert.ToInt32(tblSysElementsTODT["idSysElement"].ToString());
                    if (tblSysElementsTODT["pageElementId"] != DBNull.Value)
                        tblSysElementsTONew.PageElementId = Convert.ToInt32(tblSysElementsTODT["pageElementId"].ToString());
                    if (tblSysElementsTODT["menuId"] != DBNull.Value)
                        tblSysElementsTONew.MenuId = Convert.ToInt32(tblSysElementsTODT["menuId"].ToString());
                    if (tblSysElementsTODT["type"] != DBNull.Value)
                        tblSysElementsTONew.Type = Convert.ToString(tblSysElementsTODT["type"].ToString());
                    if (tblSysElementsTODT["moduleId"] != DBNull.Value)
                        tblSysElementsTONew.ModuleId = Convert.ToInt32(tblSysElementsTODT["moduleId"].ToString());
                    if (tblSysElementsTODT["basicModeApplicable"] != DBNull.Value)
                        tblSysElementsTONew.BasicModeApplicable = Convert.ToInt32(tblSysElementsTODT["basicModeApplicable"].ToString());

                    tblSysElementsTOList.Add(tblSysElementsTONew);
                }
            }
            return tblSysElementsTOList;
        }


        #endregion

        #region Insertion
        public static int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSysElementsTO, cmdInsert);
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

        public static int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSysElementsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSysElements]( " + 
                            "  [idSysElement]" +
                            " ,[pageElementId]" +
                            " ,[menuId]" +
                            " ,[type]" +
                            " )" +
                " VALUES (" +
                            "  @IdSysElement " +
                            " ,@PageElementId " +
                            " ,@MenuId " +
                            " ,@Type " + 
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            cmdInsert.Parameters.Add("@PageElementId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.PageElementId;
            cmdInsert.Parameters.Add("@MenuId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.MenuId;
            cmdInsert.Parameters.Add("@Type", System.Data.SqlDbType.Char).Value = tblSysElementsTO.Type;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSysElementsTO, cmdUpdate);
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

        public static int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSysElementsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSysElements] SET " + 
            "  [idSysElement] = @IdSysElement" +
            " ,[pageElementId]= @PageElementId" +
            " ,[menuId]= @MenuId" +
            " ,[type] = @Type" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            cmdUpdate.Parameters.Add("@PageElementId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.PageElementId;
            cmdUpdate.Parameters.Add("@MenuId", System.Data.SqlDbType.Int).Value = tblSysElementsTO.MenuId;
            cmdUpdate.Parameters.Add("@Type", System.Data.SqlDbType.Char).Value = tblSysElementsTO.Type;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSysElement, cmdDelete);
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

        public static int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSysElement, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSysElement, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSysElements] " +
            " WHERE idSysElement = " + idSysElement +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSysElement", System.Data.SqlDbType.Int).Value = tblSysElementsTO.IdSysElement;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
