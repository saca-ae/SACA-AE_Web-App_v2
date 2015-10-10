$(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#Course").prop("disabled", "disabled");
    $("#Group").prop("disabled", "disabled");
    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Block").change(function () {
        var plan = getCookie("SelPlanDeEstudio");
        var route = "/CursoProfesor/Cursos/List/" + plan + "/" + $('select[name="Block"]').val();
        setCookie("Grupo", "");
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, curso) {
                items += "<option value= " + curso.ID + ">" + curso.Name + "</option>";
            });


            $("#Course").prop("disabled", false);
            $("#Group").html("");
            $("#Group").prop("disabled", "disabled");
            $("#Course").html(items);
            $("#Course").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");

        });
    });
    $("#Course").change(function () {
        var plan = getCookie("SelPlanDeEstudio");
        var sede = getCookie("SelSede");
        var route = "/CursoProfesor/Grupos/List/" + $('select[name="Course"]').val() + "/" + plan + "/" + sede + "/" + $('select[name="Block"]').val() + "/" + getCookie("PeriodoHorario");
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, grupo) {
                items += "<option value='" + grupo.ID + "'>" + grupo.Number + "</option>";
            });

            $("#Group").prop("disabled", false);
            $("#Group").html(items);
            $("#Group").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");

        });
    });

    $("#Group").change(function () {
        if ($('#Group option:selected').text() != getCookie("Grupo")) {
            setCookie("Grupo", $('#sltGrupo option:selected').text())
            borrarTabla();
            Cargar();
        }
    });

    $("#LunesID").change(function () {
        var isChecked = document.getElementById("LunesID").checked;
        document.getElementById("LunesID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(0);
        }
        else {
            document.getElementById("LunesID").disabled = false;
            document.getElementById("ComboHoraInicioLunes").selectedIndex = 0;
            document.getElementById("ComboHoraFinLunes").selectedIndex = 0;
            EliminarCurso(0);
        }
    });

    $("#MartesID").change(function () {
        var isChecked = document.getElementById("MartesID").checked;
        document.getElementById("MartesID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(1);
        }
        else {
            document.getElementById("MartesID").disabled = false;
            document.getElementById("ComboHoraInicioMartes").selectedIndex = 0;
            document.getElementById("ComboHoraFinMartes").selectedIndex = 0;
            EliminarCurso(1);
        }
    });

    $("#MiercolesID").change(function () {
        var isChecked = document.getElementById("MiercolesID").checked;
        document.getElementById("MiercolesID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(2);
        }
        else {
            document.getElementById("MiercolesID").disabled = false;
            document.getElementById("ComboHoraInicioMiercoles").selectedIndex = 0;
            document.getElementById("ComboHoraFinMiercoles").selectedIndex = 0;
            EliminarCurso(2);
        }
    });

    $("#JuevesID").change(function () {
        var isChecked = document.getElementById("JuevesID").checked;
        document.getElementById("JuevesID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(3);
        }
        else {
            document.getElementById("JuevesID").disabled = false;
            document.getElementById("ComboHoraInicioJueves").selectedIndex = 0;
            document.getElementById("ComboHoraFinJueves").selectedIndex = 0;
            EliminarCurso(3);
        }
    });

    $("#ViernesID").change(function () {
        var isChecked = document.getElementById("ViernesID").checked;
        document.getElementById("ViernesID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(4);
        }
        else {
            document.getElementById("ViernesID").disabled = false;
            document.getElementById("ComboHoraInicioViernes").selectedIndex = 0;
            document.getElementById("ComboHoraFinViernes").selectedIndex = 0;
            EliminarCurso(4);
        }
    });

    $("#SabadoID").change(function () {
        var isChecked = document.getElementById("SabadoID").checked;
        document.getElementById("SabadoID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(5);
        }
        else {
            document.getElementById("SabadoID").disabled = false;
            document.getElementById("ComboHoraInicioSabado").selectedIndex = 0;
            document.getElementById("ComboHoraFinSabado").selectedIndex = 0;
            EliminarCurso(5);
        }
    });

    $("#DomingoID").change(function () {
        var isChecked = document.getElementById("DomingoID").checked;
        document.getElementById("DomingoID").disabled = true;
        if (isChecked == true) {
            AgregarCursos(6);
        }
        else {
            document.getElementById("DomingoID").disabled = false;
            document.getElementById("ComboHoraInicioDomingo").selectedIndex = 0;
            document.getElementById("ComboHoraFinDomingo").selectedIndex = 0;
            EliminarCurso(6);
        }
    });

});

