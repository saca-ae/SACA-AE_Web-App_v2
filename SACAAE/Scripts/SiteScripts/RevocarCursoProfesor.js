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
});