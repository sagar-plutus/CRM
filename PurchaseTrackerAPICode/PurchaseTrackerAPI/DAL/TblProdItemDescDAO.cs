using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using PurchaseTrackerAPI.StaticStuff;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.Models;


namespace PurchaseTrackerAPI.DAL
{
    public class TblProdItemDescDAO : ITblProdItemDescDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProdItemDescDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblProdItemDesc]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProdItemDescTO> SelectAllTblProdItemDesc(int itemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isactive = 1 and itemid = @itemId";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = itemId;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public List<TblProdItemDescTO> SelectAllTblProdItemDesc()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isactive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                // cmdSelect.Parameters.Add("@itemId", System.Data.SqlDbType.Int).Value = itemId;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemDescTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        private List<TblProdItemDescTO> ConvertDTToList(SqlDataReader sqlReader)
        {
            List<TblProdItemDescTO> list = new List<TblProdItemDescTO>();
            if (sqlReader != null)
            {
                while (sqlReader.Read())
                {
                    TblProdItemDescTO tblProdItemDescTONew = new TblProdItemDescTO();
                    if (sqlReader["idProdItemDesc"] != DBNull.Value)
                        tblProdItemDescTONew.IdProdItemDesc = Convert.ToInt32(sqlReader["idProdItemDesc"].ToString());
                    if (sqlReader["seqNo"] != DBNull.Value)
                        tblProdItemDescTONew.SeqNo = Convert.ToInt32(sqlReader["seqNo"].ToString());
                    if (sqlReader["itemId"] != DBNull.Value)
                        tblProdItemDescTONew.ItemId = Convert.ToInt32(sqlReader["itemId"].ToString());
                    if (sqlReader["isActive"] != DBNull.Value)
                        tblProdItemDescTONew.IsActive = Convert.ToInt32(sqlReader["isActive"].ToString());
                    if (sqlReader["createdBy"] != DBNull.Value)
                        tblProdItemDescTONew.CreatedBy = Convert.ToInt32(sqlReader["createdBy"].ToString());
                    if (sqlReader["updatedBy"] != DBNull.Value)
                        tblProdItemDescTONew.UpdatedBy = Convert.ToInt32(sqlReader["updatedBy"].ToString());
                    if (sqlReader["createdOn"] != DBNull.Value)
                        tblProdItemDescTONew.CreatedOn = Convert.ToDateTime(sqlReader["createdOn"].ToString());
                    if (sqlReader["updatedOn"] != DBNull.Value)
                        tblProdItemDescTONew.UpdatedOn = Convert.ToDateTime(sqlReader["updatedOn"].ToString());
                    if (sqlReader["name"] != DBNull.Value)
                        tblProdItemDescTONew.Name = Convert.ToString(sqlReader["name"].ToString());
                    if (sqlReader["description"] != DBNull.Value)
                        tblProdItemDescTONew.Description = Convert.ToString(sqlReader["description"].ToString());
                    list.Add(tblProdItemDescTONew);
                }
            }
            return list;
        }

        public TblProdItemDescTO SelectTblProdItemDesc(Int32 idProdItemDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdItemDesc = " + idProdItemDesc + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemDesc", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IdProdItemDesc;
                SqlDataReader reader = cmdSelect.ExecuteReader();
                DataTable dt = new DataTable();
                List<TblProdItemDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list[0];
                else return null;

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

        public List<TblProdItemDescTO> SelectAllTblProdItemDesc(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemDesc", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IdProdItemDesc;
                SqlDataReader reader = cmdSelect.ExecuteReader();
                DataTable dt = new DataTable();
                List<TblProdItemDescTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        public int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdItemDescTO, cmdInsert);
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

        public int InsertTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdItemDescTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProdItemDescTO tblProdItemDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdItemDesc]( " +
            "  [seqNo]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[name]" +
            " ,[description]" +
            " )" +
" VALUES (" +
            "  @SeqNo " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Name " +
            " ,@Description " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdProdItemDesc", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IdProdItemDesc;
            cmdInsert.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.SeqNo;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemDescTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemDescTO.UpdatedOn;
            cmdInsert.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblProdItemDescTO.Name;
            cmdInsert.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblProdItemDescTO.Description;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdItemDescTO, cmdUpdate);
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

        public int UpdateTblProdItemDesc(TblProdItemDescTO tblProdItemDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdItemDescTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblProdItemDescTO tblProdItemDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdItemDesc] SET " +
            "  [seqNo]= @SeqNo" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[name]= @Name" +
            " ,[description] = @Description" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItemDesc", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IdProdItemDesc;
            cmdUpdate.Parameters.Add("@SeqNo", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.SeqNo;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemDescTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemDescTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblProdItemDescTO.Name;
            cmdUpdate.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar).Value = tblProdItemDescTO.Description;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblProdItemDesc(Int32 idProdItemDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItemDesc, cmdDelete);
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

        public int DeleteTblProdItemDesc(Int32 idProdItemDesc, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItemDesc, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProdItemDesc, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdItemDesc] " +
            " WHERE idProdItemDesc = " + idProdItemDesc + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItemDesc", System.Data.SqlDbType.Int).Value = tblProdItemDescTO.IdProdItemDesc;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
