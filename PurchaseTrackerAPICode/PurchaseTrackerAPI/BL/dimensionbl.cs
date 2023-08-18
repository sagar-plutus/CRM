using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class DimensionBL : Idimensionbl
    {
        private readonly Idimensiondao _iDimensiondao;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;

        public DimensionBL(Idimensiondao idimensiondao, ITblConfigParamsDAO iTblConfigParamsDAO)
        {
            _iDimensiondao = idimensiondao;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }

        #region Selection
        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {
            return _iDimensiondao.SelectDeliPeriodForDropDown();
        }

        public List<int> GetModbusRefList()
        {
            return _iDimensiondao.GeModRefMaxData();
        }

        public List<DropDownTO> SelectDimMasterValues(Int32 masterId)
        {
            return _iDimensiondao.SelectDimMasterValues(masterId);
        }
        public List<DropDownTO> SelectAddOnFunDtls(Int32 transId)
        {
            return _iDimensiondao.SelectAddOnFunDtls(transId);
        }
       
        public List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId)
        {
            return _iDimensiondao.SelectDimMasterValuesByParentMasterValueId(parentMasterValueId);
        }
        public List<DropDownTO> SelectCDStructureForDropDown()
        {
            return _iDimensiondao.SelectCDStructureForDropDown();
        }

        public List<DropDownTO> SelectCountriesForDropDown()
        {
            return _iDimensiondao.SelectCountriesForDropDown();
        }

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {
            return _iDimensiondao.SelectStatesForDropDown(countryId);
        }
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {
            return _iDimensiondao.SelectDistrictForDropDown(stateId);
        }

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {
            return _iDimensiondao.SelectTalukaForDropDown(districtId);
        }

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {
            return _iDimensiondao.SelectOrgLicensesForDropDown();
        }

        public List<DropDownTO> SelectLocationForDropDown()
        {
            return _iDimensiondao.SelectLocationForDropDown();
        }

        public List<DropDownTO> SelectSalutationsForDropDown()
        {
            return _iDimensiondao.SelectSalutationsForDropDown();
        }

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {
            return _iDimensiondao.SelectRoleListWrtAreaAllocationForDropDown();

        }

        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {
            return _iDimensiondao.SelectAllSystemRoleListForDropDown();

        }

        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {
            return _iDimensiondao.SelectCnfDistrictForDropDown(cnfOrgId);

        }

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {
            return _iDimensiondao.SelectAllTransportModeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {
            return _iDimensiondao.SelectInvoiceTypeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {
            return _iDimensiondao.SelectInvoiceModeForDropDown();

        }
        public List<DropDownTO> SelectCurrencyForDropDown()
        {
            return _iDimensiondao.SelectCurrencyForDropDown();

        }

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            return _iDimensiondao.GetInvoiceStatusForDropDown();

        }


        public DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran)
        {
            List<DimFinYearTO> mstFinYearTOList = _iDimensiondao.SelectAllMstFinYearList(conn, tran);
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
            int result = _iDimensiondao.InsertMstFinYear(newMstFinYearTO, conn, tran);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }



        // Vaibhav [27-Sep-2017] added to select all reporting type list
        public List<DropDownTO> GetReportingType()
        {
            List<DropDownTO> reportingTypeList = _iDimensiondao.SelectReportingType();
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            List<DimVisitIssueReasonsTO> visitIssueReasonList = _iDimensiondao.SelectVisitIssueReasonsList();
            if (visitIssueReasonList != null)
                return visitIssueReasonList;
            else
                return null;
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList()
        {
            return _iDimensiondao.SelectBrandList();
        }


        /// <summary>
        /// [2018-01-02]Vijaymala:Added to get loading layer list  
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {
            return _iDimensiondao.SelectLoadingLayerList();
        }

        // Vijaymala [09-11-2017] added to get state Code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = _iDimensiondao.SelectStateCode(stateId);
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

        public String GetRoleIdsStrFromRoleTypeId(Int32 roleTypeId)
        {
            List<DropDownTO> roleList = SelectAllSystemUsersListFromRoleType(roleTypeId);

            String roleIds = String.Empty;
            if (roleList != null && roleList.Count > 0)
            {
                roleIds = String.Join(',', roleList.Select(s => s.Value).ToList());
            }

            return roleIds;
        }

        public List<DropDownTO> SelectAllSystemUsersListFromRoleType(Int32 RoleTypeId)
        {
            return _iDimensiondao.SelectAllSystemUsersListFromRoleType(RoleTypeId);
        }
        public List<DropDownTO> SelectVehicalType()
        {
            return _iDimensiondao.SelectVehicalType();
        }
        public List<DropDownTO> SelectAllSystemUsersFromRoleType(Int32 RoleTypeId)
        {
            return _iDimensiondao.SelectAllSystemUsersFromRoleType(RoleTypeId);
        }      
        #endregion

        #region Insertion

        public  int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return  _iDimensiondao.InsertTaluka(commonDimensionsTO, conn, tran);
        }

        public  int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return  _iDimensiondao.InsertDistrict(commonDimensionsTO, conn, tran);
        }
        #endregion

        #region Execute Command

        public  int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            return  _iDimensiondao.ExecuteGivenCommand(cmdStr, conn, tran);
        }


        #endregion

        //ReshmaP[24-01-22] For Internal Transfer
        public ResultMessage UpdateGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Boolean isAlreadyExist = false;
                int result = 0;
                isAlreadyExist = _iDimensiondao.CheckExistingData(DimGenericMasterTO);
                
                if (!isAlreadyExist)
                    result = _iDimensiondao.UpdateDimGenericMasterData(DimGenericMasterTO);
                else
                {
                    resultMessage.DefaultBehaviour("Name Already Present");
                    return resultMessage;
                }

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error In Method UpdateGenericMasterData");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Record Update Successfully");
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " UpdateGenericMasterData");
                return resultMessage;
            }
        }

        public List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            return _iDimensiondao  .GetGenericMasterData(IdDimension, SkipIsActiveFilter, ParentIdGenericMaster);
             
        }
        public ResultMessage PostGenericMasterData( DimGenericMasterTO DimGenericMasterTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Boolean isAlreadyExist = false;
                int result = 0;
                isAlreadyExist = _iDimensiondao.CheckAleradyAvailableData(DimGenericMasterTO);
                if (!isAlreadyExist)
                    result = _iDimensiondao.SaveDimGenericMasterData(DimGenericMasterTO);
                else
                {
                    resultMessage.DefaultBehaviour("Record Already Exists");
                    return resultMessage;
                }

                if (result !=1 ) 
                {
                    resultMessage.DefaultBehaviour("Error In Method Save New PostGenericMasterData");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Recored Save Successfully");
                resultMessage.data = DimGenericMasterTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostGenericMasterData");
                return resultMessage;
            }
            
        }

        public ResultMessage GetApprovalUserList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> approval1UserList = new List<DropDownTO>();
                List<DropDownTO> approval2UserList = new List<DropDownTO>();
                List<DropDownTO> approvalUserList = new List<DropDownTO>();
                TblConfigParamsTO Approval1ConfigParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("CP_INTERNAL_TRANFER_ALLOW_USER_LIST");
                if (Approval1ConfigParamTO != null)
                {
                    if (!String.IsNullOrEmpty(Approval1ConfigParamTO.ConfigParamVal))
                    {
                        approval1UserList =_iDimensiondao   .GetApprovalUserList(Approval1ConfigParamTO.ConfigParamVal);
                        if (approval1UserList != null && approval1UserList.Count > 0)
                        {
                            approval1UserList.ForEach(element =>
                            {
                                DropDownTO dropDownTO = element.DeepCopy();
                                dropDownTO.Type = 1;
                                approvalUserList.Add(dropDownTO);
                            });
                        }
                    }
                }
                if (approvalUserList != null && approvalUserList.Count > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                }
                resultMessage.data = approvalUserList;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetApprovalUserList");
                return resultMessage;
            }
        }
    }
}
