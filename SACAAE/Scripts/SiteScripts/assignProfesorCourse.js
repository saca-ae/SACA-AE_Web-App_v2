
function editar_asignacion_grupo(id_grupo) {

    var route_temporal = "/Curso/EditarAsignacion/" + id_grupo;
    window.location = route_temporal;
}

/***************************************************** Function use in Details.cshtml***************************************************/
/// <summary>
///  Return view for a detail group according a id group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of Group to delete/param>
/// <returns>None</returns>
function ver_detalle_grupo(vIDGroup) {
    var route_temporal = "/Curso/DetalleAsignacion/" + vIDGroup;
    window.location = route_temporal;
}

/// <summary>
///  Get the element select for delete
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of Group to delete/param>
/// <returns>None</returns>
function init_delete(vIDGroup) {
    tempIndex = vIDGroup;
}

/// <summary>
/// Remove profesor from a group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <returns> If not have problem, return message 'success' to a View, else return message 'Error'</returns>
function eliminar_asignacion_grupo() {
    var route_temporal = "/CursoProfesor/Group/" + tempIndex + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {
            var route = "/CursoProfesor/Cursos/" + get_id(3);
            document.getElementById('tabla_grupos').innerHTML = "";
            generate_table(route);
            location.reload();
        }
        else {
            alert('Error');
        }
    });
}

/*********************************************************************************************************************************************/

/***************************************************** Function use in AsignarProfesoraCurso.cshtml***************************************************/
$("#Sedes").change(function () {
    var sede = document.getElementById('Sedes').value
    var route = "/CursoProfesor/Cursos/" + get_id(3) + "/Sedes/" + sede + "/Modalidades"
    $.getJSON(route, function (data) {
        var items = "";

        $.each(data, function (i, modalidad) {
            items += "<option value='" + modalidad.ID + "'>" + modalidad.Name + "</option>";
        });


        if (items != "") {
            $("#Modalidades").html(items);
            $("#Modalidades").prepend("<option value='' selected='selected'>-- Seleccionar Modalidad --</option>");

        }
        else {
            $("#Modalidades").html("<option>No hay modalidades relacionadas a la sede.</option>")

        }
    });
});

$('#Modalidades').change(function () {
    var sede = document.getElementById('Sedes').value;
    var modalidad = document.getElementById('Modalidades').value
    var route = "/CursoProfesor/Cursos/" + get_id(3) + "/Sedes/" + sede + "/Modalidades/" + modalidad + "/Planes"
    $.getJSON(route, function (data) {
        var items = "";

        $.each(data, function (i, plan) {
            items += "<option value='" + plan.ID + "'>" + plan.Name + "</option>";
        });


        if (items != "") {
            $("#Planes").html(items);
            $("#Planes").prepend("<option value='' selected='selected'>-- Seleccionar Plan de estudio --</option>");

        }
        else {
            $("#Planes").html("<option>No hay planes de estudio relacionadas a la modalidad seleccionada.</option>")

        }
    });
});


$('#Planes').change(function () {
    var sede = document.getElementById('Sedes').value;
    var modalidad = document.getElementById('Modalidades').value
    var plan = document.getElementById('Planes').value
    var route = "/CursoProfesor/Cursos/" + get_id(3) + "/Sedes/" + sede + "/Modalidades/" + modalidad + "/Planes/" + plan + "/Bloques"
    $.getJSON(route, function (data) {
        var items = "";

        $.each(data, function (i, bloque) {

            items += "<option value='" + bloque.ID + "'>" + bloque.Description + "</option>";
        });


        if (items != "") {
            $("#Bloques").html(items);
            $("#Bloques").prepend("<option value='' selected='selected'>-- Seleccionar Plan de estudio --</option>");

        }
        else {
            $("#Bloques").html("<option>No hay bloques academicos relacionadas al plan de estudio seleccionado</option>")

        }
    });
});

