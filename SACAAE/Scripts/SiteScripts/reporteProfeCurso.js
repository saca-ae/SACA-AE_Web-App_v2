
$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#Planes").prop("disabled", "disabled");
    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Modalidades, #Sedes").change(function () {
        if ($('select[name="Sedes"]').val() == "" || $('select[name="Modalidades"]').val() == "")
            return;

        var route = "/CursoProfesor/Planes/List/" + $('select[name="Sedes"]').val() + "/" + $('select[name="Modalidades"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {
                items += "<option value= " + plan.ID + ">" + plan.Name + "</option>";
            });

            if (items != "") {
                $("#Planes").prop("disabled", false);
                $("#Planes").html(items);
                $("#Planes").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
            else {
                $("#Planes").html("<option>No hay planes para esa sede y modalidad</option>");
            }
        });
    });

    $("#Planes").change(function () {

        var route = "/ReporteProfeCursoPlan/Plan/" + $('select[name="Planes"]').val() + "/Periodo/" + $('select[name="Periodos"]').val();
        //alert(route);

        $.getJSON(route, function (data) {
            var items = "<tr><th>Código</th><th>Nombre</th><th>Grupo</th><th>Curso Externo</th><th>Horario</th><th>Aula</th><th>Cupo</th><th>Profesor</th><th>Créditos</th></tr>";
            $.each(data, function (i, info) {
                var route2 = "/CursoProfesor/Horarios/Info/" + info.GrupoID;
                var horarioItems = "";
                $.getJSON(route2, function (datos) {
                    for (var k = 0; k < datos.length; k++) {
                        horarioItems += datos[k]["Dia"] + " " + datos[k]["Hora_Inicio"] + " - " + datos[k]["Hora_Fin"] + "<br/>";
                    }
                    items += "<tr><td>" + info.Code +
                                "</td><td>" + info.Name +
                                "</td><td>" + info.Number +
                                "</td><td>" + ((info.External) ? "Si" : "No") +
                                "</td><td>" + horarioItems +
                                "</td><td>" + info.Classroom +
                                "</td><td>" + ((info.Capacity == null) ? 0 : info.Capacity) +
                                "</td><td>" + ((info.Professor == null) ? "No asignado" : info.Professor) +
                                "</td><td>" + ((info.Credits == null) ? 0 : info.Credits) +
                                "</td></tr>";
                    $("#datosReporte").html(items);
                });
            });
            $("#datosReporte").html(items);
        });
    });

    $("#btnExport").click(function (e) {
        window.open('data:application/vnd.ms-excel;charset=utf-8,' + escape($('#dvData').html()));
        e.preventDefault();
    });
});