function Load() {
    setCookie("Grupo", "");
}
function Cargar() {
    setCookie("i", 0, 1);
    setCookie("Cantidad", 0, 1);
    var route = "/getHorarios/List/" + getCookie("SelPlanDeEstudio") + "/" + getCookie("PeriodoHorario");
    $.getJSON(route, function (data) {
        $.each(data, function (i, Horario) {
            var Curso = Horario.Nombre;
            var GrupoText = Horario.Numero;
            var BloqueText = Horario.Descripcion;
            var Dia = Horario.Dia1;
            var Aula = Horario.Aula;
            var HoraInicioCookie = Horario.Hora_Inicio;
            var HoraFinCookie = Horario.Hora_Fin;
            if (GrupoText == $('#Group option:selected').text() && BloqueText == $('#Block option:selected').text()) {
                var Inicio = parseInt(HoraInicioCookie);
                var Fin = parseInt(HoraFinCookie);
                var i = Inicio;
                //Actualizo la tabla con el nuevo curso
                var CantCeldas = 0;
                var items = "";
                while (i < Fin) {

                    var IdCelda = Dia + " " + i;
                    if (i < 100) { IdCelda = Dia + " 0" + i; } //repara el string en caso de que la hora fuera 0010 ya que el parse la deja como 10
                    if (i == 0) { IdCelda = Dia + " 000"; }
                    var celda = document.getElementById(IdCelda);
                    if (i == Inicio) {
                        var primera = document.getElementById(IdCelda);
                    }
                    else if (celda != null) {
                        items += celda.innerHTML;
                        celda.parentNode.removeChild(celda);
                    }
                    i += 10;
                    if (i % 100 == 60) { i += 40; }//cuando se llega al minuto 60 se le suman 40 al numero para que pase por ejemplo de 1060 a 1100
                    CantCeldas++;
                }
                primera.innerHTML += items + "<p id=" + Dia + ">" + Curso + "<br>" + Aula + "</p>";
                primera.style.backgroundColor = "#3276b1";
                primera.rowSpan = CantCeldas.toString();
                var cookie = getCookie("i");//i es el nombre de la cookie, es un contador de cursos pero en toda la sesion no local
                var cantidad = 0;
                if (cookie != "" && cookie != null && !isNaN(cookie)) {
                    cantidad = parseInt(cookie);
                }
                cantidad++;
                setCookie("i", cantidad.toString(), 1);
                setCookie("Cookie" + cantidad.toString(), Curso + "|" + Dia + "|" + HoraInicioCookie + "|" + HoraFinCookie + "|" + "-" + "|" + Horario.ID + "|" + Horario.Aula, 1);
            }

        });
    });
}

