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
    public class TblLoadingSlipAddressDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loadAddress.*,loadLayer.layerDesc , delAddType.txnAddrType FROM tempLoadingSlipAddress loadAddress " +
                                  " LEFT JOIN dimLoadingLayers loadLayer ON loadAddress.loadingLayerId = loadLayer.idLoadingLayer " +
                                  " LEFT JOIN dimTxnAddrType delAddType ON delAddType.idTxnAddr = loadAddress.txnAddrTypeId " +

                                  // Vaibhav [20-Nov-2017] Added to select frorm finalLoadingSlipAddress

                                  " UNION ALL " +
                                  " SELECT loadAddress.*,loadLayer.layerDesc , delAddType.txnAddrType FROM finalLoadingSlipAddress loadAddress " +
                                  " LEFT JOIN dimLoadingLayers loadLayer ON loadAddress.loadingLayerId = loadLayer.idLoadingLayer " +
                                  " LEFT JOIN dimTxnAddrType delAddType ON delAddType.idTxnAddr = loadAddress.txnAddrTypeId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddress(int loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId=" + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipAddressTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public static TblLoadingSlipAddressTO SelectTblLoadingSlipAddress(Int32 idLoadSlipAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery()+ ")sq1 WHERE idLoadSlipAddr = " + idLoadSlipAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipAddressTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public static List<TblLoadingSlipAddressTO> ConvertDTToList(SqlDataReader tblLoadingSlipAddressTODT)
        {
            List<TblLoadingSlipAddressTO> tblLoadingSlipAddressTOList = new List<TblLoadingSlipAddressTO>();
            if (tblLoadingSlipAddressTODT != null)
            {
               while(tblLoadingSlipAddressTODT.Read())
                {
                    TblLoadingSlipAddressTO tblLoadingSlipAddressTONew = new TblLoadingSlipAddressTO();
                    if (tblLoadingSlipAddressTODT["idLoadSlipAddr"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.IdLoadSlipAddr = Convert.ToInt32(tblLoadingSlipAddressTODT["idLoadSlipAddr"].ToString());
                    if (tblLoadingSlipAddressTODT["bookDelAddrId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.BookDelAddrId = Convert.ToInt32(tblLoadingSlipAddressTODT["bookDelAddrId"].ToString());
                    if (tblLoadingSlipAddressTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipAddressTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipAddressTODT["loadingLayerId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.LoadingLayerId = Convert.ToInt32(tblLoadingSlipAddressTODT["loadingLayerId"].ToString());
                    if (tblLoadingSlipAddressTODT["address"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.Address = Convert.ToString(tblLoadingSlipAddressTODT["address"].ToString());
                    if (tblLoadingSlipAddressTODT["village"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.VillageName = Convert.ToString(tblLoadingSlipAddressTODT["village"].ToString());
                    if (tblLoadingSlipAddressTODT["taluka"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.TalukaName = Convert.ToString(tblLoadingSlipAddressTODT["taluka"].ToString());
                    if (tblLoadingSlipAddressTODT["district"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.DistrictName = Convert.ToString(tblLoadingSlipAddressTODT["district"].ToString());
                    if (tblLoadingSlipAddressTODT["state"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.State = Convert.ToString(tblLoadingSlipAddressTODT["state"].ToString());
                    if (tblLoadingSlipAddressTODT["country"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.Country = Convert.ToString(tblLoadingSlipAddressTODT["country"].ToString());
                    if (tblLoadingSlipAddressTODT["pincode"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.Pincode = Convert.ToString(tblLoadingSlipAddressTODT["pincode"].ToString());
                    if (tblLoadingSlipAddressTODT["comment"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.Comment = Convert.ToString(tblLoadingSlipAddressTODT["comment"].ToString());
                    if (tblLoadingSlipAddressTODT["billingName"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.BillingName = Convert.ToString(tblLoadingSlipAddressTODT["billingName"].ToString());
                    if (tblLoadingSlipAddressTODT["gstNo"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.GstNo = Convert.ToString(tblLoadingSlipAddressTODT["gstNo"].ToString());
                    if (tblLoadingSlipAddressTODT["contactNo"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.ContactNo = Convert.ToString(tblLoadingSlipAddressTODT["contactNo"].ToString());

                    if (tblLoadingSlipAddressTODT["txnAddrTypeId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.TxnAddrTypeId = Convert.ToInt32(tblLoadingSlipAddressTODT["txnAddrTypeId"].ToString());
                    if (tblLoadingSlipAddressTODT["txnAddrType"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.TxnAddrType = Convert.ToString(tblLoadingSlipAddressTODT["txnAddrType"].ToString());
                    if (tblLoadingSlipAddressTODT["layerDesc"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.LayerDesc = Convert.ToString(tblLoadingSlipAddressTODT["layerDesc"].ToString());

                    if (tblLoadingSlipAddressTODT["stateId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.StateId = Convert.ToInt32(tblLoadingSlipAddressTODT["stateId"].ToString());

                    if (tblLoadingSlipAddressTODT["panNo"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.PanNo = Convert.ToString(tblLoadingSlipAddressTODT["panNo"].ToString());
                    if (tblLoadingSlipAddressTODT["aadharNo"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.AadharNo = Convert.ToString(tblLoadingSlipAddressTODT["aadharNo"].ToString());

                    if (tblLoadingSlipAddressTODT["addrSourceTypeId"] != DBNull.Value)
                        tblLoadingSlipAddressTONew.AddrSourceTypeId = Convert.ToInt32(tblLoadingSlipAddressTODT["addrSourceTypeId"].ToString());

                    tblLoadingSlipAddressTOList.Add(tblLoadingSlipAddressTONew);
                }
            }
            return tblLoadingSlipAddressTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipAddressTO, cmdInsert);
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

        public static int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipAddressTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipAddress]( " +
                            "  [bookDelAddrId]" +
                            " ,[loadingSlipId]" +
                            " ,[loadingLayerId]" +
                            " ,[address]" +
                            " ,[village]" +
                            " ,[taluka]" +
                            " ,[district]" +
                            " ,[state]" +
                            " ,[country]" +
                            " ,[pincode]" +
                            " ,[comment]" +
                            " ,[billingName]" +
                            " ,[gstNo]" +
                            " ,[contactNo]" +
                            " ,[txnAddrTypeId]" +
                            " ,[stateId]" +
                            " ,[panNo]" +
                            " ,[aadharNo]" +
                            " ,[addrSourceTypeId]" +
                            " )" +
                " VALUES (" +
                            "  @BookDelAddrId " +
                            " ,@LoadingSlipId " +
                            " ,@LoadingLayerId " +
                            " ,@Address " +
                            " ,@Village " +
                            " ,@Taluka " +
                            " ,@District " +
                            " ,@State " +
                            " ,@Country " +
                            " ,@Pincode " +
                            " ,@Comment " +
                            " ,@billingName " +
                            " ,@gstNo " +
                            " ,@contactNo " +
                            " ,@txnAddrTypeId " +
                            " ,@stateId " +
                            " ,@panNo " +
                            " ,@aadharNo " +
                            " ,@addrSourceTypeId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadSlipAddr", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.IdLoadSlipAddr;
            cmdInsert.Parameters.Add("@BookDelAddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.BookDelAddrId);
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingLayerId;
            cmdInsert.Parameters.Add("@Address", System.Data.SqlDbType.VarChar, 256).Value = tblLoadingSlipAddressTO.Address;
            cmdInsert.Parameters.Add("@Village", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.VillageName);
            cmdInsert.Parameters.Add("@Taluka", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.TalukaName);
            cmdInsert.Parameters.Add("@District", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.DistrictName);
            cmdInsert.Parameters.Add("@State", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.State);
            cmdInsert.Parameters.Add("@Country", System.Data.SqlDbType.VarChar, 128).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Country);
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.VarChar, 24).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Pincode);
            cmdInsert.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.Comment);
            cmdInsert.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.BillingName);
            cmdInsert.Parameters.Add("@gstNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.GstNo);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar, 50).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.ContactNo);
            cmdInsert.Parameters.Add("@txnAddrTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.TxnAddrTypeId;
            cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.StateId);
            cmdInsert.Parameters.Add("@panNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.PanNo);
            cmdInsert.Parameters.Add("@aadharNo", System.Data.SqlDbType.NVarChar, 25).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipAddressTO.AadharNo);
            cmdInsert.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.AddrSourceTypeId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipAddressTO.IdLoadSlipAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipAddressTO, cmdUpdate);
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

        public static int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipAddressTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipAddress] SET " + 
                            "  [bookDelAddrId]= @BookDelAddrId" +
                            " ,[loadingSlipId]= @LoadingSlipId" +
                            " ,[loadingLayerId]= @LoadingLayerId" +
                            " ,[address]= @Address" +
                            " ,[village]= @Village" +
                            " ,[taluka]= @Taluka" +
                            " ,[district]= @District" +
                            " ,[state]= @State" +
                            " ,[country]= @Country" +
                            " ,[pincode]= @Pincode" +
                            " ,[comment] = @Comment" +
                            " ,[billingName] = @billingName" +
                            " ,[gstNo] = @gstNo" +
                            " ,[contactNo] = @contactNo" +
                            " ,[txnAddrTypeId] = @txnAddrTypeId" +
                            " ,[stateId] = @stateId" +
                            " ,[panNo] = @panNo" +
                            " ,[aadharNo] = @aadharNo" +
                            " ,[addrSourceTypeId] = @addrSourceTypeId" +
                            " WHERE [idLoadSlipAddr] = @IdLoadSlipAddr"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadSlipAddr", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.IdLoadSlipAddr;
            cmdUpdate.Parameters.Add("@BookDelAddrId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.BookDelAddrId;
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingSlipId;
            cmdUpdate.Parameters.Add("@LoadingLayerId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.LoadingLayerId;
            cmdUpdate.Parameters.Add("@Address", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.Address;
            cmdUpdate.Parameters.Add("@Village", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.VillageName;
            cmdUpdate.Parameters.Add("@Taluka", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.TalukaName;
            cmdUpdate.Parameters.Add("@District", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.DistrictName;
            cmdUpdate.Parameters.Add("@State", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.State;
            cmdUpdate.Parameters.Add("@Country", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.Country;
            cmdUpdate.Parameters.Add("@Pincode", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipAddressTO.Pincode;
            cmdUpdate.Parameters.Add("@Comment", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.Comment);
            cmdUpdate.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.BillingName);
            cmdUpdate.Parameters.Add("@gstNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.GstNo);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.ContactNo);
            cmdUpdate.Parameters.Add("@txnAddrTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.TxnAddrTypeId;
            cmdUpdate.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.StateId);
            cmdUpdate.Parameters.Add("@panNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.PanNo);
            cmdUpdate.Parameters.Add("@aadharNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblLoadingSlipAddressTO.AadharNo);
            cmdUpdate.Parameters.Add("@addrSourceTypeId", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.AddrSourceTypeId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadSlipAddr, cmdDelete);
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

        public static int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadSlipAddr, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idLoadSlipAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipAddress] " +
            " WHERE idLoadSlipAddr = " + idLoadSlipAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadSlipAddr", System.Data.SqlDbType.Int).Value = tblLoadingSlipAddressTO.IdLoadSlipAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
