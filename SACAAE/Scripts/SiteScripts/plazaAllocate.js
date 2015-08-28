var id = $("#ID").val();
var totalAllocate = $("#TotalAllocate").val();
var count = $("#professorCount").val();
var editRoute = '/Plazas/EditAllocate/' + id;
var deleteRoute = '/Plazas/DeleteAllocate/' + id;
var tempIndex = 0;

function initAdd(){
    var route = "/Plazas/Professors/List/" + id;
    $.getJSON(route, function (data) {
        var items = "";

        $.each(data, function (i, profesor) {
            items += "<option value='" + profesor.ID + "'>" + profesor.Name + "</option>";
        });

        if (items != "") {
            $("#newProfe").html(items);
            $("#newProfe").prepend("<option value='' selected='selected'>-- Seleccione Profesor --</option>");
            $("#newProfe").prop("disabled", false);
        }
        else {
            $("#newProfe").html("<option>No hay grupos abiertos para ese curso.</option>")
            $("#newProfe").prop("disabled", true);
        }
    });
}

function addProfessor() {
    var temp = $("#profesores").html();
    temp += "<input name='Professors["+count+"].ID' value='"+$("#newProfe").val()+"' type='hidden'>";
    temp += "<input name='Professors["+count+"].Name' value='"+getSelectedText("newProfe")+"' type='hidden'>";
    temp += "<input name='Professors["+count+"].Allocate' value='"+$("#addPercent").val()+"' type='hidden'>";
    $("#profesores").html(temp);
}

function initEdit(n){
    var max = 100 - totalAllocate + parseInt($("#Professors_" + n + "__Allocate").val());
    $('#editProfe').val($("#Professors_" + n + "__Name").val());
    $('#editPercent').val($("#Professors_" + n + "__Allocate").val());
    $('#editLabel').text("*Máximo " + max);
    tempIndex = n;
}

function editProfessor() {
    var temp = "";
    temp += "<input name='Professors[0].ID' value='"+$("#Professors_" + tempIndex + "__ID").val()+"' type='hidden'>";
    temp += "<input name='Professors[0].Name' value='"+$("#editProfe").val()+"' type='hidden'>";
    temp += "<input name='Professors[0].Allocate' value='"+$("#editPercent").val()+"' type='hidden'>";
    $("#profesores").html(temp);

    $('#allocateForm').attr('action', editRoute);
    $('#allocateForm').submit();
}

function initDelete(n){
    $('#deleteProfe').val($("#Professors_" + n + "__Name").val());
    $('#deletePercent').val($("#Professors_" + n + "__Allocate").val());
    tempIndex = n;
}

function deleteProfessor() {
    var temp = "";
    temp += "<input name='Professors[0].ID' value='"+$("#Professors_" + tempIndex + "__ID").val()+"' type='hidden'>";
    temp += "<input name='Professors[0].Name' value='"+$("#editProfe").val()+"' type='hidden'>";
    temp += "<input name='Professors[0].Allocate' value='"+$("#editPercent").val()+"' type='hidden'>";
    $("#profesores").html(temp);

    $('#allocateForm').attr('action', deleteRoute);
    $('#allocateForm').submit();
}

function addValidate(){
    var percent = $("#addPercent").val();
    var regex = /^[0-9]+$/;
    if(regex.test(percent)){
        if(percent <= (100 - totalAllocate)){
            document.getElementById("addBtn").disabled = false;
            return;
        }
    }
    document.getElementById("addBtn").disabled = true;
}

function editValidate(){
    var percent = $("#editPercent").val();
    var pa = parseInt($("#Professors_" + tempIndex + "__Allocate").val());
    var regex = /^[0-9]+$/;
    if(regex.test(percent)){
        if(percent <= (100 - totalAllocate + pa)){
            document.getElementById("editBtn").disabled = false;
            return;
        }
    }
    document.getElementById("editBtn").disabled = true;
}

function getSelectedText(elementId) {
    var elt = document.getElementById(elementId);

    if (elt.selectedIndex == -1)
        return null;

    return elt.options[elt.selectedIndex].text;
}