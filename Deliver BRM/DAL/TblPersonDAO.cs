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
    public class TblPersonDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT person.*,sal.salutationDesc FROM tblPerson person " +
                                  " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public static List<TblPersonTO> SelectAllTblPerson()
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static List<TblPersonTO> selectPersonsForOffline()
        {
            String sqlQuery = "SELECT  idPersonType,personType,person.*,orgPersonDtls.organizationId,org.orgTypeId, " +
                    "  orgPersonDtls.personTypeId, sal.salutationDesc FROM tblPerson person" +
                    "  LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId  " +
                    "  LEFT JOIN tblOrgPersonDtls orgPersonDtls ON person.idPerson = orgPersonDtls.personId  " +
                    "  LEFT JOIN dimPersonType personType ON personType.idPersonType = orgPersonDtls.personTypeId " +
                    "  LEFT JOIN tblOrganization org ON orgPersonDtls.organizationId = org.idOrganization ";


            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPersonTO> list = ConvertDTToListForOffline(sqlReader);
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

        public static List<DropDownTO> selectPersonDropdownListOffline()
        {
            String sqlQuery = " SELECT personId AS idPerson , concat(firstName, ' ', lastName, ', ', roleDesc) as personName,15 as " +
                " orgTypeId FROM tblUser JOIN tblUserExt ON tblUser.idUser = tblUserExt.userId " +
                "   JOIN tblUserRole ON tblUser.idUser = tblUserRole.userId" +
                "   JOIN tblRole ON tblUserRole.roleId = tblRole.idRole" +
                "   JOIN tblPerson ON tblPerson.idPerson = tblUserExt.personId" +
                " WHERE tblUserRole.isActive = 1 AND tblUser.isActive = 1" +
                " UNION ALL " +
                                     " SELECT DISTINCT person.idPerson ," +
                                     " CASE " +
                                     " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName" +
                                     " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName" +
                                     " WHEN organization.firmName IS NULL THEN person.firstName + ' ' + person.lastName" +
                                   " END " +
                                    " AS personName," +
                                    " CASE " +
                                    " WHEN organization3.orgTypeId IS NOT NULL THEN organization3.orgTypeId " +
                                    "  WHEN organization.orgTypeId IS NOT NULL THEN organization.orgTypeId " +
                                    " END " +
                                    "AS orgTypeId " +
                                    " FROM tblPerson person " +
                                    " LEFT JOIN tblOrganization organization ON organization.firstOwnerPersonId = person.idPerson OR organization.secondOwnerPersonId = person.idPerson " +
                                    " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                    " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization ";

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = ConvertDTToListDropdownForOffline(sqlReader);
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


        public static List<TblPersonTO> SelectAllTblPersonByOrganization(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        public static List<TblPersonTO> SelectAllPersonByOrganizations(Int32 organizationId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                                        " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ")" +
                                        " OR idPerson IN(SELECT personId FROM tblOrgPersonDtls WHERE organizationId = " + organizationId + ")";

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

        internal static int GetPersonIdOnUserId(int userId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblUserExt where userId=" + userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                TblPersonTO tblPersonTONew = new TblPersonTO();
                while (sqlReader.Read())
                {
                    if (sqlReader["personId"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(sqlReader["personId"].ToString());
                }
                return tblPersonTONew.IdPerson;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblPersonTO> SelectAllPersonByOrganizations(Int32 organizationId, Int32 personTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String whereCond = String.Empty;
            try
            {
                if (personTypeId > 0)
                    whereCond = "  ) " + "AND personTypeId = " + personTypeId;
                //"OR idPerson IN (SELECT personId FROM tblCRMPersonLinking WHERE PersonTypeId="+personTypeId+") ";
                else
                    whereCond = ")";
                String strPersonType = String.Empty;
                if (personTypeId == 1 || personTypeId == 0)
                {
                    strPersonType = " OR idPerson IN(SELECT firstOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") " +
                                       " OR idPerson IN(SELECT secondOwnerPersonId FROM tblOrganization WHERE idOrganization = " + organizationId + ") ";
                }
                conn.Open();
                cmdSelect.CommandText = " SELECT " +
                                       //"  CASE WHEN CRMpersonType.idPersonType IS NULL THEN CRMpersonType.personType ELSE CRMpersonType.idPersonType END AS idPersonType, " +
                                       //"  CASE WHEN CRMpersonType.personType IS NULL THEN personType.personType ELSE CRMpersonType.personType END AS personType "+
                                       " idPersonType,personType,person.*,sal.salutationDesc FROM tblPerson person " +
                                       " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId  " +
                                       " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON person.idPerson = orgPersonDtls.personId " +
                                       " LEFT JOIN dimPersonType personType ON personType.idPersonType = orgPersonDtls.personTypeId " +
                                       //" LEFT JOIN tblCRMPersonLinking CRMLinking ON CRMLinking.personId=person.idPerson " +
                                       //" LEFT JOIN dimPersonType CRMpersonType ON CRMpersonType.idPersonType = CRMLinking.personTypeId "+
                                       " WHERE " + 
                                       " idPerson IN (SELECT personId FROM tblOrgPersonDtls where organizationId=" + organizationId +
                                       whereCond + " " + strPersonType;


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

        public static TblPersonTO SelectTblPerson(Int32 idPerson)
        {
            String sqlConnStr = Startup.ConnectionString;
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

        /// <summary>
        /// Sudhir[21-JUNE-2018] 
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectDropDownListOnPersonId(int personId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText =" SELECT OrgpersonDtls.organizationId,Organization.firmName,OrgpersonDtls.personTypeId FROM tblPerson person " +
                                       " LEFT JOIN dimSalutation sal ON sal.idSalutation = person.salutationId " +
                                       " LEFT JOIN tblOrgpersonDtls OrgpersonDtls ON person.idPErson = OrgpersonDtls.personId " +
                                       " LEFT JOIN tblOrganization Organization ON OrgpersonDtls.organizationId = Organization.idOrganization " +
                                       " WHERE idPerson = " + personId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["organizationId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["organizationId"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
                    if (dateReader["personTypeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["personTypeId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTOList;
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

        /// <summary>
        /// Sudhir[23-APR-2018] Added for Get Persons Based on Orgaization Type.
        /// </summary>
        /// <param name="tblPersonTODT"></param>
        /// <returns></returns>
        /// 
        public static List<DropDownTO> SelectPersonsBasedOnOrgType(Int32 OrgType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {

                //person.idPerson,person.firstName+''+person.lastName  AS personName
                conn.Open();

                if(OrgType == (int)Constants.OrgTypeE.USERS)
                {
                    cmdSelect.CommandText = "SELECT personId AS idPerson,concat(firstName,' ',lastName,', ',roleDesc) as personName,tblUser.idUser  FROM tblUser " +
                                            " JOIN tblUserExt ON tblUser.idUser = tblUserExt.userId " +
                                            " JOIN tblUserRole ON tblUser.idUser = tblUserRole.userId  " +
                                            " JOIN tblRole ON tblUserRole.roleId = tblRole.idRole " +
                                            " JOIN tblPerson ON tblPerson.idPerson = tblUserExt.personId "+
                                            " WHERE tblUserRole.isActive=1 AND tblUser.isActive=1";
                }
                else
                {
                    #region Old Code Commented on 25-July-2018 By Sudhir
                    //cmdSelect.CommandText = " SELECT person.idPerson ," +
                    //                    " CASE " +
                    //                    " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName " +
                    //                    " WHEN organization2.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization2.firmName " +
                    //                    " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName " +
                    //                    " WHEN organization.firmName IS NULL AND organization2.firmName IS NULL THEN person.firstName + ' ' + person.lastName " +
                    //                    " END " +
                    //                    " AS personName,NULL as idUser FROM tblPerson person " +
                    //                    " LEFT JOIN tblOrganization organization ON organization.firstOwnerPersonId = person.idPerson " +
                    //                    " LEFT JOIN tblOrganization organization2 ON organization2.secondOwnerPersonId = person.idPerson " +
                    //                    " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                    //                    " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization " +
                    //                    " WHERE orgPersonDtls.organizationId IN(SELECT idOrganization FROM tblOrganization WHERE orgTypeId = " + OrgType + ") " +
                    //                    " OR organization.orgTypeId =" + OrgType + " OR organization2.orgTypeId = " + OrgType;
                    #endregion

                    cmdSelect.CommandText = " SELECT DISTINCT person.idPerson ," +
                                        " CASE " +
                                        " WHEN organization.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization.firmName " +
                                        " WHEN organization3.firmName IS NOT NULL THEN person.firstName + ' ' + person.lastName + ',' + organization3.firmName " +
                                        " WHEN organization.firmName IS NULL THEN person.firstName + ' ' + person.lastName " +
                                        " END " +
                                        " AS personName,NULL as idUser FROM tblPerson person " +
                                        " LEFT JOIN tblOrganization organization ON organization.firstOwnerPersonId = person.idPerson " +
                                        //" OR organizkation.secondOwnerPersonId = person.idPerson " + //Sudhir[2018-08-26] commented for taking time
                                        " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                        " LEFT JOIN tblOrganization organization3 ON orgPersonDtls.organizationId = organization3.idOrganization " +
                                        " WHERE orgPersonDtls.organizationId IN(SELECT idOrganization FROM tblOrganization WHERE orgTypeId = " + OrgType + ") " +
                                        " OR organization.orgTypeId =" + OrgType  ;

                }


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (sqlReader.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (sqlReader["idPerson"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(sqlReader["idPerson"].ToString());
                    if (sqlReader["personName"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(sqlReader["personName"].ToString());
                    if (sqlReader["idUser"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToInt32(sqlReader["idUser"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
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
        /// Birthdays or anniversory notifications - Tejaswini
        public static List<BirthdayAlertTO> SelectAllTblPersonByBirthdayAnniversory(DateTime Today, Int32 upcomingDays, Int32 IsBirthday)
        {
            string BirthdaysCondition = null;
            if (upcomingDays == 0)
            {
                if (IsBirthday == 1)
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfBirth) = " + Today.Day + " AND DATEPART(m , Person.dateOfBirth) = " + Today.Month;
                }
                else if (IsBirthday == 0)
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfAnniversary) = " + Today.Day + " AND DATEPART(m , Person.dateOfAnniversary) = " + Today.Month;
                }
                else
                {
                    BirthdaysCondition = " DATEPART(d , Person.dateOfBirth) = " + Today.Day + " AND DATEPART(m , Person.dateOfBirth) = " + Today.Month +
                        " OR DATEPART(d, Person.dateOfAnniversary) =  " + Today.Day + "  AND DATEPART(m, Person.dateOfAnniversary) = " + Today.Month;
                }
            }
            else
            {
                // BirthdaysCondition = " Person.dateOfBirth between convert(datetime,'" + fromDate + "',105) AND convert(datetime,'" + toDate +
                //     "',105) OR Person.dateOfAnniversary between convert(datetime,'" + fromDate + "',105) AND convert(datetime,'" + toDate + "',105)";

                if (IsBirthday == 1)
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()) / 365.25)) ";

                }
                else if (IsBirthday == 0)
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()) / 365.25)) ";

                }
                else
                {
                    BirthdaysCondition = " 1 = (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfBirth,GETDATE()) / 365.25)) " +
                                    " OR 1 = (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()+ " + upcomingDays + ") / 365.25))- (FLOOR(DATEDIFF(dd, Person.dateOfAnniversary,GETDATE()) / 365.25))";

                }
            }
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT Person.*, Salutation.salutationDesc,RoleDtls.idRole, RoleDtls.roleDesc, OrgPersonDtls.organizationId , Organization.* ,PersonType.idPersonType, PersonType.personType FROM tblPerson Person " +
                    "LEFT JOIN tblUserExt UserExt ON   UserExt.personId = Person.idPerson " +
                    "LEFT JOIN tbluserrole Userrole ON   Userrole.userId = UserExt.userId " +
                    "LEFT JOIN tblrole RoleDtls ON   RoleDtls.idRole = Userrole.roleId " +
                    "LEFT JOIN tblOrgPersonDtls OrgPersonDtls ON OrgPersonDtls.personId = Person.idPerson " +
                    "LEFT JOIN tblOrganization Organization ON  Organization.idOrganization = OrgPersonDtls.organizationId " +
                    "LEFT JOIN dimPersonType PersonType ON PersonType.idPersonType = OrgPersonDtls.personTypeId " +
                    "LEFT JOIN dimSalutation Salutation ON Salutation.idSalutation = Person.salutationId " +
                    " WHERE " +
                    BirthdaysCondition + " AND Organization.isActive = 1 AND OrgPersonDtls.isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<BirthdayAlertTO> list = ConvertBirthdayDTToList(sqlReader);
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

        //Convert to BirthdayAlertTO list - Tejaswini
        public static List<BirthdayAlertTO> ConvertBirthdayDTToList(SqlDataReader tblBirthdayPersonTODT)
        {
            List<BirthdayAlertTO> tblPersonTOList = new List<BirthdayAlertTO>();
            if (tblBirthdayPersonTODT != null)
            {
                while (tblBirthdayPersonTODT.Read())
                {
                    BirthdayAlertTO tblBirthdayTONew = new BirthdayAlertTO();
                    if (tblBirthdayPersonTODT["idPerson"] != DBNull.Value)
                        tblBirthdayTONew.IdPerson = Convert.ToInt32(tblBirthdayPersonTODT["idPerson"].ToString());
                    if (tblBirthdayPersonTODT["salutationId"] != DBNull.Value)
                        tblBirthdayTONew.SalutationId = Convert.ToInt32(tblBirthdayPersonTODT["salutationId"].ToString());
                    if (tblBirthdayPersonTODT["mobileNo"] != DBNull.Value)
                        tblBirthdayTONew.MobileNo = Convert.ToString(tblBirthdayPersonTODT["mobileNo"].ToString());
                    if (tblBirthdayPersonTODT["alternateMobNo"] != DBNull.Value)
                        tblBirthdayTONew.AlternateMobNo = Convert.ToString(tblBirthdayPersonTODT["alternateMobNo"].ToString());
                    if (tblBirthdayPersonTODT["phoneNo"] != DBNull.Value)
                        tblBirthdayTONew.PhoneNo = Convert.ToString(tblBirthdayPersonTODT["phoneNo"].ToString());
                    if (tblBirthdayPersonTODT["createdBy"] != DBNull.Value)
                        tblBirthdayTONew.CreatedBy = Convert.ToInt32(tblBirthdayPersonTODT["createdBy"].ToString());
                    if (tblBirthdayPersonTODT["dateOfBirth"] != DBNull.Value)
                        tblBirthdayTONew.DateOfBirth = Convert.ToDateTime(tblBirthdayPersonTODT["dateOfBirth"].ToString());
                    if (tblBirthdayPersonTODT["createdOn"] != DBNull.Value)
                        tblBirthdayTONew.CreatedOn = Convert.ToDateTime(tblBirthdayPersonTODT["createdOn"].ToString());
                    if (tblBirthdayPersonTODT["firstName"] != DBNull.Value)
                        tblBirthdayTONew.FirstName = Convert.ToString(tblBirthdayPersonTODT["firstName"].ToString());
                    if (tblBirthdayPersonTODT["midName"] != DBNull.Value)
                        tblBirthdayTONew.MidName = Convert.ToString(tblBirthdayPersonTODT["midName"].ToString());
                    if (tblBirthdayPersonTODT["lastName"] != DBNull.Value)
                        tblBirthdayTONew.LastName = Convert.ToString(tblBirthdayPersonTODT["lastName"].ToString());
                    if (tblBirthdayPersonTODT["primaryEmail"] != DBNull.Value)
                        tblBirthdayTONew.PrimaryEmail = Convert.ToString(tblBirthdayPersonTODT["primaryEmail"].ToString());
                    if (tblBirthdayPersonTODT["alternateEmail"] != DBNull.Value)
                        tblBirthdayTONew.AlternateEmail = Convert.ToString(tblBirthdayPersonTODT["alternateEmail"].ToString());
                    if (tblBirthdayPersonTODT["comments"] != DBNull.Value)
                        tblBirthdayTONew.Comments = Convert.ToString(tblBirthdayPersonTODT["comments"].ToString());
                    if (tblBirthdayPersonTODT["salutationDesc"] != DBNull.Value)
                        tblBirthdayTONew.SalutationName = Convert.ToString(tblBirthdayPersonTODT["salutationDesc"].ToString());
                    if (tblBirthdayPersonTODT["photoBase64"] != DBNull.Value)
                        tblBirthdayTONew.PhotoBase64 = Convert.ToString(tblBirthdayPersonTODT["photoBase64"].ToString());
                    if (tblBirthdayPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblBirthdayTONew.DateOfAnniversary = Convert.ToDateTime(tblBirthdayPersonTODT["dateOfAnniversary"].ToString());
                    if (tblBirthdayPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblBirthdayTONew.OtherDesignationId = Convert.ToInt32(tblBirthdayPersonTODT["otherDesignationId"].ToString());
                    if (tblBirthdayTONew.DateOfBirth != DateTime.MinValue)
                    {
                        tblBirthdayTONew.DobDay = tblBirthdayTONew.DateOfBirth.Day;
                        tblBirthdayTONew.DobMonth = tblBirthdayTONew.DateOfBirth.Month;
                        tblBirthdayTONew.DobYear = tblBirthdayTONew.DateOfBirth.Year;
                    }
                    if (tblBirthdayTONew.DateOfAnniversary != DateTime.MinValue)
                    {
                        tblBirthdayTONew.AnniDay = tblBirthdayTONew.DateOfAnniversary.Day;
                        tblBirthdayTONew.AnniMonth = tblBirthdayTONew.DateOfAnniversary.Month;
                        tblBirthdayTONew.AnniYear = tblBirthdayTONew.DateOfAnniversary.Year;
                    }
                    if (tblBirthdayPersonTODT["idOrganization"] != DBNull.Value)
                        tblBirthdayTONew.IdOrganization = Convert.ToInt32(tblBirthdayPersonTODT["idOrganization"].ToString());

                    if (tblBirthdayPersonTODT["firmName"] != DBNull.Value)
                        tblBirthdayTONew.FirmName = Convert.ToString(tblBirthdayPersonTODT["firmName"].ToString());
                    if (tblBirthdayPersonTODT["website"] != DBNull.Value)
                        tblBirthdayTONew.Website = Convert.ToString(tblBirthdayPersonTODT["website"].ToString());
                    if (tblBirthdayPersonTODT["personType"] != DBNull.Value)
                        tblBirthdayTONew.PersonType = Convert.ToString(tblBirthdayPersonTODT["personType"].ToString());
                    if (tblBirthdayPersonTODT["idPersonType"] != DBNull.Value)
                        tblBirthdayTONew.IdPersonType = Convert.ToInt32(tblBirthdayPersonTODT["idPersonType"].ToString());
                    if (tblBirthdayPersonTODT["idRole"] != DBNull.Value)
                        tblBirthdayTONew.IdRole = Convert.ToInt32(tblBirthdayPersonTODT["idRole"].ToString());
                    if (tblBirthdayPersonTODT["roleDesc"] != DBNull.Value)
                        tblBirthdayTONew.RoleDesc = Convert.ToString(tblBirthdayPersonTODT["roleDesc"].ToString());

                    tblPersonTOList.Add(tblBirthdayTONew);
                }
            }
            return tblPersonTOList;
        }

        public static List<TblPersonTO> ConvertDTToList(SqlDataReader tblPersonTODT)
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
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTONew.DateOfBirth!=DateTime.MinValue)
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

        public static List<DropDownTO> ConvertDTToListDropdownForOffline(SqlDataReader tblPersonTODT)
        {
            //hrushikesh
            //adding uniqueness to personid as there are same person for diffrent orgType
            List<DropDownTO> tblPersonTOList = new List<DropDownTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {
                    DropDownTO tblPersonTONew = new DropDownTO();
                    if ((tblPersonTODT["idPerson"] != DBNull.Value) && (tblPersonTODT["orgTypeId"] != DBNull.Value))
                        tblPersonTONew.Value = Convert.ToInt32(tblPersonTODT["idPerson"].ToString() + (tblPersonTODT["orgTypeId"].ToString()));
                    else if ((tblPersonTODT["idPerson"] != DBNull.Value))
                        tblPersonTONew.Value = Convert.ToInt32(tblPersonTODT["idPerson"].ToString());
                    if (tblPersonTODT["personName"] != DBNull.Value)
                        tblPersonTONew.Text = Convert.ToString(tblPersonTODT["personName"].ToString());
                    if (tblPersonTODT["orgTypeId"] != DBNull.Value)
                        tblPersonTONew.Tag = Convert.ToString(tblPersonTODT["orgTypeId"].ToString());
                    tblPersonTOList.Add(tblPersonTONew);
                }
            }
            return tblPersonTOList;
        }


        public static List<TblPersonTO> ConvertDTToListForOffline(SqlDataReader tblPersonTODT)
        {
            List<TblPersonTO> tblPersonTOList = new List<TblPersonTO>();
            if (tblPersonTODT != null)
            {
                while (tblPersonTODT.Read())
                {
                    TblPersonTO tblPersonTONew = new TblPersonTO();
                    if (tblPersonTODT["idPerson"] != DBNull.Value && tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.IdPerson = Convert.ToInt32(tblPersonTODT["idPerson"].ToString() + Convert.ToInt32(tblPersonTODT["personTypeId"].ToString()));
                    else if (tblPersonTODT["idPerson"] != DBNull.Value)
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
                    if (tblPersonTODT["dateOfAnniversary"] != DBNull.Value)
                        tblPersonTONew.DateOfAnniversary = Convert.ToDateTime(tblPersonTODT["dateOfAnniversary"].ToString());
                    if (tblPersonTODT["otherDesignationId"] != DBNull.Value)
                        tblPersonTONew.OtherDesignationId = Convert.ToInt32(tblPersonTODT["otherDesignationId"].ToString());
                    if (tblPersonTODT["organizationId"] != DBNull.Value)
                        tblPersonTONew.OrganizationId = Convert.ToInt32(tblPersonTODT["organizationId"].ToString());
                    if (tblPersonTODT["personTypeId"] != DBNull.Value)
                        tblPersonTONew.PersonTypeId = Convert.ToInt32(tblPersonTODT["personTypeId"].ToString());
                    if (tblPersonTODT["orgTypeId"] != DBNull.Value)
                        tblPersonTONew.OrgTypeId = Convert.ToInt32(tblPersonTODT["orgTypeId"].ToString());
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


        #endregion

        #region Insertion
        public static int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
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

        public static int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPersonTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblPersonTO tblPersonTO, SqlCommand cmdInsert)
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
                                " ,[dateOfAnniversary]" +
                                " ,[otherDesignationId]" +
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
                                " ,@dateOfAnniversary" +
                                " ,@OtherDesignationId" +
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
            cmdInsert.Parameters.Add("@dateOfAnniversary", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfAnniversary);
            cmdInsert.Parameters.Add("@OtherDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.OtherDesignationId);
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
        public static int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
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

        public static int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPersonTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblPersonTO tblPersonTO, SqlCommand cmdUpdate)
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
                            " ,[dateOfAnniversary]=@dateOfAnniversary" +
                            " ,[otherDesignationId]=@OtherDesignationId" +
                            " WHERE  [idPerson] = @IdPerson"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            cmdUpdate.Parameters.Add("@SalutationId", System.Data.SqlDbType.Int).Value = tblPersonTO.SalutationId;
            cmdUpdate.Parameters.Add("@MobileNo", System.Data.SqlDbType.NVarChar,20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MobileNo);
            cmdUpdate.Parameters.Add("@AlternateMobNo", System.Data.SqlDbType.NVarChar,20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateMobNo);
            cmdUpdate.Parameters.Add("@PhoneNo", System.Data.SqlDbType.NVarChar,20).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhoneNo);
            cmdUpdate.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfBirth);
            cmdUpdate.Parameters.Add("@FirstName", System.Data.SqlDbType.NVarChar).Value = tblPersonTO.FirstName;
            cmdUpdate.Parameters.Add("@MidName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.MidName);
            cmdUpdate.Parameters.Add("@LastName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.LastName);
            cmdUpdate.Parameters.Add("@PrimaryEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PrimaryEmail);
            cmdUpdate.Parameters.Add("@AlternateEmail", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.AlternateEmail);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.Comments);
            cmdUpdate.Parameters.Add("@photoBase64", System.Data.SqlDbType.NText).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.PhotoBase64);
            cmdUpdate.Parameters.Add("@dateOfAnniversary", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.DateOfAnniversary);
            cmdUpdate.Parameters.Add("@OtherDesignationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPersonTO.OtherDesignationId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public static int DeleteTblPerson(Int32 idPerson)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
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

        public static int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPerson, cmdDelete);
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

        public static int ExecuteDeletionCommand(Int32 idPerson, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPerson] " +
            " WHERE idPerson = " + idPerson +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPerson", System.Data.SqlDbType.Int).Value = tblPersonTO.IdPerson;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
