// CLOCK
function updateClock() {
    var date = new Date(),
        locale = "en-us";
    var currentHours = date.getHours();
    var currentMinutes = date.getMinutes();
    var currentSeconds = date.getSeconds();
    var currentDate = date.getDate();
    var currentMonth = date.toLocaleString(locale, { month: "long" });
    var currentYear = date.getFullYear();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10) ? ("0" + currentMinutes) : currentMinutes;
    currentSeconds = (currentSeconds < 10) ? ("0" + currentSeconds) : currentSeconds;

    // Convert an hours component of "0" to "12"
    //currentHours = (currentHours == 0) ? 12 : currentHours;

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes + ":" + currentSeconds;
    var currentDateString = currentDate + " " + currentMonth + " " + currentYear;

    $("#time").html(currentTimeString);
    $("#date").html(currentDateString);

}

$(document).ready(function () {
        setInterval('updateClock()', 1000);
    }
);