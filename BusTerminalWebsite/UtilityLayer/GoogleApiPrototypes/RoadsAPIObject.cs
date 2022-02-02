using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusTerminalWebsite.UtilityLayer.GoogleApiPrototypes
{
    public class RoadsAPIObject
    {
        public List<SnappedPoint> snappedPoints { get; set; }
    }
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
    public class SnappedPoint
    {
        public Location location { get; set; }
        public int originalIndex { get; set; }
        public string placeId { get; set; }
    }
}
