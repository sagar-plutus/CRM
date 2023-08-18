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
    public class TblCompetitorExtBL
    {
        #region Selection

        public static List<TblCompetitorExtTO> SelectAllTblCompetitorExtList()
        {
            return TblCompetitorExtDAO.SelectAllTblCompetitorExt();

        }

        public static TblCompetitorExtTO SelectTblCompetitorExtTO(Int32 idCompetitorExt)
        {
            return TblCompetitorExtDAO.SelectTblCompetitorExt(idCompetitorExt);
        }

        public static List<DropDownTO> SelectCompetitorBrandNamesDropDownList(Int32 competitorOrgId)
        {
            return TblCompetitorExtDAO.SelectCompetitorBrandNamesDropDownList(competitorOrgId);
        }

        public static List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectAllTblCompetitorExtList(orgId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<TblCompetitorExtTO> SelectAllTblCompetitorExtList(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.SelectAllTblCompetitorExt(orgId, conn, tran);

        }

        //Sudhir[09-APR-2018] Added fot GetAllCompetitorList
        public static List<DropDownTO> SelectAllCompetitorDropDownList()
        {
            try
            {

                List<TblOrganizationTO> competitorOrganizationList = BL.TblOrganizationBL.SelectAllTblOrganizationList(Constants.OrgTypeE.COMPETITOR);
                List<DropDownTO> emptyCompetitorList = new List<DropDownTO>();
                if (competitorOrganizationList != null && competitorOrganizationList.Count > 0)
                {
                    for (int i = 0; i < competitorOrganizationList.Count; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = competitorOrganizationList[i].FirmName;
                        dropDownTO.Value = competitorOrganizationList[i].IdOrganization;
                        List<TblCompetitorExtTO> competitorList = SelectAllTblCompetitorExtList(competitorOrganizationList[i].IdOrganization);
                        List<DropDownTO> emptyBrandList = new List<DropDownTO>();
                        foreach (TblCompetitorExtTO item in competitorList)
                        {
                            DropDownTO brandTo = new DropDownTO();
                            brandTo.Text = item.BrandName;
                            brandTo.Value = item.IdCompetitorExt;
                            brandTo.Tag = competitorOrganizationList[i].IdOrganization;
                            emptyBrandList.Add(brandTo);
                        }
                        dropDownTO.Tag = emptyBrandList;
                        emptyCompetitorList.Add(dropDownTO);
                    }
                    return emptyCompetitorList.OrderBy(x => x.Value).ToList(); ;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Sudhir[17-APR-2018] Added for getCompetitorList Based on BrandId.
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static List<DropDownTO> SelectCompetitorListOnBrandId(Int32 brandId)
        {
            return TblCompetitorExtDAO.SelectCompetitorListOnBrandId(brandId);
        }


            #endregion

            #region Insertion
            public static int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return TblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO);
        }

        public static int InsertTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.InsertTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Updation
        public static int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO)
        {
            return TblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO);
        }

        public static int UpdateTblCompetitorExt(TblCompetitorExtTO tblCompetitorExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.UpdateTblCompetitorExt(tblCompetitorExtTO, conn, tran);
        }

        #endregion

        #region Deletion
        public static int DeleteTblCompetitorExt(Int32 idCompetitorExt)
        {
            return TblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt);
        }

        public static int DeleteTblCompetitorExt(Int32 idCompetitorExt, SqlConnection conn, SqlTransaction tran)
        {
            return TblCompetitorExtDAO.DeleteTblCompetitorExt(idCompetitorExt, conn, tran);
        }

        #endregion

    }
}
