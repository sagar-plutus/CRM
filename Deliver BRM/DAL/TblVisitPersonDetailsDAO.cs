using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using SalesTrackerAPI.StaticStuff;
using SalesTrackerAPI.Models;

namespace SalesTrackerAPI
{
    public class TblVisitPersonDetailsDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPersonVisitDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetails(int visitTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " SELECT DISTINCT  person.idPerson, Concat(firstName,' ',lastName) as 'displayName', mobileNo,alternateMobNo,primaryEmail, " +
                                  " visitpersondetails.visitRoleId " + //visitpersondetails.visitId  " +
                                  " FROM tblVisitPersonDetails visitpersondetails INNER JOIN tblVisitRole  visitRole " +
                                  " ON visitRole.idVisitRole = visitpersondetails.visitRoleId INNER JOIN tblPerson person " +
                                  " ON person.idPerson = visitpersondetails.personId "+
                                  " INNER JOIN tblVisitDetails tblvisitdetials ON tblvisitdetials.idVisit=visitpersondetails.visitId "+
                                  " WHERE tblvisitdetials.visitTypeId = " + visitTypeId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitPersonDetailsTO> list = ConvertDTToList(visitPersonDetailsDT);
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Added to get personDtls for offline [Hrushikesh]
        public static List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String ids)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                conn.Open();
                String sqlQuery = " SELECT person.idPerson," +
                                  " CASE " +
                                  " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                                  " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                                  " END " +
                                  " AS Name, " +
                                  "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId,orgPersonDtls.organizationId " +
                                   " FROM tblPerson person" +
                                   " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                   " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                   " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId OR person.idPerson=organization.secondOwnerPersonId " +
                                   " LEFT JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization ";
                String whereCond = " WHERE orgPersonDtls.personTypeId in ( " + ids + " )";

