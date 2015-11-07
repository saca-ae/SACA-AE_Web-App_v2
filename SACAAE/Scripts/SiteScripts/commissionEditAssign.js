var contador = 1;
var acumulador = 1;

function submit_form()
{
    //validate = true -> the table is correct
    var validate = validate_schedule();
    if (validate) {
        var listName = "ScheduleCommission";

        var i = 0;
        $("#assign_comission_schedule > tbody > tr").each(function () {
            var day = $(this).data("day");
            var starthour = $(this).data("starthour");
            var endhour = $(this).data("endhour");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].Day' value='" + day + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].StartHour' value='" + starthour + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].EndHour' value='" + endhour + "'>");
            i = i + 1;
        });
       $("#formPost").submit();
    }
    else {
        alert("Existe choque de horario en el horrario de asignacion")
    }
}

function validate_schedule()
{
    var table = document.getElementById("assign_comission_schedule");
    var rowCount = table.rows.length;
    var contador = 1;
    while (contador < rowCount)
    {
        var vDay = $('#trRow' + contador).attr('data-day');
        var vstrStartHour = $('#trRow' + contador).attr('data-starthour');
        var vstrEndHour = $('#trRow' + contador).attr('data-endhour');
        var vStartHour = new Date("1/1/2015 " + vstrStartHour);
        var vEndHour = new Date("1/1/2015 " + vstrEndHour);

        var problems = false;
        for (var i = contador; i < rowCount ; i++) {

            var vNextDay = "";
            var vstrNextStartHour = "";
            var vstrNextEndHour = "";
            if (i != rowCount-1) {
                vNextDay = $('#trRow' + (i + 1)).attr('data-day');
                vstrNextStartHour = $('#trRow' + (i + 1)).attr('data-starthour');
                vstrNextEndHour = $('#trRow' + (i + 1)).attr('data-endhour');

                var vNextStartHour = new Date("1/1/2015 " + vstrNextStartHour);
                var vNextEndHour = new Date("1/1/2015 " + vstrNextEndHour);
                
                if (vDay == vNextDay)
                {
                    if ((vStartHour <= vNextStartHour && vNextStartHour <= vEndHour) ||
                        (vStartHour <= vNextEndHour && vNextEndHour <= vEndHour) ||
                        (vNextStartHour <= vStartHour && vStartHour <= vNextEndHour) ||
                        (vNextStartHour <= vEndHour && vEndHour <= vNextEndHour)||
                        (vStartHour >= vNextStartHour && vNextStartHour >= vEndHour)||
                        (vStartHour >= vNextEndHour && vNextEndHour >= vEndHour)||
                        (vNextStartHour >= vStartHour && vStartHour >= vNextEndHour) ||
                        (vNextStartHour >= vEndHour && vEndHour >= vNextEndHour))
                    {
                        //is inconsistency in the schedule
                        return false;
                    }
                }
                break;
            }
        }
        contador++;
    }
    return true;

}


