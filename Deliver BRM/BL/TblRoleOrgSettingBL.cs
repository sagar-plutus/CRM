using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.BL
{
    public class TblRoleOrgSettingBL
    {
        internal static ResultMessage SaveRolesAndOrg(List<RoleOrgTO> roleOrgTOList)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 updateOrCreatedUser = 0;
            try
            {
                conn.Open();

                if (roleOrgTOList == null || roleOrgTOList.Count == 0)
                {
                    resultMessage.Text = "Error,roleOrgTOList Found NULL : Method SaveRolesAndOrg";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #region Save List 

                for (int i = 0; i < roleOrgTOList.Count; i++)
                {
                    int res = TblRoleOrgSettingDAO.CheckIfExists(roleOrgTOList[i]);

                    if (res != 0)
                    {
                        result = TblRoleOrgSettingDAO.UpdateRolesAndOrg(roleOrgTOList[i], conn);
                    }
                    else
                    {
                        result = TblRoleOrgSettingDAO.SaveRolesAndOrg(roleOrgTOList[i], conn);
                    }
                    if (result != 1)
                    {
                        resultMessage.Text = "Error While InsertTblLoadingQuotaDeclaration : ConfirmStockSummary";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                #endregion

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.DisplayMessage = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : ConfirmStockSummary";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        internal static List<DropDownTO> SelectAllSystemRoleOrgListForDropDown(int visitTypeId, int personTypeId)
        {
            return TblRoleOrgSettingDAO.SelectAllSystemRoleOrgListForDropDown(visitTypeId, personTypeId);
        }

        internal static List<DropDownTO> SelectSavedRoles(int visitTypeId, int personTypeId)
        {
            return TblRoleOrgSettingDAO.SelectSavedRoles(visitTypeId, personTypeId);

        }

        internal static List<DropDownTO> SelectSavedOrg(int visitTypeId, int personTypeId)
        {
            return TblRoleOrgSettingDAO.SelectSavedOrg(visitTypeId, personTypeId);

        }
    }
}
