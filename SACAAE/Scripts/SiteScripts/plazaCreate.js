// <author> Adonis Mora Angulo </author>
// Disable submit button for default
document.getElementById("createBtn").disabled = true;
// Add events to validate inputs
document.getElementById("Code").oninput = function () { validate() };
document.getElementById("TotalHours").oninput = function () { validate() };
document.getElementById("EffectiveTime").oninput = function () { validate() };

function validate() {
    if (hoursValidate() && effectiveTimeValidate() && codeValidate())
        document.getElementById("createBtn").disabled = false;
    else
        document.getElementById("createBtn").disabled = true;
}

function codeValidate() {
    var code = $("#Code").val();
    if (code != "")
        return true;
    return false;
}

function hoursValidate () {
    var hours = $("#TotalHours").val();
    if (numberValidate(hours)) 
        if (hours <= 40) 
            return true;
    return false;
};

function effectiveTimeValidate() {
    var hours = $("#EffectiveTime").val();
    if (numberValidate(hours)) 
        return true;
    return false;
};

// Regex for positive number
function numberValidate(num) {
    var regex = /^[0-9]+$/;
    if (regex.test(num)) {
        return true;
    }
    return false;
}
