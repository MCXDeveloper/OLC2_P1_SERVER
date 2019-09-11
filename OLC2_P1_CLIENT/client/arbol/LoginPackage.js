"use strict";
Object.defineProperty(exports, "__esModule", { value: true });

class LoginPackage {

    constructor(message) {
        this.message = message;
    }

    ejecutar() {
        return (this.message == "[SUCCESS]") ? true : false;
    }

}

exports.LoginPackage = LoginPackage;