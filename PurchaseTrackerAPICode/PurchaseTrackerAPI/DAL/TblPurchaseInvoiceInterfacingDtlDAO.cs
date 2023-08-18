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
    public class TblPurchaseInvoiceInterfacingDtlDAO : ITblPurchaseInvoiceInterfacingDtlDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceInterfacingDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseInvoiceInterfacingDtl] tblPurchaseInvoiceInterfacingDtl "; 
            return sqlSelectQry;
        }

        public String SqlSelectQueryForReport()
        {
            String SqlSelectQueryForReport = " SELECT tblPurchaseInvoiceInterfacingDtl.purchaseInvoiceId," +
                "  masterValue1.masterValueName AS purchaseAcc,masterValue2.masterValueName AS cGSTINPUT," +
                "  masterValue3.masterValueName AS iGSTINPUT,masterValue4.masterValueName AS sGSTINPUT," +
                "  tblPurchaseInvoiceInterfacingDtl.narration" +
                "  FROM tblPurchaseInvoiceInterfacingDtl tblPurchaseInvoiceInterfacingDtl" +
                "  LEFT JOIN dimMasterValue masterValue1 ON masterValue1.idMasterValue =  tblPurchaseInvoiceInterfacingDtl.purAccId" +
                "  LEFT JOIN dimMasterValue masterValue2 ON masterValue2.idMasterValue =  tblPurchaseInvoiceInterfacingDtl.cgstId" +
                "  LEFT JOIN dimMasterValue masterValue3 ON masterValue3.idMasterValue =  tblPurchaseInvoiceInterfacingDtl.igstId" +
                "  LEFT JOIN dimMasterValue masterValue4 ON masterValue4.idMasterValue =  tblPurchaseInvoiceInterfacingDtl.sgstId";
            return SqlSelectQueryForReport;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurInvInterfacingDtl = " + idPurInvInterfacingDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceInterfacingDtl.purchaseInvoiceId = " + PurInvId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOAgainstPurInvId(Int64 PurInvId,  SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceInterfacingDtl.purchaseInvoiceId = " + PurInvId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblPurchaseInvoiceInterfacingDtlTO SelectTblPurchaseInvoiceInterfacingDtlTOForReport(Int64 PurInvId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForReport() + " WHERE tblPurchaseInvoiceInterfacingDtl.purchaseInvoiceId = " + PurInvId + " ";
                cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToListForReport(reader);
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
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();                
            }
        }


        public List<TblPurchaseInvoiceInterfacingDtlTO> SelectAllTblPurchaseInvoiceInterfacingDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceInterfacingDtlTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceInterfacingDtlTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceInterfacingDtlTODT)
        {
            List<TblPurchaseInvoiceInterfacingDtlTO> tblPurchaseInvoiceInterfacingDtlTOList = new List<TblPurchaseInvoiceInterfacingDtlTO>();
            if (tblPurchaseInvoiceInterfacingDtlTODT != null)
            {
                while (tblPurchaseInvoiceInterfacingDtlTODT.Read())
                {
                    TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTONew = new TblPurchaseInvoiceInterfacingDtlTO();
                    if (tblPurchaseInvoiceInterfacingDtlTODT["idPurInvInterfacingDtl"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.IdPurInvInterfacingDtl = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["idPurInvInterfacingDtl"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["voucherTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.VoucherTypeId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["voucherTypeId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["cgstId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.CgstId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["cgstId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["sgstId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.SgstId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["sgstId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["igstId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.IgstId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["igstId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["ipTransportAdvAccId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.IpTransportAdvAccId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["ipTransportAdvAccId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["costCategoryId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.CostCategoryId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["costCategoryId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["gradeId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.GradeId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["gradeId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["purAccId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.PurAccId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["purAccId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["otherExpAccId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.OtherExpAccId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["otherExpAccId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["materialtemId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.MaterialtemId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["materialtemId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["costCenterId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.CostCenterId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["costCenterId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.PurchaseInvoiceId = Convert.ToInt64(tblPurchaseInvoiceInterfacingDtlTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["narration"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.Narration = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["narration"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["otherExpInsuAccId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.OtherExpInsuAccId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["otherExpInsuAccId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["tdsAccId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.TdsAccId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["tdsAccId"].ToString());

                    tblPurchaseInvoiceInterfacingDtlTOList.Add(tblPurchaseInvoiceInterfacingDtlTONew);
                }
            }
            return tblPurchaseInvoiceInterfacingDtlTOList;
        }

        public List<TblPurchaseInvoiceInterfacingDtlTO> ConvertDTToListForReport(SqlDataReader tblPurchaseInvoiceInterfacingDtlTODT)
        {
            List<TblPurchaseInvoiceInterfacingDtlTO> tblPurchaseInvoiceInterfacingDtlTOList = new List<TblPurchaseInvoiceInterfacingDtlTO>();
            if (tblPurchaseInvoiceInterfacingDtlTODT != null)
            {
                while (tblPurchaseInvoiceInterfacingDtlTODT.Read())
                {
                    TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTONew = new TblPurchaseInvoiceInterfacingDtlTO();
                    if (tblPurchaseInvoiceInterfacingDtlTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.PurchaseInvoiceId = Convert.ToInt32(tblPurchaseInvoiceInterfacingDtlTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["purchaseAcc"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.PurchaseAcc = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["purchaseAcc"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["cGSTINPUT"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.CGSTINPUT = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["cGSTINPUT"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["iGSTINPUT"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.IGSTINPUT = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["iGSTINPUT"].ToString());
                    if (tblPurchaseInvoiceInterfacingDtlTODT["sGSTINPUT"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.SGSTINPUT = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["sGSTINPUT"].ToString());
                                      
                    if (tblPurchaseInvoiceInterfacingDtlTODT["narration"] != DBNull.Value)
                        tblPurchaseInvoiceInterfacingDtlTONew.Narration = Convert.ToString(tblPurchaseInvoiceInterfacingDtlTODT["narration"].ToString());
                    tblPurchaseInvoiceInterfacingDtlTOList.Add(tblPurchaseInvoiceInterfacingDtlTONew);
                }
            }
            return tblPurchaseInvoiceInterfacingDtlTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceInterfacingDtlTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceInterfacingDtlTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceInterfacingDtl]( " + 
           // "  [idPurInvInterfacingDtl]" +
            " [voucherTypeId]" +
            " ,[cgstId]" +
            " ,[sgstId]" +
            " ,[igstId]" +
            " ,[ipTransportAdvAccId]" +
            " ,[costCategoryId]" +
            " ,[gradeId]" +
            " ,[purAccId]" +
            " ,[otherExpAccId]" +
            " ,[materialtemId]" +
            " ,[costCenterId]" +
            " ,[purchaseInvoiceId]" +
            " ,[narration]" +
            " ,[otherExpInsuAccId]" +
            " ,[tdsAccId]" +
            " )" +
" VALUES (" +
           // "  @IdPurInvInterfacingDtl " +
            " @VoucherTypeId " +
            " ,@CgstId " +
            " ,@SgstId " +
            " ,@IgstId " +
            " ,@IpTransportAdvAccId " +
            " ,@CostCategoryId " +
            " ,@GradeId " +
            " ,@PurAccId " +
            " ,@OtherExpAccId " +
            " ,@MaterialtemId " +
            " ,@CostCenterId " +
            " ,@PurchaseInvoiceId " +
            " ,@Narration " +
            " ,@otherExpInsuAccId " +
            " ,@tdsAccId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
            cmdInsert.Parameters.Add("@VoucherTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.VoucherTypeId);
            cmdInsert.Parameters.Add("@CgstId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CgstId);
            cmdInsert.Parameters.Add("@SgstId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.SgstId);
            cmdInsert.Parameters.Add("@IgstId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.IgstId);
            cmdInsert.Parameters.Add("@IpTransportAdvAccId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.IpTransportAdvAccId);
            cmdInsert.Parameters.Add("@CostCategoryId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CostCategoryId);
            cmdInsert.Parameters.Add("@GradeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.GradeId);
            cmdInsert.Parameters.Add("@PurAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.PurAccId);
            cmdInsert.Parameters.Add("@OtherExpAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.OtherExpAccId);
            cmdInsert.Parameters.Add("@MaterialtemId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.MaterialtemId);
            cmdInsert.Parameters.Add("@CostCenterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CostCenterId);
            cmdInsert.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.PurchaseInvoiceId);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.Narration);
            cmdInsert.Parameters.Add("@otherExpInsuAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.OtherExpInsuAccId);
            cmdInsert.Parameters.Add("@tdsAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.TdsAccId);

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceInterfacingDtlTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceInterfacingDtl(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceInterfacingDtlTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceInterfacingDtlTO tblPurchaseInvoiceInterfacingDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceInterfacingDtl] SET " + 
           // "  [idPurInvInterfacingDtl] = @IdPurInvInterfacingDtl" +
            " [voucherTypeId]= @VoucherTypeId" +
            " ,[cgstId]= @CgstId" +
            " ,[sgstId]= @SgstId" +
            " ,[igstId]= @IgstId" +
            " ,[ipTransportAdvAccId]= @IpTransportAdvAccId" +
            " ,[costCategoryId]= @CostCategoryId" +
            " ,[gradeId]= @GradeId" +
            " ,[purAccId]= @PurAccId" +
            " ,[otherExpAccId]= @OtherExpAccId" +
            " ,[materialtemId]= @MaterialtemId" +
            " ,[costCenterId]= @CostCenterId" +
            " ,[purchaseInvoiceId]= @PurchaseInvoiceId" +
            " ,[narration] = @Narration" +
            " ,[otherExpInsuAccId] = @otherExpInsuAccId" +
            " ,[tdsAccId] = @tdsAccId" +
            " WHERE [idPurInvInterfacingDtl] = @IdPurInvInterfacingDtl "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
            cmdUpdate.Parameters.Add("@VoucherTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.VoucherTypeId);
            cmdUpdate.Parameters.Add("@CgstId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CgstId);
            cmdUpdate.Parameters.Add("@SgstId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.SgstId);
            cmdUpdate.Parameters.Add("@IgstId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.IgstId);
            cmdUpdate.Parameters.Add("@IpTransportAdvAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.IpTransportAdvAccId);
            cmdUpdate.Parameters.Add("@CostCategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CostCategoryId);
            cmdUpdate.Parameters.Add("@GradeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.GradeId);
            cmdUpdate.Parameters.Add("@PurAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.PurAccId);
            cmdUpdate.Parameters.Add("@OtherExpAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.OtherExpAccId);
            cmdUpdate.Parameters.Add("@MaterialtemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.MaterialtemId);
            cmdUpdate.Parameters.Add("@CostCenterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.CostCenterId);
            cmdUpdate.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.PurchaseInvoiceId);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.Narration);
            cmdUpdate.Parameters.Add("@otherExpInsuAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.OtherExpInsuAccId);
            cmdUpdate.Parameters.Add("@tdsAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceInterfacingDtlTO.TdsAccId);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurInvInterfacingDtl, cmdDelete);
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

        public  int DeleteTblPurchaseInvoiceInterfacingDtl(Int32 idPurInvInterfacingDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurInvInterfacingDtl, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPurInvInterfacingDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceInterfacingDtl] " +
            " WHERE idPurInvInterfacingDtl = " + idPurInvInterfacingDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurInvInterfacingDtl", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceInterfacingDtlTO.IdPurInvInterfacingDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