$("#Bloques").change(function () {

    var sede = document.getElementById('Sedes').value;
    var modalidad = document.getElementById('Modalidades').value
    var plan = document.getElementById('Planes').value
    var bloque = document.getElementById('Bloques').value

    var route = "/CursoProfesor/Sedes/" + sede + "/Planes/" + plan + "/Bloques/" + bloque + "/Cursos/" + get_id(3) + "/GroupWithoutProfesor";

    $.getJSON(route, function (data) {
        var items = "";

        $.each(data, function (i, grupo) {
            items += "<option value='" + grupo.ID + "'>" + grupo.Number + "</option>";
        });


        if (items != "") {
            $("#Grupos_Disponibles").html(items);
            $("#Grupos_Disponibles").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");

        }
        else {
            $("#Grupos_Disponibles").html("<option>No hay grupos abiertos para ese curso.</option>")
        }
    });
});

$("#Grupos_Disponibles").change(function () {
    var sede = document.getElementById('Sedes').value;
    var modalidad = document.getElementById('Modalidades').value
    var plan = document.getElementById('Planes').value
    var grupo = document.getElementById('Grupos_Disponibles').value;
    /*Obtiene la información del curso*/
    var route1 = "/CursoProfesor/Grupos/Info/" + grupo;
    /*Obtiene la información del horario*/
    var route2;

    /*Items del select de horarios*/
    var items = "";

    /*Variables de info de curso*/
    var cupo = "";
    var aula = "";
    var id = "";

    /*Horas calculadas de acuerdo al horario*/
    var horas = 0.0;

    $.getJSON(route1, function (data) {
        cupo = data.Capacity;
        horas = data.TheoreticalHours;


        if (cupo != "" || aula != "" || id != "") {
            $("#txtHoras").val(horas)
            $('#txtHorasEstimadas').val("")
        }
        else {
            $("#txtCupo").val("No Disponible")

        }

        /* Obtener información del horario */
        route2 = "/CursoProfesor/Cursos/" + get_id(3) + "/Sedes/" + sede + "/Modalidades/" + modalidad + "/Planes/" + plan + "/Grupos/" + grupo + "/Horario"
        var tabla = "<table id=\"profesores_asignados_a_curso\" class=\"table table-hover\">" +
       "<thead>" +
           "<th>Dia</th>" +
           "<th>Hora Inicio</th>" +
           "<th>Hora Fin</th>" +
           "<th>Aula</th>" +
       "</thead>" +
       "<tbody>";
        $.getJSON(route2, function (data) {

            if (data.length != 0) {
                for (i = 0; i < data.length; i++) {
                    tabla = tabla + "<tr>" +
                    "<td>" + data[i].Day + "</td>" +
                    "<td>" + data[i].StartHour + "</td>" +
                    "<td>" + data[i].EndHour + "</td>" +
                    "<td>" + data[i].Code + "</td>" +
                    "</tr>";


                }

                tabla = tabla + "</tbody> </table>";

                document.getElementById('table_schedule_group').innerHTML = tabla;
            }
            else {
                /*Sin Profesores asignados*/
                document.getElementById('table_schedule_group').innerHTML = "<p> Sin profesores asignados</p>";
            }
        });
    });
});

$("#HourCharge").change(function () {
    var HourSelection = document.getElementById("HourCharge").value;

    if (HourSelection == 1) {
        $("#txtHorasEstimadas").val(0)
    }
    else {
        var horas_teoricas = document.getElementById("txtHoras").value;

        var horas_estimadas = 11 - horas_teoricas;

        $("#txtHorasEstimadas").val(horas_estimadas);
    }
});
/*********************************************************************************************************************************************/
/***************************************************** Function use in EditarAsignacion.cshtml***************************************************/
function changeHourCharge() {
    var HourSelection = document.getElementById("editHourCharge").value;

    if (HourSelection == '1')
    {
        document.getElementById("divHorasEstimadas").innerHTML = "<p>0</p>"
    }
    else if (HourSelection != '1')
    {
        var horas_estimadas = 11 - TheoricalHour;

        document.getElementById("divHorasEstimadas").innerHTML = "<p>" + horas_estimadas + "</p>"
    }
};
/*********************************************************************************************************************************************/

function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}

function adjust_hour(hour)
{
    
    if (hour > '1000')
    {

        var new_hour = 
    }
    else
    {
    }
}