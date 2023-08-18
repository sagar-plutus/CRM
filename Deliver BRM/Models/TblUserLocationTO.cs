using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTrackerAPI.Models
{
    public class TblUserLocationTO
    {
        #region Declarations
        Int32 idlocation;
        Int32 userId;
        DateTime curTime;
        String latitude;
        String longitude;
        String distance;
        String estimatedTime;
        String actualTime;
        #endregion

        #region Constructor
        public TblUserLocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 Idlocation
        {
            get { return idlocation; }
            set { idlocation = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public DateTime CurTime
        {
            get { return curTime; }
            set { curTime = value; }
        }
        public String Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public String Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public string Distance { get => distance; set => distance = value; }
        public string EstimatedTime { get => estimatedTime; set => estimatedTime = value; }

        public string ActualTime { get => actualTime; set => actualTime = value; }

        #endregion
    }


    public class nearBymeTo
        {
        String lat;
        String lng;
        String firmName;
        String visitePlace;
        String distance;


        public string Lat { get => lat; set => lat = value; }
        public string Lng { get => lng; set => lng = value; }
        public string FirmName { get => firmName; set => firmName = value; }
        public string VisitePlace { get => visitePlace; set => visitePlace = value; }
        public string Distance { get => distance; set => distance = value; }
    }
}
