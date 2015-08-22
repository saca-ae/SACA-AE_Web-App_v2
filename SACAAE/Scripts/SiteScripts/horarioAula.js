function Load(pAula) {
    setCookie("Grupo", "");
    setCookie("Aulax", pAula, 1);
    Cargar();
}
function Cargar() {

    setCookie("i", 0, 1);
    setCookie("Cantidad", 0, 1);
    var route = "/ObtenerHorarioAula/List/" + getCookie("Aulax") + "/1";
    $.getJSON(route, function (data) {
        $.each(data, function (i, Horario) {
            var Curso = Horario.Nombre;
            var GrupoText = Horario.Numero;
            var BloqueText = Horario.Descripcion;
            var Dia = Horario.Dia1;
            var Aula = Horario.Aula;
            var HoraInicioCookie = Horario.Hora_Inicio;
            var HoraFinCookie = Horario.Hora_Fin;
            if (HoraInicioCookie != "700" && HoraInicioCookie != "800" && HoraInicioCookie != "900" && HoraInicioCookie != "1000" &&
                HoraInicioCookie != "1100" && HoraInicioCookie != "1200") {
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
                primera.innerHTML += items + "<p id=" + Dia + ">" + Curso + "<br>Grupo " + GrupoText + "</p>";
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



function setCookie(cname, cvalue, exdays) {

    document.cookie = cname + "=" + cvalue;
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

function ActualizarContadores() {
    setCookie("Cantidad", getCookie("i"), 1);
    setCookie("i", "0", 1);
}