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
    public class TblInvoiceOtherDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblInvoiceOtherDetails]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails()
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
                List<TblInvoiceOtherDetailsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idInvoiceOtherDetails = " + idInvoiceOtherDetails;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceOtherDetailsTO> list = ConvertDTToList(reader);
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

        public static List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reader = null;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceOtherDetailsTO> list = ConvertDTToList(reader);
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

        public static List<TblInvoiceOtherDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceOtherDetailsTODT)
        {
            List<TblInvoiceOtherDetailsTO> tblInvoiceOtherDetailsTOList = new List<TblInvoiceOtherDetailsTO>();
            if (tblInvoiceOtherDetailsTODT != null)
            {
                while (tblInvoiceOtherDetailsTODT.Read())
                {
                    TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTONew = new TblInvoiceOtherDetailsTO();
                    if (tblInvoiceOtherDetailsTODT["idInvoiceOtherDetails"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.IdInvoiceOtherDetails = Convert.ToInt32(tblInvoiceOtherDetailsTODT["idInvoiceOtherDetails"].ToString());
                    if (tblInvoiceOtherDetailsTODT["orgId"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.OrgId = Convert.ToInt32(tblInvoiceOtherDetailsTODT["orgId"].ToString());
                    if (tblInvoiceOtherDetailsTODT["createdBy"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.CreatedBy = Convert.ToInt32(tblInvoiceOtherDetailsTODT["createdBy"].ToString());
                    if (tblInvoiceOtherDetailsTODT["createdOn"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.CreatedOn = Convert.ToDateTime(tblInvoiceOtherDetailsTODT["createdOn"].ToString());
                    if (tblInvoiceOtherDetailsTODT["description"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.Description = Convert.ToString(tblInvoiceOtherDetailsTODT["description"].ToString());
                    if (tblInvoiceOtherDetailsTODT["detailTypeId"] != DBNull.Value)
                        tblInvoiceOtherDetailsTONew.DetailTypeId = Convert.ToInt32(tblInvoiceOtherDetailsTODT["detailTypeId"].ToString());
                    tblInvoiceOtherDetailsTOList.Add(tblInvoiceOtherDetailsTONew);
                }
            }
            return tblInvoiceOtherDetailsTOList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tblInvoiceOtherDetailsTO"></param>
        /// <returns></returns>
        public static List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
        SqlConnection conn = new SqlConnection(sqlConnStr);
        SqlCommand cmdSelect = new SqlCommand();
        SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgId = " + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceOtherDetailsTO> list = ConvertDTToList(reader);
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
        #endregion

        #region Insertion
        public static int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceOtherDetailsTO, cmdInsert);
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

        public static int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceOtherDetailsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblInvoiceOtherDetails]( " + 
            "  [orgId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[description]" +
            " ,[detailTypeId]" +
            " )" +
" VALUES (" +
            "  @OrgId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@Description " +
            " ,@DetailTypeId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoiceOtherDetails", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.IdInvoiceOtherDetails;
            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.OrgId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceOtherDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblInvoiceOtherDetailsTO.Description;
            cmdInsert.Parameters.Add("@DetailTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.DetailTypeId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public static int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceOtherDetailsTO, cmdUpdate);
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

        public static int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceOtherDetailsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblInvoiceOtherDetails] SET " + 
            "  [orgId]= @OrgId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[description]= @Description" +
            " ,[detailTypeId] = @DetailTypeId" +
            " WHERE   [idInvoiceOtherDetails] = @IdInvoiceOtherDetails" ; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoiceOtherDetails", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.IdInvoiceOtherDetails;
            cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.OrgId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceOtherDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblInvoiceOtherDetailsTO.Description;
            cmdUpdate.Parameters.Add("@DetailTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceOtherDetailsTO.DetailTypeId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoiceOtherDetails, cmdDelete);
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

        public static int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoiceOtherDetails, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idInvoiceOtherDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblInvoiceOtherDetails] " +
           " WHERE idInvoiceOtherDetails = " + idInvoiceOtherDetails + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
