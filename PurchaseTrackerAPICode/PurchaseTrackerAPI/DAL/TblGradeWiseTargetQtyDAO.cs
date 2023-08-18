using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblGradeWiseTargetQtyDAO : ITblGradeWiseTargetQtyDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblGradeWiseTargetQtyDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblGradeWiseTargetQty.*,tblProdClassification.prodClassDesc,tblProductItem.itemName FROM [tblGradeWiseTargetQty] tblGradeWiseTargetQty " +
                                  " left join tblProductItem tblProductItem on tblProductItem.idProdItem=tblGradeWiseTargetQty.prodItemId " +
                                  " left join tblProdClassification tblProdClassification on tblProdClassification.idProdClass=tblGradeWiseTargetQty.prodClassId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeWiseTargetQtyTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblGradeWiseTargetQtyTO> SelectTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGradeWiseTargetQty = " + idGradeWiseTargetQty + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeWiseTargetQtyTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public  List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE rateBandPurchaseId = " + rateBandPurchaseId + " and purchaseManagerId= " + pmId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeWiseTargetQtyTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblGradeWiseTargetQtyTO> SelectGradeWiseTargetQtyDtls(Int32 rateBandPurchaseId, Int32 pmId, SqlConnection conn, SqlTransaction tran)
        {
            // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE rateBandPurchaseId = " + rateBandPurchaseId + " and purchaseManagerId= " + pmId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeWiseTargetQtyTO> list = ConvertDTToList(reader);
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();

            }
        }
        public  List<TblGradeWiseTargetQtyTO> SelectAllTblGradeWiseTargetQty(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
                SqlDataReader TblGlobalRatePurchaseTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGradeWiseTargetQtyTO> list = ConvertDTToList(TblGlobalRatePurchaseTODT);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public  List<TblGradeWiseTargetQtyTO> ConvertDTToList(SqlDataReader tblGradeWiseTargetQtyTODT)
        {
            List<TblGradeWiseTargetQtyTO> tblGradeWiseTargetQtyTOList = new List<TblGradeWiseTargetQtyTO>();
            if (tblGradeWiseTargetQtyTODT != null)
            {
                while (tblGradeWiseTargetQtyTODT.Read())
                {
                    TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTONew = new TblGradeWiseTargetQtyTO();
                    if (tblGradeWiseTargetQtyTODT["idGradeWiseTargetQty"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.IdGradeWiseTargetQty = Convert.ToInt32(tblGradeWiseTargetQtyTODT["idGradeWiseTargetQty"].ToString());
                    if (tblGradeWiseTargetQtyTODT["purchaseManagerId"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.PurchaseManagerId = Convert.ToInt32(tblGradeWiseTargetQtyTODT["purchaseManagerId"].ToString());
                    if (tblGradeWiseTargetQtyTODT["prodItemId"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.ProdItemId = Convert.ToInt32(tblGradeWiseTargetQtyTODT["prodItemId"].ToString());
                    if (tblGradeWiseTargetQtyTODT["isRestrict"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.IsRestrict = Convert.ToInt32(tblGradeWiseTargetQtyTODT["isRestrict"].ToString());
                    if (tblGradeWiseTargetQtyTODT["prodClassId"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.ProdClassId = Convert.ToInt32(tblGradeWiseTargetQtyTODT["prodClassId"].ToString());
                    if (tblGradeWiseTargetQtyTODT["bookingTargetQty"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.BookingTargetQty = Convert.ToDouble(tblGradeWiseTargetQtyTODT["bookingTargetQty"].ToString());
                    if (tblGradeWiseTargetQtyTODT["unloadingTargetQty"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.UnloadingTargetQty = Convert.ToDouble(tblGradeWiseTargetQtyTODT["unloadingTargetQty"].ToString());
                    if (tblGradeWiseTargetQtyTODT["rateBandPurchaseId"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.RateBandPurchaseId = Convert.ToInt32(tblGradeWiseTargetQtyTODT["rateBandPurchaseId"].ToString());
                    if (tblGradeWiseTargetQtyTODT["prodClassDesc"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.ProdClassName = Convert.ToString(tblGradeWiseTargetQtyTODT["prodClassDesc"].ToString());
                    if (tblGradeWiseTargetQtyTODT["itemName"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.ItemName = Convert.ToString(tblGradeWiseTargetQtyTODT["itemName"].ToString());

                    if (tblGradeWiseTargetQtyTODT["pendingBookingQty"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.PendingBookingQty = Convert.ToDouble(tblGradeWiseTargetQtyTODT["pendingBookingQty"].ToString());

                    if (tblGradeWiseTargetQtyTODT["pendingUnloadingQty"] != DBNull.Value)
                        tblGradeWiseTargetQtyTONew.PendingUnloadingQty = Convert.ToDouble(tblGradeWiseTargetQtyTODT["pendingUnloadingQty"].ToString());

                    tblGradeWiseTargetQtyTOList.Add(tblGradeWiseTargetQtyTONew);
                }
            }
            return tblGradeWiseTargetQtyTOList;
        }
        #endregion

        #region Insertion
        public  int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGradeWiseTargetQtyTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGradeWiseTargetQtyTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGradeWiseTargetQty]( " +
            //"  [idGradeWiseTargetQty]" +
            "  [purchaseManagerId]" +
            " ,[prodItemId]" +
            " ,[isRestrict]" +
            " ,[prodClassId]" +
            " ,[rateBandPurchaseId]" +
            " ,[bookingTargetQty]" +
            " ,[unloadingTargetQty]" +
            " ,[pendingBookingQty]" +
            " ,[pendingUnloadingQty]" +
            " )" +
" VALUES (" +
            //"  @IdGradeWiseTargetQty " +
            "  @PurchaseManagerId " +
            " ,@ProdItemId " +
            " ,@IsRestrict " +
            " ,@ProdClassId " +
            " ,@RateBandPurchaseId " +
            " ,@BookingTargetQty " +
            " ,@UnloadingTargetQty " +
            " ,@PendingBookingQty " +
            " ,@PendingUnloadingQty " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
            cmdInsert.Parameters.Add("@PurchaseManagerId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.PurchaseManagerId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.ProdItemId;
            cmdInsert.Parameters.Add("@IsRestrict", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IsRestrict;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.ProdClassId;
            cmdInsert.Parameters.Add("@RateBandPurchaseId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.RateBandPurchaseId;
            cmdInsert.Parameters.Add("@BookingTargetQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.BookingTargetQty;
            cmdInsert.Parameters.Add("@UnloadingTargetQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.UnloadingTargetQty;
            cmdInsert.Parameters.Add("@PendingBookingQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.PendingBookingQty;
            cmdInsert.Parameters.Add("@PendingUnloadingQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.PendingUnloadingQty;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGradeWiseTargetQtyTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblGradeWiseTargetQty(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGradeWiseTargetQtyTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblGradeWiseTargetQtyTO tblGradeWiseTargetQtyTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGradeWiseTargetQty] SET " +
            //"  [idGradeWiseTargetQty] = @IdGradeWiseTargetQty" +
            "  [purchaseManagerId]= @PurchaseManagerId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[isRestrict]= @IsRestrict" +
            " ,[prodClassId]= @ProdClassId" +
            " ,[rateBandPurchaseId]= @RateBandPurchaseId" +
            " ,[bookingTargetQty]= @BookingTargetQty" +
            " ,[unloadingTargetQty] = @UnloadingTargetQty" +
            " ,[pendingBookingQty] = @PendingBookingQty" +
            " ,[pendingUnloadingQty] = @PendingUnloadingQty" +
            " WHERE idGradeWiseTargetQty = @IdGradeWiseTargetQty ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
            cmdUpdate.Parameters.Add("@PurchaseManagerId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.PurchaseManagerId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.ProdItemId;
            cmdUpdate.Parameters.Add("@IsRestrict", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IsRestrict;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.ProdClassId;
            cmdUpdate.Parameters.Add("@RateBandPurchaseId", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.RateBandPurchaseId;
            cmdUpdate.Parameters.Add("@BookingTargetQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.BookingTargetQty;
            cmdUpdate.Parameters.Add("@UnloadingTargetQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.UnloadingTargetQty;
            cmdUpdate.Parameters.Add("@PendingBookingQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.PendingBookingQty;
            cmdUpdate.Parameters.Add("@PendingUnloadingQty", System.Data.SqlDbType.Decimal).Value = tblGradeWiseTargetQtyTO.PendingUnloadingQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGradeWiseTargetQty, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblGradeWiseTargetQty(Int32 idGradeWiseTargetQty, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGradeWiseTargetQty, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int32 idGradeWiseTargetQty, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGradeWiseTargetQty] " +
            " WHERE idGradeWiseTargetQty = " + idGradeWiseTargetQty + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGradeWiseTargetQty", System.Data.SqlDbType.Int).Value = tblGradeWiseTargetQtyTO.IdGradeWiseTargetQty;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
