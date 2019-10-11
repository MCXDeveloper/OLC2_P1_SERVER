using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

public class Grammar : Irony.Parsing.Grammar
{
    public Grammar() : base(false)
    {
        #region PALABRAS_RESERVADAS

        KeyTerm r_if = ToTerm("if");
        KeyTerm r_as = ToTerm("as");
        KeyTerm r_do = ToTerm("do");
        KeyTerm r_on = ToTerm("on");
        KeyTerm r_by = ToTerm("by");
        KeyTerm r_in = ToTerm("in");
        KeyTerm r_is = ToTerm("is");
        KeyTerm r_int = ToTerm("int");
        KeyTerm r_for = ToTerm("for");
        KeyTerm r_new = ToTerm("new");
        KeyTerm r_not = ToTerm("not");
        KeyTerm r_add = ToTerm("add");
        KeyTerm r_use = ToTerm("use");
        KeyTerm r_key = ToTerm("key");
        KeyTerm r_set = ToTerm("set");
        KeyTerm r_asc = ToTerm("asc");
        KeyTerm r_min = ToTerm("min");
        KeyTerm r_max = ToTerm("max");
        KeyTerm r_sum = ToTerm("sum");
        KeyTerm r_avg = ToTerm("avg");
        KeyTerm r_map = ToTerm("map");
        KeyTerm r_now = ToTerm("now");
        KeyTerm r_log = ToTerm("log");
        KeyTerm r_try = ToTerm("try");
        KeyTerm r_get = ToTerm("get");
        KeyTerm r_date = ToTerm("date");
        KeyTerm r_time = ToTerm("time");
        KeyTerm r_true = ToTerm("true");
        KeyTerm r_null = ToTerm("null");
        KeyTerm r_else = ToTerm("else");
        KeyTerm r_case = ToTerm("case");
        KeyTerm r_drop = ToTerm("drop");
        KeyTerm r_type = ToTerm("type");
        KeyTerm r_user = ToTerm("user");
        KeyTerm r_with = ToTerm("with");
        KeyTerm r_into = ToTerm("into");
        KeyTerm r_from = ToTerm("from");
        KeyTerm r_desc = ToTerm("desc");
        KeyTerm r_list = ToTerm("list");
        KeyTerm r_call = ToTerm("call");
        KeyTerm r_each = ToTerm("each");
        KeyTerm r_open = ToTerm("open");
        KeyTerm r_size = ToTerm("size");
        KeyTerm r_false = ToTerm("false");
        KeyTerm r_while = ToTerm("while");
        KeyTerm r_alter = ToTerm("alter");
        KeyTerm r_table = ToTerm("table");
        KeyTerm r_grant = ToTerm("grant");
        KeyTerm r_where = ToTerm("where");
        KeyTerm r_order = ToTerm("order");
        KeyTerm r_limit = ToTerm("limit");
        KeyTerm r_begin = ToTerm("begin");
        KeyTerm r_apply = ToTerm("apply");
        KeyTerm r_batch = ToTerm("batch");
        KeyTerm r_count = ToTerm("count");
        KeyTerm r_break = ToTerm("break");
        KeyTerm r_today = ToTerm("today");
        KeyTerm r_close = ToTerm("close");
        KeyTerm r_throw = ToTerm("throw");
        KeyTerm r_catch = ToTerm("catch");
        KeyTerm r_clear = ToTerm("clear");
        KeyTerm r_double = ToTerm("double");
        KeyTerm r_string = ToTerm("string");
        KeyTerm r_switch = ToTerm("switch");
        KeyTerm r_exists = ToTerm("exists");
        KeyTerm r_create = ToTerm("create");
        KeyTerm r_delete = ToTerm("delete");
        KeyTerm r_commit = ToTerm("commit");
        KeyTerm r_revoke = ToTerm("revoke");
        KeyTerm r_insert = ToTerm("insert");
        KeyTerm r_values = ToTerm("values");
        KeyTerm r_update = ToTerm("update");
        KeyTerm r_select = ToTerm("select");
        KeyTerm r_return = ToTerm("return");
        KeyTerm r_length = ToTerm("length");
        KeyTerm r_getday = ToTerm("getday");
        KeyTerm r_cursor = ToTerm("cursor");
        KeyTerm r_remove = ToTerm("remove");
        KeyTerm r_boolean = ToTerm("boolean");
        KeyTerm r_primary = ToTerm("primary");
        KeyTerm r_getyear = ToTerm("getyear");
        KeyTerm r_gethour = ToTerm("gethour");
        KeyTerm r_default = ToTerm("default");
        KeyTerm r_counter = ToTerm("counter");
        KeyTerm r_contains = ToTerm("contains");
        KeyTerm r_truncate = ToTerm("truncate");
        KeyTerm r_database = ToTerm("database");
        KeyTerm r_rollback = ToTerm("rollback");
        KeyTerm r_password = ToTerm("password");
        KeyTerm r_continue = ToTerm("continue");
        KeyTerm r_endswith = ToTerm("endswith");
        KeyTerm r_getmonth = ToTerm("getmonth");
        KeyTerm r_usertype = ToTerm("user_type");
        KeyTerm r_procedure = ToTerm("procedure");
        KeyTerm r_toupper = ToTerm("touppercase");
        KeyTerm r_tolower = ToTerm("tolowercase");
        KeyTerm r_substring = ToTerm("substring");
        KeyTerm r_startswith = ToTerm("startswith");
        KeyTerm r_getminutes = ToTerm("getminuts");
        KeyTerm r_getseconds = ToTerm("getseconds");
        KeyTerm r_bddontexists = ToTerm("bddontexists");
        KeyTerm r_typedontexists = ToTerm("typedontexists");
        KeyTerm r_usebdexception = ToTerm("usebdexception");
        KeyTerm r_userdontexists = ToTerm("userdontexists");
        KeyTerm r_batchexception = ToTerm("batchexception");
        KeyTerm r_bdalreadyexists = ToTerm("bdalreadyexists");
        KeyTerm r_tabledontexists = ToTerm("tabledontexists");
        KeyTerm r_valuesexception = ToTerm("valuesexception");
        KeyTerm r_columnexception = ToTerm("columnexception");
        KeyTerm r_typealreadyexists = ToTerm("typealreadyexists");
        KeyTerm r_useralreadyexists = ToTerm("useralreadyexists");
        KeyTerm r_indexoutexception = ToTerm("indexoutexception");
        KeyTerm r_tablealreadyexists = ToTerm("tablealreadyexists");
        KeyTerm r_arithmeticexception = ToTerm("arithmeticexception");
        KeyTerm r_objectalreadyexists = ToTerm("objectalreadyexists");
        KeyTerm r_countertypeexception = ToTerm("countertypeexception");
        KeyTerm r_nullpointerexception = ToTerm("nullpointerexception");
        KeyTerm r_functionalreadyexists = ToTerm("functionalreadyexists");
        KeyTerm r_numberreturnsexception = ToTerm("numberreturnsexception");
        KeyTerm r_procedurealreadyexists = ToTerm("procedurealreadyexists");

        MarkReservedWords(
            "if",
            "as",
            "do",
            "on",
            "by",
            "in",
            "is",
            "int",
            "for",
            "new",
            "not",
            "add",
            "use",
            "key",
            "set",
            "asc",
            "min",
            "max",
            "sum",
            "avg",
            "map",
            "now",
            "log",
            "try",
            "get",
            "date",
            "time",
            "true",
            "null",
            "else",
            "case",
            "drop",
            "type",
            "user",
            "with",
            "into",
            "from",
            "desc",
            "list",
            "call",
            "each",
            "open",
            "size",
            "clear",
            "false",
            "while",
            "alter",
            "table",
            "grant",
            "where",
            "order",
            "limit",
            "begin",
            "apply",
            "batch",
            "count",
            "break",
            "today",
            "close",
            "throw",
            "catch",
            "remove",
            "double",
            "string",
            "switch",
            "exists",
            "create",
            "delete",
            "commit",
            "revoke",
            "insert",
            "values",
            "update",
            "select",
            "return",
            "length",
            "getday",
            "cursor",
            "boolean",
            "primary",
            "getyear",
            "gethour",
            "default",
            "counter",
            "contains",
            "truncate",
            "database",
            "rollback",
            "password",
            "continue",
            "endswith",
            "getmonth",
            "user_type",
            "procedure",
            "substring",
            "startswith",
            "getminuts",
            "getseconds",
            "touppercase",
            "tolowercase",
            "bddontexists",
            "typedontexists",
            "usebdexception",
            "userdontexists",
            "batchexception",
            "bdalreadyexists",
            "tabledontexists",
            "valuesexception",
            "columnexception",
            "typealreadyexists",
            "useralreadyexists",
            "indexoutexception",
            "tablealreadyexists",
            "arithmeticexception",
            "objectalreadyexists",
            "countertypeexception",
            "nullpointerexception",
            "functionalreadyexists",
            "procedurealreadyexists",
            "numberreturnsexception"
        );

        #endregion

        #region TERMINALES

        KeyTerm mas = ToTerm("+");
        KeyTerm por = ToTerm("*");
        KeyTerm div = ToTerm("/");
        KeyTerm mod = ToTerm("%");
        KeyTerm or = ToTerm("||");
        KeyTerm xor = ToTerm("^");
        KeyTerm not = ToTerm("!");
        KeyTerm pot = ToTerm("**");
        KeyTerm inc = ToTerm("++");
        KeyTerm dec = ToTerm("--");
        KeyTerm and = ToTerm("&&");
        KeyTerm coma = ToTerm(",");
        KeyTerm puco = ToTerm(";");
        KeyTerm menos = ToTerm("-");
        KeyTerm igual = ToTerm("=");
        KeyTerm xmas = ToTerm("+=");
        KeyTerm xpor = ToTerm("*=");
        KeyTerm xdiv = ToTerm("/=");
        KeyTerm menor = ToTerm("<");
        KeyTerm mayor = ToTerm(">");
        KeyTerm dospu = ToTerm(":");
        KeyTerm punto = ToTerm(".");
        KeyTerm par_a = ToTerm("(");
        KeyTerm par_c = ToTerm(")");
        KeyTerm cor_a = ToTerm("[");
        KeyTerm cor_c = ToTerm("]");
        KeyTerm xmenos = ToTerm("-=");
        KeyTerm llave_a = ToTerm("{");
        KeyTerm llave_c = ToTerm("}");
        KeyTerm igualdad = ToTerm("==");
        KeyTerm diferente = ToTerm("!=");
        KeyTerm menor_igual = ToTerm("<=");
        KeyTerm mayor_igual = ToTerm(">=");
        KeyTerm interrogacion = ToTerm("?");
        CommentTerminal COMENTARIO_MULTIPLE = new CommentTerminal("COMENTARIO_MULTIPLE", "/*", "*/");
        CommentTerminal COMENTARIO_DE_LINEA = new CommentTerminal("COMENTARIO_DE_LINEA", "//", "\n", "\r\n");

        RegisterOperators(1, Associativity.Right, igual);
        RegisterOperators(2, Associativity.Right, interrogacion, dospu);
        RegisterOperators(3, Associativity.Left, or);
        RegisterOperators(4, Associativity.Left, and);
        RegisterOperators(5, Associativity.Left, xor);
        RegisterOperators(6, Associativity.Left, igualdad, diferente);
        RegisterOperators(7, Associativity.Neutral, mayor, menor, mayor_igual, menor_igual);
        RegisterOperators(8, Associativity.Left, mas, menos);
        RegisterOperators(9, Associativity.Left, por, div, mod, pot);
        RegisterOperators(10, Associativity.Right, not);
        RegisterOperators(11, Associativity.Neutral, inc, dec);
        RegisterOperators(12, Associativity.Neutral, par_a, par_c);

        NonGrammarTerminals.Add(COMENTARIO_DE_LINEA);
        NonGrammarTerminals.Add(COMENTARIO_MULTIPLE);

        #endregion

        #region EXPRESIONES_REGULARES

        NumberLiteral numero = new NumberLiteral("numero");
        IdentifierTerminal identificador = new IdentifierTerminal("identificador");
        StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);
        RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[a-zA-Z]([a-zA-Z]|_|[0-9]+)*");
        RegexBasedTerminal fecha = new RegexBasedTerminal("fecha", "'\\d{4}-((0\\d)|(1[012]))-(([012]\\d)|3[01])'");
        RegexBasedTerminal hora = new RegexBasedTerminal("hora", "'(0|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])'");

