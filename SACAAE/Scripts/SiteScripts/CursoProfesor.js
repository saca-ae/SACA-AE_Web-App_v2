$(document).ready(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltPlan").prop("disabled", "disabled");
    $("#sltCurso").prop("disabled", "disabled");
    $("#sltGrupo").prop("disabled", "disabled");
    $("#sltBloque").prop("disabled", "disabled");
    /* Se agregan los option por defecto a los select vacíos */
    itemSltPlan = "<option>Seleccione Sede y Modalidad</option>";
    itemSltBloque = "<option>Seleccione Sede, Modalidad y Plan de Estudio</option>";
    itemSltGrupo = "<option>Seleccione Sede, Modalidad, Plan de Estudio, Bloque y Curso</option>";
    itemSltCurso = "<option>Seleccione Sede, Modalidad, Plan de Estudio y Bloque</option>";

    $("#sltPlan").html(itemSltPlan);
    $("#sltCurso").html(itemSltCurso);
    $("#sltGrupo").html(itemSltGrupo);
    $("#sltBloque").html(itemSltBloque);

    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#Modalidades, #Sedes").change(function () {

        var route = "/CursoProfesor/Planes/List/" + $('select[name="Sedes"]').val() + "/" + $('select[name="Modalidades"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {
                items += "<option value= " + plan.ID + ">" + plan.Name + "</option>";
            });

            if (items != "") {
                $("#sltPlan").prop("disabled", false);
                $("#sltPlan").html(items);
                $("#sltPlan").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
            else {
                $("#sltPlan").html("<option>No hay planes para esa sede y modalidad</option>");
            }
        });
    });

    $("#sltPlan").change(function () {
        var route = "/CursoProfesor/Bloques/List/" + $('select[name="sltPlan"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, bloque) {
                items += "<option value= " + bloque.ID + ">" + bloque.Description + "</option>";
            });

            if (items != "") {
                $("#sltBloque").html(items);
                $("#sltBloque").prepend("<option value='' selected='selected'>-- Seleccionar Bloque --</option>");
                $("#sltBloque").prop("disabled", false);
            }
            else {
                $("#sltBloque").html("<option>No hay Bloque asignados para ese plan de estudio.</option>")
            }
        });

    });
    $("#sltBloque").change(function () {
        var route = "/CursoProfesor/Cursos/List/" + $('select[name="sltPlan"]').val() + "/" + $('select[name="sltBloque"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, curso) {
                items += "<option value= " + curso.ID + ">" + (curso.Code + " - " + curso.Name) + "</option>";
            });


            $("#sltCurso").prop("disabled", false);
            $("#sltGrupo").html("");
            $("#sltGrupo").prop("disabled", "disabled");
            $("#sltCurso").html(items);
            $("#sltCurso").prepend("<option value='' selected='selected'>-- Seleccionar Curso --</option>");

        });
    });

    $("#sltCurso").change(function () {
        var route = "/CursoProfesor/GruposSinProfesor/List/" + $('select[name="sltCurso"]').val() + "/" + $('select[name="sltPlan"]').val() + "/" + $('select[name="Sedes"]').val() + "/" + $('select[name="sltBloque"]').val();
        $.getJSON(route, function (data) {
            var items = "";

            $.each(data, function (i, grupo) {
                items += "<option value='" + grupo.ID + "'>" + grupo.Number + "</option>";
            });

            /*for (var i = 0; i < data.length; i++) {
                items += "<option value='" + data[i]["ID"] + "'>" + data[i]["Curso"] + " - " + data[i]["Nombre"] + "</option>";
            }*/

            if (items != "") {
                $("#sltGrupo").html(items);
                $("#sltGrupo").prepend("<option value='' selected='selected'>-- Seleccionar Grupo --</option>");
                $("#sltGrupo").prop("disabled", false);
            }
            else {
                $("#sltGrupo").html("<option>No hay grupos abiertos para ese curso.</option>")
            }
        });
    });

    $("#sltGrupo").change(function () {
        /*Obtiene la información del curso*/
        var route1 = "/CursoProfesor/Grupos/Info/" + $('select[name="sltGrupo"]').val();
        /*Obtiene la información del horario*/
        var route2;

        /*Items del select de horarios*/
        var items = "";

        /*Variables de info de curso*/
        var cupo = "";
        var aula = "";
        var id = "";

        /*Horas calculadas de acuerdo al horario*/
        var horas = 0.0;

        $.getJSON(route1, function (data) {
            cupo = data.Capacity;
            //aula = data[0]["Aula"];

            //alert("id es " + id);

            if (cupo != "" || aula != "" || id != "") {
                $("#txtCupo").val(cupo)
                //$("#txtAula").val(aula);
            }
            else {
                $("#txtCupo").val("No Disponible")
                $("#txtAula").val("No Disponible");
            }

            /* Obtener información del horario */
            route2 = "/CursoProfesor/Horarios/Info/" + $('select[name="sltGrupo"]').val();

            //$.getJSON(route2, function (data) {
            //    //alert(data.toSource());
            //    for (var i = 0; i < data.length; i++) {
            //        items += data[i]["Dia1"] + " " + data[i]["Hora_Inicio"] + " - " + data[i]["Hora_Fin"] + "\n";
            //        //horas += (parseInt(data[i]["Hora_Fin"], 10) - parseInt(data[i]["Hora_Inicio"], 10));
            //        var horafin = parseInt(data[i]["Hora_Fin"]);
            //        var horainicio = parseInt(data[i]["Hora_Inicio"]);

            //        horas += horafin - horainicio;
            //    }

            //    //alert(horas);
            //    horas = horas / 100;

            //    if (items != "") {
            //        $("#txtHorario").val(items);
            //        $("#Asignar").prop("disabled", false);
            //        $("#txtHoras").val(Math.ceil(horas));
            //    }
            //    else {
            //        $("#txtHorario").val("No hay horarios para ese curso.")
            //    }
            //});
        });
    });
});

function habilitarHoras() {

    $("#txtHoras").prop("disabled", false);
}