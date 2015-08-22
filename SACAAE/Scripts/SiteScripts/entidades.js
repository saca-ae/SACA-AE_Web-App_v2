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