function AgregarCursos(pDia) {

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

        var table = document.getElementById("Resultado");
        var Curso = document.getElementById("Course").options[document.getElementById("sltCurso").selectedIndex].text;
        var Bloque = document.getElementById("Block").options[document.getElementById("sltBloque").selectedIndex].value;
        var Grupo = document.getElementById("Group").options[document.getElementById("sltGrupo").selectedIndex].value;
        var GrupoText = document.getElementById("Group").options[document.getElementById("sltGrupo").selectedIndex].text;
        var Aula = document.getElementById("sltAula").options[document.getElementById("sltAula").selectedIndex].text;
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
        alert("ERROR: Debe seleccionar el grupo de un curso en una aula");
        document.getElementById(IdDiaSeleccionado).checked = false;
        document.getElementById(IdDiaSeleccionado).disabled = false;
        document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
        document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
        return;
    }
    if (Inicio == Fin) {
        alert("ERROR: La hora de inicio y fin son iguales");
        document.getElementById(IdDiaSeleccionado).checked = false;
        document.getElementById(IdDiaSeleccionado).disabled = false;
        document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
        document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
        return;
    }
    if (Inicio > Fin) {
        alert("ERROR: La hora de inicio es posterior a la de fin");
        document.getElementById(IdDiaSeleccionado).checked = false;
        document.getElementById(IdDiaSeleccionado).disabled = false;
        document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
        document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
        return;
    }
    if (Grupo == "") {
        alert("ERROR: Seleccione un grupo");
        document.getElementById(IdDiaSeleccionado).checked = false;
        document.getElementById(IdDiaSeleccionado).disabled = false;
        document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
        document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
        return;
    }
    else if (Aula == "-- Seleccionar Aula --") {
        alert("ERROR: Seleccione una aula");
        document.getElementById(IdDiaSeleccionado).checked = false;
        document.getElementById(IdDiaSeleccionado).disabled = false;
        document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
        document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
        return;
    }

    var choque = 0;


    for (k = 1; k <= getCookie("i") ; k++) {
        var Detalles = getCookie("Cookie" + k);
        var Partes = Detalles.split("|");
        if (Partes[0] != "d") {
            if ((Partes[1] == Dia && ((Partes[2] <= Inicio && Partes[3] >= Inicio) || (Partes[2] <= Fin && Partes[3] >= Fin)
            || (Partes[2] <= Inicio && Partes[3] >= Fin) || (Partes[2] >= Inicio && Partes[3] <= Fin)))) {
                alert("ERROR: Ya hay un curso impartido en el horario seleccionado.");
                document.getElementById(IdDiaSeleccionado).checked = false;
                document.getElementById(IdDiaSeleccionado).disabled = false;
                document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
                document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
                choque = 1;
            }
        }

    }

    if (choque == 0) {
        var routeh = "/getHorarios/List/" + getCookie("SelPlanDeEstudio") + "/" + getCookie("PeriodoHorario");
        var hayChoque = 0;
        $.getJSON(routeh, function (data) {
            $.each(data, function (i, Horario) {
                var vCurso = Horario.Nombre;
                var vGrupoText = Horario.Numero;
                var vBloqueText = Horario.Descripcion;
                var vDia = Horario.Dia1;
                var vAula = Horario.Aula;
                var vHoraInicioCookie = Horario.Hora_Inicio;
                var vHoraFinCookie = Horario.Hora_Fin;
                var vHoraInicio = parseInt(vHoraInicioCookie);
                var vHoraFin = parseInt(vHoraFinCookie);

                if (Aula == vAula && Dia == vDia && ((vHoraInicio <= Inicio && vHoraFin >= Inicio) || (vHoraInicio <= Fin && vHoraFin >= Fin)
                || (vHoraInicio <= Inicio && vHoraFin >= Fin) || (vHoraInicio >= Inicio && vHoraFin <= Fin))) {
                    hayChoque = 1;
                    alert("ERROR: Ya hay un curso impartido en el aula y horario seleccionados: " + vCurso);
                    document.getElementById(IdDiaSeleccionado).checked = false;
                    document.getElementById(IdDiaSeleccionado).disabled = false;
                    document.getElementById(IdHoraInicioSeleccionada).selectedIndex = 0;
                    document.getElementById(IdHoraFinSeleccionada).selectedIndex = 0;
                    return false;
                }
            });
            if (hayChoque == 0) {
                i = Inicio;
                var CantCeldas = 0;
                var items = "";
                while (i < Fin) {

                    var IdCelda = Dia + " " + i;
                    if (i < 100) { IdCelda = Dia + " 0" + i; } //repara el string en caso de que la hora fuera 0010 ya que el parse la deja como 10
                    if (i == 0) { IdCelda = Dia + " 000"; }
                    var celda = document.getElementById(IdCelda);
                    if (i == Inicio) {
                        var primera = document.getElementById(IdCelda);
                    }
                    else if (celda != null) {
                        items += celda.innerHTML;
                        celda.parentNode.removeChild(celda);
                    }
                    i += 10;
                    if (i % 100 == 60) { i += 40; }//cuando se llega al minuto 60 se le suman 40 al numero para que pase por ejemplo de 1060 a 1100
                    CantCeldas++;
                }
                primera.innerHTML += items + "<p id=" + Dia + ">" + Curso + "<br>" + Aula + "</p>";
                primera.style.backgroundColor = "#51ff00";
                primera.rowSpan = CantCeldas;

                //Actualizo el contador
                var cookie = getCookie("i");//i es el nombre de la cookie, es un contador de cursos pero en toda la sesion no local
                var cantidad = 0;
                if (cookie != "" && cookie != null && !isNaN(cookie)) {
                    cantidad = parseInt(cookie);
                }
                cantidad++;
                setCookie("i", cantidad.toString(), 1);
                setCookie("Cookie" + cantidad.toString(), Curso + "|" + Dia + "|" + HoraInicio + "|" + HoraFin + "|" + Bloque + "|" + Grupo + "|" + Aula, 1);
                document.getElementById(IdDiaSeleccionado).disabled = false;
            }
        });
    }


}


