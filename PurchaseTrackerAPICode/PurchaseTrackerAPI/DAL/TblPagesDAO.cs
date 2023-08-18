using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPagesDAO : ITblPagesDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblPagesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPages]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPagesTO> SelectAllTblPages()
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
                List<TblPagesTO> list = ConvertDTToList(rdr);
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

        public  List<TblPagesTO> SelectAllTblPages(int moduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                if (moduleId > 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE moduleId=" + moduleId;
                else
                    cmdSelect.CommandText = SqlSelectQuery();

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPagesTO> list = ConvertDTToList(rdr);
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

        public  TblPagesTO SelectTblPages(Int32 idPage)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPage = " + idPage +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPagesTO> list = ConvertDTToList(rdr);
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

        public  List<TblPagesTO> ConvertDTToList(SqlDataReader tblPagesTODT)
        {
            List<TblPagesTO> tblPagesTOList = new List<TblPagesTO>();
            if (tblPagesTODT != null)
            {
                while (tblPagesTODT.Read())
                {
                    TblPagesTO tblPagesTONew = new TblPagesTO();
                    if (tblPagesTODT["idPage"] != DBNull.Value)
                        tblPagesTONew.IdPage = Convert.ToInt32(tblPagesTODT["idPage"].ToString());
                    if (tblPagesTODT["moduleId"] != DBNull.Value)
                        tblPagesTONew.ModuleId = Convert.ToInt32(tblPagesTODT["moduleId"].ToString());
                    if (tblPagesTODT["createdOn"] != DBNull.Value)
                        tblPagesTONew.CreatedOn = Convert.ToDateTime(tblPagesTODT["createdOn"].ToString());
                    if (tblPagesTODT["pageName"] != DBNull.Value)
                        tblPagesTONew.PageName = Convert.ToString(tblPagesTODT["pageName"].ToString());
                    if (tblPagesTODT["pageDesc"] != DBNull.Value)
                        tblPagesTONew.PageDesc = Convert.ToString(tblPagesTODT["pageDesc"].ToString());
                    tblPagesTOList.Add(tblPagesTONew);
                }
            }
            return tblPagesTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPages(TblPagesTO tblPagesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPagesTO, cmdInsert);
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

        public  int InsertTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPagesTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPagesTO tblPagesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPages]( " + 
            "  [idPage]" +
            " ,[moduleId]" +
            " ,[createdOn]" +
            " ,[pageName]" +
            " ,[pageDesc]" +
            " )" +
" VALUES (" +
            "  @IdPage " +
            " ,@ModuleId " +
            " ,@CreatedOn " +
            " ,@PageName " +
            " ,@PageDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPage", System.Data.SqlDbType.Int).Value = tblPagesTO.IdPage;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblPagesTO.ModuleId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPagesTO.CreatedOn;
            cmdInsert.Parameters.Add("@PageName", System.Data.SqlDbType.NVarChar).Value = tblPagesTO.PageName;
            cmdInsert.Parameters.Add("@PageDesc", System.Data.SqlDbType.NVarChar).Value = tblPagesTO.PageDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPages(TblPagesTO tblPagesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPagesTO, cmdUpdate);
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

        public  int UpdateTblPages(TblPagesTO tblPagesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPagesTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPagesTO tblPagesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPages] SET " + 
            "  [idPage] = @IdPage" +
            " ,[moduleId]= @ModuleId" +
            " ,[createdOn]= @CreatedOn" +
            " ,[pageName]= @PageName" +
            " ,[pageDesc] = @PageDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPage", System.Data.SqlDbType.Int).Value = tblPagesTO.IdPage;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblPagesTO.ModuleId;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPagesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@PageName", System.Data.SqlDbType.NVarChar).Value = tblPagesTO.PageName;
            cmdUpdate.Parameters.Add("@PageDesc", System.Data.SqlDbType.NVarChar).Value = tblPagesTO.PageDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPages(Int32 idPage)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPage, cmdDelete);
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

        public  int DeleteTblPages(Int32 idPage, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPage, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPage, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPages] " +
            " WHERE idPage = " + idPage +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPage", System.Data.SqlDbType.Int).Value = tblPagesTO.IdPage;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
