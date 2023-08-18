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
    public class TblAddonsFunDtlsDAO: ITblAddonsFunDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAddonsFunDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblAddonsFunDtls.*,dimAdd.funName,userdata.userdisplayName FROM [tblAddonsFunDtls] " +
                                  " LEFT JOIN dimAddonsFun dimAdd  " +
                                  " ON dimAdd.idAddonsFun=tblAddonsFunDtls.funRefId " +
                                  "LEFT JOIN tbluser userdata ON userdata.iduser = tblAddonsFunDtls.createdBy " +
                                  "WHERE tblAddonsFunDtls.isActive = 1 ";
            return sqlSelectQry;
        }

        #endregion

        #region Selection
        public  List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls()
        {

            String sqlConnStr =   _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(sqlReader);
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

        public  TblAddonsFunDtlsTO SelectTblAddonsFunDtls(int idAddonsfunDtls)
        {
            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND idAddonsfunDtls = " + idAddonsfunDtls + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idAddonsFun", System.Data.SqlDbType.Int).Value = dimAddonsFunTO.IdAddonsFun;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(rdr);
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

         public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByRootScheduleId(Int32 rootScheduleId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND tblAddonsFunDtls.transId = " + rootScheduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

         public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsByPurchaseInvoiceId(Int32 purchaseInvoiceId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND tblAddonsFunDtls.transId = " + purchaseInvoiceId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
         public  List<TblAddonsFunDtlsTO> SelectTblAddonsFunDtlsBySpotVehicleId(Int32 spotVehicleId,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND tblAddonsFunDtls.transId = " + spotVehicleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public  List<TblAddonsFunDtlsTO> SelectAddonDetailsList(int transId, int ModuleId, String TransactionType, String PageElementId, String transIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                var query = "";
                if (transIds == null || transIds == "null")
                {
                    query = "AND transId = " + transId + " AND transType = '" + TransactionType + "' ";
                    if (ModuleId != 0)
                        query += "AND moduleId = " + ModuleId + " ";
                    if (PageElementId != null && PageElementId != "null")
                        query += "AND pageElementId = " + PageElementId + " ";
                }
                else
                {
                    query = "AND transId IN(" + transIds + ") AND transType = '" + TransactionType + "' ";
                }
                cmdSelect.CommandText = SqlSelectQuery() + query;// "AND transId = " + transId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(rdr);
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


        public  List<TblAddonsFunDtlsTO> SelectAllTblAddonsFunDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            cmdSelect.Connection = conn;
            cmdSelect.Transaction = tran;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = ConvertDTToList(reader);
                return list;

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
        public List<TblAddonsFunDtlsTO> SelectAddonDetailsList(String transIds, int ModuleId, String TransactionType, String PageElementId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblAddonsFunDtlsTODT = null;
            try
            {
                conn.Open();
                var query = "SELECT transId,COUNT(*) as Count FROM tblAddonsFunDtls where transId IN(" + transIds + ")" +
                    " AND transType = '" + TransactionType + "' AND moduleId = " + ModuleId + " AND pageElementId = '" + PageElementId + "'" +
                    " AND isActive = 1 GROUP BY transId";
                cmdSelect.CommandText = query;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                tblAddonsFunDtlsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddonsFunDtlsTO> list = new List<TblAddonsFunDtlsTO>();
                if (tblAddonsFunDtlsTODT != null)
                {
                    while (tblAddonsFunDtlsTODT.Read())
                    {
                        TblAddonsFunDtlsTO tblAddonsFunDtlsTONew = new TblAddonsFunDtlsTO();
                        if (tblAddonsFunDtlsTODT["transId"] != DBNull.Value)
                            tblAddonsFunDtlsTONew.TransId = Convert.ToInt32(tblAddonsFunDtlsTODT["transId"].ToString());

                        if (tblAddonsFunDtlsTODT["Count"] != DBNull.Value)
                            tblAddonsFunDtlsTONew.Count = Convert.ToInt32(tblAddonsFunDtlsTODT["Count"].ToString());

                        list.Add(tblAddonsFunDtlsTONew);
                    }
                }
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
        #endregion

        #region Insertion
        public  int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {

            String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAddonsFunDtlsTO, cmdInsert);
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

        public  int InsertTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAddonsFunDtlsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAddonsFunDtls]( " +
            "  [createdOn]" +
            // " ,[idAddonsfunDtls]" +
            " ,[moduleId]" +
            " ,[pageElementId]" +
            " ,[transId]" +
            " ,[funRefId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +
            " ,[isActive]" +
            " ,[transType]" +
            " ,[funRefVal]" +
            " )" +
" VALUES (" +
            "  @CreatedOn " +
            //   " ,@IdAddonsfunDtls " +
            " ,@ModuleId " +
            " ,@PageElementId " +
            " ,@TransId " +
            " ,@FunRefId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@UpdatedOn " +
            " ,@IsActive " +
            " ,@TransType " +
            " ,@FunRefVal " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAddonsFunDtlsTO.CreatedOn;
            // cmdInsert.Parameters.Add("@IdAddonsfunDtls", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.IdAddonsfunDtls;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.ModuleId;
            cmdInsert.Parameters.Add("@PageElementId", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.PageElementId;
            cmdInsert.Parameters.Add("@TransId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.TransId;
            cmdInsert.Parameters.Add("@FunRefId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.FunRefId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddonsFunDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.IsActive;
            cmdInsert.Parameters.Add("@TransType", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.TransType;
            cmdInsert.Parameters.Add("@FunRefVal", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.FunRefVal;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAddonsFunDtlsTO, cmdUpdate);
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

        public  int UpdateTblAddonsFunDtls(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAddonsFunDtlsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblAddonsFunDtlsTO tblAddonsFunDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAddonsFunDtls] SET " +
            "  [createdOn] = @CreatedOn" +
            //  " ,[idAddonsfunDtls]= @IdAddonsfunDtls" +
            " ,[moduleId]= @ModuleId" +
            " ,[pageElementId]= @PageElementId" +
            " ,[transId]= @TransId" +
            " ,[funRefId]= @FunRefId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[isActive]= @IsActive" +
            " ,[transType]= @TransType" +
            " ,[funRefVal] = @FunRefVal" +
            " WHERE  idAddonsfunDtls= " + tblAddonsFunDtlsTO.IdAddonsfunDtls;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAddonsFunDtlsTO.CreatedOn;
            //    cmdUpdate.Parameters.Add("@IdAddonsfunDtls", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.IdAddonsfunDtls;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.ModuleId;
            cmdUpdate.Parameters.Add("@PageElementId", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.PageElementId;
            cmdUpdate.Parameters.Add("@TransId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.TransId;
            cmdUpdate.Parameters.Add("@FunRefId", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.FunRefId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAddonsFunDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAddonsFunDtlsTO.IsActive;
            cmdUpdate.Parameters.Add("@TransType", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.TransType;
            cmdUpdate.Parameters.Add("@FunRefVal", System.Data.SqlDbType.NVarChar).Value = tblAddonsFunDtlsTO.FunRefVal;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblAddonsFunDtls(int idAddonsfunDtls)
        {
            String sqlConnStr =   _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAddonsfunDtls, cmdDelete);
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
        public int DeleteTblAddonsFunDtls(String idAddonsfunDtlsStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = "DELETE FROM [tblAddonsFunDtls] " +
                " WHERE idAddonsfunDtls IN(" + idAddonsfunDtlsStr + ")";
                cmdDelete.CommandType = System.Data.CommandType.Text;
                return cmdDelete.ExecuteNonQuery();
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
        public  int DeleteTblAddonsFunDtls(int idAddonsfunDtls, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAddonsfunDtls, cmdDelete);
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

        public  int ExecuteDeletionCommand(int idAddonsfunDtls, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAddonsFunDtls] " +
            " WHERE idAddonsfunDtls = " + idAddonsfunDtls + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }

         public  int DeleteAllPhotoAgainstVehScheduleId(Int32 rootScheduleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblAddonsFunDtls WHERE transId = " + rootScheduleId;
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
         public  int DeleteAllPhotoAgainstVehInvoiceId(Int32 purchaseInvoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblAddonsFunDtls WHERE transId = " + purchaseInvoiceId;
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
         public  int DeleteAllPhotoAgainstSpotVehId(Int32 spotVehicleId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.CommandText = "DELETE FROM tblAddonsFunDtls WHERE transId = " + spotVehicleId;
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        #endregion

        public  List<TblAddonsFunDtlsTO> ConvertDTToList(SqlDataReader tblAddonsFunDtlsTODT)
        {
            List<TblAddonsFunDtlsTO> tblAddonsFunDtlsTOList = new List<TblAddonsFunDtlsTO>();
            if (tblAddonsFunDtlsTODT != null)
            {
                while (tblAddonsFunDtlsTODT.Read())
                {
                    TblAddonsFunDtlsTO tblAddonsFunDtlsTONew = new TblAddonsFunDtlsTO();
                    if (tblAddonsFunDtlsTODT["createdOn"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.CreatedOn = Convert.ToDateTime(tblAddonsFunDtlsTODT["createdOn"].ToString());
                    if (tblAddonsFunDtlsTODT["idAddonsfunDtls"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.IdAddonsfunDtls = Convert.ToInt32(tblAddonsFunDtlsTODT["idAddonsfunDtls"].ToString());
                    if (tblAddonsFunDtlsTODT["moduleId"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.ModuleId = Convert.ToInt32(tblAddonsFunDtlsTODT["moduleId"].ToString());
                    if (tblAddonsFunDtlsTODT["pageElementId"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.PageElementId = Convert.ToString(tblAddonsFunDtlsTODT["pageElementId"].ToString());
                    if (tblAddonsFunDtlsTODT["transId"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.TransId = Convert.ToInt32(tblAddonsFunDtlsTODT["transId"].ToString());
                    if (tblAddonsFunDtlsTODT["funRefId"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.FunRefId = Convert.ToInt32(tblAddonsFunDtlsTODT["funRefId"].ToString());
                    if (tblAddonsFunDtlsTODT["createdBy"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.CreatedBy = Convert.ToInt32(tblAddonsFunDtlsTODT["createdBy"].ToString());
                    if (tblAddonsFunDtlsTODT["updatedBy"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.UpdatedBy = Convert.ToInt32(tblAddonsFunDtlsTODT["updatedBy"].ToString());
                    if (tblAddonsFunDtlsTODT["updatedOn"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.UpdatedOn = Convert.ToDateTime(tblAddonsFunDtlsTODT["updatedOn"].ToString());
                    if (tblAddonsFunDtlsTODT["isActive"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.IsActive = Convert.ToInt32(tblAddonsFunDtlsTODT["isActive"].ToString());
                    if (tblAddonsFunDtlsTODT["transType"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.TransType = Convert.ToString(tblAddonsFunDtlsTODT["transType"].ToString());
                    if (tblAddonsFunDtlsTODT["funRefVal"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.FunRefVal = Convert.ToString(tblAddonsFunDtlsTODT["funRefVal"].ToString());
                    if (tblAddonsFunDtlsTODT["funName"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.AddOnType = Convert.ToString(tblAddonsFunDtlsTODT["funName"].ToString());
                    if (tblAddonsFunDtlsTODT["userdisplayName"] != DBNull.Value)
                        tblAddonsFunDtlsTONew.UserdisplayName = Convert.ToString(tblAddonsFunDtlsTODT["userdisplayName"].ToString());

                    tblAddonsFunDtlsTOList.Add(tblAddonsFunDtlsTONew);
                }
            }
            return tblAddonsFunDtlsTOList;
        }

    }
}
