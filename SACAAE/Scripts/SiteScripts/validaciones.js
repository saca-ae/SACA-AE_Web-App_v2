function Validar() {
    if ($('#sltPeriodo :selected').val() == " ") {
        alert("ERROR: Seleccione un periodo");
        return false;
    }
    else
        if ($('#sltSede :selected').val() == " ") {
            alert("ERROR: Seleccione una sede");
            return false;
        }
        else
            if ($('#sltModalidad :selected').val() == " ") {
                alert("ERROR: Seleccione una modalidad");
                return false;
            }

            else
                if ($('#sltPlan :selected').val() == "") {
                    alert("ERROR: Seleccione un plan");
                    return false;
                }
    return true;
}