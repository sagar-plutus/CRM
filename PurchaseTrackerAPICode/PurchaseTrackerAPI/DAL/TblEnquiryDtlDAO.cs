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
    public class TblEnquiryDtlDAO : ITblEnquiryDtlDAO
    {
        

        private readonly IConnectionString _iConnectionString;
        public TblEnquiryDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblEnquiryDtl]";
            return sqlSelectQry;
        }
        #endregion


        #region Selection
        public  List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl()
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
                List<TblEnquiryDtlTO> list = ConvertDTToList(sqlReader);
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

        public  List<TblEnquiryDtlTO> SelectAllTblEnquiryDtl(String dealerIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM tblEnquiryDtl enqDtl " +
                                        " INNER JOIN(SELECT organizationId, MAX(asOnDate) enquiryDate FROM tblEnquiryDtl " +
                                        " GROUP BY organizationId ) AS latestEnquiryDtl " +
                                        " ON enqDtl.organizationId = latestEnquiryDtl.organizationId " +
                                        " AND enqDtl.asOnDate = latestEnquiryDtl.enquiryDate " +
                                        " WHERE enqDtl.organizationId IN (" + dealerIds + ")";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEnquiryDtlTO> list = ConvertDTToList(sqlReader);
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


        public  List<TblEnquiryDtlTO> SelectEnquiryDtlList(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM tblEnquiryDtl enqDtl " +
                                        " INNER JOIN(SELECT organizationId, MAX(asOnDate) enquiryDate FROM tblEnquiryDtl " +
                                        " GROUP BY organizationId ) AS latestEnquiryDtl " +
                                        " ON enqDtl.organizationId = latestEnquiryDtl.organizationId " +
                                        " AND enqDtl.asOnDate = latestEnquiryDtl.enquiryDate " +
                                        " WHERE enqDtl.organizationId = " + organizationId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEnquiryDtlTO> list = ConvertDTToList(sqlReader);
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


        public  TblEnquiryDtlTO SelectTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idEnquiryDtl = " + idEnquiryDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEnquiryDtlTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
            }
        }

        public  List<TblEnquiryDtlTO> ConvertDTToList(SqlDataReader tblEnquiryDtlTODT)
        {
            List<TblEnquiryDtlTO> tblEnquiryDtlTOList = new List<TblEnquiryDtlTO>();
            if (tblEnquiryDtlTODT != null)
            {
                while (tblEnquiryDtlTODT.Read())
                {
                    TblEnquiryDtlTO tblEnquiryDtlTONew = new TblEnquiryDtlTO();
                    if (tblEnquiryDtlTODT["idEnquiryDtl"] != DBNull.Value)
                        tblEnquiryDtlTONew.IdEnquiryDtl = Convert.ToInt32(tblEnquiryDtlTODT["idEnquiryDtl"].ToString());
                    if (tblEnquiryDtlTODT["organizationId"] != DBNull.Value)
                        tblEnquiryDtlTONew.OrganizationId = Convert.ToInt32(tblEnquiryDtlTODT["organizationId"].ToString());
                    if (tblEnquiryDtlTODT["createdBy"] != DBNull.Value)
                        tblEnquiryDtlTONew.CreatedBy = Convert.ToInt32(tblEnquiryDtlTODT["createdBy"].ToString());
                    if (tblEnquiryDtlTODT["asOnDate"] != DBNull.Value)
                        tblEnquiryDtlTONew.AsOnDate = Convert.ToDateTime(tblEnquiryDtlTODT["asOnDate"].ToString());
                    if (tblEnquiryDtlTODT["createdOn"] != DBNull.Value)
                        tblEnquiryDtlTONew.CreatedOn = Convert.ToDateTime(tblEnquiryDtlTODT["createdOn"].ToString());
                    if (tblEnquiryDtlTODT["enquiryAmt"] != DBNull.Value)
                        tblEnquiryDtlTONew.EnquiryAmt = Convert.ToDouble(tblEnquiryDtlTODT["enquiryAmt"].ToString());
                    if (tblEnquiryDtlTODT["partyName"] != DBNull.Value)
                        tblEnquiryDtlTONew.PartyName = Convert.ToString(tblEnquiryDtlTODT["partyName"].ToString());
                    tblEnquiryDtlTOList.Add(tblEnquiryDtlTONew);
                }
            }
            return tblEnquiryDtlTOList;
        }


        /// <summary>
        /// [04-12-2017] Vijaymala :To get previous enquiry details of organization using enquiry reference Id
        /// </summary>
        /// <param name="tblEnquiryDtlTO"></param>
        /// <returns></returns>
        /// 

        public  TblEnquiryDtlTO SelectOrganizationEnquiryDtl(string enqRefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT enquriDtl.* FROM tblEnquiryDtl enquriDtl INNER JOIN tblOrganization" +
                                       " orgInfo ON enquriDtl.organizationId = orgInfo.idOrganization "+
                                       " WHERE orgInfo.enq_ref_id = " + enqRefId ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEnquiryDtlTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
            }
        }
        #endregion

        #region Insertion
        public  int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblEnquiryDtlTO, cmdInsert);
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

        public  int InsertTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblEnquiryDtlTO, cmdInsert);
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

        /// <summary>
        /// [04-12-2017]Vijaymala :Added to deactivate previous enquiry detail of organization
        /// </summary>
        /// <param name="tblEnquiryDtlTO"></param>
        /// <param name="cmdInsert"></param>
        /// <returns></returns>

        public  int DeactivateOrgEnqDeatls(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblEnquiryDtl] SET " +
                                   " [isActive]= @IsActive" +
                                   " WHERE idEnquiryDtl=@IdEnquiryDtl";
                                  
                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@IdEnquiryDtl", System.Data.SqlDbType.Int).Value = idEnquiryDtl;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblEnquiryDtlTO tblEnquiryDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblEnquiryDtl]( " +
            //"  [idEnquiryDtl]" +
            " [organizationId]" +
            " ,[createdBy]" +
            " ,[asOnDate]" +
            " ,[createdOn]" +
            " ,[enquiryAmt]" +
            " ,[partyName]" +
            " )" +
