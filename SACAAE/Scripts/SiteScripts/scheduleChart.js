function get_id(number) {
    var url = window.location.pathname.split('/');
    return url[number];
}

function datos_matriz(data) {
    var datos = [];
    var dato_unico;
    $.each(data, function (i, horario) {
        var nombre_curso = data[i].Name;
        var numero_curso = data[i].Number;
        var numero_dia = data[i].Day;

        var hora_entrada = data[i].StartHour;

        var hora_salida = data[i].EndHour;

        var int_hora_entrada
        switch (hora_entrada) {
            case 1:
                int_hora_entrada = 730;
                break;
            case 2:
                int_hora_entrada = 830;
                break;
            case 3:
                int_hora_entrada = 930;
                break;
            case 4:
                int_hora_entrada = 1030;
                break;
            case 5:
                int_hora_entrada = 1130;
                break;
            case 6:
                int_hora_entrada = 1230;
                break;

            case 7:
                int_hora_entrada = 1300;
                break;
            case 8:
                int_hora_entrada = 1400;
                break;

            case 9:
                int_hora_entrada = 1500;
                break;
            case 10:
                int_hora_entrada = 1600;
                break;

            case 11:
                int_hora_entrada = 1700;
                break;
            case 12:
                int_hora_entrada = 1800;
                break;

            case 13:
                int_hora_entrada = 1900;
                break;
            case 14:
                int_hora_entrada = 2000;
                break;

            case 15:
                int_hora_entrada = 2100;
                break;
        }

        var int_hora_salida;
        switch (hora_salida) {
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

        var diferencia = (int_hora_salida - int_hora_entrada);

        if (int_hora_entrada < 1300) {
            diferencia = diferencia + 10;
        }
        else if (int_hora_entrada < 1300 & int_hora_salida > 1300) {
            diferencia = diferencia + 110;
        }
        else {
            diferencia = diferencia + 50;
        }
        var bloque_inicio;


        bloque_inicio = hora_entrada;
        /*switch (int_hora_entrada) {
            case 730:
                bloque_inicio = 1;
                break;
            case 830:
                bloque_inicio = 2;
                break;
            case 930:
                bloque_inicio = 3;
                break;
            case 1030:
                bloque_inicio = 4;
                break;
            case 1130:
                bloque_inicio = 5;
                break;
            case 1230:
                bloque_inicio = 6;
                break;
            case 1300:
                bloque_inicio = 7;
                break;
            case 1400:
                bloque_inicio = 8;
                break;
            case 1500:
                bloque_inicio = 9;
                break;
            case 1600:
                bloque_inicio = 10;
                break;
            case 1700:
                bloque_inicio = 11;
                break;
            case 1800:
                bloque_inicio = 12;
                break;
            case 1900:
                bloque_inicio = 13;
                break;
            case 2000:
                bloque_inicio = 14;
                break;
            case 2100:
                bloque_inicio = 15;
                break;
        }*/
        diferencia = diferencia / 100

        dato_unico = [numero_dia, bloque_inicio, diferencia, nombre_curso, numero_curso];
        datos.push(dato_unico);
    });

    return datos;
}
function load_window(id_aula) {

}

function obtener_dias(data) {
    var cantidad_datos = data.length;
    var posicion_datos = 0;
    var body_table = "";
    var header_table = ""
    var header_table = table_header();

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
        if (cantidad_datos != 0) {
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
        else {
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