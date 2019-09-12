class ErrorPackage {

    constructor(params) {
        this.fila = params.fila;
        this.columna = params.columna;
        this.tipo_error = params.tipo_error;
        this.descripcion = params.descripcion;
    }

    ejecutar() {
        return ("<tr>".concat("<td>", this.fila, "</td>", "<td>", this.columna, "</td>", "<td>", this.tipo_error, "</td>", "<td>", this.descripcion, "</td>") + "</tr>");
    }

}