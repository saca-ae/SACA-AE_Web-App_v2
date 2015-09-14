$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltCursosImpartidos").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltCursosImpartidos = "<option>Seleccione Profesor</option>";


    $("#sltCursosImpartidos").html(itemsltCursosImpartidos);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Profesores").change(function () {

        if ($('select[name="Profesores"]').val() == "") {

            $("#sltCursosImpartidos").prop("disabled", "disabled");
            itemsltCursosImpartidos = "<option>Seleccione Profesor</option>";
            $("#sltCursosImpartidos").html(itemsltCursosImpartidos);
            $("#Revocar").prop("disabled", true);

        } else {

            var route = "/CursoProfesor/Profesor/Cursos/" + $('select[name="Profesores"]').val();
            $.getJSON(route, function (data) {
                var items = "";
                for (var i = 0; i < data.length; i++) {
                    items += "<option value=" + data[i]["ID"] + ">" + data[i]["Code"] + " - " + data[i]["Name"] + "</option>"
                }
                if (items != "") {
                    $("#sltCursosImpartidos").html(items);
                    $("#sltCursosImpartidos").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");
                    $("#sltCursosImpartidos").prop("disabled", false);
                    $("#Revocar").prop("disabled", false);
                }
                else {
                    $("#sltCursosImpartidos").html("<option>No hay cursos impartidos por ese profesor.</option>");
                }
            });
        }
    });

    $("#sltCursosImpartidos").change(function () {
        //the function get_id(number) is in the JS file assignProfesorCourse.js, value of sede default = 1 (Cartago)
        var route = "/CursoProfesor/Cursos/" + $('select[name="sltCursosImpartidos"]').val() + "/Sedes/1/Profesores/" + $('select[name="Profesores"]').val();
        //the function generate_table(route) is in the JS file table.js
        /// <summary>
        ///  Function generate table for a list of a groups with number of group, hour, day and profesor
        ///  
        /// </summary>
        /// <autor> Esteban Segura Benavides </autor>
        /// <param name="route"> Ruta donde buscar la informacion</param>
        /// <returns>None</returns>
        generate_table(route);
            

   
    });


    
});

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
            var route = "/CursoProfesor/Cursos/" + $('select[name="sltCursosImpartidos"]').val() + "/Sedes/1/Profesores/" + $('select[name="Profesores"]').val();
           
            generate_table(route);
            location.reload();
        }
        else {
            alert('Error');
        }
    });
}

function generate_table(route)
{
    var tabla = "<table id=\"profesores_asignados_a_curso\" class=\"table\">" +
                "<thead>" +
                    "<th>Grupo</th>" +
                    "<th>Sede</th>" +
                    "<th>Inicio</th>" +
                    "<th>Fin</th>" +
                    "<th>Aula</th>" +
                    "<th>Dia</th>" +
                    "<th>Acciones</th>" +
                "</thead>" +
                "<tbody>";
    $.getJSON(route, function (data) {
        if (data.length != 0) {
            for (i = 0; i < data.length; i++) {
                if (data[i + 1] != undefined) {
                    if (data[i].GroupID == data[i + 1].GroupID) {
                        tabla = tabla + "<tr>" +
                        "<td rowspan=\"2\" style=\"vertical-align:middle\">" + data[i].GroupNumber + "</td>" +
                      "<td>" + data[i].SedeName + "</td>" +
                        "<td>" + data[i].StartHour + "</td>" +
                        "<td>" + data[i].EndHour + "</td>" +
                        "<td>" + data[i].ClassroomCode + "</td>" +
                        "<td>" + data[i].Day + "</td>" +
                        "<td rowspan=\"2\"  style=\"vertical-align:middle\">" +


                            "</td> " + "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\">Revocar</a>" +
                                "</tr>" +

                                "<tr>" +
                                    "<td>" + data[i + 1].SedeName + "</td>" +
                                    "<td>" + data[i + 1].StartHour + "</td>" +
                                    "<td>" + data[i + 1].EndHour + "</td>" +
                                    "<td>" + data[i + 1].ClassroomCode + "</td>" +
                                    "<td>" + data[i + 1].Day + "</td>" +

                                "</tr>";
                        i++;
                    }
                    else {

                        tabla = tabla + "<tr>" +
                        "<td>" + data[i].GroupNumber + "</td>" +
                        "<td>" + data[i].SedeName + "</td>";
                        if (data[i].StartHour != null) {
                            tabla = tabla + "<td>" + data[i].StartHour + "</td>" +
                            "<td>" + data[i].EndHour + "</td>" +
                            "<td>" + data[i].ClassroomCode + "</td>" +
                            "<td>" + data[i].Day + "</td>";
                        }
                        else {
                            tabla = tabla + "<td>-</td>" +
                            "<td>-</td>" +
                            "<td>-</td>" +
                            "<td>-</td>";
                        }
                        tabla = tabla + "<td>" +
                              "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\">Revocar</a>" +
                            "</td> " +
                        "</tr>";

                    }
                }
                else {

                    tabla = tabla + "<tr>" +
                    "<td>" + data[i].GroupNumber + "</td>" +
                    "<td>" + data[i].SedeName + "</td>";
                    if (data[i].StartHour != null) {
                        tabla = tabla + "<td>" + data[i].StartHour + "</td>" +
                       "<td>" + data[i].EndHour + "</td>" +
                       "<td>" + data[i].ClassroomCode + "</td>" +
                       "<td>" + data[i].Day + "</td>";
                    }
                    else {
                        tabla = tabla + "<td>-</td>" +
                                "<td>-</td>" +
                                "<td>-</td>" +
                                "<td>-</td>";
                    }
                    tabla = tabla + "<td>" +
                       "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\">Revocar</a>" +
                    "</td> " +
                "</tr>";

                }
            };
            tabla = tabla + "</tbody> </table>";

            document.getElementById('table_information_group').innerHTML = tabla;
        }
        else {
            //Return a empty table with 2 rows
            tabla = tabla +
                    "<tr>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td> " +
                    "</tr>" +
                "<tr>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td>" +
                        "<td></td> " +
                    "</tr>";

        }
    });
}