        #endregion

        #region NO_TERMINALES

        NonTerminal IF = new NonTerminal("IF");
        NonTerminal LOG = new NonTerminal("LOG");
        NonTerminal TIPO = new NonTerminal("TIPO");
        NonTerminal ELSE = new NonTerminal("ELSE");
        NonTerminal CASE = new NonTerminal("CASE");
        NonTerminal ORDER = new NonTerminal("ORDER");
        NonTerminal INICIO = new NonTerminal("INICIO");
        NonTerminal ACCESO = new NonTerminal("ACCESO");
        NonTerminal ELSE_IF = new NonTerminal("ELSE_IF");
        NonTerminal ATR_MAP = new NonTerminal("ATR_MAP");
        NonTerminal COLUMNA = new NonTerminal("COLUMNA");
        NonTerminal ATR_TYPE = new NonTerminal("ATR_TYPE");
        NonTerminal EXPRESION = new NonTerminal("EXPRESION");
        NonTerminal PRIMITIVO = new NonTerminal("PRIMITIVO");
        NonTerminal USER_TYPE = new NonTerminal("USER_TYPE");
        NonTerminal PARAMETRO = new NonTerminal("PARAMETRO");
        NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
        NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
        NonTerminal DECLARACION = new NonTerminal("DECLARACION");
        NonTerminal LISTA_CASES = new NonTerminal("LISTA_CASES");
        NonTerminal CREATE_TYPE = new NonTerminal("CREATE_TYPE");
        NonTerminal LISTA_ORDER = new NonTerminal("LISTA_ORDER");
        NonTerminal SENTENCIA_IF = new NonTerminal("SENTENCIA_IF");
        NonTerminal LISTA_ACCESO = new NonTerminal("LISTA_ACCESO");
        NonTerminal TIPO_FUN_AGG = new NonTerminal("TIPO_FUN_AGG");
        NonTerminal LISTA_ELSE_IF = new NonTerminal("LISTA_ELSE_IF");
        NonTerminal LISTA_ATR_MAP = new NonTerminal("LISTA_ATR_MAP");
        NonTerminal SENTENCIA_FOR = new NonTerminal("SENTENCIA_FOR");
        NonTerminal ACCION_CURSOR = new NonTerminal("ACCION_CURSOR");
        NonTerminal LISTA_ATR_TYPE = new NonTerminal("LISTA_ATR_TYPE");
        NonTerminal LISTA_COLUMNAS = new NonTerminal("LISTA_COLUMNAS");
        NonTerminal TIPO_EXCEPCION = new NonTerminal("TIPO_EXCEPCION");
        NonTerminal TIPO_ASIGNACION = new NonTerminal("TIPO_ASIGNACION");
        NonTerminal LISTA_VARIABLES = new NonTerminal("LISTA_VARIABLES");
        NonTerminal LLAMADA_FUNCION = new NonTerminal("LLAMADA_FUNCION");
        NonTerminal SENTENCIA_WHILE = new NonTerminal("SENTENCIA_WHILE");
        NonTerminal SENTENCIA_BREAK = new NonTerminal("SENTENCIA_BREAK");
        NonTerminal SENTENCIA_GRANT = new NonTerminal("SENTENCIA_GRANT");
        NonTerminal SENTENCIA_THROW = new NonTerminal("SENTENCIA_THROW");
        NonTerminal SENTENCIA_BATCH = new NonTerminal("SENTENCIA_BATCH");
        NonTerminal SENTENCIA_SWITCH = new NonTerminal("SENTENCIA_SWITCH");
        NonTerminal SENTENCIA_ACCESO = new NonTerminal("SENTENCIA_ACCESO");
        NonTerminal EXPRESION_LOGICA = new NonTerminal("EXPRESION_LOGICA");
        NonTerminal LISTA_PARAMETROS = new NonTerminal("LISTA_PARAMETROS");
        NonTerminal SENTENCIA_DB_USE = new NonTerminal("SENTENCIA_DB_USE");
        NonTerminal SENTENCIA_RETURN = new NonTerminal("SENTENCIA_RETURN");
        NonTerminal SENTENCIA_REVOKE = new NonTerminal("SENTENCIA_REVOKE");
        NonTerminal SENTENCIA_COMMIT = new NonTerminal("SENTENCIA_COMMIT");
        NonTerminal SENTENCIA_INC_DEC = new NonTerminal("SENTENCIA_INC_DEC");
        NonTerminal LISTA_EXPRESIONES = new NonTerminal("LISTA_EXPRESIONES");
        NonTerminal SENTENCIA_DB_DROP = new NonTerminal("SENTENCIA_DB_DROP");
        NonTerminal SENTENCIA_TB_DROP = new NonTerminal("SENTENCIA_TB_DROP");
        NonTerminal SENTENCIA_FOREACH = new NonTerminal("SENTENCIA_FOREACH");
        NonTerminal SENTENCIA_DO_WHILE = new NonTerminal("SENTENCIA_DO_WHILE");
        NonTerminal SENTENCIA_TB_ALTER = new NonTerminal("SENTENCIA_TB_ALTER");
        NonTerminal SENTENCIA_CONTINUE = new NonTerminal("SENTENCIA_CONTINUE");
        NonTerminal FUNCION_AGREGACION = new NonTerminal("FUNCION_AGREGACION");
        NonTerminal ASIGNACION_COLUMNA = new NonTerminal("ASIGNACION_COLUMNA");
        NonTerminal DECLARACION_CURSOR = new NonTerminal("DECLARACION_CURSOR");
        NonTerminal SENTENCIA_ROLLBACK = new NonTerminal("SENTENCIA_ROLLBACK");
        NonTerminal SENTENCIA_DB_CREATE = new NonTerminal("SENTENCIA_DB_CREATE");
        NonTerminal LISTA_INSTRUCCIONES = new NonTerminal("LISTA_INSTRUCCIONES");
        NonTerminal DECLARACION_FUNCION = new NonTerminal("DECLARACION_FUNCION");
        NonTerminal SENTENCIA_TB_CREATE = new NonTerminal("SENTENCIA_TB_CREATE");
        NonTerminal SENTENCIA_TB_INSERT = new NonTerminal("SENTENCIA_TB_INSERT");
        NonTerminal SENTENCIA_TB_SELECT = new NonTerminal("SENTENCIA_TB_SELECT");
        NonTerminal SENTENCIA_TB_DELETE = new NonTerminal("SENTENCIA_TB_DELETE");
        NonTerminal SENTENCIA_TRY_CATCH = new NonTerminal("SENTENCIA_TRY_CATCH");
        NonTerminal SENTENCIA_TB_UPDATE = new NonTerminal("SENTENCIA_TB_UPDATE");
        NonTerminal ASIGNACION_MULTIPLE = new NonTerminal("ASIGNACION_MULTIPLE");
        NonTerminal EXPRESION_ARITMETICA = new NonTerminal("EXPRESION_ARITMETICA");
        NonTerminal EXPRESION_RELACIONAL = new NonTerminal("EXPRESION_RELACIONAL");
        NonTerminal SENTENCIA_TB_TRUNCATE = new NonTerminal("SENTENCIA_TB_TRUNCATE");
        NonTerminal LLAMADA_PROCEDIMIENTO = new NonTerminal("LLAMADA_PROCEDIMIENTO");
        NonTerminal LISTA_IDENTIFICADORES = new NonTerminal("LISTA_IDENTIFICADORES");
        NonTerminal SENTENCIA_CREATE_USER = new NonTerminal("SENTENCIA_CREATE_USER");
        NonTerminal LISTA_ASIGNACION_COLUMNA = new NonTerminal("LISTA_ASIGNACION_COLUMNA");
        NonTerminal DECLARACION_PROCEDIMIENTO = new NonTerminal("DECLARACION_PROCEDIMIENTO");

