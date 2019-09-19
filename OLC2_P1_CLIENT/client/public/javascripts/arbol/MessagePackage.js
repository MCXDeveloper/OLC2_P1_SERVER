class MessagePackage {

    constructor(message) {
        this.message = message;
    }

    ejecutar() {
        return this.message.replace('\"', '"') + String.fromCharCode(13) + String.fromCharCode(10);
    }

}