using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class GeoLocationAddressTo
    {
        public string route { get; set; }
        public string neighborhood { get; set; }
        public string sublocality_level_2 { get; set; }
        public string sublocality_level_1 { get; set; }
        public string locality { get; set; }
        public string administrative_area_level_2 { get; set; }
        public string administrative_area_level_1 { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

        public string formatted_address
        {
            get; set;
            //public string FullAddress { get; set; }

        }
    }
    public class newdata
    {
        String lat;
        String lng;
        Int32 idtblVisitDetails;
        public string Lng { get => lng; set => lng = value; }
        public int IdtblVisitDetails { get => idtblVisitDetails; set => idtblVisitDetails = value; }
        public string Lat { get => lat; set => lat = value; }
    }
}
