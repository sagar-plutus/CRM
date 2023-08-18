using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblStockAsPerBooksDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblStockAsPerBooks]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblStockAsPerBooksTO> SelectAllTblStockAsPerBooks()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlDataReader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlDataReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlDataReader != null)
                    sqlDataReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblStockAsPerBooksTO SelectTblStockAsPerBooks(Int32 idStockAsPerBooks)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlDataReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStockAsPerBooks = " + idStockAsPerBooks +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblStockAsPerBooksTO> list = ConvertDTToList(sqlDataReader);
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
                if (sqlDataReader != null)
                    sqlDataReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblStockAsPerBooksTO SelectTblStockAsPerBooks(DateTime stockDate)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlDataReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE DAY(createdOn) = " + stockDate.Day + " AND MONTH(createdOn)=" + stockDate.Month + " AND YEAR(createdOn)=" + stockDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlDataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblStockAsPerBooksTO> list = ConvertDTToList(sqlDataReader);
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
                if (sqlDataReader != null)
                    sqlDataReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblStockAsPerBooksTO> ConvertDTToList(SqlDataReader tblStockAsPerBooksTODT)
        {
            List<TblStockAsPerBooksTO> tblStockAsPerBooksTOList = new List<TblStockAsPerBooksTO>();
            if (tblStockAsPerBooksTODT != null)
            {
                while (tblStockAsPerBooksTODT.Read())
                {
                    TblStockAsPerBooksTO tblStockAsPerBooksTONew = new TblStockAsPerBooksTO();
                    if (tblStockAsPerBooksTODT["idStockAsPerBooks"] != DBNull.Value)
                        tblStockAsPerBooksTONew.IdStockAsPerBooks = Convert.ToInt32(tblStockAsPerBooksTODT["idStockAsPerBooks"].ToString());
                    if (tblStockAsPerBooksTODT["isConfirmed"] != DBNull.Value)
                        tblStockAsPerBooksTONew.IsConfirmed = Convert.ToInt32(tblStockAsPerBooksTODT["isConfirmed"].ToString());
                    if (tblStockAsPerBooksTODT["createdBy"] != DBNull.Value)
                        tblStockAsPerBooksTONew.CreatedBy = Convert.ToInt32(tblStockAsPerBooksTODT["createdBy"].ToString());
                    if (tblStockAsPerBooksTODT["createdOn"] != DBNull.Value)
                        tblStockAsPerBooksTONew.CreatedOn = Convert.ToDateTime(tblStockAsPerBooksTODT["createdOn"].ToString());
                    if (tblStockAsPerBooksTODT["stockInMT"] != DBNull.Value)
                        tblStockAsPerBooksTONew.StockInMT = Convert.ToDouble(tblStockAsPerBooksTODT["stockInMT"].ToString());
                    if (tblStockAsPerBooksTODT["stockFactor"] != DBNull.Value)
                        tblStockAsPerBooksTONew.StockFactor = Convert.ToDouble(tblStockAsPerBooksTODT["stockFactor"].ToString());
                    if (tblStockAsPerBooksTODT["remark"] != DBNull.Value)
                        tblStockAsPerBooksTONew.Remark = Convert.ToString(tblStockAsPerBooksTODT["remark"].ToString());
                    tblStockAsPerBooksTOList.Add(tblStockAsPerBooksTONew);
                }
            }
            return tblStockAsPerBooksTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockAsPerBooksTO, cmdInsert);
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

        public static int InsertTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockAsPerBooksTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockAsPerBooks]( " +
                            "  [isConfirmed]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[stockInMT]" +
                            " ,[stockFactor]" +
                            " ,[remark]" +
                            " )" +
                " VALUES (" +
                            "  @IsConfirmed " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@StockInMT " +
                            " ,@StockFactor " +
                            " ,@Remark " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockAsPerBooks", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.IdStockAsPerBooks;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.IsConfirmed;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockAsPerBooksTO.CreatedOn;
            cmdInsert.Parameters.Add("@StockInMT", System.Data.SqlDbType.NVarChar).Value = tblStockAsPerBooksTO.StockInMT;
            cmdInsert.Parameters.Add("@StockFactor", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockAsPerBooksTO.StockFactor);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblStockAsPerBooksTO.Remark);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblStockAsPerBooksTO.IdStockAsPerBooks = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockAsPerBooksTO, cmdUpdate);
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

        public static int UpdateTblStockAsPerBooks(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockAsPerBooksTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblStockAsPerBooksTO tblStockAsPerBooksTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockAsPerBooks] SET " + 
                            "  [isConfirmed]= @IsConfirmed" +
                            " ,[stockInMT]= @StockInMT" +
                            " ,[stockFactor]= @StockFactor" +
                            " ,[remark] = @Remark" +
                            " WHERE [idStockAsPerBooks] = @IdStockAsPerBooks "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockAsPerBooks", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.IdStockAsPerBooks;
            cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@StockInMT", System.Data.SqlDbType.NVarChar).Value = tblStockAsPerBooksTO.StockInMT;
            cmdUpdate.Parameters.Add("@StockFactor", System.Data.SqlDbType.NVarChar).Value = tblStockAsPerBooksTO.StockFactor;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockAsPerBooksTO.Remark);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockAsPerBooks, cmdDelete);
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

        public static int DeleteTblStockAsPerBooks(Int32 idStockAsPerBooks, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockAsPerBooks, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idStockAsPerBooks, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockAsPerBooks] " +
            " WHERE idStockAsPerBooks = " + idStockAsPerBooks +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockAsPerBooks", System.Data.SqlDbType.Int).Value = tblStockAsPerBooksTO.IdStockAsPerBooks;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
