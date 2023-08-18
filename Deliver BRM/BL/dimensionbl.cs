using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace SalesTrackerAPI.BL
{
    public class DimensionBL
    {

        #region Selection
        public static List<DropDownTO> SelectDeliPeriodForDropDown()
        {
            return DimensionDAO.SelectDeliPeriodForDropDown();
        }

        public static List<DropDownTO> SelectBookingTypeDropDown()
        {
            return DimensionDAO.SelectBookingTypeDropDown();
        }
        public static List<DropDownTO> SelectCDStructureForDropDown()
        {
            return DimensionDAO.SelectCDStructureForDropDown();
        }

        public static List<DropDownTO> SelectCountriesForDropDown()
        {
            return DimensionDAO.SelectCountriesForDropDown();
        }

        public static List<DropDownTO> SelectStatesForDropDown(int countryId)
        {
            return DimensionDAO.SelectStatesForDropDown(countryId);
        }
        public static List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {
            return DimensionDAO.SelectDistrictForDropDown(stateId);
        }

        public static List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {
            return DimensionDAO.SelectTalukaForDropDown(districtId);
        }

        public static List<DropDownTO> SelectOrgLicensesForDropDown()
        {
            return DimensionDAO.SelectOrgLicensesForDropDown();
        }

        public static List<DropDownTO> SelectSalutationsForDropDown()
        {
            return DimensionDAO.SelectSalutationsForDropDown();
        }

        public static List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {
            return DimensionDAO.SelectRoleListWrtAreaAllocationForDropDown();

        }

        public static List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {
            return DimensionDAO.SelectAllSystemRoleListForDropDown();

        }

        public static List<DropDownTO> SelectDefaultRoleListForDropDown()
        {
            return DimensionDAO.SelectDefaultRoleListForDropDown();
        }
        
        public static List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {
            return DimensionDAO.SelectCnfDistrictForDropDown(cnfOrgId);

        }

        public static List<DropDownTO> SelectAllTransportModeForDropDown()
        {
            return DimensionDAO.SelectAllTransportModeForDropDown();

        }

        public static List<DropDownTO> SelectInvoiceTypeForDropDown()
        {
            return DimensionDAO.SelectInvoiceTypeForDropDown();

        }


        public static List<DropDownTO> SelectInvoiceModeForDropDown()
        {
            return DimensionDAO.SelectInvoiceModeForDropDown();

        }
        public static List<DropDownTO> SelectCurrencyForDropDown()
        {
            return DimensionDAO.SelectCurrencyForDropDown();

        }

        public static List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            return DimensionDAO.GetInvoiceStatusForDropDown();

        }

        internal static List<DropDownTO> GetUserListDepartmentWise(string deptId)
        {
            return DimensionDAO.GetUserListDepartmentWise(deptId);

        }
        public static List<DropDownTO> SelectLoadingLayerList()
        {
            return DimensionDAO.SelectLoadingLayerList();
        }

        //Priyanka [23-04-2019]
        public static List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {
            return DimensionDAO.GetSAPMasterDropDown(dimensionId);
        }
        public static TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                DimFinYearTO curFinYearTO = GetCurrentFinancialYear(createdOn, conn, tran);
                if (curFinYearTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                TblEntityRangeTO EntityRangeTO = BL.TblEntityRangeBL.SelectEntityRangeTOFromInvoiceType(entityName, curFinYearTO.IdFinYear, conn, tran);
                EntityRangeTO.EntityPrevValue = EntityRangeTO.EntityPrevValue + EntityRangeTO.IncrementBy;
                result = BL.TblEntityRangeBL.UpdateTblEntityRange(EntityRangeTO);
                if (result == 0)
                {
                    tran.Rollback();
                    return null;
                }
                tran.Commit();
                return EntityRangeTO;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        public static DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran)
        {
            List<DimFinYearTO> mstFinYearTOList = DimensionDAO.SelectAllMstFinYearList(conn, tran);
            for (int i = 0; i < mstFinYearTOList.Count; i++)
            {
                DimFinYearTO mstFinYearTO = mstFinYearTOList[i];
                if (curDate >= mstFinYearTO.FinYearStartDate &&
                    curDate <= mstFinYearTO.FinYearEndDate)
                    return mstFinYearTO;
            }

            //Means Current Financial year not found so insert it
            DateTime startDate = Constants.GetStartDateTimeOfYear(curDate);
            DateTime endDate = Constants.GetEndDateTimeOfYear(curDate);
            int finYear = startDate.Year;
            DimFinYearTO newMstFinYearTO = new DimFinYearTO();
            newMstFinYearTO.FinYearDisplayName = finYear + "-" + (finYear + 1);
            newMstFinYearTO.FinYearEndDate = endDate;
            newMstFinYearTO.IdFinYear = finYear;
            newMstFinYearTO.FinYearStartDate = startDate;
            int result = DimensionDAO.InsertMstFinYear(newMstFinYearTO, conn, tran);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }


        public static DimFinYearTO GetCurrentFinancialYear(DateTime curDate)
        {
            List<DimFinYearTO> mstFinYearTOList = DimensionDAO.SelectAllMstFinYearList();
            for (int i = 0; i < mstFinYearTOList.Count; i++)
            {
                DimFinYearTO mstFinYearTO = mstFinYearTOList[i];
                if (curDate >= mstFinYearTO.FinYearStartDate &&
                    curDate <= mstFinYearTO.FinYearEndDate)
                    return mstFinYearTO;
            }

            //Means Current Financial year not found so insert it
            DateTime startDate = Constants.GetStartDateTimeOfYear(curDate);
            DateTime endDate = Constants.GetEndDateTimeOfYear(curDate);
            int finYear = startDate.Year;
            DimFinYearTO newMstFinYearTO = new DimFinYearTO();
            newMstFinYearTO.FinYearDisplayName = finYear + "-" + (finYear + 1);
            newMstFinYearTO.FinYearEndDate = endDate;
            newMstFinYearTO.IdFinYear = finYear;
            newMstFinYearTO.FinYearStartDate = startDate;
            int result = DimensionDAO.InsertMstFinYear(newMstFinYearTO);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }


        // Vaibhav [27-Sep-2017] added to select all reporting type list
        public static List<DropDownTO> GetReportingType()
        {
            List<DropDownTO> reportingTypeList = DAL.DimensionDAO.SelectReportingType();
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public static List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            List<DimVisitIssueReasonsTO> visitIssueReasonList = DimensionDAO.SelectVisitIssueReasonsList();
            if (visitIssueReasonList != null)
                return visitIssueReasonList;
            else
                return null;
        }
        // Vijaymala [09-11-2017] added to get state Code
        public static DropDownTO SelectStateCode(Int32 stateId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = DimensionDAO.SelectStateCode(stateId);
                if (dropDownTO != null)
                    return dropDownTO;
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
        }

        public static List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            return DimensionDAO.GetItemProductCategoryListForDropDown();
        }


        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> GetCallBySelfForDropDown()
        {
            return DimensionDAO.GetCallBySelfForDropDown();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> GetArrangeForDropDown()
        {
            return DimensionDAO.GetArrangeForDropDown();
        }


        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public static List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return DimensionDAO.GetArrangeVisitToDropDown();
        }

        //Sudhir[22-01-2018] Added  for GetInvoiceStatusList.
        public static List<DropDownTO> GetInvoiceStatusDropDown()
        {
            return DimensionDAO.GetInvoiceStatusDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public static List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            return DimensionDAO.SelectAllFirmTypesForDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Influencer List.
        public static List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            return DimensionDAO.SelectAllInfluencerTypesForDropDown();
        }

        //Sudhir[24-APR-2018] Added for Get All Organization Type.
        public static List<DropDownTO> GetAllOrganizationType()
        {
            return DimensionDAO.SelectAllOrganizationType();
        }

        //Priyanka [23-05-2018] Added to get the invoice tax type according to idInvoiceType
        public static DropDownTO SelectInvoiceTypeForDropDownByIdInvoice(Int32 idInvoiceType)
        {
            return DimensionDAO.SelectInvoiceTypeForDropDownByIdInvoice(idInvoiceType);

        }
        public static List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            return DimensionDAO.SelectAllVisitTypeListForDropDown();
        }

        public static List<DropDownTO> GetFixedDropDownValues()
        {
            return DimensionDAO.GetFixedDropDownList();
        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        public static List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = TblRoleOrgSettingBL.SelectSavedRoles(visitTypeId, personTypeId);
            list = DimensionDAO.SelectAllSystemRoleListForDropDown();


            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Role = list[i].Text;
                roleorgTO.RoleId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }
                roleOrgTOList.Add(roleorgTO);
            }

            return roleOrgTOList;
        }
        public static List<RoleOrgTO> SelectAllSystemOrgListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            list = DimensionDAO.SelectAllOrganizationType();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = TblRoleOrgSettingBL.SelectSavedOrg(visitTypeId, personTypeId);



            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Org = list[i].Text;
                roleorgTO.OrgId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }

                roleOrgTOList.Add(roleorgTO);
            }
            return roleOrgTOList;
        }

        public static List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            return DimensionDAO.SelectMasterSiteTypes(parentSiteTypeId);
        }


        #endregion

        #region Insertion

        public static int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimensionDAO.InsertTaluka(commonDimensionsTO, conn, tran);
        }

        public static int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return DimensionDAO.InsertDistrict(commonDimensionsTO, conn, tran);
        }

        //Kiran[15-MAR-2018] Added for Select All Dimension Tables from tblMasterDimension
        public static List<DimensionTO> SelectAllMasterDimensionList()
        {
            return DimensionDAO.SelectAllMasterDimensionList();

        }

        public static List<Dictionary<string, string>> GetColumnName(string tableName, Int32 tableValue)
        {
            return DimensionDAO.GetColumnName(tableName, tableValue);
        }


        //Kiran[15-MAR-2018] Added for New Dimension in  Selected Dim Tables 
        public static Int32 saveNewDimensional(Dictionary<string, string> tableData, string tableName)
        {
            var result = 0;
            int cnt = 0;
            string query = "INSERT INTO " + tableName + "( ";
            string values = "VALUES(";
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    result = getIdentityOfTable(item.Key, tableName);
                    if (result > 0)
                    {
                        query += item.Key + ",";
                        values += "'" + result + "',";
                    }
                }
                else
                {
                    query += item.Key + ",";
                    values += "'" + item.Value + "',";
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + ")" + values.TrimEnd(',') + ")";
            return DimensionDAO.InsertdimentionalData(executeQuery);
        }

        //Kiran[15-MAR-2018] Added for Update Selected Dim Tables 
        public static Int32 UpdateDimensionalData(Dictionary<string, string> tableData, string tableName)
        {
            Int32 result = 0;
            string query = "UPDATE " + tableName + " SET ";
            string value = "";
            Int32 cnt = 0;
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    value += item.Key + " = " + item.Value;
                }
                else
                {
                    string stree = string.Empty;
                    if (item.Value != null)
                        if (item.Value.Contains("PM") || item.Value.Contains("AM"))
                        {
                            DateTime dt = Convert.ToDateTime(item.Value);
                            stree = dt.ToString("yyyy-MM-dd HH:MM");
                        }
                    if (stree == string.Empty)
                        query += item.Key + " = '" + item.Value + "',";
                    else
                    {
                        query += item.Key + " = '" + stree + "',";
                        stree = string.Empty;
                    }
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + " WHERE " + value;
            return DimensionDAO.InsertdimentionalData(executeQuery);
        }

        //Kiran[15-MAR-2018] Added for Validate Identity of Selected Tables Coloumn.
        public static Int32 getIdentityOfTable(string columnName, string tableName)
        {

            var query = "SELECT name FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = " + "'" + tableName + "'";
            Int32 result = DimensionDAO.getidentityOfTable(query);
            if (result == 0)
            {
                return DimensionDAO.getMaxCountOfTable(columnName, tableName) + 1;
            }
            return 0;
        }

        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users
        ///
        internal static List<DropDownTO> GetUserListDepartmentWise(int deptId)
        {
            return DimensionDAO.GetUserListDepartmentWise(deptId);

        }

        #endregion

#region  iotsupport
   public static int GetModbusRefId(ModbusTO modbusTO)
   {
       return DimensionDAO.GetModbusRefId(modbusTO);
   }

        public static DropDownTO GetPortNumberUsingModRef(int modbusRefId)
        {
            return DimensionDAO.GetPortNumberUsingModRef(modbusRefId);
        }

        #endregion
    }
}

