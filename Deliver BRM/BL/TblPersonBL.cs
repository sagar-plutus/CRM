using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblPersonBL
    {
        #region Selection
        public static List<TblPersonTO> SelectAllTblPersonList()
        {
            return TblPersonDAO.SelectAllTblPerson();
        }

        public static List<TblPersonTO> selectPersonsForOffline()
        {

            //return TblPersonDAO.selectPersonsForOffline();
            return new List<TblPersonTO>();
        }

        public static List<DropDownTO> selectPersonsDropdownForOffline()
        {

            return TblPersonDAO.selectPersonDropdownListOffline();
        }

        public static TblPersonTO SelectTblPersonTO(Int32 idPerson)
        {
            return TblPersonDAO.SelectTblPerson(idPerson);
        }


        /// <summary>
        /// Tejaswini [13-11-2018] Added for Getting Birthday or anniversory today
        /// </summary>
        /// <param name="BirthdayAlertTO"></param>
        /// <returns></returns>
        public static List<BirthdayAlertTO> SelectAllPersonBirthday(DateTime Today, Int32 UpcomingDays, Int32 IsBirthday)
        {
            List<BirthdayAlertTO> list = TblPersonDAO.SelectAllTblPersonByBirthdayAnniversory(Today, UpcomingDays, IsBirthday);
            return list;
        }

        /// <summary>
        /// Sudhir[21-June-2018]
        /// </summary>
        /// <param name="idPerson"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectDropDownListOnPersonId(Int32 idPerson)
        {
            return TblPersonDAO.SelectDropDownListOnPersonId(idPerson);
        }


        public static List<TblPersonTO> SelectAllPersonListByOrganization(int organizationId)
        {
            return TblPersonDAO.SelectAllTblPersonByOrganization(organizationId);
        }

        /// <summary>
        /// Sudhir[20-March-2018] Added for Get Person List On OrganizationId also in tblOrgPersonDtls.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<TblPersonTO> SelectAllPersonListByOrganizationV2(int organizationId, Int32 personTypeId)
        {
            return TblPersonDAO.SelectAllPersonByOrganizations(organizationId, personTypeId);
        }

        /// <summary>
        /// Sudhir[23-APR-2018] Added for GetPersonList Based on OrgType
        /// </summary>
        /// <param name="tblPersonTO"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectPersonBasedOnOrgType(Int32 OrgType)
        {
            List<DropDownTO> list= TblPersonDAO.SelectPersonsBasedOnOrgType(OrgType);
            if (list != null)
            {
                list = list.OrderBy(ele => ele.Text).ToList();
                return list;
            }
            else
                return null;
        }
        #endregion

        #region Insertion

        public static ResultMessage AddNewPerson(TblPersonTO tblPersonTO)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                result = TblPersonBL.InsertTblPerson(tblPersonTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While AddNewPerson");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal static TblPersonTO GetPersonOnUserId(int userId)
        {
            int PersonId = TblPersonDAO.GetPersonIdOnUserId(userId);

            return SelectTblPersonTO(PersonId);
        }



        public static ResultMessage AddNewPersonWithAddressDetails(TblPersonTO tblPersonTO, TblAddressTO tblAddressTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Insert into tblPerson
                result = TblPersonBL.InsertTblPerson(tblPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Inserting New Person");
                    return resultMessage;
                }

                tblAddressTO.CreatedOn = tblPersonTO.CreatedOn;
                tblAddressTO.CreatedBy = tblPersonTO.CreatedBy;
                //Insert in to tblAddress 
                result = TblAddressBL.InsertTblAddress(tblAddressTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Inserting Into TblAddress While adding  New Person");
                    return resultMessage;
                }

                if (result == 1)
                {
                    //Here Add Person And Address Mapping

                    TblPersonAddrDtlTO tblPersonAddrDtlTO = new TblPersonAddrDtlTO();
                    tblPersonAddrDtlTO.AddressId = tblAddressTO.IdAddr;
                    tblPersonAddrDtlTO.AddressTypeId = tblAddressTO.AddrTypeId;
                    tblPersonAddrDtlTO.PersonId = tblPersonTO.IdPerson;
                    tblPersonAddrDtlTO.CreatedBy = tblPersonTO.CreatedBy;
                    tblPersonAddrDtlTO.CreatedOn = tblPersonTO.CreatedOn;
                    result = TblPersonAddrDtlBL.InsertTblPersonAddrDtl(tblPersonAddrDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While Inserting into TblPersonAddrDtlTO While adding  New Person");
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour("Error into AddNewPersonWithAddressDetails");
                return resultMessage;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }
        public static int InsertTblPerson(TblPersonTO tblPersonTO)
        {
            return TblPersonDAO.InsertTblPerson(tblPersonTO);
        }

        public static int InsertTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonDAO.InsertTblPerson(tblPersonTO, conn, tran);
        }

        /// <summary>
        /// Sudhir[20-MARCH-2018] Added for Add Person based on OrganizationId and PersonTypeId.
        /// </summary>
        /// <param name="tblPersonTO"></param>
        /// <param name="organizationId"></param>
        /// <param name="personTypeId"></param>
        /// <returns></returns>
        public static ResultMessage SaveNewPersonAgainstOrganization(TblPersonTO tblPersonTO, Int32 organizationId, Int32 personTypeId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = TblPersonBL.InsertTblPerson(tblPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AddNewPersonWithOrgId");
                    return resultMessage;
                }

                TblOrgPersonDtlsTO tblOrgPersonDtlsTO = new TblOrgPersonDtlsTO();
                tblOrgPersonDtlsTO.PersonId = tblPersonTO.IdPerson;
                tblOrgPersonDtlsTO.OrganizationId = organizationId;
                tblOrgPersonDtlsTO.IsActive = 1;
                tblOrgPersonDtlsTO.CreatedBy = tblPersonTO.CreatedBy;
                tblOrgPersonDtlsTO.CreatedOn = tblPersonTO.CreatedOn;
                tblOrgPersonDtlsTO.PersonTypeId = personTypeId;

                result = TblOrgPersonDtlsBL.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AddNewPersonWithOrgId");
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error While Adding New Person with organizationId");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }

        #endregion

        #region Updation
        public static int UpdateTblPerson(TblPersonTO tblPersonTO)
        {
            return TblPersonDAO.UpdateTblPerson(tblPersonTO);
        }

        public static int UpdateTblPerson(TblPersonTO tblPersonTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonDAO.UpdateTblPerson(tblPersonTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public static int DeleteTblPerson(Int32 idPerson)
        {
            return TblPersonDAO.DeleteTblPerson(idPerson);
        }

        public static int DeleteTblPerson(Int32 idPerson, SqlConnection conn, SqlTransaction tran)
        {
            return TblPersonDAO.DeleteTblPerson(idPerson, conn, tran);
        }

        #endregion
        
    }
}
