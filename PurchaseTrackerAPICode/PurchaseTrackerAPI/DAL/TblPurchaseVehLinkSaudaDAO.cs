using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseVehLinkSaudaDAO : ITblPurchaseVehLinkSaudaDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseVehLinkSaudaDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseVehLinkSauda] tblPurchaseVehLinkSauda"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehLinkSaudaTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Close();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 idVehLinkSauda)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehLinkSauda = " + idVehLinkSauda +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehLinkSaudaTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Close();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseVehLinkSaudaTO> SelectPurchaseVehLinkSauda(Int32 rootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseVehLinkSauda.isActive = 1 AND tblPurchaseVehLinkSauda.rootScheduleId = " + rootScheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehLinkSaudaTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Close();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblPurchaseVehLinkSaudaTO> SelectAllTblPurchaseVehLinkSauda(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehLinkSaudaTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseVehLinkSaudaTO> SelectTblPurchaseVehLinkSauda(Int32 rootScheduleId,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + "  WHERE tblPurchaseVehLinkSauda.isActive = 1 AND tblPurchaseVehLinkSauda.rootScheduleId = " + rootScheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseVehLinkSaudaTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseVehLinkSaudaTO> ConvertDTToList(SqlDataReader tblPurchaseVehLinkSaudaTODT)
        {
            List<TblPurchaseVehLinkSaudaTO> tblPurchaseVehLinkSaudaTOList = new List<TblPurchaseVehLinkSaudaTO>();
            if (tblPurchaseVehLinkSaudaTODT != null)
            {
                while(tblPurchaseVehLinkSaudaTODT.Read())
                {
                    TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTONew = new TblPurchaseVehLinkSaudaTO();
                    if (tblPurchaseVehLinkSaudaTODT ["idVehLinkSauda"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.IdVehLinkSauda = Convert.ToInt32(tblPurchaseVehLinkSaudaTODT ["idVehLinkSauda"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT ["rootScheduleId"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.RootScheduleId = Convert.ToInt32(tblPurchaseVehLinkSaudaTODT ["rootScheduleId"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT ["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseVehLinkSaudaTODT ["purchaseEnquiryId"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT ["createdBy"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.CreatedBy = Convert.ToInt32(tblPurchaseVehLinkSaudaTODT ["createdBy"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT ["createdOn"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.CreatedOn = Convert.ToDateTime(tblPurchaseVehLinkSaudaTODT ["createdOn"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT ["linkedQty"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.LinkedQty = Convert.ToDouble(tblPurchaseVehLinkSaudaTODT ["linkedQty"].ToString());
                    if (tblPurchaseVehLinkSaudaTODT["isActive"] != DBNull.Value)
                        tblPurchaseVehLinkSaudaTONew.IsActive = Convert.ToInt32(tblPurchaseVehLinkSaudaTODT["isActive"].ToString());

                    tblPurchaseVehLinkSaudaTOList.Add(tblPurchaseVehLinkSaudaTONew);
                }
            }
            return tblPurchaseVehLinkSaudaTOList;
        }

        #endregion

        #region Insertion
        public   int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseVehLinkSaudaTO, cmdInsert);
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

        public   int InsertTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseVehLinkSaudaTO, cmdInsert);
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

        public   int ExecuteInsertionCommand(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseVehLinkSauda]( " + 
            //"  [idVehLinkSauda]" +
            "  [rootScheduleId]" +
            " ,[purchaseEnquiryId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[linkedQty]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
            //"  @IdVehLinkSauda " +
            "  @RootScheduleId " +
            " ,@PurchaseEnquiryId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@LinkedQty " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVehLinkSauda", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.IdVehLinkSauda;
            cmdInsert.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.RootScheduleId;
            cmdInsert.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.PurchaseEnquiryId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehLinkSaudaTO.CreatedOn;
            cmdInsert.Parameters.Add("@LinkedQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehLinkSaudaTO.LinkedQty;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.IsActive;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseVehLinkSaudaTO, cmdUpdate);
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

        public   int UpdateTblPurchaseVehLinkSauda(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseVehLinkSaudaTO, cmdUpdate);
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

        public   int ExecuteUpdationCommand(TblPurchaseVehLinkSaudaTO tblPurchaseVehLinkSaudaTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseVehLinkSauda] SET " + 
            //"  [idVehLinkSauda] = @IdVehLinkSauda" +
            "  [rootScheduleId]= @RootScheduleId" +
            " ,[purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[linkedQty] = @LinkedQty" +
            " ,[isActive] = @IsActive" +
            " WHERE idVehLinkSauda = @IdVehLinkSauda "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehLinkSauda", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.IdVehLinkSauda;
            cmdUpdate.Parameters.Add("@RootScheduleId", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.RootScheduleId;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.PurchaseEnquiryId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseVehLinkSaudaTO.CreatedOn;
            cmdUpdate.Parameters.Add("@LinkedQty", System.Data.SqlDbType.NVarChar).Value = tblPurchaseVehLinkSaudaTO.LinkedQty;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehLinkSauda, cmdDelete);
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

        public int DeleteTblPurchaseVehLinkSauda(Int32 idVehLinkSauda, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehLinkSauda, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVehLinkSauda, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseVehLinkSauda] " +
            " WHERE idVehLinkSauda = " + idVehLinkSauda +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehLinkSauda", System.Data.SqlDbType.Int).Value = tblPurchaseVehLinkSaudaTO.IdVehLinkSauda;
            return cmdDelete.ExecuteNonQuery();
        }

        public int DeletePurchaseVehLinkSaudaDtls(Int32 purchaseScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblPurchaseVehLinkSauda WHERE rootScheduleId=" + purchaseScheduleId + "";
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        #endregion

    }
}
