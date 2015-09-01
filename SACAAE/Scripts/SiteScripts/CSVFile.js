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
            });
        }
        alert("Se han realizado las asignaciones");
    }
});

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