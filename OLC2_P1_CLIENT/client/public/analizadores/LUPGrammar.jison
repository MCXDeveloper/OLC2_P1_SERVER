/* Funciones utilizadas dentro del parser */
%{

    const path = require('path');
    let AST = require(path.resolve(__dirname, '../../abstracto/AST')).AST;
    let DataPackage = require(path.resolve(__dirname, '../../arbol/DataPackage')).DataPackage;
    let ErrorPackage = require(path.resolve(__dirname, '../../arbol/ErrorPackage')).ErrorPackage;
    let LoginPackage = require(path.resolve(__dirname, '../../arbol/LoginPackage')).LoginPackage;
    let MessagePackage = require(path.resolve(__dirname, '../../arbol/MessagePackage')).MessagePackage;

%}

/* Definición Léxica */
%lex

%options case-insensitive

%%

/* Espacios en blanco */
[ \r\t]+            {}
\n                  {}

\[\+ERROR\]                         return 'open_error';
\[\-ERROR\]                         return 'close_error';
\[\+LINE\](.*)\[\-LINE\]            return 'error_line';
\[\+COLUMN]\](.*)\[\-COLUMN]\]      return 'error_column';
\[\+TYPE]\](.*)\[\-TYPE]\]          return 'error_type';
\[\+DESC]\](.*)\[\-DESC]\]          return 'error_description';
\[\+DATA\](.*)\[\-DATA\]            return 'data_package';
\[\+MESSAGE\](.*)\[\-MESSAGE\]      return 'message_package';
\[\+LOGIN\](.*)\[\-LOGIN\]          return 'login_package';
<<EOF>>                             return 'eof';
.                                   { console.log('Error léxico: ' + yytext + ', en la linea: ' + yylloc.first_line + ', en la columna: ' + yylloc.first_column); }
/lex

%start INICIO

/* Definición de la gramática */
%%

INICIO
	: LISTA_INSTRUCCIONES eof                                                           { $$ = new AST($1); }
;

LISTA_INSTRUCCIONES
	: LISTA_INSTRUCCIONES INSTRUCCION                                                   { $$ = $1;  $$.push($2); }
	| INSTRUCCION                                                                       { $$ = [];  $$.push($1); }
	| error                                                                             { console.log('Error sintáctico: ' + yytext + ', en la linea: ' + this._$.first_line + ', en la columna: ' + this._$.first_column); }
;

INSTRUCCION
	: data_package                                                                      { $$ = new DataPackage($1.replace(/\[\+DATA]|\[\-DATA]/g, "")); }
	| message_package                                                                   { $$ = new MessagePackage($1.replace(/\[\+MESSAGE]|\[\-MESSAGE]/g, "")); }
	| login_package                                                                     { $$ = new LoginPackage($1.replace(/\[\+LOGIN]|\[\-LOGIN]/g, "")); }
	| open_error error_line error_column error_type error_description close_error       { $$ = new ErrorPackage({ fila: $2.replace(/\[\+LINE]|\[\-LINE]/g, ""), columna: $3.replace(/\[\+LINE]|\[\-LINE]/g, ""), tipo_error: $4.replace(/\[\+TYPE]|\[\-TYPE]/g, ""), descripcion: $5.replace(/\[\+DESC]|\[\-DESC]/g, "") }); }
;