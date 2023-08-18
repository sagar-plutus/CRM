using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblReportsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblReports]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblReportsTO> SelectAllTblReports()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
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

        public static TblReportsTO SelectTblReports(Int32 idReports)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idReports = " + idReports + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
                return list[0];
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

        public static List<TblReportsTO> SelectAllTblReports(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
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
        public static int InsertTblReports(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblReportsTO, cmdInsert);
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

        public static int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblReportsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblReportsTO tblReportsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblReports]( " +
            //"  [idReports]" +
            " [moduleId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[reportName]" +
            " ,[apiName]" +
            " ,[sqlQuery]" +
            " )" +
" VALUES (" +
            //"  @IdReports " +
            " @ModuleId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@ReportName " +
            " ,@ApiName " +
            " ,@SqlQuery " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblReportsTO.ModuleId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblReportsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ReportName;
            cmdInsert.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ApiName;
            cmdInsert.Parameters.Add("@SqlQuery", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.SqlQuery;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblReports(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblReportsTO, cmdUpdate);
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

        public static int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblReportsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblReportsTO tblReportsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblReports] SET " +
            "  [idReports] = @IdReports" +
            " ,[moduleId]= @ModuleId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[reportName]= @ReportName" +
            " ,[apiName]= @ApiName" +
            " ,[sqlQuery] = @SqlQuery" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblReportsTO.ModuleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblReportsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ReportName;
            cmdUpdate.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ApiName;
            cmdUpdate.Parameters.Add("@SqlQuery", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.SqlQuery;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblReports(Int32 idReports)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReports, cmdDelete);
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

        public static int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReports, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idReports, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblReports] " +
            " WHERE idReports = " + idReports + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblReportsTO> ConvertDTToList(SqlDataReader tblReportsTODT)
        {
            List<TblReportsTO> tblReportsTOList = new List<TblReportsTO>();
            if (tblReportsTODT != null)
            {
                while (tblReportsTODT.Read())
                {
                    TblReportsTO tblReportsTONew = new TblReportsTO();
                    if (tblReportsTODT["idReports"] != DBNull.Value)
                        tblReportsTONew.IdReports = Convert.ToInt32(tblReportsTODT["idReports"].ToString());
                    if (tblReportsTODT["moduleId"] != DBNull.Value)
                        tblReportsTONew.ModuleId = Convert.ToInt32(tblReportsTODT["moduleId"].ToString());
                    if (tblReportsTODT["isActive"] != DBNull.Value)
                        tblReportsTONew.IsActive = Convert.ToInt32(tblReportsTODT["isActive"].ToString());
                    if (tblReportsTODT["createdBy"] != DBNull.Value)
                        tblReportsTONew.CreatedBy = Convert.ToInt32(tblReportsTODT["createdBy"].ToString());
                    if (tblReportsTODT["updatedBy"] != DBNull.Value)
                        tblReportsTONew.UpdatedBy = Convert.ToInt32(tblReportsTODT["updatedBy"].ToString());
                    if (tblReportsTODT["createdOn"] != DBNull.Value)
                        tblReportsTONew.CreatedOn = Convert.ToDateTime(tblReportsTODT["createdOn"].ToString());
                    if (tblReportsTODT["updatedOn"] != DBNull.Value)
                        tblReportsTONew.UpdatedOn = Convert.ToDateTime(tblReportsTODT["updatedOn"].ToString());
                    if (tblReportsTODT["reportName"] != DBNull.Value)
                        tblReportsTONew.ReportName = Convert.ToString(tblReportsTODT["reportName"].ToString());
                    if (tblReportsTODT["apiName"] != DBNull.Value)
                        tblReportsTONew.ApiName = Convert.ToString(tblReportsTODT["apiName"].ToString());
                    if (tblReportsTODT["sqlQuery"] != DBNull.Value)
                        tblReportsTONew.SqlQuery = Convert.ToString(tblReportsTODT["sqlQuery"].ToString());
                    if (tblReportsTODT["whereClause"] != DBNull.Value)
                        tblReportsTONew.WhereClause = Convert.ToString(tblReportsTODT["whereClause"].ToString());
                    
                    tblReportsTOList.Add(tblReportsTONew);
                }
            }
            return tblReportsTOList;
        }



        public static List<DynamicReportTO> GetDynamicSqlData(string connectionstring, string sql)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand comm = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                SqlDataReader sqlReader  = comm.ExecuteReader();
                return SqlDataReaderToList(sqlReader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                comm.Dispose();
                conn.Close();

            }
        }

        public static List<DynamicReportTO> SqlDataReaderToList(SqlDataReader reader)
        {
            try
            {
                List<DynamicReportTO> list = new List<DynamicReportTO>();

                while (reader.Read())
                {
                    DynamicReportTO dynamicReportTO = new DynamicReportTO();
                    List<DropDownTO> dropDownList = new List<DropDownTO>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = reader.GetName(i);
                        dropDownTO.Tag = reader[i];
                        dropDownList.Add(dropDownTO);
                    }
                    dynamicReportTO.DropDownList = dropDownList;
                    list.Add(dynamicReportTO);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
