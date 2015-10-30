$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltPlan").prop("disabled", "disabled");
    $("#sltCurso").prop("disabled", "disabled");
    $("#sltGrupo").prop("disabled", "disabled");
    $("#sltBloque").prop("disabled", "disabled");
    /* Se agregan los option por defecto a los select vacíos */
    itemSltPlan = "<option>Seleccione Sede y Modalidad</option>";
    itemSltBloque = "<option>Seleccione Sede, Modalidad y Plan de Estudio</option>";
    itemSltGrupo = "<option>Seleccione Sede, Modalidad, Plan de Estudio, Bloque y Curso</option>";
    itemSltCurso = "<option>Seleccione Sede, Modalidad, Plan de Estudio y Bloque</option>";

    $("#sltPlan").html(itemSltPlan);
    $("#sltCurso").html(itemSltCurso);
    $("#sltGrupo").html(itemSltGrupo);
    $("#sltBloque").html(itemSltBloque);

    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltModalidad, #sltSede").change(function () {

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
            else {
                $("#sltPlan").html("<option>No hay planes para esa sede y modalidad</option>");
            }
        });
    });

    $("#sltPlan").change(function () {
        var route = "/CursoProfesor/Bloques/List/" + $('select[name="sltPlan"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, bloque) {
                items += "<option value= " + bloque.ID + ">" + bloque.Description + "</option>";
            });

            if (items != "") {
                $("#sltBloque").html(items);
                $("#sltBloque").prepend("<option value='' selected='selected'>-- Seleccionar Bloque --</option>");
                $("#sltBloque").prop("disabled", false);
            }
            else {
                $("#sltBloque").html("<option>No hay Bloque asignados para ese plan de estudio.</option>")
            }
        });

    });
    $("#sltBloque").change(function () {
        var route = "/CursoProfesor/Cursos/List/" + $('select[name="sltPlan"]').val() + "/" + $('select[name="sltBloque"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, curso) {
                items += "<option value= " + curso.ID + ">" + (curso.Code + " - " + curso.Name) + "</option>";
            });


            $("#sltCurso").prop("disabled", false);
            $("#sltGrupo").html("");
            $("#sltGrupo").prop("disabled", "disabled");
            $("#sltCurso").html(items);
            $("#sltCurso").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");

        });
    });

    $("#sltCurso").change(function () 
    {
        //CursoProfesor/Sedes/{pSede:int}/Planes/{pPlan:int}/Bloques/{pBloque:int}/Cursos/{pCurso:int}/GroupWithoutProfesor
        var route = 
            "/CursoProfesor/Sedes/"+ $('select[name="sltSede"]').val() +"/Planes/"+ $('select[name="sltPlan"]').val() +"/Bloques/"+ $('select[name="sltBloque"]').val()+"/Cursos/"+ $('select[name="sltCurso"]').val() + "/GroupWithoutProfesor";
        $.getJSON(route, function (data) {
            var items = "";

            $.each(data, function (i, grupo) {
                items += "<option value='" + grupo.ID + "'>" + grupo.Number + "</option>";
            });

            /*for (var i = 0; i < data.length; i++) {
                items += "<option value='" + data[i]["ID"] + "'>" + data[i]["Curso"] + " - " + data[i]["Nombre"] + "</option>";
            }*/

            if (items != "") {
                $("#sltGrupo").html(items);
                $("#sltGrupo").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");
                $("#sltGrupo").prop("disabled", false);
            }
            else {
                $("#sltGrupo").html("<option>No hay grupos abiertos para ese curso.</option>")
            }
        });
    });

    $("#sltGrupo").change(function () {
        /*Obtiene la información del curso*/
        var route1 = "/CursoProfesor/Grupos/Info/" + $('select[name="sltGrupo"]').val();
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

        $('#HourCharge').prop("disabled", false);
        $("#txtHorasEstimadas").val(0)
        $.getJSON(route1, function (data) {
            cupo = data.Capacity;
            //aula = data[0]["Aula"];

            //alert("id es " + id);

            if (cupo != "" || aula != "" || id != "") {
                $("#txtCupo").val(cupo)
                
                //$("#txtAula").val(aula);
            }
            else {
                $("#txtCupo").val("No Disponible")
                $("#txtAula").val("No Disponible");
            }
            $('#txtHoras').val(data.TheoreticalHours)
            /* Obtener información del horario */
           // route2 = "/CursoProfesor/Horarios/Info/" + $('select[name="sltGrupo"]').val();

            /*************************/
            var curso = $('select[name="sltCurso"]').val();
            var sede = $('select[name="sltSede"]').val();
            var modalidad = $('select[name="sltModalidad"]').val();

            var plan = $('select[name="sltPlan"]').val();
            var grupo = $('select[name="sltGrupo"]').val();
            /* Obtener información del horario */
            route2 = "/CursoProfesor/Cursos/" + curso + "/Sedes/" + sede + "/Modalidades/" + modalidad + "/Planes/" + plan + "/Grupos/" + grupo + "/Horario"
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
                    /*Sin Horario Asignado*/
                    document.getElementById('table_schedule_group').innerHTML =
                        "<table>" +
                    "<thead>" +
                    "<th class=\"table_custom\">Dia</th>" +
                    "<th class=\"table_custom\">Hora Inicio</th>" +
                    "<th class=\"table_custom\">Hora Fin</th>" +
                    "<th class=\"table_custom\">Aula</th>" +
                    "</thead>" +
                    "<tbody>" +
                        "<tr>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                    "</tr>" +
                        "<tr>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                            "<td class=\"table_custom\">-</td>" +
                        "</tr>" +
                    "</tbody>" +
                    "</table>" +
            "</div>";
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

            var horas_estimadas = 10 - horas_teoricas;

            $("#txtHorasEstimadas").val(horas_estimadas);

        }

    });
});

function habilitarHoras() {

    $("#txtHoras").prop("disabled", false);
}