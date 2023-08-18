using SalesTrackerAPI.DAL;
using SalesTrackerAPI.Models;
using SalesTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SalesTrackerAPI.BL
{
    public class TblProdClassificationBL
    {
        #region Selection

        public static List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "")
        {
            return TblProdClassificationDAO.SelectAllTblProdClassification(prodClassType);
        }
        public static List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            return TblProdClassificationDAO.SelectAllTblProdClassification(conn, tran, prodClassType);
        }

        public static List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            return TblProdClassificationDAO.SelectAllProdClassificationForDropDown(parentClassId);

        }
        public static TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass)
        {
            return TblProdClassificationDAO.SelectTblProdClassification(idProdClass);
        }

        public static List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            return TblProdClassificationDAO.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
        }
        //Sudhir[15-March-2018] Added for getList classification  based on  Categeory or Subcategory or specification.
        public static string SelectProdtClassificationListOnType(Int32 idProdClass)
        {
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                TblProdClassificationTO tblProdClassificationTO = allProdClassificationList.Where(ele => ele.IdProdClass == idProdClass).FirstOrDefault();
                String tempids = String.Empty;
                String idsProdClass = String.Empty;
                if (allProdClassificationList != null && tblProdClassificationTO != null)
                {
                    GetIdsofProductClassification(allProdClassificationList, idProdClass, ref tempids);
                }
                idsProdClass = tempids.TrimEnd(',');
                return idsProdClass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Sudhir[15-March-2018] Added for getIds of productClassification.
        public static void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, ref String ids)
        {
            ids += parentId + ",";
            List<TblProdClassificationTO> childList = allList.Where(ele => ele.ParentProdClassId == parentId).ToList();
            if (childList != null && childList.Count > 0)
            {
                foreach (TblProdClassificationTO item in childList)
                {
                    GetIdsofProductClassification(allList, item.IdProdClass, ref ids);
                }
            }
        }
        #endregion

        #region Product Classification DisplayName
        public static void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList)
        {
            String DisplayName = String.Empty;
            List<TblProdClassificationTO> DisplayNameList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTO != null)
            {
                //List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                GetDisplayName(allProdClassificationList, tblProdClassificationTO.ParentProdClassId, DisplayNameList);
                DisplayNameList = DisplayNameList.OrderBy(x => x.IdProdClass).ToList();
                if (DisplayNameList != null && DisplayNameList.Count > 0)
                {
                    for (int ele = 0; ele < DisplayNameList.Count; ele++)
                    {
                        TblProdClassificationTO tempTo = DisplayNameList[ele];
                        DisplayName += tempTo.ProdClassDesc + "/";
                    }
                }
                else if (DisplayNameList.Count == 0)
                {

                }
                else
                {
                    DisplayName += DisplayNameList[0].ProdClassDesc + "/";
                }
                tblProdClassificationTO.DisplayName = DisplayName + tblProdClassificationTO.ProdClassDesc;
            }
        }

        public static void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList)
        {

            if (allProdClassificationList != null && allProdClassificationList.Count > 0)
            {
                List<TblProdClassificationTO> tempList = allProdClassificationList.Where(ele => ele.IdProdClass == parentId).ToList();
                if (tempList != null && tempList.Count > 0)
                {
                    if (tempList[0].ParentProdClassId == 0)
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                    }
                    else
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                        GetDisplayName(allProdClassificationList, tempList[0].ParentProdClassId, DisplayNameList);
                    }
                }
            }
        }
        #endregion

        #region Insertion

        /// <summary>
        /// Priyanka [23-02-2018] Added for Set the DisplayName of Product Classification.
        /// </summary>
        /// <param name="tblProdClassificationTO"></param>
        /// <returns></returns>
        public static int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
            SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
            return TblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public static int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return TblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public static int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
        }


        /// <summary>
        /// Priyanka [23-02-2018] Added for Updating DisplayName Recursively.
        /// </summary>
        /// <param name="allProdClassificationList"></param>
        /// <param name="ProdClassificationTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, ref String idClassStr, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            List<TblProdClassificationTO> childList = allProdClassificationList.Where(ele => ele.ParentProdClassId == ProdClassificationTO.IdProdClass).ToList();
            if (childList != null && childList.Count > 0)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    TblProdClassificationTO tempTo = childList[i];
                    tempTo.UpdatedOn = childList[i].CreatedOn;
                    tempTo.UpdatedBy = childList[i].CreatedBy;
                    tempTo.CodeTypeId = ProdClassificationTO.CodeTypeId;                        //Priyanka [21-05-18]
                    tempTo.IsActive = ProdClassificationTO.IsActive;
                    SetProductClassificationDisplayName(tempTo, allProdClassificationList);
                    result = UpdateTblProdClassification(tempTo, conn, tran);

                    idClassStr += tempTo.IdProdClass + ",";
                    if (result >= 0)
                    {
                        result = UpdateDisplayName(allProdClassificationList, tempTo, ref idClassStr, conn, tran);
                    }
                    else
                        return -1;
                }
                if (idClassStr != String.Empty)
                {
                    idClassStr = idClassStr.TrimEnd(',');
                    result = BL.TblProductItemBL.UpdateTblProductItemTaxType(idClassStr, ProdClassificationTO.CodeTypeId, ProdClassificationTO.IsActive,conn, tran);
                }
            }
            return result;
        }


        #endregion

        #region Updation
        public static int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return TblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO);
        }

        public static int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        /// <summary>
        /// Priyanka [23-02-2018] Added for updating productclassificaiton and its Displayname where its refrences.
        /// </summary>
        /// <param name="tblProdClassificationTO"></param>
        /// <returns></returns>

        public static int UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                conn.Open();
                tran = conn.BeginTransaction();
                result = TblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
                if (result > 0)
                {
                    allProdClassificationList = SelectAllTblProdClassificationList(conn, tran, "");
                    String updatedIds = String.Empty;
                    result = UpdateDisplayName(allProdClassificationList, tblProdClassificationTO, ref updatedIds, conn, tran);
                    if (result >= 0)
                    {
                        result = 1;
                        tran.Commit();
                    }
                    else
                    {
                        return -1;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region Deletion
        public static int DeleteTblProdClassification(Int32 idProdClass)
        {
            return TblProdClassificationDAO.DeleteTblProdClassification(idProdClass);
        }

        public static int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            return TblProdClassificationDAO.DeleteTblProdClassification(idProdClass, conn, tran);
        }

        #endregion

    }
}
