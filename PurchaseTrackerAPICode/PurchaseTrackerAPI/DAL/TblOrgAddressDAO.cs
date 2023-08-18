using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblOrgAddressDAO : ITblOrgAddressDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblOrgAddressDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOrgAddress]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblOrgAddressTO> SelectAllTblOrgAddress()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgAddressTO> list = ConvertDTToList(sqlReader);
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

        public  TblOrgAddressTO SelectTblOrgAddress(Int32 idOrgAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idOrgAddr = " + idOrgAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgAddressTO> list = ConvertDTToList(reader);
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

        public  List<TblOrgAddressTO> ConvertDTToList(SqlDataReader tblOrgAddressTODT)
        {
            List<TblOrgAddressTO> tblOrgAddressTOList = new List<TblOrgAddressTO>();
            if (tblOrgAddressTODT != null)
            {
                while (tblOrgAddressTODT.Read())
                {
                    TblOrgAddressTO tblOrgAddressTONew = new TblOrgAddressTO();
                    if (tblOrgAddressTODT["idOrgAddr"] != DBNull.Value)
                        tblOrgAddressTONew.IdOrgAddr = Convert.ToInt32(tblOrgAddressTODT["idOrgAddr"].ToString());
                    if (tblOrgAddressTODT["organizationId"] != DBNull.Value)
                        tblOrgAddressTONew.OrganizationId = Convert.ToInt32(tblOrgAddressTODT["organizationId"].ToString());
                    if (tblOrgAddressTODT["addrTypeId"] != DBNull.Value)
                        tblOrgAddressTONew.AddrTypeId = Convert.ToInt32(tblOrgAddressTODT["addrTypeId"].ToString());
                    if (tblOrgAddressTODT["addressId"] != DBNull.Value)
                        tblOrgAddressTONew.AddressId = Convert.ToInt32(tblOrgAddressTODT["addressId"].ToString());
                    if (tblOrgAddressTODT["createdBy"] != DBNull.Value)
                        tblOrgAddressTONew.CreatedBy = Convert.ToInt32(tblOrgAddressTODT["createdBy"].ToString());
                    if (tblOrgAddressTODT["updatedBy"] != DBNull.Value)
                        tblOrgAddressTONew.UpdatedBy = Convert.ToInt32(tblOrgAddressTODT["updatedBy"].ToString());
                    if (tblOrgAddressTODT["createdOn"] != DBNull.Value)
                        tblOrgAddressTONew.CreatedOn = Convert.ToDateTime(tblOrgAddressTODT["createdOn"].ToString());
                    if (tblOrgAddressTODT["updatedOn"] != DBNull.Value)
                        tblOrgAddressTONew.UpdatedOn = Convert.ToDateTime(tblOrgAddressTODT["updatedOn"].ToString());
                    tblOrgAddressTOList.Add(tblOrgAddressTONew);
                }
            }
            return tblOrgAddressTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgAddressTO, cmdInsert);
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

        public  int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgAddressTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblOrgAddressTO tblOrgAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgAddress]( " + 
                                " [organizationId]" +
                                " ,[addrTypeId]" +
                                " ,[addressId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " )" +
                    " VALUES (" +
                                "  @OrganizationId " +
                                " ,@AddrTypeId " +
                                " ,@AddressId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdOrgAddr", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.IdOrgAddr;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.OrganizationId;
            cmdInsert.Parameters.Add("@AddrTypeId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.AddrTypeId;
            cmdInsert.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.AddressId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgAddressTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgAddressTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgAddressTO.UpdatedOn);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblOrgAddressTO.IdOrgAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgAddressTO, cmdUpdate);
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

        public  int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgAddressTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblOrgAddressTO tblOrgAddressTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgAddress] SET " + 
            "  [idOrgAddr] = @IdOrgAddr" +
            " ,[organizationId]= @OrganizationId" +
            " ,[addrTypeId]= @AddrTypeId" +
            " ,[addressId]= @AddressId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgAddr", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.IdOrgAddr;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.OrganizationId;
            cmdUpdate.Parameters.Add("@AddrTypeId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.AddrTypeId;
            cmdUpdate.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.AddressId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgAddressTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgAddressTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblOrgAddress(Int32 idOrgAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgAddr, cmdDelete);
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

        public  int DeleteTblOrgAddress(Int32 idOrgAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgAddr, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idOrgAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgAddress] " +
            " WHERE idOrgAddr = " + idOrgAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgAddr", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.IdOrgAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
