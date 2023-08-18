using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseManagerSupplierBL : ITblPurchaseManagerSupplierBL
    {
        
        #region Selection
        private readonly ITblPurchaseManagerSupplierDAO _iTblPurchaseManagerSupplierDAO;
        public TblPurchaseManagerSupplierBL(ITblPurchaseManagerSupplierDAO iTblPurchaseManagerSupplierDAO)
        {
            _iTblPurchaseManagerSupplierDAO = iTblPurchaseManagerSupplierDAO;
        }

        public  List<DropDownTO> SelectPurchaseFromRoleForDropDown()
        {
            return _iTblPurchaseManagerSupplierDAO.SelectPurchaseFromRoleForDropDown();

        }
          public  List<DropDownTO> SelectPurchaseFromRoleForDropDown(SqlConnection conn,SqlTransaction tran)
        {
            return _iTblPurchaseManagerSupplierDAO.SelectPurchaseFromRoleForDropDown(conn,tran);

        }

        public  List<DropDownTO> GetSupplierByPMDropDownList(string userId)
        {
            return _iTblPurchaseManagerSupplierDAO.GetSupplierByPMDropDownList(userId);

        }
         public  List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId)
        {
            return _iTblPurchaseManagerSupplierDAO.GetPurchaseManagerListOfSupplierForDropDown(supplierId);

        }

         public  List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblPurchaseManagerSupplierDAO.GetPurchaseManagerListOfSupplierForDropDown(supplierId,conn,tran);

        }
        public  Int32 GetSupplierStateId(int supplierID)
        {
            return _iTblPurchaseManagerSupplierDAO.GetSupplierStateId(supplierID);

        }

       
        /// <summary>
        /// swati pisal [2018-02-08]
        /// To get list of supplier
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public  List<TblPurchaseManagerSupplierTO> SelectAllActivePurchaseManagerSupplierList(Int32 userId)
        {
            List<TblPurchaseManagerSupplierTO> pmSupplierTOList = new List<TblPurchaseManagerSupplierTO>();
            List<TblPurchaseManagerSupplierTO> list = _iTblPurchaseManagerSupplierDAO.SelectAllPurchaseManagerSupplier();
            if (list != null)
            {
                Dictionary<int, int> assignSupplierDict = _iTblPurchaseManagerSupplierDAO.SelectPurchaseManagerWithSupplierDCT(userId);

                Dictionary<int, int> pmSupplierDCT = SinkUpDictionaryAndList(ref assignSupplierDict, list);
                for (int i = 0; i < list.Count; i++)
                {
                    TblPurchaseManagerSupplierTO supplierTO = new TblPurchaseManagerSupplierTO();
                    if (pmSupplierDCT != null && pmSupplierDCT.ContainsKey(list[i].OrganizationId))
                    {
                        supplierTO.OrganizationId = list[i].OrganizationId;
                    }
                    else
                        supplierTO.OrganizationId = 0;
                    //supplierTO.IdPurchaseManagerSupplier = list[i].IdPurchaseManagerSupplier;
                    //if (pmSupplierDCT != null && pmSupplierDCT.ContainsValue(list[i].UserId))
                    {
                        supplierTO.UserId = pmSupplierDCT[list[i].OrganizationId];
                        //list[i].UserId;
                    }
                    //else
                    //    supplierTO.UserId = 0;

                    supplierTO.SupplierName = list[i].SupplierName;
                    supplierTO.CreatedBy = list[i].CreatedBy;
                    supplierTO.CreatedOn = list[i].CreatedOn;



                    pmSupplierTOList.Add(supplierTO);

                }
            }

            return pmSupplierTOList;
        }

        private  Dictionary<int, int> SinkUpDictionaryAndList(ref Dictionary<int, int> assignSupplierDict, List<TblPurchaseManagerSupplierTO> pmSupplierList)
        {
            if (pmSupplierList != null && pmSupplierList.Count > 0)
            {
                if (assignSupplierDict != null && assignSupplierDict.Count > 0)
                {
                    for (int i = 0; i < pmSupplierList.Count; i++)
                    {
                        //if key present then override else insert
                        if (!assignSupplierDict.ContainsKey(pmSupplierList[i].OrganizationId))
                        {
                            assignSupplierDict.Add(pmSupplierList[i].OrganizationId, pmSupplierList[i].UserId);
                        }
                    }
                }
                else // create new dictionary and add all user entitlement
                {
                    assignSupplierDict = new Dictionary<int, int>();
                    for (int i = 0; i < pmSupplierList.Count; i++)
                    {
                        //if key not present then insert else override
                        if (!assignSupplierDict.ContainsKey(pmSupplierList[i].OrganizationId))
                        {
                            assignSupplierDict.Add(pmSupplierList[i].OrganizationId, 0);
                        }

                    }
                }
            }
            return assignSupplierDict;
        }

        #endregion

        #region Insertion
        public  int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO)
        {
            return _iTblPurchaseManagerSupplierDAO.InsertUpdateTblPurchaseManagerSupplier(tblPurchaseManagerSupplierTO);
        }

        #endregion

    }
}
