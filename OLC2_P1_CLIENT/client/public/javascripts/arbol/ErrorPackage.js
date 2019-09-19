class ErrorPackage {

    constructor(params) {
        this.fila = params.fila;
        this.columna = params.columna;
        this.ubicacion = params.ubicacion;
        this.tipo_error = params.tipo_error;
        this.descripcion = params.descripcion;
    }

    ejecutar() {
        return [String(this.tipo_error), String(this.ubicacion), String(this.descripcion), String(this.fila), String(this.columna)];
    }

}