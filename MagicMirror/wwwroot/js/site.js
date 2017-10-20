// CLOCK
function updateClock() {
    var date = new Date(),
        locale = "en-us";
    var currentHours = date.getHours();
    var currentMinutes = date.getMinutes();
    var currentSeconds = date.getSeconds();
    var currentDay = date.getDay();
    var currentDate = date.getDate();
    var currentMonth = date.toLocaleString(locale, { month: "long" });
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


// DOORS FROM MQTT HUB
$(function () {

    let hubUrl = 'http://localhost:65510/reportsPublisher';
    let httpConnection = new signalR.HttpConnection(hubUrl);
    let hubConnection = new signalR.HubConnection(httpConnection);

    let doorsDiv = $('#doors');
    hubConnection.on('OnDoorUpdate', data => {
        if ($('#doors>p.' + data.entity_id).length !== 0) {
            $("." + data.entity_id).text(data.entity_id + ": " + data.state);
            console.log("if-state");
        } else {
            doorsDiv.prepend($('<p>').addClass(data.entity_id).text(data.entity_id + ": " + data.state));
            console.log("else-state");
        }
        if (data.state === "Open") {
            $('#doors>p.' + data.entity_id).addClass("text-danger").removeClass("text-success");
        } else {
            $('#doors>p.' + data.entity_id).addClass("text-success").removeClass("text-danger");
        }
    });
    hubConnection.start();
});

// WEATHER FROM MQTT HUB
$(function () {

    let hubUrl = 'http://localhost:65510/reportsPublisher';
    let httpConnection = new signalR.HttpConnection(hubUrl);
    let hubConnection = new signalR.HubConnection(httpConnection);

    let weatherDiv = $('#weather');
    hubConnection.on('OnWeatherUpdate', data => {
        //if (data.entity_id === "") {
            
        //}
        weatherDiv.prepend($('<p>').addClass(data.entity_id).text(data.entity_id + ": " + data.state));
        //if ($('#doors>p.' + data.entity_id).length !== 0) {
        //    $("." + data.entity_id).text(data.entity_id + ": " + data.state);
        //    console.log("if-state");
        //} else {
        //    doorsDiv.prepend($('<p>').addClass(data.entity_id).text(data.entity_id + ": " + data.state));
        //    console.log("else-state");
        //}
        //if (data.state === "Open") {
        //    $('#doors>p.' + data.entity_id).addClass("text-danger").removeClass("text-success");
        //} else {
        //    $('#doors>p.' + data.entity_id).addClass("text-success").removeClass("text-danger");
        //}
    });
    hubConnection.start();
});
// FORCAST FROM MQTT HUB
//$(function () {

//    let hubUrl = 'http://localhost:65510/reportsPublisher';
//    let httpConnection = new signalR.HttpConnection(hubUrl);
//    let hubConnection = new signalR.HubConnection(httpConnection);

//    let doorsDiv = $('#doors');
//    hubConnection.on('OnForcastUpdate', data => {
//        if ($('#doors>p.' + data.entity_id).length !== 0) {
//            $("." + data.entity_id).text(data.entity_id + ": " + data.state);
//            console.log("if-state");
//        } else {
//            doorsDiv.prepend($('<p>').addClass(data.entity_id).text(data.entity_id + ": " + data.state));
//            console.log("else-state");
//        }
//        if (data.state === "Open") {
//            $('#doors>p.' + data.entity_id).addClass("text-danger").removeClass("text-success");
//        } else {
//            $('#doors>p.' + data.entity_id).addClass("text-success").removeClass("text-danger");
//        }
//    });
//    hubConnection.start();
//});


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