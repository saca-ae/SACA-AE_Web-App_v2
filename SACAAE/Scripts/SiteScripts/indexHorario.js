$(document).ready(function () {


    $("#GuardarCambios").on("click", function () {
        var listName = "NewSchedule";

        var i = 0;
        $("#new_schedule > tbody > tr").each(function () {
            var day = $(this).data("day");
            var starthour = $(this).data("starthour");
            var endhour = $(this).data("endhour");
            var classroom = $(this).data("classroom");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].Day' value='" + day + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].StartHour' value='" + starthour + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].EndHour' value='" + endhour + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].Classroom' value='" + classroom + "'>");
            i = i + 1;
        });
    });
});

$(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#Plan").prop("disabled", "disabled");
    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Modality").change(function () {

        var route = "/CursoProfesor/Planes/List/" + $('select[name="Sede"]').val() + "/" + $('select[name="Modality"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {
                items += "<option value= " + plan.ID + ">" + plan.Name + "</option>";
            });

            if (items != "") {
                $("#Plan").prop("disabled", false);
                $("#Plan").html(items);
                $("#Plan").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
        });
    });
    //Funcion llamada cuando se cambien los valores de los planes
    $("#Plan").change(function ()
    {
        var route = "/Horarios/Plan/" + $('select[name="Plan"]').val() + "/Bloques"
        $.getJSON(route, function (data)
        {
            var items = "";
            $.each(data, function (i, bloque) {
                items += "<option value= " + bloque.ID + ">" + bloque.Description + "</option>";
            });

            if (items != "") {
                $("#Block").prop("disabled", false);
                $("#Block").html(items);
                $("#Block").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
        })
    })

    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Block").change(function () {
        var plan = $('select[name="Plan"]').val();

        var route = "/CursoProfesor/Cursos/List/" + plan + "/" + $('select[name="Block"]').val();
        
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

        $("#Course").change(function () {
            var plan = $('select[name="Plan"]').val();
            var sede = $('select[name="Sede"]').val();
            var route = "/CursoProfesor/Grupos/List/" + $('select[name="Course"]').val() + "/" + plan + "/" + sede + "/" + $('select[name="Block"]').val() + "/" + $('select[name="PeriodID"]').val();
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
            var route = "/Horarios/Sedes/" + $('select[name="Sede"]').val() + "/Aulas";
            $.getJSON(route, function (data) {
                var items = "";
                $.each(data, function (i, aula) {
                    if (i == 0) {
                        $('#trRow1').attr("data-classroom", aula.ID);
                    }
                    items += "<option value='" + aula.ID + "'>" + aula.Code + "</option>";
                });

                $('#tdRow1Classroom').prop("disabled", false);
                $('#tdRow1Classroom').html(items);

            });
        });
    });
});

function setCookie(cname, cvalue, exdays) {

    document.cookie = cname + "=" + cvalue + ";path=/";
}

function Enviar() {
    if ($('#PeriodID :selected').val() == " ") {
        alert("ERROR: Seleccione un periodo");
        return false;
    }
    else
        if ($('#Sede :selected').val() == " ") {
            alert("ERROR: Seleccione una sede");
            return false;
        }
        else
            if ($('#Modality :selected').val() == " ") {
                alert("ERROR: Seleccione una modalidad");
                return false;
            }

            else
                if ($('#Plan :selected').val() == "") {
                    alert("ERROR: Seleccione un plan");
                    return false;
                }

    setCookie("PeriodoHorario", $('#PeriodID :selected').val(), 1);
    setCookie("SelPlanDeEstudio", $('#Plan :selected').val(), 1);
    setCookie("SelModalidad", $('#Modality :selected').val(), 1);
    setCookie("SelSede", $('#Sede :selected').val(), 1);
}

/*Horarios*/
var contador = 2;
var acumulador = 2;

