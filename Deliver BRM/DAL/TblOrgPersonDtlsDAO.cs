using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.DAL
{
    public class TblOrgPersonDtlsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOrgPersonDtls]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls()
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

                //cmdSelect.Parameters.Add("@idOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
                // SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                SqlDataReader dataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgPersonDtlsTO> list = ConvertDTToList(dataReader);
                if (list != null)
                    return list;
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

        public static TblOrgPersonDtlsTO SelectTblOrgPersonDtls(Int32 idOrgPersonDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgPersonDtl = " + idOrgPersonDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
                SqlDataReader dataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgPersonDtlsTO> list = ConvertDTToList(dataReader);
                if (list != null && list.Count==1)
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

        public static List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
                SqlDataReader dataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgPersonDtlsTO> list = ConvertDTToList(dataReader);
                if (list != null)
                    return list;
                else
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
        public static int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgPersonDtlsTO, cmdInsert);
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

        public static int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgPersonDtlsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgPersonDtls]( " +
                                //"  [idOrgPersonDtl]" +
                                " [personId]" +
                                " ,[organizationId]" +
                                " ,[personTypeId]" +
                                " ,[createdBy]" +
                                " ,[isActive]" +
                                " ,[createdOn]" +
                                " )" +
                    " VALUES (" +
                                //"  @IdOrgPersonDtl " +
                                " @PersonId " +
                                " ,@OrganizationId " +
                                " ,@PersonTypeId " +
                                " ,@CreatedBy " +
                                " ,@IsActive " +
                                " ,@CreatedOn " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
            cmdInsert.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.PersonId;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.OrganizationId;
            cmdInsert.Parameters.Add("@PersonTypeId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.PersonTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgPersonDtlsTO.CreatedOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgPersonDtlsTO, cmdUpdate);
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

        public static int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgPersonDtlsTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgPersonDtls] SET " +
            //"  [idOrgPersonDtl] = @IdOrgPersonDtl" +
            " [personId]= @PersonId" +
            " ,[organizationId]= @OrganizationId" +
            " ,[personTypeId]= @PersonTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn] = @CreatedOn" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
            cmdUpdate.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.PersonId;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.OrganizationId;
            cmdUpdate.Parameters.Add("@PersonTypeId", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.PersonTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgPersonDtlsTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgPersonDtl, cmdDelete);
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

        public static int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgPersonDtl, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idOrgPersonDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgPersonDtls] " +
            " WHERE idOrgPersonDtl = " + idOrgPersonDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgPersonDtl", System.Data.SqlDbType.Int).Value = tblOrgPersonDtlsTO.IdOrgPersonDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        public static List<TblOrgPersonDtlsTO> ConvertDTToList(SqlDataReader sqlDataReader)
        {
            List<TblOrgPersonDtlsTO> tblOrgPersonDtlsTOList = new List<TblOrgPersonDtlsTO>();
            if (sqlDataReader.HasRows)
            {
                while(sqlDataReader.Read())
                {
                    TblOrgPersonDtlsTO tblOrgPersonDtlsTONew = new TblOrgPersonDtlsTO();
                    if (sqlDataReader["idOrgPersonDtl"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.IdOrgPersonDtl = Convert.ToInt32(sqlDataReader["idOrgPersonDtl"].ToString());
                    if (sqlDataReader["personId"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.PersonId = Convert.ToInt32(sqlDataReader["personId"].ToString());
                    if (sqlDataReader["organizationId"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.OrganizationId = Convert.ToInt32(sqlDataReader["organizationId"].ToString());
                    if (sqlDataReader["personTypeId"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.PersonTypeId = Convert.ToInt32(sqlDataReader["personTypeId"].ToString());
                    if (sqlDataReader["createdBy"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.CreatedBy = Convert.ToInt32(sqlDataReader["createdBy"].ToString());
                    if (sqlDataReader["isActive"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.IsActive = Convert.ToInt32(sqlDataReader["isActive"].ToString());
                    if (sqlDataReader["createdOn"] != DBNull.Value)
                        tblOrgPersonDtlsTONew.CreatedOn = Convert.ToDateTime(sqlDataReader["createdOn"].ToString());
                    tblOrgPersonDtlsTOList.Add(tblOrgPersonDtlsTONew);
                }
            }
            return tblOrgPersonDtlsTOList;
        }

    }
}
