$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltComisiones").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltComisiones = "<option>Seleccione Comision</option>";


    $("#sltComisiones").html(itemsltComisiones);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Profesores").change(function () {

        if ($('select[name="Profesores"]').val() == "") {

            $("#sltComisiones").prop("disabled", "disabled");
            itemsltComisiones = "<option>Seleccione Profesor</option>";
            $("#sltComisiones").html(itemsltComisiones);
            $("#Revocar").prop("disabled", true);

        } else {

            var route = "/ComisionProfesor/Profesor/Comisiones/" + $('select[name="Profesores"]').val();
            $.getJSON(route, function (data) {
                var items = "";
                for (var i = 0; i < data.length; i++) {
                    items += "<option value=" + data[i]["ID"] + ">" + data[i]["Name"] + "</option>"
                }

                if (items != "") {
                    $("#sltComisiones").html(items);
                    $("#sltComisiones").prepend("<option value='' selected='selected'>-- Seleccionar Comisión --</option>");
                    $("#sltComisiones").prop("disabled", false);
                    $("#Revocar").prop("disabled", false);
                }
                else {
                    $("#sltComisiones").html("<option>No hay comisiones a las que pertenezca el profesor.</option>");
                }
            });
        }
    });
});


/// <summary>
///  Get the element select for delete
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <param name="vIDGroup"> ID of Group to delete/param>
/// <returns>None</returns>
function init_delete() {
    //tempIndex = vIDGroup;
}


/// <summary>
/// Remove profesor from a group
/// </summary>
/// <autor> Esteban Segura Benavides </autor>
/// <returns> If not have problem, return message 'success' to a View, else return message 'Error'</returns>
function eliminar_asignacion_comision() {
    
    //ComisionProfesor/ComisionProfesor/{pIDComisionProfesor:int}/removeProfesor
    var route_temporal = "/ComisionProfesor/ComisionProfesor/" + $('select[name="sltComisiones"]').val() + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {
            
            location.reload();
        }
        else {
            alert('Error');
        }
    });
}