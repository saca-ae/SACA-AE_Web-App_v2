$("#btnLoadCSV").click(function () {
    var fileUpload = document.getElementById("fileInput");
    if (fileUpload.value != null) {
        var uploadFile = new FormData();
        var files = $("#fileInput").get(0).files;
        
        // Add the uploaded file content to the form data collection
        if (files.length > 0) {
            uploadFile.append("CsvDoc", files[0]);
            $.ajax({
                url: "/CSV/UploadCsvFile",
                contentType: false,
                processData: false,
                data: uploadFile,
                type: 'POST',
                success: function (data) {
                    console && console.log(data);
                    drawTable(data);
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    console && console.log("request failed");
                }
            });
        }
       
    }
});

function drawTable(data) {
    for (var i = 0; i < data.length; i++) {
        drawRow(data[i]);
    }
}

function drawRow(rowData) {
    var row = $("<tr />")
    $("#results").append(row);
    row.append($("<td>" + rowData.Grupo + "</td>"));
    row.append($("<td>" + rowData.Curso + "</td>"));
    row.append($("<td>" + rowData.Profesor + "</td>"));
    row.append($("<td>" + rowData.Dia + "</td>"));
    row.append($("<td>" + rowData.HInicio + "</td>"));
    row.append($("<td>" + rowData.HFinal + "</td>"));
    row.append($("<td>" + rowData.Aula + "</td>"));
    row.append($("<td>" + rowData.EstadoAsignacion + "</td>"));
    row.append($("<td>" + rowData.DetalleAsignacion + "</td>"));
}

$("#fileInput").change(function () {
    var selectedText = $("#fileInput").val();
    var extension = selectedText.split('.');
    if (extension[1] != "csv") {
        $("#msjFile").focus();
        $("#btnLoadCSV").attr("disabled", "disabled")
        alert("Por favor seleecione un archivo .csv");
        return;
    }
    $("#msjFile").val(selectedText);
    $("#btnLoadCSV").removeAttr("disabled")

});