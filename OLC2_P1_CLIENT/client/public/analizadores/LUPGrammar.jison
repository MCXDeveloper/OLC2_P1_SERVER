/* Definición Léxica */
%lex

%options case-insensitive

%%

/* Espacios en blanco */
[ \r\t]+            {}
\n                  {}

\[\+ERROR\]                         	return 'open_error';
\[\-ERROR\]                         	return 'close_error';
\[\+DATABASES\]                     	return 'open_databases';
\[\-DATABASES\]                     	return 'close_databases';
\[\+DATABASE\]                     		return 'open_database';
\[\-DATABASE\]                     		return 'close_database';
\[\+TABLES\]                     		return 'open_tables';
\[\-TABLES\]                     		return 'close_tables';
\[\+TABLE\]                     		return 'open_table';
\[\-TABLE\]                     		return 'close_table';
\[\+TYPES\]                     		return 'open_types';
\[\-TYPES\]                     		return 'close_types';
\[\+TYPE\]                     			return 'open_type';
\[\-TYPE\]                     			return 'close_type';
"[+NAME]"[^\[]*"[-NAME]"          		return 'name_package';
\[\+COLUMNS\](.*)\[\-COLUMNS\]          return 'columns_package';
\[\+ATTRIBUTES\](.*)\[\-ATTRIBUTES\]	return 'attributes_package';
\[\+PROCEDURES\](.*)\[\-PROCEDURES\]    return 'procedures_package';
\[\+LINE\](.*)\[\-LINE\]            	return 'error_line';
\[\+COLUMN]\](.*)\[\-COLUMN]\]      	return 'error_column';
\[\+LOCATION]\](.*)\[\-LOCATION]\]  	return 'error_location';
\[\+TYPE]\](.*)\[\-TYPE]\]          	return 'error_type';
\[\+DESC]\](.*)\[\-DESC]\]          	return 'error_description';
"[+DATA]"[^\[]*"[-DATA]"            	return 'data_package';
"[+MESSAGE]"[^\[]*"[-MESSAGE]"      	return 'message_package';
\[\+LOGIN\](.*)\[\-LOGIN\]          	return 'login_package';
\[\+LOGOUT\](.*)\[\-LOGOUT\]        	return 'logout_package';
<<EOF>>                             	return 'eof';
.                                   	{ console.log('Error léxico: ' + yytext + ', en la linea: ' + yylloc.first_line + ', en la columna: ' + yylloc.first_column); }
/lex

%start INICIO

/* Definición de la gramática */
%%

INICIO
	: LISTA_INSTRUCCIONES eof                                                                           { $$ = new AST($1); return $$; }	
;

LISTA_INSTRUCCIONES
	: LISTA_INSTRUCCIONES INSTRUCCION                                                                   { $$ = $1;  $$.push($2); }
	| INSTRUCCION                                                                                       { $$ = [];  $$.push($1); }
	| error                                                                                             { console.log('Error sintáctico: ' + yytext + ', en la linea: ' + this._$.first_line + ', en la columna: ' + this._$.first_column); }
;

INSTRUCCION
	: data_package                                                                                      { $$ = new DataPackage($1.replace(/\[\+DATA]|\[\-DATA]/g, "")); }
	| message_package                                                                                   { $$ = new MessagePackage($1.replace(/\[\+MESSAGE]|\[\-MESSAGE]/g, "")); }
	| login_package                                                                                     { $$ = new LoginPackage($1.replace(/\[\+LOGIN]|\[\-LOGIN]/g, "")); }
	| logout_package                                                                                    { $$ = new LoginPackage($1.replace(/\[\+LOGOUT]|\[\-LOGOUT]/g, "")); }
	| open_error error_line error_column error_type error_location error_description close_error        { $$ = new ErrorPackage({ fila: $2.replace(/\[\+LINE]|\[\-LINE]/g, ""), columna: $3.replace(/\[\+COLUMN]|\[\-COLUMN]/g, ""), tipo_error: $4.replace(/\[\+TYPE]|\[\-TYPE]/g, ""), ubicacion: $5.replace(/\[\+LOCATION]|\[\-LOCATION]/g, ""), descripcion: $6.replace(/\[\+DESC]|\[\-DESC]/g, "") }); }
	| open_databases LISTA_BDS													    					{ $$ = new StructPackage($2); }
;

LISTA_BDS
	: LISTA_BDS DATABASE																				{ $$ = $1;  $$.push($2); }
	| DATABASE																							{ $$ = [];  $$.push($1); }
	| close_databases																					{ $$ = []; }
;

DATABASE
	: open_database + name_package + TABLES_BLOCK + TYPES_BLOCK + LISTA_PROCEDURES + close_database		{ $$ = { name: $2, lista_tablas: $3, lista_types: $4, lista_procs: $5 }; }
;

TABLES_BLOCK
	: open_tables LISTA_TABLES close_tables																{ $$ = $2; }
	| %empty																							{ $$ = []; }
;

LISTA_TABLES
	: LISTA_TABLES open_table name_package LISTA_COLUMNAS close_table									{ $$ = $1;  $$.push({ name: $3.replace(/\[\+NAME]|\[\-NAME]/g, ""), lista_columnas: $4 }); }
	| open_table name_package LISTA_COLUMNAS close_table												{ $$ = [];  $$.push({ name: $2.replace(/\[\+NAME]|\[\-NAME]/g, ""), lista_columnas: $3 }); }
;

LISTA_COLUMNAS
	: LISTA_COLUMNAS columns_package																	{ $$ = $1;  $$.push($2.replace(/\[\+COLUMNS]|\[\-COLUMNS]/g, "")); }
	| columns_package																					{ $$ = [];  $$.push($1.replace(/\[\+COLUMNS]|\[\-COLUMNS]/g, "")); }
;

TYPES_BLOCK
	: open_types LISTA_TYPES close_types																{ $$ = $2; }
	| %empty																							{ $$ = []; }
;

LISTA_TYPES
	: LISTA_TYPES open_type name_package LISTA_ATRIBUTOS close_type										{ $$ = $1;  $$.push({ name: $3.replace(/\[\+NAME]|\[\-NAME]/g, ""), lista_atributos: $4 }); }
	| open_type name_package LISTA_ATRIBUTOS close_type													{ $$ = [];  $$.push({ name: $2.replace(/\[\+NAME]|\[\-NAME]/g, ""), lista_atributos: $3 }); }
;

LISTA_ATRIBUTOS
	: LISTA_ATRIBUTOS attributes_package																{ $$ = $1;  $$.push($2.replace(/\[\+ATTRIBUTES]|\[\-ATTRIBUTES]/g, "")); }
	| attributes_package																				{ $$ = [];  $$.push($1.replace(/\[\+ATTRIBUTES]|\[\-ATTRIBUTES]/g, "")); }
;

LISTA_PROCEDURES
	: LISTA_PROCEDURES procedures_package																{ $$ = $1;  $$.push($2.replace(/\[\+PROCEDURES]|\[\-PROCEDURES]/g, "")); }
	| procedures_package																				{ $$ = [];  $$.push($1.replace(/\[\+PROCEDURES]|\[\-PROCEDURES]/g, "")); }
;