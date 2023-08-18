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
    public class DimensionDAO : Idimensiondao
    {
        private readonly IConnectionString _iConnectionString;
        public DimensionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        public  List<DropDownTO> SelectDeliPeriodForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimDelPeriod WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDelPeriod"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDelPeriod"].ToString());
                    if (dateReader["deliveryPeriod"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["deliveryPeriod"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectCDStructureForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCdStructure WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCdStructure"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCdStructure"].ToString());
                    if (dateReader["cdValue"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["cdValue"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectDimMasterValues(Int32 masterId)
        {

            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimMasterValue WHERE isActive = 1 AND masterId = " + masterId + " ORDER BY seqNo ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idMasterValue"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idMasterValue"].ToString());
                    if (dateReader["masterValueName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["masterValueName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectAddOnFunDtls(Int32 transId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblAddonsFunDtls WHERE isActive = 1 AND transId = " + transId;


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["transId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["transId"].ToString());
                    if (dateReader["transType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["transType"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        /// <summary>
        /// Priyanka [19-02-2019]
        /// </summary>
        /// <param name="masterId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId)
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimMasterValue WHERE isActive = 1 AND parentMasterValueId = " + parentMasterValueId + " ORDER BY seqNo ";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idMasterValue"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idMasterValue"].ToString());
                    if (dateReader["masterValueName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["masterValueName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectCountriesForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCountry";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCountry"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCountry"].ToString());
                    if (dateReader["countryName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["countryName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }



        public  List<DropDownTO> SelectOrgLicensesForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCommerLicenceInfo WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLicense"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLicense"].ToString());
                    if (dateReader["licenseName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["licenseName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectAllSystemUsersListFromRoleType(int roleTypeId)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                //String aqlQuery = "select distinct roleId from tblUser userm " +
                //                 " left join tblUserRole on userm.iduser = tblUserRole.userId" +
                //                 " left join tblRole on   tblUserRole.roleId = tblRole.idRole where roleTypeId = " + roleTypeId + " AND tblRole.isActive = 1 ";


                String aqlQuery = "select idRole,roleDesc from tblRole" +
                                 "  where tblRole.roleTypeId = " + roleTypeId + " AND tblRole.isActive = 1 ";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());

                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectAllSystemUsersFromRoleType(int roleTypeId)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                //Prajakta[2019-05-28]Added tblUserRole.isActive=1 condition
                String aqlQuery = "select distinct userId,userDisplayName from tblUser userm " +
                                 " left join tblUserRole on userm.iduser = tblUserRole.userId" +
                                 " left join tblRole on   tblUserRole.roleId = tblRole.idRole where roleTypeId = " + roleTypeId + " and tblUserRole.isActive=1 and userm.isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["userId"].ToString());

                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = dateReader["userDisplayName"].ToString();

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)

            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectLocationForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblLocation";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLocation"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLocation"].ToString());
                    if (dateReader["locationDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["locationDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectSalutationsForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimSalutation";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSalutation"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSalutation"].ToString());
                    if (dateReader["salutationDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["salutationDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (stateId > 0)
                    sqlQuery = "SELECT * FROM dimDistrict WHERE stateId=" + stateId;
                else
                    sqlQuery = "SELECT * FROM dimDistrict ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idDistrict"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idDistrict"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectStatesForDropDown(int countryId)
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (countryId > 0)
                    sqlQuery = "SELECT * FROM dimState ";  //No where condition. As we dont have country column in states
                else
                    sqlQuery = "SELECT * FROM dimState ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idState"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idState"].ToString());
                    if (dateReader["stateName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["stateName"].ToString());
                    if (dateReader["stateOrUTCode"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["stateOrUTCode"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId;
                else
                    sqlQuery = "SELECT * FROM dimTaluka ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTaluka"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTaluka"].ToString());
                    if (dateReader["talukaName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["talukaName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE enableAreaAlloc=1 AND isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }


        public  List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblRole WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idRole"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idRole"].ToString());
                    if (dateReader["roleDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["roleDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = " SELECT distinct districtId,dimDistrict.districtName FROM tblOrganization  " +
                           " LEFT JOIN tblOrgAddress ON idOrganization = organizationId " +
                           " LEFT JOIN tblAddress ON idAddr = addressId " +
                           " LEFT JOIN dimDistrict ON idDistrict = districtId " +
                           " WHERE tblOrganization.isActive=1 AND tblOrganization.idOrganization IN(SELECT dealerOrgId FROM tblCnfDealers WHERE cnfOrgId=" + cnfOrgId + " and isActive=1) " +
                           " ORDER BY dimDistrict.districtName ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["districtId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["districtId"].ToString());
                    if (dateReader["districtName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["districtName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectAllTransportModeForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimTransportMode WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idTransMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idTransMode"].ToString());
                    if (dateReader["transportMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["transportMode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectInvoiceTypeForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceTypes WHERE  isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceType"].ToString());
                    if (dateReader["invoiceTypeDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceTypeDesc"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectInvoiceModeForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceMode WHERE  1=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceMode"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceMode"].ToString());
                    if (dateReader["invoiceMode"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["invoiceMode"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> SelectCurrencyForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DropDownTO> GetInvoiceStatusForDropDown()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCurrency";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idCurrency"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idCurrency"].ToString());
                    if (dateReader["currencyName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["currencyName"].ToString());
                    if (dateReader["currencySymbol"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["currencySymbol"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public  List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                String aqlQuery = "SELECT * FROM dimFinYear ";

                cmdSelect = new SqlCommand(aqlQuery, conn, tran);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimFinYearTO> finYearTOList = new List<DimFinYearTO>();
                while (dateReader.Read())
                {
                    DimFinYearTO finYearTO = new DimFinYearTO();
                    if (dateReader["idFinYear"] != DBNull.Value)
                        finYearTO.IdFinYear = Convert.ToInt32(dateReader["idFinYear"].ToString());
                    if (dateReader["finYearDisplayName"] != DBNull.Value)
                        finYearTO.FinYearDisplayName = Convert.ToString(dateReader["finYearDisplayName"].ToString());
                    if (dateReader["finYearStartDate"] != DBNull.Value)
                        finYearTO.FinYearStartDate = Convert.ToDateTime(dateReader["finYearStartDate"].ToString());
                    if (dateReader["finYearEndDate"] != DBNull.Value)
                        finYearTO.FinYearEndDate = Convert.ToDateTime(dateReader["finYearEndDate"].ToString());

                    finYearTOList.Add(finYearTO);
                }

                return finYearTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                cmdSelect.Dispose();
            }

        }

        public List<int> GeModRefMaxData()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255  modbusRefId FROM tblPurchaseScheduleSummary WHERE modbusRefId IS NOT NULL GROUP BY modbusRefId ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<int> GeModRefMaxDataNonMulti()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr  = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255 modbusRefId FROM tblPurchaseScheduleSummary WHERE modbusRefId IS NOT NULL ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [27-Sep-2017] added to select reporting type list
        public  List<DropDownTO> SelectReportingType()
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM dimReportingType WHERE isActive= 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader reportingTypeTO = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (reportingTypeTO.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reportingTypeTO["idReportingType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reportingTypeTO["idReportingType"].ToString());
                    if (reportingTypeTO["reportingTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reportingTypeTO["reportingTypeName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public  List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList()
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimVisitIssueReasons WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DimVisitIssueReasonsTO> visitIssueReasonTOList = new List<DimVisitIssueReasonsTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DimVisitIssueReasonsTO dimVisitIssueReasonsTONew = new DimVisitIssueReasonsTO();
                    if (visitIssueReasonTODT["idVisitIssueReasons"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IdVisitIssueReasons = Convert.ToInt32(visitIssueReasonTODT["idVisitIssueReasons"].ToString());
                    if (visitIssueReasonTODT["issueTypeId"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.IssueTypeId = Convert.ToInt32(visitIssueReasonTODT["issueTypeId"].ToString());
                    if (visitIssueReasonTODT["visitIssueReasonName"] != DBNull.Value)
                        dimVisitIssueReasonsTONew.VisitIssueReasonName = Convert.ToString(visitIssueReasonTODT["visitIssueReasonName"].ToString());

                    visitIssueReasonTOList.Add(dimVisitIssueReasonsTONew);
                }
                return visitIssueReasonTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectReportingType");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> SelectBrandList()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimBrand WHERE isActive=1 ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idBrand"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idBrand"].ToString());
                    if (dateReader["brandName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["brandName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// [2017-01-02]Vijaymala:Added to get loading layer list 
        /// </summary>
        /// <returns></returns>
        public  List<DropDownTO> SelectLoadingLayerList()
        {

             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimLoadingLayers WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idLoadingLayer"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idLoadingLayer"].ToString());
                    if (dateReader["layerDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["layerDesc"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        // Vijaymala [09-11-2017] added to get state code
        public  DropDownTO SelectStateCode(Int32 stateId)
        {
             String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = "select idState,stateOrUTCode from dimState  WHERE  idState = " + stateId;

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTO = new DropDownTO();
                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        if (departmentTODT["idState"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idState"].ToString());
                        if (departmentTODT["stateOrUTCode"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["stateOrUTCode"].ToString());
                    }
                }
                return dropDownTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //ReshmaP[19-01-2022] For Internal Transfer
        public List<DropDownTO> SelectVehicalType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimVehicalType   ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idVehicalType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idVehicalType"].ToString());
                    if (dateReader["VehicalTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["VehicalTypeName"].ToString());

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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #region Insertion

        public  int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimTaluka]( " +
                            "  [districtId]" +
                            " ,[talukaCode]" +
                            " ,[talukaName]" +
                            " )" +
                " VALUES (" +
                            "  @districtId " +
                            " ,@talukaCode " +
                            " ,@talukaName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@districtId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@talukaCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@talukaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
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

        public  int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimDistrict]( " +
                            "  [stateId]" +
                            " ,[districtCode]" +
                            " ,[districtName]" +
                            " )" +
                " VALUES (" +
                            "  @stateId " +
                            " ,@districtCode " +
                            " ,@districtName " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                String sqlSelectIdentityQry = "Select @@Identity";

                cmdInsert.Parameters.Add("@stateId", System.Data.SqlDbType.Int).Value = commonDimensionsTO.ParentId;
                cmdInsert.Parameters.Add("@districtCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionCode);
                cmdInsert.Parameters.Add("@districtName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(commonDimensionsTO.DimensionName);
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    cmdInsert.CommandText = sqlSelectIdentityQry;
                    commonDimensionsTO.IdDimension = Convert.ToInt32(cmdInsert.ExecuteScalar());
                    return 1;
                }
                else return 0;
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

        public  int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;

                String sqlQuery = @" INSERT INTO [dimFinYear]( " +
                            "  [idFinYear]" +
                            " ,[finYearDisplayName]" +
                            " ,[finYearStartDate]" +
                            " ,[finYearEndDate]" +
                            " )" +
                " VALUES (" +
                            "  @idFinYear " +
                            " ,@finYearDisplayName " +
                            " ,@finYearStartDate " +
                            " ,@finYearEndDate " +
                            " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@idFinYear", System.Data.SqlDbType.Int).Value = newMstFinYearTO.IdFinYear;
                cmdInsert.Parameters.Add("@finYearDisplayName", System.Data.SqlDbType.NVarChar).Value = newMstFinYearTO.FinYearDisplayName;
                cmdInsert.Parameters.Add("@finYearStartDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearStartDate;
                cmdInsert.Parameters.Add("@finYearEndDate", System.Data.SqlDbType.DateTime).Value = newMstFinYearTO.FinYearEndDate;
                return cmdInsert.ExecuteNonQuery();
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

        //ReshmaP Added For Inter Transfer
        public int SaveDimGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            int result = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                string smdInsert = "insert into dimGenericMaster (value,isActive,dimensionId,parentIdGenericMaster) " +
                    "  values ( @value,@isActive,@dimensionId,@parentIdGenericMaster )";

                cmdInsert.CommandText = smdInsert;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@value", System.Data.SqlDbType.NVarChar).Value = DimGenericMasterTO.Value ;
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int ).Value = DimGenericMasterTO.IsActive ;
                cmdInsert.Parameters.Add("@dimensionId", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.DimensionId ;
                cmdInsert.Parameters.Add("@parentIdGenericMaster", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(DimGenericMasterTO.ParentIdGenericMaster);
                return cmdInsert.ExecuteNonQuery();
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
            return result;
        }
        public int UpdateDimGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            int result = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                string smdInsert = "update dimGenericMaster set value =@Values,isActive=@isActive,parentIdGenericMaster=@parentIdGenericMaster where idGenericMaster =@idGenericMaster";

                cmdInsert.CommandText = smdInsert;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@Values", System.Data.SqlDbType.NVarChar).Value = DimGenericMasterTO.Value;
                cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.IsActive;
                cmdInsert.Parameters.Add("@parentIdGenericMaster", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(DimGenericMasterTO.ParentIdGenericMaster);
                cmdInsert.Parameters.Add("@idGenericMaster", System.Data.SqlDbType.Int).Value = DimGenericMasterTO.IdGenericMaster ;
                return cmdInsert.ExecuteNonQuery();
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
            return result;
        }
        public List<DimGenericMasterTO > GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster= 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = @"SELECT dimGenericMaster.*, parentGenericMaster.value as ParentName FROM dimGenericMaster dimGenericMaster
                LEFT JOIN dimGenericMaster parentGenericMaster on parentGenericMaster.idGenericMaster = dimGenericMaster.parentIdGenericMaster
                WHERE dimGenericMaster.dimensionId = " + IdDimension;
                if(SkipIsActiveFilter == 0)
                {
                    sqlQuery += " AND dimGenericMaster.isActive = 1";
                }
                if(ParentIdGenericMaster > 0)
                {
                    sqlQuery += " AND dimGenericMaster.parentIdGenericMaster = "+ ParentIdGenericMaster;
                }
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimGenericMasterTO> dropDownTOList = new List<Models.DimGenericMasterTO>();
                while (dateReader.Read())
                {
                    DimGenericMasterTO dropDownTONew = new DimGenericMasterTO();
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Value   = Convert.ToString (dateReader["value"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dropDownTONew.IsActive = Convert.ToInt16  (dateReader["isActive"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.DimensionId  = Convert.ToInt16(dateReader["dimensionId"].ToString());
                    if (dateReader["mappedTxnId"] != DBNull.Value)
                        dropDownTONew.MappedTxnId = Convert.ToString (dateReader["mappedTxnId"].ToString());
                    if (dateReader["idGenericMaster"] != DBNull.Value)
                        dropDownTONew.IdGenericMaster  = Convert.ToInt16(dateReader["idGenericMaster"].ToString());
                    if (dateReader["parentIdGenericMaster"] != DBNull.Value)
                        dropDownTONew.ParentIdGenericMaster = Convert.ToInt16(dateReader["parentIdGenericMaster"].ToString());
                    if (dateReader["ParentName"] != DBNull.Value)
                        dropDownTONew.ParentName = Convert.ToString(dateReader["ParentName"].ToString());
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetApprovalUserList(String idUserStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT idUser,userDisplayName FROM tblUser WHERE idUser IN(" + idUserStr + ") AND isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dropDownTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (dropDownTODT != null)
                {
                    while (dropDownTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (dropDownTODT["idUser"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(dropDownTODT["idUser"].ToString());
                        if (dropDownTODT["userDisplayName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(dropDownTODT["userDisplayName"].ToString());
                        list.Add(dropDownTO);
                    }
                }
                if (dropDownTODT != null)
                    dropDownTODT.Dispose();
                if (list != null)
                    return list;
                else
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

        public Boolean CheckAleradyAvailableData(DimGenericMasterTO DimGenericMasterTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimGenericMaster where dimensionId =" + DimGenericMasterTO.DimensionId +
                    " and UPPER(value)=UPPER ('" + DimGenericMasterTO.Value + "') ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                 
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false ;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }

        public Boolean CheckExistingData(DimGenericMasterTO DimGenericMasterTO)
        {
            Boolean isAvailable = false;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimGenericMaster  where idGenericMaster !=" + DimGenericMasterTO.IdGenericMaster  +
                " AND dimensionId = " + DimGenericMasterTO.DimensionId + " and UPPER(value)=UPPER('" + DimGenericMasterTO.Value + "')";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    isAvailable = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
            return isAvailable;
        }
        #endregion

        #region Execute Command

        public  int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandText = cmdStr;

                cmdDelete.CommandType = System.Data.CommandType.Text;

                return cmdDelete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -2;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        #endregion

    }
}
