using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblFilterReportDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = "  SELECT tblFilterReport.*,htmlInputTypes.htmlInputTypeName FROM [tblFilterReport] tblFilterReport" +
                                  "  LEFT JOIN dimHtmlInputTypes htmlInputTypes ON htmlInputTypes.idHtmlInputType = tblFilterReport.htmlInputTypeId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblFilterReportTO> SelectAllTblFilterReport()
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
                List<TblFilterReportTO> list = ConvertDTToList(sqlReader);
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

        public static TblFilterReportTO SelectTblFilterReport(Int32 idFilterReport)
        {
            String sqlConnStr =Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idFilterReport = " + idFilterReport + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFilterReportTO> list = ConvertDTToList(sqlReader);
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

        //
        public static List<TblFilterReportTO> SelectTblFilterReportList(Int32 reportId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE reportId = " + reportId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFilterReportTO> list = ConvertDTToList(sqlReader);
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
        public static List<TblFilterReportTO> SelectAllTblFilterReport(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFilterReportTO> list = ConvertDTToList(sqlReader);
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
        public static int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblFilterReportTO, cmdInsert);
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

        public static int InsertTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblFilterReportTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblFilterReportTO tblFilterReportTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblFilterReport]( " +
            //"  [idFilterReport]" +
            " [reportId]" +
            " ,[isRequired]" +
            " ,[filterName]" +
            " ,[inputType]" +
            " ,[sourceApiName]" +
            " ,[destinationApiName]" +
            " ,[placeHolder]" +
            " ,[idHtml]" +
            " )" +
" VALUES (" +
            //"  @IdFilterReport " +
            " @ReportId " +
            " ,@IsRequired " +
            " ,@FilterName " +
            " ,@InputType " +
            " ,@SourceApiName " +
            " ,@DestinationApiName " +
            " ,@PlaceHolder " +
            " ,@IdHtml " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdFilterReport", System.Data.SqlDbType.Int).Value = tblFilterReportTO.IdFilterReport;
            cmdInsert.Parameters.Add("@ReportId", System.Data.SqlDbType.Int).Value = tblFilterReportTO.ReportId;
            cmdInsert.Parameters.Add("@IsRequired", System.Data.SqlDbType.Int).Value = tblFilterReportTO.IsRequired;
            cmdInsert.Parameters.Add("@FilterName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.FilterName;
            cmdInsert.Parameters.Add("@InputType", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.InputType;
            cmdInsert.Parameters.Add("@SourceApiName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.SourceApiName;
            cmdInsert.Parameters.Add("@DestinationApiName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.DestinationApiName;
            cmdInsert.Parameters.Add("@PlaceHolder", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.PlaceHolder;
            cmdInsert.Parameters.Add("@IdHtml", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.IdHtml;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblFilterReportTO, cmdUpdate);
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

        public static int UpdateTblFilterReport(TblFilterReportTO tblFilterReportTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblFilterReportTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblFilterReportTO tblFilterReportTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblFilterReport] SET " +
            "  [idFilterReport] = @IdFilterReport" +
            " ,[reportId]= @ReportId" +
            " ,[isRequired]= @IsRequired" +
            " ,[filterName]= @FilterName" +
            " ,[inputType]= @InputType" +
            " ,[sourceApiName]= @SourceApiName" +
            " ,[destinationApiName]= @DestinationApiName" +
            " ,[placeHolder]= @PlaceHolder" +
            " ,[idHtml] = @IdHtml" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdFilterReport", System.Data.SqlDbType.Int).Value = tblFilterReportTO.IdFilterReport;
            cmdUpdate.Parameters.Add("@ReportId", System.Data.SqlDbType.Int).Value = tblFilterReportTO.ReportId;
            cmdUpdate.Parameters.Add("@IsRequired", System.Data.SqlDbType.Int).Value = tblFilterReportTO.IsRequired;
            cmdUpdate.Parameters.Add("@FilterName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.FilterName;
            cmdUpdate.Parameters.Add("@InputType", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.InputType;
            cmdUpdate.Parameters.Add("@SourceApiName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.SourceApiName;
            cmdUpdate.Parameters.Add("@DestinationApiName", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.DestinationApiName;
            cmdUpdate.Parameters.Add("@PlaceHolder", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.PlaceHolder;
            cmdUpdate.Parameters.Add("@IdHtml", System.Data.SqlDbType.NVarChar).Value = tblFilterReportTO.IdHtml;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblFilterReport(Int32 idFilterReport)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idFilterReport, cmdDelete);
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

        public static int DeleteTblFilterReport(Int32 idFilterReport, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idFilterReport, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idFilterReport, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblFilterReport] " +
            " WHERE idFilterReport = " + idFilterReport + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idFilterReport", System.Data.SqlDbType.Int).Value = tblFilterReportTO.IdFilterReport;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblFilterReportTO> ConvertDTToList(SqlDataReader tblFilterReportTODT)
        {
            List<TblFilterReportTO> tblFilterReportTOList = new List<TblFilterReportTO>();
            if (tblFilterReportTODT != null)
            {
                while(tblFilterReportTODT.Read())
                {
                    TblFilterReportTO tblFilterReportTONew = new TblFilterReportTO();
                    if (tblFilterReportTODT["idFilterReport"] != DBNull.Value)
                        tblFilterReportTONew.IdFilterReport = Convert.ToInt32(tblFilterReportTODT["idFilterReport"].ToString());
                    if (tblFilterReportTODT["reportId"] != DBNull.Value)
                        tblFilterReportTONew.ReportId = Convert.ToInt32(tblFilterReportTODT["reportId"].ToString());
                    if (tblFilterReportTODT["isRequired"] != DBNull.Value)
                        tblFilterReportTONew.IsRequired = Convert.ToInt32(tblFilterReportTODT["isRequired"].ToString());
                    if (tblFilterReportTODT["filterName"] != DBNull.Value)
                        tblFilterReportTONew.FilterName = Convert.ToString(tblFilterReportTODT["filterName"].ToString());
                    if (tblFilterReportTODT["inputType"] != DBNull.Value)
                        tblFilterReportTONew.InputType = Convert.ToString(tblFilterReportTODT["inputType"].ToString());
                    if (tblFilterReportTODT["sourceApiName"] != DBNull.Value)
                        tblFilterReportTONew.SourceApiName = Convert.ToString(tblFilterReportTODT["sourceApiName"].ToString());
                    if (tblFilterReportTODT["destinationApiName"] != DBNull.Value)
                        tblFilterReportTONew.DestinationApiName = Convert.ToString(tblFilterReportTODT["destinationApiName"].ToString());
                    if (tblFilterReportTODT["placeHolder"] != DBNull.Value)
                        tblFilterReportTONew.PlaceHolder = Convert.ToString(tblFilterReportTODT["placeHolder"].ToString());
                    if (tblFilterReportTODT["idHtml"] != DBNull.Value)
                        tblFilterReportTONew.IdHtml = Convert.ToString(tblFilterReportTODT["idHtml"].ToString());
                    if (tblFilterReportTODT["htmlInputTypeId"] != DBNull.Value)
                        tblFilterReportTONew.HtmlInputTypeId = Convert.ToInt32(tblFilterReportTODT["htmlInputTypeId"].ToString());
                    if (tblFilterReportTODT["htmlInputTypeName"] != DBNull.Value)
                        tblFilterReportTONew.HtmlInputTypeName = Convert.ToString(tblFilterReportTODT["htmlInputTypeName"].ToString());
                    if (tblFilterReportTODT["sourceApiModuleId"] != DBNull.Value)
                        tblFilterReportTONew.SourceApiModuleId= Convert.ToInt32(tblFilterReportTODT["sourceApiModuleId"].ToString());
                    if (tblFilterReportTODT["orderArguments"] != DBNull.Value)
                        tblFilterReportTONew.OrderArguments = Convert.ToInt32(tblFilterReportTODT["orderArguments"].ToString());
                    if (tblFilterReportTODT["parentControlId"] != DBNull.Value)
                        tblFilterReportTONew.ParentControlId = Convert.ToInt32(tblFilterReportTODT["parentControlId"].ToString());
                    if (tblFilterReportTODT["SqlParameterName"] != DBNull.Value)
                        tblFilterReportTONew.SqlParameterName= Convert.ToString(tblFilterReportTODT["sqlParameterName"].ToString());
                    if (tblFilterReportTODT["sqlDbTypeValue"] != DBNull.Value)
                        tblFilterReportTONew.SqlDbTypeValue = Convert.ToInt32(tblFilterReportTODT["sqlDbTypeValue"].ToString());
                    if (tblFilterReportTODT["whereClause"] != DBNull.Value)
                        tblFilterReportTONew.WhereClause = Convert.ToString(tblFilterReportTODT["whereClause"].ToString());
                    if (tblFilterReportTODT["isOptional"] != DBNull.Value)
                        tblFilterReportTONew.IsOptional = Convert.ToInt32(tblFilterReportTODT["isOptional"].ToString());
                    



                    tblFilterReportTOList.Add(tblFilterReportTONew);
                }
            }
            return tblFilterReportTOList;
        }
    }
}
