//function updateClock() {
//    var currentTime = new Date();
//    var currentHours = currentTime.getHours();
//    var currentMinutes = currentTime.getMinutes();
//    var currentSeconds = currentTime.getSeconds();
//    var currentDate = currentTime.getDate();

//    // Pad the minutes and seconds with leading zeros, if required
//    currentMinutes = (currentMinutes < 10) ? ("0" + currentMinutes) : currentMinutes;
//    currentSeconds = (currentSeconds < 10) ? ("0" + currentSeconds) : currentSeconds;


//    // Convert an hours component of "0" to "12"
//    currentHours = (currentHours == 0) ? 12 : currentHours;

//    // Compose the string for display
//    var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds + " ";

//    $("#time").html(currentTimeString);


//}

//$(document).ready(function () {
//        setInterval('updateClock()', 1000);
//    }
//);