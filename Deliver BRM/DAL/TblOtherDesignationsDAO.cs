using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblOtherDesignationsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOtherDesignations]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblOtherDesignationsTO> SelectAllTblOtherDesignations()
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

                //cmdSelect.Parameters.Add("@idOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherDesignationsTO> list = ConvertDTToList(reader);
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

        public static TblOtherDesignationsTO SelectTblOtherDesignations(Int32 idOtherDesignation)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOtherDesignation = " + idOtherDesignation + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherDesignationsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
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

        public static List<TblOtherDesignationsTO> SelectAllTblOtherDesignations(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherDesignationsTO> list = ConvertDTToList(reader);
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
        public static int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOtherDesignationsTO, cmdInsert);
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

        public static int InsertTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOtherDesignationsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblOtherDesignationsTO tblOtherDesignationsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOtherDesignations]( " +
            "  [idOtherDesignation]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[name]" +
            " ,[designationDesc]" +
            " ,[remark]" +
            " )" +
" VALUES (" +
            "  @IdOtherDesignation " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Name " +
            " ,@DesignationDesc " +
            " ,@Remark " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherDesignationsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherDesignationsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.Name;
            cmdInsert.Parameters.Add("@DesignationDesc", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.DesignationDesc;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.Remark;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOtherDesignationsTO, cmdUpdate);
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

        public static int UpdateTblOtherDesignations(TblOtherDesignationsTO tblOtherDesignationsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOtherDesignationsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblOtherDesignationsTO tblOtherDesignationsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOtherDesignations] SET " +
            "  [idOtherDesignation] = @IdOtherDesignation" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[name]= @Name" +
            " ,[designationDesc]= @DesignationDesc" +
            " ,[remark] = @Remark" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherDesignationsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherDesignationsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.Name;
            cmdUpdate.Parameters.Add("@DesignationDesc", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.DesignationDesc;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblOtherDesignationsTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblOtherDesignations(Int32 idOtherDesignation)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOtherDesignation, cmdDelete);
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

        public static int DeleteTblOtherDesignations(Int32 idOtherDesignation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOtherDesignation, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idOtherDesignation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOtherDesignations] " +
            " WHERE idOtherDesignation = " + idOtherDesignation + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOtherDesignation", System.Data.SqlDbType.Int).Value = tblOtherDesignationsTO.IdOtherDesignation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblOtherDesignationsTO> ConvertDTToList(SqlDataReader tblOtherDesignationsTODT)
        {
            List<TblOtherDesignationsTO> tblOtherDesignationsTOList = new List<TblOtherDesignationsTO>();
            if (tblOtherDesignationsTODT != null)
            {
                while (tblOtherDesignationsTODT.Read())
                {
                    TblOtherDesignationsTO tblOtherDesignationsTONew = new TblOtherDesignationsTO();
                    if (tblOtherDesignationsTODT["idOtherDesignation"] != DBNull.Value)
                        tblOtherDesignationsTONew.IdOtherDesignation = Convert.ToInt32(tblOtherDesignationsTODT["idOtherDesignation"].ToString());
                    if (tblOtherDesignationsTODT["createdBy"] != DBNull.Value)
                        tblOtherDesignationsTONew.CreatedBy = Convert.ToInt32(tblOtherDesignationsTODT["createdBy"].ToString());
                    if (tblOtherDesignationsTODT["updatedBy"] != DBNull.Value)
                        tblOtherDesignationsTONew.UpdatedBy = Convert.ToInt32(tblOtherDesignationsTODT["updatedBy"].ToString());
                    if (tblOtherDesignationsTODT["createdOn"] != DBNull.Value)
                        tblOtherDesignationsTONew.CreatedOn = Convert.ToDateTime(tblOtherDesignationsTODT["createdOn"].ToString());
                    if (tblOtherDesignationsTODT["updatedOn"] != DBNull.Value)
                        tblOtherDesignationsTONew.UpdatedOn = Convert.ToDateTime(tblOtherDesignationsTODT["updatedOn"].ToString());
                    if (tblOtherDesignationsTODT["name"] != DBNull.Value)
                        tblOtherDesignationsTONew.Name = Convert.ToString(tblOtherDesignationsTODT["name"].ToString());
                    if (tblOtherDesignationsTODT["designationDesc"] != DBNull.Value)
                        tblOtherDesignationsTONew.DesignationDesc = Convert.ToString(tblOtherDesignationsTODT["designationDesc"].ToString());
                    if (tblOtherDesignationsTODT["remark"] != DBNull.Value)
                        tblOtherDesignationsTONew.Remark = Convert.ToString(tblOtherDesignationsTODT["remark"].ToString());
                    tblOtherDesignationsTOList.Add(tblOtherDesignationsTONew);
                }
            }
            return tblOtherDesignationsTOList;
        }

    }
}
