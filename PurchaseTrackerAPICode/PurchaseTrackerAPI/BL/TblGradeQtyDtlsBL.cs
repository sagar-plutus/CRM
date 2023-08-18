using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.StaticStuff;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace BL
{
    public class TblGradeQtyDtlsBL: ITblGradeQtyDtlsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblGradeQtyDtlsDAO _itblGradeQtyDtlsDAO;
        private readonly Icommondao _icommondao;


        public TblGradeQtyDtlsBL(ITblGradeQtyDtlsDAO itblGradeQtyDtlsDAO, IConnectionString iConnectionString,
            Icommondao icommondao)
        {
            _iConnectionString = iConnectionString;
            _itblGradeQtyDtlsDAO = itblGradeQtyDtlsDAO;
            _icommondao = icommondao;
        }

        #region Selection
        public List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls()
        {
            return _itblGradeQtyDtlsDAO.SelectAllTblGradeQtyDtls();
        }

        
        public List<TblGradeQtyDtlsTO> SelectTblGradeQtyDtls(Int32 idGradeQtyDtls)
        {
            return _itblGradeQtyDtlsDAO.SelectTblGradeQtyDtls(idGradeQtyDtls);           
        }

        public List<TblGradeQtyDtlsTO> SelectAllTblGradeQtyDtls(SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeQtyDtlsDAO.SelectAllTblGradeQtyDtls(conn,tran);
        }


        #endregion

        #region Insertion
        public int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO)
        {
            return _itblGradeQtyDtlsDAO.InsertTblGradeQtyDtls(tblGradeQtyDtlsTO);
        }

        public int InsertTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeQtyDtlsDAO.InsertTblGradeQtyDtls(tblGradeQtyDtlsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO)
        {
            return _itblGradeQtyDtlsDAO.UpdateTblGradeQtyDtls(tblGradeQtyDtlsTO);
        }

        public int UpdateTblGradeQtyDtls(TblGradeQtyDtlsTO tblGradeQtyDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeQtyDtlsDAO.UpdateTblGradeQtyDtls(tblGradeQtyDtlsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls)
        {
            return _itblGradeQtyDtlsDAO.DeleteTblGradeQtyDtls(idGradeQtyDtls);
        }

        public int DeleteTblGradeQtyDtls(Int32 idGradeQtyDtls, SqlConnection conn, SqlTransaction tran)
        {
            return _itblGradeQtyDtlsDAO.DeleteTblGradeQtyDtls(idGradeQtyDtls, conn, tran);
        }

        #endregion

        public ResultMessage SavePurchaseGradeQtyDtls(List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            DateTime currentDate = _icommondao.ServerDateTime;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if(tblGradeQtyDtlsTOList == null || tblGradeQtyDtlsTOList.Count == 0)
                {
                    throw new Exception("tblGradeQtyDtlsTOList == null");
                }

                for (int i = 0; i < tblGradeQtyDtlsTOList.Count; i++)
                {
                    TblGradeQtyDtlsTO tblGradeQtyDtlsTO = tblGradeQtyDtlsTOList[i];

                    //check for extisting
                    tblGradeQtyDtlsTO.Date = currentDate;
                    List<TblGradeQtyDtlsTO> exitingList = _itblGradeQtyDtlsDAO.SelectExistingGradeQtyDtls(tblGradeQtyDtlsTO, conn, tran);
                    if(exitingList != null && exitingList.Count > 0)
                    {
                        for (int k = 0; k < exitingList.Count; k++)
                        {
                            exitingList[k].IsActive = 0;
                            result = _itblGradeQtyDtlsDAO.UpdateTblGradeQtyDtls(exitingList[k], conn, tran);
                            if(result <= -1)
                            {
                                throw new Exception("_itblGradeQtyDtlsDAO.UpdateTblGradeQtyDtls(exitingList[k], conn, tran);");
                            }
                        }
                    }

                    tblGradeQtyDtlsTO.IsActive = 1;
                    tblGradeQtyDtlsTO.Date = currentDate;
                    tblGradeQtyDtlsTO.CreatedOn = currentDate;
                    tblGradeQtyDtlsTO.CreatedBy = loginUserId;

                    result = _itblGradeQtyDtlsDAO.InsertTblGradeQtyDtls(tblGradeQtyDtlsTO, conn, tran);
                    if(result != 1)
                    {
                        throw new Exception("_itblGradeQtyDtlsDAO.InsertTblGradeQtyDtls(tblGradeQtyDtlsTO, conn, tran);");
                    }

                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = "Grade Qty Submitted Successfully.";
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in SavePurchaseGradeQtyDtls(List<TblGradeQtyDtlsTO> tblGradeQtyDtlsTOList, Int32 loginUserId)");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<dynamic> SelectGradeQtyDetails(TblReportsTO tblReportsTO)
        {            
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;
            DataTable resultDT = new DataTable();
            dynamic gradeQtyDtlsTOList = new List<dynamic>();

            resultDT = _itblGradeQtyDtlsDAO.SelectGradeQtyDetails(tblReportsTO);

            if (resultDT != null && resultDT.Rows.Count > 0)
            {
                if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
                {
                    TblFilterReportTO fromDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "FromDate").FirstOrDefault();
                    if(fromDateTO != null)
                    {
                        fromDate = Convert.ToDateTime(fromDateTO.OutputValue);
                    }

                    TblFilterReportTO toDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "ToDate").FirstOrDefault();
                    if (fromDateTO != null)
                    {
                        toDate = Convert.ToDateTime(toDateTO.OutputValue);
                    }
                }

                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    int prodClassId = 0,prodItemId=0,supervisorId =0;
                    Double unloadingStock = 0, closingStock = 0, openingStock =0, consumptionStock = 0, averageRate = 0;

                    dynamic gradeQtyDtlsTO = new JObject();

                    DataRow reportDtlsRow = resultDT.Rows[i];

                    DataTable dt = new DataTable();

                    bool isParsable1 = Int32.TryParse(reportDtlsRow["prodClassId"].ToString(), out prodClassId);
                    bool isParsable2 = Int32.TryParse(reportDtlsRow["prodItemId"].ToString(), out prodItemId);
                    bool isParsable3 = Int32.TryParse(reportDtlsRow["supervisorId"].ToString(), out supervisorId);
                    bool isParsabl4 = Double.TryParse(reportDtlsRow["qty"].ToString(), out openingStock);
                    bool isParsable5 = Double.TryParse(reportDtlsRow["qty2"].ToString(), out consumptionStock);

                    dt = _itblGradeQtyDtlsDAO.GetUnloadingQty(fromDate, toDate, prodClassId, prodItemId, supervisorId);

                    if ((dt != null) && (dt.Rows.Count > 0))
                    {                        
                        bool isParsable6 = Double.TryParse(dt.Rows[0]["qty"].ToString(), out unloadingStock);                        
                        bool isParsable7 = Double.TryParse(dt.Rows[0]["averageRate"].ToString(), out averageRate);
                    }

                    closingStock = (((openingStock) + (unloadingStock)) - (consumptionStock));

                    gradeQtyDtlsTO.Date = reportDtlsRow["Date"].ToString();
                    gradeQtyDtlsTO.Material = reportDtlsRow["prodClassDesc"].ToString();
                    gradeQtyDtlsTO.Grade = reportDtlsRow["itemName"].ToString();
                    gradeQtyDtlsTO.Supervisor = reportDtlsRow["userDisplayName"].ToString();
                    gradeQtyDtlsTO["Average Rate"] = averageRate;
                    gradeQtyDtlsTO["Opening Stock"] = openingStock;
                    gradeQtyDtlsTO["Consumption Stock"] = consumptionStock;
                    gradeQtyDtlsTO["Unloading Stock"] = unloadingStock;
                    gradeQtyDtlsTO["Closing Stock"] = Math.Round(closingStock,3);
                    gradeQtyDtlsTO["isTotalRow"] = 0;
                    gradeQtyDtlsTOList.Add(gradeQtyDtlsTO);                    
                }
            }          
             
            return gradeQtyDtlsTOList;           
        }       

    }
}
