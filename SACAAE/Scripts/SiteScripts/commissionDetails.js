var tempIndex = 0;
window.onload = function () {
    var table = document.getElementById('professor_assign_commission').getElementsByTagName('tbody')[0];
    var route = "/Commission/" + get_id(3) + "/Professors"
    $.getJSON(route, function (data) {
        for (i = 0; i < data.length; i++) {
            var row = table.insertRow(table.rows.length);
            var name_profesor_cell = row.insertCell(0);
            var hour_cell = row.insertCell(1);
            var action_cell = row.insertCell(2);
            name_profesor_cell.innerHTML = data[i].Name;
            hour_cell.innerHTML = "<p style=\"margin-left:20px;\">" + data[i].Hours + "</p>";
            action_cell.innerHTML = "<a onclick=ver_detalle_comision(" + data[i].commissionProfessorID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                    "<a onclick=editar_asignacion_comision(" + data[i].commissionProfessorID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp" +
                                     "<a href=\"\"  onclick = init_delete(" + data[i].professorID + ") data-toggle=\"modal\" data-target=\"#deleteModal\" title=\"Eliminar\"><span class=\"glyphicon  glyphicon-trash\" aria-hidden=\"true\"></span></a>";
        }
    });
}

/// <summary>
///  Return view for a detail group according a id group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of Group to delete/param>
/// <returns>None</returns>
function ver_detalle_comision(vIDComision) {
    var route_temporal = "/Comision/DetalleAsignacion/" + vIDComision;
    window.location = route_temporal;
}

function editar_asignacion_comision(vIDComision) {
    var route_temporal = "/Comision/EditarAsignacion/" + vIDComision;
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
function eliminar_asignacion_commission() {
    var route_temporal = "/Commission/" + get_id(3) + "/Professor/" + tempIndex + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {

            location.reload();

        }
        else {
            alert('Error');
        }
    });
}


function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}