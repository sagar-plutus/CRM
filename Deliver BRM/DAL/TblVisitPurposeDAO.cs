using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;


namespace SalesTrackerAPI.DAL
{
    public class TblVisitPurposeDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitPurpose] WHERE isActive = 1 ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVisitPurposeTO> SelectAllTblVisitPurpose()
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


                SqlDataReader visitPurposeDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitPurposeTO> list = ConvertDTToList(visitPurposeDT);
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPurpose");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DataTable SelectTblVisitPurpose(Int32 idVisitPurpose)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisitPurpose = " + idVisitPurpose + " ";
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

        public static DataTable SelectAllTblVisitPurpose(SqlConnection conn, SqlTransaction tran)
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

        // Vaibhav [2-Oct-2017] added to select visit purpose list
        public static List<DropDownTO> SelectVisitPurposeListForDropDown(int visitTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND visitTypeId =" + visitTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitPurposeTO = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (visitPurposeTO.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (visitPurposeTO["idVisitPurpose"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(visitPurposeTO["idVisitPurpose"].ToString());
                    if (visitPurposeTO["visitPurposeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(visitPurposeTO["visitPurposeDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitPurposeList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitPurposeTO> ConvertDTToList(SqlDataReader visitPurposeDT)
        {
            List<TblVisitPurposeTO> visitPurposeTOList = new List<TblVisitPurposeTO>();
            if (visitPurposeDT != null)
            {
                while (visitPurposeDT.Read())
                {
                    TblVisitPurposeTO visitPurposeTONew = new TblVisitPurposeTO();
                    if (visitPurposeDT["idVisitPurpose"] != DBNull.Value)
                        visitPurposeTONew.IdVisitPurpose = Convert.ToInt32(visitPurposeDT["idVisitPurpose"].ToString());
                    if (visitPurposeDT["visitPurposeDesc"] != DBNull.Value)
                        visitPurposeTONew.VisitPurposeDesc = visitPurposeDT["visitPurposeDesc"].ToString();
                    if (visitPurposeDT["visitTypeId"] != DBNull.Value)
                        visitPurposeTONew.VisitTypeId = Convert.ToInt32(visitPurposeDT["visitTypeId"].ToString());
                    if (visitPurposeDT["createdBy"] != DBNull.Value)
                        visitPurposeTONew.CreatedBy = Convert.ToInt32(visitPurposeDT["createdBy"].ToString());
                    if (visitPurposeDT["createdOn"] != DBNull.Value)
                        visitPurposeTONew.CreatedOn = Convert.ToDateTime(visitPurposeDT["createdOn"].ToString());
                    if (visitPurposeDT["updatedBy"] != DBNull.Value)
                        visitPurposeTONew.UpdatedBy = Convert.ToInt32(visitPurposeDT["updatedBy"].ToString());
                    if (visitPurposeDT["updatedOn"] != DBNull.Value)
                        visitPurposeTONew.UpdatedOn = Convert.ToDateTime(visitPurposeDT["updatedOn"].ToString());
                    if (visitPurposeDT["isActive"] != DBNull.Value)
                        visitPurposeTONew.IsActive = Convert.ToInt32(visitPurposeDT["isActive"].ToString());
                    visitPurposeTOList.Add(visitPurposeTONew);
                }
            }
            return visitPurposeTOList;
        }
        #endregion

        #region Insertion
        public static int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(ref tblVisitPurposeTO, cmdInsert);
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

        public static int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(ref tblVisitPurposeTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitPurpose");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(ref TblVisitPurposeTO tblVisitPurposeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitPurpose]( " +
            " [visitTypeId]" +
            "  ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[visitPurposeDesc]" +
            " )" +
" VALUES (" +
            " @VisitTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VisitPurposeDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@VisitTypeId", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.VisitTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitPurposeTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitPurposeTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblVisitPurposeTO.UpdatedOn);
            cmdInsert.Parameters.Add("@VisitPurposeDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitPurposeTO.VisitPurposeDesc;
            //return cmdInsert.ExecuteNonQuery();

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblVisitPurposeTO.IdVisitPurpose = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
            return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitPurposeTO, cmdUpdate);
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

        public static int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitPurposeTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblVisitPurposeTO tblVisitPurposeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitPurpose] SET " +
            "  [idVisitPurpose] = @IdVisitPurpose" +
            " ,[visitTypeId]= @VisitTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[visitPurposeDesc] = @VisitPurposeDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisitPurpose", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.IdVisitPurpose;
            cmdUpdate.Parameters.Add("@VisitTypeId", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.VisitTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitPurposeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblVisitPurposeTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@VisitPurposeDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitPurposeTO.VisitPurposeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitPurpose(Int32 idVisitPurpose)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisitPurpose, cmdDelete);
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

        public static int DeleteTblVisitPurpose(Int32 idVisitPurpose, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisitPurpose, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idVisitPurpose, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitPurpose] " +
            " WHERE idVisitPurpose = " + idVisitPurpose + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisitPurpose", System.Data.SqlDbType.Int).Value = tblVisitPurposeTO.IdVisitPurpose;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