        #endregion

        #region GRAMATICA

        INICIO.Rule = LISTA_INSTRUCCIONES;

        LISTA_INSTRUCCIONES.Rule = MakePlusRule(LISTA_INSTRUCCIONES, INSTRUCCION);

        INSTRUCCION.Rule = DECLARACION + puco
            //| DECLARACION_CURSOR + puco
            | SENTENCIA_ACCESO + puco
            | ASIGNACION_MULTIPLE + puco
            | ASIGNACION + puco
            | LOG
            | CREATE_TYPE + puco
            | SENTENCIA_IF
            | SENTENCIA_SWITCH
            | SENTENCIA_WHILE
            | SENTENCIA_FOR
            | SENTENCIA_DO_WHILE
            | SENTENCIA_INC_DEC + puco
            | DECLARACION_FUNCION
            | DECLARACION_PROCEDIMIENTO
            | ACCION_CURSOR + puco
            | LLAMADA_FUNCION + puco
            | LLAMADA_PROCEDIMIENTO + puco
            | SENTENCIA_DB_CREATE
            | SENTENCIA_TRY_CATCH
            | SENTENCIA_FOREACH
            | SENTENCIA_COMMIT + puco
            | SENTENCIA_ROLLBACK + puco
            | SENTENCIA_BATCH + puco
            | SENTENCIA_TB_UPDATE + puco
            | SENTENCIA_THROW + puco
            | SENTENCIA_DB_USE + puco
            | SENTENCIA_DB_DROP + puco
            | SENTENCIA_CREATE_USER + puco
            | SENTENCIA_GRANT + puco
            | SENTENCIA_REVOKE + puco
            | SENTENCIA_TB_CREATE + puco
            | SENTENCIA_TB_ALTER + puco
            | SENTENCIA_TB_DROP + puco
            | SENTENCIA_TB_TRUNCATE + puco
            | SENTENCIA_TB_INSERT + puco
            | SENTENCIA_TB_SELECT + puco
            | SENTENCIA_TB_DELETE + puco
            | SENTENCIA_BREAK + puco
            | SENTENCIA_RETURN + puco
            | SENTENCIA_CONTINUE + puco
            ;

