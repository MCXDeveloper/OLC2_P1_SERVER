"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

class DataPackage {

    constructor(content) {
        this.content = content;
    }

    ejecutar() {
        return this.content;
    }

}

exports.DataPackage = DataPackage;