window.onload = function () {
    document.getElementById("Commissions").value = get_id(3);

    var professors = document.getElementById("Professors");
    var hourType = document.getElementById("HourCharge");
    var table = document.getElementById('assign_comission_schedule').getElementsByTagName('tbody')[0];
    var route = "/Comission/" + get_id(3) + "/Profesor"

    $.getJSON(route, function (data) {
        for (i = 0; i < data.length; i++) {
            professors.value = data[i].professorID;
            if (data[i].HourAllocatedTypeID == 1) {
                hourType.value = 1;
            }
            else {
                hourType.value = 0;
            }

        }
    });
    route = "/Commission/getScheduleProfesor/" + get_id(3)
    $.getJSON(route, function (data) {
        
        for (i = 0; i < data.length; i++) {
            var row = table.insertRow(table.rows.length);
            row.setAttribute("id", "trRow" + (acumulador));
            row.setAttribute("data-day", data[i].Day);
            row.setAttribute("data-starthour", data[i].StartHour);
            row.setAttribute("data-endhour", data[i].EndHour);
            var day_cell = row.insertCell(0);

            var start_cell = row.insertCell(1);

            var end_cell = row.insertCell(2);

            var delete_cell = row.insertCell(3);

            day_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "Day\" onchange=\"onchangeDaySelection(" + (acumulador) + ")\">" +
                                  "<option>Lunes</option>" +
                                  "<option>Martes</option>" +
                                  "<option>Miércoles</option>" +
                                  "<option>Jueves</option>" +
                                  "<option>Viernes</option>" +
                                 " <option>Sábado</option>" +
                              "</select>";
            $('#tdRow' + (acumulador) + 'Day option:contains(' + data[i].Day + ')').prop({ selected: true });

            start_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "StartHour\" onchange=\"onchangeStartHourSelection(" + (acumulador) + ")\">" +
                                  "<option>07:30 am</option>" +
                                  "<option>08:30 am</option>" +
                                  "<option>09:30 am</option>" +
                                  "<option>10:30 am</option>" +
                                  "<option>11:30 am</option>" +
                                  "<option>12:30 pm</option>" +
                                  "<option>01:00 pm</option>" +
                                  "<option>02:00 pm</option>" +
                                  "<option>03:00 pm</option>" +
                                  "<option>04:00 pm</option>" +
                                  "<option>05:00 pm</option>" +
                                  "<option>06:00 pm</option>" +
                                  "<option>07:00 pm</option>" +
                                  "<option>08:00 pm</option>" +
                                  "<option>09:00 pm</option>" +
                              "</select>";
            $('#tdRow' + (acumulador) + 'StartHour option:contains(' + data[i].StartHour + ')').prop({ selected: true });

            end_cell.innerHTML = "<select id=\"tdRow" + (acumulador) + "EndHour\" onchange=\"onchangeEndHourSelection(" + (acumulador) + ")\">" +
                                      "<option>08:20 am</option>" +
                                      "<option>09:20 am</option>" +
                                      "<option>10:20 am</option>" +
                                      "<option>11:20 am</option>" +
                                      "<option>12:20 pm</option>" +
                                      "<option>01:50 pm</option>" +
                                      "<option>02:50 pm</option>" +
                                      "<option>03:50 pm</option>" +
                                      "<option>04:50 pm</option>" +
                                      "<option>05:50 pm</option>" +
                                      "<option>06:50 pm</option>" +
                                      "<option>07:50 pm</option>" +
                                      "<option>08:50 pm</option>" +
                                      "<option>09:50 pm</option>" +
                                    "</select>";
            $('#tdRow' + (acumulador) + 'EndHour option:contains(' + data[i].EndHour + ')').prop({ selected: true });

            delete_cell.innerHTML = "<a  onclick = \"delete_row(" + (acumulador) + ")\" title=\"Eliminar\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span></a>";
            delete_cell.style.textAlign = "center";
            delete_cell.getElementsByTagName("a")[0].style.marginTop = "10px";

            contador += 1;
            acumulador += 1;

        }
    });
}

function delete_row(i)
{
    var table = document.getElementById("assign_comission_schedule");
    var trRow = table.getElementsByTagName("tr");
    var rowCount = table.rows.length;
    if (rowCount> 2) {
        
        
        for(var j=0;j<trRow.length;j++)
        {
            if (trRow[j].id == "trRow" + i) {
                table.deleteRow(j);
                contador -= 1;
            }
        }
    }
  
}




