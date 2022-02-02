using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ButTerminalWeb.Pages.ButTerminalPages.GoogleApiPrototypes;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace ButTerminalWeb.Pages.ButTerminalPages
{
    public class AdminModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private string _key;

        static HttpClient client = new HttpClient();
        static List<BusRoute> busRoutes = new List<BusRoute>();

        public string GetKey 
        {
            get
            {
                return _key;
            } 
        }
        public AdminModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("US");
            _key = _configuration.GetValue<string>("API_KEY:KEY");
            //Call Google Directions API
            GetDirections("Kolding dk", "Sønderborg dk", GetKey);
            GetDirections("Vojens dk", "Sønderborg dk", GetKey);

        }

        /// <summary>
        /// Call Google Directions <see langword="API"/> and receive a <see cref="DirectionsAPIObject"/> that contains a rough route from Start to Finish
        /// </summary>
        private static async void GetDirections(string _startLocation, string _endLocation, string key)
        {
            DirectionsAPIObject directions = await client.GetFromJsonAsync<DirectionsAPIObject>($"https://maps.googleapis.com/maps/api/directions/json?origin={_startLocation}&destination={_endLocation}&key={key}");
            GetRoadsFromDirections(directions, key);
        }
        /// <summary>
        /// 
        /// </summary>
        private static async void GetRoadsFromDirections(DirectionsAPIObject _directions, string key)
        {
            string pathURL = "";
            foreach (Route route in _directions.routes)
            {
                foreach (Leg leg in route.legs)
                {
                    pathURL += $"{leg.start_location.lat.ToString(CultureInfo.InvariantCulture)}, {leg.start_location.lng.ToString(CultureInfo.InvariantCulture)}";
                    foreach (Step step in leg.steps)
                    {
                        pathURL += $"|{step.start_location.lat.ToString(CultureInfo.InvariantCulture)}, {step.start_location.lng.ToString(CultureInfo.InvariantCulture)}";
                        pathURL += $"|{step.end_location.lat.ToString(CultureInfo.InvariantCulture)}, {step.end_location.lng.ToString(CultureInfo.InvariantCulture)}";
                    }
                    pathURL += $"|{leg.end_location.lat.ToString(CultureInfo.InvariantCulture)}, {leg.end_location.lng.ToString(CultureInfo.InvariantCulture)}";
                }
            }

            RoadsAPIObject newRoad = await client.GetFromJsonAsync<RoadsAPIObject>($"https://roads.googleapis.com/v1/snapToRoads?interpolate=true&path={pathURL}&key={key}");

            int countID = 0;
            byte count = 0;
            int totalCount = 0;
            BusRoute newBusRoute = new BusRoute();
            List<BusRoute.Part> tempParts = new List<BusRoute.Part>();
            List<string> tempCords = new List<string>();
            

            newBusRoute.ID = countID;
            foreach (SnappedPoint snappedPoint in newRoad.snappedPoints)
            {
                if (count < 99)
                {
                    tempCords.Add('{'+$" lat: {snappedPoint.location.latitude.ToString(CultureInfo.InvariantCulture)}, lng: {snappedPoint.location.longitude.ToString(CultureInfo.InvariantCulture)}"+'}');
                    count++;
                }
                else if (totalCount == newRoad.snappedPoints.Count)
                {
                    tempCords.Add('{' + $" lat: {snappedPoint.location.latitude.ToString(CultureInfo.InvariantCulture)}, lng: {snappedPoint.location.longitude.ToString(CultureInfo.InvariantCulture)}" + '}');

                    tempParts.Add(new BusRoute.Part { Cords = tempCords });

                    tempCords = new List<string>();
                    count = 0;
                    countID++;
                }
                else
                {
                    tempCords.Add('{' + $" lat: {snappedPoint.location.latitude.ToString(CultureInfo.InvariantCulture)}, lng: {snappedPoint.location.longitude.ToString(CultureInfo.InvariantCulture)}" + '}');

                    tempParts.Add(new BusRoute.Part { Cords = tempCords });

                    tempCords = new List<string>();
                    count = 0;
                    countID++;
                }
                totalCount++;
            }

            
            newBusRoute.PathPart = tempParts;
            
            busRoutes.Add(newBusRoute);
        }
        /// <summary>
        /// 
        /// </summary>
        private void CreateArrayForJavaScript()
        {

        }
        public class BusRoute
        {
            public int ID { get; set; }
            public List<Part> PathPart { get; set; }
            public class Part
            {
                public List<string> Cords { get; set; }
            }
        }
    }
}
