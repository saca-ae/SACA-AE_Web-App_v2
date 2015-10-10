$(document).ready(function () {
    var entidadid = "#" + getCookie("Entidad");
    if (entidadid == "#") { Load('TEC'); }
    $(entidadid).addClass('active');

    var route = "/Professor/Schedules/" + get_id(3);
    $.getJSON(route, function (data) {
        datos = datos_matriz(data);
        obtener_dias(datos);

    });
});

function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}

function datos_matriz(data) {
    console.log(data);
    var datos = [];
    var dato_unico;
    $.each(data, function (i, horario) {
        var nombre_curso = data[i].Name;
        var numero_curso = data[i].Number;
        var numero_dia = data[i].Day;

        var hora_entrada = data[i].StartHour;

        var hora_salida = data[i].EndHour;

        var diferencia = Math.ceil((Date.parse(hora_salida) - Date.parse(hora_entrada)) / 3600000);
        
        var bloque_inicio = getHourBlock(hora_entrada);

        numero_dia = numero_dia == "Lunes" ? 1 :
                     numero_dia == "Martes" ? 2 :
                     numero_dia == "Miércoles" ? 3 :
                     numero_dia == "Jueves" ? 4 :
                     numero_dia == "Viernes" ? 5 :
                     numero_dia == "Sábado" ? 6 :
                     0;

        console.log(numero_dia);

        dato_unico = [numero_dia, bloque_inicio, diferencia, nombre_curso, numero_curso]; console.log(dato_unico);//////////////////////////////////
        datos.push(dato_unico);
    });

    return datos;
}

function obtener_dias(data) {
    var cantidad_datos = data.length;
    var posicion_datos = 0;
    var body_table = "";

    var lunes = 0;
    var martes = 0;
    var miercoles = 0;
    var jueves = 0;
    var viernes = 0;
    var sabado = 0;

    for (i = 1; i < 16; i++) {
        var hora;
        switch (i) {
            case 1:
                var hora = '7:30 a 8:20';
                break;
            case 2:
                var hora = '8:30 a 9:20';
                break;
            case 3:
                var hora = '9:30 a 10:20';
                break;
            case 4:
                var hora = '10:30 a 11:20';
                break;
            case 5:
                var hora = '11:30 a 12:20';
                break;
            case 6:
                var hora = '12:30 a 12:50';
                break;
            case 7:
                var hora = '13:00 a 13:50';
                break;
            case 8:
                var hora = '14:00 a 14:50';
                break;
            case 9:
                var hora = '15:00 a 15:50';
                break;
            case 10:
                var hora = '16:00 a 16:50';
                break;
            case 11:
                var hora = '17:00 a 17:50';
                break;
            case 12:
                var hora = '18:00 a 18:50';
                break;
            case 13:
                var hora = '19:00 a 19:50';
                break;
            case 13:
                var hora = '20:00 a 20:50';
                break;
            case 14:
                var hora = '21:00 a 21:50';
                break;
        }
        body_table = body_table + "<tr>";
        var info_temp = "<th class=\"tabla_horario_th\">" + hora + "</th>";


        //1 Lunes
        if (cantidad_datos != 0)
        {
            if (lunes == 0) {
                if (data[posicion_datos][0] == 1 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td  id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\">" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    if (data[posicion_datos][2] > 1)
                        lunes = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                    "</td >";
                }
            }
            //2 Martes
            if (martes == 0) {
                if (data[posicion_datos][0] == 2 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\">" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    "</td >";
                    if (data[posicion_datos][2] > 1)
                        martes = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                    "</td >";
                }
            }
            //3 Miercoles
            if (miercoles == 0) {
                if (data[posicion_datos][0] == 3 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td  id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\" >" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    "</td >";
                    if (data[posicion_datos][2] > 1)
                        miercoles = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                    "</td >";
                }
            }
            //4 Jueves
            if (jueves == 0) {
                if (data[posicion_datos][0] == 4 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td  id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\">" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    "</td >";
                    if (data[posicion_datos][2] > 1)
                        jueves = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                     "</td >";
                }
            }
            //5 Viernes
            if (viernes == 0) {
                if (data[posicion_datos][0] == 5 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\">" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    "</td >";
                    if (data[posicion_datos][2] > 1)
                        viernes = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                    "</td >";
                }
            }
            //6 Sabado
            if (sabado == 0) {
                if (data[posicion_datos][0] == 6 && data[posicion_datos][1] == i) {
                    info_temp = info_temp + "<td id=\"seleccion_td\" rowSpan=\"" + data[posicion_datos][2] + "\" >" + data[posicion_datos][3] + "</br> Grupo: " +
                                                                                                 data[posicion_datos][4] + "</td >";
                    "</td >";
                    if (data[posicion_datos][2] > 1)
                        sabado = data[posicion_datos][2];

                    if (posicion_datos < (cantidad_datos - 1))
                        posicion_datos = posicion_datos + 1;
                }
                else {
                    info_temp = info_temp + "<td class=\"tabla_horario_td\" >" +
                    "</td >";
                }
            }
        }
        else
        {
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
            info_temp = info_temp + "<td class=\"tabla_horario_td\" ></td >";
        }
        info_temp = info_temp + "</tr>";
        body_table = body_table + info_temp;

        if (lunes != 0)
            lunes = lunes - 1;
        if (martes != 0)
            martes = martes - 1;
        if (miercoles != 0)
            miercoles = miercoles - 1;
        if (jueves != 0)
            jueves = jueves - 1
        if (viernes != 0)
            viernes = viernes - 1;
        if (sabado != 0)
            sabado = sabado - 1;

    }


    body_table = body_table + "</tbody></table>";
    document.getElementById('div_tabla').innerHTML = table_header() + body_table;
}

function getHourBlock(time) {
    switch (time) {
        case "07:30 am":
            return 1;
        case "08:30 am":
            return 2;
        case "09:30 am":
            return 3;
        case "10:30 am":
            return 4;
        case "11:30 am":
            return 5;
        case "12:30 pm":
            return 6;
        case "01:00 pm":
            return 7;
        case "02:00 pm":
            return 8;
        case "03:00 pm":
            return 9;
        case "04:00 pm":
            return 10;
        case "05:00 pm":
            return 11;
        case "06:00 pm":
            return 12;
        case "07:00 pm":
            return 13;
        case "08:00 pm":
            return 14;
        case "09:00 pm":
            return 15;
    }
}

function table_header() {
    var table_header = "<table id=\"tabla_horario\">" +
        "<thead id=\"tabla_horario_head\">" +
            "<tr \">" +
                "<td class = \"tabla_horario_td\" id=\"td_horario\">" +
                    "Horario" +
                "</td>" +
                "<td id=\"row1\">" +
                    "Lunes" +
                "</td>" +
                "<td id=\"row2\">" +
                    "Martes" +
                "</td>" +
                "<td id=\"row3\">" +
                    "Miercoles" +
                "</td>" +
                "<td id=\"row4\">" +
                    "Jueves" +
                "</td>" +
                "<td id=\"row5\">" +
                    "Viernes" +
                "</td>" +
                "<td id=\"row6\">" +
                    "Sabado" +
                "</td>" +
            "</tr>" +
        "</thead>";

    return table_header;

}