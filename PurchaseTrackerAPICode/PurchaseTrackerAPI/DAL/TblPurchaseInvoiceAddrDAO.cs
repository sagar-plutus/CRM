using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseInvoiceAddrDAO : ITblPurchaseInvoiceAddrDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceAddrDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPurchaseInvoiceAddr] tblPurchaseInvoiceAddr"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr()
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

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceAddrTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(Int64 purchaseInvoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceAddr.purchaseInvoiceId = " + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceAddrTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(Int64 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceAddr.purchaseInvoiceId = " + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceAddrTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceAddrTO SelectTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPurchaseInvoiceAddr = " + idPurchaseInvoiceAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceAddrTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceAddrTO> SelectAllTblPurchaseInvoiceAddr(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceAddrTO> list = ConvertDTToList(reader);
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

        public  List<TblPurchaseInvoiceAddrTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceAddrTODT)
        {
            List<TblPurchaseInvoiceAddrTO> tblPurchaseInvoiceAddrTOList = new List<TblPurchaseInvoiceAddrTO>();
            if (tblPurchaseInvoiceAddrTODT != null)
            {
              while(tblPurchaseInvoiceAddrTODT.Read())
                {
                    TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTONew = new TblPurchaseInvoiceAddrTO();
                    if (tblPurchaseInvoiceAddrTODT["txnAddrTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.TxnAddrTypeId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["txnAddrTypeId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["billingPartyOrgId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.BillingPartyOrgId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["billingPartyOrgId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["talukaId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.TalukaId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["talukaId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["districtId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.DistrictId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["districtId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["stateId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.StateId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["stateId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["countryId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.CountryId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["countryId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["addrSourceTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.AddrSourceTypeId = Convert.ToInt32(tblPurchaseInvoiceAddrTODT["addrSourceTypeId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["idPurchaseInvoiceAddr"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.IdPurchaseInvoiceAddr = Convert.ToInt64(tblPurchaseInvoiceAddrTODT["idPurchaseInvoiceAddr"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.PurchaseInvoiceId = Convert.ToInt64(tblPurchaseInvoiceAddrTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["billingPartyName"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.BillingPartyName = Convert.ToString(tblPurchaseInvoiceAddrTODT["billingPartyName"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["gstinNo"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.GstinNo = Convert.ToString(tblPurchaseInvoiceAddrTODT["gstinNo"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["panNo"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.PanNo = Convert.ToString(tblPurchaseInvoiceAddrTODT["panNo"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["aadharNo"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.AadharNo = Convert.ToString(tblPurchaseInvoiceAddrTODT["aadharNo"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["contactNo"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.ContactNo = Convert.ToString(tblPurchaseInvoiceAddrTODT["contactNo"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["address"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.Address = Convert.ToString(tblPurchaseInvoiceAddrTODT["address"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["taluka"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.Taluka = Convert.ToString(tblPurchaseInvoiceAddrTODT["taluka"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["district"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.District = Convert.ToString(tblPurchaseInvoiceAddrTODT["district"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["state"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.State = Convert.ToString(tblPurchaseInvoiceAddrTODT["state"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["pinCode"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.PinCode = Convert.ToString(tblPurchaseInvoiceAddrTODT["pinCode"].ToString());
                    if (tblPurchaseInvoiceAddrTODT["country"] != DBNull.Value)
                        tblPurchaseInvoiceAddrTONew.Country = Convert.ToString(tblPurchaseInvoiceAddrTODT["country"].ToString());
                    tblPurchaseInvoiceAddrTOList.Add(tblPurchaseInvoiceAddrTONew);
                }
            }
            return tblPurchaseInvoiceAddrTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceAddrTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceAddrTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceAddr]( " + 
            "  [txnAddrTypeId]" +
            " ,[billingPartyOrgId]" +
            " ,[talukaId]" +
            " ,[districtId]" +
            " ,[stateId]" +
            " ,[countryId]" +
            " ,[addrSourceTypeId]" +
           // " ,[idPurchaseInvoiceAddr]" +
            " ,[purchaseInvoiceId]" +
            " ,[billingPartyName]" +
            " ,[gstinNo]" +
            " ,[panNo]" +
            " ,[aadharNo]" +
            " ,[contactNo]" +
            " ,[address]" +
            " ,[taluka]" +
            " ,[district]" +
            " ,[state]" +
            " ,[pinCode]" +
            " ,[country]" +
            " )" +
" VALUES (" +
            "  @TxnAddrTypeId " +
            " ,@BillingPartyOrgId " +
            " ,@TalukaId " +
            " ,@DistrictId " +
            " ,@StateId " +
            " ,@CountryId " +
            " ,@AddrSourceTypeId " +
           // " ,@IdPurchaseInvoiceAddr " +
            " ,@PurchaseInvoiceId " +
            " ,@BillingPartyName " +
            " ,@GstinNo " +
            " ,@PanNo " +
            " ,@AadharNo " +
            " ,@ContactNo " +
            " ,@Address " +
            " ,@Taluka " +
            " ,@District " +
            " ,@State " +
            " ,@PinCode " +
            " ,@Country " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.TxnAddrTypeId);
            cmdInsert.Parameters.Add("@BillingPartyOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.BillingPartyOrgId);
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.TalukaId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.DistrictId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.StateId);
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.CountryId);
            cmdInsert.Parameters.Add("@AddrSourceTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.AddrSourceTypeId);
          //  cmdInsert.Parameters.Add("@IdPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
            cmdInsert.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value =tblPurchaseInvoiceAddrTO.PurchaseInvoiceId;
            cmdInsert.Parameters.Add("@BillingPartyName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.BillingPartyName);
            cmdInsert.Parameters.Add("@GstinNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.GstinNo);
            cmdInsert.Parameters.Add("@PanNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.PanNo);
            cmdInsert.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.AadharNo);
            cmdInsert.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.ContactNo);
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Address);
            cmdInsert.Parameters.Add("@Taluka", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Taluka);
            cmdInsert.Parameters.Add("@District", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.District);
            cmdInsert.Parameters.Add("@State", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.State);
            cmdInsert.Parameters.Add("@PinCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.PinCode);
            cmdInsert.Parameters.Add("@Country", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Country);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceAddrTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceAddr(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceAddrTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceAddrTO tblPurchaseInvoiceAddrTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceAddr] SET " + 
            "  [txnAddrTypeId] = @TxnAddrTypeId" +
            " ,[billingPartyOrgId]= @BillingPartyOrgId" +
            " ,[talukaId]= @TalukaId" +
            " ,[districtId]= @DistrictId" +
            " ,[stateId]= @StateId" +
            " ,[countryId]= @CountryId" +
            " ,[addrSourceTypeId]= @AddrSourceTypeId" +
            //" ,[idPurchaseInvoiceAddr]= @IdPurchaseInvoiceAddr" +
            " ,[purchaseInvoiceId]= @PurchaseInvoiceId" +
            " ,[billingPartyName]= @BillingPartyName" +
            " ,[gstinNo]= @GstinNo" +
            " ,[panNo]= @PanNo" +
            " ,[aadharNo]= @AadharNo" +
            " ,[contactNo]= @ContactNo" +
            " ,[address]= @Address" +
            " ,[taluka]= @Taluka" +
            " ,[district]= @District" +
            " ,[state]= @State" +
            " ,[pinCode]= @PinCode" +
            " ,[country] = @Country" +
            " WHERE [idPurchaseInvoiceAddr]= @IdPurchaseInvoiceAddr "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@TxnAddrTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceAddrTO.TxnAddrTypeId;
            cmdUpdate.Parameters.Add("@BillingPartyOrgId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceAddrTO.BillingPartyOrgId;
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.TalukaId);
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.DistrictId);
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceAddrTO.StateId;
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.CountryId);
            cmdUpdate.Parameters.Add("@AddrSourceTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceAddrTO.AddrSourceTypeId;
            cmdUpdate.Parameters.Add("@IdPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr);
            cmdUpdate.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.PurchaseInvoiceId;
            cmdUpdate.Parameters.Add("@BillingPartyName", System.Data.SqlDbType.NVarChar).Value = tblPurchaseInvoiceAddrTO.BillingPartyName;
            cmdUpdate.Parameters.Add("@GstinNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.GstinNo);
            cmdUpdate.Parameters.Add("@PanNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.PanNo);
            cmdUpdate.Parameters.Add("@AadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.AadharNo);
            cmdUpdate.Parameters.Add("@ContactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.ContactNo);
            cmdUpdate.Parameters.Add("@Address", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Address);
            cmdUpdate.Parameters.Add("@Taluka", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Taluka);
            cmdUpdate.Parameters.Add("@District", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.District);
            cmdUpdate.Parameters.Add("@State", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.State);
            cmdUpdate.Parameters.Add("@PinCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.PinCode);
            cmdUpdate.Parameters.Add("@Country", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceAddrTO.Country);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseInvoiceAddr, cmdDelete);
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

        public  int DeleteTblPurchaseInvoiceAddr(Int64 idPurchaseInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseInvoiceAddr, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int64 idPurchaseInvoiceAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceAddr] " +
            " WHERE idPurchaseInvoiceAddr = " + idPurchaseInvoiceAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseInvoiceAddr", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceAddrTO.IdPurchaseInvoiceAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
