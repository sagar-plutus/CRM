using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblPaymentTermDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentTerm]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblPaymentTerm()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        // Vaibhav [3-Oct-2017] added to select payment terms for drop down
        public static List<DropDownTO> SelecPaymentTermForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader paymenttermDT= cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (paymenttermDT.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (paymenttermDT["idPaymentTerm"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(paymenttermDT["idPaymentTerm"].ToString());
                    if (paymenttermDT["paymentTermDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(paymenttermDT["paymentTermDisplayName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;                
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelecPaymentTermForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectTblPaymentTerm(Int32 idPaymentTerm)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPaymentTerm = " + idPaymentTerm + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        public static DataTable SelectAllTblPaymentTerm(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public static int InsertTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentTermTO, cmdInsert);
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

        public static int InsertTblPaymentTerm(ref TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentTermTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPaymentTerm");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblPaymentTermTO tblPaymentTermTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentTerm]( " +
            //"  [idPaymentTerm]" +
            " [createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[paymentTermDisplayName]" +
            " ,[paymentTermDesc]" +
            " )" +
            " VALUES (" +
            //"  @IdPaymentTerm " +
            " @CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@PaymentTermDisplayName " +
            " ,@PaymentTermDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.IdPaymentTerm;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblPaymentTermTO.IsActive;
            cmdInsert.Parameters.Add("@PaymentTermDisplayName", System.Data.SqlDbType.NVarChar).Value = tblPaymentTermTO.PaymentTermDisplayName;
            cmdInsert.Parameters.Add("@PaymentTermDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPaymentTermTO.PaymentTermDesc);
            //return cmdInsert.ExecuteNonQuery();

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPaymentTermTO.IdPaymentTerm = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else         
            return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentTermTO, cmdUpdate);
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

        public static int UpdateTblPaymentTerm(TblPaymentTermTO tblPaymentTermTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentTermTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblPaymentTermTO tblPaymentTermTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentTerm] SET " +
            "  [idPaymentTerm] = @IdPaymentTerm" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[paymentTermCode]= @PaymentTermCode" +
            " ,[paymentTermDisplayName]= @PaymentTermDisplayName" +
            " ,[paymentTermDesc] = @PaymentTermDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.IdPaymentTerm;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentTermTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblPaymentTermTO.IsActive;
            //cmdUpdate.Parameters.Add("@PaymentTermCode", System.Data.SqlDbType.NVarChar).Value = tblPaymentTermTO.PaymentTermCode;
            cmdUpdate.Parameters.Add("@PaymentTermDisplayName", System.Data.SqlDbType.NVarChar).Value = tblPaymentTermTO.PaymentTermDisplayName;
            cmdUpdate.Parameters.Add("@PaymentTermDesc", System.Data.SqlDbType.NVarChar).Value = tblPaymentTermTO.PaymentTermDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblPaymentTerm(Int32 idPaymentTerm)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPaymentTerm, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblPaymentTerm(Int32 idPaymentTerm, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPaymentTerm, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idPaymentTerm, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentTerm] " +
            " WHERE idPaymentTerm = " + idPaymentTerm + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPaymentTerm", System.Data.SqlDbType.Int).Value = tblPaymentTermTO.IdPaymentTerm;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
