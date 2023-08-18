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
    public class TblOrgStructureBL
    {

        #region Selection

        public static List<TblOrgStructureTO> SelectAllTblOrgStructureList()
        {
            List<TblOrgStructureTO> OrgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();
            return OrgStructureTOList;
        }

        public static TblOrgStructureTO SelectBOMOrgStructure()
        {
            TblOrgStructureTO OrgStructureTO = TblOrgStructureDAO.SelectBOMOrgStructure();
            return OrgStructureTO;
        }

        // Vaibhav [26-Sep-2017] get all Organization structure list
        public static List<TblOrgStructureTO> SelectAllOrgStructureList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                // Added orgnization information at zeroth position in list for display
                List<TblOrgStructureTO> orgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();
                if (orgStructureTOList != null)
                {
                    TblOrganizationTO OrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTO((int)Constants.DefaultCompanyId);

                    #region Commented Code
                    // Commented to remove organization name from organization structure.
                    //if (OrganizationTO != null)
                    //{
                    //    TblOrgStructureTO tblOrgStructureTO = new TblOrgStructureTO();
                    //    tblOrgStructureTO.IdOrgStructure = 0;
                    //    tblOrgStructureTO.ParentOrgStructureId = 0;
                    //    tblOrgStructureTO.DeptId = 0;
                    //    tblOrgStructureTO.DesignationId = 0;
                    //    tblOrgStructureTO.IsActive = 1;
                    //    tblOrgStructureTO.OrgStructureDesc = OrganizationTO.FirmName;

                    //    orgStructureTOList.Insert(0, tblOrgStructureTO);
                    //}
                    #endregion

                    return orgStructureTOList;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllOrgStructureList");
                return null;
            }
        }

        public static List<TblUserReportingDetailsTO> GetOrgStructureUserDetailsForBom(Int16 orgStructureId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblUserReportingDetailsTO> userList = TblOrgStructureDAO.SelectOrgStructureUserDetailsForBom(orgStructureId);
                if (userList != null)
                    return userList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetOrgStructureUserDetails");
                return null;
            }
        }


        public static List<TblUserReportingDetailsTO> GetOrgStructureUserDetails(Int16 orgStructureId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblUserReportingDetailsTO> userList = TblOrgStructureDAO.SelectOrgStructureUserDetails(orgStructureId);
                if (userList != null)
                    return userList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetOrgStructureUserDetails");
                return null;
            }
        }

        public static List<TblUserReportingDetailsTO> GetOrgStructureUserDetails(Int16 orgStructureId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblUserReportingDetailsTO> userList = TblOrgStructureDAO.SelectOrgStructureUserDetails(orgStructureId, conn, tran);
                if (userList != null)
                    return userList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetOrgStructureUserDetails");
                return null;
            }
        }
        public static ResultMessage UpdateUserReportingDetailsBom(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (userReportingId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTODeactivation = SelectUserReportingDetailsTOBom(userReportingId, conn, tran);
                    if (tblUserReportingDetailsTODeactivation != null)
                    {
                        tblUserReportingDetailsTODeactivation.IsActive = 0;
                        result = UpdateUserReportingDetail(tblUserReportingDetailsTODeactivation, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                        return resultMessage;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }




        public static List<TblOrgStructureTO> SelectAllOrgStructureList(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                // Added orgnization information at zeroth position in list for display
                List<TblOrgStructureTO> orgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy(conn, tran);
                if (orgStructureTOList != null)
                {
                    TblOrganizationTO OrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTO((int)Constants.DefaultCompanyId, conn, tran);
                    return orgStructureTOList;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllOrgStructureList");
                return null;
            }
        }
        public static List<DimMstDeptTO> SelectAllDimMstDeptList(SqlConnection conn, SqlTransaction tran)
        {
            return DimMstDeptDAO.SelectAllDimMstDept(conn, tran);
        }


        public static List<TblOrgStructureHierarchyTO> SelectTblOrgStructureHierarchyOnReportingType(int reportingTypeId)
        {
            List<TblOrgStructureHierarchyTO> OrgStructureHierarchyTOList = TblOrgStructureDAO.SelectTblOrgStructureHierarchyOnReportingType(reportingTypeId);
            return OrgStructureHierarchyTOList;
        }


        public static TblOrgStructureHierarchyTO SelectTblOrgStructureHierarchyTO(int orgStrctureId, int parentOrgStrctureId, int reportingTypeId, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.SelectAllTblOrgStructureHierarchy(orgStrctureId, parentOrgStrctureId, reportingTypeId, conn, tran);
        }

        // Vaibhav [11-Oct-2017] added to get all reporting to employee list
        public static List<DropDownTO> SelectReportingToUserList(Int32 orgStructureId, Int32 type)
        {

            String id = String.Empty;
            //id = orgStructureId.ToString() + ",";
            //Write Recursion
            GetParentPositionIds(orgStructureId, ref id, type);


            //List<TblOrgStructureHierarchyTO> hierarchyList = TblOrgStructureDAO.SelectTblOrgStructureHierarchyOnOrgStructutreId(orgStructureId);
            //String ids = String.Empty;
            //if (hierarchyList != null && hierarchyList.Count > 0)
            //{
            //    for (int i = 0; i < hierarchyList.Count; i++)
            //    {
            //        ids += hierarchyList[i].ParentOrgStructId + ",";
            //    }
            //}

            id = id.TrimEnd(',');

            List<DropDownTO> reportingToUserList = TblOrgStructureDAO.SelectReportingToUserList(id, type);
            if (reportingToUserList != null)
                return reportingToUserList;
            else
                return new List<DropDownTO>();
        }


        //Sudhir[09-July-2018] Added for Find Parent Positions.
        public static void GetParentPositionIds(int orgStructureId, ref string id, int type)
        {
            if (orgStructureId > 0)
            {
                List<TblOrgStructureHierarchyTO> hierarchyList = TblOrgStructureDAO.SelectTblOrgStructureHierarchyOnOrgStructutreId(orgStructureId)
                    .Where(e => e.ReportingTypeId == type).ToList();
                //String ids = String.Empty;
                if (hierarchyList != null && hierarchyList.Count > 0)
                {
                    for (int i = 0; i < hierarchyList.Count; i++)
                    {
                        //ids += hierarchyList[i].ParentOrgStructId + ",";
                        id += hierarchyList[i].ParentOrgStructId + ",";
                        GetParentPositionIds(hierarchyList[i].ParentOrgStructId, ref id, type);
                    }
                }
            }
        }

        public static DropDownTO SelectParentUserOnUserId(int userId, int reportingTypeId)
        {

            List<TblUserReportingDetailsTO> tblUserReportingDetialsList = TblOrgStructureDAO.SelectUserReportingListOnUserId(userId);


            //Hrushikesh Added to return parent on reporting type 

            if (reportingTypeId > 0)
            {
                tblUserReportingDetialsList = tblUserReportingDetialsList.Where(e => e.ReportingTypeId == reportingTypeId).ToList();
            }
            if (tblUserReportingDetialsList != null && tblUserReportingDetialsList.Count > 0)
            {

                if (tblUserReportingDetialsList[0].ReportingTo == 0)
                {
                    return TblUserDAO.SelectTblUser(userId);
                }
                else
                {
                    return TblUserDAO.SelectTblUser(tblUserReportingDetialsList[0].ReportingTo);
                }
            }
            else
                return null;
        }

        // Vaibhav Get OrgStuctureList For Hierarchy
        public static List<TblOrgStructureTO> SelectOrgStuctureListForHierarchy(int reportingTypeId)
        {
            List<TblOrgStructureTO> OrgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();
            List<TblOrgStructureTO> employeeHierarchyList = new List<TblOrgStructureTO>();

            if (OrgStructureTOList != null)
            {
                // get default organization name - Bhagyalaxmi Rolling Mills
                TblOrganizationTO OrganizationTO = BL.TblOrganizationBL.SelectTblOrganizationTO(Constants.DefaultCompanyId);

                if (OrganizationTO == null)
                {
                    return null;
                }

                TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
                orgStructureTO.IdOrgStructure = 1;
                orgStructureTO.ParentOrgStructureId = 0;
                orgStructureTO.OrgStructureDesc = OrganizationTO.FirmName;

                employeeHierarchyList.Add(orgStructureTO);

                for (int i = 0; i < OrgStructureTOList.Count; i++)
                {
                    List<TblUserReportingDetailsTO> userList = TblOrgStructureDAO.SelectOrgStructureUserDetails(OrgStructureTOList[i].IdOrgStructure);

                    // filter user list by reporting type

                    //List<TblUserReportingDetailsTO> userListByReportingType = userList.FindAll(ele => ele.ReportingTypeId == reportingTypeId);  --OLD
                    List<TblUserReportingDetailsTO> userListByReportingType = userList.FindAll(ele => 1 == reportingTypeId);

                    for (int j = 0; j < userListByReportingType.Count; j++)
                    {
                        TblOrgStructureTO finalUserReportingDtl1 = new TblOrgStructureTO();
                        if (userListByReportingType[j].ReportingTo <= 0)
                        {

                            int lastIndex = employeeHierarchyList.Count;

                            finalUserReportingDtl1.IdOrgStructure = lastIndex + 1;
                            finalUserReportingDtl1.ParentOrgStructureId = 1;
                            finalUserReportingDtl1.OrgStructureDesc = OrgStructureTOList[i].OrgStructureDesc;
                            finalUserReportingDtl1.EmployeeName = userListByReportingType[j].UserName;
                            finalUserReportingDtl1.EmployeeId = userListByReportingType[j].UserId;
                            finalUserReportingDtl1.ActualOrgStructureId = OrgStructureTOList[i].IdOrgStructure;
                            employeeHierarchyList.Add(finalUserReportingDtl1);
                        }
                        else
                        {
                            int lastIndex = employeeHierarchyList.Count;
                            finalUserReportingDtl1.IdOrgStructure = lastIndex + 1;

                            // filter by reporting employee id to get hierarchy parent id
                            List<TblOrgStructureTO> orgStructureTOList = OrgStructureTOList.FindAll(ele => ele.IdOrgStructure == userListByReportingType[j].OrgStructureId);
                            finalUserReportingDtl1.ActualOrgStructureId = orgStructureTOList[0].IdOrgStructure;

                            List<TblOrgStructureTO> employeeList = employeeHierarchyList.FindAll(ele => ele.EmployeeId == userList[j].ReportingTo && ele.ActualOrgStructureId == orgStructureTOList[0].ParentOrgStructureId);

                            if (employeeList != null && employeeList.Count > 0)
                            {
                                finalUserReportingDtl1.ParentOrgStructureId = employeeList[0].IdOrgStructure;
                                finalUserReportingDtl1.OrgStructureDesc = OrgStructureTOList[i].OrgStructureDesc;
                                finalUserReportingDtl1.EmployeeName = userListByReportingType[j].UserName;
                                finalUserReportingDtl1.EmployeeId = userListByReportingType[j].UserId;
                                employeeHierarchyList.Add(finalUserReportingDtl1);
                            }
                        }
                    }
                }
            }
            return employeeHierarchyList;
        }

        public static List<TblOrgStructureHierarchyTO> SelectAllTblOrgStructureHierarchy()
        {
            List<TblOrgStructureHierarchyTO> list = TblOrgStructureDAO.SelectAllTblOrgStructureHierarchy();
            if (list != null && list.Count > 0)
                return list;
            else
                return null;
        }

        //Sudhir[01-JAN-2018] Added for Display organization Structure New Req.
        public static List<TblOrgStructureTO> GetOrgStructureListForDisplayTree()
        {
            List<TblOrgStructureTO> finalOrgStructureTOListUserDisplay = new List<TblOrgStructureTO>();
            List<DimMstDeptTO> mstDeptToList = DimMstDeptBL.SelectAllDimMstDeptList();
            List<TblOrgStructureTO> orgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();
            List<TblOrgStructureTO> orgStructureTOListForDisplay = new List<TblOrgStructureTO>();
            List<TblUserReportingDetailsTO> alluserReportingDetailsTOList = TblOrgStructureDAO.SelectOrgStructureUserDetails(0);
            List<TblOrgStructureTO> orgStructureTOListUserDisplay = new List<TblOrgStructureTO>();
            List<TblOrgStructureHierarchyTO> hierarchyList = TblOrgStructureBL.SelectAllTblOrgStructureHierarchy();
            //
            //List<TblRoleTO> allRoleList = TblRoleBL.SelectAllTblRoleList();

            orgStructureTOList[0].PositionName = orgStructureTOList[0].OrgStructureDesc;
            orgStructureTOList[0].IsAddDept = 1;

            for (int ele = 0; ele < orgStructureTOList.Count; ele++)
            {
                orgStructureTOList[ele].ParentOrgDisplayId = "##" + orgStructureTOList[ele].ParentOrgStructureId;
                //orgStructureTOList[ele].TempOrgStructId = "##" + orgStructureTOList[ele].IdOrgStructure;
            }
            orgStructureTOList[0].ParentOrgDisplayId = "*" + 0;
            orgStructureTOList[0].TempOrgStructId = "*" + 1;
            //orgStructureTOList[0].IsPosition = 1;
            orgStructureTOListUserDisplay.Add(orgStructureTOList[0]);

            if (mstDeptToList != null)
            {
                mstDeptToList = mstDeptToList.Where(x => x.ParentDeptId != 0).ToList();
                for (int ele = 0; ele < mstDeptToList.Count; ele++)
                {
                    DimMstDeptTO dimMstDept = mstDeptToList[ele];
                    TblOrgStructureTO tblOrgStructureTOForDepartment = new TblOrgStructureTO();
                    //orgStructureTO.IdOrgStructure = list[i].IdDept;
                    tblOrgStructureTOForDepartment.DeptId = dimMstDept.IdDept;
                    tblOrgStructureTOForDepartment.TempOrgStructId = "*" + dimMstDept.IdDept;
                    tblOrgStructureTOForDepartment.OrgStructureDesc = dimMstDept.DeptDisplayName;
                    tblOrgStructureTOForDepartment.PositionName = dimMstDept.DeptDisplayName;
                    //tblOrgStructureTOForDepartment.IdOrgStructure=
                    tblOrgStructureTOForDepartment.ParentOrgDisplayId = "*" + dimMstDept.ParentDeptId;
                    orgStructureTOListUserDisplay.Add(tblOrgStructureTOForDepartment);
                    tblOrgStructureTOForDepartment.IsDept = 1;
                    if (dimMstDept.DeptTypeId != Convert.ToInt16(Constants.DepartmentTypeE.SUB_DEPARTMENT))
                    {
                        tblOrgStructureTOForDepartment.IsAddDept = 1;
                    }
                    //if(ele!=0)
                    //{
                    TblOrgStructureTO tblOrgStructureTOForPosition = new TblOrgStructureTO();
                    tblOrgStructureTOForPosition.ParentOrgDisplayId = tblOrgStructureTOForDepartment.TempOrgStructId;
                    tblOrgStructureTOForPosition.TempOrgStructId = "#" + dimMstDept.IdDept;
                    tblOrgStructureTOForPosition.OrgStructureDesc = "Positions";
                    tblOrgStructureTOForPosition.PositionName = "Positions";
                    tblOrgStructureTOForPosition.IsEmptyPosition = 1;
                    orgStructureTOListUserDisplay.Add(tblOrgStructureTOForPosition);
                    List<TblOrgStructureTO> orgStructureTOlistForChild = orgStructureTOList.Where(x => x.DeptId == dimMstDept.IdDept).ToList();
                    if (orgStructureTOlistForChild != null)
                    {
                        for (int i = 0; i < orgStructureTOlistForChild.Count; i++)
                        {
                            TblOrgStructureTO tblOrgStructureForChild = orgStructureTOlistForChild[i];
                            tblOrgStructureForChild.ParentOrgDisplayId = tblOrgStructureTOForPosition.TempOrgStructId;
                            tblOrgStructureForChild.IsPosition = 1;
                            //  tblOrgStructureForChild.RoleTypeId=

                            orgStructureTOListUserDisplay.Add(tblOrgStructureForChild);
                        }
                        //}
                    }

                }
            }
            orgStructureTOListUserDisplay.ForEach(
         orgStructureTOLi =>
         {
             mstDeptToList.ForEach(deptLi => {
                 if (orgStructureTOLi.DeptId == deptLi.IdDept)
                 {
                     orgStructureTOLi.DeptTypeId = deptLi.DeptTypeId;
                 }
             });
             finalOrgStructureTOListUserDisplay.Add(orgStructureTOLi);
         });

            #region OldCode.
            ////foreach (DimMstDeptTO mstDeptTo in mstDeptToList)
            ////{
            ////    Int32 IdDept = mstDeptTo.IdDept;
            ////    List<TblOrgStructureTO> orgStructureTOListTemp = orgStructureTOList.FindAll(ele => ele.DeptId == IdDept);
            ////    if (orgStructureTOListTemp.Count > 0)
            ////    {
            ////        for (int ele = 0; ele < orgStructureTOListTemp.Count; ele++)
            ////        {
            ////             orgStructureTOListTemp.Where(c => c.DeptId == IdDept).ToList().ForEach(cc => cc.IdOrgStructure = orgStructureTOListTemp[0].IdOrgStructure);
            ////            List<TblOrgStructureTO> list = orgStructureTOList.Where(x => x.ParentOrgStructureId == orgStructureTOListTemp[ele].IdOrgStructure).ToList();
            ////            orgStructureTOListForDisplay.Add(orgStructureTOListTemp[ele]);
            ////        }
            ////    }
            ////    else
            ////    {
            ////        TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
            ////        orgStructureTO.IdOrgStructure = mstDeptTo.IdDept;
            ////        orgStructureTO.DeptId = mstDeptTo.IdDept;
            ////        orgStructureTO.ParentOrgStructureId = mstDeptTo.ParentDeptId;
            ////        orgStructureTO.OrgStructureDesc = mstDeptTo.DeptDisplayName;
            ////        orgStructureTOListForDisplay.Add(orgStructureTO);


            ////    }
            ////}

            //if (orgStructureTOList != null) //if True Then First Show All Department Structure.
            //{
            //    for (int ele = 0; ele < orgStructureTOList.Count; ele++)
            //    {
            //        orgStructureTOList[ele].ParentOrgDisplayId = "#" + orgStructureTOList[ele].ParentOrgStructureId;
            //        orgStructureTOList[ele].TempOrgStructId = "#" + orgStructureTOList[ele].IdOrgStructure;
            //    }

            //    for (int i = 0; i < orgStructureTOList.Count; i++)
            //    {
            //        orgStructureTOList[i].IsDept = 0;

            //        //if (childUserReportingList != null)
            //        //{
            //        //    //for (int lst = 0; lst < childUserReportingList.Count; lst++)
            //        //    //{
            //        //    //    TblUserReportingDetailsTO userReportingDetailsTO = childUserReportingList[lst];
            //        //    //    orgStructureTOList[i].EmployeeName = childUserReportingList[lst].UserName;
            //        //    //    orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //        //    //}

            //        //    //if (childUserReportingList.Count == 1)
            //        //    //{
            //        //    //    orgStructureTOList[i].EmployeeName = childUserReportingList[0].UserName;
            //        //    //}
            //        //    //else
            //        //    //{

            //        //    //}
            //        //}
            //        //else
            //        //{
            //        // orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //        //}
            //        orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //        List<DimMstDeptTO> tempList = mstDeptToList.Where(x => x.ParentDeptId == orgStructureTOList[i].DeptId).ToList();

            //        if (tempList != null && tempList.Count > 0)
            //        {

            //            for (int k = 0; k < tempList.Count; k++)
            //            {


            //                List<TblOrgStructureTO> list = orgStructureTOList.Where(ele => ele.ParentOrgStructureId == orgStructureTOList[i].IdOrgStructure && tempList[k].IdDept == ele.DeptId).ToList();
            //                if (list != null && list.Count == 0)
            //                {
            //                    //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //                    GetOrg(orgStructureTOList[i].TempOrgStructId, orgStructureTOList[i].DeptId, orgStructureTOListForDisplay, mstDeptToList, tempList[k].IdDept);
            //                }
            //                else
            //                {
            //                    //for (int j = 0; j < orgStructureTOList.Count; j++)
            //                    //{
            //                    //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //                    //}

            //                }
            //            }
            //        }
            //        else
            //        {
            //            //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
            //        }

            //    }

            //    //List<DimMstDeptTO> mstDeptToList = DimMstDeptBL.SelectAllDimMstDeptList();
            //    //for (int i = 0; i < mstDeptToList.Count; i++)
            //    //{
            //    //    TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
            //    //    orgStructureTO.IdOrgStructure = mstDeptToList[i].IdDept;
            //    //    orgStructureTO.DeptId = mstDeptToList[i].IdDept;
            //    //    orgStructureTO.ParentOrgStructureId = mstDeptToList[i].ParentDeptId;
            //    //    orgStructureTO.OrgStructureDesc = mstDeptToList[i].DeptDisplayName;
            //    //    OrgStructureTOList.Add(orgStructureTO);
            //    //}
            //}
            //UserwiseOrgChartTree(orgStructureTOListForDisplay, alluserReportingDetailsTOList, "#0", orgStructureTOListUserDisplay, 0, "#0", 0);
            #endregion

            return finalOrgStructureTOListUserDisplay;
        }

        public static void UserwiseOrgChartTree(List<TblOrgStructureTO> OrgStructureTOList, List<TblUserReportingDetailsTO> alluserReportingDetailsTOList, String ParentId, List<TblOrgStructureTO> orgStructureTOListUserDisplay, Int32 userId, String newParent, Int32 uniquiNo)
        {
            List<TblOrgStructureTO> tempList = OrgStructureTOList.Where(x => x.ParentOrgDisplayId == ParentId).ToList();

            if (tempList != null && tempList.Count > 0)
            {
                for (int ele = 0; ele < tempList.Count; ele++)
                {
                    TblOrgStructureTO tblOrgStructureTO = tempList[ele];
                    List<TblUserReportingDetailsTO> userList = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == tblOrgStructureTO.IdOrgStructure).ToList();

                    Boolean isAdded = false;

                    if (userList != null && userList.Count > 0)
                    {
                        for (int i = 0; i < userList.Count; i++)
                        {
                            if (userList[i].ReportingTo == userId)
                            {
                                isAdded = true;

                                TblOrgStructureTO tblOrgStructureTOUser = tblOrgStructureTO.Clone();

                                tblOrgStructureTOUser.EmployeeName = userList[i].UserName;

                                //tblOrgStructureTOUser.TempOrgStructId += "_" + i;
                                tblOrgStructureTOUser.TempOrgStructId += "_" + uniquiNo + "_" + userList[i].IdUserReportingDetails;
                                uniquiNo++;

                                tblOrgStructureTOUser.ParentOrgDisplayId = newParent;

                                orgStructureTOListUserDisplay.Add(tblOrgStructureTOUser);

                                UserwiseOrgChartTree(OrgStructureTOList, alluserReportingDetailsTOList, tblOrgStructureTO.TempOrgStructId, orgStructureTOListUserDisplay, userList[i].UserId, tblOrgStructureTOUser.TempOrgStructId, uniquiNo);
                            }
                        }
                    }
                    if (!isAdded)
                    {

                        TblOrgStructureTO tblOrgStructureTOUser = tblOrgStructureTO.Clone();

                        //tblOrgStructureTOUser.TempOrgStructId += "_E" + userId;
                        tblOrgStructureTOUser.TempOrgStructId += "_" + uniquiNo + "_U" + userId; ;
                        uniquiNo++;

                        tblOrgStructureTOUser.ParentOrgDisplayId = newParent;

                        orgStructureTOListUserDisplay.Add(tblOrgStructureTOUser);
                        UserwiseOrgChartTree(OrgStructureTOList, alluserReportingDetailsTOList, tblOrgStructureTO.TempOrgStructId, orgStructureTOListUserDisplay, 0, tblOrgStructureTOUser.TempOrgStructId, uniquiNo);
                    }
                }
            }
            #region Commented Code
            //if (alluserReportingDetailsTOList != null)
            //{
            //    for (int i = 0; i < OrgStructureTOList.Count; i++)
            //    {
            //        TblOrgStructureTO orgStructureTO = OrgStructureTOList[i];
            //        List<TblUserReportingDetailsTO> list = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == orgStructureTO.IdOrgStructure).ToList();
            //        if(list !=null && list.Count > 0)
            //        {

            //        }
            //    }
            //    //List<TblUserReportingDetailsTO> childUserReportingList = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == orgStructureTOList[i].IdOrgStructure).ToList();

            //}
            #endregion
        }


        //Sudhir[03-JAN-2018] Added Recursive Method For Add Department in OrgnizationStructure List.
        public static void GetOrgTree(String ParentId, Int32 DeptId, List<TblOrgStructureTO> OrgStructureTOList, List<DimMstDeptTO> MstDeptTOList, Int32 deptId)
        {
            List<DimMstDeptTO> list = new List<DimMstDeptTO>();
            if (deptId > 0)
            {
                list = MstDeptTOList.Where(x => x.ParentDeptId == DeptId && x.IdDept == deptId).ToList();
            }
            else
            {
                list = MstDeptTOList.Where(x => x.ParentDeptId == DeptId).ToList();
            }
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
                    //orgStructureTO.IdOrgStructure = list[i].IdDept;
                    orgStructureTO.DeptId = list[i].IdDept;
                    orgStructureTO.TempOrgStructId = "*" + list[i].IdDept + ParentId;
                    if (ParentId.Contains('*'))
                    {
                        orgStructureTO.ParentOrgStructureId = 0;
                    }
                    else
                    {
                        orgStructureTO.ParentOrgStructureId = Convert.ToInt32(ParentId.Trim('#'));
                    }
                    orgStructureTO.OrgStructureDesc = list[i].DeptDisplayName;
                    orgStructureTO.PositionName = list[i].DeptDisplayName;
                    orgStructureTO.ParentOrgDisplayId = ParentId;
                    OrgStructureTOList.Add(orgStructureTO);
                    orgStructureTO.IsDept = 1;
                    GetOrgTree(orgStructureTO.TempOrgStructId, list[i].IdDept, OrgStructureTOList, MstDeptTOList, 0);
                }
            }
        }

        public static TblOrgStructureTO SelectTblOrgStructure(int idOrgStructure)
        {
            TblOrgStructureTO tblOrgStructureTO = TblOrgStructureDAO.SelectTblOrgStructure(idOrgStructure);
            return tblOrgStructureTO;
        }

        public static List<DimLevelsTO> GetAllLevelsToList()
        {
            List<DimLevelsTO> AllLevelsTOList = TblOrgStructureDAO.SelectAllDimLevels();
            return AllLevelsTOList;
        }

        public static String GetDeptIds(List<DimMstDeptTO> allDeptList, int deptId, string ids)
        {
            DimMstDeptTO dimMstDeptTO = new DimMstDeptTO();
            if (deptId > 0 && allDeptList != null)
            {
                dimMstDeptTO = allDeptList.Where(x => x.IdDept == deptId).FirstOrDefault();
                if (dimMstDeptTO != null && dimMstDeptTO.ParentDeptId != 0)
                {
                    //for (int i = 0; i < dimMstDeptTOList.Count; i++)
                    //{
                    //    DimMstDeptTO dimMstDeptTO = dimMstDeptTOList[i];
                    //    ids += dimMstDeptTO.ParentDeptId + ",";
                    //    GetDeptIds(allDeptList, dimMstDeptTO.ParentDeptId, ids);
                    //}
                    ids += dimMstDeptTO.ParentDeptId + ",";
                    GetDeptIds(allDeptList, dimMstDeptTO.ParentDeptId, ids);
                }
                else
                    return ids;
            }
            return ids;
        }

        public static List<TblOrgStructureTO> GetAllchildPositionList(int idOrgstructure, int reportingTypeId)
        {
            if (reportingTypeId != 0)
            {
                String strIds = String.Empty;
                String finalIds = String.Empty;
                TblOrgStructureTO tblOrgStructureTO = SelectTblOrgStructure(idOrgstructure);
                List<DimMstDeptTO> mstDeptlist = DimMstDeptBL.SelectAllDimMstDeptList();
                DimMstDeptTO dimMstDeptTO = mstDeptlist.Where(x => x.IdDept == tblOrgStructureTO.DeptId).FirstOrDefault();
                strIds = tblOrgStructureTO.DeptId.ToString() + ",";
                if (dimMstDeptTO != null)
                {
                    strIds += dimMstDeptTO.ParentDeptId.ToString() + ",";
                    strIds = GetDeptIds(mstDeptlist, dimMstDeptTO.ParentDeptId, strIds);
                }

                finalIds = strIds.Trim(',');

                List<TblOrgStructureTO> childPositionsList = TblOrgStructureDAO.SelectChildPositionList(finalIds, idOrgstructure);
                List<TblOrgStructureHierarchyTO> orgHierarchyList = TblOrgStructureBL.SelectTblOrgStructureHierarchyOnReportingType(reportingTypeId);
                List<TblOrgStructureTO> newListForChild = new List<TblOrgStructureTO>();
                TblOrgStructureTO structureTO = new TblOrgStructureTO();
                if (orgHierarchyList != null)
                {
                    for (int i = 0; i < orgHierarchyList.Count; i++)
                    {
                        structureTO = childPositionsList.Where(x => x.IdOrgStructure == orgHierarchyList[i].OrgStructureId).FirstOrDefault();
                        if (structureTO != null)
                        {
                            newListForChild.Add(structureTO);
                        }
                    }
                }
                if (dimMstDeptTO.ParentDeptId != 1)
                {
                    childPositionsList = newListForChild;
                }
                childPositionsList = childPositionsList.OrderBy(x => x.IdOrgStructure).ToList();
                if (childPositionsList.Any(x => x.IdOrgStructure == 1))
                {
                    childPositionsList[0].PositionName = childPositionsList[0].OrgStructureDesc;
                }
                if (childPositionsList != null)
                {
                    return childPositionsList;
                }
                else
                    return null;
            }
            else
                return null;

        }

        public static List<TblOrgStructureTO> GetNotLinkedPositionsList()
        {
            List<TblOrgStructureTO> NotLinkedPositionsList = TblOrgStructureDAO.SelectAllNotLinkedPositionsList();
            if (NotLinkedPositionsList != null)
            {
                //NotLinkedPositionsList[0].PositionName = NotLinkedPositionsList[0].OrgStructureDesc;
                //Hrushikesh [26/11/2018] only active positions for this call
                NotLinkedPositionsList = NotLinkedPositionsList.Where(x => x.IdOrgStructure > 1 && x.IsActive == 1).ToList();
                return NotLinkedPositionsList;
            }
            else
                return null;
        }

        //Sudhir[04-July-2018] Added for Select All Parent Position Link Details.
        public static List<TblOrgStructureTO> SelectPositionLinkDetails(int idOrgStructure)
        {
            List<TblOrgStructureTO> OrgStructureTOList = TblOrgStructureDAO.SelectPositionLinkDetails(idOrgStructure);
            List<TblOrgStructureTO> OutOrgStructureTOList = new List<TblOrgStructureTO>();
            if (OrgStructureTOList != null && OrgStructureTOList.Count > 0)
            {
                for (int i = 0; i < OrgStructureTOList.Count; i++)
                {
                    if (OrgStructureTOList[i].ReportingTypeId == (int)Constants.ReportingTypeE.ADMINISTRATIVE)
                    {
                        TblOrgStructureTO structureTO = TblOrgStructureDAO.SelectTblOrgStructure(OrgStructureTOList[i].ParentOrgStructureId);
                        if (structureTO != null)
                        {
                            structureTO.ReportingName = "Administrative";
                            OutOrgStructureTOList.Add(structureTO);
                        }
                    }
                    else if (OrgStructureTOList[i].ReportingTypeId == (int)Constants.ReportingTypeE.TECHNICAL)
                    {
                        TblOrgStructureTO structureTO = TblOrgStructureDAO.SelectTblOrgStructure(OrgStructureTOList[i].ParentOrgStructureId);
                        if (structureTO != null)
                        {
                            structureTO.ReportingName = "Technical";
                            OutOrgStructureTOList.Add(structureTO);
                        }
                    }
                }
            }
            return OutOrgStructureTOList;
        }

        public static List<TblOrgStructureTO> SelectChildPositions(int idOrgStructure)
        {
            List<TblOrgStructureTO> OrgStructureTOList = TblOrgStructureDAO.SelectChildPositionsDetails(idOrgStructure);

            return OrgStructureTOList;
        }


        public static ResultMessage PostDelinkPosition(int SelfOrgId, int ParentPositionId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;

            try
            {
                TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO = DAL.TblOrgStructureDAO.SelectTblOrgStructureHierarchyForDelink(SelfOrgId, ParentPositionId);

                conn.Open();
                tran = conn.BeginTransaction();


                if (tblOrgStructureHierarchyTO != null)
                {
                    tblOrgStructureHierarchyTO.IsActive = 0;
                    tblOrgStructureHierarchyTO.UpdatedOn = StaticStuff.Constants.ServerDateTime;
                    tblOrgStructureHierarchyTO.UpdatedBy = tblOrgStructureHierarchyTO.CreatedBy;
                    result = DAL.TblOrgStructureDAO.UpdateTblOrgStructureHierarchy(tblOrgStructureHierarchyTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region User ReportingDetails
        public static List<TblUserReportingDetailsTO> SelectAllUserReportingDetails()
        {
            return TblOrgStructureDAO.SelectAllUserReportingDetails();
        }
        public static List<TblUserReportingDetailsTO> SelectAllUserReportingDetails(SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.SelectAllUserReportingDetails(conn, tran);
        }

        public static TblUserReportingDetailsTO SelectUserReportingDetailsTO(int IdUserReportingDetails)
        {
            return TblOrgStructureDAO.SelectUserReportingDetailsTO(IdUserReportingDetails);
        }
        public static TblUserReportingDetailsTO SelectUserReportingDetailsTO(int IdUserReportingDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.SelectUserReportingDetailsTO(IdUserReportingDetails, conn, tran);
        }
        #endregion

        //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        public static object ChildUserListOnUserId(Int32 userId, int? isObjectType, int reportingType)
        {
            try
            {
                List<TblUserReportingDetailsTO> allUserReportingDetailsList = SelectAllUserReportingDetails();
                List<TblUserReportingDetailsTO> usersonUserIdList = allUserReportingDetailsList.Where(ele => ele.UserId == userId).ToList();
                string tempids = String.Empty;
                string idsUsers = String.Empty;
                if (usersonUserIdList != null && usersonUserIdList.Count > 0)
                {
                    foreach (TblUserReportingDetailsTO userReportingTo in usersonUserIdList)
                    {
                        GetUserIdsOnParentId(allUserReportingDetailsList, userReportingTo.UserId, ref tempids, reportingType);
                    }
                }
                if (tempids != String.Empty && tempids != "")
                {
                    tempids = tempids.TrimEnd(',');
                    List<int> userIdList = tempids.Split(',').Select(int.Parse).ToList();
                    userIdList.RemoveAt(userIdList.IndexOf(userId));

                    //if (tempids.Contains(userId + ","))
                    //{
                    //    tempids = tempids.Replace(userId + ",", "");
                    //}
                    idsUsers = string.Join<int>(",", userIdList);  //tempids.TrimEnd(',');

                    if (idsUsers != string.Empty)
                    {
                        List<DropDownTO> dropDownList = new List<DropDownTO>();
                        dropDownList = TblUserBL.SelectUsersOnUserIds(idsUsers);
                        if (isObjectType == 1) //Added condition for return List of Id's Only else return DropDownTo List.
                        {
                            List<int> list = dropDownList.Select(x => x.Value).ToList();
                            object obj1 = (object)list;
                            return obj1;
                        }
                        object obj = (object)dropDownList;
                        return obj;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void GetUserIdsOnParentId(List<TblUserReportingDetailsTO> allUserReportinglist, Int32 parentId, ref string userIds, int reportingType)
        {
            userIds += parentId + ",";
            List<TblUserReportingDetailsTO> childList = allUserReportinglist;
            if (reportingType > 0)
            {
                childList = childList.Where(ele => ele.ReportingTo == parentId &&
              ele.ReportingTypeId == reportingType).ToList();
            }
            if (childList != null && childList.Count > 0)
            {
                foreach (TblUserReportingDetailsTO item in childList)
                {
                    GetUserIdsOnParentId(allUserReportinglist, item.UserId, ref userIds, reportingType);
                }
            }
        }

        #region Insertion
        public static int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            return TblOrgStructureDAO.InsertTblOrgStructure(tblOrgStructureTO);
        }

        public static int InsertTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.InsertTblOrgStructure(tblOrgStructureTO, conn, tran);
        }


        // Vaibhav [26-Sep-2017] Added to save organization structure hierarchy
        public static ResultMessage SaveOrganizationStructureHierarchy(TblOrgStructureTO tblOrgStructureTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            List<TblOrgStructureTO> oldOrgStructureTOList = new List<TblOrgStructureTO>();
            TblOrgStructureTO orgStructureTOForUpdate = new TblOrgStructureTO();
            int result = 0;

            try
            {
                oldOrgStructureTOList = SelectAllOrgStructureList();
                orgStructureTOForUpdate = oldOrgStructureTOList.Where(x => x.IsNewAdded == 1).FirstOrDefault();


                conn.Open();
                tran = conn.BeginTransaction();

                result = InsertTblOrgStructure(tblOrgStructureTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While SaveOrganizationStructureHierarchy");
                    return resultMessage;
                }
                if (result == 1)
                {
                    // Select current organization srtucture id
                    SqlCommand cmdSelect = new SqlCommand();
                    cmdSelect.CommandType = CommandType.Text;
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    cmdSelect.CommandText = "SELECT MAX(idOrgStructure) FROM tblOrgStructure";

                    int orgStructureId = Convert.ToInt32(cmdSelect.ExecuteScalar());

                    cmdSelect.Dispose();

                    TblRoleTO tblRoleTO = new TblRoleTO();
                    tblRoleTO.RoleDesc = tblOrgStructureTO.OrgStructureDesc;
                    tblRoleTO.IsActive = 1;
                    tblRoleTO.IsSystem = 0;
                    tblRoleTO.CreatedBy = tblOrgStructureTO.CreatedBy;
                    tblRoleTO.CreatedOn = tblOrgStructureTO.CreatedOn;
                    tblRoleTO.OrgStructureId = orgStructureId;
                    tblRoleTO.RoleTypeId = tblOrgStructureTO.RoleTypeId; // added by aniket
                    result = BL.TblRoleBL.InsertTblRole(tblRoleTO, conn, tran);
                    if (result == 1)
                    {
                        if (orgStructureTOForUpdate != null)
                        {
                            orgStructureTOForUpdate.IsNewAdded = 0;
                            orgStructureTOForUpdate.UpdatedOn = Constants.ServerDateTime; ;
                            orgStructureTOForUpdate.UpdatedBy = tblOrgStructureTO.CreatedBy;
                            result = UpdateTblOrgStructure(orgStructureTOForUpdate, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While UpdateTblOrgStructure");
                                return resultMessage;
                            }
                        }
                    }
                }
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While InsertTblRole");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveOrganizationStructureHierarchy");
                return resultMessage;
            }
            finally
            {
                conn.Close();
                tran.Dispose();
            }
        }

        // Vaibhav [26-Sep-2017] added to attach employee to specific organization structure hierarchy
        public static ResultMessage AttachNewUserToOrgStructure(TblUserReportingDetailsTO tblUserReportingDetailsTO, List<TblUserRoleTO> deactivatePosList)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region Deactivation

                if (deactivatePosList != null && deactivatePosList.Count > 0)
                {
                    //Hrushikesh[31 / 12 / 2018] Commented Deactivation of userRole + user Reporting  on deactivation List
                    //Hrushikesh[10 / 12 / 2018]Added to Deactivate UserRole while Deactivation of user from position with userRole
                    //foreach (TblUserReportingDetailsTO deTblUserReportingDetailsTO in deactivatePosList)
                    //{
                    //    deTblUserReportingDetailsTO.IsActive = 0;
                    //    List<TblUserReportingDetailsTO> EmptyUserReportingDetailsList = new List<TblUserReportingDetailsTO>();
                    //    result = UpdateUserReportingDetails(EmptyUserReportingDetailsList, deTblUserReportingDetailsTO.IdUserReportingDetails, conn, tran);
                    //    if (result != 1)
                    //    {
                    //        tran.Rollback();
                    //        resultMessage.DefaultBehaviour("Error While Updating tblUserole Against UserId" + deTblUserReportingDetailsTO.UserId);
                    //        return resultMessage;
                    //    }
                    //}

                    //Hrushikesh[31 / 12 / 2018] Added to Deactivate Only userRoles on deactivation List
                    foreach (TblUserRoleTO deTblUserReportingDetailsTO in deactivatePosList)
                    {
                        deTblUserReportingDetailsTO.IsActive = 0;
                        result = TblUserRoleBL.UpdateTblUserRole(deTblUserReportingDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Updating tblUserole Against UserId" + deTblUserReportingDetailsTO.UserId);
                            return resultMessage;
                        }
                    }



                }

                //Hrushikesh [31/12/2018]Changed to deactivate all current reportingTO for same reporting type.

                List<TblUserReportingDetailsTO> allPrevUserReportingTOList = SelectAllUserReportingDetails();

                if (allPrevUserReportingTOList != null && allPrevUserReportingTOList.Count > 0)
                {
                    allPrevUserReportingTOList = allPrevUserReportingTOList.Where(e => e.UserId == tblUserReportingDetailsTO.UserId &&
                 e.ReportingTypeId == tblUserReportingDetailsTO.ReportingTypeId).ToList();
                    if (allPrevUserReportingTOList != null && allPrevUserReportingTOList.Count > 0)
                    {

                        foreach (TblUserReportingDetailsTO deTblUserReportingDetailsTO in allPrevUserReportingTOList)
                        {
                            deTblUserReportingDetailsTO.IsActive = 0;
                            result = UpdateUserReportingDetail(deTblUserReportingDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Updating tblUserReporting ");
                                return resultMessage;
                            }
                        }
                    }


                }

                #endregion

                result = DAL.TblOrgStructureDAO.InsertTblUserReportingDetails(tblUserReportingDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                    return resultMessage;
                }


                // Hrushikesh ADDED TO AVOID DUPLICATE ROLE ENTRY
                TblRoleTO tblRoleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblUserReportingDetailsTO.OrgStructureId, conn, tran);
                List<TblUserRoleTO> tblUserRoleToList = TblUserRoleBL.SelectAllActiveUserRoleList(tblUserReportingDetailsTO.UserId)
                    .Where(e => e.RoleId == tblRoleTO.IdRole).ToList();

                if (tblUserRoleToList.Count == 0)
                {

                    if (tblRoleTO != null)
                    {
                        TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                        tblUserRoleTO.RoleId = tblRoleTO.IdRole;
                        tblUserRoleTO.RoleDesc = tblRoleTO.RoleDesc;
                        tblUserRoleTO.IsActive = 1;
                        tblUserRoleTO.UserId = tblUserReportingDetailsTO.UserId;
                        tblUserRoleTO.EnableAreaAlloc = tblRoleTO.EnableAreaAlloc;
                        tblUserRoleTO.CreatedOn = tblUserReportingDetailsTO.CreatedOn;
                        tblUserRoleTO.CreatedBy = tblUserReportingDetailsTO.CreatedBy;
                        result = TblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While adding into tblUserRole");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While adding into tblUserRole");
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static ResultMessage AttachNewUserToOrgStructure(TblUserReportingDetailsTO tblUserReportingDetailsTO, List<TblUserReportingDetailsTO> deactivatePosList, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;

            try
            {
                result = DAL.TblOrgStructureDAO.InsertTblUserReportingDetails(tblUserReportingDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                    return resultMessage;
                }

                //Sudhir[22-08-2018] Added For Deactivat Previous User Role From UserId.
                //List<TblUserRoleTO> tblUSerRoleToList = TblUserRoleBL.SelectAllActiveUserRoleList(tblUserReportingDetailsTO.UserId, conn, tran);
                //if (tblUSerRoleToList != null && tblUSerRoleToList.Count > 0)
                //{
                //    foreach (TblUserRoleTO tblUserRoleTO in tblUSerRoleToList)
                //    {
                //        tblUserRoleTO.IsActive = 0;
                //        result = TblUserRoleBL.UpdateTblUserRole(tblUserRoleTO, conn, tran);
                //        if (result != 1)
                //        {
                //            // tran.Rollback();
                //            resultMessage.DefaultBehaviour("Error While Updating tblUserole Against UserId" + tblUserRoleTO.UserId);
                //            return resultMessage;
                //        }
                //    }
                //}

                List<TblUserReportingDetailsTO> tblUSerReprtList = TblOrgStructureDAO.SelectAllUserReportingDetails(conn, tran);
                //Hrushikesh [10/12/2018]Added to Deactivate UserRole while Deactivation of user from position
                if (deactivatePosList != null && deactivatePosList.Count > 0)
                {

                    foreach (TblUserReportingDetailsTO tblUserRoleTO in deactivatePosList)
                    {

                        tblUserRoleTO.IsActive = 0;

                        result = DAL.TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReportingDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Updating tblUserole Against UserId" + tblUserRoleTO.UserId);
                            return resultMessage;
                        }
                    }
                }





                TblRoleTO tblRoleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblUserReportingDetailsTO.OrgStructureId, conn, tran);
                List<TblUserRoleTO> tblUserRoleToList = TblUserRoleBL.SelectAllActiveUserRoleList(tblUserReportingDetailsTO.UserId)
                    .Where(e => e.RoleId == tblRoleTO.IdRole).ToList();

                if (tblUserRoleToList.Count == 0)
                {

                    if (tblRoleTO != null)
                    {
                        TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                        tblUserRoleTO.RoleId = tblRoleTO.IdRole;
                        tblUserRoleTO.RoleDesc = tblRoleTO.RoleDesc;
                        tblUserRoleTO.IsActive = 1;
                        tblUserRoleTO.UserId = tblUserReportingDetailsTO.UserId;
                        tblUserRoleTO.EnableAreaAlloc = tblRoleTO.EnableAreaAlloc;
                        tblUserRoleTO.CreatedOn = tblUserReportingDetailsTO.CreatedOn;
                        tblUserRoleTO.CreatedBy = tblUserReportingDetailsTO.CreatedBy;
                        result = TblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While adding into tblUserRole");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While adding into tblUserRole");
                        return resultMessage;
                    }
                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
            }
        }
        public static int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO orgStructureHierarchyTO)
        {
            return TblOrgStructureDAO.InsertTblOrgStructureHierarchy(orgStructureHierarchyTO);
        }

        public static int InsertTblOrgStructureHierarchy(TblOrgStructureHierarchyTO orgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.InsertTblOrgStructureHierarchy(orgStructureHierarchyTO, conn, tran);
        }

        public static int InsertTblUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.InsertTblUserReportingDetails(tblUserReportingDetailsTO, conn, tran);
        }

        #endregion

        #region Updation
        public static ResultMessage UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                int result = 0;

                conn.Open();

                result = UpdateTblOrgStructure(tblOrgStructureTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateTblOrgStructure");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int UpdateTblOrgStructure(TblOrgStructureTO tblOrgStructureTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.UpdateTblOrgStructure(tblOrgStructureTO, conn, tran);
        }

        // Vaibhav [28-Sep-2017] added to deactivate respective organization structure. 
        public static ResultMessage DeactivateOrgStructure(TblOrgStructureTO tblOrgStructureTO, Int32 ReportingTypeId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();

            List<TblOrgStructureHierarchyTO> orgHierarchyList = TblOrgStructureBL.SelectAllTblOrgStructureHierarchy();
            List<TblOrgStructureHierarchyTO> updationHierarchyList = new List<TblOrgStructureHierarchyTO>();
            try
            {
                #region
                //int result = 0;
                //conn.Open();
                //tran = conn.BeginTransaction();
                //result = DAL.TblOrgStructureDAO.UpdateTblOrgStructure(tblOrgStructureTO, conn, tran);
                //if (result < 1)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour("Error While DeactivateOrgStructure");
                //    return resultMessage;
                //}

                //// select all org srtucture id and its child ids list 
                //String orgStructureIdList = TblOrgStructureDAO.SelectAllOrgStructureIdList(tblOrgStructureTO, conn, tran);

                //// Update all employees from parent and its child org structure ids 
                //result = TblOrgStructureDAO.DeactivateOrgStructureEmployees(orgStructureIdList, conn, tran);

                //if (result < 0)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour("Error While DeactivateOrgStructureEmployees");
                //    return resultMessage;
                //}

                //tran.Commit();
                //resultMessage.DefaultSuccessBehaviour();
                #endregion

                int result = 0;
                conn.Open();
                tran = conn.BeginTransaction();
                result = DAL.TblOrgStructureDAO.UpdateTblOrgStructure(tblOrgStructureTO, conn, tran);
                if (result < 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While DeactivateOrgStructure");
                    return resultMessage;
                }

                //After Deactivating Position Need to Deactivate Role 

                TblRoleTO tblRoleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblOrgStructureTO.IdOrgStructure, conn, tran);
                if (tblRoleTO != null)
                {
                    tblRoleTO.IsActive = 0;

                    result = TblRoleBL.UpdateTblRole(tblRoleTO, conn, tran);
                    if (result < 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While DeactivateOrgStructure");
                        return resultMessage;
                    }
                }


                //For Geting OrgId Wise and Specific ReportingTypeId for Deactivation.
                updationHierarchyList = orgHierarchyList.Where(x => x.OrgStructureId == tblOrgStructureTO.IdOrgStructure && x.ReportingTypeId == ReportingTypeId).ToList();
                if (updationHierarchyList != null && updationHierarchyList.Count > 0)
                {
                    for (int i = 0; i < updationHierarchyList.Count; i++)
                    {
                        TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO = updationHierarchyList[i];
                        tblOrgStructureHierarchyTO.IsActive = 0;
                        tblOrgStructureHierarchyTO.UpdatedOn = Constants.ServerDateTime;
                        tblOrgStructureHierarchyTO.UpdatedBy = tblOrgStructureTO.UpdatedBy;
                        result = UpdateTblOrgStructureHierarchy(tblOrgStructureHierarchyTO, conn, tran);
                        if (result < 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While DeactivateOrgStructure");
                            return resultMessage;
                        }
                    }
                }

                TblOrgStructureTO newTblOrgStructureTO = SelectBOMOrgStructure();
                if (newTblOrgStructureTO != null)
                {
                    newTblOrgStructureTO.IsNewAdded = 1;
                    newTblOrgStructureTO.UpdatedOn = Constants.ServerDateTime;
                    newTblOrgStructureTO.UpdatedBy = tblOrgStructureTO.UpdatedBy;
                    result = DAL.TblOrgStructureDAO.UpdateTblOrgStructure(newTblOrgStructureTO, conn, tran);
                    if (result < 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While DeactivateOrgStructure");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateDepartment");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        // Vaibhav [29-Sep-2017] added to update user reporting details
        public static ResultMessage UpdateUserReportingDetails(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                int result = 0;

                conn.Open();

                result = DAL.TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReportingDetailsTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateUserReportingDetails");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateUserReportingDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int UpdateUserReportingDetail(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            return TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReportingDetailsTO);
        }
        public static int UpdateUserReportingDetail(TblUserReportingDetailsTO tblUserReportingDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReportingDetailsTO, conn, tran);
        }


        public static ResultMessage UpdateChildOrgStructure(TblOrgStructureTO orgStructureTO, Int32 reportingTypeId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (orgStructureTO != null)
                {
                    orgStructureTO.UpdatedOn = Constants.ServerDateTime;
                    result = DAL.TblOrgStructureDAO.UpdateChildTblOrgStructure(orgStructureTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While UpdateChildOrgStructure");
                        return resultMessage;
                    }

                    TblOrgStructureHierarchyTO existingTblOrgStructureHierarchyTO = BL.TblOrgStructureBL.SelectTblOrgStructureHierarchyTO(orgStructureTO.IdOrgStructure, orgStructureTO.ParentOrgStructureId, reportingTypeId, conn, tran);
                    if (existingTblOrgStructureHierarchyTO != null)
                    {
                        existingTblOrgStructureHierarchyTO.IsActive = 1;
                        result = UpdateTblOrgStructureHierarchy(existingTblOrgStructureHierarchyTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblOrgStructureHierarchy");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        TblOrgStructureHierarchyTO orgStructureHierarchyTO = new TblOrgStructureHierarchyTO();
                        orgStructureHierarchyTO.OrgStructureId = orgStructureTO.IdOrgStructure;
                        orgStructureHierarchyTO.ParentOrgStructId = orgStructureTO.ParentOrgStructureId;
                        orgStructureHierarchyTO.ReportingTypeId = reportingTypeId;
                        orgStructureHierarchyTO.CreatedOn = orgStructureTO.UpdatedOn;
                        orgStructureHierarchyTO.CreatedBy = orgStructureTO.UpdatedBy;
                        orgStructureHierarchyTO.IsActive = 1;
                        result = InsertTblOrgStructureHierarchy(orgStructureHierarchyTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblOrgStructureHierarchy");
                            return resultMessage;
                        }
                    }

                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveOrganizationStructureHierarchy");
                return resultMessage;
            }
            finally
            {
                conn.Close();
                tran.Dispose();
            }
        }

        #endregion

        #region Deletion
        public static int DeleteTblOrgStructure(Int32 idOrgStructure)
        {
            return TblOrgStructureDAO.DeleteTblOrgStructure(idOrgStructure);
        }

        public static int DeleteTblOrgStructure(Int32 idOrgStructure, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.DeleteTblOrgStructure(idOrgStructure, conn, tran);
        }

        #endregion


        #region Organization Structure Display

        //Sudhir[01-JAN-2018] Added for Display organization Structure New Req.
        public static List<TblOrgStructureTO> GetOrgStructureListForDisplay(int reportingTypeId)
        {
            List<TblOrgStructureTO> allorgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();
            List<DimMstDeptTO> mstDeptToList = DimMstDeptBL.SelectAllDimMstDeptList();
            List<TblOrgStructureTO> orgStructureTOList = new List<TblOrgStructureTO>();
            List<TblUserReportingDetailsTO> bomEmployeeList = TblOrgStructureDAO.SelectOrgStructureUserDetailsForBom(1);
            TblOrgStructureTO orgStructureBOMTO = SelectBOMOrgStructure();
            List<TblOrgStructureHierarchyTO> tblOrgStructureHierarchyTO = SelectTblOrgStructureHierarchyOnReportingType(reportingTypeId);
            orgStructureTOList.Add(orgStructureBOMTO);
            //List<TblOrgStructureTO> orgStructureTOList = TblOrgStructureDAO.SelectAllOrgStructureHierarchy();

            //Call Function to Arrange Parrent Child List Based on HierarchyTable.
            MakeOrgStructureList(orgStructureBOMTO, allorgStructureTOList, tblOrgStructureHierarchyTO, reportingTypeId, orgStructureTOList);
            List<TblOrgStructureTO> orgStructureTOListForDisplay = new List<TblOrgStructureTO>();
            List<TblUserReportingDetailsTO> alluserReportingDetailsTOList = TblOrgStructureDAO.SelectOrgStructureUserDetails(0);

            if (reportingTypeId == Convert.ToInt16(Constants.ReportingTypeE.ADMINISTRATIVE))
            {
                alluserReportingDetailsTOList = TblOrgStructureDAO.SelectOrgStructureUserDetailsByReportingType(0, reportingTypeId);
            }
            else
            {
                alluserReportingDetailsTOList = TblOrgStructureDAO.SelectOrgStructureUserDetailsByReportingType(0, reportingTypeId);
            }
            List<TblOrgStructureTO> orgStructureTOListUserDisplay = new List<TblOrgStructureTO>();
            orgStructureTOList[0].PositionName = orgStructureTOList[0].OrgStructureDesc;
            #region OLD CODE
            //foreach (DimMstDeptTO mstDeptTo in mstDeptToList)
            //{
            //    Int32 IdDept = mstDeptTo.IdDept;
            //    List<TblOrgStructureTO> orgStructureTOListTemp = orgStructureTOList.FindAll(ele => ele.DeptId == IdDept);
            //    if (orgStructureTOListTemp.Count > 0)
            //    {
            //        for (int ele = 0; ele < orgStructureTOListTemp.Count; ele++)
            //        {
            //             orgStructureTOListTemp.Where(c => c.DeptId == IdDept).ToList().ForEach(cc => cc.IdOrgStructure = orgStructureTOListTemp[0].IdOrgStructure);
            //            List<TblOrgStructureTO> list = orgStructureTOList.Where(x => x.ParentOrgStructureId == orgStructureTOListTemp[ele].IdOrgStructure).ToList();
            //            orgStructureTOListForDisplay.Add(orgStructureTOListTemp[ele]);
            //        }
            //    }
            //    else
            //    {
            //        TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
            //        orgStructureTO.IdOrgStructure = mstDeptTo.IdDept;
            //        orgStructureTO.DeptId = mstDeptTo.IdDept;
            //        orgStructureTO.ParentOrgStructureId = mstDeptTo.ParentDeptId;
            //        orgStructureTO.OrgStructureDesc = mstDeptTo.DeptDisplayName;
            //        orgStructureTOListForDisplay.Add(orgStructureTO);


            //    }
            //}
            #endregion

            if (orgStructureTOList != null) //if True Then First Show All Department Structure.
            {

                for (int ele = 0; ele < orgStructureTOList.Count; ele++)
                {
                    orgStructureTOList[ele].ParentOrgDisplayId = "#" + orgStructureTOList[ele].ParentOrgStructureId;
                    orgStructureTOList[ele].TempOrgStructId = "#" + orgStructureTOList[ele].IdOrgStructure;
                }

                for (int i = 0; i < orgStructureTOList.Count; i++)
                {
                    orgStructureTOList[i].IsDept = 0;

                    #region OLD CODE
                    //if (childUserReportingList != null)
                    //{
                    //    //for (int lst = 0; lst < childUserReportingList.Count; lst++)
                    //    //{
                    //    //    TblUserReportingDetailsTO userReportingDetailsTO = childUserReportingList[lst];
                    //    //    orgStructureTOList[i].EmployeeName = childUserReportingList[lst].UserName;
                    //    //    orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                    //    //}

                    //    //if (childUserReportingList.Count == 1)
                    //    //{
                    //    //    orgStructureTOList[i].EmployeeName = childUserReportingList[0].UserName;
                    //    //}
                    //    //else
                    //    //{

                    //    //}
                    //}
                    //else
                    //{
                    // orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                    //}
                    #endregion

                    orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                    List<DimMstDeptTO> tempList = mstDeptToList.Where(x => x.ParentDeptId == orgStructureTOList[i].DeptId).ToList();

                    if (tempList != null && tempList.Count > 0)
                    {
                        for (int k = 0; k < tempList.Count; k++)
                        {
                            List<TblOrgStructureTO> list = new List<TblOrgStructureTO>();

                            if (reportingTypeId == Convert.ToInt16(Constants.ReportingTypeE.ADMINISTRATIVE))
                            {
                                list = orgStructureTOList.Where(ele => ele.ParentOrgStructureId == orgStructureTOList[i].IdOrgStructure && tempList[k].IdDept == ele.DeptId).ToList();
                            }
                            else
                            {
                                list = orgStructureTOList.Where(ele => ele.ParentOrgStructureId == orgStructureTOList[i].IdOrgStructure && tempList[k].IdDept == ele.DeptId).ToList();
                            }

                            if (list != null && list.Count == 0)
                            {
                                //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                                GetOrg(orgStructureTOList[i].TempOrgStructId, orgStructureTOList[i].DeptId, orgStructureTOListForDisplay, mstDeptToList, tempList[k].IdDept, reportingTypeId);
                            }
                            else
                            {
                                //for (int j = 0; j < orgStructureTOList.Count; j++)
                                //{
                                //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                                //}

                            }
                        }
                    }
                    else
                    {
                        //orgStructureTOListForDisplay.Add(orgStructureTOList[i]);
                    }

                }
                #region Commented
                //List<DimMstDeptTO> mstDeptToList = DimMstDeptBL.SelectAllDimMstDeptList();
                //for (int i = 0; i < mstDeptToList.Count; i++)
                //{
                //    TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
                //    orgStructureTO.IdOrgStructure = mstDeptToList[i].IdDept;
                //    orgStructureTO.DeptId = mstDeptToList[i].IdDept;
                //    orgStructureTO.ParentOrgStructureId = mstDeptToList[i].ParentDeptId;
                //    orgStructureTO.OrgStructureDesc = mstDeptToList[i].DeptDisplayName;
                //    OrgStructureTOList.Add(orgStructureTO);
                //}
                #endregion
            }
            if (alluserReportingDetailsTOList != null)
            {
                UserwiseOrgChart(orgStructureTOListForDisplay, alluserReportingDetailsTOList, "#0", orgStructureTOListUserDisplay, 0, "#0", 0);
                //Make Missed UserWise List.
                MakeMissedUserList(alluserReportingDetailsTOList, orgStructureTOListUserDisplay, 1);
            }
            else
                  if (alluserReportingDetailsTOList != null)
            {
                UserwiseOrgChart(orgStructureTOListForDisplay, alluserReportingDetailsTOList, "#0", orgStructureTOListUserDisplay, 0, "#0", 0);
                //Make Missed UserWise List.
                MakeMissedUserList(alluserReportingDetailsTOList, orgStructureTOListUserDisplay, 1);
            }
            else
                orgStructureTOListUserDisplay = orgStructureTOListForDisplay;

            TblOrgStructureTO bomHeadTO = new TblOrgStructureTO();

            bomHeadTO.IdOrgStructure = 0;
            bomHeadTO.ParentOrgDisplayId = orgStructureTOListUserDisplay[0].TempOrgStructId;
            bomHeadTO.OrgStructureDesc = "BOM";
            bomHeadTO.TempOrgStructId = "*#BOM-HEAD_";
            bomHeadTO.ActualOrgStructureId = 1;
            bomHeadTO.PositionName = "BOARD OF MEMBERS ";
            bomHeadTO.ParentOrgStructureId = 1;
            orgStructureTOListUserDisplay.Add(bomHeadTO);


            if (bomEmployeeList != null && bomEmployeeList.Count > 0)
                for (int i = 0; i < bomEmployeeList.Count; i++)
                {
                    TblOrgStructureTO bomEmployeeTO = new TblOrgStructureTO();

                    bomEmployeeTO.IdOrgStructure = 0;
                    bomEmployeeTO.ParentOrgDisplayId = bomHeadTO.TempOrgStructId;
                    bomEmployeeTO.OrgStructureDesc = bomEmployeeList[i].UserName;
                    bomEmployeeTO.EmployeeName = bomEmployeeList[i].UserName;
                    bomEmployeeTO.EmployeeId = bomEmployeeList[i].UserId;
                    bomEmployeeTO.PositionName = "BOARD OF MEMBERS";
                    bomEmployeeTO.TempOrgStructId = bomEmployeeList[i].UserName + i;
                    bomEmployeeTO.ActualOrgStructureId = 1;
                    bomEmployeeTO.ParentOrgStructureId = 1;
                    orgStructureTOListUserDisplay.Add(bomEmployeeTO);

                }


            return orgStructureTOListUserDisplay;
        }

        public static void MakeMissedUserList(List<TblUserReportingDetailsTO> allUserReportingList, List<TblOrgStructureTO> displayOrgStructureList, Int32 uniquiNo)
        {
            if (allUserReportingList != null && allUserReportingList.Count > 0 && displayOrgStructureList != null && displayOrgStructureList.Count > 0)
            {
                for (int i = 0; i < allUserReportingList.Count; i++)
                {
                    List<TblOrgStructureTO> list = displayOrgStructureList.Where(ele => ele.EmployeeId == allUserReportingList[i].UserId).ToList();
                    if (list.Count == 0)
                    {
                        TblOrgStructureTO orgStructureTO = displayOrgStructureList.Where(ele => ele.EmployeeId == allUserReportingList[i].ReportingTo).FirstOrDefault();

                        if (orgStructureTO != null)
                        {
                            TblOrgStructureTO childTo = new TblOrgStructureTO();

                            childTo.EmployeeName = allUserReportingList[i].UserName;
                            childTo.EmployeeId = allUserReportingList[i].UserId;
                            childTo.PositionName = allUserReportingList[i].PositionName;
                            //tblOrgStructureTOUser.TempOrgStructId += "_" + i;
                            childTo.TempOrgStructId += "#_" + uniquiNo + "_" + allUserReportingList[i].IdUserReportingDetails;
                            uniquiNo++;
                            childTo.ParentOrgDisplayId = orgStructureTO.TempOrgStructId;

                            displayOrgStructureList.Add(childTo);
                        }

                    }
                    else
                    {
                        continue;
                    }

                }
            }
        }

        public static void UserwiseOrgChart(List<TblOrgStructureTO> OrgStructureTOList, List<TblUserReportingDetailsTO> alluserReportingDetailsTOList, String ParentId, List<TblOrgStructureTO> orgStructureTOListUserDisplay, Int32 userId, String newParent, Int32 uniquiNo)
        {
            List<TblOrgStructureTO> tempList = OrgStructureTOList.Where(x => x.ParentOrgDisplayId == ParentId).ToList();

            if (tempList != null && tempList.Count > 0)
            {
                for (int ele = 0; ele < tempList.Count; ele++)
                {

                    if (tempList[ele].IdOrgStructure == 3)
                    {

                    }

                    TblOrgStructureTO tblOrgStructureTO = tempList[ele];
                    List<TblUserReportingDetailsTO> userList = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == tblOrgStructureTO.IdOrgStructure).ToList();

                    Boolean isAdded = false;

                    if (userList != null && userList.Count > 0)
                    {
                        for (int i = 0; i < userList.Count; i++)
                        {
                            if (userList[i].ReportingTo == userId)
                            {
                                isAdded = true;

                                TblOrgStructureTO tblOrgStructureTOUser = tblOrgStructureTO.Clone();

                                tblOrgStructureTOUser.EmployeeName = userList[i].UserName;
                                tblOrgStructureTOUser.EmployeeId = userList[i].UserId;

                                //tblOrgStructureTOUser.TempOrgStructId += "_" + i;
                                tblOrgStructureTOUser.TempOrgStructId += "_" + uniquiNo + "_" + userList[i].IdUserReportingDetails;
                                uniquiNo++;

                                tblOrgStructureTOUser.ParentOrgDisplayId = newParent;

                                orgStructureTOListUserDisplay.Add(tblOrgStructureTOUser);

                                UserwiseOrgChart(OrgStructureTOList, alluserReportingDetailsTOList, tblOrgStructureTO.TempOrgStructId, orgStructureTOListUserDisplay, userList[i].UserId, tblOrgStructureTOUser.TempOrgStructId, uniquiNo);
                            }
                        }
                    }
                    if (!isAdded)
                    {

                        TblOrgStructureTO tblOrgStructureTOUser = tblOrgStructureTO.Clone();

                        //tblOrgStructureTOUser.TempOrgStructId += "_E" + userId;
                        tblOrgStructureTOUser.TempOrgStructId += "_" + uniquiNo + "_U" + userId; ;
                        uniquiNo++;

                        tblOrgStructureTOUser.ParentOrgDisplayId = newParent;

                        orgStructureTOListUserDisplay.Add(tblOrgStructureTOUser);
                        UserwiseOrgChart(OrgStructureTOList, alluserReportingDetailsTOList, tblOrgStructureTO.TempOrgStructId, orgStructureTOListUserDisplay, 0, tblOrgStructureTOUser.TempOrgStructId, uniquiNo);

                    }


                }
            }

            #region Commented
            //if (alluserReportingDetailsTOList != null)
            //{
            //    for (int i = 0; i < OrgStructureTOList.Count; i++)
            //    {
            //        TblOrgStructureTO orgStructureTO = OrgStructureTOList[i];
            //        List<TblUserReportingDetailsTO> list = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == orgStructureTO.IdOrgStructure).ToList();
            //        if(list !=null && list.Count > 0)
            //        {

            //        }
            //    }
            //    //List<TblUserReportingDetailsTO> childUserReportingList = alluserReportingDetailsTOList.Where(x => x.OrgStructureId == orgStructureTOList[i].IdOrgStructure).ToList();

            //}
            #endregion
        }

        //Sudhir[03-JAN-2018] Added Recursive Method For Add Department in OrgnizationStructure List.
        public static void GetOrg(String ParentId, Int32 DeptId, List<TblOrgStructureTO> OrgStructureTOList, List<DimMstDeptTO> MstDeptTOList, Int32 deptId, int reportingTypeId)
        {
            List<DimMstDeptTO> list = new List<DimMstDeptTO>();
            if (deptId > 0)
            {
                list = MstDeptTOList.Where(x => x.ParentDeptId == DeptId && x.IdDept == deptId).ToList();
            }
            else
            {
                list = MstDeptTOList.Where(x => x.ParentDeptId == DeptId).ToList();
            }
            if (list != null && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    TblOrgStructureTO orgStructureTO = new TblOrgStructureTO();
                    //orgStructureTO.IdOrgStructure = list[i].IdDept;
                    orgStructureTO.DeptId = list[i].IdDept;
                    orgStructureTO.TempOrgStructId = "*" + list[i].IdDept + ParentId;
                    if (ParentId.Contains('*'))
                    {
                        orgStructureTO.ParentOrgStructureId = 0;
                    }
                    else
                    {
                        orgStructureTO.ParentOrgStructureId = Convert.ToInt32(ParentId.Trim('#'));
                    }
                    orgStructureTO.OrgStructureDesc = list[i].DeptDisplayName;
                    orgStructureTO.PositionName = list[i].DeptDisplayName;
                    orgStructureTO.ParentOrgDisplayId = ParentId;
                    OrgStructureTOList.Add(orgStructureTO);
                    orgStructureTO.IsDept = 1;
                    GetOrg(orgStructureTO.TempOrgStructId, list[i].IdDept, OrgStructureTOList, MstDeptTOList, 0, reportingTypeId);
                }
            }
        }

        public static void MakeOrgStructureList(TblOrgStructureTO orgstrctureTO, List<TblOrgStructureTO> allOrgStructutreList, List<TblOrgStructureHierarchyTO> hierarchyList, int reportingTypeId, List<TblOrgStructureTO> listForDisplay)
        {
            if (orgstrctureTO.IdOrgStructure == 3)
            {

            }

            List<TblOrgStructureHierarchyTO> tempHierarchyList = hierarchyList.Where(x => x.ParentOrgStructId == orgstrctureTO.IdOrgStructure && x.ReportingTypeId == reportingTypeId).ToList();
            if (tempHierarchyList != null && tempHierarchyList.Count > 0)
            {
                for (int i = 0; i < tempHierarchyList.Count; i++)
                {
                    TblOrgStructureHierarchyTO temp = tempHierarchyList[i];
                    TblOrgStructureTO orgTO = new TblOrgStructureTO();
                    orgTO = allOrgStructutreList.Where(x => x.IdOrgStructure == temp.OrgStructureId).FirstOrDefault();
                    if (orgTO != null)
                    {
                        orgTO.ParentOrgStructureId = temp.ParentOrgStructId;
                        listForDisplay.Add(orgTO);
                        MakeOrgStructureList(orgTO, allOrgStructutreList, hierarchyList, reportingTypeId, listForDisplay);
                    }
                }
            }
        }

        #endregion

        #region Deactivation
        public static ResultMessage DeactivateUserReporting(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            List<TblUserReportingDetailsTO> allUserReportingToList = TblOrgStructureDAO.SelectAllUserReportingDetails();
            List<TblOrgStructureHierarchyTO> orgStructureHierarchyTOList = TblOrgStructureDAO.SelectAllTblOrgStructureHierarchy();
            Int32 UserId = tblUserReportingDetailsTO.UserId;

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                result = DAL.TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReportingDetailsTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                    return resultMessage;
                }
                #region Commented Code
                //if(result==1)
                //{
                //    List<TblOrgStructureHierarchyTO> parentOrgHierarchyList = orgStructureHierarchyTOList.Where(ele => ele.ParentOrgStructId == tblUserReportingDetailsTO.OrgStructureId).ToList();
                //    for (int i = 0; i < parentOrgHierarchyList.Count; i++)
                //    {
                //        TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO = parentOrgHierarchyList[i];
                //        List<TblUserReportingDetailsTO> userReportingDetailsTOList = allUserReportingToList.Where(x => x.OrgStructureId == tblOrgStructureHierarchyTO.OrgStructureId && x.ReportingTo == UserId).ToList();
                //        if(userReportingDetailsTOList != null && userReportingDetailsTOList.Count > 0)
                //        {
                //            for (int ele = 0; ele < userReportingDetailsTOList.Count; ele++)
                //            {
                //                TblUserReportingDetailsTO tblUserReporting = userReportingDetailsTOList[ele];
                //                tblUserReporting.IsActive = 0;
                //                tblUserReporting.DeActivatedBy = tblUserReportingDetailsTO.DeActivatedBy;
                //                tblUserReporting.DeActivatedOn = tblUserReportingDetailsTO.DeActivatedOn;
                //                result = DAL.TblOrgStructureDAO.UpdateUserReportingDetails(tblUserReporting, conn, tran);
                //                if(result !=1)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                //                    return resultMessage;
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public static ResultMessage UpdateUserReportingDetails(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            ResultMessage resultMessage = new ResultMessage();
            SqlTransaction tran = null;
            int result = 0;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (tblUserReportingDetailsTO != null && tblUserReportingDetailsTO.Count > 0)
                {

                    foreach (TblUserReportingDetailsTO userReportingTo in tblUserReportingDetailsTO)
                    {
                        result = UpdateUserReportingDetail(userReportingTo, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return resultMessage;
                        }
                    }


                }

                if (userReportingId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTODeactivation = SelectUserReportingDetailsTO(userReportingId, conn, tran);

                    TblRoleTO roleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblUserReportingDetailsTODeactivation.OrgStructureId, conn, tran);

                    List<TblUserRoleTO> tblUserReportingRoleTODeactivation = TblUserRoleBL.SelectAllTblUserRoleList();

                    //Added to Deactivate UserRole while Deactivation of user from position
                    List<TblUserReportingDetailsTO> multipleReportingList = SelectAllUserReportingDetails()
                        .Where(e => e.UserId == tblUserReportingDetailsTODeactivation.UserId &&
                                e.OrgStructureId == tblUserReportingDetailsTODeactivation.OrgStructureId &&
                                e.IdUserReportingDetails != userReportingId).ToList();
                    if (multipleReportingList.Count == 0)
                    {

                        List<TblUserRoleTO> deactivateUserRoleList = tblUserReportingRoleTODeactivation.Where(x => x.RoleId == roleTO.IdRole &&
                        x.UserId == tblUserReportingDetailsTODeactivation.UserId).ToList();
                        if (deactivateUserRoleList.Count > 0)

                        {
                            foreach (TblUserRoleTO UserRoleToDeactivate in deactivateUserRoleList)

                            {
                                UserRoleToDeactivate.IsActive = 0;

                                result = TblUserRoleBL.UpdateTblUserRole(UserRoleToDeactivate, conn, tran);
                            }

                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                                return resultMessage;
                            }
                        }
                    }
                    if (tblUserReportingDetailsTODeactivation != null)
                    {
                        tblUserReportingDetailsTODeactivation.IsActive = 0;
                        result = UpdateUserReportingDetail(tblUserReportingDetailsTODeactivation, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return resultMessage;
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                        return resultMessage;
                    }
                }

                else
                {
                    resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public static int UpdateUserReportingDetails(List<TblUserReportingDetailsTO> tblUserReportingDetailsTO, Int32 userReportingId, SqlConnection conn, SqlTransaction tran)
        {


            ResultMessage resultMessage = new ResultMessage();

            int result = 0;

            try
            {


                if (tblUserReportingDetailsTO != null && tblUserReportingDetailsTO.Count > 0)
                {

                    foreach (TblUserReportingDetailsTO userReportingTo in tblUserReportingDetailsTO)
                    {
                        result = UpdateUserReportingDetail(userReportingTo, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return result;
                        }
                    }


                }

                if (userReportingId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTODeactivation = SelectUserReportingDetailsTO(userReportingId, conn, tran);

                    TblRoleTO roleTO = TblRoleBL.SelectTblRoleOnOrgStructureId(tblUserReportingDetailsTODeactivation.OrgStructureId, conn, tran);

                    List<TblUserRoleTO> tblUserReportingRoleTODeactivation = TblUserRoleBL.SelectAllTblUserRoleList();


                    //Hrushikesh [10/12/2018]Added to Deactivate UserRole while Deactivation of user from position
                    List<TblUserRoleTO> deactivateUserRoleList = tblUserReportingRoleTODeactivation.Where(x => x.RoleId == roleTO.IdRole &&
                    x.UserId == tblUserReportingDetailsTODeactivation.UserId).ToList();
                    if (deactivateUserRoleList.Count > 0)

                    {
                        foreach (TblUserRoleTO UserRoleToDeactivate in deactivateUserRoleList)

                        {
                            UserRoleToDeactivate.IsActive = 0;

                            result = TblUserRoleBL.UpdateTblUserRole(UserRoleToDeactivate, conn, tran);
                        }

                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return result;
                        }
                    }
                    if (tblUserReportingDetailsTODeactivation != null)
                    {
                        tblUserReportingDetailsTODeactivation.IsActive = 0;
                        result = UpdateUserReportingDetail(tblUserReportingDetailsTODeactivation, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While AttachNewEmployeeToOrgStructure");
                            return result;
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                        return result;
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("Error While Changing UserReporting Details");
                    return result;
                }

                resultMessage.DefaultSuccessBehaviour();
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AttachNewEmployeeToOrgStructure");
                return result;
            }
            finally
            {
                //  tran.Dispose();
            }
        }


        public static List<TblUserReportingDetailsTO> SelectOrgStructureUserDetails(string ids)
        {
            return TblOrgStructureDAO.SelectOrgStructureUserDetails(ids);
        }

        public static List<TblUserReportingDetailsTO> SelectUserReportingOnuserIds(string ids, int reportingTo)
        {
            return TblOrgStructureDAO.SelectUserReportingOnuserIds(ids, reportingTo);
        }


        /// <summary>
        /// 17-OCT-2018 Added for Showing Reporting To List Of Employee for Deactivation
        /// </summary>
        /// <param name="tblUserReportingDetailsTO"></param>
        /// <returns></returns>
        public static List<TblUserReportingDetailsTO> SelectDeactivateUserReportingList(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            String id = String.Empty;
            List<TblUserReportingDetailsTO> userReportingDetailsList = new List<TblUserReportingDetailsTO>();
            try
            {
                if (tblUserReportingDetailsTO != null)
                {
                    GetParentPositionIds(tblUserReportingDetailsTO.OrgStructureId, ref id, tblUserReportingDetailsTO.ReportingTypeId);

                    id = id + tblUserReportingDetailsTO.OrgStructureId;
                    //SelectDeactivateChildEmployeeList(tblUserReportingDetailsTO);
                    userReportingDetailsList = SelectOrgStructureUserDetails(id);
                    if (userReportingDetailsList != null && userReportingDetailsList.Count > 0)
                    {
                        userReportingDetailsList = userReportingDetailsList.Where(element => element.UserId != tblUserReportingDetailsTO.UserId).ToList();
                        //userReportingDetailsList.ForEach(element => element.ReportingTo = 0);
                    }
                }
                return userReportingDetailsList;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 17-OCT-2018 Added for Attaching Child Position List.
        /// </summary>
        /// <param name="tblUserReportingDetailsTO"></param>
        /// <returns></returns>
        public static List<TblUserReportingDetailsTO> SelectDeactivateChildEmployeeList(TblUserReportingDetailsTO tblUserReportingDetailsTO)
        {
            String id = String.Empty;
            List<TblUserReportingDetailsTO> listUserReportingList = new List<TblUserReportingDetailsTO>();
            try
            {
                if (tblUserReportingDetailsTO != null)
                {
                    object list = ChildUserListOnUserId(tblUserReportingDetailsTO.UserId, 1, tblUserReportingDetailsTO.ReportingTypeId);
                    if (list != null)
                    {
                        List<int> intList = (List<int>)list;
                        id = string.Join<int>(",", intList);
                        listUserReportingList = SelectUserReportingOnuserIds(id, tblUserReportingDetailsTO.UserId);
                        if (listUserReportingList != null && listUserReportingList.Count > 0)
                        {
                            listUserReportingList.ForEach(element => { element.ReportingTo = 0; element.ReportingToName = String.Empty; });
                        }
                    }
                }
                return listUserReportingList;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {

            }
        }

        public static TblUserReportingDetailsTO SelectUserReportingDetailsTOBom(int IdUserReportingDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.SelectUserReportingDetailsTOBom(IdUserReportingDetails, conn, tran);
        }

        #endregion


        public static int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO)
        {
            return TblOrgStructureDAO.UpdateTblOrgStructureHierarchy(tblOrgStructureHierarchyTO);
        }
        public static int UpdateTblOrgStructureHierarchy(TblOrgStructureHierarchyTO tblOrgStructureHierarchyTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblOrgStructureDAO.UpdateTblOrgStructureHierarchy(tblOrgStructureHierarchyTO, conn, tran);
        }
    }
}
