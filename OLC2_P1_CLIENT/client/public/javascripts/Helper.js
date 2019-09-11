class Helper {

    static GetErrorMessage(jqXHR, textStatus) {

        let message = "";

        if(jqXHR.status===0){
            message = 'No hay conexion con el servidor';
        }else if(jqXHR.status == 404){
            message = 'No se encontro la url';
        }else if(jqXHR.status == 500){
            message = 'Error de servidor';
        } else if (textStatus === 'parsererror') {
            message = 'Requested JSON parse failed.';
        } else if (textStatus === 'timeout') {
            message = 'Intentelo mas tarde hay un error de internet';
        } else if (textStatus === 'abort') {
            message = 'Ajax request aborted.';
        } else {
            message = 'Uncaught Error: ' + jqXHR.responseText;
        }

        return message;

    }

}