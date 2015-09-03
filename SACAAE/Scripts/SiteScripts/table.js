/// <summary>
///  Function generate table for a list of a groups with number of group, hour, day and profesor
///  
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="route"> Ruta donde buscar la informacion</param>
/// <returns>None</returns>
function generate_table(route)
{
    var tabla = "<table id=\"profesores_asignados_a_curso\" class=\"table\">" +
        "<thead>" +
            "<th>Grupo</th>" +
            "<th>Nombre Profesor</th>" +
            "<th>Sede</th>"+
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
                        "<td rowspan=\"2\">" + data[i].GroupNumber + "</td>" +
                        "<td rowspan=\"2\">" + data[i].ProfessorName + "</td>" +
                      "<td>"+data[i].SedeName+"</td>"+
                        "<td>" + data[i].StartHour + "</td>" +
                        "<td>" + data[i].EndHour + "</td>" +
                        "<td>" + data[i].ClassroomCode + "</td>" +
                        "<td>" + data[i].Day + "</td>" +
                        "<td rowspan=\"2\">" +

                                "<a onclick=ver_detalle_grupo(" + data[i].GroupID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                "<a onclick=editar_asignacion_grupo(" + data[i].GroupID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                    "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                                // "<a onclick=eliminar_asignacion_grupo(" + data[i].ID + ") title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                            "</td> " +
                                "</tr>" +

                                "<tr>" +
                                    "<td>"+ data[i+1].SedeName+"</td>"+
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
                        "<td>" + data[i].ProfessorName + "</td>" +
                        "<td>" + data[i].SedeName + "</td>";
                        if (data[i].StartHour != null) {
                            tabla = tabla +"<td>" + data[i].StartHour + "</td>" +
                            "<td>" + data[i].EndHour + "</td>" +
                            "<td>" + data[i].ClassroomCode + "</td>" +
                            "<td>" + data[i].Day + "</td>";
                        }
                        else
                        {
                            tabla = tabla + "<td>-</td>" +
                            "<td>-</td>" +
                            "<td>-</td>" +
                            "<td>-</td>";
                        }
                        tabla = tabla + "<td>" +
                             "<a onclick=ver_detalle_grupo(" + data[i].GroupID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                "<a onclick=editar_asignacion_grupo(" + data[i].GroupID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                    "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                                // "<a onclick=eliminar_asignacion_grupo(" + data[i].ID + ") title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                            "</td> " +
                        "</tr>";

                    }
                }
                else {

                    tabla = tabla + "<tr>" +
                    "<td>" + data[i].GroupNumber + "</td>" +
                    "<td>" + data[i].ProfessorName + "</td>" +
                    "<td>" + data[i].SedeName + "</td>";
                    if (data[i].StartHour != null) {
                        tabla = tabla + "<td>" + data[i].StartHour + "</td>" +
                       "<td>" + data[i].EndHour + "</td>" +
                       "<td>" + data[i].ClassroomCode + "</td>" +
                       "<td>" + data[i].Day + "</td>";
                    }
                    else
                    {
                        tabla = tabla + "<td>-</td>" +
                                "<td>-</td>" +
                                "<td>-</td>" +
                                "<td>-</td>";
                    }
                    tabla = tabla + "<td>" +
                             "<a onclick=ver_detalle_grupo(" + data[i].GroupID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                "<a onclick=editar_asignacion_grupo(" + data[i].GroupID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                    "<a href=\"\"  onclick = init_delete(" + data[i].GroupID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                                // "<a onclick=eliminar_asignacion_grupo(" + data[i].ID + ") title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
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
                        "<td></td>" +
                        "<td></td> " +
                    "</tr>";

        }
    });

};

/// <summary>
///  Function generate table for a list of a groups with the schedule of the group (Start Hour, End Hour and Day)
///  
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="route"> Ruta donde buscar la informacion</param>
/// <returns>None</returns>
function generate_table_schedule_group(route2) {
    
    var tabla_head = "<table id=\"profesores_asignados_a_curso\" >" +
   "<thead>" +
       "<th>Dia</th>" +
       "<th>Hora Inicio</th>" +
       "<th>Hora Fin</th>" +
       "<th>Aula</th>" +
   "</thead>" +
   "<tbody>";
    $.getJSON(route2, function (data) {
        var tabla = "";
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

            document.getElementById('table_schedule_group').innerHTML = tabla_head+tabla;
        }
        else {
            var tabla = "<table id=\"profesores_asignados_a_curso\" >" +
               "<thead>" +
                   "<th>Dia</th>" +
                   "<th>Hora Inicio</th>" +
                   "<th>Hora Fin</th>" +
                   "<th>Aula</th>" +
               "</thead> " +
               "<tr>" +
              "<td>-</td>" +
                "<td>-</td>" +
               "<td>-</td>" +
                "<td>-</td>" +
                "</tr>"+
                "<tr>" +
              "<td>-</td>" +
                "<td>-</td>" +
               "<td>-</td>" +
                "<td>-</td>" +
                "</tr>";;
            tabla = tabla + "</tbody> </table>";
            document.getElementById('table_schedule_group').innerHTML = table_head+tabla;
        }
    });
}