        SENTENCIA_COMMIT.Rule = r_commit;

        SENTENCIA_ROLLBACK.Rule = r_rollback;

        SENTENCIA_BATCH.Rule = r_begin + r_batch + LISTA_INSTRUCCIONES + r_apply + r_batch;

        ASIGNACION_MULTIPLE.Rule = LISTA_VARIABLES + igual + LLAMADA_PROCEDIMIENTO;

        SENTENCIA_FOREACH.Rule = r_for + r_each + par_a + LISTA_PARAMETROS + par_c + r_in + variable + llave_a + LISTA_INSTRUCCIONES + llave_c;

        //DECLARACION_CURSOR.Rule = r_cursor + variable + r_is + SENTENCIA_TB_SELECT;

        ACCION_CURSOR.Rule = r_open + variable
            | r_close + variable
            ;

        SENTENCIA_TB_UPDATE.Rule = r_update + identificador + r_set + LISTA_ASIGNACION_COLUMNA
            | r_update + identificador + r_set + LISTA_ASIGNACION_COLUMNA + r_where + EXPRESION
            ;

        LISTA_ASIGNACION_COLUMNA.Rule = MakePlusRule(LISTA_ASIGNACION_COLUMNA, coma, ASIGNACION_COLUMNA);

        ASIGNACION_COLUMNA.Rule = identificador + TIPO_ASIGNACION + EXPRESION
            | identificador + cor_a + EXPRESION + cor_c + TIPO_ASIGNACION + EXPRESION
            | identificador + punto + LISTA_ACCESO + TIPO_ASIGNACION + EXPRESION
            ;

