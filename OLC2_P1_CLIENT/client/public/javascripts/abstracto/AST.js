class AST {

    constructor(ListaInstrucciones) {
        this.ListaInstrucciones = ListaInstrucciones;
    }

    ejecutar(callback) {

        let finalData = [];
        let finalLogin = "";
        let finalErrors = [];
        let finalMessages = "";
        let finalStructTree = "";

        finalErrors.push(["Tipo", "Ubicacion", "Descripcion", "Fila", "Columna"]);

        this.ListaInstrucciones.forEach(obj => {

            if (obj instanceof ErrorPackage) {
                finalErrors.push(obj.ejecutar());
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
            else if (obj instanceof StructPackage) {
                finalStructTree = obj.ejecutar();
            }

        });

        callback(finalMessages, finalErrors, finalData, finalLogin, finalStructTree);

    }

}