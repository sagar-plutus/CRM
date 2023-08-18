using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class MapMyIndiaResponse
    {

       String houseNumber;
        String houseName;
        String poi;
        String poi_dist;
		String street;
        String street_dist;
        String subSubLocality;
        String subLocality;
        String locality;
        String village;
        String district;
        String subDistrict;
        String city;
        String state; 
		String pincode;
        String lat;
        String lng;
        String area;
        String formatted_address;

        public string HouseNumber { get => houseNumber; set => houseNumber = value; }
        public string HouseName { get => houseName; set => houseName = value; }
        public string Poi { get => poi; set => poi = value; }
        public string Poi_dist { get => poi_dist; set => poi_dist = value; }
        public string Street { get => street; set => street = value; }
        public string Street_dist { get => street_dist; set => street_dist = value; }
        public string SubSubLocality { get => subSubLocality; set => subSubLocality = value; }
        public string SubLocality { get => subLocality; set => subLocality = value; }
        public string Locality { get => locality; set => locality = value; }
        public string Village { get => village; set => village = value; }
        public string District { get => district; set => district = value; }
        public string SubDistrict { get => subDistrict; set => subDistrict = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string Pincode { get => pincode; set => pincode = value; }
        public string Lat { get => lat; set => lat = value; }
        public string Lng { get => lng; set => lng = value; }
        public string Area { get => area; set => area = value; }
        public string Formatted_address { get => formatted_address; set => formatted_address = value; }
    }
}
