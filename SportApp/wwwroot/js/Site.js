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


$(".dropdown dt a").on('click', function () {
    $(".dropdown dd ul").slideToggle('fast');
});

$(".dropdown dd ul li a").on('click', function () {
    $(".dropdown dd ul").hide();
});

function getSelectedValue(id) {
    return $("#" + id).find("dt a span.value").html();
}

$(document).bind('click', function (e) {
    var $clicked = $(e.target);
    if (!$clicked.parents().hasClass("dropdown")) $(".dropdown dd ul").hide();
});

$('.mutliSelect input[type="checkbox"]').on('click', function () {

    var title = $(this).closest('.mutliSelect').find('input[type="checkbox"]').val(),
        title = $(this).val() + ",";

    if ($(this).is(':checked')) {
        var html = '<span title="' + title + '">' + title + '</span>';
        $('.multiSel').append(html);
        $(".hida").hide();
    } else {
        $('span[title="' + title + '"]').remove();
        var ret = $(".hida");
        $('.dropdown dt a').append(ret);

    }
});



$(document).ready(function () {
    $("#Facilities").select2();
    //select2Dropdown('make-hdn', 'Facilities', 'Search for facility(s)', 'search', 'get', true);
});

$("#input-id").rating({ showClear: false, showCaption: false });
/* 
function select2Dropdown(hiddenID, valueID, ph, listAction, getAction, isMultiple) {
    var sid = '#' + hiddenID;
    $(sid).select2({
        placeholder: ph,
        minimumInputLength: 2,
        allowClear: true,
        multiple: isMultiple,
        ajax: {
            url: "/api/facilities/" + listAction,
            dataType: 'json',
            data: function (term, page) {
                return {
                    id: term // search term
                };
            },
            results: function (data) {
                return { results: data };
            }
        },
        initSelection: function (element, callback) {
            // the input tag has a value attribute preloaded that points to a preselected make's id
            // this function resolves that id attribute to an object that select2 can render
            // using its formatResult renderer - that way the make text is shown preselected
            var id = $('#' + valueID).val();
            if (id !== null && id.length > 0) {
                $.ajax("/api/facilities/" + getAction + "?" + id, {
                    dataType: "json"
                }).done(function (data) { callback(data); });
            }
        },
        formatResult: s2FormatResult,
        formatSelection: s2FormatSelection
    });
 
    $(document.body).on("change", sid, function (ev) {
        var choice;
        var values = ev.val;
        // This is assuming the value will be an array of strings.
        // Convert to a comma-delimited string to set the value.
        if (values !== null && values.length > 0) {
            for (var i = 0; i < values.length; i++) {
                if (typeof choice !== 'undefined') {
                    choice += ",";
                    choice += values[i];
                }
                else {
                    choice = values[i];
                }
            }
        }
 
        // Set the value so that MVC will load the form values in the postback.
        $('#' + valueID).val(choice);
    });
}
*/
 
function s2FormatResult(item) {
    return item.text;
}
 
function s2FormatSelection(item) {
    return item.text;
}

function postgyms(id) {

    //alert(id);
    $.post("/api/users-gyms/add/" + id, function () {
        //alert("send");

    })
        .done(function () {
            //alert("second success");
            $("#addbtn").hide();
            $("#removebtn").show();

        })
        .fail(function () {
            alert("error");
        })
    
}

function deletegyms(id) {

    //alert(id);
    $.post("/api/users-gyms/delete/" + id, function () {
        //alert("send");
    })
        .done(function () {
            //alert("second success");
            $("#removebtn").hide();
            $("#addbtn").show();
        })
        .fail(function () {
            alert("error");
        })

}
