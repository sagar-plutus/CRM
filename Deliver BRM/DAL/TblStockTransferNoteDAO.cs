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
    public class TblStockTransferNoteDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblStockTransferNote]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblStockTransferNoteTO> SelectAllTblStockTransferNote()
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
                List<TblStockTransferNoteTO> list = ConvertDTToList(rdr);
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

        public static TblStockTransferNoteTO SelectTblStockTransferNote(Int32 idStkTransferNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idStkTransferNote = " + idStkTransferNote + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockTransferNoteTO> list = ConvertDTToList(rdr);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblStockTransferNoteTO> ConvertDTToList(SqlDataReader tblStockTransferNoteTODT)
        {
            List<TblStockTransferNoteTO> tblStockTransferNoteTOList = new List<TblStockTransferNoteTO>();
            if (tblStockTransferNoteTODT != null)
            {
                while (tblStockTransferNoteTODT.Read())
                {
                    TblStockTransferNoteTO tblStockTransferNoteTONew = new TblStockTransferNoteTO();
                    if (tblStockTransferNoteTODT["idStkTransferNote"] != DBNull.Value)
                        tblStockTransferNoteTONew.IdStkTransferNote = Convert.ToInt32(tblStockTransferNoteTODT["idStkTransferNote"].ToString());
                    if (tblStockTransferNoteTODT["locationId"] != DBNull.Value)
                        tblStockTransferNoteTONew.LocationId = Convert.ToInt32(tblStockTransferNoteTODT["locationId"].ToString());
                    if (tblStockTransferNoteTODT["prodCatId"] != DBNull.Value)
                        tblStockTransferNoteTONew.ProdCatId = Convert.ToInt32(tblStockTransferNoteTODT["prodCatId"].ToString());
                    if (tblStockTransferNoteTODT["prodSpecId"] != DBNull.Value)
                        tblStockTransferNoteTONew.ProdSpecId = Convert.ToInt32(tblStockTransferNoteTODT["prodSpecId"].ToString());
                    if (tblStockTransferNoteTODT["materialId"] != DBNull.Value)
                        tblStockTransferNoteTONew.MaterialId = Convert.ToInt32(tblStockTransferNoteTODT["materialId"].ToString());
                    if (tblStockTransferNoteTODT["stockQtyBundles"] != DBNull.Value)
                        tblStockTransferNoteTONew.StockQtyBundles = Convert.ToInt32(tblStockTransferNoteTODT["stockQtyBundles"].ToString());
                    if (tblStockTransferNoteTODT["txnOpTypeId"] != DBNull.Value)
                        tblStockTransferNoteTONew.TxnOpTypeId = Convert.ToInt32(tblStockTransferNoteTODT["txnOpTypeId"].ToString());
                    if (tblStockTransferNoteTODT["createdBy"] != DBNull.Value)
                        tblStockTransferNoteTONew.CreatedBy = Convert.ToInt32(tblStockTransferNoteTODT["createdBy"].ToString());
                    if (tblStockTransferNoteTODT["createdOn"] != DBNull.Value)
                        tblStockTransferNoteTONew.CreatedOn = Convert.ToDateTime(tblStockTransferNoteTODT["createdOn"].ToString());
                    if (tblStockTransferNoteTODT["stockQtyMT"] != DBNull.Value)
                        tblStockTransferNoteTONew.StockQtyMT = Convert.ToDouble(tblStockTransferNoteTODT["stockQtyMT"].ToString());
                    if (tblStockTransferNoteTODT["remark"] != DBNull.Value)
                        tblStockTransferNoteTONew.Remark = Convert.ToString(tblStockTransferNoteTODT["remark"].ToString());
                    tblStockTransferNoteTOList.Add(tblStockTransferNoteTONew);
                }
            }
            return tblStockTransferNoteTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockTransferNoteTO, cmdInsert);
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

        public static int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockTransferNoteTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblStockTransferNoteTO tblStockTransferNoteTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockTransferNote]( " +
                                "  [locationId]" +
                                " ,[prodCatId]" +
                                " ,[prodSpecId]" +
                                " ,[materialId]" +
                                " ,[stockQtyBundles]" +
                                " ,[txnOpTypeId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[stockQtyMT]" +
                                " ,[remark]" +
                                " )" +
                    " VALUES (" +
                                "  @LocationId " +
                                " ,@ProdCatId " +
                                " ,@ProdSpecId " +
                                " ,@MaterialId " +
                                " ,@StockQtyBundles " +
                                " ,@TxnOpTypeId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@StockQtyMT " +
                                " ,@Remark " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStkTransferNote", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.IdStkTransferNote;
            cmdInsert.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.LocationId;
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.ProdCatId;
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.ProdSpecId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.MaterialId;
            cmdInsert.Parameters.Add("@StockQtyBundles", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.StockQtyBundles;
            cmdInsert.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.TxnOpTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockTransferNoteTO.CreatedOn;
            cmdInsert.Parameters.Add("@StockQtyMT", System.Data.SqlDbType.NVarChar).Value = tblStockTransferNoteTO.StockQtyMT;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblStockTransferNoteTO.Remark;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblStockTransferNoteTO.IdStkTransferNote = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockTransferNoteTO, cmdUpdate);
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

        public static int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockTransferNoteTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblStockTransferNoteTO tblStockTransferNoteTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockTransferNote] SET " + 
            "  [idStkTransferNote] = @IdStkTransferNote" +
            " ,[locationId]= @LocationId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[materialId]= @MaterialId" +
            " ,[stockQtyBundles]= @StockQtyBundles" +
            " ,[txnOpTypeId]= @TxnOpTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[stockQtyMT]= @StockQtyMT" +
            " ,[remark] = @Remark" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStkTransferNote", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.IdStkTransferNote;
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.LocationId;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.ProdCatId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.MaterialId;
            cmdUpdate.Parameters.Add("@StockQtyBundles", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.StockQtyBundles;
            cmdUpdate.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.TxnOpTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockTransferNoteTO.CreatedOn;
            cmdUpdate.Parameters.Add("@StockQtyMT", System.Data.SqlDbType.NVarChar).Value = tblStockTransferNoteTO.StockQtyMT;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblStockTransferNoteTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblStockTransferNote(Int32 idStkTransferNote)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStkTransferNote, cmdDelete);
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

        public static int DeleteTblStockTransferNote(Int32 idStkTransferNote, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStkTransferNote, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idStkTransferNote, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockTransferNote] " +
            " WHERE idStkTransferNote = " + idStkTransferNote +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStkTransferNote", System.Data.SqlDbType.Int).Value = tblStockTransferNoteTO.IdStkTransferNote;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