function obtener_dias() {
    var table = document.getElementById("new_schedule");
    //var row_count = table.rows.length
    //alert(contador);
    if (contador < 7) {
        var row = table.insertRow(contador);
        row.setAttribute("id", "trRow" + acumulador);
        row.setAttribute("data-day", "Lunes");
        row.setAttribute("data-starthour", "07:30 am");
        row.setAttribute("data-endhour", "08:20 am");
        
        contador++;

        var celda_Day = row.insertCell(0);

        var celda_StarHour = row.insertCell(1);
        var celda_EndHour = row.insertCell(2);
        var celda_Classroom = row.insertCell(3);
        var delete_cell = row.insertCell(4);

        celda_Day.innerHTML = "<select id=\"tdRow" + acumulador+ "Day\" onchange=\"onchangeDaySelection(" +acumulador+ ")\">" +
                                  "<option>Lunes</option>" +
                                  "<option>Martes</option>" +
                                  "<option>Miércoles</option>" +
                                  "<option>Jueves</a></option>" +
                                  "<option>Viernes</a></option>" +
                                 " <option>Sábado</a></option>" +
                              "</select>";
        celda_StarHour.innerHTML = "<select id=\"tdRow" + acumulador+ "StartHour\" onchange=\"onchangeStartHourSelection(" +acumulador+ ")\">" +
                                  "<option>07:30 am</option>" +
                                  "<option>08:30 am</option>" +
                                  "<option>09:30 am</option>" +
                                  "<option>10:30 am</option>" +
                                  "<option>11:30 am</option>" +
                                  "<option>12:30 pm</option>" +
                                  "<option>01:00 pm</option>" +
                                  "<option>02:00 pm</option>" +
                                  "<option>03:00 pm</option>" +
                                  "<option>04:00 pm</option>" +
                                  "<option>05:00 pm</option>" +
                                  "<option>06:00 pm</option>" +
                                  "<option>07:00 pm</option>" +
                                  "<option>08:00 pm</option>" +
                                  "<option>09:00 pm</option>" +
                              "</select>";
        celda_EndHour.innerHTML = "<select id=\"tdRow" + acumulador+ "EndHour\" onchange=\"onchangeEndHourSelection(" + acumulador+ ")\">" +
                                      "<option>08:20 am</option>" +
                                      "<option>09:20 am</option>" +
                                      "<option>10:20 am</option>" +
                                      "<option>11:20 am</option>" +
                                      "<option>12:20 pm</option>" +
                                      "<option>01:50 pm</option>" +
                                      "<option>02:50 pm</option>" +
                                      "<option>03:50 pm</option>" +
                                      "<option>04:50 pm</option>" +
                                      "<option>05:50 pm</option>" +
                                      "<option>06:50 pm</option>" +
                                      "<option>07:50 pm</option>" +
                                      "<option>08:50 pm</option>" +
                                      "<option>09:50 pm</option>" +
                                    "</select>";

        var current_acumulador = acumulador;
        var route = "/Horarios/Sedes/" + $('select[name="Sede"]').val() + "/Aulas";
        
        
        $.getJSON(route, function (data) {
            var items = "<select id=\"tdRow" + current_acumulador + "Classroom\" onchange=\"onchangeClassroomSelection(" + current_acumulador + ")\">";
            $.each(data, function (i, aula) {
                if (i == 0) {
                    row.setAttribute("data-classroom", aula.ID);
                    
                }
                items +="<option value='" + aula.ID + "'>" + aula.Code + "</option>";
            });
            items+="</select>"
            
            celda_Classroom.innerHTML = items;
        });
       
        delete_cell.innerHTML = "<a  onclick = \"delete_row(" + acumulador + ")\" title=\"Eliminar\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span></a>";
        delete_cell.style.textAlign = "center";
        delete_cell.style.borderCollapse = "collapse";
        delete_cell.getElementsByTagName("a")[0].style.marginTop = "10px";
       
    }

    acumulador += 1;
}

function onchangeDaySelection(identificador) {
    $('#trRow' + identificador).attr('data-day', $('#tdRow' + identificador + 'Day').val());

}

function onchangeClassroomSelection(identificador) {
    $('#trRow' + identificador).attr('data-classroom', $('#tdRow' + identificador + 'Classroom').val());
}
function onchangeStartHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    $('#trRow' + identificador).attr('data-starthour', vStartHour);
}

function onchangeEndHourSelection(identificador) {
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();
    $('#trRow'+identificador).attr('data-endhour', vEndHour)
}

function delete_row(i) {
    var table = document.getElementById("new_schedule");
    var trRow = table.getElementsByTagName("tr");
    var rowCount = table.rows.length;
    if (rowCount > 2) {
        for (var j = 0; j < trRow.length; j++) {
            if (trRow[j].id == "trRow" + i) {
                table.deleteRow(j);
                contador -= 1;
            }
        }
    }
}