                if (!String.IsNullOrEmpty(ids))
                {
                    sqlQuery = sqlQuery + whereCond;
                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value && visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                        {
                            tblVisitPersonDetailsTO.IdPerson = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString() + visitPersonDetailsDT["personTypeId"].ToString());
                            tblVisitPersonDetailsTO.PersonId = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString() + visitPersonDetailsDT["personTypeId"].ToString());
                        }

                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            tblVisitPersonDetailsTO.DisplayName = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            tblVisitPersonDetailsTO.PersonTypeId = Convert.ToInt32(visitPersonDetailsDT["personTypeId"].ToString());
                        if (visitPersonDetailsDT["organizationId"] != DBNull.Value)
                            tblVisitPersonDetailsTO.OrganizationId = Convert.ToInt32(visitPersonDetailsDT["organizationId"].ToString());
                        tblVisitPersonDetailsTOList.Add(tblVisitPersonDetailsTO);
                    }
                }
                return tblVisitPersonDetailsTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static DataTable SelectTblPersonVisitDetails(Int32 personId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE personId = " + personId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public static DataTable SelectAllTblPersonVisitDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public static List<TblVisitPersonDetailsTO> ConvertDTToList(SqlDataReader visitPersonDetailsDT)
        {
            List<TblVisitPersonDetailsTO> visitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        TblVisitPersonDetailsTO visitPersonDetailsTONew = new TblVisitPersonDetailsTO(); 
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            visitPersonDetailsTONew.IdPerson = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["displayName"] != DBNull.Value)
                            visitPersonDetailsTONew.DisplayName = visitPersonDetailsDT["displayName"].ToString().TrimEnd('-');
                        //if (visitPersonDetailsDT["midName"] != DBNull.Value)
                        //    visitPersonDetailsTONew.MidName = visitPersonDetailsDT["midName"].ToString();
                        //if (visitPersonDetailsDT["lastName"] != DBNull.Value)
                        //    visitPersonDetailsTONew.LastName = visitPersonDetailsDT["lastName"].ToString();
                        if (visitPersonDetailsDT["mobileNo"] != DBNull.Value)
                            visitPersonDetailsTONew.MobileNo = visitPersonDetailsDT["mobileNo"].ToString();
                        if (visitPersonDetailsDT["alternateMobNo"] != DBNull.Value)
                            visitPersonDetailsTONew.AlternateMobNo = visitPersonDetailsDT["alternateMobNo"].ToString();
                        if (visitPersonDetailsDT["primaryEmail"] != DBNull.Value)
                            visitPersonDetailsTONew.PrimaryEmail = visitPersonDetailsDT["primaryEmail"].ToString();
                        if (visitPersonDetailsDT["visitRoleId"] != DBNull.Value)
                            visitPersonDetailsTONew.VisitRoleId = Convert.ToInt32(visitPersonDetailsDT["visitRoleId"].ToString());
                        //if (visitPersonDetailsDT["visitId"] != DBNull.Value)
                        //    visitPersonDetailsTONew.VisitId = Convert.ToInt32(visitPersonDetailsDT["visitId"].ToString());
                       
                        visitPersonDetailsTOList.Add(visitPersonDetailsTONew);
                    }
                }
                return visitPersonDetailsTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ConvertDTToList");
                return null;
            }
        }

        //Sudhir - Added for Get All Visit Person TypeList.
        public static List<DropDownTO> SelectAllVisitPersonTypeList()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " SELECT * FROM dimPersonType ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPersonType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPersonType"].ToString());
                        if (visitPersonDetailsDT["personType"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["personType"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                //String sqlQuery = " SELECT CRMPersonLinking.personId,(ISNULL(person.firstName,'') +' '+ ISNULL(person.lastName,'')) as Name,CRMPersonLinking.personTypeId FROM tblCRMPersonLinking " +
                //    " CRMPersonLinking " +
                //    "LEFT JOIN tblPerson person ON CRMPersonLinking.personId = person.idPerson  " +
                //    "LEFT JOIN tblOrgPersonDtls orgPersonDtls ON CRMPersonLinking.personId = orgPersonDtls.personId" +
                //    " WHERE CRMPersonLinking.personTypeId = "+ personType + " OR orgPersonDtls.personTypeId="+ personType;

                String sqlQuery = " SELECT person.idPerson," +
                                  " CASE " +
                                  " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                                  " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                                  " END " +
                                  " AS Name, " +
                                  "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId " +
                                   " FROM tblPerson person" +
                                   " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                   " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                   " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId OR person.idPerson=organization.secondOwnerPersonId " +
                                   " LEFT JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization " +
                                   " WHERE " +
                                   //" CRMPersonLinking.personTypeId = " + personType + " OR orgPersonDtls.personTypeId = " + personType + " Order BY person.idPerson ";
                                   "(orgPersonDtls.personTypeId = " + personType + ") AND(CRMPersonLinking.personTypeId =" + personType + ")  " +
                                   //" AND(organization.orgTypeId NOT IN(9) " + " OR    organization2.orgTypeId NOT IN(9)) " + //Sudhir[16-July-2018] Commmentd
                                   " Order BY person.idPerson";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(visitPersonDetailsDT["personTypeId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[17-July-2-2018] Added For More Filtering Data by PersonType and OrganizationId.
        public static List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType,int? organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string whereCond = String.Empty;
                if (organizationId > 0)
                    whereCond = " AND (orgPersonDtls.organizationId=" + organizationId + ")";
                else
                    whereCond = String.Empty;

                conn.Open();

                //String sqlQuery = " SELECT CRMPersonLinking.personId,(ISNULL(person.firstName,'') +' '+ ISNULL(person.lastName,'')) as Name,CRMPersonLinking.personTypeId FROM tblCRMPersonLinking " +
                //    " CRMPersonLinking " +
                //    "LEFT JOIN tblPerson person ON CRMPersonLinking.personId = person.idPerson  " +
                //    "LEFT JOIN tblOrgPersonDtls orgPersonDtls ON CRMPersonLinking.personId = orgPersonDtls.personId" +
                //    " WHERE CRMPersonLinking.personTypeId = "+ personType + " OR orgPersonDtls.personTypeId="+ personType;



                String sqlQuery = " SELECT person.idPerson," +
                                  " CASE " +
                                  " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                                  " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                                  " END " +
                                  " AS Name, " +
                                  "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId " +
                                   " FROM tblPerson person" +
                                   " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                   " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                   " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId OR person.idPerson=organization.secondOwnerPersonId " +
                                   " LEFT JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization " +
                                   " WHERE " +
                                   //" CRMPersonLinking.personTypeId = " + personType + " OR orgPersonDtls.personTypeId = " + personType + " Order BY person.idPerson ";
                                   "(orgPersonDtls.personTypeId = " + personType + ") AND(CRMPersonLinking.personTypeId =" + personType + ")  " +
                                   //" AND(organization.orgTypeId NOT IN(9) " + " OR    organization2.orgTypeId NOT IN(9)) " + //Sudhir[16-July-2018] Commmentd
                                   whereCond + " Order BY person.idPerson";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(visitPersonDetailsDT["personTypeId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static int SelectVisitPersonCount(int visitId,int personId,int persontypeId,SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                String sqlQuery = " SELECT COUNT(personId) FROM tblVisitPersonDetails WHERE personId= " + personId + " AND visitRoleId = " + persontypeId + " AND visitId =" + visitId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                return Convert.ToInt32(cmdSelect.ExecuteScalar());

                //return cmdSelect.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return -1;
            }
            finally
            {
                //conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Select Visit person role list
        public static List<DropDownTO> SelectVisitPersonRoleListForDropDown()
        {
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();
                String sqlQuery = " SELECT idVisitRole,visitRoleName,visitPersonTypeId FROM tblVisitRole visitRole "+
                                  " INNER JOIN  tblVisitPersonRole visitPersonRole "+
                                  " ON visitRole.idVisitRole = visitPersonRole.visitRoleId ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitRoleDT = cmdSelect.ExecuteReader(CommandBehavior.Default);


                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitRoleDT != null)
                {
                    while (visitRoleDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitRoleDT["idVisitRole"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitRoleDT["idVisitRole"].ToString());
                        if (visitRoleDT["visitRoleName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitRoleDT["visitRoleName"].ToString());
                        if (visitRoleDT["visitPersonTypeId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(visitRoleDT["visitPersonTypeId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectVisitPersonRoleListForDropDown");
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
        public static int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitPersonDetailsTO, cmdInsert);
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

        public static int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitPersonDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPersonVisitDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitPersonDetails]( " +
            "  [personId]" +
            " ,[visitRoleId]" +
            " ,[visitId]" +
            " )" +
            " VALUES (" +
            "  @PersonId " +
            " ,@VisitRoleId " +
            " ,@VisitId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonId;
            cmdInsert.Parameters.Add("@VisitRoleId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitRoleId;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public static int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitPersonDetailsTO, cmdUpdate);
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

        public static int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitPersonDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblPersonVisitDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitPersonDetails] SET " +
            "  [personId] = @PersonId" +

            " WHERE  [personId] = @PersonId AND  [visitRoleId]= @VisitRoleId AND  [visitId] = @VisitId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonId;
            cmdUpdate.Parameters.Add("@VisitRoleId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitRoleId;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public static int DeleteTblPersonVisitDetails(Int32 personId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(personId, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public static int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(personId, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 personId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPersonVisitDetails] " +
            " WHERE personId = " + personId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@personId", System.Data.SqlDbType.Int).Value = tblPersonVisitDetailsTO.PersonId;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
