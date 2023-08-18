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
    public class TblOtherTaxesDAO : ITblOtherTaxesDAO
    {


        private readonly IConnectionString _iConnectionString;
        public TblOtherTaxesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOtherTaxes]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblOtherTaxesTO> SelectAllTblOtherTaxes()
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
                List<TblOtherTaxesTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  TblOtherTaxesTO SelectTblOtherTaxes(Int32 idOtherTax)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idOtherTax = " + idOtherTax +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherTaxesTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblOtherTaxesTO> ConvertDTToList(SqlDataReader tblOtherTaxesTODT)
        {
            List<TblOtherTaxesTO> tblOtherTaxesTOList = new List<TblOtherTaxesTO>();
            if (tblOtherTaxesTODT != null)
            {
                while (tblOtherTaxesTODT.Read())
                {
                    TblOtherTaxesTO tblOtherTaxesTONew = new TblOtherTaxesTO();
                    if (tblOtherTaxesTODT["idOtherTax"] != DBNull.Value)
                        tblOtherTaxesTONew.IdOtherTax = Convert.ToInt32(tblOtherTaxesTODT["idOtherTax"].ToString());
                    if (tblOtherTaxesTODT["isBefore"] != DBNull.Value)
                        tblOtherTaxesTONew.IsBefore = Convert.ToInt32(tblOtherTaxesTODT["isBefore"].ToString());
                    if (tblOtherTaxesTODT["isAfter"] != DBNull.Value)
                        tblOtherTaxesTONew.IsAfter = Convert.ToInt32(tblOtherTaxesTODT["isAfter"].ToString());
                    if (tblOtherTaxesTODT["both"] != DBNull.Value)
                        tblOtherTaxesTONew.Both = Convert.ToInt32(tblOtherTaxesTODT["both"].ToString());
                    if (tblOtherTaxesTODT["isActive"] != DBNull.Value)
                        tblOtherTaxesTONew.IsActive = Convert.ToInt32(tblOtherTaxesTODT["isActive"].ToString());
                    if (tblOtherTaxesTODT["createdBy"] != DBNull.Value)
                        tblOtherTaxesTONew.CreatedBy = Convert.ToInt32(tblOtherTaxesTODT["createdBy"].ToString());
                    if (tblOtherTaxesTODT["createdOn"] != DBNull.Value)
                        tblOtherTaxesTONew.CreatedOn = Convert.ToDateTime(tblOtherTaxesTODT["createdOn"].ToString());
                    if (tblOtherTaxesTODT["defaultPct"] != DBNull.Value)
                        tblOtherTaxesTONew.DefaultPct = Convert.ToDouble(tblOtherTaxesTODT["defaultPct"].ToString());
                    if (tblOtherTaxesTODT["defaultAmt"] != DBNull.Value)
                        tblOtherTaxesTONew.DefaultAmt = Convert.ToDouble(tblOtherTaxesTODT["defaultAmt"].ToString());
                    if (tblOtherTaxesTODT["taxName"] != DBNull.Value)
                        tblOtherTaxesTONew.TaxName = Convert.ToString(tblOtherTaxesTODT["taxName"].ToString());
                    tblOtherTaxesTOList.Add(tblOtherTaxesTONew);
                }
            }
            return tblOtherTaxesTOList;
        }

        #endregion

        #region Insertion
        public  int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOtherTaxesTO, cmdInsert);
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

        public  int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOtherTaxesTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblOtherTaxesTO tblOtherTaxesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOtherTaxes]( " + 
                                "  [isBefore]" +
                                " ,[isAfter]" +
                                " ,[both]" +
                                " ,[isActive]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[defaultPct]" +
                                " ,[defaultAmt]" +
                                " ,[taxName]" +
                                " )" +
                    " VALUES (" +
                                "  @IsBefore " +
                                " ,@IsAfter " +
                                " ,@Both " +
                                " ,@IsActive " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@DefaultPct " +
                                " ,@DefaultAmt " +
                                " ,@TaxName " + 
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOtherTax", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IdOtherTax;
            cmdInsert.Parameters.Add("@IsBefore", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsBefore;
            cmdInsert.Parameters.Add("@IsAfter", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsAfter;
            cmdInsert.Parameters.Add("@Both", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.Both;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherTaxesTO.CreatedOn;
            cmdInsert.Parameters.Add("@DefaultPct", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.DefaultPct;
            cmdInsert.Parameters.Add("@DefaultAmt", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.DefaultAmt;
            cmdInsert.Parameters.Add("@TaxName", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.TaxName;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblOtherTaxesTO.IdOtherTax = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOtherTaxesTO, cmdUpdate);
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

        public  int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOtherTaxesTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblOtherTaxesTO tblOtherTaxesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOtherTaxes] SET " + 
                            "  [isBefore]= @IsBefore" +
                            " ,[isAfter]= @IsAfter" +
                            " ,[both]= @Both" +
                            " ,[isActive]= @IsActive" +
                            " ,[createdBy]= @CreatedBy" +
                            " ,[createdOn]= @CreatedOn" +
                            " ,[defaultPct]= @DefaultPct" +
                            " ,[defaultAmt]= @DefaultAmt" +
                            " ,[taxName] = @TaxName" +
                            " WHERE [idOtherTax] = @IdOtherTax"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOtherTax", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IdOtherTax;
            cmdUpdate.Parameters.Add("@IsBefore", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsBefore;
            cmdUpdate.Parameters.Add("@IsAfter", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsAfter;
            cmdUpdate.Parameters.Add("@Both", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.Both;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOtherTaxesTO.CreatedOn;
            cmdUpdate.Parameters.Add("@DefaultPct", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.DefaultPct;
            cmdUpdate.Parameters.Add("@DefaultAmt", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.DefaultAmt;
            cmdUpdate.Parameters.Add("@TaxName", System.Data.SqlDbType.NVarChar).Value = tblOtherTaxesTO.TaxName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblOtherTaxes(Int32 idOtherTax)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOtherTax, cmdDelete);
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

        public  int DeleteTblOtherTaxes(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOtherTax, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idOtherTax, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOtherTaxes] " +
            " WHERE idOtherTax = " + idOtherTax +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOtherTax", System.Data.SqlDbType.Int).Value = tblOtherTaxesTO.IdOtherTax;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
