using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;
namespace SalesTrackerAPI.DAL
{
    public class TblInvoiceBankDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblInvoiceBankDetails]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails()
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
                List<TblInvoiceBankDetailsTO> list = ConvertDTToList(reader);
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

        public static TblInvoiceBankDetailsTO SelectTblInvoiceBankDetails(Int32 idBank)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBank=" + idBank;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceBankDetailsTO> list = ConvertDTToList(reader);
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

        public  static List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceBankDetailsTO> list = ConvertDTToList(reader);
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
        public static List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgId=" + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceBankDetailsTO> list = ConvertDTToList(reader);
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


        public static List<TblInvoiceBankDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceBankDetailsTODT)
        {
            List<TblInvoiceBankDetailsTO> tblInvoiceBankDetailsTOList = new List<TblInvoiceBankDetailsTO>();
            if (tblInvoiceBankDetailsTODT != null)
            {
                while (tblInvoiceBankDetailsTODT.Read())
                {
                    TblInvoiceBankDetailsTO tblInvoiceBankDetailsTONew = new TblInvoiceBankDetailsTO();
                    if (tblInvoiceBankDetailsTODT["idBank"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.IdBank = Convert.ToInt32(tblInvoiceBankDetailsTODT ["idBank"].ToString());
                    if (tblInvoiceBankDetailsTODT ["orgId"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.OrgId = Convert.ToInt32(tblInvoiceBankDetailsTODT ["orgId"].ToString());
                    if (tblInvoiceBankDetailsTODT ["talukaId"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.TalukaId = Convert.ToInt32(tblInvoiceBankDetailsTODT ["talukaId"].ToString());
                    if (tblInvoiceBankDetailsTODT ["districtId"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.DistrictId = Convert.ToInt32(tblInvoiceBankDetailsTODT ["districtId"].ToString());
                    if (tblInvoiceBankDetailsTODT ["stateId"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.StateId = Convert.ToInt32(tblInvoiceBankDetailsTODT ["stateId"].ToString());
                    if (tblInvoiceBankDetailsTODT ["countryId"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.CountryId = Convert.ToInt32(tblInvoiceBankDetailsTODT ["countryId"].ToString());
                    if (tblInvoiceBankDetailsTODT ["pincode"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.Pincode = Convert.ToInt32(tblInvoiceBankDetailsTODT ["pincode"].ToString());
                    if (tblInvoiceBankDetailsTODT ["createdBy"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.CreatedBy = Convert.ToInt32(tblInvoiceBankDetailsTODT ["createdBy"].ToString());
                    if (tblInvoiceBankDetailsTODT ["createdOn"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.CreatedOn = Convert.ToDateTime(tblInvoiceBankDetailsTODT ["createdOn"].ToString());
                    if (tblInvoiceBankDetailsTODT ["bankName"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.BankName = Convert.ToString(tblInvoiceBankDetailsTODT ["bankName"].ToString());
                    if (tblInvoiceBankDetailsTODT ["accountNo"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.AccountNo = Convert.ToString(tblInvoiceBankDetailsTODT ["accountNo"].ToString());
                    if (tblInvoiceBankDetailsTODT ["ifscCode"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.IfscCode = Convert.ToString(tblInvoiceBankDetailsTODT ["ifscCode"].ToString());
                    if (tblInvoiceBankDetailsTODT ["branch"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.Branch = Convert.ToString(tblInvoiceBankDetailsTODT["branch"].ToString());
                    if (tblInvoiceBankDetailsTODT["areaName"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.AreaName = Convert.ToString(tblInvoiceBankDetailsTODT ["areaName"].ToString());
                    if (tblInvoiceBankDetailsTODT ["villageName"] != DBNull.Value)
                        tblInvoiceBankDetailsTONew.VillageName = Convert.ToString(tblInvoiceBankDetailsTODT ["villageName"].ToString());
                    tblInvoiceBankDetailsTOList.Add(tblInvoiceBankDetailsTONew);
                }
            }
            return tblInvoiceBankDetailsTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceBankDetailsTO, cmdInsert);
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

        public static int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceBankDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblInvoiceBankDetails]( " + 
            " [orgId]" +
            " ,[talukaId]" +
            " ,[districtId]" +
            " ,[stateId]" +
            " ,[countryId]" +
            " ,[pincode]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[bankName]" +
            " ,[accountNo]" +
            " ,[ifscCode]" +
            " ,[branch]" +
            " ,[areaName]" +
            " ,[villageName]" +
            " )" +
" VALUES (" +
            " @OrgId " +
            " ,@TalukaId " +
            " ,@DistrictId " +
            " ,@StateId " +
            " ,@CountryId " +
            " ,@Pincode " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@BankName " +
            " ,@AccountNo " +
            " ,@IfscCode " +
            " ,@Branch " +
            " ,@AreaName " +
            " ,@VillageName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBank", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.IdBank;
            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.OrgId;
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.TalukaId;
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.DistrictId;
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.StateId;
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.CountryId;
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.Pincode;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceBankDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@BankName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.BankName;
            cmdInsert.Parameters.Add("@AccountNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.AccountNo;
            cmdInsert.Parameters.Add("@IfscCode", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.IfscCode;
            cmdInsert.Parameters.Add("@Branch", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.Branch;
            cmdInsert.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.AreaName;
            cmdInsert.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.VillageName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceBankDetailsTO, cmdUpdate);
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

        public static int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceBankDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblInvoiceBankDetails] SET " + 
            
            " [orgId]= @OrgId" +
            " ,[talukaId]= @TalukaId" +
            " ,[districtId]= @DistrictId" +
            " ,[stateId]= @StateId" +
            " ,[countryId]= @CountryId" +
            " ,[pincode]= @Pincode" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[bankName]= @BankName" +
            " ,[accountNo]= @AccountNo" +
            " ,[ifscCode]= @IfscCode" +
            " ,[branch]= @Branch" +
            " ,[areaName]= @AreaName" +
            " ,[villageName] = @VillageName" +
            " WHERE [idBank] = @IdBank" ; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBank", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.IdBank;
            cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.OrgId;
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.TalukaId;
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.DistrictId;
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.StateId;
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.CountryId;
            cmdUpdate.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.Pincode;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceBankDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceBankDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@BankName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.BankName;
            cmdUpdate.Parameters.Add("@AccountNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.AccountNo;
            cmdUpdate.Parameters.Add("@IfscCode", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.IfscCode;
            cmdUpdate.Parameters.Add("@Branch", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.Branch;
            cmdUpdate.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.AreaName;
            cmdUpdate.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = tblInvoiceBankDetailsTO.VillageName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceBankDetails(Int32 idBank)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBank, cmdDelete);
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

        public static int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBank, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idBank, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblInvoiceBankDetails] " +
             " WHERE idBank = " + idBank + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
