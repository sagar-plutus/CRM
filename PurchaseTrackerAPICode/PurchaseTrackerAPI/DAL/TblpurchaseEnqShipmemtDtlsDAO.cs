using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using System.Globalization;

namespace PurchaseTrackerAPI.DAL
{
    public class TblpurchaseEnqShipmemtDtlsDAO : ITblpurchaseEnqShipmemtDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblpurchaseEnqShipmemtDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblpurchaseEnqShipmemtDtls.*,tblPurchaseEnquiry.indentureName ,tblOrganization.firmName FROM [tblpurchaseEnqShipmemtDtls] tblpurchaseEnqShipmemtDtls" +
                " LEFT JOIN tblPurchaseEnquiry tblPurchaseEnquiry ON tblpurchaseEnqShipmemtDtls.purchaseEnquiryId = tblPurchaseEnquiry.idPurchaseEnquiry " +
                " left join tblOrganization tblOrganization ON tblOrganization.idOrganization = tblPurchaseEnquiry.SupplierId "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtls()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsTO> list = ConvertDTToList(sqlReader);
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

        public TblpurchaseEnqShipmemtDtlsTO SelectTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idShipmentDtls = " + idShipmentDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblpurchaseEnqShipmemtDtlsTO> SelectTblpurchaseEnqShipmemtDtlsTOByEnqId(Int32 purchaseEnqId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblpurchaseEnqShipmemtDtls.isActive = 1 AND purchaseEnquiryId = " + purchaseEnqId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
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

        public List<TblpurchaseEnqShipmemtDtlsTO> SelectAllTblpurchaseEnqShipmemtDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblpurchaseEnqShipmemtDtlsTO> list = ConvertDTToList(sqlReader);
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


