using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseSchTcDtlsBL : ITblPurchaseSchTcDtlsBL
    {
        private readonly ITblPurchaseSchTcDtlsDAO _iTblPurchaseSchTcDtlsDAO;
        private readonly IDimPurchaseTcTypeElementBL _iDimPurchaseTcTypeElementBL;

        public TblPurchaseSchTcDtlsBL(ITblPurchaseSchTcDtlsDAO iTblPurchaseSchTcDtlsDAO, IDimPurchaseTcTypeElementBL iDimPurchaseTcTypeElementBL)
        {
            _iTblPurchaseSchTcDtlsDAO = iTblPurchaseSchTcDtlsDAO;
            _iDimPurchaseTcTypeElementBL = iDimPurchaseTcTypeElementBL;
        }

        #region Selection
        public List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtls()
        {
            return _iTblPurchaseSchTcDtlsDAO.SelectAllTblPurchaseSchTcDtls();
        }

        public List<TblPurchaseSchTcDtlsTO> SelectAllTblPurchaseSchTcDtlsList()
        {
            return _iTblPurchaseSchTcDtlsDAO.SelectAllTblPurchaseSchTcDtls();
        }

        public TblPurchaseSchTcDtlsTO SelectTblPurchaseSchTcDtlsTO(Int32 idPurchasseSchTcDtls)
        {
            List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = _iTblPurchaseSchTcDtlsDAO.SelectTblPurchaseSchTcDtls(idPurchasseSchTcDtls);
            if(tblPurchaseSchTcDtlsTOList != null && tblPurchaseSchTcDtlsTOList.Count == 1)
                return tblPurchaseSchTcDtlsTOList[0];
            else
                return null;
        }
        public List<TblPurchaseSchTcDtlsTO> SelectScheTcDtlsByRootScheduleId(String rootScheduleIds)
        {
            List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = _iTblPurchaseSchTcDtlsDAO.SelectScheTcDtlsByRootScheduleId(rootScheduleIds);
            return tblPurchaseSchTcDtlsTOList;
        }

        public List<TblPurchaseSchTcDtlsTO> SelectAllScheTcDtls(String rootScheduleId)
        {
            List<TblPurchaseSchTcDtlsTO> tblPurchaseSchTcDtlsTOList = new List<TblPurchaseSchTcDtlsTO>();
            List<TblPurchaseSchTcDtlsTO> existingSchTcDtlsList = new List<TblPurchaseSchTcDtlsTO>();

            if (!String.IsNullOrEmpty(rootScheduleId))
            {
                existingSchTcDtlsList = SelectScheTcDtlsByRootScheduleId(rootScheduleId);
            }

            List<DimPurchaseTcTypeElementTO> elementList = new List<DimPurchaseTcTypeElementTO>();
            elementList = _iDimPurchaseTcTypeElementBL.SelectAllDimPurchaseTcTypeElement();
            if (elementList != null && elementList.Count > 0)
            {
                for (int i = 0; i < elementList.Count; i++)
                {
                    DimPurchaseTcTypeElementTO elementTO = elementList[i];

                    TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO = new TblPurchaseSchTcDtlsTO();
                    tblPurchaseSchTcDtlsTO.TcTypeId = elementTO.TcTypeId;
                    tblPurchaseSchTcDtlsTO.TcTypeName= elementTO.TcTypeName;
                    tblPurchaseSchTcDtlsTO.TcElementId = elementTO.TcElementId;
                    tblPurchaseSchTcDtlsTO.TcElementName = elementTO.TcElementName;

                    if (existingSchTcDtlsList != null && existingSchTcDtlsList.Count > 0)
                    {
                        TblPurchaseSchTcDtlsTO existingTO = existingSchTcDtlsList.Where(a => a.TcTypeId == elementTO.TcTypeId && a.TcElementId == elementTO.TcElementId).FirstOrDefault();
                        if (existingTO != null)
                        {
                            tblPurchaseSchTcDtlsTO.TcEleValue = existingTO.TcEleValue;
                        }
                    }

                    tblPurchaseSchTcDtlsTOList.Add(tblPurchaseSchTcDtlsTO);

                }
            }

            return tblPurchaseSchTcDtlsTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO)
        {
            return _iTblPurchaseSchTcDtlsDAO.InsertTblPurchaseSchTcDtls(tblPurchaseSchTcDtlsTO);
        }

        public int InsertTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchTcDtlsDAO.InsertTblPurchaseSchTcDtls(tblPurchaseSchTcDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO)
        {
            return _iTblPurchaseSchTcDtlsDAO.UpdateTblPurchaseSchTcDtls(tblPurchaseSchTcDtlsTO);
        }

        public int UpdateTblPurchaseSchTcDtls(TblPurchaseSchTcDtlsTO tblPurchaseSchTcDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchTcDtlsDAO.UpdateTblPurchaseSchTcDtls(tblPurchaseSchTcDtlsTO, conn, tran);
        }
        public int UpdateIsActiveAgainstSch(Int32 rootScheduleId, Int32 isActive, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchTcDtlsDAO.UpdateIsActiveAgainstSch(rootScheduleId, isActive, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls)
        {
            return _iTblPurchaseSchTcDtlsDAO.DeleteTblPurchaseSchTcDtls(idPurchasseSchTcDtls);
        }

        public int DeleteTblPurchaseSchTcDtls(Int32 idPurchasseSchTcDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPurchaseSchTcDtlsDAO.DeleteTblPurchaseSchTcDtls(idPurchasseSchTcDtls, conn, tran);
        }

        #endregion
        
    }
}
