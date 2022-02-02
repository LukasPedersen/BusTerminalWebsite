// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function initMap ()
{
    const map = new google.maps.Map(document.getElementById("map"), {
        center: { lat: 54.9079961, lng: 9.8053493 },
        zoom: 11,
        mapTypeId: "terrain",
    });
    // Define the symbol, using one of the predefined paths ('CIRCLE')
    // supplied by the Google Maps JavaScript API.
    const lineSymbol = {
        path: google.maps.SymbolPath.CIRCLE,
        scale: 8,
        strokeColor: "#393",
    };
    // Create the polyline and add the symbol to it via the 'icons' property.
    const line = new google.maps.Polyline({
        path: [
            //All Points that should be drawn on google maps
            { lat: 54.91965208247463, lng: 9.757947350198526 },
            { lat: 54.91965208247463, lng: 9.757947350198526 },
            { lat: 54.9195241, lng: 9.7580837 },
            { lat: 54.9194557, lng: 9.7581629 },
            { lat: 54.919418, lng: 9.7582243 },
            { lat: 54.9193799, lng: 9.758293199999999 },
            { lat: 54.91930409999999, lng: 9.7584466 },
            { lat: 54.91924220000001, lng: 9.7585993 },
            { lat: 54.9191866, lng: 9.7586891 },
            { lat: 54.9191528, lng: 9.758740099999999 },
            { lat: 54.9189751, lng: 9.758990599999999 },
            { lat: 54.9189751, lng: 9.758990599999999 },
            { lat: 54.9189751, lng: 9.758990599999999 },
            { lat: 54.9188635, lng: 9.758609199999999 },
            { lat: 54.9188635, lng: 9.758609199999999 },
            { lat: 54.918743000000006, lng: 9.758179 },
            { lat: 54.918743000000006, lng: 9.758179 },
            { lat: 54.91870600000001, lng: 9.758071 },
            { lat: 54.918567499999995, lng: 9.7577621 },
            { lat: 54.918567499999995, lng: 9.7577621 },
        ],
        icons: [
            {
                icon: lineSymbol,
                offset: "100%",
            },
        ],

        map: map,
    });

    animateCircle(line);
}

// Use the DOM setInterval() function to change the offset of the symbol
// at fixed intervals.
function animateCircle (line)
{
    let count = 0;

    window.setInterval(() => {
        count = (count + 1) % 200;

        const icons = line.get("icons");

        icons[0].offset = count / 2 + "%";
        line.set("icons", icons);
    }, 200);
}

function AddPointJS( _number) {
    const input = document.createElement("input");
    input.setAttribute("type", "Search");
    input.setAttribute("id", "Point_" + _number);
    input.setAttribute("placeholder", "From");
    document.getElementById("Points").appendChild(input);
    }