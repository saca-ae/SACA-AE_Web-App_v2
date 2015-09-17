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
            i = i+1;
        });
    });
});

//Function validate the correct structure of StartHour and EndHour
function validar_hora_inicio_hora_fin()
{
    
}

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
        celda_Day.innerHTML = "<select id=\"tdRow" + (contador - 1) + "Day\" onchange=\"onchangeDaySelection("+(contador - 1) +")\">" +
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
                                  "<option>01:00 pm</option>"+
                                  "<option>02:00 pm</option>"+
                                  "<option>03:00 pm</option>"+
                                  "<option>04:00 pm</option>"+
                                  "<option>05:00 pm</option>"+
                                  "<option>06:00 pm</option>"+
                                  "<option>07:00 pm</option>"+
                                  "<option>08:00 pm</option>"+
                                  "<option>09:00 pm</option>"+
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


function onchangeDaySelection(identificador)
{
    $('#trRow'+identificador).attr('data-day', $('#tdRow'+identificador+'Day').val());

}

function onchangeStartHourSelection(identificador)
{
    var pStartHour = $('#tdRow' + identificador + 'StartHour').val();
    $('#trRow'+identificador).attr('data-starthour',pStartHour);
}

function onchangeEndHourSelection(identificador) {
    var vStartHour = $('#tdRow' + identificador + 'StartHour').val();
    var vEndHour = $('#tdRow' + identificador + 'EndHour').val();


    var int_hora_entrada;
    var int_hora_salida;
    switch (vStartHour)
    {
        case "07:30 am":
            int_hora_entrada = 730;
            break;
        case "08:30 am":
            int_hora_entrada = 830;
            break;
        case "09:30 am":
            int_hora_entrada = 930;
            break;
        case "10:30 am":
            int_hora_entrada = 1030;
            break;
        case "11:30 am":
            int_hora_entrada = 1130;
            break;
        case "12:30 pm":
            int_hora_entrada = 1230;
            break;

        case "01:00 pm":
            int_hora_entrada = 1300;
            break;
        case "02:00 pm":
            int_hora_entrada = 1400;
            break;

        case "03:00 pm":
            int_hora_entrada = 1500;
            break;
        case "04:00 pm":
            int_hora_entrada = 1600;
            break;

        case "05:00 pm":
            int_hora_entrada = 1700;
            break;
        case "06:00 pm":
            int_hora_entrada = 1800;
            break;

        case "07:00 pm":
            int_hora_entrada = 1900;
            break;
        case "08:00 pm":
            int_hora_entrada = 2000;
            break;

        case "09:00 pm":
            int_hora_entrada = 2100;
            break;
    }
    /*
    switch (vEndHour)
    {
        case "07:30 am":
            int_hora_entrada = 730;
            break;
        case "08:30 am":
            int_hora_entrada = 830;
            break;
        case "09:30 am":
            int_hora_entrada = 930;
            break;
        case "10:30 am":
            int_hora_entrada = 1030;
            break;
        case "11:30 am":
            int_hora_entrada = 1130;
            break;
        case "12:30 pm":
            int_hora_entrada = 1230;
            break;

        case "01:00 pm":
            int_hora_entrada = 1300;
            break;
        case "02:00 pm":
            int_hora_entrada = 1400;
            break;

        case "03:00 pm":
            int_hora_entrada = 1500;
            break;
        case "04:00 pm":
            int_hora_entrada = 1600;
            break;

        case "05:00 pm":
            int_hora_entrada = 1700;
            break;
        case "06:00 pm":
            int_hora_entrada = 1800;
            break;

        case "07:00 pm":
            int_hora_entrada = 1900;
            break;
        case "08:00 pm":
            int_hora_entrada = 2000;
            break;

        case "09:00 pm":
            int_hora_entrada = 2100;
            break;
    }
    */
    switch (vEndHour)
    {
        case "08:20 am":
            int_hora_salida = 820;
            break;
        case "09:20 am":
            int_hora_salida = 920;
            break;
        case "10:20 am":
            int_hora_salida = 1020;
            break;
        case "11:20 am":
            int_hora_salida = 1120;
            break;
        case "12:20 pm":
            int_hora_salida = 1220;
            break;

        case "01:50 pm":
            int_hora_salida = 1350;
            break;
        case "02:50 pm":
            int_hora_salida = 1450;
            break;

        case "03:50 pm":
            int_hora_salida = 1550;
            break;
        case "04:50 pm":
            int_hora_salida = 1650;
            break;

        case "05:50 pm":
            int_hora_salida = 1750;
            break;
        case "06:50 pm":
            int_hora_salida = 1850;
            break;

        case "07:50 pm":
            int_hora_salida = 1950;
            break;
        case "08:50 pm":
            int_hora_salida = 2050;
            break;

        case "09:50 pm":
            int_hora_salida = 2150;
            break;
    }

    //********************************************************************************
    if (int_hora_salida > int_hora_entrada) {
        $('#trRow' + identificador).attr('data-endhour', vEndHour);
    }
    else
    {
       // alert("Error");
    }
   
}

