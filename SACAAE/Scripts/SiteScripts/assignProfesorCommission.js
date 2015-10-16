$(document).ready(function () {


    $("#buttonPost").on("click", function () {
        var listName = "ScheduleCommission";

        var i = 0;
        $("#schedule_comission > tbody > tr").each(function () {
            var day = $(this).data("day");
            var starthour = $(this).data("starthour");
            var endhour = $(this).data("endhour");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].Day' value='" + day + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].StartHour' value='" + starthour + "'>");
            $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + i + "].EndHour' value='" + endhour + "'>");
            i = i + 1;
        });
    });
});
var contador = 2;
function obtener_dias() {
    var table = document.getElementById("schedule_comission");
    //var row_count = table.rows.length
    //alert(contador);
    if (contador < 11) {
        var row = table.insertRow(contador);
        row.setAttribute("id", "trRow" + contador);
        row.setAttribute("data-day", "Lunes");
        row.setAttribute("data-starthour", "07:30 am");
        row.setAttribute("data-endhour", "08:20 am");
        contador++;

        var celda_Day = row.insertCell(0);

        var celda_StarHour = row.insertCell(1);
        var celda_EndHour = row.insertCell(2);
        celda_Day.innerHTML = "<select id=\"tdRow" + (contador - 1) + "Day\" onchange=\"onchangeDaySelection(" + (contador - 1) + ")\">" +
                                  "<option>Lunes</option>" +
                                  "<option>Martes</option>" +
                                  "<option>Miercoles</option>" +
                                  "<option>Jueves</a></option>" +
                                  "<option><a href=\"#\">Viernes</a></option>" +
                                 " <option><a href=\"#\">Sabado</a></option>" +
                              "</select>";
        celda_StarHour.innerHTML = "<select id=\"tdRow" + (contador - 1) + "StartHour\" onchange=\"onchangeStartHourSelection(" + (contador - 1) + ")\">" +
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
        celda_EndHour.innerHTML = "<select id=\"tdRow" + (contador - 1) + "EndHour\" onchange=\"onchangeEndHourSelection(" + (contador - 1) + ")\">" +
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
    }


}


function onchangeDaySelection(identificador) {
    $('#trRow' + identificador).attr('data-day', $('#tdRow' + identificador + 'Day').val());

}

function onchangeStartHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    $('#trRow' + identificador).attr('data-starthour', vStartHour);
}

function onchangeEndHourSelection(identificador) {
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();

    $('#trRow' + identificador).attr('data-endhour', vEndHour);
  
}

function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}

