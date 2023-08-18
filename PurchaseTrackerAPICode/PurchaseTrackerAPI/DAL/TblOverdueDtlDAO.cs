using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblOverdueDtlDAO : ITblOverdueDtlDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblOverdueDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOverdueDtl]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblOverdueDtlTO> SelectAllTblOverdueDtl()
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

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOverdueDtlTO> list = ConvertDTToList(sqlReader);
                    return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblOverdueDtlTO> SelectAllTblOverdueDtl(String dealerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM tblOverDuedtl allDtl " +
                                        " INNER JOIN(SELECT organizationId, MAX(asOnDate) overDueDate FROM tblOverDuedtl " +
                                        " GROUP BY organizationId ) AS latestOverDue " +
                                        " ON allDtl.organizationId = latestOverDue.organizationId " +
                                        " AND allDtl.asOnDate = latestOverDue.overDueDate " +
                                        " WHERE allDtl.organizationId IN (" + dealerIds + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOverdueDtlTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblOverdueDtlTO SelectTblOverdueDtl(Int32 idOverdueDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idOverdueDtl = " + idOverdueDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOverdueDtlTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if(sqlReader!=null)
                    sqlReader.Dispose();
                conn.Close();
            }
        }

        public  List<TblOverdueDtlTO> ConvertDTToList(SqlDataReader tblOverdueDtlTODT)
        {
            List<TblOverdueDtlTO> tblOverdueDtlTOList = new List<TblOverdueDtlTO>();
            if (tblOverdueDtlTODT != null)
            {
                while(tblOverdueDtlTODT.Read())
                {
                    TblOverdueDtlTO tblOverdueDtlTONew = new TblOverdueDtlTO();
                    if (tblOverdueDtlTODT["idOverdueDtl"] != DBNull.Value)
                        tblOverdueDtlTONew.IdOverdueDtl = Convert.ToInt32(tblOverdueDtlTODT["idOverdueDtl"].ToString());
                    if (tblOverdueDtlTODT["organizationId"] != DBNull.Value)
                        tblOverdueDtlTONew.OrganizationId = Convert.ToInt32(tblOverdueDtlTODT["organizationId"].ToString());
                    if (tblOverdueDtlTODT["createdBy"] != DBNull.Value)
                        tblOverdueDtlTONew.CreatedBy = Convert.ToInt32(tblOverdueDtlTODT["createdBy"].ToString());
                    if (tblOverdueDtlTODT["asOnDate"] != DBNull.Value)
                        tblOverdueDtlTONew.AsOnDate = Convert.ToDateTime(tblOverdueDtlTODT["asOnDate"].ToString());
                    if (tblOverdueDtlTODT["createdOn"] != DBNull.Value)
                        tblOverdueDtlTONew.CreatedOn = Convert.ToDateTime(tblOverdueDtlTODT["createdOn"].ToString());
                    if (tblOverdueDtlTODT["overdueAmt"] != DBNull.Value)
                        tblOverdueDtlTONew.OverdueAmt = Convert.ToDouble(tblOverdueDtlTODT["overdueAmt"].ToString());
                    if (tblOverdueDtlTODT["partyName"] != DBNull.Value)
                        tblOverdueDtlTONew.PartyName = Convert.ToString(tblOverdueDtlTODT["partyName"].ToString());
                    tblOverdueDtlTOList.Add(tblOverdueDtlTONew);
                }
            }
            return tblOverdueDtlTOList;
        }


        public  List<TblOverdueDtlTO> SelectTblOverdueDtlList(Int32 dealerId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM tblOverDuedtl allDtl " +
                                        " INNER JOIN(SELECT organizationId, MAX(asOnDate) overDueDate FROM tblOverDuedtl " +
                                        " GROUP BY organizationId ) AS latestOverDue " +
                                        " ON allDtl.organizationId = latestOverDue.organizationId " +
                                        " AND allDtl.asOnDate = latestOverDue.overDueDate " +
                                        " WHERE allDtl.organizationId =" + dealerId  ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOverdueDtlTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public  int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOverdueDtlTO, cmdInsert);
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

        public  int InsertTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOverdueDtlTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblOverdueDtlTO tblOverdueDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOverdueDtl]( " + 
            //"  [idOverdueDtl]" +
            "  [organizationId]" +
            " ,[createdBy]" +
            " ,[asOnDate]" +
            " ,[createdOn]" +
            " ,[overdueAmt]" +
            " ,[partyName]" +
            " )" +
" VALUES (" +
            //"  @IdOverdueDtl " +
            "  @OrganizationId " +
            " ,@CreatedBy " +
            " ,@AsOnDate " +
            " ,@CreatedOn " +
            " ,@OverdueAmt " +
            " ,@PartyName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOverdueDtl", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.IdOverdueDtl;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOverdueDtlTO.OrganizationId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@AsOnDate", System.Data.SqlDbType.DateTime).Value = tblOverdueDtlTO.AsOnDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOverdueDtlTO.CreatedOn;
            cmdInsert.Parameters.Add("@OverdueAmt", System.Data.SqlDbType.Decimal).Value = tblOverdueDtlTO.OverdueAmt;
            cmdInsert.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = tblOverdueDtlTO.PartyName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOverdueDtlTO, cmdUpdate);
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

        public  int UpdateTblOverdueDtl(TblOverdueDtlTO tblOverdueDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOverdueDtlTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblOverdueDtlTO tblOverdueDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOverdueDtl] SET " + 
            "  [idOverdueDtl] = @IdOverdueDtl" +
            " ,[organizationId]= @OrganizationId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[asOnDate]= @AsOnDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[overdueAmt]= @OverdueAmt" +
            " ,[partyName] = @PartyName" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOverdueDtl", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.IdOverdueDtl;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.OrganizationId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.CreatedBy;
            cmdUpdate.Parameters.Add("@AsOnDate", System.Data.SqlDbType.DateTime).Value = tblOverdueDtlTO.AsOnDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOverdueDtlTO.CreatedOn;
            cmdUpdate.Parameters.Add("@OverdueAmt", System.Data.SqlDbType.Decimal).Value = tblOverdueDtlTO.OverdueAmt;
            cmdUpdate.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = tblOverdueDtlTO.PartyName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblOverdueDtl(Int32 idOverdueDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOverdueDtl, cmdDelete);
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

        public  int DeleteTblOverdueDtl(Int32 idOverdueDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOverdueDtl, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idOverdueDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOverdueDtl] " +
            " WHERE idOverdueDtl = " + idOverdueDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOverdueDtl", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.IdOverdueDtl;
            return cmdDelete.ExecuteNonQuery();
        }

        /// <summary>
        /// [11-12-2017]Vijaymala: Added to delete previous overdue details
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public  int DeleteTblOverdueDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = "DELETE FROM [tblOverdueDtl] ";
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

    }
}
