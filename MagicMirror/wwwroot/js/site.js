// CLOCK
function updateClock() {
    var date = new Date(),
        locale = "en-us";
    var currentHours = date.getHours();
    var currentMinutes = date.getMinutes();
    var currentSeconds = date.getSeconds();
    var currentDay = date.getDay();
    var currentDate = date.getDate();
    var currentMonth = date.toLocaleString(locale, { month: "short" });
    var currentYear = date.getFullYear();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10) ? ("0" + currentMinutes) : currentMinutes;
    currentSeconds = (currentSeconds < 10) ? ("0" + currentSeconds) : currentSeconds;

    // Convert an hours component of "0" to "12"
    //currentHours = (currentHours == 0) ? 12 : currentHours;

    // Convert day number to day name
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds;
    var currentDateString = weekday[currentDay] + ", " + currentDate + " " + currentMonth + " " + currentYear;

    $("#time").html(currentTimeString);
    $("#date").html(currentDateString);

}

$(document).ready(function () {
    setInterval('updateClock()', 1000);
}
);


// WEATHER AND DOORS FROM MQTT HUB
$(function () {

    let hubUrl = 'http://localhost:65510/signalRHub';
    let httpConnection = new signalR.HttpConnection(hubUrl);
    let hubConnection = new signalR.HubConnection(httpConnection);

    let weatherDiv = $('#weather-section');
    hubConnection.on('OnWeatherUpdate',
        data => {

            weatherDiv.find("#" + data.entity_id).text(data.state);
            console.log(data.entity_id);

            //if (data.message != null) {
            //    weatherDiv.find("#" + data.entity_id + "_icon").html(data.message);
            //console.log(data.entity_id + "picture!");
            //}
        });
    hubConnection.on('OnEntityUpdate',
        data => {
            let hassDiv = $('#hassTest');

            hassDiv.text(data.entity_id + ": " + data.state);
            console.log(data.entity_id);

            //if (data.message != null) {
            //    weatherDiv.find("#" + data.entity_id + "_icon").html(data.message);
            //console.log(data.entity_id + "picture!");
            //}
        });
    let doorsDiv = $('#doors');
    hubConnection.on('OnDoorUpdate',
        data => {
            if ($('#doors>p.' + data.entity_id).length !== 0) {
                $("." + data.entity_id).text(data.entity_id + ": " + data.state);
                console.log("if-state");
            } else {
                doorsDiv.prepend($('<p>').addClass(data.entity_id).text(data.entity_id + ": " + data.state));
                console.log("else-state");
            }
            if (data.state === "Open" || data.state === "Unknown") {
                $('#doors>p.' + data.entity_id).addClass("text-danger").removeClass("text-success");
            } else {
                $('#doors>p.' + data.entity_id).addClass("text-success").removeClass("text-danger");
            }
        });

    hubConnection.on('setTransports',
        function (data) {
            if (data.length > 0) {
                $("#firstTransport").html(data[0].groupOfLine + " to " + data[0].destination + " " + data[0].displayTime);
                $("#secondTransport").html(data[1].groupOfLine + " to " + data[1].destination + " " + data[1].displayTime);
                $("#thirdTransport").html(data[2].groupOfLine + " to " + data[2].destination + " " + data[2].displayTime);
            }
        });

    //hubConnection.on('setTransports',
    //    function(data) {
    //        //Set forecast in divs and spans
    //    });

    $("#getTraffic").click(function() {
        hubConnection.invoke("GetTransports");
    });
    $("#getWeather").click(function () {
        hubConnection.invoke("GetForecast");
    });
    //setInterval(function () {
    //    hubConnection.invoke("GetTransports");
    //    console.log("intervallen triggad sl-api");
    //},
    //    45000);
    hubConnection.start();
});

// Upcoming week days for weather forecast
$(function () {
    var date = new Date(),
        locale = "en-us";
    var currentDay = date.getDay();

    var weekday = new Array(7);
    weekday[1] = "Mon";
    weekday[2] = "Tue";
    weekday[3] = "Wed";
    weekday[4] = "Thu";
    weekday[5] = "Fri";
    weekday[6] = "Sat";
    weekday[7] = "Sun";
    weekday[8] = "Mon";
    weekday[9] = "Tue";
    weekday[10] = "Wed";
    weekday[11] = "Thu";
    weekday[12] = "Fri";
    weekday[13] = "Sat";
    weekday[14] = "Sun";

    for (var i = 1; i <= 7; i++) {
        $("#day_" + i).text(weekday[currentDay + i]);
    }
});


////API TEST
//    $(document).ready(function () {
//        $.ajax({
//            type: "GET",
//            url: "/api/LitterBox",
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (data) {
//                //alert(JSON.stringify(data));                  
//                console.log(data.state);
//                $("#DIV").html(data.state);
//                console.log(data);
//            }, //End of AJAX Success function  

//            failure: function (data) {
//                alert("fail "+data.responseText);
//            }, //End of AJAX failure function  
//            error: function (data) {
//                alert("error " + data.responseText);
//            } //End of AJAX error function  

//        });         
//    });


//$(function() {

////Anropa hub-metod från js:
////var hubProxy = $.connection.hub.createHubProxy("signalRHub");
//    let hubUrl = 'http://localhost:65510/signalRHub';
//    let httpConnection = new signalR.HttpConnection(hubUrl);
//    let hubConnection = new signalR.HubConnection(httpConnection);

//    hubConnection.on('setMetro',
//        function(data) {

//            $("#firstSubWay").html(data[0].Destination + " " + data[0].DisplayTime);
//            $("#secondSubway").html(data[1].Destination + " " + data[1].DisplayTime);
//            $("#thirdSubway").html(data[2].Destination + " " + data[2].DisplayTime);

//            console.log("SL-api!");
//        });

////$.connection.hub.start().done(function () {
//    hubConnection.start(function() {
//        setInterval(function() {
//                hubConnection.server.getMetro();
//            },
//            45000);

//    });
//});