        public List<TblpurchaseEnqShipmemtDtlsTO> ConvertDTToList(SqlDataReader tblpurchaseEnqShipmemtDtlsTODT)
        {
            List<TblpurchaseEnqShipmemtDtlsTO> tblpurchaseEnqShipmemtDtlsTOList = new List<TblpurchaseEnqShipmemtDtlsTO>();
            if (tblpurchaseEnqShipmemtDtlsTODT != null)
            {
                while (tblpurchaseEnqShipmemtDtlsTODT.Read())
                {
                    TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTONew = new TblpurchaseEnqShipmemtDtlsTO();
                    if (tblpurchaseEnqShipmemtDtlsTODT["idShipmentDtls"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.IdShipmentDtls = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsTODT["idShipmentDtls"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.PurchaseEnquiryId = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsTODT["purchaseEnquiryId"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["createdBy"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.CreatedBy = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsTODT["createdBy"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["updatedBy"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.UpdatedBy = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsTODT["updatedBy"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["isActive"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.IsActive = Convert.ToInt32(tblpurchaseEnqShipmemtDtlsTODT["isActive"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["createdOn"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.CreatedOn = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsTODT["createdOn"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["updatedOn"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.UpdatedOn = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsTODT["updatedOn"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["billDate"] != DBNull.Value)
                    {
                        tblpurchaseEnqShipmemtDtlsTONew.BillDate = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsTODT["billDate"].ToString());
                        tblpurchaseEnqShipmemtDtlsTONew.BillDateStr = tblpurchaseEnqShipmemtDtlsTONew.BillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                        
                    }
                    if (tblpurchaseEnqShipmemtDtlsTODT["beDate"] != DBNull.Value)
                    {
                        tblpurchaseEnqShipmemtDtlsTONew.BeDate = Convert.ToDateTime(tblpurchaseEnqShipmemtDtlsTODT["beDate"].ToString());
                        tblpurchaseEnqShipmemtDtlsTONew.BeDateStr = tblpurchaseEnqShipmemtDtlsTONew.BeDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

                    }
                    if (tblpurchaseEnqShipmemtDtlsTODT["ShippingLine"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.ShippingLine = Convert.ToString(tblpurchaseEnqShipmemtDtlsTODT["ShippingLine"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["billNo"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.BillNo = Convert.ToString(tblpurchaseEnqShipmemtDtlsTODT["billNo"].ToString());
                    if (tblpurchaseEnqShipmemtDtlsTODT["beNo"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.BeNo = Convert.ToString(tblpurchaseEnqShipmemtDtlsTODT["beNo"].ToString());

                    if (tblpurchaseEnqShipmemtDtlsTODT["firmName"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.SupplierName = Convert.ToString(tblpurchaseEnqShipmemtDtlsTODT["firmName"].ToString());

                    if (tblpurchaseEnqShipmemtDtlsTODT["indentureName"] != DBNull.Value)
                        tblpurchaseEnqShipmemtDtlsTONew.IndentureName = Convert.ToString(tblpurchaseEnqShipmemtDtlsTODT["indentureName"].ToString());

                    tblpurchaseEnqShipmemtDtlsTOList.Add(tblpurchaseEnqShipmemtDtlsTONew);
                }
            }
            return tblpurchaseEnqShipmemtDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblpurchaseEnqShipmemtDtlsTO, cmdInsert);
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

        public  int InsertTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblpurchaseEnqShipmemtDtlsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblpurchaseEnqShipmemtDtls]( " + 
            "  [purchaseEnquiryId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[billDate]" +
            " ,[beDate]" +
            " ,[ShippingLine]" +
            " ,[billNo]" +
            " ,[beNo]" +
            " )" +
" VALUES (" +
            "  @PurchaseEnquiryId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@BillDate " +
            " ,@BeDate " +
            " ,@ShippingLine " +
            " ,@BillNo " +
            " ,@BeNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
            cmdInsert.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.PurchaseEnquiryId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@BillDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BillDate);
            cmdInsert.Parameters.Add("@BeDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BeDate);
            cmdInsert.Parameters.Add("@ShippingLine", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.ShippingLine);
            cmdInsert.Parameters.Add("@BillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BillNo);
            cmdInsert.Parameters.Add("@BeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BeNo);
            
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblpurchaseEnqShipmemtDtlsTO, cmdUpdate);
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

        public  int UpdateTblpurchaseEnqShipmemtDtls(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblpurchaseEnqShipmemtDtlsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblpurchaseEnqShipmemtDtlsTO tblpurchaseEnqShipmemtDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblpurchaseEnqShipmemtDtls] SET " + 
            "  [purchaseEnquiryId]= @PurchaseEnquiryId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[billDate]= @BillDate" +
            " ,[beDate]= @BeDate" +
            " ,[ShippingLine]= @ShippingLine" +
            " ,[billNo]= @BillNo" +
            " ,[beNo] = @BeNo" +
            " WHERE 1 = 1"; 

            cmdUpdate.CommandText = sqlQuery + "AND idShipmentDtls = @IdShipmentDtls";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
            cmdUpdate.Parameters.Add("@PurchaseEnquiryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.PurchaseEnquiryId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@BillDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BillDate);
            cmdUpdate.Parameters.Add("@BeDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BeDate);
            cmdUpdate.Parameters.Add("@ShippingLine", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.ShippingLine);
            cmdUpdate.Parameters.Add("@BillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BillNo);
            cmdUpdate.Parameters.Add("@BeNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblpurchaseEnqShipmemtDtlsTO.BeNo);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idShipmentDtls, cmdDelete);
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

        public  int DeleteTblpurchaseEnqShipmemtDtls(Int32 idShipmentDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idShipmentDtls, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idShipmentDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblpurchaseEnqShipmemtDtls] " +
            " WHERE idShipmentDtls = " + idShipmentDtls +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idShipmentDtls", System.Data.SqlDbType.Int).Value = tblpurchaseEnqShipmemtDtlsTO.IdShipmentDtls;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
