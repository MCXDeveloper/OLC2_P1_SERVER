class StructPackage {

    constructor(dbsArray) {
        this.dbsArray = dbsArray;
    }

    ejecutar() {
        return "{ 'core' : { 'data' : [ 'CQL-TEACHER (SERVER)', { 'text' : 'BASES DE DATOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetDatabasesJSON(this.dbsArray) +" ] } ] } }";
    }

    GetDatabasesJSON(dbsArr) {

        let dbsJSON = [];

        if (!(dbsArr === undefined || dbsArr.length == 0)) {
            
            dbsArr.forEach(element => {
                dbsJSON.push("{ 'text' : '" + element.name + "', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ { 'text' : 'TABLAS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetTablesJSON(element.lista_tablas) +" ] }, { 'text' : 'OBJETOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetObjectsJSON(element.lista_types) +" ] }, { 'text' : 'PROCEDIMIENTOS', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetProcsJSON(element.lista_procs) +" ] } ] }");
            });

            return dbsJSON.join(", ");

        }

        return "";

    }

    GetTablesJSON(tablesArray) {
        
        let tablesJSON = [];

        if (!(tablesArray === undefined || tablesArray.length == 0)) {
            
            tablesArray.forEach(element => {
                console.log(element.lista_columnas);
                tablesJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetColAtrJSON(element.lista_columnas) +" ] }");
            });

            return tablesJSON.join(", ");

        }

        return "";

    }

    GetObjectsJSON(objsArray) {

        let objsJSON = [];

        if (!(objsArray === undefined || objsArray.length == 0)) {
            
            objsArray.forEach(element => {
                objsJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ "+ this.GetColAtrJSON(element.lista_atributos) +" ] }");
            });

            return objsJSON.join(", ");

        }

        return "";

    }

    GetProcsJSON(procsArray) {
        
        let procsJSON = [];

        if (!(procsArray === undefined || procsArray.length == 0)) {
            
            procsArray.forEach(element => {
                procsJSON.push("{ 'text' : '"+ element.name +"', 'state' : { 'opened' : true, 'selected' : false }, 'children' : [ ] }");
            });

            return procsJSON.join(", ");

        }

        return "";

    }

    GetColAtrJSON(array) {

        let _json_ = [];

        array.forEach(element => {
            console.log("Elemento: ", element);
            _json_.push("{ 'text' : '"+ element +"' }");
        });

        return _json_.join(", ");

    }

}