function obtener_dias() {
    var table = document.getElementById("assign_comission_schedule");
    //var row_count = table.rows.length
    //alert(contador);
    if (contador < 11) {
        var row = table.insertRow(contador);
        row.setAttribute("id", "trRow" + acumulador);
        row.setAttribute("data-day", "Lunes");
        row.setAttribute("data-starthour", "07:30 am");
        row.setAttribute("data-endhour", "08:20 am");

        contador += 1;

        var day_cell = row.insertCell(0);

        var starhour_cell = row.insertCell(1);
        var endhour_cell = row.insertCell(2);
        var delete_cell = row.insertCell(3);
        day_cell.innerHTML = "<select id=\"tdRow" + acumulador + "Day\" onchange=\"onchangeDaySelection(" + acumulador + ")\">" +
                                  "<option>Lunes</option>" +
                                  "<option>Martes</option>" +
                                  "<option>Miércoles</option>" +
                                  "<option>Jueves</option>" +
                                  "<option>Viernes</option>" +
                                 " <option>Sábado</option>" +
                              "</select>";
        starhour_cell.innerHTML = "<select id=\"tdRow" + acumulador + "StartHour\" onchange=\"onchangeStartHourSelection(" +acumulador + ")\">" +
                                  "<option>07:30 am</option>" +
                                  "<option>08:30 am</option>" +
                                  "<option>09:30 am</option>" +
                                  "<option>10:30 am</option>" +
                                  "<option>11:30 am</option>" +
                                  "<option>12:30 pm</option>" +
                                  "<option>01:00 pm</option>" +
                                  "<option>02:00 pm</option>" +
                                  "<option>03:00 pm</option>" +
                                  "<option>04:00 pm</option>" +
                                  "<option>05:00 pm</option>" +
                                  "<option>06:00 pm</option>" +
                                  "<option>07:00 pm</option>" +
                                  "<option>08:00 pm</option>" +
                                  "<option>09:00 pm</option>" +
                              "</select>";
        endhour_cell.innerHTML = "<select id=\"tdRow" + acumulador + "EndHour\" onchange=\"onchangeEndHourSelection(" + acumulador + ")\">" +
                                      "<option>08:20 am</option>" +
                                      "<option>09:20 am</option>" +
                                      "<option>10:20 am</option>" +
                                      "<option>11:20 am</option>" +
                                      "<option>12:20 pm</option>" +
                                      "<option>01:50 pm</option>" +
                                      "<option>02:50 pm</option>" +
                                      "<option>03:50 pm</option>" +
                                      "<option>04:50 pm</option>" +
                                      "<option>05:50 pm</option>" +
                                      "<option>06:50 pm</option>" +
                                      "<option>07:50 pm</option>" +
                                      "<option>08:50 pm</option>" +
                                      "<option>09:50 pm</option>" +
                                    "</select>";
        delete_cell.innerHTML = "<a  onclick = \"delete_row(" + acumulador + ")\" title=\"Eliminar\"><span class=\"glyphicon glyphicon-remove\" aria-hidden=\"true\"></span></a>";
        delete_cell.style.textAlign = "center";
        delete_cell.getElementsByTagName("a")[0].style.marginTop = "10px";

        acumulador += 1;
    }


}


function onchangeDaySelection(identificador) {
    $('#trRow' + identificador).attr('data-day', $('#tdRow' + identificador + 'Day').val());

}

function onchangeStartHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    var vLastStartHour = $('#trRow' + identificador).attr('data-starthour');
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();

    var date1 = new Date('1/1/2015 ' + vStartHour);
    var date2 = new Date('1/1/2015 ' + vEndHour);

    if (date1 < date2) {
        $('#trRow' + identificador).attr('data-starthour', vStartHour);
    }
    else
    {
        alert("Horario no permitido ");
        $('#tdRow' + identificador + 'StartHour option:contains(' + vLastStartHour + ')').prop({ selected: true });
    }
}

function onchangeEndHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    var vLastEndHour = $('#trRow' + identificador).attr('data-endhour');
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();

    var date1 = new Date('1/1/2015 ' + vStartHour);
    var date2 = new Date('1/1/2015 ' + vEndHour);

    if (date1 < date2) {
        $('#trRow' + identificador).attr('data-endhour', vEndHour);
    }
    else {
        alert("Horario no permitido ");
        $('#tdRow' + identificador + 'EndHour option:contains(' + vLastEndHour + ')').prop({ selected: true });

    }
}

function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}