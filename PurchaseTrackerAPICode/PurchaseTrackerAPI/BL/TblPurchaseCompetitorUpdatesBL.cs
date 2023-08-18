using PurchaseTrackerAPI.BL.Interfaces;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PurchaseTrackerAPI.BL
{
    public class TblPurchaseCompetitorUpdatesBL : ITblPurchaseCompetitorUpdatesBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly Itblpurchasecompetitorupdatesdao _itblpurchasecompetitorupdatesdao;
        public TblPurchaseCompetitorUpdatesBL(Itblpurchasecompetitorupdatesdao itblpurchasecompetitorupdatesdao, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _itblpurchasecompetitorupdatesdao = itblpurchasecompetitorupdatesdao;
        }
        #region Selection


        public  List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            return _itblpurchasecompetitorupdatesdao.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
        }

        public  List<DropDownTO> SelectCompetitorMaterialGradeDropDownList(Int32 MaterialId,Int32 competitorOrgId)
        {
            return _itblpurchasecompetitorupdatesdao.SelectCompetitorMaterialGradeDropDownList(MaterialId,competitorOrgId);
        }
        

        public  TblPurchaseCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return _itblpurchasecompetitorupdatesdao.SelectLastPriceForCompetitorAndBrand(brandId);
        }

        /*
        public  List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList()
        {
            return TblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates();

        }

        public  List<TblCompetitorUpdatesTO> SelectAllTblCompetitorUpdatesList(Int32 competitorId, Int32 enteredBy, DateTime fromDate, DateTime toDate)
        {
            return TblCompetitorUpdatesDAO.SelectAllTblCompetitorUpdates(competitorId, enteredBy, fromDate, toDate);

        }

        public  TblCompetitorUpdatesTO SelectTblCompetitorUpdatesTO(Int32 idCompeUpdate)
        {
            return TblCompetitorUpdatesDAO.SelectTblCompetitorUpdates(idCompeUpdate);

        }

        public  List<DropDownTO> SelectCompeUpdateUserDropDown()
        {
            return TblCompetitorUpdatesDAO.SelectCompeUpdateUserDropDown();

        }

        public  TblCompetitorUpdatesTO SelectLastPriceForCompetitorAndBrand(Int32 brandId)
        {
            return TblCompetitorUpdatesDAO.SelectLastPriceForCompetitorAndBrand(brandId);
        }
        */
        #endregion

        #region Insertion

        public  int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            return _itblpurchasecompetitorupdatesdao.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO);
        }

        public  int InsertTblCompetitorUpdates(TblPurchaseCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _itblpurchasecompetitorupdatesdao.InsertTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }

        public ResultMessage SaveMarketUpdate(List<TblPurchaseCompetitorUpdatesTO> competitorUpdatesTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                if (competitorUpdatesTOList == null || competitorUpdatesTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "competitorUpdatesTOList Found Null : SaveMarketUpdate";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                for (int i = 0; i < competitorUpdatesTOList.Count; i++)
                {

                    //if (competitorUpdatesTOList[i].OtherSourceId == 0 && competitorUpdatesTOList[i].DealerId == 0)
                    //{
                    //    TblOtherSourceTO otherSourceTO = new TblOtherSourceTO();
                    //    otherSourceTO.OtherDesc = competitorUpdatesTOList[i].OtherSourceOtherDesc;
                    //    otherSourceTO.CreatedBy = competitorUpdatesTOList[i].CreatedBy;
                    //    otherSourceTO.CreatedOn = competitorUpdatesTOList[i].CreatedOn;

                    //    result = BL.TblOtherSourceBL.InsertTblOtherSource(otherSourceTO, conn, tran);//Need to check...

                    //    if (result != 1)
                    //    {
                    //        tran.Rollback();
                    //        resultMessage.Text = "Error While InsertTblOtherSource : SaveMarketUpdate";
                    //        resultMessage.MessageType = ResultMessageE.Error;
                    //        resultMessage.Result = 0;
                    //        return resultMessage;
                    //    }

                    //    competitorUpdatesTOList[i].OtherSourceId = otherSourceTO.IdOtherSource;
                    //}

                    result = InsertTblCompetitorUpdates(competitorUpdatesTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While InsertTblCompetitorUpdates : SaveMarketUpdate";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveMarketUpdate";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        
        #endregion

        #region Updation
        /*
        public  int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO)
        {
            return TblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO);
        }

        public  int UpdateTblCompetitorUpdates(TblCompetitorUpdatesTO tblCompetitorUpdatesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorUpdatesDAO.UpdateTblCompetitorUpdates(tblCompetitorUpdatesTO, conn, tran);
        }
        */
        #endregion

        #region Deletion
        /*
        public  int DeleteTblCompetitorUpdates(Int32 idCompeUpdate)
        {
            return TblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate);
        }

        public  int DeleteTblCompetitorUpdates(Int32 idCompeUpdate, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorUpdatesDAO.DeleteTblCompetitorUpdates(idCompeUpdate, conn, tran);
        }
        */
        #endregion

    }
}
