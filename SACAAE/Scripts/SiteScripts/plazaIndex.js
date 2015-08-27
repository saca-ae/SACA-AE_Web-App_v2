var tempID = 0;

function initDelete(n) {
    $('#codeLabel').text("Codigo: " + $("#Code_" + n).val());
    tempID = n;
}

function deletePlaza() {
    var deleteRoute = '/Plazas/Delete/' + tempID;
    $('#plazaForm').attr('action', deleteRoute);
    $('#plazaForm').submit();
}