using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblPersonDAO : ITblPersonDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblPersonDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT person.*,sal.salutationDesc FROM tblPerson person " +
                                  " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblPersonTO> SelectAllTblPerson()
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
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                return list;
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
                cmdSelect.Dispose();
            }
        }

        public  List<TblPersonTO> SelectAllTblPersonByOrganization(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                                        " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                return list;
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
                cmdSelect.Dispose();
            }
        }

        public  TblPersonTO SelectTblPerson(Int32 idPerson)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson = " + idPerson + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
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
                cmdSelect.Dispose();
            }
        }

        public  List<TblPersonTO> ConvertDTToList(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {
                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["idPerson"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["salutationId"] != DBNull.Value)
                        tblPersonTONew.SalutationId = Convert.ToInt32(tblPersonTODT["salutationId"].ToString());
                    if (tblPersonTODT["mobileNo"] != DBNull.Value)
                        tblPersonTONew.MobileNo = Convert.ToString(tblPersonTODT["mobileNo"].ToString());
                    if (tblPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblPersonTONew.AlternateMobNo = Convert.ToString(tblPersonTODT["alternateMobNo"].ToString());
                    if (tblPersonTODT["phoneNo"] != DBNull.Value)
                        tblPersonTONew.PhoneNo = Convert.ToString(tblPersonTODT["phoneNo"].ToString());
                    if (tblPersonTODT["createdBy"] != DBNull.Value)
                        tblPersonTONew.CreatedBy = Convert.ToInt32(tblPersonTODT["createdBy"].ToString());
                    if (tblPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblPersonTONew.DateOfBirth = Convert.ToDateTime(tblPersonTODT["dateOfBirth"].ToString());
                    if (tblPersonTODT["createdOn"] != DBNull.Value)
                        tblPersonTONew.CreatedOn = Convert.ToDateTime(tblPersonTODT["createdOn"].ToString());
                    if (tblPersonTODT["firstName"] != DBNull.Value)
                        tblPersonTONew.FirstName = Convert.ToString(tblPersonTODT["firstName"].ToString());
                    if (tblPersonTODT["midName"] != DBNull.Value)
                        tblPersonTONew.MidName = Convert.ToString(tblPersonTODT["midName"].ToString());
                    if (tblPersonTODT["lastName"] != DBNull.Value)
                        tblPersonTONew.LastName = Convert.ToString(tblPersonTODT["lastName"].ToString());
                    if (tblPersonTODT["primaryEmail"] != DBNull.Value)
                        tblPersonTONew.PrimaryEmail = Convert.ToString(tblPersonTODT["primaryEmail"].ToString());
                    if (tblPersonTODT["alternateEmail"] != DBNull.Value)
                        tblPersonTONew.AlternateEmail = Convert.ToString(tblPersonTODT["alternateEmail"].ToString());
                    if (tblPersonTODT["comments"] != DBNull.Value)
                        tblPersonTONew.Comments = Convert.ToString(tblPersonTODT["comments"].ToString());
                    if (tblPersonTODT["salutationDesc"] != DBNull.Value)
                        tblPersonTONew.SalutationName = Convert.ToString(tblPersonTODT["salutationDesc"].ToString());
                    if (tblPersonTODT["photoBase64"] != DBNull.Value)
                        tblPersonTONew.PhotoBase64 = Convert.ToString(tblPersonTODT["photoBase64"].ToString());

                    if (tblPersonTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblPersonTONew.DobDay = tblPersonTONew.DateOfBirth.Day;
                        tblPersonTONew.DobMonth = tblPersonTONew.DateOfBirth.Month;
                        tblPersonTONew.DobYear = tblPersonTONew.DateOfBirth.Year;

                    }
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }

        /// <summary>
        /// swati pisal
        /// Get person details by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  TblPersonTO SelectAllTblPersonByUser(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson IN(SELECT [personId] FROM [tblUserExt] where [userId]=" + userId + " )";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                {
                    return list[0];
                }
                return null;
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
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public  int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblPersonTO tblPersonTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPerson]( " +
                                "  [salutationId]" +
                                " ,[mobileNo]" +
                                " ,[alternateMobNo]" +
                                " ,[phoneNo]" +
                                " ,[createdBy]" +
                                " ,[dateOfBirth]" +
                                " ,[createdOn]" +
                                " ,[firstName]" +
                                " ,[midName]" +
                                " ,[lastName]" +
                                " ,[primaryEmail]" +
                                " ,[alternateEmail]" +
                                " ,[comments]" +
                                " ,[photoBase64]" +
                                " )" +
                    " VALUES (" +
                                "  @SalutationId " +
                                " ,@MobileNo " +
                                " ,@AlternateMobNo " +
                                " ,@PhoneNo " +
                                " ,@CreatedBy " +
                                " ,@DateOfBirth " +
                                " ,@CreatedOn " +
                                " ,@FirstName " +
                                " ,@MidName " +
                                " ,@LastName " +
                                " ,@PrimaryEmail " +
                                " ,@AlternateEmail " +
                                " ,@Comments " +
                                " ,@photoBase64 " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            cmdInsert.Parameters.Add("@SalutationId", System.Data.SqlDbType.Int).Value = tblPersonTO.SalutationId;
            cmdInsert.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar).Value = tblPersonTO.MobileNo;
            cmdInsert.Parameters.Add("@AlternateMobNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateMobNo);
            cmdInsert.Parameters.Add("@PhoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhoneNo);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPersonTO.CreatedBy;
            cmdInsert.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfBirth);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPersonTO.CreatedOn;
            cmdInsert.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = tblPersonTO.FirstName;
            cmdInsert.Parameters.Add("@MidName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MidName);
            cmdInsert.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.LastName);
            cmdInsert.Parameters.Add("@PrimaryEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PrimaryEmail);
            cmdInsert.Parameters.Add("@AlternateEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateEmail);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.Comments);
            cmdInsert.Parameters.Add("@photoBase64", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhotoBase64);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblPersonTO.IdPerson = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public  int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPersonTO tblPersonTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPerson] SET " +
                            "  [salutationId]= @SalutationId" +
                            " ,[mobileNo]= @MobileNo" +
                            " ,[alternateMobNo]= @AlternateMobNo" +
                            " ,[phoneNo]= @PhoneNo" +
                            " ,[dateOfBirth]= @DateOfBirth" +
                            " ,[firstName]= @FirstName" +
                            " ,[midName]= @MidName" +
                            " ,[lastName]= @LastName" +
                            " ,[primaryEmail]= @PrimaryEmail" +
                            " ,[alternateEmail]= @AlternateEmail" +
                            " ,[comments] = @Comments" +
                            " ,[photoBase64] = @photoBase64" +
                            " WHERE  [idPerson] = @IdPerson";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            cmdUpdate.Parameters.Add("@SalutationId", System.Data.SqlDbType.Int).Value = tblPersonTO.SalutationId;
            cmdUpdate.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobileNo);
            cmdUpdate.Parameters.Add("@AlternateMobNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateMobNo);
            cmdUpdate.Parameters.Add("@PhoneNo", System.Data.SqlDbType.NVarChar, 20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhoneNo);
            cmdUpdate.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfBirth);
            cmdUpdate.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = tblPersonTO.FirstName;
            cmdUpdate.Parameters.Add("@MidName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MidName);
            cmdUpdate.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.LastName);
            cmdUpdate.Parameters.Add("@PrimaryEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PrimaryEmail);
            cmdUpdate.Parameters.Add("@AlternateEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateEmail);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.Comments);
            cmdUpdate.Parameters.Add("@photoBase64", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhotoBase64);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public  int DeleteTblPerson(Int32 idPerson)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
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

        public  int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPerson, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPerson] " +
            " WHERE idPerson = " + idPerson + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
