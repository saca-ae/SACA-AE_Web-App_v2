loadAlerts();

$(document).ready(function () {
    var entidadid = "#" + getCookie("Entidad");
    if (entidadid == "#") { Load('TEC'); }
    $(entidadid).addClass('active');
});

function setEntidad(entidad) {
    Load(entidad);
    location.reload();
}


function Load(entidad) {
    setCookie("Entidad", entidad, "/");
}


function setCookie(name, value, path) {
    var curCookie = name + "=" + value +
        ((path) ? "; path=" + path : "");
    document.cookie = curCookie;
}

function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim();
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}

function loadAlerts() {
    var lastCheckedTime = getCookie("alertTime");
    var now = new Date();
    if (lastCheckedTime != "") {
        var lastDate = new Date(lastCheckedTime);
        if (lastDate.getDay() < now.getDay()) {
            setAlert(now);
        }
    }
    else {
        setAlert(now);
    }
}

function setAlert(date) {
    setCookie("alertTime", date.toDateString(), "/");
    var route = "/Alerts/Expired";
    $.getJSON(route, function (data) {
        setCookie("alerts", data, "/");
        location.reload();
    });
}