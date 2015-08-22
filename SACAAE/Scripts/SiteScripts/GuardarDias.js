$(function () {
    //Funciones que cambia el estado del checkbox y agrega o elimina el horario seleccionado en los combobox
    $("#LunesID").change(function () {
        var isChecked = document.getElementById("LunesID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioLunes").disabled = true;
            document.getElementById("ComboHoraFinLunes").disabled = true;
            AgregarCursos(0);
        }
        else {
            document.getElementById("ComboHoraInicioLunes").disabled = false;
            document.getElementById("ComboHoraFinLunes").disabled = false;
            document.getElementById("ComboHoraInicioLunes").selectedIndex = 0;
            document.getElementById("ComboHoraFinLunes").selectedIndex = 0;
            EliminarCurso(0);
        }
    });

    $("#MartesID").change(function () {
        var isChecked = document.getElementById("MartesID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioMartes").disabled = true;
            document.getElementById("ComboHoraFinMartes").disabled = true;
            AgregarCursos(1);
        }
        else {
            document.getElementById("ComboHoraInicioMartes").disabled = false;
            document.getElementById("ComboHoraFinMartes").disabled = false;
            document.getElementById("ComboHoraInicioMartes").selectedIndex = 0;
            document.getElementById("ComboHoraFinMartes").selectedIndex = 0;
            EliminarCurso(1);
        }
    });

    $("#MiercolesID").change(function () {
        var isChecked = document.getElementById("MiercolesID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioMiercoles").disabled = true;
            document.getElementById("ComboHoraFinMiercoles").disabled = true;
            AgregarCursos(2);
        }
        else {
            document.getElementById("ComboHoraInicioMiercoles").disabled = false;
            document.getElementById("ComboHoraFinMiercoles").disabled = false;
            document.getElementById("ComboHoraInicioMiercoles").selectedIndex = 0;
            document.getElementById("ComboHoraFinMiercoles").selectedIndex = 0;
            EliminarCurso(2);
        }
    });

    $("#JuevesID").change(function () {
        var isChecked = document.getElementById("JuevesID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioJueves").disabled = true;
            document.getElementById("ComboHoraFinJueves").disabled = true;
            AgregarCursos(3);
        }
        else {
            document.getElementById("ComboHoraInicioJueves").disabled = false;
            document.getElementById("ComboHoraFinJueves").disabled = false;
            document.getElementById("ComboHoraInicioJueves").selectedIndex = 0;
            document.getElementById("ComboHoraFinJueves").selectedIndex = 0;
            EliminarCurso(3);
        }
    });

    $("#ViernesID").change(function () {
        var isChecked = document.getElementById("ViernesID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioViernes").disabled = true;
            document.getElementById("ComboHoraFinViernes").disabled = true;
            AgregarCursos(4);
        }
        else {
            document.getElementById("ComboHoraInicioViernes").disabled = false;
            document.getElementById("ComboHoraFinViernes").disabled = false;
            document.getElementById("ComboHoraInicioViernes").selectedIndex = 0;
            document.getElementById("ComboHoraFinViernes").selectedIndex = 0;
            EliminarCurso(4);
        }
    });

    $("#SabadoID").change(function () {
        var isChecked = document.getElementById("SabadoID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioSabado").disabled = true;
            document.getElementById("ComboHoraFinSabado").disabled = true;
            AgregarCursos(5);
        }
        else {
            document.getElementById("ComboHoraInicioSabado").disabled = false;
            document.getElementById("ComboHoraFinSabado").disabled = false;
            document.getElementById("ComboHoraInicioSabado").selectedIndex = 0;
            document.getElementById("ComboHoraFinSabado").selectedIndex = 0;
            EliminarCurso(5);
        }
    });

    $("#DomingoID").change(function () {
        var isChecked = document.getElementById("DomingoID").checked;
        if (isChecked == true) {
            document.getElementById("ComboHoraInicioDomingo").disabled = true;
            document.getElementById("ComboHoraFinDomingo").disabled = true;
            AgregarCursos(6);
        }
        else {
            document.getElementById("ComboHoraInicioDomingo").disabled = false;
            document.getElementById("ComboHoraFinDomingo").disabled = false;
            document.getElementById("ComboHoraInicioDomingo").selectedIndex = 0;
            document.getElementById("ComboHoraFinDomingo").selectedIndex = 0;
            EliminarCurso(6);
        }
    });

});

