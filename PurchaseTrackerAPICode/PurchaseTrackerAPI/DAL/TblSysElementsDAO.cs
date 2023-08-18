using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblSysElementsDAO : ITblSysElementsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblSysElementsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT sysElement.* , " +
                                  " CASE WHEN sysElement.menuId IS NULL THEN pgElement.elementDisplayName " +
                                  " ELSE menus.menuName END AS elementName, " +
                                  " CASE WHEN sysElement.menuId IS NULL THEN pgElement.elementDesc " +
                                  " ELSE menus.menuDesc END AS elementDesc " +
                                  " FROM tblSysElements sysElement " +
                                  " LEFT JOIN tblPageElements pgElement ON sysElement.pageElementId = pgElement.idPageElement " +
                                  " LEFT JOIN tblMenuStructure menus ON sysElement.menuId = menus.idMenu ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblSysElementsTO> SelectAllTblSysElements(int menuPageId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (menuPageId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE sysElement.type='MI' ";
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

        public  TblSysElementsTO SelectTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  List<TblSysElementsTO> ConvertDTToList(SqlDataReader tblSysElementsTODT)
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

        #endregion

        #region Insertion
        public  int InsertTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteInsertionCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdInsert)
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
        public  int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteUpdationCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdUpdate)
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
        public  int DeleteTblSysElements(Int32 idSysElement)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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

        public  int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran)
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

        public  int ExecuteDeletionCommand(Int32 idSysElement, SqlCommand cmdDelete)
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
