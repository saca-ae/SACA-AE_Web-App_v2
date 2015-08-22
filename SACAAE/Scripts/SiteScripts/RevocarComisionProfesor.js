$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltComisiones").prop("disabled", "disabled");

    /* Se agregan los option por defecto a los select vacíos */
    itemsltComisiones = "<option>Seleccione Profesor</option>";


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