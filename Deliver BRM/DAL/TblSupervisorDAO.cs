using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class TblSupervisorDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT Supervisor.*, ISNULL(firstName,'') + ' ' + ISNULL(lastName,'')  AS supervisorName,person.*,sal.salutationDesc " +
                                  " FROM tblSupervisor Supervisor " +
                                  " LEFT JOIN tblPerson Person " +
                                  " ON Person.idPerson = Supervisor.personId" +
                                  " LEFT JOIN dimSalutation sal ON sal.idSalutation = Person.salutationId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblSupervisorTO> SelectAllTblSupervisor()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSupervisorTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static TblSupervisorTO SelectTblSupervisor(Int32 idSupervisor)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSupervisor = " + idSupervisor +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblSupervisorTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<TblSupervisorTO> ConvertDTToList(SqlDataReader tblSupervisorTODT)
        {
            List<TblSupervisorTO> tblSupervisorTOList = new List<TblSupervisorTO>();
            if (tblSupervisorTODT != null)
            {
                while (tblSupervisorTODT.Read())
                {
                    TblSupervisorTO tblSupervisorTONew = new TblSupervisorTO();
                    if (tblSupervisorTODT["idSupervisor"] != DBNull.Value)
                        tblSupervisorTONew.IdSupervisor = Convert.ToInt32(tblSupervisorTODT["idSupervisor"].ToString());
                    if (tblSupervisorTODT["isActive"] != DBNull.Value)
                        tblSupervisorTONew.IsActive = Convert.ToInt32(tblSupervisorTODT["isActive"].ToString());
                    if (tblSupervisorTODT["createdBy"] != DBNull.Value)
                        tblSupervisorTONew.CreatedBy = Convert.ToInt32(tblSupervisorTODT["createdBy"].ToString());
                    if (tblSupervisorTODT["createdOn"] != DBNull.Value)
                        tblSupervisorTONew.CreatedOn = Convert.ToDateTime(tblSupervisorTODT["createdOn"].ToString());
                    if (tblSupervisorTODT["supervisorName"] != DBNull.Value)
                        tblSupervisorTONew.SupervisorName = Convert.ToString(tblSupervisorTODT["supervisorName"].ToString());
                    if (tblSupervisorTODT["personId"] != DBNull.Value)
                        tblSupervisorTONew.PersonId = Convert.ToInt32(tblSupervisorTODT["personId"].ToString());

                    TblPersonTO tblPersonTONew = new TblPersonTO();

                    if (tblSupervisorTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblSupervisorTODT["idPerson"].ToString());
                    if (tblSupervisorTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblSupervisorTODT["salutationId"].ToString());
                    if (tblSupervisorTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblSupervisorTODT["mobileNo"].ToString());
                    if (tblSupervisorTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblSupervisorTODT["alternateMobNo"].ToString());
                    if (tblSupervisorTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblSupervisorTODT["phoneNo"].ToString());
                    if (tblSupervisorTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblSupervisorTODT["createdBy"].ToString());
                    if (tblSupervisorTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblSupervisorTODT["dateOfBirth"].ToString());
                    if (tblSupervisorTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblSupervisorTODT["createdOn"].ToString());
                    if (tblSupervisorTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblSupervisorTODT["firstName"].ToString());
                    if (tblSupervisorTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblSupervisorTODT["midName"].ToString());
                    if (tblSupervisorTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblSupervisorTODT["lastName"].ToString());
                    if (tblSupervisorTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblSupervisorTODT["primaryEmail"].ToString());
                    if (tblSupervisorTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblSupervisorTODT["alternateEmail"].ToString());
                    if (tblSupervisorTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblSupervisorTODT["comments"].ToString());
                    if (tblSupervisorTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblSupervisorTODT["salutationDesc"].ToString());
                    if (tblSupervisorTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblSupervisorTODT["photoBase64"].ToString());

                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }

                    tblSupervisorTONew.PersonTO = tblPersonTONew;

                    tblSupervisorTOList.Add(tblSupervisorTONew);
                }
            }
            return tblSupervisorTOList;
        }

        #endregion

        #region Insertion
        public static int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblSupervisorTO, cmdInsert);
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

        public static int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblSupervisorTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblSupervisorTO tblSupervisorTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblSupervisor]( " +
                                "  [isActive]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[personId]" +
                                " )" +
                    " VALUES (" +
                                "  @IsActive " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@personId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSupervisor", System.Data.SqlDbType.Int).Value = tblSupervisorTO.IdSupervisor;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSupervisorTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblSupervisorTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblSupervisorTO.CreatedOn;
            cmdInsert.Parameters.Add("@personId", System.Data.SqlDbType.Int).Value = tblSupervisorTO.PersonId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblSupervisorTO.IdSupervisor = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public static int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblSupervisorTO, cmdUpdate);
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

        public static int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblSupervisorTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblSupervisorTO tblSupervisorTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblSupervisor] SET " + 
                                "  [isActive]= @IsActive" +
                                " WHERE [idSupervisor] = @IdSupervisor "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSupervisor", System.Data.SqlDbType.Int).Value = tblSupervisorTO.IdSupervisor;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblSupervisorTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblSupervisor(Int32 idSupervisor)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSupervisor, cmdDelete);
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

        public static int DeleteTblSupervisor(Int32 idSupervisor, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSupervisor, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idSupervisor, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblSupervisor] " +
            " WHERE idSupervisor = " + idSupervisor +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSupervisor", System.Data.SqlDbType.Int).Value = tblSupervisorTO.IdSupervisor;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
