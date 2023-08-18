using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblItemBroadCategoriesDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblItemBroadCategories]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories()
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
                List<TblItemBroadCategoriesTO> list = ConvertDTToList(reader);
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

        public static TblItemBroadCategoriesTO SelectTblItemBroadCategories(Int32 iditemBroadCategories)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE iditemBroadCategories = " + iditemBroadCategories + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemBroadCategoriesTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else
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

        public static List<TblItemBroadCategoriesTO> SelectAllTblItemBroadCategories(SqlConnection conn, SqlTransaction tran)
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
                List<TblItemBroadCategoriesTO> list = ConvertDTToList(reader);
                return list;
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
        public static int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblItemBroadCategoriesTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public static int InsertTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblItemBroadCategoriesTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblItemBroadCategories]( " +
            //"  [iditemBroadCategories]" +
            " [isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[name]" +
            " ,[description]" +
            " ,[remark]" +
            " )" +
" VALUES (" +
            //"  @IditemBroadCategories " +
            " @IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Name " +
            " ,@Description " +
            " ,@Remark " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IditemBroadCategories", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.IdItemBroadCategories;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemBroadCategoriesTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblItemBroadCategoriesTO.UpdatedOn;
            cmdInsert.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Name;
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Description;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Remark;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblItemBroadCategoriesTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public static int UpdateTblItemBroadCategories(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblItemBroadCategoriesTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblItemBroadCategoriesTO tblItemBroadCategoriesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblItemBroadCategories] SET " +
            //"  [iditemBroadCategories] = @IditemBroadCategories" +
            " [isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[name]= @Name" +
            " ,[description]= @Description" +
            " ,[remark] = @Remark" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IditemBroadCategories", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.IdItemBroadCategories;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemBroadCategoriesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblItemBroadCategoriesTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Name;
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Description;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblItemBroadCategoriesTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblItemBroadCategories(Int32 iditemBroadCategories)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(iditemBroadCategories, cmdDelete);
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

        public static int DeleteTblItemBroadCategories(Int32 iditemBroadCategories, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(iditemBroadCategories, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 iditemBroadCategories, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblItemBroadCategories] " +
            " WHERE iditemBroadCategories = " + iditemBroadCategories + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@iditemBroadCategories", System.Data.SqlDbType.Int).Value = tblItemBroadCategoriesTO.IditemBroadCategories;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblItemBroadCategoriesTO> ConvertDTToList(SqlDataReader tblItemBroadCategoriesTODR)
        {
            List<TblItemBroadCategoriesTO> tblItemBroadCategoriesTOList = new List<TblItemBroadCategoriesTO>();
            if (tblItemBroadCategoriesTODR != null)
            {
                while (tblItemBroadCategoriesTODR.Read())
                {
                    TblItemBroadCategoriesTO tblItemBroadCategoriesTONew = new TblItemBroadCategoriesTO();
                    if (tblItemBroadCategoriesTODR["iditemBroadCategories"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.IdItemBroadCategories = Convert.ToInt32(tblItemBroadCategoriesTODR["iditemBroadCategories"].ToString());
                    if (tblItemBroadCategoriesTODR["isActive"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.IsActive = Convert.ToInt32(tblItemBroadCategoriesTODR["isActive"].ToString());
                    if (tblItemBroadCategoriesTODR["createdBy"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.CreatedBy = Convert.ToInt32(tblItemBroadCategoriesTODR["createdBy"].ToString());
                    if (tblItemBroadCategoriesTODR["updatedBy"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.UpdatedBy = Convert.ToInt32(tblItemBroadCategoriesTODR["updatedBy"].ToString());
                    if (tblItemBroadCategoriesTODR["createdOn"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.CreatedOn = Convert.ToDateTime(tblItemBroadCategoriesTODR["createdOn"].ToString());
                    if (tblItemBroadCategoriesTODR["updatedOn"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.UpdatedOn = Convert.ToDateTime(tblItemBroadCategoriesTODR["updatedOn"].ToString());
                    if (tblItemBroadCategoriesTODR["name"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.Name = Convert.ToString(tblItemBroadCategoriesTODR["name"].ToString());
                    if (tblItemBroadCategoriesTODR["description"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.Description = Convert.ToString(tblItemBroadCategoriesTODR["description"].ToString());
                    if (tblItemBroadCategoriesTODR["remark"] != DBNull.Value)
                        tblItemBroadCategoriesTONew.Remark = Convert.ToString(tblItemBroadCategoriesTODR["remark"].ToString());
                    if (tblItemBroadCategoriesTODR["weightMeasurUnitId"] != DBNull.Value) //Sudhir[23-MAY-2018] Added for Spec WeightMeasureId
                        tblItemBroadCategoriesTONew.WeightMeasurUnitId = Convert.ToInt32(tblItemBroadCategoriesTODR["weightMeasurUnitId"].ToString());
                    tblItemBroadCategoriesTOList.Add(tblItemBroadCategoriesTONew);

                }
            }
            return tblItemBroadCategoriesTOList;
        }

    }
}
