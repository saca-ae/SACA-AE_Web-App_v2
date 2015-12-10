var contador = 2;
var acumulador = 2;

function submit_form() {
    var validate = validate_schedule();
    if (validate) {
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

        $("#formPost").submit();
    }
    else {
        alert("Existe conflicto de horario en la tabla de horarios")
    }
}


function validate_schedule() {
    var table = document.getElementById("new_schedule");
    var rowCount = table.rows.length;
    var contador = acumulador - (rowCount - 1);
    while (contador < acumulador) {
        var vDay = $('#trRow' + contador).attr('data-day');
        var vstrStartHour = $('#trRow' + contador).attr('data-starthour');
        var vstrEndHour = $('#trRow' + contador).attr('data-endhour');
        var vStartHour = new Date("1/1/2015 " + vstrStartHour);
        var vEndHour = new Date("1/1/2015 " + vstrEndHour);

        for (var i = contador; i < acumulador ; i++) {

            var vNextDay = "";
            var vstrNextStartHour = "";
            var vstrNextEndHour = "";
            if (i + 1 == acumulador) {
                var contador_siguiente = i + 1;
                vNextDay = $('#trRow' + (contador_siguiente)).attr('data-day');
                vstrNextStartHour = $('#trRow' + (contador_siguiente)).attr('data-starthour');
                vstrNextEndHour = $('#trRow' + (contador_siguiente)).attr('data-endhour');

                var vNextStartHour = new Date("1/1/2015 " + vstrNextStartHour);
                var vNextEndHour = new Date("1/1/2015 " + vstrNextEndHour);


                if (vDay == vNextDay) {
                    if ((vStartHour <= vNextStartHour && vNextStartHour <= vEndHour) ||
                        (vStartHour <= vNextEndHour && vNextEndHour <= vEndHour) ||
                        (vNextStartHour <= vStartHour && vStartHour <= vNextEndHour) ||
                        (vNextStartHour <= vEndHour && vEndHour <= vNextEndHour)) {
                        //is inconsistency in the schedule
                        return false;
                    }
                }
                break;
            }
        }
        contador++;
    }
    return true;
}
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
    $("#Plan").change(function () {
        var route = "/Horarios/Plan/" + $('select[name="Plan"]').val() + "/Bloques"
        $.getJSON(route, function (data) {
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

        //Se repite por razones desconocidas
        var rep = 1;
        $("#Group").change(function () {
            if (rep == 1) {
                var table = document.getElementById('new_schedule');
                var table_tbody = table.getElementsByTagName('tbody')[0];


                var route = "/Horarios/Sedes/" + $('select[name="Sede"]').val() + "/Aulas";
                var current_acumulador = acumulador + 1;


                var items = "";

                $.getJSON(route, function (data) {
                    var table = document.getElementById("new_schedule");
                    var trRow = table.getElementsByTagName("tr");
                    var rowCount = table.rows.length;

                    items = "<select>";
                    $.each(data, function (i, aula) {
                        if (i == 0) {
                            firstClassroomID = aula.ID;
                        }
                        items += "<option value='" + aula.ID + "'>" + aula.Code + "</option>";
                    });
                    items += "</select>"

                    /***********************************************
                    Cargamos los datos en la tabla
                    ************************************************/
                    load_info_group(table, table_tbody, items, firstClassroomID)
                });
            }
            else { return 0 }
        });

    });
});

function select_item(classroom_cell) {
    return classroom_cell.getElementsByTagName('select')[0];
}

