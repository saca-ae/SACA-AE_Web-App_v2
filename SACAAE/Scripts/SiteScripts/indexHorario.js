$(function () {
    /* Deshabilitar componentes que no tienen datos cargados */
    $("#sltPlan").prop("disabled", "disabled");
    /* Funcion llamada cuando se cambien los valores de las sedes o las modalidades */
    $("#sltModalidad, #sltSede").change(function () {

        var route = "/Plans/Planes/List/" + $('select[name="sltSede"]').val() + "/" + $('select[name="sltModalidad"]').val();
        $.getJSON(route, function (data) {
            var items = "";
            $.each(data, function (i, plan) {
                items += "<option value= " + plan.ID + ">" + plan.Nombre + "</option>";
            });

            if (items != "") {
                $("#sltPlan").prop("disabled", false);
                $("#sltPlan").html(items);
                $("#sltPlan").prepend("<option value='' selected='selected'>-- Seleccionar Plan de Estudio --</option>");
            }
        });
    });
});

function setCookie(cname, cvalue, exdays) {

    document.cookie = cname + "=" + cvalue + ";path=/";
}

function Enviar() {
    if ($('#sltPeriodo :selected').val() == " ") {
        alert("ERROR: Seleccione un periodo");
        return false;
    }
    else
        if ($('#sltSede :selected').val() == " ") {
            alert("ERROR: Seleccione una sede");
            return false;
        }
        else
            if ($('#sltModalidad :selected').val() == " ") {
                alert("ERROR: Seleccione una modalidad");
                return false;
            }

            else
                if ($('#sltPlan :selected').val() == "") {
                    alert("ERROR: Seleccione un plan");
                    return false;
                }

    setCookie("PeriodoHorario", $('#sltPeriodo :selected').val(), 1);
    setCookie("SelPlanDeEstudio", $('#sltPlan :selected').val(), 1);
    setCookie("SelModalidad", $('#sltModalidad :selected').val(), 1);
    setCookie("SelSede", $('#sltSede :selected').val(), 1);
}