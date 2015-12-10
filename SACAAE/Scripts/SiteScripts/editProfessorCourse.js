/***************************************************** Function use in EditarAsignacion.cshtml***************************************************/
window.onload = function () {
    var idGrupo = get_id(3);
    var idCurso = "";
    var idSede = "";
    var idModalidad = "";
    var idPlan = "";

    route = "/Cursos/Group/" + idGrupo;
    $.getJSON(route, function (data) {
        $.each(data, function (i, grupo) {
            idCurso = data[i].curso_id;
            idSede = data[i].sede_id;
            idModalidad = data[i].modalidad_id;
            idPlan = data[i].plan_id;
            document.getElementById('nombreCurso').innerHTML = "<p>" + data[i].curso_name + "</p>";
            document.getElementById('numeroGrupo').innerHTML = "<p>" + data[i].Number + "</p>";
            document.getElementById('sede').innerHTML = "<p>" + data[i].sede_name + "</p>";
            document.getElementById('modalidad').innerHTML = "<p>" + data[i].modalidad_name + "</p>";
            document.getElementById('plan').innerHTML = "<p>" + data[i].plan_name + "</p>";
            document.getElementById('bloque').innerHTML = "<p>" + data[i].descripcion_bloque + "</p>";
            TheoricalHour = data[i].TheoreticalHours;
            document.getElementById('horasTeoricas').innerHTML = "<p>" + data[i].TheoreticalHours + "</p>"
            document.getElementById('Profesores').value = data[i].profesor_id;
            var vEstimateHour = 0;
            if (data[i].asignacion_id == 1) {
                document.getElementById('editHourCharge').selectedIndex = 0;
                document.getElementById('divHorasEstimadas').innerHTML = "<p>" + vEstimateHour + "</p>"
            }
            else {
                vEstimateHour = 10 - data[i].TheoreticalHours
                document.getElementById('editHourCharge').selectedIndex = 1;
                document.getElementById('divHorasEstimadas').innerHTML = "<p>" + vEstimateHour + "</p>"
            }


        });
        route2 = "/CursoProfesor/Cursos/" + idCurso + "/Sedes/" + idSede + "/Modalidades/" + idModalidad + "/Planes/" + idPlan + "/Grupos/" + idGrupo + "/Horario"
        generate_table_schedule_group(route2);
    })


}

function changeHourCharge() {
    var HourSelection = document.getElementById("editHourCharge").value;

    if (HourSelection == '1' || HourSelection == '2') {
        document.getElementById("divHorasEstimadas").innerHTML = "<p>0</p>"
    }
    else if (HourSelection == '3') {
        var horas_estimadas = 10 - TheoricalHour;

        document.getElementById("divHorasEstimadas").innerHTML = "<p>" + horas_estimadas + "</p>"
    }
};

function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}