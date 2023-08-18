using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using System.Linq;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblProdClassificationBL : ITblProdClassificationBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblProdClassificationDAO _itblProdClassificationDAO;
        public TblProdClassificationBL(ITblProdClassificationDAO itblProdClassificationDAO, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _itblProdClassificationDAO = itblProdClassificationDAO;
        }
        #region Selection

        public  List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "")
        {
            return _itblProdClassificationDAO.SelectAllTblProdClassification(prodClassType);
        }
        public  List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            return _itblProdClassificationDAO.SelectAllTblProdClassification(conn,tran, prodClassType);
        }
        //Prajakta[2018-08-13] Added to get sub Category or specification List
          public  List<TblProdClassificationTO> SelectAllTblProdClassification(string parentProdClassId, string prodClassType = "")
        {
            return _itblProdClassificationDAO.SelectAllTblProdClassification(parentProdClassId, prodClassType);
        }
        public  List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            return _itblProdClassificationDAO.SelectAllProdClassificationForDropDown(parentClassId);

        }
        public  TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass)
        {
            return  _itblProdClassificationDAO.SelectTblProdClassification(idProdClass);
        }
        public  List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            return _itblProdClassificationDAO.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
        }


        #endregion

        #region Product Classification DisplayName
        public  void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList)
        {
            String DisplayName = String.Empty;
            List<TblProdClassificationTO> DisplayNameList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTO != null)
            {
                //List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                GetDisplayName(allProdClassificationList, tblProdClassificationTO.ParentProdClassId, DisplayNameList);
                DisplayNameList=DisplayNameList.OrderBy(x => x.IdProdClass).ToList();
                if(DisplayNameList != null && DisplayNameList.Count > 0)
                {
                    for (int ele = 0; ele < DisplayNameList.Count; ele++)
                    {
                        TblProdClassificationTO tempTo = DisplayNameList[ele];
                        DisplayName += tempTo.ProdClassDesc + "/";
                    }
                }
                else if(DisplayNameList.Count == 0)
                {

                }
                else
                {
                    DisplayName += DisplayNameList[0].ProdClassDesc + "/";
                }
                tblProdClassificationTO.DisplayName = DisplayName + tblProdClassificationTO.ProdClassDesc;
            }
        }
        public  void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList)
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

        //Sudhir[12-Jan-2018] Added for Set the DisplayName of Product Classification.
        public  int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
            SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
            return _itblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public  int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _itblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public  int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion

        //Sudhir[12-Jan-2018] Added for Updating DisplayName Recursively.
        public  int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO,SqlConnection conn,SqlTransaction tran)
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
                    SetProductClassificationDisplayName(tempTo, allProdClassificationList);
                    result= UpdateTblProdClassification(tempTo, conn,tran);
                    if (result >= 0)
                    {
                        result=UpdateDisplayName(allProdClassificationList, tempTo, conn, tran);
                    }
                    else
                        return -1;
                }
            }
            return result;
        }

        #region Updation

        //Sudhir[12-Jan-2018] Added for updating productclassificaiton and its Displayname where its refrences.
        public  int UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                conn.Open();
                tran = conn.BeginTransaction();
                result=_itblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO,conn,tran);
                if(result > 0)
                {
                   allProdClassificationList= SelectAllTblProdClassificationList(conn,tran,"");
                    result = UpdateDisplayName(allProdClassificationList, tblProdClassificationTO, conn, tran);
                    if(result >= 0)
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

        public  int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _itblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO);
        }

        public  int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblProdClassification(Int32 idProdClass)
        {
            return _itblProdClassificationDAO.DeleteTblProdClassification(idProdClass);
        }

        public  int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            return _itblProdClassificationDAO.DeleteTblProdClassification(idProdClass, conn, tran);
        }

        #endregion
        
    }
}
