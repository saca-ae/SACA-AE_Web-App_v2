var contador = 2;
function obtener_dias() {
    var table = document.getElementById("schedule_comission");
    //var row_count = table.rows.length
    //alert(contador);
    if (contador < 6) {
        var row = table.insertRow(contador);
        row.setAttribute("id", "trRow" + contador);
        row.setAttribute("data-day", "Lunes");
        row.setAttribute("data-starthour", "7:30 AM");
        row.setAttribute("data-endhour", "8:20 AM");
        contador++;

        var celda_Day = row.insertCell(0);
        
        var celda_StarHour = row.insertCell(1);
        var celda_EndHour = row.insertCell(2);
        celda_dia.innerHTML = "<div class=\"campo_dia\">" +

                              "<select id=\"tdRow"+contador+"Day\">" +
                                  "<option><a href=\"#\">Lunes</a></option>" +
                                  "<option><a href=\"#\">Martes</a></option>" +
                                  "<option><a href=\"#\">Miercoles</a></option>" +
                                  "<option><a href=\"#\">Jueves</a></option>" +
                                  "<option><a href=\"#\">Viernes</a></option>" +
                                 " <option><a href=\"#\">Sabado</a></option>" +
                              "</select>" +

                          "</div>";
        celda_StarHour.innerHTML = "<div class=\"campo_inicio\">" +

                              "<select id=\"tdRow" + contador + "StartHour\">"+
                                  "<option>7:30 AM</option>" +
                                  "<option>8:30 AM</option>" +
                                  "<option>9:30 AM</option>" +
                                  "<option><a href=\"#\">10:30 am</a></option>" +
                                  "<option><a href=\"#\">11:30 am</a></option>" +
                                  "<option><a href=\"#\">12:30 pm</a></option>" +
                                  "<option><a href=\"#\">1:00 pm</a></option>"+
                                  "<option><a href=\"#\">2:00 pm</a></option>"+
                                  "<option><a href=\"#\">3:00 pm</a></option>"+
                                  "<option><a href=\"#\">4:00 pm</a></option>"+
                                  "<option><a href=\"#\">5:00 pm</a></option>"+
                                  "<option><a href=\"#\">6:00 pm</a></option>"+
                                  "<option><a href=\"#\">7:00 pm</a></option>"+
                                  "<option><a href=\"#\">8:00 pm</a></option>"+
                                  "<option><a href=\"#\">9:00 pm</a></option>"+
                              "</select>" +

                          "</div>";
        celda_EndHour.innerHTML = "<select id=\"tdRow" + contador + "EndHour\">"
                                  "<option>8:20 AM</option>" +
                                  "<option>9:20 AM</option>" +
                                  "<option>10:20 AM</option>" +
                                  "<option>11:20 AM</option>" +
                                  "<option>12:20 PM</option>" +
                                  "<option>1:50 PM</option>" +
                                  "<option>2:50 PM</option>" +
                                  "<option>3:50 PM</option>" +
                                  "<option>4:50 PM</option>" +
                                  "<option>5:50 PM</option>" +
                                  "<option>6:50 PM</option>" +
                                  "<option>7:50 PM</option>" +
                                  "<option>8:50 PM</option>" +
                                  "<option>9:50 PM</option>" +
                              "</select>";
    }


}

$('#tdRow1Day').change(function () 
{
    $('#trRow1').attr('data-day', $('#tdRow1Day').val());

})


$("#tdRow1StartHour").change(function () {
    $('#trRow1').attr('data-starthour', $('#tdRow1StartHour').val());
})

$("#tdRow1EndHour").change(function () {
    $('#trRow1').attr('data-endhour', $('#tdRow1EndHour').val());
})


$('#tdRow2Day').change(function () {
    $('#trRow2').attr('data-day', $('#tdRow2Day').val());

})


$("#tdRow2StartHour").change(function () {
    $('#trRow2').attr('data-starthour', $('#tdRow2StartHour').val());
})

$("#tdRow2EndHour").change(function () {
    $('#trRow2').attr('data-endhour', $('#tdRow2EndHour').val());
})
/*
$("#buttonPost").on("click", function () {
    var listName = "ScheduleComission";

    var qtd = 0;
    $("#schedule_comission > tbody > tr").each(function () {
        var nome = $(this).data("nome");
        var tel = $(this).data("tel");
        $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + qtd + "].Nome' value='" + nome + "'>");
        $("#formPost").prepend("<input type='hidden' name='" + listName + "[" + qtd + "].Telefone' value='" + tel + "'>");
        qtd += 1;
        alert(nome);
    });
});*/

