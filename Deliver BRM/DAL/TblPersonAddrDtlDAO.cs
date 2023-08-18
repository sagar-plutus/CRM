using SalesTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblPersonAddrDtlDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPersonAddrDtl]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl()
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
                List<TblPersonAddrDtlTO> tblPersonAddrDtlTO = ConvertDTToList(sqlReader);
                return tblPersonAddrDtlTO;
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

        public static TblPersonAddrDtlTO SelectTblPersonAddrDtl(Int32 idPersonAddrDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPersonAddrDtl = " + idPersonAddrDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonAddrDtlTO> tblPersonAddrDtlTOList = ConvertDTToList(sqlReader);
                if (tblPersonAddrDtlTOList != null && tblPersonAddrDtlTOList.Count > 0)
                {
                    return tblPersonAddrDtlTOList[0];
                }
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

        //Sudhir[13-MAR-2018] Added for get TO for the personAddressDtl Based on PersonId and AddressTypeId.
        public static TblPersonAddrDtlTO SelectTblPersonAddrDtl(Int32 personId, Int32 addressTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE personId = " + personId + " AND addressTypeId = " + addressTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonAddrDtlTO> tblPersonAddrDtlTOList = ConvertDTToList(sqlReader);
                if (tblPersonAddrDtlTOList != null && tblPersonAddrDtlTOList.Count > 0)
                {
                    return tblPersonAddrDtlTOList[0];
                }
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

        public static List<TblPersonAddrDtlTO> SelectAllTblPersonAddrDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonAddrDtlTO> tblPersonAddrDtlTOList = ConvertDTToList(sqlReader);
                return tblPersonAddrDtlTOList;
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
        public static int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPersonAddrDtlTO, cmdInsert);
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

        public static int InsertTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPersonAddrDtlTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPersonAddrDtl]( " +
            //"  [idPersonAddrDtl]" +
            " [personId]" +
            " ,[addressId]" +
            " ,[addressTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            //"  @IdPersonAddrDtl " +
            " @PersonId " +
            " ,@AddressId " +
            " ,@AddressTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdPersonAddrDtl", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.IdPersonAddrDtl;
            cmdInsert.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.PersonId;
            cmdInsert.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.AddressId;
            cmdInsert.Parameters.Add("@AddressTypeId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.AddressTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.UpdatedOn);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblPersonAddrDtlTO.IdPersonAddrDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else
                return 0;
            //return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPersonAddrDtlTO, cmdUpdate);
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

        public static int UpdateTblPersonAddrDtl(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPersonAddrDtlTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblPersonAddrDtlTO tblPersonAddrDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPersonAddrDtl] SET " +
            //"  [idPersonAddrDtl] = @IdPersonAddrDtl" +
            " [personId]= @PersonId" +
            " ,[addressId]= @AddressId" +
            " ,[addressTypeId]= @AddressTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE [idPersonAddrDtl] = @IdPersonAddrDtl ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPersonAddrDtl", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.IdPersonAddrDtl;
            cmdUpdate.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.PersonId;
            cmdUpdate.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.AddressId;
            cmdUpdate.Parameters.Add("@AddressTypeId", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.AddressTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonAddrDtlTO.UpdatedOn);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPersonAddrDtl, cmdDelete);
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

        public static int DeleteTblPersonAddrDtl(Int32 idPersonAddrDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPersonAddrDtl, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idPersonAddrDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPersonAddrDtl] " +
            " WHERE idPersonAddrDtl = " + idPersonAddrDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPersonAddrDtl", System.Data.SqlDbType.Int).Value = tblPersonAddrDtlTO.IdPersonAddrDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public static List<TblPersonAddrDtlTO> ConvertDTToList(SqlDataReader tblPersonAddrDtlTODT)
        {
            List<TblPersonAddrDtlTO> tblPersonAddrDtlTOList = new List<TblPersonAddrDtlTO>();
            if (tblPersonAddrDtlTODT != null)
            {
                while (tblPersonAddrDtlTODT.Read())
                {
                    TblPersonAddrDtlTO tblPersonAddrDtlTONew = new TblPersonAddrDtlTO();
                    if (tblPersonAddrDtlTODT["idPersonAddrDtl"] != DBNull.Value)
                        tblPersonAddrDtlTONew.IdPersonAddrDtl = Convert.ToInt32(tblPersonAddrDtlTODT["idPersonAddrDtl"].ToString());
                    if (tblPersonAddrDtlTODT["personId"] != DBNull.Value)
                        tblPersonAddrDtlTONew.PersonId = Convert.ToInt32(tblPersonAddrDtlTODT["personId"].ToString());
                    if (tblPersonAddrDtlTODT["addressId"] != DBNull.Value)
                        tblPersonAddrDtlTONew.AddressId = Convert.ToInt32(tblPersonAddrDtlTODT["addressId"].ToString());
                    if (tblPersonAddrDtlTODT["addressTypeId"] != DBNull.Value)
                        tblPersonAddrDtlTONew.AddressTypeId = Convert.ToInt32(tblPersonAddrDtlTODT["addressTypeId"].ToString());
                    if (tblPersonAddrDtlTODT["createdBy"] != DBNull.Value)
                        tblPersonAddrDtlTONew.CreatedBy = Convert.ToInt32(tblPersonAddrDtlTODT["createdBy"].ToString());
                    if (tblPersonAddrDtlTODT["updatedBy"] != DBNull.Value)
                        tblPersonAddrDtlTONew.UpdatedBy = Convert.ToInt32(tblPersonAddrDtlTODT["updatedBy"].ToString());
                    if (tblPersonAddrDtlTODT["createdOn"] != DBNull.Value)
                        tblPersonAddrDtlTONew.CreatedOn = Convert.ToDateTime(tblPersonAddrDtlTODT["createdOn"].ToString());
                    if (tblPersonAddrDtlTODT["updatedOn"] != DBNull.Value)
                        tblPersonAddrDtlTONew.UpdatedOn = Convert.ToDateTime(tblPersonAddrDtlTODT["updatedOn"].ToString());
                    tblPersonAddrDtlTOList.Add(tblPersonAddrDtlTONew);
                }
            }
            return tblPersonAddrDtlTOList;
        }
    }
}
