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
    public class TblPageElementsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPageElements]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblPageElementsTO> SelectAllTblPageElements()
        {
            String sqlConnStr = Startup.ConnectionString;
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
                List<TblPageElementsTO> list = ConvertDTToList(rdr);
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

        public static List<TblPageElementsTO> SelectAllTblPageElements(int pageId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (pageId == 0)
                    cmdSelect.CommandText = SqlSelectQuery();
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE pageId=" + pageId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPageElementsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
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

        public static TblPageElementsTO SelectTblPageElements(Int32 idPageElement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPageElement = " + idPageElement +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPageElementsTO> list = ConvertDTToList(rdr);
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

        public static List<TblPageElementsTO> ConvertDTToList(SqlDataReader tblPageElementsTODT)
        {
            List<TblPageElementsTO> tblPageElementsTOList = new List<TblPageElementsTO>();
            if (tblPageElementsTODT != null)
            {
                while (tblPageElementsTODT.Read())
                {
                    TblPageElementsTO tblPageElementsTONew = new TblPageElementsTO();
                    if (tblPageElementsTODT["idPageElement"] != DBNull.Value)
                        tblPageElementsTONew.IdPageElement = Convert.ToInt32(tblPageElementsTODT["idPageElement"].ToString());
                    if (tblPageElementsTODT["pageId"] != DBNull.Value)
                        tblPageElementsTONew.PageId = Convert.ToInt32(tblPageElementsTODT["pageId"].ToString());
                    if (tblPageElementsTODT["pageEleTypeId"] != DBNull.Value)
                        tblPageElementsTONew.PageEleTypeId = Convert.ToInt32(tblPageElementsTODT["pageEleTypeId"].ToString());
                    if (tblPageElementsTODT["createdOn"] != DBNull.Value)
                        tblPageElementsTONew.CreatedOn = Convert.ToDateTime(tblPageElementsTODT["createdOn"].ToString());
                    if (tblPageElementsTODT["elementName"] != DBNull.Value)
                        tblPageElementsTONew.ElementName = Convert.ToString(tblPageElementsTODT["elementName"].ToString());
                    if (tblPageElementsTODT["elementDesc"] != DBNull.Value)
                        tblPageElementsTONew.ElementDesc = Convert.ToString(tblPageElementsTODT["elementDesc"].ToString());
                    if (tblPageElementsTODT["elementDisplayName"] != DBNull.Value)
                        tblPageElementsTONew.ElementDisplayName = Convert.ToString(tblPageElementsTODT["elementDisplayName"].ToString());
                    tblPageElementsTOList.Add(tblPageElementsTONew);
                }
            }
            return tblPageElementsTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPageElementsTO, cmdInsert);
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

        public static int InsertTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPageElementsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblPageElementsTO tblPageElementsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPageElements]( " + 
            "  [idPageElement]" +
            " ,[pageId]" +
            " ,[pageEleTypeId]" +
            " ,[createdOn]" +
            " ,[elementName]" +
            " ,[elementDesc]" +
            " ,[elementDisplayName]" +
            " )" +
" VALUES (" +
            "  @IdPageElement " +
            " ,@PageId " +
            " ,@PageEleTypeId " +
            " ,@CreatedOn " +
            " ,@ElementName " +
            " ,@ElementDesc " +
            " ,@ElementDisplayName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPageElement", System.Data.SqlDbType.Int).Value = tblPageElementsTO.IdPageElement;
            cmdInsert.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblPageElementsTO.PageId;
            cmdInsert.Parameters.Add("@PageEleTypeId", System.Data.SqlDbType.Int).Value = tblPageElementsTO.PageEleTypeId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPageElementsTO.CreatedOn;
            cmdInsert.Parameters.Add("@ElementName", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementName;
            cmdInsert.Parameters.Add("@ElementDesc", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementDesc;
            cmdInsert.Parameters.Add("@ElementDisplayName", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementDisplayName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPageElementsTO, cmdUpdate);
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

        public static int UpdateTblPageElements(TblPageElementsTO tblPageElementsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPageElementsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblPageElementsTO tblPageElementsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPageElements] SET " + 
            "  [idPageElement] = @IdPageElement" +
            " ,[pageId]= @PageId" +
            " ,[pageEleTypeId]= @PageEleTypeId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[elementName]= @ElementName" +
            " ,[elementDesc]= @ElementDesc" +
            " ,[elementDisplayName] = @ElementDisplayName" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPageElement", System.Data.SqlDbType.Int).Value = tblPageElementsTO.IdPageElement;
            cmdUpdate.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = tblPageElementsTO.PageId;
            cmdUpdate.Parameters.Add("@PageEleTypeId", System.Data.SqlDbType.Int).Value = tblPageElementsTO.PageEleTypeId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPageElementsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ElementName", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementName;
            cmdUpdate.Parameters.Add("@ElementDesc", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementDesc;
            cmdUpdate.Parameters.Add("@ElementDisplayName", System.Data.SqlDbType.NVarChar).Value = tblPageElementsTO.ElementDisplayName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblPageElements(Int32 idPageElement)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPageElement, cmdDelete);
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

        public static int DeleteTblPageElements(Int32 idPageElement, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPageElement, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idPageElement, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPageElements] " +
            " WHERE idPageElement = " + idPageElement +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPageElement", System.Data.SqlDbType.Int).Value = tblPageElementsTO.IdPageElement;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
