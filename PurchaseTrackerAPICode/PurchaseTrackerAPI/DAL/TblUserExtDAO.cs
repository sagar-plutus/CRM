using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblUserExtDAO : ITblUserExtDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUserExtDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUserExt]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblUserExtTO> SelectAllTblUserExt()
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
                List<TblUserExtTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public  TblUserExtTO SelectTblUserExt(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE userId=" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserExtTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public  List<TblUserExtTO> ConvertDTToList(SqlDataReader tblUserExtTODT)
        {
            List<TblUserExtTO> tblUserExtTOList = new List<TblUserExtTO>();
            if (tblUserExtTODT != null)
            {
                while(tblUserExtTODT.Read())
                {
                    TblUserExtTO tblUserExtTONew = new TblUserExtTO();
                    if (tblUserExtTODT["userId"] != DBNull.Value)
                        tblUserExtTONew.UserId = Convert.ToInt32(tblUserExtTODT["userId"].ToString());
                    if (tblUserExtTODT["personId"] != DBNull.Value)
                        tblUserExtTONew.PersonId = Convert.ToInt32(tblUserExtTODT["personId"].ToString());
                    if (tblUserExtTODT["addressId"] != DBNull.Value)
                        tblUserExtTONew.AddressId = Convert.ToInt32(tblUserExtTODT["addressId"].ToString());
                    if (tblUserExtTODT["createdBy"] != DBNull.Value)
                        tblUserExtTONew.CreatedBy = Convert.ToInt32(tblUserExtTODT["createdBy"].ToString());
                    if (tblUserExtTODT["createdOn"] != DBNull.Value)
                        tblUserExtTONew.CreatedOn = Convert.ToDateTime(tblUserExtTODT["createdOn"].ToString());
                    if (tblUserExtTODT["comments"] != DBNull.Value)
                        tblUserExtTONew.Comments = Convert.ToString(tblUserExtTODT["comments"].ToString());
                    if (tblUserExtTODT["organizationId"] != DBNull.Value)
                        tblUserExtTONew.OrganizationId = Convert.ToInt32(tblUserExtTODT["organizationId"].ToString());
                    tblUserExtTOList.Add(tblUserExtTONew);
                }
            }
            return tblUserExtTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblUserExt(TblUserExtTO tblUserExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserExtTO, cmdInsert);
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

        public  int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserExtTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = "";
            if (tblUserExtTO.AddressId == 0)
            {
                 sqlQuery = @" INSERT INTO [tblUserExt]( " +
                                "  [userId]" +
                                " ,[personId]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[comments]" +
                                " ,[organizationId]" +
                                " )" +
                    " VALUES (" +
                                "  @UserId " +
                                " ,@PersonId " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@Comments " +
                                " ,@organizationId " +
                                " )";
            }
            else
            {
                 sqlQuery = @" INSERT INTO [tblUserExt]( " +
                                    "  [userId]" +
                                    " ,[personId]" +
                                    " ,[addressId]" +
                                    " ,[createdBy]" +
                                    " ,[createdOn]" +
                                    " ,[comments]" +
                                    " ,[organizationId]" +
                                    " )" +
                        " VALUES (" +
                                    "  @UserId " +
                                    " ,@PersonId " +
                                    " ,@AddressId " +
                                    " ,@CreatedBy " +
                                    " ,@CreatedOn " +
                                    " ,@Comments " +
                                    " ,@organizationId " +
                                    " )";
                
            }
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserExtTO.UserId;
            cmdInsert.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblUserExtTO.PersonId;
            if (tblUserExtTO.AddressId != 0)
                cmdInsert.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblUserExtTO.AddressId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserExtTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserExtTO.CreatedOn;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserExtTO.Comments);
            cmdInsert.Parameters.Add("@organizationId", System.Data.SqlDbType.NVarChar).Value = tblUserExtTO.OrganizationId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblUserExt(TblUserExtTO tblUserExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserExtTO, cmdUpdate);
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

        public  int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserExtTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserExt] SET " + 
                                "  [personId]= @PersonId" +
                                " ,[addressId]= @AddressId" +
                                " ,[comments] = @Comments" +
                                " WHERE  [userId] = @UserId "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserExtTO.UserId;
            cmdUpdate.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblUserExtTO.PersonId;
            cmdUpdate.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblUserExtTO.AddressId;
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserExtTO.Comments);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblUserExt()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(cmdDelete);
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

        public  int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(cmdDelete);
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

        public  int ExecuteDeletionCommand(SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserExt] " +
            " ";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