" VALUES (" +
            //"  @IdEnquiryDtl " +
            " @OrganizationId " +
            " ,@CreatedBy " +
            " ,@AsOnDate " +
            " ,@CreatedOn " +
            " ,@EnquiryAmt " +
            " ,@PartyName " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdEnquiryDtl", System.Data.SqlDbType.Int).Value = tblEnquiryDtlTO.IdEnquiryDtl;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblEnquiryDtlTO.OrganizationId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEnquiryDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@AsOnDate", System.Data.SqlDbType.DateTime).Value = tblEnquiryDtlTO.AsOnDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEnquiryDtlTO.CreatedOn;
            cmdInsert.Parameters.Add("@EnquiryAmt", System.Data.SqlDbType.Decimal).Value = tblEnquiryDtlTO.EnquiryAmt;
            cmdInsert.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = tblEnquiryDtlTO.PartyName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public  int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblEnquiryDtlTO, cmdUpdate);
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

        public  int UpdateTblEnquiryDtl(TblEnquiryDtlTO tblEnquiryDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblEnquiryDtlTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblEnquiryDtlTO tblEnquiryDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEnquiryDtl] SET " +
            "  [idEnquiryDtl] = @IdEnquiryDtl" +
            " ,[organizationId]= @OrganizationId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[asOnDate]= @AsOnDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[enquiryAmt]= @EnquiryAmt" +
            " ,[partyName] = @PartyName" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdEnquiryDtl", System.Data.SqlDbType.Int).Value = tblEnquiryDtlTO.IdEnquiryDtl;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblEnquiryDtlTO.OrganizationId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEnquiryDtlTO.CreatedBy;
            cmdUpdate.Parameters.Add("@AsOnDate", System.Data.SqlDbType.DateTime).Value = tblEnquiryDtlTO.AsOnDate;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblEnquiryDtlTO.CreatedOn;
            cmdUpdate.Parameters.Add("@EnquiryAmt", System.Data.SqlDbType.NVarChar).Value = tblEnquiryDtlTO.EnquiryAmt;
            cmdUpdate.Parameters.Add("@PartyName", System.Data.SqlDbType.NVarChar).Value = tblEnquiryDtlTO.PartyName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblEnquiryDtl(Int32 idEnquiryDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idEnquiryDtl, cmdDelete);
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

        public  int DeleteTblEnquiryDtl(Int32 idEnquiryDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idEnquiryDtl, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idEnquiryDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblEnquiryDtl] " +
            " WHERE idEnquiryDtl = " + idEnquiryDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOverdueDtl", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.IdOverdueDtl;
            return cmdDelete.ExecuteNonQuery();
        }

        public  int DeleteTblEnquiryDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = "DELETE FROM [tblEnquiryDtl] ";

                cmdDelete.CommandType = System.Data.CommandType.Text;

                //cmdDelete.Parameters.Add("@idOverdueDtl", System.Data.SqlDbType.Int).Value = tblOverdueDtlTO.IdOverdueDtl;
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