        SENTENCIA_THROW.Rule = r_throw + r_new + TIPO_EXCEPCION;

        SENTENCIA_TRY_CATCH.Rule = r_try + llave_a + LISTA_INSTRUCCIONES + llave_c + r_catch + par_a + TIPO_EXCEPCION + variable + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        SENTENCIA_GRANT.Rule = r_grant + identificador + r_on + identificador;

        SENTENCIA_REVOKE.Rule = r_revoke + identificador + r_on + identificador;

        SENTENCIA_CREATE_USER.Rule = r_create + r_user + identificador + r_with + r_password + cadena;

        SENTENCIA_TB_DELETE.Rule = r_delete + r_from + identificador
            | r_delete + r_from + identificador + r_where + EXPRESION
            | r_delete + identificador + cor_a + EXPRESION + cor_c + r_from + identificador
            | r_delete + identificador + cor_a + EXPRESION + cor_c + r_from + identificador + r_where + EXPRESION
            ;

        SENTENCIA_TB_SELECT.Rule = r_select + por + r_from + identificador
            | r_select + por + r_from + identificador + r_where + EXPRESION
            | r_select + por + r_from + identificador + r_limit + EXPRESION
            | r_select + por + r_from + identificador + r_order + r_by + LISTA_ORDER
            | r_select + por + r_from + identificador + r_where + EXPRESION + r_limit + EXPRESION
            | r_select + por + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER
            | r_select + por + r_from + identificador + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
            | r_select + por + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
            | r_select + LISTA_EXPRESIONES + r_from + identificador
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_limit + EXPRESION
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_order + r_by + LISTA_ORDER
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_limit + EXPRESION
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
            | r_select + LISTA_EXPRESIONES + r_from + identificador + r_where + EXPRESION + r_order + r_by + LISTA_ORDER + r_limit + EXPRESION
            ;

