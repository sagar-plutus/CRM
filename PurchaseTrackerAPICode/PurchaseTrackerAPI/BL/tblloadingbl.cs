using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using PurchaseTrackerAPI.DAL;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;

namespace PurchaseTrackerAPI.BL
{
    public class TblLoadingBL : Itblloadingbl
    {


        private readonly Itblloadingdao _iTblLoadingDAO;
        public TblLoadingBL(Itblloadingdao iTblLoadingDAO)
        {
            _iTblLoadingDAO = iTblLoadingDAO;
        }
        #region Selection

        public  List<VehicleNumber> SelectAllVehicles()
        {
            return _iTblLoadingDAO.SelectAllVehicles();
        }

        #endregion

    }
}
