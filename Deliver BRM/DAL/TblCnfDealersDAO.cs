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
    public class TblCnfDealersDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = "  SELECT cnfDealer.*,cnfOrg.firmName as cnfOrgName,dealerOrg.firmName dealerOrgName " +
                                  "  FROM tblCnfDealers cnfDealer " +
                                  "  LEFT JOIN tblOrganization cnfOrg ON cnfOrg.idOrganization = cnfDealer.cnfOrgId " +
                                  "  LEFT JOIN tblOrganization dealerOrg ON dealerOrg.idOrganization = cnfDealer.dealerOrgId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblCnfDealersTO> SelectAllTblCnfDealers()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblCnfDealersTO SelectTblCnfDealers(Int32 idCnfDealerId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idCnfDealerId = " + idCnfDealerId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCnfDealersTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblCnfDealersTO SelectTblCnfDealers(Int32 cnfOrgId,Int32 dealerOrgId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE cnfOrgId = " + cnfOrgId + " AND dealerOrgId=" + dealerOrgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCnfDealersTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblCnfDealersTO> SelectAllTblCnfDealers(Int32 dealerId, Boolean isSpecialOnly, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (isSpecialOnly)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE cnfDealer.isActive=1 AND cnfDealer.isSpecialCnf = 1 AND dealerOrgId=" + dealerId;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE cnfDealer.isActive=1 AND dealerOrgId=" + dealerId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public static List<TblCnfDealersTO> ConvertDTToList(SqlDataReader tblCnfDealersTODT)
        {
            List<TblCnfDealersTO> tblCnfDealersTOList = new List<TblCnfDealersTO>();
            if (tblCnfDealersTODT != null)
            {
                while (tblCnfDealersTODT.Read())
                {
                    TblCnfDealersTO tblCnfDealersTONew = new TblCnfDealersTO();
                    if (tblCnfDealersTODT["idCnfDealerId"] != DBNull.Value)
                        tblCnfDealersTONew.IdCnfDealerId = Convert.ToInt32(tblCnfDealersTODT["idCnfDealerId"].ToString());
                    if (tblCnfDealersTODT["cnfOrgId"] != DBNull.Value)
                        tblCnfDealersTONew.CnfOrgId = Convert.ToInt32(tblCnfDealersTODT["cnfOrgId"].ToString());
                    if (tblCnfDealersTODT["dealerOrgId"] != DBNull.Value)
                        tblCnfDealersTONew.DealerOrgId = Convert.ToInt32(tblCnfDealersTODT["dealerOrgId"].ToString());
                    if (tblCnfDealersTODT["createdBy"] != DBNull.Value)
                        tblCnfDealersTONew.CreatedBy = Convert.ToInt32(tblCnfDealersTODT["createdBy"].ToString());
                    if (tblCnfDealersTODT["isActive"] != DBNull.Value)
                        tblCnfDealersTONew.IsActive = Convert.ToInt32(tblCnfDealersTODT["isActive"].ToString());
                    if (tblCnfDealersTODT["createdOn"] != DBNull.Value)
                        tblCnfDealersTONew.CreatedOn = Convert.ToDateTime(tblCnfDealersTODT["createdOn"].ToString());
                    if (tblCnfDealersTODT["remark"] != DBNull.Value)
                        tblCnfDealersTONew.Remark = Convert.ToString(tblCnfDealersTODT["remark"].ToString());
                    if (tblCnfDealersTODT["isSpecialCnf"] != DBNull.Value)
                        tblCnfDealersTONew.IsSpecialCnf = Convert.ToInt32(tblCnfDealersTODT["isSpecialCnf"].ToString());
                    if (tblCnfDealersTODT["cnfOrgName"] != DBNull.Value)
                        tblCnfDealersTONew.CnfOrgName = Convert.ToString(tblCnfDealersTODT["cnfOrgName"].ToString());
                    if (tblCnfDealersTODT["dealerOrgName"] != DBNull.Value)
                        tblCnfDealersTONew.DealerOrgName = Convert.ToString(tblCnfDealersTODT["dealerOrgName"].ToString());
                    tblCnfDealersTOList.Add(tblCnfDealersTONew);
                }
            }
            return tblCnfDealersTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblCnfDealersTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblCnfDealersTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblCnfDealersTO tblCnfDealersTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblCnfDealers]( " +
                            "  [cnfOrgId]" +
                            " ,[dealerOrgId]" +
                            " ,[createdBy]" +
                            " ,[isActive]" +
                            " ,[createdOn]" +
                            " ,[remark]" +
                            " ,[isSpecialCnf]" +
                            " )" +
                " VALUES (" +
                            "  @CnfOrgId " +
                            " ,@DealerOrgId " +
                            " ,@CreatedBy " +
                            " ,@IsActive " +
                            " ,@CreatedOn " +
                            " ,@Remark " +
                            " ,@isSpecialCnf " +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdCnfDealerId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.IdCnfDealerId;
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.CnfOrgId;
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.DealerOrgId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCnfDealersTO.CreatedOn;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCnfDealersTO.Remark);
            cmdInsert.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCnfDealersTO.IsSpecialCnf);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblCnfDealersTO.IdCnfDealerId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblCnfDealersTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblCnfDealers(TblCnfDealersTO tblCnfDealersTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblCnfDealersTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblCnfDealersTO tblCnfDealersTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCnfDealers] SET " +
                                "  [cnfOrgId]= @CnfOrgId" +
                                " ,[dealerOrgId]= @DealerOrgId" +
                                " ,[isActive]= @IsActive" +
                                " ,[remark] = @Remark" +
                                " ,[isSpecialCnf] = @isSpecialCnf" +
                                " WHERE [idCnfDealerId] = @IdCnfDealerId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdCnfDealerId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.IdCnfDealerId;
            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.CnfOrgId;
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.DealerOrgId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.IsActive;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCnfDealersTO.Remark);
            cmdUpdate.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCnfDealersTO.IsSpecialCnf);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblCnfDealers(Int32 idCnfDealerId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idCnfDealerId, cmdDelete);
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

        public static int DeleteTblCnfDealers(Int32 idCnfDealerId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idCnfDealerId, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idCnfDealerId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblCnfDealers] " +
            " WHERE idCnfDealerId = " + idCnfDealerId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idCnfDealerId", System.Data.SqlDbType.Int).Value = tblCnfDealersTO.IdCnfDealerId;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