        SENTENCIA_BREAK.Rule = r_break;

        SENTENCIA_RETURN.Rule = r_return + LISTA_EXPRESIONES;

        SENTENCIA_CONTINUE.Rule = r_continue;

        LISTA_ORDER.Rule = MakePlusRule(LISTA_ORDER, coma, ORDER);

        ORDER.Rule = identificador + r_asc
            | identificador + r_desc
            | identificador
            ;

        SENTENCIA_TB_INSERT.Rule = r_insert + r_into + identificador + r_values + par_a + LISTA_EXPRESIONES + par_c
            | r_insert + r_into + identificador + par_a + LISTA_IDENTIFICADORES + par_c + r_values + par_a + LISTA_EXPRESIONES + par_c
            ;

        SENTENCIA_TB_TRUNCATE.Rule = r_truncate + r_table + identificador;

        SENTENCIA_TB_DROP.Rule = r_drop + r_table + identificador
            | r_drop + r_table + r_if + r_exists + identificador
            ;

        SENTENCIA_TB_ALTER.Rule = r_alter + r_table + identificador + r_add + LISTA_COLUMNAS
            | r_alter + r_table + identificador + r_drop + LISTA_IDENTIFICADORES
            ;

        SENTENCIA_TB_CREATE.Rule = r_create + r_table + identificador + par_a + LISTA_COLUMNAS + par_c
            | r_create + r_table + identificador + par_a + LISTA_COLUMNAS + coma + r_primary + r_key + par_a + LISTA_IDENTIFICADORES + par_c + par_c
            | r_create + r_table + r_if + r_not + r_exists + identificador + par_a + LISTA_COLUMNAS + par_c
            | r_create + r_table + r_if + r_not + r_exists + identificador + par_a + LISTA_COLUMNAS + coma + r_primary + r_key + par_a + LISTA_IDENTIFICADORES + par_c + par_c
            ;

        LISTA_COLUMNAS.Rule = MakePlusRule(LISTA_COLUMNAS, coma, COLUMNA);

        COLUMNA.Rule = identificador + TIPO
            | identificador + TIPO + r_primary + r_key
            ;

        LISTA_IDENTIFICADORES.Rule = MakePlusRule(LISTA_IDENTIFICADORES, coma, identificador);

        SENTENCIA_DB_CREATE.Rule = r_create + r_database + identificador + puco
            | r_create + r_database + r_if + r_not + r_exists + identificador + puco
            ;

        SENTENCIA_DB_USE.Rule = r_use + identificador;

