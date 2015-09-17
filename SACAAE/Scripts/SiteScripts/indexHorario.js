$(document).ready(function () {


    $("#buttonPost").on("click", function () {
        var listName = "NewSchedule";

        var i = 0;
        $("#schedule_comission > tbody > tr").each(function () {
            var day = $(this).data("day");
            var starthour = $(this).data("starthour");
            var endhour = $(this).data("endhour");
            var classroom = $(this).data("classroom");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].Day' value='" + day + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].StartHour' value='" + starthour + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].EndHour' value='" + endhour + "'>");
            $("#formPost").prepend("<input type='hidden' name '" + listName + "[" + i + "].Classroom' value='" + classroom + "'>");
            i = i + 1;
        });
    });
});

$(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltPlan").prop("disabled", "disabled");
    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltModalidad").change(function () {

        var route = "/CursoProfesor/Planes/List/" + $('select[name="sltSede"]').val() + "/" + $('select[name="sltModalidad"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {
                items += "<option value= " + plan.ID + ">" + plan.Name + "</option>";
            });

            if (items != "") {
                $("#sltPlan").prop("disabled", false);
                $("#sltPlan").html(items);
                $("#sltPlan").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
        });
    });
    //Funcion llamada cuando se cambien los valores de los planes
    $("#sltPlan").change(function ()
    {
        var route = "/Horarios/Plan/" + $('select[name="sltPlan"]').val() + "/Bloques"
        $.getJSON(route, function (data)
        {
            var items = "";
            $.each(data, function (i, bloque) {
                items += "<option value= " + bloque.ID + ">" + bloque.Description + "</option>";
            });

            if (items != "") {
                $("#sltBloque").prop("disabled", false);
                $("#sltBloque").html(items);
                $("#sltBloque").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
        })
    })

    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltBloque").change(function () {
        var plan = $('select[name="sltPlan"]').val();

        var route = "/CursoProfesor/Cursos/List/" + plan + "/" + $('select[name="sltBloque"]').val();
        
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, curso) {
                items += "<option value= " + curso.ID + ">" + curso.Name + "</option>";
            });


            $("#sltCurso").prop("disabled", false);
            $("#sltGrupo").html("");
            $("#sltGrupo").prop("disabled", "disabled");
            $("#sltCurso").html(items);
            $("#sltCurso").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");

        });

        $("#sltCurso").change(function () {
            var plan = $('select[name="sltPlan"]').val();
            var sede = $('select[name="sltSede"]').val();
            var route = "/CursoProfesor/Grupos/List/" + $('select[name="sltCurso"]').val() + "/" + plan + "/" + sede + "/" + $('select[name="sltBloque"]').val() + "/" + $('select[name="sltPeriodo"]').val();
            $.getJSON(route, function (data) {
                var items = "";
                $.each(data, function (i, grupo) {
                    items += "<option value='" + grupo.ID + "'>" + grupo.Number + "</option>";
                });

                $("#sltGrupo").prop("disabled", false);
                $("#sltGrupo").html(items);
                $("#sltGrupo").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");

            });
        });

        $("#sltGrupo").change(function () {
            var route = "/Horarios/Sedes/" + $('select[name="sltSede"]').val() + "/Aulas";
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
    if ($('#sltPeriodo :selected').val() == " ") {
        alert("ERROR: Seleccione un periodo");
        return false;
    }
    else
        if ($('#sltSede :selected').val() == " ") {
            alert("ERROR: Seleccione una sede");
            return false;
        }
        else
            if ($('#sltModalidad :selected').val() == " ") {
                alert("ERROR: Seleccione una modalidad");
                return false;
            }

            else
                if ($('#sltPlan :selected').val() == "") {
                    alert("ERROR: Seleccione un plan");
                    return false;
                }

    setCookie("PeriodoHorario", $('#sltPeriodo :selected').val(), 1);
    setCookie("SelPlanDeEstudio", $('#sltPlan :selected').val(), 1);
    setCookie("SelModalidad", $('#sltModalidad :selected').val(), 1);
    setCookie("SelSede", $('#sltSede :selected').val(), 1);
}

/*Horarios*/
var contador = 2;
function obtener_dias() {
    var table = document.getElementById("new_schedule");
    //var row_count = table.rows.length
    //alert(contador);
    if (contador < 5) {
        var row = table.insertRow(contador);
        row.setAttribute("id", "trRow" + contador);
        row.setAttribute("data-day", "Lunes");
        row.setAttribute("data-starthour", "07:30 am");
        row.setAttribute("data-endhour", "08:20 am");
        
        contador++;

        var celda_Day = row.insertCell(0);

        var celda_StarHour = row.insertCell(1);
        var celda_EndHour = row.insertCell(2);
        var celda_Classroom = row.insertCell(3);

        celda_Day.innerHTML = "<select id=\"tdRow" + (contador - 1) + "Day\" onchange=\"onchangeDaySelection(" + (contador - 1) + ")\">" +
                                  "<option>Lunes</option>" +
                                  "<option>Martes</option>" +
                                  "<option>Miercoles</option>" +
                                  "<option>Jueves</a></option>" +
                                  "<option><a href=\"#\">Viernes</a></option>" +
                                 " <option><a href=\"#\">Sabado</a></option>" +
                              "</select>";
        celda_StarHour.innerHTML = "<select id=\"tdRow" + (contador - 1) + "StartHour\" onchange=\"onchangeStartHourSelection(" + (contador - 1) + ")\">" +
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
        celda_EndHour.innerHTML = "<select id=\"tdRow" + (contador - 1) + "EndHour\" onchange=\"onchangeEndHourSelection(" + (contador - 1) + ")\">" +
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

        var route = "/Horarios/Sedes/" + $('select[name="sltSede"]').val() + "/Aulas";
        
        $.getJSON(route, function (data) {
            var items = "<select id=\"tdRow" + (contador - 1) + "Classroom\" onchange=\"onchangeClassroomSelection(" + (contador - 1) + ")\">";
            $.each(data, function (i, aula) {
                if (i == 0) {
                    row.setAttribute("data-classroom", aula.ID);
                    
                }
                items +="<option value='" + aula.ID + "'>" + aula.Code + "</option>";
            });
            items+="</select>"
            
            celda_Classroom.innerHTML = items;
        });
       

       
    }


}

function onchangeDaySelection(identificador) {
    $('#trRow' + identificador).attr('data-day', $('#tdRow' + identificador + 'Day').val());

}

function onchangeClassroomSelection(identificador) {
    $('#trRow' + identificador).attr('data-classroom', $('#tdRow' + identificador + 'Classroom').val());
}
function onchangeStartHourSelection(identificador) {
    var pStartHour = $('#tdRow' + identificador + 'StartHour').val();
    $('#trRow' + identificador).attr('data-starthour', pStartHour);
}

function onchangeEndHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();


    var int_hora_entrada;
    var int_hora_salida;
    switch (vStartHour) {
        case "07:30 am":
            int_hora_entrada = 730;
            break;
        case "08:30 am":
            int_hora_entrada = 830;
            break;
        case "09:30 am":
            int_hora_entrada = 930;
            break;
        case "10:30 am":
            int_hora_entrada = 1030;
            break;
        case "11:30 am":
            int_hora_entrada = 1130;
            break;
        case "12:30 pm":
            int_hora_entrada = 1230;
            break;

        case "01:00 pm":
            int_hora_entrada = 1300;
            break;
        case "02:00 pm":
            int_hora_entrada = 1400;
            break;

        case "03:00 pm":
            int_hora_entrada = 1500;
            break;
        case "04:00 pm":
            int_hora_entrada = 1600;
            break;

        case "05:00 pm":
            int_hora_entrada = 1700;
            break;
        case "06:00 pm":
            int_hora_entrada = 1800;
            break;

        case "07:00 pm":
            int_hora_entrada = 1900;
            break;
        case "08:00 pm":
            int_hora_entrada = 2000;
            break;

        case "09:00 pm":
            int_hora_entrada = 2100;
            break;
    }
    /*
    switch (vEndHour)
    {
        case "07:30 am":
            int_hora_entrada = 730;
            break;
        case "08:30 am":
            int_hora_entrada = 830;
            break;
        case "09:30 am":
            int_hora_entrada = 930;
            break;
        case "10:30 am":
            int_hora_entrada = 1030;
            break;
        case "11:30 am":
            int_hora_entrada = 1130;
            break;
        case "12:30 pm":
            int_hora_entrada = 1230;
            break;

        case "01:00 pm":
            int_hora_entrada = 1300;
            break;
        case "02:00 pm":
            int_hora_entrada = 1400;
            break;

        case "03:00 pm":
            int_hora_entrada = 1500;
            break;
        case "04:00 pm":
            int_hora_entrada = 1600;
            break;

        case "05:00 pm":
            int_hora_entrada = 1700;
            break;
        case "06:00 pm":
            int_hora_entrada = 1800;
            break;

        case "07:00 pm":
            int_hora_entrada = 1900;
            break;
        case "08:00 pm":
            int_hora_entrada = 2000;
            break;

        case "09:00 pm":
            int_hora_entrada = 2100;
            break;
    }
    */
    switch (vEndHour) {
        case "08:20 am":
            int_hora_salida = 820;
            break;
        case "09:20 am":
            int_hora_salida = 920;
            break;
        case "10:20 am":
            int_hora_salida = 1020;
            break;
        case "11:20 am":
            int_hora_salida = 1120;
            break;
        case "12:20 pm":
            int_hora_salida = 1220;
            break;

        case "01:50 pm":
            int_hora_salida = 1350;
            break;
        case "02:50 pm":
            int_hora_salida = 1450;
            break;

        case "03:50 pm":
            int_hora_salida = 1550;
            break;
        case "04:50 pm":
            int_hora_salida = 1650;
            break;

        case "05:50 pm":
            int_hora_salida = 1750;
            break;
        case "06:50 pm":
            int_hora_salida = 1850;
            break;

        case "07:50 pm":
            int_hora_salida = 1950;
            break;
        case "08:50 pm":
            int_hora_salida = 2050;
            break;

        case "09:50 pm":
            int_hora_salida = 2150;
            break;
    }

    //********************************************************************************
    if (int_hora_salida > int_hora_entrada) {
        $('#trRow' + identificador).attr('data-endhour', vEndHour);
    }
    else {
        // alert("Error");
    }

}