function Load() {
    //Funcion que carga las cookies iniciales
    setCookie("i", 0, 1);
    setCookie("Cantidad", 0, 1);

}


function AgregarCursos(pDia) {
    //Funcion que agrega los cursos, crea un cookie por curso
    var IdDiaSeleccionado = "";
    var IdHoraInicioSeleccionada = "";
    var IdHoraFinSeleccionada = "";
    var Dia = "";
    switch (pDia) {
        case 0: Dia = "Lunes"; IdDiaSeleccionado = "LunesID"; IdHoraInicioSeleccionada = "ComboHoraInicioLunes"; IdHoraFinSeleccionada = "ComboHoraFinLunes"; break;
        case 1: Dia = "Martes"; IdDiaSeleccionado = "MartesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMartes"; IdHoraFinSeleccionada = "ComboHoraFinMartes"; break;
        case 2: Dia = "Miercoles"; IdDiaSeleccionado = "MiercolesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMiercoles"; IdHoraFinSeleccionada = "ComboHoraFinMiercoles"; break;
        case 3: Dia = "Jueves"; IdDiaSeleccionado = "JuevesID"; IdHoraInicioSeleccionada = "ComboHoraInicioJueves"; IdHoraFinSeleccionada = "ComboHoraFinJueves"; break;
        case 4: Dia = "Viernes"; IdDiaSeleccionado = "ViernesID"; IdHoraInicioSeleccionada = "ComboHoraInicioViernes"; IdHoraFinSeleccionada = "ComboHoraFinViernes"; break;
        case 5: Dia = "Sabado"; IdDiaSeleccionado = "SabadoID"; IdHoraInicioSeleccionada = "ComboHoraInicioSabado"; IdHoraFinSeleccionada = "ComboHoraFinSabado"; break;
        case 6: Dia = "Domingo"; IdDiaSeleccionado = "DomingoID"; IdHoraInicioSeleccionada = "ComboHoraInicioDomingo"; IdHoraFinSeleccionada = "ComboHoraFinDomingo"; break;
        default: break;
    }


    try {


        var HoraInicioTemp = document.getElementById(IdHoraInicioSeleccionada).options[document.getElementById(IdHoraInicioSeleccionada).selectedIndex].value;
        var HoraFinTemp = document.getElementById(IdHoraFinSeleccionada).options[document.getElementById(IdHoraFinSeleccionada).selectedIndex].value;
        var separador = ":";
        var HoraInicio = HoraInicioTemp.replace(separador, '');
        var HoraFin = HoraFinTemp.replace(separador, '');
        var Inicio = parseInt(HoraInicio)
        var Fin = parseInt(HoraFin)
        var i = Inicio
    }
    catch (err) {
        alert("ERROR: No se pudo convertir el dia seleccionado");
        return;
    }
    /*
    if (Inicio == Fin) {
        alert("ERROR: La hora de inicio y fin son iguales");
        return;
    }
    */
    if (Inicio > Fin) {
        alert("ERROR: La hora de inicio es posterior a la de fin");
        return;
    }
    //Actualizo el contador
    var cookie = getCookie("i");//i es el nombre de la cookie, es un contador de cursos pero en toda la sesion no local
    var cantidad = 0;
    if (cookie != "" && cookie != null && !isNaN(cookie)) {
        cantidad = parseInt(cookie);
    }
    cantidad++;
    setCookie("i", cantidad.toString(), 1);
    setCookie("DiaSeleccionadoCookie" + cantidad.toString(), Dia + "|" + HoraInicio + "|" + HoraFin, 1);

}