//Carga la informacion en la tabla en caso de poseer
function load_info_group(table, table_tbody, items, firstClassroomID) {
    var routeGroup = "/Horarios/Grupo/" + $('select[name="Group"]').val()
    var i;
    var error = false;

    var current_item = items;

    $.getJSON(routeGroup, function (data) {
        if (data.length != 0) {

            delete_rows_from_table(table.length)
            for (i = 0; i < data.length; i++) {

                var row = table_tbody.insertRow(table_tbody.rows.length);
                row.setAttribute("id", "trRow" + (acumulador));
                row.setAttribute("data-day", data[i].Day);
                row.setAttribute("data-starthour", data[i].StartHour);
                row.setAttribute("data-endhour", data[i].EndHour);
                row.setAttribute("data-classroom", data[i].ClassroomID);

                var day_cell = row.insertCell(0);

                var start_cell = row.insertCell(1);

                var end_cell = row.insertCell(2);

                var classroom_cell = row.insertCell(3);

                var delete_cell = row.insertCell(4);

                day_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "Day\" onchange=\"onchangeDaySelection(" + (acumulador) + ")\">" +
                          optionDay() +
                      "</select>";
                $('#tdRow' + (acumulador) + 'Day option:contains(' + data[i].Day + ')').prop({ selected: true });

                start_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "StartHour\" onchange=\"onchangeStartHourSelection(" + (acumulador) + ")\">" +
                                      optionStartHour() +
                                  "</select>";
                $('#tdRow' + (acumulador) + 'StartHour option:contains(' + data[i].StartHour + ')').prop({ selected: true });

                end_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "EndHour\" onchange=\"onchangeEndHourSelection(" + (acumulador) + ")\">" +
                                          optionEndHour() +
                                        "</select>";
                $('#tdRow' + (acumulador) + 'EndHour option:contains(' + data[i].EndHour + ')').prop({ selected: true });


                classroom_cell.innerHTML = current_item;

                var select = classroom_cell.getElementsByTagName('select')[0];

                select.setAttribute("id", "tdRow" + acumulador + "Classroom");
                select.setAttribute("onchange", "onchangeClassroomSelection(" + acumulador + ")")
                $('#tdRow' + acumulador + 'Classroom option:contains(' + data[i].Code + ')').prop({ selected: true });

                delete_cell.innerHTML = "<a  onclick = \"delete_row(" + (acumulador) + ")\" title=\"Eliminar\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span></a>";
                delete_cell.style.textAlign = "center";
                delete_cell.getElementsByTagName("a")[0].style.marginTop = "10px";

                contador += 1;
                acumulador += 1;

            }
        }

            //***************************************************************
            //**************** En caso de no encontrarse datos de la consulta
            //****************************************************************
        else {
            delete_rows_from_table(table.length)
            var row = table_tbody.insertRow(table_tbody.rows.length);
            row.setAttribute("id", "trRow" + (acumulador));
            row.setAttribute("data-day", "Lunes");
            row.setAttribute("data-starthour", "07:30 am");
            row.setAttribute("data-endhour", "08:20 am");
            row.setAttribute("data-classroom", firstClassroomID);

            var day_cell = row.insertCell(0);

            var start_cell = row.insertCell(1);

            var end_cell = row.insertCell(2);

            var classroom_cell = row.insertCell(3);

            var delete_cell = row.insertCell(4);

            day_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "Day\" onchange=\"onchangeDaySelection(" + (acumulador) + ")\">" +
                                     optionDay() +
                                  "</select>";

            start_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "StartHour\" onchange=\"onchangeStartHourSelection(" + (acumulador) + ")\">" +
                                  optionStartHour() +
                              "</select>";

            end_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "EndHour\" onchange=\"onchangeEndHourSelection(" + (acumulador) + ")\">" +
                                     optionEndHour() +
                                    "</select>";

            classroom_cell.innerHTML = items;

            var select = select_item(classroom_cell);
            select.setAttribute("id", "tdRow" + acumulador + "Classroom");
            select.setAttribute("onchange", "onchangeClassroomSelection(" + acumulador + ")")

            delete_cell.innerHTML = "<a  onclick = \"delete_row(" + (acumulador) + ")\" title=\"Eliminar\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span></a>";
            delete_cell.style.textAlign = "center";
            delete_cell.getElementsByTagName("a")[0].style.marginTop = "10px";

            contador += 1;
            acumulador += 1;
        }

    });
}
//Limpiar todas las tablas de tbody
function delete_rows_from_table(n) {
    var table = document.getElementById("new_schedule");
    var i;
    var trRow = table.getElementsByTagName("tr");
    var rowCount = table.rows.length;
    for (i = 1; i < rowCount; i++) {
        table.deleteRow(1);
    }
    contador = 1;
}
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


function obtener_dias() {
    var table = document.getElementById("new_schedule");
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

        celda_Day.innerHTML = "<select id=\"tdRow" + acumulador + "Day\" onchange=\"onchangeDaySelection(" + acumulador + ")\">" +
                                 optionDay() +
                              "</select>";
        celda_StarHour.innerHTML = "<select id=\"tdRow" + acumulador + "StartHour\" onchange=\"onchangeStartHourSelection(" + acumulador + ")\">" +
                                  optionStartHour() +
                              "</select>";
        celda_EndHour.innerHTML = "<select id=\"tdRow" + acumulador + "EndHour\" onchange=\"onchangeEndHourSelection(" + acumulador + ")\">" +
                                     optionEndHour() +
                                    "</select>";

        var current_acumulador = acumulador;
        var route = "/Horarios/Sedes/" + $('select[name="Sede"]').val() + "/Aulas";


        $.getJSON(route, function (data) {
            var items = "<select id=\"tdRow" + current_acumulador + "Classroom\" onchange=\"onchangeClassroomSelection(" + current_acumulador + ")\">";
            $.each(data, function (i, aula) {
                if (i == 0) {
                    row.setAttribute("data-classroom", aula.ID);

                }
                items += "<option value='" + aula.ID + "'>" + aula.Code + "</option>";
            });
            items += "</select>"

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
    $('#trRow' + identificador).attr('data-endhour', vEndHour)
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

//Static information
function optionDay() {
    var vOptionDay = "<option>Lunes</option>" +
                        "<option>Martes</option>" +
                        "<option>Miércoles</option>" +
                        "<option>Jueves</a></option>" +
                        "<option>Viernes</a></option>" +
                        " <option>Sábado</a></option>";

    return vOptionDay;
}

function optionStartHour() {
    var vOptionStartHour = "<option>07:30 am</option>" +
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
                            "<option>09:00 pm</option>";

    return vOptionStartHour;
}

function optionEndHour() {
    vOptionEndtHour = "<option>08:20 am</option>" +
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
                        "<option>09:50 pm</option>";

    return vOptionEndtHour;

}