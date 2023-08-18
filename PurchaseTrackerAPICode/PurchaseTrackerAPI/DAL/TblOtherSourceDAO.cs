using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;

namespace PurchaseTrackerAPI.DAL
{
    public class TblOtherSourceDAO : ITblOtherSourceDAO
    {


        private readonly IConnectionString _iConnectionString;
        public TblOtherSourceDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOtherSource]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblOtherSourceTO> SelectAllTblOtherSource()
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
                return ConvertDTToList(sqlReader);
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

        public  TblOtherSourceTO SelectTblOtherSource(Int32 idOtherSource)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idOtherSource = " + idOtherSource +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherSourceTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblOtherSourceTO> SelectTblOtherSourceListFromDesc(string OtherSourceDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE otherDesc = '" + OtherSourceDesc + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
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

        public  List<DropDownTO> SelectOtherSourceOfMarketTrendForDropDown()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT idOtherSource,otherDesc FROM tblOtherSource";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOtherSource"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOtherSource"].ToString());
                        if (tblOrgReader["otherDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["otherDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
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

        public  List<TblOtherSourceTO> ConvertDTToList(SqlDataReader tblOtherSourceTODT)
        {
            List<TblOtherSourceTO> tblOtherSourceTOList = new List<TblOtherSourceTO>();
            if (tblOtherSourceTODT != null)
            {
                while (tblOtherSourceTODT.Read())
                {
                    TblOtherSourceTO tblOtherSourceTONew = new TblOtherSourceTO();
                    if (tblOtherSourceTODT["idOtherSource"] != DBNull.Value)
                        tblOtherSourceTONew.IdOtherSource = Convert.ToInt32(tblOtherSourceTODT["idOtherSource"].ToString());
                    if (tblOtherSourceTODT["createdBy"] != DBNull.Value)
                        tblOtherSourceTONew.CreatedBy = Convert.ToInt32(tblOtherSourceTODT["createdBy"].ToString());
                    if (tblOtherSourceTODT["createdOn"] != DBNull.Value)
                        tblOtherSourceTONew.CreatedOn = Convert.ToDateTime(tblOtherSourceTODT["createdOn"].ToString());
                    if (tblOtherSourceTODT["otherDesc"] != DBNull.Value)
                        tblOtherSourceTONew.OtherDesc = Convert.ToString(tblOtherSourceTODT["otherDesc"].ToString());
                    tblOtherSourceTOList.Add(tblOtherSourceTONew);
                }
            }
            return tblOtherSourceTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOtherSourceTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOtherSourceTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblOtherSourceTO tblOtherSourceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOtherSource]( " + 
                                "  [createdBy]" +
                                " ,[createdOn]" +
                                " ,[otherDesc]" +
                                " )" +
                    " VALUES (" +
                                "  @CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@OtherDesc " + 
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOtherSource", System.Data.SqlDbType.Int).Value = tblOtherSourceTO.IdOtherSource;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOtherSourceTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherSourceTO.CreatedOn;
            cmdInsert.Parameters.Add("@OtherDesc", System.Data.SqlDbType.NVarChar).Value = tblOtherSourceTO.OtherDesc;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = PurchaseTrackerAPI.StaticStuff.Constants.IdentityColumnQuery;
                tblOtherSourceTO.IdOtherSource = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;

        }
        #endregion
        
        #region Updation
        public  int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOtherSourceTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblOtherSource(TblOtherSourceTO tblOtherSourceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOtherSourceTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblOtherSourceTO tblOtherSourceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOtherSource] SET " + 
                                "  [otherDesc] = @OtherDesc" +
                                " WHERE [idOtherSource] = @IdOtherSource "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOtherSource", System.Data.SqlDbType.Int).Value = tblOtherSourceTO.IdOtherSource;
            cmdUpdate.Parameters.Add("@OtherDesc", System.Data.SqlDbType.NVarChar).Value = tblOtherSourceTO.OtherDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblOtherSource(Int32 idOtherSource)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOtherSource, cmdDelete);
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

        public  int DeleteTblOtherSource(Int32 idOtherSource, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOtherSource, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idOtherSource, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOtherSource] " +
            " WHERE idOtherSource = " + idOtherSource +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOtherSource", System.Data.SqlDbType.Int).Value = tblOtherSourceTO.IdOtherSource;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