        SENTENCIA_DB_DROP.Rule = r_drop + r_database + identificador;

        DECLARACION_PROCEDIMIENTO.Rule = r_procedure + identificador + par_a + LISTA_PARAMETROS + par_c + coma + par_a + LISTA_PARAMETROS + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        LLAMADA_PROCEDIMIENTO.Rule = r_call + identificador + par_a + LISTA_EXPRESIONES + par_c;

        LLAMADA_FUNCION.Rule = identificador + par_a + LISTA_EXPRESIONES + par_c
            | identificador + par_a + par_c
            ;

        DECLARACION_FUNCION.Rule = TIPO + identificador + par_a + LISTA_PARAMETROS + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        LISTA_PARAMETROS.Rule = MakeStarRule(LISTA_PARAMETROS, coma, PARAMETRO);

        PARAMETRO.Rule = TIPO + variable;

        CREATE_TYPE.Rule = r_create + r_type + identificador + par_a + LISTA_ATR_TYPE + par_c
            | r_create + r_type + r_if + r_not + r_exists + identificador + par_a + LISTA_ATR_TYPE + par_c
            ;

        LISTA_ATR_TYPE.Rule = MakePlusRule(LISTA_ATR_TYPE, coma, ATR_TYPE);

        ATR_TYPE.Rule = identificador + TIPO;

        DECLARACION.Rule = TIPO + LISTA_VARIABLES
            | TIPO + LISTA_VARIABLES + igual + EXPRESION
            | TIPO + LISTA_VARIABLES + r_is + SENTENCIA_TB_SELECT
            ;

        ASIGNACION.Rule = variable + TIPO_ASIGNACION + EXPRESION
            | variable + punto + LISTA_ACCESO + TIPO_ASIGNACION + EXPRESION
            ;

        LISTA_ATR_MAP.Rule = MakePlusRule(LISTA_ATR_MAP, coma, ATR_MAP);

        ATR_MAP.Rule = PRIMITIVO + dospu + EXPRESION;

        LISTA_VARIABLES.Rule = MakePlusRule(LISTA_VARIABLES, coma, variable);

        LISTA_ACCESO.Rule = MakePlusRule(LISTA_ACCESO, punto, ACCESO);

        ACCESO.Rule = r_insert + par_a + EXPRESION + coma + EXPRESION + par_c
            | r_set + par_a + EXPRESION + coma + EXPRESION + par_c
            | r_substring + par_a + EXPRESION + coma + EXPRESION + par_c
            | r_insert + par_a + EXPRESION + par_c
            | r_remove + par_a + EXPRESION + par_c
            | r_contains + par_a + EXPRESION + par_c
            | r_startswith + par_a + EXPRESION + par_c
            | r_endswith + par_a + EXPRESION + par_c
            | r_get + par_a + EXPRESION + par_c
            | r_size + par_a + par_c
            | r_clear + par_a + par_c
            | r_length + par_a + par_c
            | r_toupper + par_a + par_c
            | r_tolower + par_a + par_c
            | r_getyear + par_a + par_c
            | r_getmonth + par_a + par_c
            | r_getday + par_a + par_c
            | r_gethour + par_a + par_c
            | r_getminutes + par_a + par_c
            | r_getseconds + par_a + par_c
            | identificador
            ;

        LOG.Rule = r_log + par_a + EXPRESION + par_c + puco;

        SENTENCIA_WHILE.Rule = r_while + par_a + EXPRESION + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        SENTENCIA_DO_WHILE.Rule = r_do + llave_a + LISTA_INSTRUCCIONES + llave_c + r_while + par_a + EXPRESION + par_c + puco;

        SENTENCIA_FOR.Rule = r_for + par_a + DECLARACION + puco + EXPRESION + puco + ASIGNACION + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c
            | r_for + par_a + ASIGNACION + puco + EXPRESION + puco + ASIGNACION + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c
            | r_for + par_a + DECLARACION + puco + EXPRESION + puco + SENTENCIA_INC_DEC + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c
            | r_for + par_a + ASIGNACION + puco + EXPRESION + puco + SENTENCIA_INC_DEC + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c
            ;

        SENTENCIA_IF.Rule = IF
            | IF + LISTA_ELSE_IF
            | IF + LISTA_ELSE_IF + ELSE
            | IF + ELSE
            ;

        LISTA_ELSE_IF.Rule = MakePlusRule(LISTA_ELSE_IF, ELSE_IF);

        IF.Rule = r_if + par_a + EXPRESION + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        ELSE_IF.Rule = r_else + r_if + par_a + EXPRESION + par_c + llave_a + LISTA_INSTRUCCIONES + llave_c;

        ELSE.Rule = r_else + llave_a + LISTA_INSTRUCCIONES + llave_c;

