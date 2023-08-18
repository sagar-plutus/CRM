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
    public class TblContactUsDtlsBL
    {
        TblContactUsDtlsDAO TblContactUsDtlsDAO;
        #region selection
        public TblContactUsDtlsBL()
        {
            this.TblContactUsDtlsDAO = new TblContactUsDtlsDAO();
        }

          // Select contacts on condition - Tejaswini
        public List<IGrouping<int,TblContactUsDtls>> SelectContactUsDtls (int IsActive)
        {
            List<TblContactUsDtls> ContactUsDtlsList = new List<TblContactUsDtls>();

            if (IsActive == 0 || IsActive == 1)
            {
                ContactUsDtlsList = this.TblContactUsDtlsDAO.SelectContactUsDtls(IsActive);   
            }
            else
            {
                ContactUsDtlsList = this.TblContactUsDtlsDAO.SelectAllContactUsDtls();
            }
            List<TblContactUsDtls> ContactUsDtlsListTemp = new List<TblContactUsDtls>();
            var tempData = ContactUsDtlsList.GroupBy(g => g.SupportTypeId).Select(ele=>ele).ToList();
            return tempData;
        }

        #endregion
    }
}