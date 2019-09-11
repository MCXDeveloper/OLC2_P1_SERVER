"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

const XDP = require('../arbol/DataPackage');
const XEP = require('../arbol/ErrorPackage');
const XMP = require('../arbol/MessagePackage');

class AST {

    constructor(ListaInstrucciones) {
        this.ListaInstrucciones = ListaInstrucciones;
    }

    ejecutar(callback) {

        let finalData = [];
        let finalMessages = "";
        let finalErrors = '<table class="table table-bordered"><thead><tr><th>Fila</th><th>Columna</th><th>Tipo</th><th>Descripcion</th></tr></thead><tbody>';

        this.ListaInstrucciones.forEach(obj => {

            if (obj instanceof XEP.ErrorPackage) {
                finalErrors += obj.ejecutar();
            }
            else if (obj instanceof XMP.MessagePackage) {
                finalMessages += obj.ejecutar();
            }
            else if (obj instanceof XDP.DataPackage) {
                finalData.push(obj.ejecutar());
            }

        });

        finalErrors = "</tbody></table>";

        callback(finalMessages, finalErrors, finalData);

    }

}

exports.AST = AST;