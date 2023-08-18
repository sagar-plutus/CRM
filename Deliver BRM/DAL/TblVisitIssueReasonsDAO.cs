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
    public class TblVisitIssueReasonsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVisitIssueReasons]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static DataTable SelectAllTblVisitIssueReasons()
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

        public static DataTable SelectTblVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idVisitIssueReasons = " + idVisitIssueReasons + " ";
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

        public static DataTable SelectAllTblVisitIssueReasons(SqlConnection conn, SqlTransaction tran)
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

        public static List<TblVisitIssueReasonsTO> SelectAllTblVisitIssueReasonsForDropDOwn()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitIssueReasonDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitIssueReasonsTO> list = ConvertDTToList(visitIssueReasonDT);

                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitIssueReasonsForDropDOwn");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblVisitIssueReasonsTO> ConvertDTToList(SqlDataReader visitIssueReasonDT)
        {
            List<TblVisitIssueReasonsTO> visitIssueReasonTOList = new List<TblVisitIssueReasonsTO>();
            if (visitIssueReasonDT != null)
            {
                while (visitIssueReasonDT.Read())
                {
                    TblVisitIssueReasonsTO visitIssueReasonTONew = new TblVisitIssueReasonsTO();
                    if (visitIssueReasonDT["idVisitIssueReasons"] != DBNull.Value)
                        visitIssueReasonTONew.IdVisitIssueReasons = Convert.ToInt32(visitIssueReasonDT["idVisitIssueReasons"].ToString());
                    if (visitIssueReasonDT["issueTypeId"] != DBNull.Value)
                        visitIssueReasonTONew.IssueTypeId = Convert.ToInt32(visitIssueReasonDT["issueTypeId"]);
                    if (visitIssueReasonDT["visitIssueReasonName"] != DBNull.Value)
                        visitIssueReasonTONew.VisitIssueReasonName = visitIssueReasonDT["visitIssueReasonName"].ToString();
                    if (visitIssueReasonDT["visitIssueReasonDesc"] != DBNull.Value)
                        visitIssueReasonTONew.VisitIssueReasonDesc = visitIssueReasonDT["visitIssueReasonDesc"].ToString();             
                    if (visitIssueReasonDT["isActive"] != DBNull.Value)
                        visitIssueReasonTONew.IsActive = Convert.ToInt32(visitIssueReasonDT["isActive"].ToString());
                    visitIssueReasonTOList.Add(visitIssueReasonTONew);
                }
            }
            return visitIssueReasonTOList;
        }


        #endregion

        #region Insertion
        public static int InsertTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(ref tblVisitIssueReasonsTO, cmdInsert);
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

        public static int InsertTblVisitIssueReasons(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(ref tblVisitIssueReasonsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblVisitIssueReasons");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitIssueReasons]( " +
            //"  [idVisitIssueReasons]" +
            " [issueTypeId]" +
            " ,[isActive]" +
            " ,[visitIssueReasonName]" +
            " ,[visitIssueReasonDesc]" +
            " )" +
           " VALUES (" +
            //"  @IdVisitIssueReasons " +
            " @IssueTypeId " +
            " ,@IsActive " +
            " ,@VisitIssueReasonName " +
            " ,@VisitIssueReasonDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdVisitIssueReasons", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IdVisitIssueReasons;
            cmdInsert.Parameters.Add("@IssueTypeId", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IssueTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IsActive;
            cmdInsert.Parameters.Add("@VisitIssueReasonName", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueReasonsTO.VisitIssueReasonName;
            cmdInsert.Parameters.Add("@VisitIssueReasonDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueReasonsTO.VisitIssueReasonDesc;

            //return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblVisitIssueReasonsTO.IdVisitIssueReasons = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return -1;
        }
        #endregion

        #region Updation
        public static int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitIssueReasonsTO, cmdUpdate);
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

        public static int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitIssueReasonsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitIssueReasons] SET " +
            "  [idVisitIssueReasons] = @IdVisitIssueReasons" +
            " ,[issueTypeId]= @IssueTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[visitIssueReasonName]= @VisitIssueReasonName" +
            " ,[visitIssueReasonDesc] = @VisitIssueReasonDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVisitIssueReasons", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IdVisitIssueReasons;
            cmdUpdate.Parameters.Add("@IssueTypeId", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IssueTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IsActive;
            cmdUpdate.Parameters.Add("@VisitIssueReasonName", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueReasonsTO.VisitIssueReasonName;
            cmdUpdate.Parameters.Add("@VisitIssueReasonDesc", System.Data.SqlDbType.NVarChar).Value = tblVisitIssueReasonsTO.VisitIssueReasonDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVisitIssueReasons, cmdDelete);
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

        public static int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVisitIssueReasons, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idVisitIssueReasons, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVisitIssueReasons] " +
            " WHERE idVisitIssueReasons = " + idVisitIssueReasons + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVisitIssueReasons", System.Data.SqlDbType.Int).Value = tblVisitIssueReasonsTO.IdVisitIssueReasons;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
