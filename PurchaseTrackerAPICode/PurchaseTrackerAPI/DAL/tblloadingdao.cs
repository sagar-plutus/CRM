using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PurchaseTrackerAPI.Models;
using PurchaseTrackerAPI.StaticStuff;
using System.Linq;
using PurchaseTrackerAPI.DAL.Interfaces;
using PurchaseTrackerAPI.BL.Interfaces;

namespace PurchaseTrackerAPI.DAL
{
    public class TblLoadingDAO : Itblloadingdao 
    {

        private readonly IConnectionString _iConnectionString;
        public TblLoadingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods

        public  List<VehicleNumber> SelectAllVehicles()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT distinct vehicleNo FROM tempLoading";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleNumber> list = new List<VehicleNumber>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            vehicleNo = Convert.ToString(sqlReader["vehicleNo"].ToString());

                        if (!string.IsNullOrEmpty(vehicleNo))
                        {
                            String[] vehNoPart = vehicleNo.Split(' ');
                            if (vehNoPart.Length == 4)
                            {
                                VehicleNumber vehicleNumber = new VehicleNumber();
                                for (int i = 0; i < vehNoPart.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        vehicleNumber.StateCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 1)
                                    {
                                        vehicleNumber.DistrictCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 2)
                                    {
                                        vehicleNumber.UniqueLetters = vehNoPart[i];
                                        if (vehicleNumber.UniqueLetters == "undefined")
                                            vehicleNumber.UniqueLetters = "";
                                        else
                                            vehicleNumber.UniqueLetters = vehicleNumber.UniqueLetters.ToUpper();
                                    }
                                    if (i == 3)
                                    {
                                        if (Constants.IsInteger(vehNoPart[i]))
                                        {
                                            vehicleNumber.VehicleNo = Convert.ToInt32(vehNoPart[i]);
                                        }
                                        else break;
                                    }
                                }

                                if (vehicleNumber.VehicleNo > 0)
                                    list.Add(vehicleNumber);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

    }
}
