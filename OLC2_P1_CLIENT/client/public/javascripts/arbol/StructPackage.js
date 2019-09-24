class StructPackage {

    constructor(dbsArray) {
        this.dbsArray = dbsArray;
    }

    ejecutar() {
        return "{ 'core' : { 'data' : [ 'CQL-TEACHER (SERVER)', { 'text' : 'BASES DE DATOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetDatabasesJSON(this.dbsArray) +" ] } ] } }";
    }

    GetDatabasesJSON(dbsArr) {

        let dbsJSON = [];

        dbsArr.forEach(element => {
            dbsJSON.push("{ 'text' : '" + element.name + "', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ { 'text' : 'TABLAS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetTablesJSON(element.lista_tablas) +" ] }, { 'text' : 'OBJETOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetObjectsJSON(element.lista_types) +" ] }, { 'text' : 'PROCEDIMIENTOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetProcsJSON(element.lista_procs) +" ] } ] }");
        });

        return dbsJSON.join(", ");

    }

    GetTablesJSON(tablesArray) {
        
        let tablesJSON = [];

        tablesArray.forEach(element => {
            tablesJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetColAtrJSON(element.lista_columnas) +" ] }");
        });

        return tablesJSON.join(", ");

    }

    GetObjectsJSON(objsArray) {

        let objsJSON = [];

        objsArray.forEach(element => {
            objsJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetColAtrJSON(element.lista_atributos) +" ] }");
        });

        return objsJSON.join(", ");

    }

    GetProcsJSON(procsArray) {
        
        let procsJSON = [];

        procsArray.forEach(element => {
            procsJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ ] }");
        });

        return procsJSON.join(", ");

    }

    GetColAtrJSON(array) {

        let _json_ = [];

        array.forEach(element => {
            _json_.push("{ 'text' : "+ element +" }");
        });

        return _json_.join(", ");

    }

}