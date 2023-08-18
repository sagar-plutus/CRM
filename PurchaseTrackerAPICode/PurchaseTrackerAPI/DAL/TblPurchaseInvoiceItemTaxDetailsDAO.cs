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
    public class TblPurchaseInvoiceItemTaxDetailsDAO : ITblPurchaseInvoiceItemTaxDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceItemTaxDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseInvoiceItemTaxDetails] tblPurchaseInvoiceItemTaxDetails Left Join tblTaxRates on tblTaxRates.idTaxRate =tblPurchaseInvoiceItemTaxDetails.taxRateId "; 
            return sqlSelectQry;
        }
        
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails()
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemTaxDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(Int64 purchaseInvoiceItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceItemTaxDetails.purchaseInvoiceItemId =" + purchaseInvoiceItemId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemTaxDetailsTO> list = ConvertDTToList(reader);
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


        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(Int64 purchaseInvoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceItemTaxDetails.purchaseInvoiceItemId = " + purchaseInvoiceItemId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemTaxDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  TblPurchaseInvoiceItemTaxDetailsTO SelectTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseInvItemTaxDtl = " + idPurchaseInvItemTaxDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemTaxDetailsTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblPurchaseInvoiceItemTaxDetailsTO> SelectAllTblPurchaseInvoiceItemTaxDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceItemTaxDetailsTO> list = ConvertDTToList(reader);
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
        public  List<TblPurchaseInvoiceItemTaxDetailsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceItemTaxDetailsTODT)
        {
            List<TblPurchaseInvoiceItemTaxDetailsTO> tblPurchaseInvoiceItemTaxDetailsTOList = new List<TblPurchaseInvoiceItemTaxDetailsTO>();
            if (tblPurchaseInvoiceItemTaxDetailsTODT != null)
            {
                while (tblPurchaseInvoiceItemTaxDetailsTODT.Read())
                {
                    TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTONew = new TblPurchaseInvoiceItemTaxDetailsTO();
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxRateId"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxRateId = Convert.ToInt32(tblPurchaseInvoiceItemTaxDetailsTODT["taxRateId"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxPct"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxPct = Convert.ToDouble(tblPurchaseInvoiceItemTaxDetailsTODT["taxPct"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxRatePct"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxRatePct = Convert.ToDouble(tblPurchaseInvoiceItemTaxDetailsTODT["taxRatePct"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxableAmt"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxableAmt = Convert.ToDouble(tblPurchaseInvoiceItemTaxDetailsTODT["taxableAmt"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxAmt"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxAmt = Convert.ToDouble(tblPurchaseInvoiceItemTaxDetailsTODT["taxAmt"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["idPurchaseInvItemTaxDtl"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.IdPurchaseInvItemTaxDtl = Convert.ToInt64(tblPurchaseInvoiceItemTaxDetailsTODT["idPurchaseInvItemTaxDtl"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["purchaseInvoiceItemId"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.PurchaseInvoiceItemId = Convert.ToInt64(tblPurchaseInvoiceItemTaxDetailsTODT["purchaseInvoiceItemId"].ToString());
                    if (tblPurchaseInvoiceItemTaxDetailsTODT["taxTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceItemTaxDetailsTONew.TaxTypeId = Convert.ToInt32(tblPurchaseInvoiceItemTaxDetailsTODT["taxTypeId"].ToString());
                    tblPurchaseInvoiceItemTaxDetailsTOList.Add(tblPurchaseInvoiceItemTaxDetailsTONew);
                }
            }
            return tblPurchaseInvoiceItemTaxDetailsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceItemTaxDetailsTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceItemTaxDetailsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceItemTaxDetails]( " + 
            "  [taxRateId]" +
            " ,[taxPct]" +
            " ,[taxRatePct]" +
            " ,[taxableAmt]" +
            " ,[taxAmt]" +
           // " ,[idPurchaseInvItemTaxDtl]" +
            " ,[purchaseInvoiceItemId]" +
            " )" +
" VALUES (" +
            "  @TaxRateId " +
            " ,@TaxPct " +
            " ,@TaxRatePct " +
            " ,@TaxableAmt " +
            " ,@TaxAmt " +
            //" ,@IdPurchaseInvItemTaxDtl " +
            " ,@PurchaseInvoiceItemId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@TaxRateId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxRateId;
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxPct;
            cmdInsert.Parameters.Add("@TaxRatePct", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxRatePct;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxableAmt;
            cmdInsert.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxAmt;
          //  cmdInsert.Parameters.Add("@IdPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
            cmdInsert.Parameters.Add("@PurchaseInvoiceItemId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.PurchaseInvoiceItemId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceItemTaxDetailsTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceItemTaxDetails(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceItemTaxDetailsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceItemTaxDetailsTO tblPurchaseInvoiceItemTaxDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceItemTaxDetails] SET " + 
            "  [taxRateId] = @TaxRateId" +
            " ,[taxPct]= @TaxPct" +
            " ,[taxRatePct]= @TaxRatePct" +
            " ,[taxableAmt]= @TaxableAmt" +
            " ,[taxAmt]= @TaxAmt" +
         //   " ,[idPurchaseInvItemTaxDtl]= @IdPurchaseInvItemTaxDtl" +
            " ,[purchaseInvoiceItemId] = @PurchaseInvoiceItemId" +
            " WHERE [idPurchaseInvItemTaxDtl]= @IdPurchaseInvItemTaxDtl "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@TaxRateId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxRateId;
            cmdUpdate.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxPct;
            cmdUpdate.Parameters.Add("@TaxRatePct", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxRatePct;
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@TaxAmt", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceItemTaxDetailsTO.TaxAmt;
         //   cmdUpdate.Parameters.Add("@IdPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
            cmdUpdate.Parameters.Add("@PurchaseInvoiceItemId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.PurchaseInvoiceItemId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseInvItemTaxDtl, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblPurchaseInvoiceItemTaxDetails(Int64 idPurchaseInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseInvItemTaxDtl, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblPurchaseInvoiceItemTaxDetailsByPurchaseInvoiceId(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceItemTaxDetails] " +
                    " WHERE purchaseInvoiceItemId IN ( SELECT idPurchaseInvoiceItem from " +
                                        " tblPurchaseInvoiceItemDetails WHERE purchaseInvoiceId =" + purchaseInvoiceId + ") ";
                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdDelete.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
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

        public  int ExecuteDeletionCommand(Int64 idPurchaseInvItemTaxDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceItemTaxDetails] " +
            " WHERE idPurchaseInvItemTaxDtl = " + idPurchaseInvItemTaxDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseInvItemTaxDtl", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceItemTaxDetailsTO.IdPurchaseInvItemTaxDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
