$(document).ready(function () {
    $("#Courses").change(function () {

        // default value of sede = 1 (Cartago)
        var route = "/CursoProfesor/Cursos/" + $('select[name="Courses"]').val() + "/1";
        //the function generate_table(route) is in the JS file table.js
        generate_table(route);

        route = "/CursoProfesor/Cursos/" + $('select[name="Courses"]').val() + "/Sedes/";
        $.getJSON(route, function (data) {
            var items = "";


            $.each(data, function (i, sede) {
                if (sede.ID == 1)
                    items += "<option selected='selected' value='" + sede.ID + "'>" + sede.Name + "</option>";
                else
                    items += "<option  value='" + sede.ID + "'>" + sede.Name + "</option>";
            });


            if (items != "") {
                $("#Sedes").html(items);

            }
            else {
                $("#table_information_group").html("<p>No hay sedes relacionadas al grupo.</p>")

            }
        });
    });

    $('#Sedes').change(function () {

        var vSede = document.getElementById('Sedes').value
        var vRoute = "/CursoProfesor/Cursos/" + $('select[name="Courses"]').val() + "/" + vSede;
        //the function generate_table(route) is in the JS file table.js
        generate_table(vRoute);
    });
});


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
            var route = "/CursoProfesor/Cursos/" + $('select[name="Courses"]').val();
            document.getElementById('table_information_group').innerHTML = "";
            generate_table(route);
            location.reload();
        }
        else {
            alert('Error');
        }
    });
}

function editar_asignacion_grupo(id_grupo) {

    var route_temporal = "/Curso/EditarAsignacion/" + id_grupo;
    window.location = route_temporal;
}