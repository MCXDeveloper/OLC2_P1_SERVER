class AST {

    constructor(ListaInstrucciones) {
        this.ListaInstrucciones = ListaInstrucciones;
    }

    ejecutar(callback) {

        let finalData = [];
        let finalLogin = "";
        let finalMessages = "";
        let finalErrors = '<table class="table table-bordered"><thead><tr><th>Fila</th><th>Columna</th><th>Tipo</th><th>Descripcion</th></tr></thead><tbody>';

        this.ListaInstrucciones.forEach(obj => {

            if (obj instanceof ErrorPackage) {
                finalErrors += obj.ejecutar();
            }
            else if (obj instanceof MessagePackage) {
                finalMessages += obj.ejecutar();
            }
            else if (obj instanceof DataPackage) {
                finalData.push(obj.ejecutar());
            }
            else if (obj instanceof LoginPackage) {
                finalLogin = obj.ejecutar();
            }

        });

        finalErrors = "</tbody></table>";

        callback(finalMessages, finalErrors, finalData, finalLogin);

    }

}