function EliminarCurso(pDia) {

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

        var table = document.getElementById("Resultado");
        var Curso = document.getElementById("sltCurso").options[document.getElementById("sltCurso").selectedIndex].text;
        var Bloque = document.getElementById("sltBloque").options[document.getElementById("sltBloque").selectedIndex].value;
        var Grupo = document.getElementById("sltGrupo").options[document.getElementById("sltGrupo").selectedIndex].value;
        var GrupoText = document.getElementById("sltGrupo").options[document.getElementById("sltGrupo").selectedIndex].text;
        var Aula = document.getElementById("sltAula").options[document.getElementById("sltAula").selectedIndex].text;
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
        alert("ERROR: Debe seleccionar el grupo de un curso en una aula");
        return;
    }

    if (Grupo == "") {
        alert("ERROR: Debe seleccionar el grupo")
        return;
    }

    var Cookie = getCookie("i");
    if (Cookie == "" || Cookie == null || isNaN(Cookie)) {
        alert("ERROR: No hay cursos que eliminar");
        return;
    }
    //Busco la cookie que se va a eliminar
    var objetivo;
    var Cantidad = parseInt(Cookie);
    for (var j = 1; j <= Cantidad; j++) {
        var ArrayCookie = getCookie("Cookie" + j.toString()).split("|");
        var CursoCookie = ArrayCookie[0];
        var DiaCookie = ArrayCookie[1];
        var HoraInicioCookie = ArrayCookie[2];
        var HoraFinCookie = ArrayCookie[3];
        var CursoCookieGrupo = ArrayCookie[5];
        if (HoraInicioCookie.charAt(0) == 0) {
            HoraInicioCookie = HoraInicioCookie.substr(1, 3);
        }
        if (CursoCookie == Curso && DiaCookie == Dia && CursoCookieGrupo == Grupo) {
            objetivo = j;
            //Elimino la cookie
            ArrayCookie[0] = "d";
            var celda = document.getElementById(Dia + " " + HoraInicioCookie);
            celda.style.backgroundColor = "";
            setCookie("Cookie" + j.toString(), ArrayCookie.join("|"), 1);
            $("p[id=" + Dia + "]").remove(":contains('" + Curso + "" + Aula + "')");
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

function borrarTabla() {


    var Dia = "";
    for (var k = 0; k < 7; k++) {
        Dia = Dias(k);
        var Fin = 2300;
        //Actualizo la tabla
        var i = 000;
        while (i < Fin) {
            var IdCelda = Dia + " " + i;
            if (i < 100) { IdCelda = Dia + " 0" + i; } //repara el string en caso de que la hora fuera 0010 ya que el parse la deja como 10
            if (i == 0) { IdCelda = Dia + " 000"; }
            var objetivo = document.getElementById(IdCelda);
            var columna;
            switch (Dia) {
                case "Lunes":
                    columna = 1;
                    break;
                case "Martes":
                    columna = 2;
                    break;
                case "Miercoles":
                    columna = 3;
                    break;
                case "Jueves":
                    columna = 4;
                    break;
                case "Viernes":
                    columna = 5;
                    break;
                case "Sabado":
                    columna = 6;
                    break;
                case "Domingo":
                    columna = 7;
                    break;
            }
            if (objetivo != null && objetivo.innerHTML != "") {
                objetivo.innerHTML = "";
                objetivo.style.backgroundColor = "";
                objetivo.rowSpan = "1";
            }
            else if (Dia != "Lunes" && objetivo == null) {
                var DiaAnterior = DiaAnt(Dia);
                IdCelda = DiaAnterior + " " + i
                objetivo = document.getElementById(IdCelda);
                if (objetivo != null) {
                    objetivo.outerHTML += '<td id="' + Dia + ' ' + i + '"></td>'
                }
            }
            else if (Dia == "Lunes" && objetivo == null) {
                objetivo = DiaAntNull(i);
                if (objetivo != null)
                    objetivo.outerHTML = '<td id="' + Dia + ' ' + i + '"></td>' + objetivo.outerHTML;
            }
            i += 10;
            if (i % 100 == 60) { i += 40; }//cuando se llega al minuto 60 se le suman 40 al numero para que pase por ejemplo de 1060 a 1100
        }
    }

}

function DiaAnt(Dia) {
    switch (Dia) {
        case "Lunes":
            return "Domingo";
            break;
        case "Martes":
            return "Lunes";
            break;
        case "Miercoles":
            return "Martes";
            break;
        case "Jueves":
            return "Miercoles";
            break;
        case "Viernes":
            return "Jueves";
            break;
        case "Sabado":
            return "Viernes";
            break;
        case "Domingo":
            return "Sabado";
            break;
    }
}

function DiaAntNull(i) {
    for (var k = 0; k < 7; k++) {
        var Dia = Dias(k);
        var IdCelda = Dia + " " + i;
        var objetivo = document.getElementById(IdCelda);
        if (objetivo != null)
            return objetivo;
    }

}

function Dias(k) {

    switch (k) {
        case 0:
            return "Lunes";
        case 1:
            return "Martes";
        case 2:
            return "Miercoles";
        case 3:
            return "Jueves";
        case 4:
            return "Viernes";
        case 5:
            return "Sabado";
        case 6:
            return "Domingo";
    }

}

/* ESTA FUNCION AGREGA TODOS LOS CURSOS QUE ESTAN CHECKED EN EL HORARIO 
function AgregarCurso() {
    for (contador = 0; contador < 7; contador++) //Busca si hay check en los dias y agrega el horario
    {
        var IdDiaSeleccionado = "";
        var IdHoraInicioSeleccionada = "";
        var IdHoraFinSeleccionada = "";
        var Dia = "";
        switch (contador) {
            case 0: Dia = "Lunes"; IdDiaSeleccionado = "LunesID"; IdHoraInicioSeleccionada = "ComboHoraInicioLunes"; IdHoraFinSeleccionada = "ComboHoraFinLunes"; break;
            case 1: Dia = "Martes"; IdDiaSeleccionado = "MartesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMartes"; IdHoraFinSeleccionada = "ComboHoraFinMartes"; break;
            case 2: Dia = "Miercoles"; IdDiaSeleccionado = "MiercolesID"; IdHoraInicioSeleccionada = "ComboHoraInicioMiercoles"; IdHoraFinSeleccionada = "ComboHoraFinMiercoles"; break;
            case 3: Dia = "Jueves"; IdDiaSeleccionado = "JuevesID"; IdHoraInicioSeleccionada = "ComboHoraInicioJueves"; IdHoraFinSeleccionada = "ComboHoraFinJueves"; break;
            case 4: Dia = "Viernes"; IdDiaSeleccionado = "ViernesID"; IdHoraInicioSeleccionada = "ComboHoraInicioViernes"; IdHoraFinSeleccionada = "ComboHoraFinViernes"; break;
            case 5: Dia = "Sabado"; IdDiaSeleccionado = "SabadoID"; IdHoraInicioSeleccionada = "ComboHoraInicioSabado"; IdHoraFinSeleccionada = "ComboHoraFinSabado"; break;
            case 6: Dia = "Domingo"; IdDiaSeleccionado = "DomingoID"; IdHoraInicioSeleccionada = "ComboHoraInicioDomingo"; IdHoraFinSeleccionada = "ComboHoraFinDomingo"; break;
            default: break;
        }

        var isCheck = document.getElementById(IdDiaSeleccionado).checked;

        if (isCheck == true) {
            try {
                
                var table = document.getElementById("Resultado");
                var Curso = document.getElementById("sltCurso").options[document.getElementById("sltCurso").selectedIndex].text;
                var Bloque = document.getElementById("sltBloque").options[document.getElementById("sltBloque").selectedIndex].value;
                var Grupo = document.getElementById("sltGrupo").options[document.getElementById("sltGrupo").selectedIndex].value;
                var GrupoText = document.getElementById("sltGrupo").options[document.getElementById("sltGrupo").selectedIndex].text;
                var Aula = document.getElementById("sltAula").options[document.getElementById("sltAula").selectedIndex].text;
                var HoraInicioTemp = document.getElementById(IdHoraInicioSeleccionada).options[document.getElementById(IdHoraInicioSeleccionada).selectedIndex].value;
                var HoraFinTemp = document.getElementById(IdHoraFinSeleccionada).options[document.getElementById(IdHoraFinSeleccionada).selectedIndex].value;
                var separador = ":";
                var HoraInicio = HoraInicioTemp.replace(separador,'');
                var HoraFin = HoraFinTemp.replace(separador, '');
                var Inicio = parseInt(HoraInicio)
                var Fin = parseInt(HoraFin)
                var i = Inicio
            }
            catch (err) {
                alert("ERROR: Debe seleccionar el grupo de un curso en una aula");
                return;
            }
            if (Inicio == Fin) {
                alert("ERROR: La hora de inicio y fin son iguales");
                return;
            }
            if (Inicio > Fin) {
                alert("ERROR: La hora de inicio es posterior a la de fin");
                return;
            }
            if (Grupo == "") {
                alert("ERROR: Seleccione un grupo");
                return;
            }
            else if (Aula == "-- Seleccionar Aula --") {
                alert("ERROR: Seleccione una aula");
                return;
            }
            
            var choque = 0;
            
            for (k = 1; k <= getCookie("i") ; k++) {
                var Detalles = getCookie("Cookie" + k);
                var Partes = Detalles.split("|");
                if (Partes[0] != "d") {
                    if ((Partes[1] == Dia && ((Partes[2] <= Inicio && Partes[3] >= Inicio) || (Partes[2] <= Fin && Partes[3] >= Fin)
                        || (Partes[2] <= Inicio && Partes[3] >= Fin) || (Partes[2] >= Inicio && Partes[3] <= Fin)))) {
                        alert("ERROR: Ya hay un curso impartido en ese horario o esa aula");
                        choque = 1;
                    }
                }

            }
            if (choque == 0) {
                i = Inicio;
                var CantCeldas = 0;
                var items = "";
                while (i < Fin) {

                    var IdCelda = Dia + " " + i;
                    if (i < 100) { IdCelda = Dia + " 0" + i; } //repara el string en caso de que la hora fuera 0010 ya que el parse la deja como 10
                    if (i == 0) { IdCelda = Dia + " 000"; }
                    var celda = document.getElementById(IdCelda);
                    if (i == Inicio) {
                        var primera = document.getElementById(IdCelda);
                    }
                    else if (celda != null) {
                        items += celda.innerHTML;
                        celda.parentNode.removeChild(celda);
                    }
                    i += 10;
                    if (i % 100 == 60) { i += 40; }//cuando se llega al minuto 60 se le suman 40 al numero para que pase por ejemplo de 1060 a 1100
                    CantCeldas++;
                }
                primera.innerHTML += items + "<p id=" + Dia + ">" + Curso + "<br>" + Aula + "</p>";
                primera.style.backgroundColor = "#51ff00";
                primera.rowSpan = CantCeldas;

                //Actualizo el contador
                var cookie = getCookie("i");//i es el nombre de la cookie, es un contador de cursos pero en toda la sesion no local
                var cantidad = 0;
                if (cookie != "" && cookie != null && !isNaN(cookie)) {
                    cantidad = parseInt(cookie);
                }
                cantidad++;
                setCookie("i", cantidad.toString(), 1);
                setCookie("Cookie" + cantidad.toString(), Curso + "|" + Dia + "|" + HoraInicio + "|" + HoraFin + "|" + Bloque + "|" + Grupo + "|" + Aula, 1);
            }
        }
    }

}
*/