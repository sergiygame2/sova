// Write your Javascript code.
function initialize() {
    google.maps.visualRefresh = true;
    var Tunisie = new google.maps.LatLng(36.81881, 10.16596);
    
    var map = new google.maps.Map(document.getElementById('map-canvas'), {
        zoom: 8,
        center: Tunisie,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    });

    var data = [
        { "Id": 1, "PlaceName": "Zaghouan", "GeoLong": "36.401081", "GeoLat": "10.16596" },
        { "Id": 2, "PlaceName": "Hammamet ", "GeoLong": "36.4", "GeoLat": "10.616667" },
        { "Id": 3, "PlaceName": "Sousse", "GeoLong": "35.8329809", "GeoLat": "10.63875" },
        { "Id": 4, "PlaceName": "Sfax", "GeoLong": "34.745159", "GeoLat": "10.7613" }
    ];

    $.each(data,
        function(i, item) {
            var marker = new google.maps.Marker({
                'position': new google.maps.LatLng(item.GeoLong, item.GeoLat),
                'map': map,
                'title': item.PlaceName
            });
             
            var infowindow = new google.maps.InfoWindow({
                content: "<div class='infoDiv'><h2>" + item.PlaceName + "</div></div>"
            });

            // finally hook up an "OnClick" listener to the map so it pops up out info-window when the marker-pin is clicked! 
            google.maps.event.addListener(marker,
                'click',
                function() {
                    infowindow.open(map, marker);
                });
        });
} 

