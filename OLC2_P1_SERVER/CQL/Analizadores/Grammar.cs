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
        KeyTerm r_do = ToTerm("do");
        KeyTerm r_on = ToTerm("on");
        KeyTerm r_by = ToTerm("by");
        KeyTerm r_in = ToTerm("in");
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
        KeyTerm r_getminutes = ToTerm("getminutes");
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
            "do",
            "on",
            "by",
            "in",
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
            "getminutes",
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
        RegisterOperators(2, Associativity.Right, interrogacion);
        RegisterOperators(3, Associativity.Left, or);
        RegisterOperators(4, Associativity.Left, and);
        RegisterOperators(5, Associativity.Left, xor);
        RegisterOperators(6, Associativity.Left, igualdad, diferente);
        RegisterOperators(7, Associativity.Neutral, mayor, menor, mayor_igual, menor_igual);
        RegisterOperators(8, Associativity.Left, mas, menos);
        RegisterOperators(9, Associativity.Left, por, div, mod);
        RegisterOperators(10, Associativity.Right, not);
        RegisterOperators(11, Associativity.Neutral, inc, dec);
        RegisterOperators(12, Associativity.Left, par_a, par_c);

        NonGrammarTerminals.Add(COMENTARIO_DE_LINEA);
        NonGrammarTerminals.Add(COMENTARIO_MULTIPLE);

        #endregion

        #region EXPRESIONES_REGULARES

        NumberLiteral numero = new NumberLiteral("numero");
        IdentifierTerminal identificador = new IdentifierTerminal("identificador");
        StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);

        RegexBasedTerminal decima = new RegexBasedTerminal("decima", "[0-9]+(.[0-9][0-9]?)?");
        RegexBasedTerminal variable = new RegexBasedTerminal("variable", "@[a-zA-Z]([a-zA-Z]|_|[0-9]+)*");
        RegexBasedTerminal fecha = new RegexBasedTerminal("fecha", "((0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-[12][0-9]{3})");
        RegexBasedTerminal hora = new RegexBasedTerminal("hora", "(00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])");

        #endregion

        #region NO_TERMINALES

        NonTerminal IF = new NonTerminal("IF");
        NonTerminal LOG = new NonTerminal("LOG");
        NonTerminal TIPO = new NonTerminal("TIPO");
        NonTerminal ELSE = new NonTerminal("ELSE");
        NonTerminal CASE = new NonTerminal("CASE");
        NonTerminal INICIO = new NonTerminal("INICIO");
        NonTerminal ACCESO = new NonTerminal("ACCESO");
        NonTerminal ELSE_IF = new NonTerminal("ELSE_IF");
        NonTerminal ATR_MAP = new NonTerminal("ATR_MAP");
        NonTerminal ATR_TYPE = new NonTerminal("ATR_TYPE");
        NonTerminal EXPRESION = new NonTerminal("EXPRESION");
        NonTerminal PRIMITIVO = new NonTerminal("PRIMITIVO");
        NonTerminal USER_TYPE = new NonTerminal("USER_TYPE");
        NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
        NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");
        NonTerminal DECLARACION = new NonTerminal("DECLARACION");
        NonTerminal LISTA_CASES = new NonTerminal("LISTA_CASES");
        NonTerminal CREATE_TYPE = new NonTerminal("CREATE_TYPE");
        NonTerminal SENTENCIA_IF = new NonTerminal("SENTENCIA_IF");
        NonTerminal LISTA_ACCESO = new NonTerminal("LISTA_ACCESO");
        NonTerminal LISTA_ELSE_IF = new NonTerminal("LISTA_ELSE_IF");
        NonTerminal LISTA_ATR_MAP = new NonTerminal("LISTA_ATR_MAP");
        NonTerminal LISTA_ATR_TYPE = new NonTerminal("LISTA_ATR_TYPE");
        NonTerminal TIPO_ASIGNACION = new NonTerminal("TIPO_ASIGNACION");
        NonTerminal LISTA_VARIABLES = new NonTerminal("LISTA_VARIABLES");
        NonTerminal SENTENCIA_SWITCH = new NonTerminal("SENTENCIA_SWITCH");
        NonTerminal EXPRESION_LOGICA = new NonTerminal("EXPRESION_LOGICA");
        NonTerminal SENTENCIA_INC_DEC = new NonTerminal("SENTENCIA_INC_DEC");
        NonTerminal LISTA_EXPRESIONES = new NonTerminal("LISTA_EXPRESIONES");
        NonTerminal LISTA_INSTRUCCIONES = new NonTerminal("LISTA_INSTRUCCIONES");
        NonTerminal EXPRESION_ARITMETICA = new NonTerminal("EXPRESION_ARITMETICA");
        NonTerminal EXPRESION_RELACIONAL = new NonTerminal("EXPRESION_RELACIONAL");
        
        #endregion

        #region GRAMATICA

        INICIO.Rule = LISTA_INSTRUCCIONES;

        LISTA_INSTRUCCIONES.Rule = MakePlusRule(LISTA_INSTRUCCIONES, INSTRUCCION);

        INSTRUCCION.Rule = DECLARACION
            | ASIGNACION
            | LOG
            | CREATE_TYPE
            | SENTENCIA_IF
            | SENTENCIA_SWITCH
            | SENTENCIA_INC_DEC
            ;

        CREATE_TYPE.Rule = r_create + r_type + identificador + par_a + LISTA_ATR_TYPE + par_c + puco
            | r_create + r_type + r_if + r_not + r_exists + identificador + par_a + LISTA_ATR_TYPE + par_c + puco
            ;

        LISTA_ATR_TYPE.Rule = MakePlusRule(LISTA_ATR_TYPE, coma, ATR_TYPE);

        ATR_TYPE.Rule = identificador + TIPO;

        DECLARACION.Rule = TIPO + LISTA_VARIABLES + puco
            | TIPO + LISTA_VARIABLES + igual + EXPRESION + puco
            ;

        ASIGNACION.Rule = variable + TIPO_ASIGNACION + EXPRESION + puco
            | variable + punto + LISTA_ACCESO + TIPO_ASIGNACION + EXPRESION + puco
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
            | r_new + TIPO
            | r_today + par_a + par_c
            | r_now + par_a + par_c
            | variable + punto + LISTA_ACCESO
            | llave_a + LISTA_ATR_MAP + llave_c
            | llave_a + LISTA_EXPRESIONES + llave_c
            | par_a + EXPRESION + par_c
            | par_a + TIPO + par_c + EXPRESION
            //| EXPRESION + interrogacion + EXPRESION + dospu + EXPRESION
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
            | decima
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

        this.Root = INICIO;

        #endregion


    }
}