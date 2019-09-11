"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

class MessagePackage {

    constructor(message) {
        this.message = message;
    }

    ejecutar() {
        return this.message + "\n";
    }

}

exports.MessagePackage = MessagePackage;