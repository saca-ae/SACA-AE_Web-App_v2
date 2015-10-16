var tempIndex = 0;
$(document).ready(function () {
    $("#Projects").change(function () {
        clear_table();

        var table = document.getElementById('professor_assign_project').getElementsByTagName('tbody')[0];
        var route = "/Project/" + $('select[name="Projects"]').val() + "/Professors"
        $.getJSON(route, function (data) {
            //if (data.length != 0) {
            for (i = 0; i < data.length; i++) {
                var row = table.insertRow(table.rows.length);

                var name_profesor_cell = row.insertCell(0);
                var hour_cell = row.insertCell(1);
                var action_cell = row.insertCell(2);

                name_profesor_cell.innerHTML = data[i].Name;
                hour_cell.innerHTML = "<p style=\"margin-left:20px;\">" + data[i].Hours + "</p>";
                action_cell.innerHTML = "<a onclick=ver_detalle_proyecto(" + data[i].projectProfessorID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                        "<a onclick=editar_asignacion_proyecto(" + data[i].projectProfessorID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp";

            }
        });
    });
})

function clear_table() {
    var table = document.getElementById("professor_assign_project");
    var rowCount = table.rows.length;
    for (var x = rowCount - 1; x > 0; x--) {
        table.deleteRow(x);
    }
}

/// <summary>
///  Return view for a detail group according a id group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of Group to delete/param>
/// <returns>None</returns>
function ver_detalle_proyecto(vIDProyecto) {
    var route_temporal = "/Proyecto/DetalleAsignacion/" + vIDProyecto;
    window.location = route_temporal;
}

function editar_asignacion_proyecto(vIDProyecto) {
    var route_temporal = "/Proyecto/EditarAsignacion/" + vIDProyecto;
    window.location = route_temporal;
}
/// <summary>
///  Get the element select for delete
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of assign to delete/param>
/// <returns>None</returns>


function init_delete(vIDProfessor) {
    tempIndex = vIDProfessor;
}

/// <summary>
/// Remove profesor from a group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <returns> If not have problem, return message 'success' to a View, else return message 'Error'</returns>
function eliminar_asignacion_proyecto() {
    var route_temporal = "/Proyecto/" + $('select[name="Projects"]').val() + "/Professor/" + tempIndex + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {

            location.reload();

        }
        else {
            alert('Error');
        }
    });
}