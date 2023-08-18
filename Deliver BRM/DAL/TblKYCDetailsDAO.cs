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
    public class TblKYCDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblKYCDetails]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblKYCDetailsTO> SelectAllTblKYCDetails()
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
                List<TblKYCDetailsTO> list = ConvertDTToList(reader);
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

        public static TblKYCDetailsTO SelectTblKYCDetails(Int32 idKYCDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idKYCDetails = " + idKYCDetails +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblKYCDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
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

        public static TblKYCDetailsTO SelectTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM [tblKYCDetails] WHERE idKYCDetails = " + idKYCDetails ;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblKYCDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
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

        public static TblKYCDetailsTO SelectTblKYCDetailsTOByOrgId(Int32 organizationId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM [tblKYCDetails] WHERE organizationId = " + organizationId +" AND isActive =" +1;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblKYCDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
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


        //Priyanka [22-10-2018]
        public static List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText ="SELECT * FROM [tblKYCDetails] WHERE organizationId=" + organizationId + "AND isActive=" + 1;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }








        public static List<TblKYCDetailsTO> SelectAllTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM [tblKYCDetails] WHERE idKYCDetails=" + idKYCDetails;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblKYCDetailsTO> list = ConvertDTToList(reader);
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

        public static List<TblKYCDetailsTO> ConvertDTToList(SqlDataReader tblKYCDetailsTODT)
        {
            List<TblKYCDetailsTO> tblKYCDetailsTOList = new List<TblKYCDetailsTO>();
            if (tblKYCDetailsTODT != null)
            {
                while (tblKYCDetailsTODT.Read())
                {
                    TblKYCDetailsTO tblKYCDetailsTONew = new TblKYCDetailsTO();
                    if (tblKYCDetailsTODT["idKYCDetails"] != DBNull.Value)
                        tblKYCDetailsTONew.IdKYCDetails = Convert.ToInt32(tblKYCDetailsTODT["idKYCDetails"].ToString());
                    if (tblKYCDetailsTODT["organizationId"] != DBNull.Value)
                        tblKYCDetailsTONew.OrganizationId = Convert.ToInt32(tblKYCDetailsTODT["organizationId"].ToString());
                    if (tblKYCDetailsTODT["aggrSign"] != DBNull.Value)
                        tblKYCDetailsTONew.AggrSign = Convert.ToInt32(tblKYCDetailsTODT["aggrSign"].ToString());
                    if (tblKYCDetailsTODT["chequeRcvd"] != DBNull.Value)
                        tblKYCDetailsTONew.ChequeRcvd = Convert.ToInt32(tblKYCDetailsTODT["chequeRcvd"].ToString());
                    if (tblKYCDetailsTODT["KYCCompleted"] != DBNull.Value)
                        tblKYCDetailsTONew.KYCCompleted = Convert.ToInt32(tblKYCDetailsTODT["KYCCompleted"].ToString());
                    if (tblKYCDetailsTODT["createdBy"] != DBNull.Value)
                        tblKYCDetailsTONew.CreatedBy = Convert.ToInt32(tblKYCDetailsTODT["createdBy"].ToString());
                    if (tblKYCDetailsTODT["updatedBy"] != DBNull.Value)
                        tblKYCDetailsTONew.UpdatedBy = Convert.ToInt32(tblKYCDetailsTODT["updatedBy"].ToString());
                    if (tblKYCDetailsTODT["createdOn"] != DBNull.Value)
                        tblKYCDetailsTONew.CreatedOn = Convert.ToDateTime(tblKYCDetailsTODT["createdOn"].ToString());
                    if (tblKYCDetailsTODT["updatedOn"] != DBNull.Value)
                        tblKYCDetailsTONew.UpdatedOn = Convert.ToDateTime(tblKYCDetailsTODT["updatedOn"].ToString());
                    if (tblKYCDetailsTODT["isActive"] != DBNull.Value)
                        tblKYCDetailsTONew.IsActive = Convert.ToInt32(tblKYCDetailsTODT["isActive"].ToString());
                    tblKYCDetailsTOList.Add(tblKYCDetailsTONew);
                }
            }
            return tblKYCDetailsTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblKYCDetailsTO, cmdInsert);
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

        public static int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblKYCDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblKYCDetailsTO tblKYCDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblKYCDetails]( " + 
            //"  [idKYCDetails]" +
            " [organizationId]" +
            " ,[aggrSign]" +
            " ,[chequeRcvd]" +
            " ,[KYCCompleted]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
            //"  @IdKYCDetails " +
            " @OrganizationId " +
            " ,@AggrSign " +
            " ,@ChequeRcvd " +
            " ,@KYCCompleted " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdKYCDetails", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.IdKYCDetails;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.OrganizationId;
            cmdInsert.Parameters.Add("@AggrSign", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.AggrSign;
            cmdInsert.Parameters.Add("@ChequeRcvd", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.ChequeRcvd;
            cmdInsert.Parameters.Add("@KYCCompleted", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.KYCCompleted;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblKYCDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblKYCDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblKYCDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.IsActive;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblKYCDetailsTO, cmdUpdate);
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

        public static int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblKYCDetailsTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
               // conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblKYCDetailsTO tblKYCDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblKYCDetails] SET " + 
            //"  [idKYCDetails] = @IdKYCDetails" +
            " [organizationId]= @OrganizationId" +
            " ,[aggrSign]= @AggrSign" +
            " ,[chequeRcvd]= @ChequeRcvd" +
            " ,[KYCCompleted]= @KYCCompleted" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[isActive] = @IsActive" +
            " WHERE [idKYCDetails] = @idKYCDetails"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdKYCDetails", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.IdKYCDetails;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.OrganizationId;
            cmdUpdate.Parameters.Add("@AggrSign", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.AggrSign;
            cmdUpdate.Parameters.Add("@ChequeRcvd", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.ChequeRcvd;
            cmdUpdate.Parameters.Add("@KYCCompleted", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.KYCCompleted;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblKYCDetailsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblKYCDetailsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblKYCDetailsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblKYCDetails(Int32 idKYCDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idKYCDetails, cmdDelete);
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

        public static int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idKYCDetails, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idKYCDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblKYCDetails] " +
            " WHERE idKYCDetails = " + idKYCDetails +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idKYCDetails", System.Data.SqlDbType.Int).Value = tblKYCDetailsTO.IdKYCDetails;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
