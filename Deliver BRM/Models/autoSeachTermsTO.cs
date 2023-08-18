using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTrackerAPI.Models
{
    public class autoSeachTermsTO
    {

        List<SuggestedLocTO> suggestedLocations;

        public List<SuggestedLocTO> SuggestedLocations { get => suggestedLocations; set => suggestedLocations = value; }
    }


    public class SuggestedLocTO
    {
        String type;
        Int32 typeX;
        String placeAddress;
        double latitude;
        double longitude;
        String eLoc;
        double entryLatitude;
        double entryLongitude;
        String placeName;
        String alternateName;
        Int32 p;
        Int32 orderIndex;
        double score;
        String suggester;

        public string Type { get => type; set => type = value; }
        public int TypeX { get => typeX; set => typeX = value; }
        public string PlaceAddress { get => placeAddress; set => placeAddress = value; }
        public double Latitude { get => latitude; set => latitude = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public string ELoc { get => eLoc; set => eLoc = value; }
        public double EntryLatitude { get => entryLatitude; set => entryLatitude = value; }
        public double EntryLongitude { get => entryLongitude; set => entryLongitude = value; }
        public string PlaceName { get => placeName; set => placeName = value; }
        public string AlternateName { get => alternateName; set => alternateName = value; }
        public int P { get => p; set => p = value; }
        public int OrderIndex { get => orderIndex; set => orderIndex = value; }
        public double Score { get => score; set => score = value; }
        public string Suggester { get => suggester; set => suggester = value; }
    }


}
