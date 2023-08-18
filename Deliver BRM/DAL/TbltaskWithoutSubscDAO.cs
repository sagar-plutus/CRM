using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TbltaskWithoutSubscDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tbltaskWithoutSubsc]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TbltaskWithoutSubscTO SelectTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idTaskWithoutSubsc = " + idTaskWithoutSubsc + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TbltaskWithoutSubscTO> taskWithoutSubcList= ConvertDTToList(sqlReader);
                if (taskWithoutSubcList != null)
                    return taskWithoutSubcList[0];
                else
                    return new TbltaskWithoutSubscTO();
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

        public static List<TbltaskWithoutSubscTO> SelectTbltaskWithoutSubscList(Int32 moduleId,Int32 entityId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE entityId = " + entityId + " AND moduleId= "+moduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TbltaskWithoutSubscTO> taskWithoutSubcList = ConvertDTToList(sqlReader);
                if (taskWithoutSubcList != null)
                    return taskWithoutSubcList;
                else
                    return new List<TbltaskWithoutSubscTO>();
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

        public static List<TbltaskWithoutSubscTO> SelectAllTbltaskWithoutSubsc(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTaskWithoutSubsc", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IdTaskWithoutSubsc;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TbltaskWithoutSubscTO> taskWithoutSubcList = ConvertDTToList(sqlReader);
                if (taskWithoutSubcList != null)
                    return taskWithoutSubcList;
                else
                    return new List<TbltaskWithoutSubscTO>();
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
        public static int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tbltaskWithoutSubscTO, cmdInsert);
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

        public static int InsertTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tbltaskWithoutSubscTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlCommand cmdInsert)
        {
            String sqlSelectIdentityQry = "Select @@Identity";
            String sqlQuery = @" INSERT INTO [tbltaskWithoutSubsc]( " +
            //"  [idTaskWithoutSubsc]" +
            " [moduleId]" +
            " ,[entityId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[entityWhen]" +
            " ,[createdOn]" +
            " ,[udatedOn]" +
            " ,[description]" +
            " ,[entityTypeId] " +
            " )" +
" VALUES (" +
            //"  @IdTaskWithoutSubsc " +
            " @ModuleId " +
            " ,@EntityId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@EntityWhen " +
            " ,@CreatedOn " +
            " ,@UdatedOn " +
            " ,@Description " +
            " ,@EntityTypeId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTaskWithoutSubsc", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IdTaskWithoutSubsc;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.ModuleId;
            cmdInsert.Parameters.Add("@EntityId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.EntityId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tbltaskWithoutSubscTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IsActive;
            cmdInsert.Parameters.Add("@EntityWhen", System.Data.SqlDbType.DateTime).Value = tbltaskWithoutSubscTO.EntityWhen;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tbltaskWithoutSubscTO.CreatedOn;
            cmdInsert.Parameters.Add("@UdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tbltaskWithoutSubscTO.UdatedOn);
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tbltaskWithoutSubscTO.Description;
            cmdInsert.Parameters.Add("@EntityTypeId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.EntityTypeId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tbltaskWithoutSubscTO.IdTaskWithoutSubsc = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public static int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tbltaskWithoutSubscTO, cmdUpdate);
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

        public static int UpdateTbltaskWithoutSubsc(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tbltaskWithoutSubscTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TbltaskWithoutSubscTO tbltaskWithoutSubscTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tbltaskWithoutSubsc] SET " +
           // "  [idTaskWithoutSubsc] = @IdTaskWithoutSubsc" +
            " [moduleId]= @ModuleId" +
            " ,[entityId]= @EntityId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[entityWhen]= @EntityWhen" +
            " ,[createdOn]= @CreatedOn" +
            " ,[udatedOn]= @UdatedOn" +
            " ,[description] = @Description" +
            " ,[entityTypeId]=@EntityTypeId" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

           // cmdUpdate.Parameters.Add("@IdTaskWithoutSubsc", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IdTaskWithoutSubsc;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.ModuleId;
            cmdUpdate.Parameters.Add("@EntityId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.EntityId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IsActive;
            cmdUpdate.Parameters.Add("@EntityWhen", System.Data.SqlDbType.DateTime).Value = tbltaskWithoutSubscTO.EntityWhen;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tbltaskWithoutSubscTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UdatedOn", System.Data.SqlDbType.DateTime).Value = tbltaskWithoutSubscTO.UdatedOn;
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tbltaskWithoutSubscTO.Description;
            cmdUpdate.Parameters.Add("@EntityTypeId", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.EntityTypeId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTaskWithoutSubsc, cmdDelete);
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

        public static int DeleteTbltaskWithoutSubsc(Int32 idTaskWithoutSubsc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTaskWithoutSubsc, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idTaskWithoutSubsc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tbltaskWithoutSubsc] " +
            " WHERE idTaskWithoutSubsc = " + idTaskWithoutSubsc + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTaskWithoutSubsc", System.Data.SqlDbType.Int).Value = tbltaskWithoutSubscTO.IdTaskWithoutSubsc;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TbltaskWithoutSubscTO> ConvertDTToList(SqlDataReader tbltaskWithoutSubscTODT)
        {
            List<TbltaskWithoutSubscTO> tbltaskWithoutSubscTOList = new List<TbltaskWithoutSubscTO>();
            if (tbltaskWithoutSubscTODT != null)
            {
                while (tbltaskWithoutSubscTODT.Read())
                {
                    TbltaskWithoutSubscTO tbltaskWithoutSubscTONew = new TbltaskWithoutSubscTO();
                    if (tbltaskWithoutSubscTODT["idTaskWithoutSubsc"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.IdTaskWithoutSubsc = Convert.ToInt32(tbltaskWithoutSubscTODT["idTaskWithoutSubsc"].ToString());
                    if (tbltaskWithoutSubscTODT["moduleId"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.ModuleId = Convert.ToInt32(tbltaskWithoutSubscTODT["moduleId"].ToString());
                    if (tbltaskWithoutSubscTODT["entityId"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.EntityId = Convert.ToInt32(tbltaskWithoutSubscTODT["entityId"].ToString());
                    if (tbltaskWithoutSubscTODT["createdBy"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.CreatedBy = Convert.ToInt32(tbltaskWithoutSubscTODT["createdBy"].ToString());
                    if (tbltaskWithoutSubscTODT["updatedBy"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.UpdatedBy = Convert.ToInt32(tbltaskWithoutSubscTODT["updatedBy"].ToString());
                    if (tbltaskWithoutSubscTODT["isActive"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.IsActive = Convert.ToInt32(tbltaskWithoutSubscTODT["isActive"].ToString());
                    if (tbltaskWithoutSubscTODT["entityWhen"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.EntityWhen = Convert.ToDateTime(tbltaskWithoutSubscTODT["entityWhen"].ToString());
                    if (tbltaskWithoutSubscTODT["createdOn"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.CreatedOn = Convert.ToDateTime(tbltaskWithoutSubscTODT["createdOn"].ToString());
                    if (tbltaskWithoutSubscTODT["udatedOn"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.UdatedOn = Convert.ToDateTime(tbltaskWithoutSubscTODT["udatedOn"].ToString());
                    if (tbltaskWithoutSubscTODT["description"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.Description = Convert.ToString(tbltaskWithoutSubscTODT["description"].ToString());
                    if (tbltaskWithoutSubscTODT["entityTypeId"] != DBNull.Value)
                        tbltaskWithoutSubscTONew.EntityTypeId = Convert.ToInt16(tbltaskWithoutSubscTODT["entityTypeId"].ToString());
                    tbltaskWithoutSubscTOList.Add(tbltaskWithoutSubscTONew);
                }
            }
            return tbltaskWithoutSubscTOList;
        }
    }
}