        SENTENCIA_SWITCH.Rule = r_switch + par_a + EXPRESION + par_c + llave_a + LISTA_CASES + r_default + dospu + llave_a + LISTA_INSTRUCCIONES + llave_c + llave_c;

        LISTA_CASES.Rule = MakePlusRule(LISTA_CASES, CASE);

        CASE.Rule = r_case + EXPRESION + dospu + llave_a + LISTA_INSTRUCCIONES + llave_c;

        LISTA_EXPRESIONES.Rule = MakePlusRule(LISTA_EXPRESIONES, coma, EXPRESION);

        FUNCION_AGREGACION.Rule = TIPO_FUN_AGG + par_a + menor + menor + SENTENCIA_TB_SELECT + mayor + mayor + par_c;

        SENTENCIA_ACCESO.Rule = variable + punto + LISTA_ACCESO;

        SENTENCIA_INC_DEC.Rule = variable + inc
            | variable + dec
            | variable + punto + LISTA_ACCESO + inc
            | variable + punto + LISTA_ACCESO + dec
            ;

        EXPRESION.Rule = EXPRESION_ARITMETICA
            | EXPRESION_LOGICA
            | EXPRESION_RELACIONAL
            | PRIMITIVO
            | SENTENCIA_INC_DEC
            | LLAMADA_FUNCION
            | LLAMADA_PROCEDIMIENTO
            | FUNCION_AGREGACION
            | identificador
            | r_null
            | r_new + TIPO
            | r_today + par_a + par_c
            | r_now + par_a + par_c
            | SENTENCIA_ACCESO
            | identificador + punto + LISTA_ACCESO
            | llave_a + LISTA_ATR_MAP + llave_c
            | cor_a + LISTA_ATR_MAP + cor_c
            | llave_a + LISTA_EXPRESIONES + llave_c
            | cor_a + LISTA_EXPRESIONES + cor_c
            | par_a + EXPRESION + par_c
            | par_a + TIPO + par_c + EXPRESION
            | identificador + cor_a + EXPRESION + cor_c
            | llave_a + LISTA_EXPRESIONES + llave_c + r_as + identificador
            | EXPRESION + r_in + par_a + LISTA_EXPRESIONES + par_c
            | EXPRESION + r_in + EXPRESION
            | EXPRESION + interrogacion + EXPRESION + dospu + EXPRESION
            ;
        
        EXPRESION_ARITMETICA.Rule = menos + EXPRESION
            | EXPRESION + mas + EXPRESION
            | EXPRESION + menos + EXPRESION
            | EXPRESION + por + EXPRESION
            | EXPRESION + div + EXPRESION
            | EXPRESION + pot + EXPRESION
            | EXPRESION + mod + EXPRESION
            ;

        EXPRESION_RELACIONAL.Rule = EXPRESION + mayor + EXPRESION
            | EXPRESION + menor + EXPRESION
            | EXPRESION + mayor_igual + EXPRESION
            | EXPRESION + menor_igual + EXPRESION
            | EXPRESION + diferente + EXPRESION
            | EXPRESION + igualdad + EXPRESION
            ;

        EXPRESION_LOGICA.Rule = not + EXPRESION
            | EXPRESION + and + EXPRESION
            | EXPRESION + xor + EXPRESION
            | EXPRESION + or + EXPRESION
            ;

        PRIMITIVO.Rule = numero
            | cadena
            | fecha
            | hora
            | r_true
            | r_false
            | r_null
            | variable
            ;

        TIPO.Rule = r_int
            | r_double
            | r_string
            | r_boolean
            | r_date
            | r_time
            | r_counter
            | r_cursor
            | r_set + menor + TIPO + mayor
            | r_set
            | r_list + menor + TIPO + mayor
            | r_list
            | r_map + menor + TIPO + coma + TIPO + mayor
            | r_map
            | identificador
            ;

        TIPO_ASIGNACION.Rule = igual
            | xmas
            | xmenos
            | xpor
            | xdiv
            ;

        TIPO_FUN_AGG.Rule = r_count
            | r_max
            | r_min
            | r_sum
            | r_avg
            ;

        TIPO_EXCEPCION.Rule = r_typealreadyexists
            | r_typedontexists
            | r_bdalreadyexists
            | r_bddontexists
            | r_usebdexception
            | r_tablealreadyexists
            | r_tabledontexists
            | r_countertypeexception
            | r_useralreadyexists
            | r_userdontexists
            | r_valuesexception
            | r_columnexception
            | r_batchexception
            | r_indexoutexception
            | r_arithmeticexception
            | r_nullpointerexception
            | r_numberreturnsexception
            | r_functionalreadyexists
            | r_procedurealreadyexists
            | r_objectalreadyexists
            ;

        this.Root = INICIO;

        #endregion
    }
}