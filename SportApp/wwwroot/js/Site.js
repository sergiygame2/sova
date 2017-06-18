// Write your Javascript code.
class Gym {

    constructor(GymName, GymRate, GymLocation, Region, MbrshipPrice, GymArea, FoundYear, Facilities, Url,
        Description, GymImgUrl,Comments, Longitude, Latitude) {
        this.GymName = GymName;
        this.GymRate = GymRate;
        this.GymLocation = GymLocation;
        this.Region = Region;
        this.MbrshipPrice = MbrshipPrice;
        this.GymArea = GymArea;
        this.FoundYear = FoundYear;
        this.Facilities = Facilities;
        this.Url = Url;
        this.Description = Description;
        this.GymImgUrl = GymImgUrl;
        this.Comments = Comments;
        this.Longitude = Longitude;
        this.Latitude = Latitude;
    }
}




function initialize(gyms) {

    google.maps.visualRefresh = true;
    var Kyiv = new google.maps.LatLng(50.499988, 30.223344);

    var map = new google.maps.Map(document.getElementById('map-canvas'), {
        zoom: 8,
        center: Kyiv,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    });

        //console.log(gyms[0].GymName);
        //console.log(gyms[0].Longitude);
        //console.log(gyms[0].Latitude);
        //console.log(gyms[1].GymName);
        //console.log(gyms[1].Longitude);
        //console.log(gyms[1].Latitude);

    $.each(gyms,
        function (i, item) {
            var marker = new google.maps.Marker({
                'position': new google.maps.LatLng(item.Latitude, item.Longitude),
                'map': map,
                'title': item.GymName
            });

            var infowindow = new google.maps.InfoWindow({
                content: "<div class='infoDiv'><h2>" + item.GymName + "</div></div>"
            });

            // finally hook up an "OnClick" listener to the map so it pops up out info-window when the marker-pin is clicked! 
            google.maps.event.addListener(marker,
                'click',
                function () {
                    infowindow.open(map, marker);
                });
        });
} 


