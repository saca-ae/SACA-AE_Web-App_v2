$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltProyectos").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltProyectos = "<option>Seleccione Profesor</option>";


    $("#sltProyectos").html(itemsltProyectos);


    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Profesores").change(function () {
        
        if ($('select[name="Profesores"]').val() == "") {

            $("#sltProyectos").prop("disabled", "disabled");
            itemsltProyectos = "<option>Seleccione Profesor</option>";
            $("#sltProyectos").html(itemsltProyectos);
            $("#Revocar").prop("disabled", true);

        } else {

            var route = "/ProyectoProfesor/Profesor/Proyecto/" + $('select[name="Profesores"]').val();
            $.getJSON(route, function (data) {
                var items = "";
                for (var i = 0; i < data.length; i++) {
                    items += "<option value=" + data[i]["ID"] + ">" + data[i]["Name"] + "</option>"
                }

                if (items != "") {
                    $("#sltProyectos").html(items);
                    $("#sltProyectos").prepend("<option value='' selected='selected'>-- Seleccionar Proyecto --</option>");
                    $("#sltProyectos").prop("disabled", false);
                    $("#Revocar").prop("disabled", false);
                }
                else {
                    $("#sltProyectos").html("<option>No hay proyectos a las que pertenezca el profesor.</option>");
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
function eliminar_asignacion_proyecto() {
    
    //ComisionProfesor/ComisionProfesor/{pIDComisionProfesor:int}/removeProfesor
    var route_temporal = "/ProyectoProfesor/ProyectoProfesor/" + $('select[name="sltProyectos"]').val() + "/removeProfesor";
    $.getJSON(route_temporal, function (data) {
        if (data.respuesta = 'success') {

            location.reload();
        }
        else {
            alert('Error');
        }
    });
}