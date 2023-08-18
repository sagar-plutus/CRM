using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPurchaseInvoiceDocumentsDAO : ITblPurchaseInvoiceDocumentsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPurchaseInvoiceDocumentsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblPurchaseInvoiceDocs.*,tblPurchaseDocToVerify.masterId,tblPurchaseDocToVerify.isFromMaster,dimm.masterValueDesc  FROM [tblPurchaseInvoiceDocuments] tblPurchaseInvoiceDocs " +
                "LEFT JOIN tblPurchaseDocToVerify tblPurchaseDocToVerify ON tblPurchaseDocToVerify.idPurchaseDocType = tblPurchaseInvoiceDocs.documentTypeId" +
                " left join dimMasterValue dimm on dimm.idMasterValue = tblPurchaseInvoiceDocs.masterValueId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
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
                List<TblPurchaseInvoiceDocumentsTO> list = ConvertDTToList(reader);
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



        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(Int64 purchaseInvoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where purchaseInvoiceId =" + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceDocumentsTO> list = ConvertDTToList(reader);
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

        public  TblPurchaseInvoiceDocumentsTO SelectTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblPurchaseInvoiceDocs.idPurchaseInvDocument = " + idPurchaseInvDocument + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceDocumentsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

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
        //Priyanka [07-02-2019]
        public  List<TblPurchaseInvoiceDocumentsTO> SelecTblPurDocToVerifyWithDocDtlsAgainstPurInvId(Int64 purchaseInvoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  ISNULL(tblPurchaseInvoiceDocs.purchaseInvoiceId,0) = " + purchaseInvoiceId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceDocumentsTO> list = ConvertDTToList(reader);
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
        public  List<TblPurchaseInvoiceDocumentsTO> SelectAllTblPurchaseInvoiceDocuments(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPurchaseInvDocument", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceDocumentsTO.IdPurchaseInvDocument;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseInvoiceDocumentsTO> list = ConvertDTToList(reader);
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
        public  List<TblPurchaseInvoiceDocumentsTO> ConvertDTToList(SqlDataReader tblPurchaseInvoiceDocumentsTODT)
        {
            List<TblPurchaseInvoiceDocumentsTO> tblPurchaseInvoiceDocumentsTOList = new List<TblPurchaseInvoiceDocumentsTO>();
            if (tblPurchaseInvoiceDocumentsTODT != null)
            {

                while (tblPurchaseInvoiceDocumentsTODT.Read())
                {
                    TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTONew = new TblPurchaseInvoiceDocumentsTO();
                    if (tblPurchaseInvoiceDocumentsTODT["documentTypeId"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.DocumentTypeId = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["documentTypeId"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["documentId"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.DocumentId = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["documentId"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["createdBy"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.CreatedBy = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["createdBy"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.UpdatedBy = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["updatedBy"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["isActive"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.IsActive = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["isActive"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["createdOn"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.CreatedOn = Convert.ToDateTime(tblPurchaseInvoiceDocumentsTODT["createdOn"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseInvoiceDocumentsTODT["updatedOn"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["idPurchaseInvDocument"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.IdPurchaseInvDocument = Convert.ToInt64(tblPurchaseInvoiceDocumentsTODT["idPurchaseInvDocument"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["purchaseInvoiceId"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.PurchaseInvoiceId = Convert.ToInt64(tblPurchaseInvoiceDocumentsTODT["purchaseInvoiceId"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["documentTypeValue"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.DocumentTypeValue = Convert.ToString(tblPurchaseInvoiceDocumentsTODT["documentTypeValue"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["isDocAttach"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.IsDocAttach = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["isDocAttach"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["masterValueId"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.MasterValueId = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["masterValueId"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["masterId"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.MasterId = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["masterId"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["isFromMaster"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.IsFromMaster = Convert.ToInt32(tblPurchaseInvoiceDocumentsTODT["isFromMaster"].ToString());
                    if (tblPurchaseInvoiceDocumentsTODT["masterValueDesc"] != DBNull.Value)
                        tblPurchaseInvoiceDocumentsTONew.MasterValueDesc = tblPurchaseInvoiceDocumentsTODT["masterValueDesc"].ToString();
                    tblPurchaseInvoiceDocumentsTOList.Add(tblPurchaseInvoiceDocumentsTONew);
                }
            }
            return tblPurchaseInvoiceDocumentsTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPurchaseInvoiceDocumentsTO, cmdInsert);
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

        public  int InsertTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPurchaseInvoiceDocumentsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPurchaseInvoiceDocuments]( " +
            "  [documentTypeId]" +
            " ,[documentId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            // " ,[idPurchaseInvDocument]" +
            " ,[purchaseInvoiceId]" +
            " ,[documentTypeValue]" +
            " ,[isDocAttach]" +
            " ,[masterValueId]" +
            " )" +
" VALUES (" +
            "  @DocumentTypeId " +
            " ,@DocumentId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            // " ,@IdPurchaseInvDocument " +
            " ,@PurchaseInvoiceId " +
            " ,@DocumentTypeValue " +
            " ,@IsDocAttach " +
            " ,@MasterValueId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@DocumentTypeId", System.Data.SqlDbType.Int).Value = tblPurchaseInvoiceDocumentsTO.DocumentTypeId;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.DocumentId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.UpdatedOn);
            //cmdInsert.Parameters.Add("@IdPurchaseInvDocument", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IdPurchaseInvDocument);
            cmdInsert.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.PurchaseInvoiceId);
            cmdInsert.Parameters.Add("@DocumentTypeValue", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.DocumentTypeValue);
            cmdInsert.Parameters.Add("@IsDocAttach", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IsDocAttach);
            cmdInsert.Parameters.Add("@MasterValueId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.MasterValueId);

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPurchaseInvoiceDocumentsTO, cmdUpdate);
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

        public  int UpdateTblPurchaseInvoiceDocuments(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPurchaseInvoiceDocumentsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPurchaseInvoiceDocumentsTO tblPurchaseInvoiceDocumentsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPurchaseInvoiceDocuments] SET " +
            "  [documentTypeId] = @DocumentTypeId" +
            " ,[documentId]= @DocumentId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            // " ,[idPurchaseInvDocument]= @IdPurchaseInvDocument" +
            " ,[purchaseInvoiceId]= @PurchaseInvoiceId" +
            " ,[documentTypeValue] = @DocumentTypeValue" +
            " ,[isDocAttach] = @IsDocAttach" +
            " ,[masterValueId] = @MasterValueId" +
            " WHERE [idPurchaseInvDocument]= @IdPurchaseInvDocument ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@DocumentTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.DocumentTypeId);
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.DocumentId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@IdPurchaseInvDocument", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IdPurchaseInvDocument);
            cmdUpdate.Parameters.Add("@PurchaseInvoiceId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.PurchaseInvoiceId);
            cmdUpdate.Parameters.Add("@DocumentTypeValue", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.DocumentTypeValue);
            cmdUpdate.Parameters.Add("@IsDocAttach", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.IsDocAttach);
            cmdUpdate.Parameters.Add("@MasterValueId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseInvoiceDocumentsTO.MasterValueId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPurchaseInvDocument, cmdDelete);
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

        public  int DeleteTblPurchaseInvoiceDocuments(Int64 idPurchaseInvDocument, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPurchaseInvDocument, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int64 idPurchaseInvDocument, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPurchaseInvoiceDocuments] " +
            " WHERE idPurchaseInvDocument = " + idPurchaseInvDocument + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPurchaseInvDocument", System.Data.SqlDbType.BigInt).Value = tblPurchaseInvoiceDocumentsTO.IdPurchaseInvDocument;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
