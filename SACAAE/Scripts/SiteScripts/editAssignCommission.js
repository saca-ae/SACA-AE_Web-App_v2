var tempIndex = 0;
$(document).ready(function () {
    $("#Commission").change(function () {
        
        clear_table();
        var table = document.getElementById('professor_assign_commission').getElementsByTagName('tbody')[0];
        
        var route = "/Commission/" + $('select[name="Commission"]').val() + "/Professors"
        $.getJSON(route, function (data) {
            if (data.length > 0) {
                
                for (i = 0; i < data.length; i++) {
                    var row = table.insertRow(table.rows.length);
                    var name_profesor_cell = row.insertCell(0);
                    var hour_cell = row.insertCell(1);
                    var action_cell = row.insertCell(2);
                    name_profesor_cell.innerHTML = data[i].Name;
                    hour_cell.innerHTML = "<p style=\"margin-left:20px;\">" + data[i].Hours + "</p>";
                    action_cell.innerHTML = "<a onclick=ver_detalle_comision(" + data[i].commissionProfessorID + ") title=\"Ver Detalle Curso\"><span class=\"glyphicon glyphicon-eye-open\" aria-hidden=\"true\"></span></a> &nbsp" +
                                            "<a onclick=editar_asignacion_comision(" + data[i].commissionProfessorID + ") title=\"Editar\"><span class=\"glyphicon glyphicon-pencil\" aria-hidden=\"true\"></span></a> &nbsp";
                }
            }
            
        });
    })
})

function clear_table()
{
    var table = document.getElementById("professor_assign_commission");
    var rowCount = table.rows.length;
    for (var x = rowCount - 1; x > 0; x--) {
        table.deleteRow(x);
    }
   /*var table = document.getElementById('professor_assign_commission');
    var tableRows = table.getElementsByTagName('tr');
    var rowCount = tableRows.length;
    alert(rowCount);
    for (var x = table.rows.length ; x > 0; x--) {
        table.deleteRow(x);
    }*/
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
    var route_temporal = "/Commission/" + $('select[name="Commission"]').val() + "/Professor/" + tempIndex + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {

            location.reload();

        }
        else {
            alert('Error');
        }
    });
}