function EliminarCurso(pDia) {
    //Funcion que elimina el cookie al deshabilitar un checkbox
    var IdDiaSeleccionado = "";
    var IdHoraInicioSeleccionada = "";
    var IdHoraFinSeleccionada = "";
    var Dia = "";
    switch (pDia) {
        case 0: Dia = "Lunes"; IdDiaSeleccionado = "LunesID"; IdHoraInicioSeleccionada = "ComboHoraInicioLunes"; IdHoraFinSeleccionada = "ComboHoraFinLunes"; break;
        case 1: Dia = "Martes"; IdDiaSeleccionado = "MartesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMartes"; IdHoraFinSeleccionada = "ComboHoraFinMartes"; break;
        case 2: Dia = "Miercoles"; IdDiaSeleccionado = "MiercolesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMiercoles"; IdHoraFinSeleccionada = "ComboHoraFinMiercoles"; break;
        case 3: Dia = "Jueves"; IdDiaSeleccionado = "JuevesID"; IdHoraInicioSeleccionada = "ComboHoraInicioJueves"; IdHoraFinSeleccionada = "ComboHoraFinJueves"; break;
        case 4: Dia = "Viernes"; IdDiaSeleccionado = "ViernesID"; IdHoraInicioSeleccionada = "ComboHoraInicioViernes"; IdHoraFinSeleccionada = "ComboHoraFinViernes"; break;
        case 5: Dia = "Sabado"; IdDiaSeleccionado = "SabadoID"; IdHoraInicioSeleccionada = "ComboHoraInicioSabado"; IdHoraFinSeleccionada = "ComboHoraFinSabado"; break;
        case 6: Dia = "Domingo"; IdDiaSeleccionado = "DomingoID"; IdHoraInicioSeleccionada = "ComboHoraInicioDomingo"; IdHoraFinSeleccionada = "ComboHoraFinDomingo"; break;
        default: break;
    }


    try {

        var HoraInicioTemp = document.getElementById(IdHoraInicioSeleccionada).options[document.getElementById(IdHoraInicioSeleccionada).selectedIndex].value;
        var HoraFinTemp = document.getElementById(IdHoraFinSeleccionada).options[document.getElementById(IdHoraFinSeleccionada).selectedIndex].value;
        var separador = ":";
        var HoraInicio = HoraInicioTemp.replace(separador, '');
        var HoraFin = HoraFinTemp.replace(separador, '');
        var Inicio = parseInt(HoraInicio)
        var Fin = parseInt(HoraFin)
        var i = Inicio
    }
    catch (err) {
        alert("ERROR: No se pudo convertir el dia seleccionado");
        return;
    }

    var Cookie = getCookie("i");

    //Busco la cookie que se va a eliminar
    var objetivo;
    var Cantidad = parseInt(Cookie);
    for (var j = 1; j <= Cantidad; j++) {
        var ArrayCookie = getCookie("DiaSeleccionadoCookie" + j.toString()).split("|");

        var DiaCookie = ArrayCookie[0];
        var HoraInicioCookie = ArrayCookie[1];
        var HoraFinCookie = ArrayCookie[2];
        if (HoraInicioCookie.charAt(0) == 0) {
            HoraInicioCookie = HoraInicioCookie.substr(1, 3);
        }
        if (DiaCookie == Dia) {
            objetivo = j;
            //Elimino la cookie
            ArrayCookie[0] = "d";
            setCookie("DiaSeleccionadoCookie" + j.toString(), ArrayCookie.join("|"), 1);
            break;
        }
    }


}

function setCookie(cname, cvalue, exdays) {

    document.cookie = cname + "=" + cvalue;
}

function ActualizarContadores() {
    setCookie("Cantidad", getCookie("i"), 1);
    setCookie("i", "0", 1);
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