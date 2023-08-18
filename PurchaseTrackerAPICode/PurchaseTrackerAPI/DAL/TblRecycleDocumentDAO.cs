using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblRecycleDocumentDAO : ITblRecycleDocumentDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRecycleDocumentDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblRecycleDocument]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocument()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
               SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public  List<TblRecycleDocumentTO>  SelectTblRecycleDocument(Int32 idRecycleDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idRecycleDocument = " + idRecycleDocument +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

         public  List<TblRecycleDocumentTO> SelectRecycleDocumentList(string txnId,Int32 txnTypeId,Int32 isActive,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if(isActive==1)
                    cmdSelect.CommandText = SqlSelectQuery()+ " WHERE txnId IN ( " + txnId + ") AND txnTypeId= " + txnTypeId + " AND isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery()+ " WHERE txnId IN ( " + txnId + ") AND txnTypeId= " + txnTypeId ;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocument(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
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

         public  List<TblRecycleDocumentTO> ConvertDTToList(SqlDataReader tblRecycleDocumentTODT )
        {
            List<TblRecycleDocumentTO> tblRecycleDocumentTOList = new List<TblRecycleDocumentTO>();
            if (tblRecycleDocumentTODT != null)
            {
                while(tblRecycleDocumentTODT.Read())
                {
                    TblRecycleDocumentTO tblRecycleDocumentTONew = new TblRecycleDocumentTO();
                    if(tblRecycleDocumentTODT ["idRecycleDocument"] != DBNull.Value)
                        tblRecycleDocumentTONew.IdRecycleDocument = Convert.ToInt32( tblRecycleDocumentTODT ["idRecycleDocument"].ToString());
                    if(tblRecycleDocumentTODT ["documentId"] != DBNull.Value)
                        tblRecycleDocumentTONew.DocumentId = Convert.ToInt32( tblRecycleDocumentTODT ["documentId"].ToString());
                    if(tblRecycleDocumentTODT ["txnId"] != DBNull.Value)
                        tblRecycleDocumentTONew.TxnId = Convert.ToInt32( tblRecycleDocumentTODT ["txnId"].ToString());
                    if(tblRecycleDocumentTODT ["txnTypeId"] != DBNull.Value)
                        tblRecycleDocumentTONew.TxnTypeId = Convert.ToInt32( tblRecycleDocumentTODT ["txnTypeId"].ToString());
                    if(tblRecycleDocumentTODT ["createdBy"] != DBNull.Value)
                        tblRecycleDocumentTONew.CreatedBy = Convert.ToInt32( tblRecycleDocumentTODT ["createdBy"].ToString());
                    if(tblRecycleDocumentTODT ["updatedBy"] != DBNull.Value)
                        tblRecycleDocumentTONew.UpdatedBy = Convert.ToInt32( tblRecycleDocumentTODT ["updatedBy"].ToString());
                    if(tblRecycleDocumentTODT ["isActive"] != DBNull.Value)
                        tblRecycleDocumentTONew.IsActive = Convert.ToInt32( tblRecycleDocumentTODT ["isActive"].ToString());
                    if(tblRecycleDocumentTODT ["createdOn"] != DBNull.Value)
                        tblRecycleDocumentTONew.CreatedOn = Convert.ToDateTime( tblRecycleDocumentTODT ["createdOn"].ToString());
                    if(tblRecycleDocumentTODT ["updatedOn"] != DBNull.Value)
                        tblRecycleDocumentTONew.UpdatedOn = Convert.ToDateTime( tblRecycleDocumentTODT ["updatedOn"].ToString());
                    tblRecycleDocumentTOList.Add(tblRecycleDocumentTONew);
                }
            }
            return tblRecycleDocumentTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRecycleDocumentTO, cmdInsert);
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

        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRecycleDocumentTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRecycleDocument]( " + 
            //"  [idRecycleDocument]" +
            "  [documentId]" +
            " ,[txnId]" +
            " ,[txnTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            //"  @IdRecycleDocument " +
            "  @DocumentId " +
            " ,@TxnId " +
            " ,@TxnTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.DocumentId;
            cmdInsert.Parameters.Add("@TxnId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnId;
            cmdInsert.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRecycleDocumentTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblRecycleDocumentTO.UpdatedOn);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRecycleDocumentTO, cmdUpdate);
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

        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRecycleDocumentTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRecycleDocument] SET " + 
            //"  [idRecycleDocument] = @IdRecycleDocument" +
            "  [documentId]= @DocumentId" +
            " ,[txnId]= @TxnId" +
            " ,[txnTypeId]= @TxnTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE idRecycleDocument = @IdRecycleDocument "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.DocumentId;
            cmdUpdate.Parameters.Add("@TxnId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnId;
            cmdUpdate.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRecycleDocument, cmdDelete);
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

        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRecycleDocument, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idRecycleDocument, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRecycleDocument] " +
            " WHERE idRecycleDocument = " + idRecycleDocument +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
