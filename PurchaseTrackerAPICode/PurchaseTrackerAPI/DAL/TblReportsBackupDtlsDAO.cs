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
using System.Linq;

namespace PurchaseTrackerAPI.DAL
{
    public class TblReportsBackupDtlsDAO : ITblReportsBackupDtlsDAO
    {
        #region Methods
        private readonly IConnectionString _iConnectionString;
        public TblReportsBackupDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblReportsBackupDtls]"; 
            return sqlSelectQry;
        }
        
        #endregion

        #region Selection
        public List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public DateTime SelectReportMinBackupDate()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            DateTime reportMindate = DateTime.MinValue;

            String reportNameStr = "'" + Constants.BRMReportNameE.CC_TRANSPORT_ENQUIRY + "'" +"," +  "'"  +Constants.BRMReportNameE.GRADE_NOTE_ENQUIRY_REPORT + "'"  +"," +
                                    "'" + Constants.BRMReportNameE.TALLY_CREDIT_NOTE_ORDER_REPORT + "'" + "," + "'" + Constants.BRMReportNameE.TALLY_PR_ENQUIRY_REPORT+  "'"  + "," +
                                    "'" +  Constants.BRMReportNameE.TALLY_TRANSPORT_ENQUIRY+  "'"  + "," + "'" +  Constants.BRMReportNameE.WB_REPORT + "'" ;

            if(!Startup.IsForBRM)
            {
                reportNameStr = "'" + Constants.BRMReportNameE.TALLY_REPORT + "'";
            }

            try
            {
                conn.Open();
                cmdSelect.CommandText = " select  max(backupDate) AS backupDate from tblReportsBackupDtls " +
                                        " where reportName IN( " + reportNameStr + ")" +
                                        " group by reportName " ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = new List<TblReportsBackupDtlsTO>();
                while (sqlReader.Read())
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTO = new TblReportsBackupDtlsTO();

                    if (sqlReader["backupDate"] != DBNull.Value)
                        tblReportsBackupDtlsTO.BackupDate = Convert.ToDateTime(sqlReader["backupDate"].ToString());

                    list.Add(tblReportsBackupDtlsTO);

                }

                if(list != null && list.Count > 0)
                {
                    reportMindate = list.Min(a => a.BackupDate);
                    //TblReportsBackupDtlsTO tempTO = list.OrderBy(x => x.BackupDate).FirstOrDefault();
                    //reportMindate = tempTO.BackupDate;
                }
                        
                sqlReader.Dispose();
                return reportMindate;
            }
            catch (Exception ex)
            {
                return DateTime.MinValue;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblReportsBackupDtlsTO> SelectReportBackupDtls(string reportName,DateTime currentDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE reportName= '" + reportName + "'" +
                    " AND CAST(backupDate AS date) = @currentDate";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@currentDate", System.Data.SqlDbType.Date).Value = currentDate;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        //Added by minal 02 May 2021 For Get Backup from Report Name
        public List<TblReportsBackupDtlsTO> SelectReportBackupDateDtls(string reportName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idReportBackup IN (" +
                    " SELECT MAX(idReportBackup) FROM[tblReportsBackupDtls]  " +
                    " WHERE reportName= '" + reportName + "')";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;                

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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


        public List<TblReportsBackupDtlsTO> SelectTblReportsBackupDtls(Int32 idReportBackup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReportBackup = " + idReportBackup +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public List<TblReportsBackupDtlsTO> SelectAllTblReportsBackupDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsBackupDtlsTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public List<TblReportsBackupDtlsTO> ConvertDTToList(SqlDataReader tblReportsBackupDtlsTODT)
        {
            List<TblReportsBackupDtlsTO> tblReportsBackupDtlsTOList = new List<TblReportsBackupDtlsTO>();
            if (tblReportsBackupDtlsTODT != null)
            {
                while(tblReportsBackupDtlsTODT.Read())
                {
                    TblReportsBackupDtlsTO tblReportsBackupDtlsTONew = new TblReportsBackupDtlsTO();
                    if (tblReportsBackupDtlsTODT ["idReportBackup"] != DBNull.Value)
                        tblReportsBackupDtlsTONew.IdReportBackup = Convert.ToInt32(tblReportsBackupDtlsTODT ["idReportBackup"].ToString());
                    if (tblReportsBackupDtlsTODT ["isBackUp"] != DBNull.Value)
                        tblReportsBackupDtlsTONew.IsBackUp = Convert.ToInt32(tblReportsBackupDtlsTODT ["isBackUp"].ToString());
                    if (tblReportsBackupDtlsTODT ["backupDate"] != DBNull.Value)
                        tblReportsBackupDtlsTONew.BackupDate = Convert.ToDateTime(tblReportsBackupDtlsTODT ["backupDate"].ToString());
                    if (tblReportsBackupDtlsTODT ["reportName"] != DBNull.Value)
                        tblReportsBackupDtlsTONew.ReportName = Convert.ToString(tblReportsBackupDtlsTODT ["reportName"].ToString());
                    tblReportsBackupDtlsTOList.Add(tblReportsBackupDtlsTONew);
                }
            }
            return tblReportsBackupDtlsTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblReportsBackupDtlsTO, cmdInsert);
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

        public int InsertTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblReportsBackupDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblReportsBackupDtls]( " + 
            //"  [idReportBackup]" +
            "  [isBackUp]" +
            " ,[backupDate]" +
            " ,[reportName]" +
            " )" +
" VALUES (" +
            //"  @IdReportBackup " +
            "  @IsBackUp " +
            " ,@BackupDate " +
            " ,@ReportName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdReportBackup", System.Data.SqlDbType.Int).Value = tblReportsBackupDtlsTO.IdReportBackup;
            cmdInsert.Parameters.Add("@IsBackUp", System.Data.SqlDbType.Int).Value = tblReportsBackupDtlsTO.IsBackUp;
            cmdInsert.Parameters.Add("@BackupDate", System.Data.SqlDbType.DateTime).Value = tblReportsBackupDtlsTO.BackupDate;
            cmdInsert.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsBackupDtlsTO.ReportName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblReportsBackupDtlsTO, cmdUpdate);
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

        public int UpdateTblReportsBackupDtls(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblReportsBackupDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblReportsBackupDtlsTO tblReportsBackupDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblReportsBackupDtls] SET " + 
            //"  [idReportBackup] = @IdReportBackup" +
            "  [isBackUp]= @IsBackUp" +
            " ,[backupDate]= @BackupDate" +
            " ,[reportName] = @ReportName" +
            " WHERE idReportBackup = @IdReportBackup "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReportBackup", System.Data.SqlDbType.Int).Value = tblReportsBackupDtlsTO.IdReportBackup;
            cmdUpdate.Parameters.Add("@IsBackUp", System.Data.SqlDbType.Int).Value = tblReportsBackupDtlsTO.IsBackUp;
            cmdUpdate.Parameters.Add("@BackupDate", System.Data.SqlDbType.DateTime).Value = tblReportsBackupDtlsTO.BackupDate;
            cmdUpdate.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsBackupDtlsTO.ReportName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblReportsBackupDtls(Int32 idReportBackup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReportBackup, cmdDelete);
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

        public int DeleteTblReportsBackupDtls(Int32 idReportBackup, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReportBackup, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idReportBackup, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblReportsBackupDtls] " +
            " WHERE idReportBackup = " + idReportBackup +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReportBackup", System.Data.SqlDbType.Int).Value = tblReportsBackupDtlsTO.IdReportBackup;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
