using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;

namespace SalesTrackerAPI.DAL
{
    public class DimensionDAO
    {
        public static List<DropDownTO> SelectDeliPeriodForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectBookingTypeDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimBookingType WHERE isActive=1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idBookingType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idBookingType"].ToString());
                    if (dateReader["BookingTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["BookingTypeName"].ToString());

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
        /// Deepali[19-10-2018]added :to get Department wise Users

        internal static List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select URD.*,U.idUser,U.userDisplayName from tblUserReportingDetails URD left join tblOrgStructure OS"
                                   + " on OS.idOrgStructure = URD.orgStructureId "
                                   + "left join tblUser U on u.idUser = URD.userId "
                                   + "where U.isActive = 1 and OS.isActive = 1 and URD.isActive = 1 ";

                aqlQuery = aqlQuery + "and OS.deptId IN (" + deptId + ")";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

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
        public static List<DropDownTO> SelectLoadingLayerList()
        {

            String sqlConnStr = Startup.ConnectionString;
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


        public static List<DropDownTO> SelectCDStructureForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectCountriesForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimCountry ORDER BY  countryName";

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

        public static List<DropDownTO> SelectOrgLicensesForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectSalutationsForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectStatesForDropDown(int countryId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (countryId > 0)
                    sqlQuery = "SELECT * FROM dimState ORDER BY stateName ";  //No where condition. As we dont have country column in states
                else
                    sqlQuery = "SELECT * FROM dimState ORDER BY stateName ";


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

        public static List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            String orderByClause = " ORDER BY talukaName";
            try
            {

                conn.Open();
                if (districtId > 0)
                    sqlQuery = "SELECT * FROM dimTaluka WHERE districtId=" + districtId + orderByClause;
                else
                    sqlQuery = "SELECT * FROM dimTaluka " + orderByClause;


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

        public static List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectDefaultRoleListForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT Role.* FROM tblRole Role LEFT JOIN  dimOrgType OrgType  ON Role.idROle = OrgType.defaultRoleId AND Role.isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
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

        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users

        internal static List<DropDownTO> GetUserListDepartmentWise(int deptId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "select URD.*,U.idUser,U.userDisplayName from tblUserReportingDetails URD left join tblOrgStructure OS"
                + " on OS.idOrgStructure = URD.orgStructureId "
                + "left join tblUser U on u.idUser = URD.userId "
                + "where U.isActive = 1 and OS.isActive = 1 and URD.isActive = 1 ";

                aqlQuery = aqlQuery + "and OS.deptId =" + deptId;

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idUser"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idUser"].ToString());
                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["userDisplayName"].ToString());

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

        public static List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectAllTransportModeForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectInvoiceTypeForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectInvoiceModeForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> SelectCurrencyForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DropDownTO> GetInvoiceStatusForDropDown()
        {

            String sqlConnStr = Startup.ConnectionString;
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

        public static List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran)
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

        public static List<DimFinYearTO> SelectAllMstFinYearList()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimFinYear ";

                cmdSelect = new SqlCommand(aqlQuery, conn);
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
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        // Vaibhav [27-Sep-2017] added to select reporting type list
        public static List<DropDownTO> SelectReportingType()
        {
            String sqlConnStr = Startup.ConnectionString;
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
        public static List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList()
        {
            String sqlConnStr = Startup.ConnectionString;
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

        // Vijaymala [09-11-2017] added to get state code
        public static DropDownTO SelectStateCode(Int32 stateId)
        {
            String sqlConnStr = Startup.ConnectionString;
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


        /// <summary>
        /// Sanjay[2018-02-19] To Get dropdown list of Item Product Categories in the system
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimItemProdCateg WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idItemProdCat"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idItemProdCat"].ToString());
                    if (visitIssueReasonTODT["itemProdCategory"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["itemProdCategory"].ToString());
                    if (visitIssueReasonTODT["itemProdCategoryDesc"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToString(visitIssueReasonTODT["itemProdCategoryDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetItemProductCategoryListForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Priyanka [23-04-2019] : Added for get drop down list for SAP
        public static List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String SqlQuery = "SELECT * FROM dimGenericMaster WHERE dimensionId =" + dimensionId + " AND isActive=1";

                cmdSelect = new SqlCommand(SqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSAPMaster"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSAPMaster"].ToString());
                    if (dateReader["value"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["value"].ToString());
                    if (dateReader["dimensionId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["dimensionId"].ToString());

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
        public static List<Dictionary<string, string>> GetColumnName(string tablename, Int32 tableValue)
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlCommand cmdtblSelect = null;
            String sqlQuery = null;
            try
            {
                conn.Open();
                // String aqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION; SELECT * from " + tablename + "";
                if (tableValue > 0)
                {
                    sqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION;" +
                               "SELECT sapMaster.* from " + tablename + " sapMaster " +
                               "LEFT JOIN tblMasterDimension mstDimension ON mstDimension.idDimension = sapMaster.dimensionId " +
                               "WHERE sapMaster.dimensionId = " + tableValue;
                }
                else
                {
                    sqlQuery = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = " + "'" + tablename + "'" + " ORDER BY ORDINAL_POSITION; SELECT * from " + tablename + "";

                }
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<string> columnName = new List<string>();
                List<Dictionary<string, string>> main = new List<Dictionary<string, string>>();
                while (dateReader.Read())
                {
                    if (dateReader["COLUMN_NAME"] != DBNull.Value)
                        columnName.Add(Convert.ToString(dateReader["COLUMN_NAME"]));
                }
                if (dateReader.NextResult())
                {
                    if (dateReader.HasRows)
                    {
                        while (dateReader.Read())
                        {
                            Dictionary<string, string> hh = new Dictionary<string, string>();
                            for (int i = 0; i < columnName.Count; i++)
                            {
                                hh.Add(columnName[i], Convert.ToString(dateReader[columnName[i]]));
                            }
                            main.Add(hh);
                        }
                    }
                    else
                    {
                        Dictionary<string, string> hh = new Dictionary<string, string>();
                        for (int i = 0; i < columnName.Count; i++)
                        {
                            hh.Add(columnName[i], null);
                        }
                        main.Add(hh);
                    }

                }

                if (dateReader != null)
                    dateReader.Dispose();

                return main;
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

        public static Int32 InsertdimentionalData(string tableQuery)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                cmdInsert.CommandText = tableQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
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
        }

        public static List<DimensionTO> SelectAllMasterDimensionList()
        {

            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM tblMasterDimension";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimensionTO> dropDownTOList = new List<Models.DimensionTO>();
                while (dateReader.Read())
                {
                    DimensionTO dimensionTONew = new DimensionTO();
                    if (dateReader["idDimension"] != DBNull.Value)
                        dimensionTONew.IdDimension = Convert.ToInt32(dateReader["idDimension"].ToString());
                    if (dateReader["displayName"] != DBNull.Value)
                        dimensionTONew.DisplayName = Convert.ToString(dateReader["displayName"].ToString());
                    if (dateReader["dimensionValue"] != DBNull.Value)
                        dimensionTONew.DimensionValue = Convert.ToString(dateReader["dimensionValue"].ToString());
                    if (dateReader["isActive"] != DBNull.Value)
                        dimensionTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    if (dateReader["isGeneric"] != DBNull.Value)
                        dimensionTONew.IsGeneric = Convert.ToInt32(dateReader["isGeneric"].ToString());
                    dropDownTOList.Add(dimensionTONew);
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

        public static Int32 getidentityOfTable(string Query)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                cmdSelect = new SqlCommand(Query, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (dateReader.HasRows)
                {
                    return 1;
                }
                dateReader.Dispose();
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static Int32 getMaxCountOfTable(string CoulumName, string tableName)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = " select max(" + CoulumName + ") as cnt from " + tableName;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToInt32(dateReader);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[22-01-2018] Added for getStatusofInvoice.
        public static List<DropDownTO> GetInvoiceStatusDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceStatus";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvStatus"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvStatus"].ToString());
                    if (dateReader["statusName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["statusName"].ToString());
                    if (dateReader["statusDesc"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["statusDesc"].ToString());

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

        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public static List<DropDownTO> SelectAllFirmTypesForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM dimFirmType WHERE isActive=1 ";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idFirmType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idFirmType"].ToString());
                    if (dateReader["firmName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["firmName"].ToString());
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

        //Sudhir[07-MAR-2018] Added for All Firm Types List.
        public static List<DropDownTO> SelectAllInfluencerTypesForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "SELECT * FROM dimInfluencerType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInfluencerType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInfluencerType"].ToString());
                    if (dateReader["influencerTypeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["influencerTypeName"].ToString());
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

        public static List<DropDownTO> SelectAllOrganizationType()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "SELECT * FROM dimOrgType WHERE isActive=1";

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idOrgType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idOrgType"].ToString());
                    if (dateReader["OrgType"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["OrgType"].ToString());
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> GetCallBySelfForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimCallBySelfTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idCallBySelf"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idCallBySelf"].ToString());
                    if (visitIssueReasonTODT["callBySelfDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["callBySelfDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetCallBySelfForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> GetArrangeForDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeFor WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeFor"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeFor"].ToString());
                    if (visitIssueReasonTODT["arrangeForDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeForDesc"].ToString());

                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> GetArrangeVisitToDropDown()
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimArrangeVisitTo WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitIssueReasonTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (visitIssueReasonTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (visitIssueReasonTODT["idArrangeVisitTo"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(visitIssueReasonTODT["idArrangeVisitTo"].ToString());
                    if (visitIssueReasonTODT["arrangeVisitToDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(visitIssueReasonTODT["arrangeVisitToDesc"].ToString());
                    dropDownTOList.Add(dropDownTO);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetArrangeVisitToDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static List<DropDownTO> GetFixedDropDownList()
        {
            List<DropDownTO> dropDownToList = new List<DropDownTO>()
            {
                new DropDownTO {Text=" USER ",Value=1,Tag=1},
                new DropDownTO {Text=" STATE ",Value=2,Tag=0},
                new DropDownTO {Text=" DISTRICT",Value=3,Tag=0}
            };
            return dropDownToList;
        }

        //Priyanka [23-05-2018] : Added to get the invoice type according to their idInvoiceType.
        public static DropDownTO SelectInvoiceTypeForDropDownByIdInvoice(int idInvoiceType)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            DropDownTO dropDownTO = new DropDownTO();
            try
            {
                conn.Open();
                String aqlQuery = "SELECT * FROM dimInvoiceTypes WHERE  idInvoiceType= " + idInvoiceType + "AND isActive = 1";

                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idInvoiceType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idInvoiceType"].ToString());
                    if (dateReader["entityName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["entityName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }
                if (dropDownTOList != null && dropDownTOList.Count > 0)
                {
                    dropDownTO = dropDownTOList[0];
                }
                return dropDownTO;
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

        public static List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            {

                String sqlConnStr = Startup.ConnectionString;
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlCommand cmdSelect = null;
                SqlDataReader dateReader = null;
                try
                {
                    conn.Open();
                    String aqlQuery = "SELECT * FROM dimVisitType WHERE  isActive=1";

                    cmdSelect = new SqlCommand(aqlQuery, conn);
                    dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                    while (dateReader.Read())
                    {
                        DropDownTO dropDownTONew = new DropDownTO();
                        if (dateReader["idVisit"] != DBNull.Value)
                            dropDownTONew.Value = Convert.ToInt32(dateReader["idVisit"].ToString());
                        if (dateReader["visitType"] != DBNull.Value)
                            dropDownTONew.Text = Convert.ToString(dateReader["visitType"].ToString());

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

        }

        public static List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  [dbo].[tblCRMSiteType] WHERE parentSiteTypeId=" + parentSiteTypeId;

                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idSiteType"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idSiteType"].ToString());
                    if (dateReader["siteTypeDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["siteTypeDisplayName"].ToString());
                    if (dateReader["parentSiteTypeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["parentSiteTypeId"].ToString());

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

        public static int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
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

        public static int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
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

        internal static int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran)
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
        internal static int InsertMstFinYear(DimFinYearTO newMstFinYearTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {

                conn.Open();

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
                conn.Close();
                cmdInsert.Dispose();
            }
        }
        #endregion


        public static int GetModbusRefId(ModbusTO modbusTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                if (modbusTO.LoadingId != 0)
                    sqlQuery = "select IsNUll(modbusRefId,0) from temploading where idLoading=" + modbusTO.LoadingId;
                else if (!string.IsNullOrEmpty(modbusTO.LoadingSlipNo) && modbusTO.LoadingSlipNo != "undefined")
                    sqlQuery = "select IsNUll(modbusRefId,0) from temploading where loadingSlipNo='" + modbusTO.LoadingSlipNo + "'";
                else if (modbusTO.InvoiceId != 0)
                {
                    sqlQuery = "select IsNUll(modbusRefId,0) from temploading where idloading=( " +
        " select loadingId from temploadingSlip where idLoadingSlip=(select loadingSlipId from tempinvoice where idinvoice=" + modbusTO.InvoiceId + "))";
                }
                else if (!string.IsNullOrEmpty(modbusTO.LoadingSlipsNo) && modbusTO.LoadingSlipsNo != "undefined")
                    sqlQuery = "select IsNUll(modbusRefId,0) from temploading where idLoading=( " +
 " select loadingId from temploadingslip where loadingSlipNo='" + modbusTO.LoadingSlipsNo + "')";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                return Convert.ToInt32(cmdSelect.ExecuteScalar());
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public static DropDownTO GetPortNumberUsingModRef(int modbusRefId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();
                sqlQuery = "SELECT portNumber,machineIP,IoTUrl FROM temploading loading LEFT JOIN tblGate gate ON loading.gateId = gate.idGate WHERE modbusRefId =" + modbusRefId;
                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO dropDownTONew = new DropDownTO();
                while (dateReader.Read())
                {
                    if (dateReader["portNumber"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["portNumber"].ToString());
                    if (dateReader["machineIP"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["machineIP"].ToString());
                    if (dateReader["IoTUrl"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["IoTUrl"].ToString());
                }

                if (dateReader != null)
                    dateReader.Dispose();

                return dropDownTONew;
                //return Convert.ToInt32(cmdSelect.ExecuteScalar());
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
        public static Int64 GetProductItemIdFromGivenRMDetails(int prodCatId, int prodSpecId, int materialId, int brandId, int rmProdItemId)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            SqlDataReader dateReader = null;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM  tblProductItemRmToFGConfig WHERE  isFgToFgMapping=1 AND isActive=1 AND ISNULL(prodCatId,0)=" + prodCatId + " AND ISNULL(prodSpecId,0)=" + prodSpecId +
                          " AND ISNULL(materialId,0)=" + materialId + " AND ISNULL(brandId,0)=" + brandId + " AND ISNULL(rmProductItemId,0)=" + rmProdItemId;

                cmdSelect = new SqlCommand(sqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();  // Return first ever record as above condition should produce single record in the table
                    if (dateReader["fgProductItemId"] != DBNull.Value)
                        return Convert.ToInt64(dateReader["fgProductItemId"].ToString());
                }

                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (dateReader != null)
                    dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblOtherTaxesTO> GetAllActiveOtherTaxesList(SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblOtherTaxesTODT = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = Startup.ConnectionString;
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                cmdSelect.CommandText = "SELECT * FROM tblOtherTaxes WHERE isActive=1";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                tblOtherTaxesTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

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
                        if (tblOtherTaxesTODT["sequanceNo"] != DBNull.Value)
                            tblOtherTaxesTONew.SequanceNo = Convert.ToInt32(tblOtherTaxesTODT["sequanceNo"].ToString());
                        if (tblOtherTaxesTODT["createdOn"] != DBNull.Value)
                            tblOtherTaxesTONew.CreatedOn = Convert.ToDateTime(tblOtherTaxesTODT["createdOn"].ToString());
                        if (tblOtherTaxesTODT["defaultPct"] != DBNull.Value)
                            tblOtherTaxesTONew.DefaultPct = Convert.ToDouble(tblOtherTaxesTODT["defaultPct"].ToString());
                        if (tblOtherTaxesTODT["defaultAmt"] != DBNull.Value)
                            tblOtherTaxesTONew.DefaultAmt = Convert.ToDouble(tblOtherTaxesTODT["defaultAmt"].ToString());
                        if (tblOtherTaxesTODT["taxName"] != DBNull.Value)
                            tblOtherTaxesTONew.TaxName = Convert.ToString(tblOtherTaxesTODT["taxName"].ToString());
                        if (tblOtherTaxesTODT["sapExpenseCode"] != DBNull.Value)
                            tblOtherTaxesTONew.SapExpenseCode = Convert.ToString(tblOtherTaxesTODT["sapExpenseCode"].ToString());
                        tblOtherTaxesTOList.Add(tblOtherTaxesTONew);

                    }
                }
                return tblOtherTaxesTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tblOtherTaxesTODT != null)
                    tblOtherTaxesTODT.Dispose();
                if (tran == null)
                    conn.Close();
                cmdSelect.Dispose();
            }
        }
        public static List<TblProdGstCodeDtlsTO> getSAPTaxCodeByIdProdGstCode(int idProdGstCode)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = "select tblTaxRates.sapTaxCode, tblTaxRates.taxTypeId, tblProdGstCodeDtls.idProdGstCode from tblProdGstCodeDtls tblProdGstCodeDtls " +
                " JOIN tblTaxRates tblTaxRates on tblTaxRates.gstCodeId = tblProdGstCodeDtls.gstCodeId " +
                " where tblProdGstCodeDtls.idProdGstCode = " + idProdGstCode + "";
                cmdSelect = new SqlCommand(Query, conn);
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblProdGstCodeDtlsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdGstCodeDtlsTO> list = new List<TblProdGstCodeDtlsTO>();
                while (tblProdGstCodeDtlsTODT.Read())
                {
                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTONew = new TblProdGstCodeDtlsTO();
                    if (tblProdGstCodeDtlsTODT["idProdGstCode"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.IdProdGstCode = Convert.ToInt32(tblProdGstCodeDtlsTODT["idProdGstCode"].ToString());
                    if (tblProdGstCodeDtlsTODT["taxTypeId"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.TaxTypeId = Convert.ToInt32(tblProdGstCodeDtlsTODT["taxTypeId"].ToString());
                    if (tblProdGstCodeDtlsTODT["sapTaxCode"] != DBNull.Value)
                        tblProdGstCodeDtlsTONew.SapTaxCode = Convert.ToString(tblProdGstCodeDtlsTODT["sapTaxCode"].ToString());
                    list.Add(tblProdGstCodeDtlsTONew);
                }

                if (tblProdGstCodeDtlsTODT != null)
                    tblProdGstCodeDtlsTODT.Dispose();
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
    }

}
