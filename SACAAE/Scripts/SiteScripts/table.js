﻿/// <summary>
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
                    if (data[i].ID == data[i + 1].ID) {
                        tabla = tabla + "<tr>" +
                        "<td rowspan=\"2\">" + data[i].Number + "</td>" +
                        "<td rowspan=\"2\">" + data[i].Name + "</td>" +
                        "<td>" + data[i].StartHour + "</td>" +
                        "<td>" + data[i].EndHour + "</td>" +
                        "<td>" + data[i].Code + "</td>" +
                        "<td>" + data[i].Day + "</td>" +
                        "<td rowspan=\"2\">" +

                                "<a onclick=ver_detalle_grupo(" + data[i].ID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                "<a onclick=editar_asignacion_grupo(" + data[i].ID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                    "<a href=\"\"  onclick = init_delete(" + data[i].ID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                                // "<a onclick=eliminar_asignacion_grupo(" + data[i].ID + ") title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                            "</td> " +
                                "</tr>" +

                                "<tr>" +
                                    "<td>" + data[i + 1].StartHour + "</td>" +
                                    "<td>" + data[i + 1].EndHour + "</td>" +
                                    "<td>" + data[i + 1].Code + "</td>" +
                                    "<td>" + data[i + 1].Day + "</td>" +

                                "</tr>";
                        i++;
                    }
                    else {

                        tabla = tabla + "<tr>" +
                        "<td>" + data[i].Number + "</td>" +
                        "<td>" + data[i].Name + "</td>" +
                        "<td>" + data[i].StartHour + "</td>" +
                        "<td>" + data[i].EndHour + "</td>" +
                        "<td>" + data[i].Code + "</td>" +
                        "<td>" + data[i].Day + "</td>" +
                        "<td>" +
                            "<a onclick=ver_detalle_grupo(" + data[i].ID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                "<a title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                " <a title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
                            "</td> " +
                        "</tr>";

                    }
                }
                else {

                    tabla = tabla + "<tr>" +
                    "<td>" + data[i].Number + "</td>" +
                    "<td>" + data[i].Name + "</td>" +
                    "<td>" + data[i].StartHour + "</td>" +
                    "<td>" + data[i].EndHour + "</td>" +
                    "<td>" + data[i].Code + "</td>" +
                    "<td>" + data[i].Day + "</td>" +
                    "<td>" +
                            "<a onclick= ver_detalle_grupo(" + data[i].ID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                            "<a title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +

                            " <a title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>" +
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

};

/// <summary>
///  Function generate table for a list of a groups with the schedule of the group (Start Hour, End Hour and Day)
///  
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="route"> Ruta donde buscar la informacion</param>
/// <returns>None</returns>
function generate_table_schedule_group(route2) {

    var tabla = "<table id=\"profesores_asignados_a_curso\" >" +
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


        }
    });
}

generate_graphic_schedule(route)
{

}