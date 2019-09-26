using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

namespace OLC2_P1_SERVER.Analizadores
{
    public class CHI_Grammar : Irony.Parsing.Grammar
    {
        public CHI_Grammar() : base(false)
        {
            #region PALABRAS_RESERVADAS

            KeyTerm open = ToTerm("<");
            KeyTerm coma = ToTerm(",");
            KeyTerm igual = ToTerm("=");
            KeyTerm punto = ToTerm(".");
            KeyTerm cor_a = ToTerm("[");
            KeyTerm cor_c = ToTerm("]");
            KeyTerm close = ToTerm(">");
            KeyTerm r_in = ToTerm("IN");
            KeyTerm llave_a = ToTerm("{");
            KeyTerm llave_c = ToTerm("}");
            KeyTerm r_out = ToTerm("OUT");
            KeyTerm r_as = ToTerm("\"AS\"");
            KeyTerm r_pk = ToTerm("\"PK\"");
            KeyTerm r_true = ToTerm("TRUE");
            KeyTerm open_file = ToTerm("${");
            KeyTerm close_file = ToTerm("}$");
            KeyTerm r_int = ToTerm("\"INT\"");
            KeyTerm r_false = ToTerm("FALSE");
            KeyTerm open_chison = ToTerm("$<");
            KeyTerm close_chison = ToTerm(">$");
            KeyTerm r_name = ToTerm("\"NAME\"");
            KeyTerm r_data = ToTerm("\"DATA\"");
            KeyTerm r_type = ToTerm("\"TYPE\"");
            KeyTerm r_date = ToTerm("\"DATE\"");
            KeyTerm r_time = ToTerm("\"TIME\"");
            KeyTerm r_chison = ToTerm("chison");
            KeyTerm r_attrs = ToTerm("\"ATTRS\"");
            KeyTerm r_table = ToTerm("\"TABLE\"");
            KeyTerm r_instr = ToTerm("\"INSTR\"");
            KeyTerm r_users = ToTerm("\"USERS\"");
            KeyTerm r_object = ToTerm("\"OBJECT\"");
            KeyTerm r_double = ToTerm("\"DOUBLE\"");
            KeyTerm r_string = ToTerm("\"STRING\"");
            KeyTerm r_cursor = ToTerm("\"CURSOR\"");
            KeyTerm r_counter = ToTerm("\"COUNTER\"");
            KeyTerm r_columns = ToTerm("\"COLUMNS\"");
            KeyTerm r_boolean = ToTerm("\"BOOLEAN\"");
            KeyTerm r_cql_type = ToTerm("\"CQL-TYPE\"");
            KeyTerm r_password = ToTerm("\"PASSWORD\"");
            KeyTerm r_databases = ToTerm("\"DATABASES\"");
            KeyTerm r_procedure = ToTerm("\"PROCEDURE\"");
            KeyTerm r_parameters = ToTerm("\"PARAMETERS\"");
            KeyTerm r_permissions = ToTerm("\"PERMISSIONS\"");

            MarkReservedWords(
                "IN",
                "OUT",
                "TRUE",
                "FALSE",
                "\"PK\"",
                "\"AS\"",
                "chison",
                "\"INT\"",
                "\"NAME\"",
                "\"DATA\"",
                "\"TYPE\"",
                "\"DATE\"",
                "\"TIME\"",
                "\"ATTRS\"",
                "\"TABLE\"",
                "\"INSTR\"",
                "\"USERS\"",
                "\"OBJECT\"",
                "\"DOUBLE\"",
                "\"STRING\"",
                "\"CURSOR\"",
                "\"COUNTER\"",
                "\"COLUMNS\"",
                "\"BOOLEAN\"",
                "\"CQL-TYPE\"",
                "\"PASSWORD\"",
                "\"DATABASES\"",
                "\"PROCEDURE\"",
                "\"PARAMETERS\"",
                "\"PERMISSIONS\""
            );

            #endregion

            #region REGEX_TERMINALS

            NumberLiteral numero = new NumberLiteral("numero");
            IdentifierTerminal identificador = new IdentifierTerminal("identificador");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);
            RegexBasedTerminal procedure_content = new RegexBasedTerminal("procedure_content", "\\$[^\\$]*\\$");
            RegexBasedTerminal fecha = new RegexBasedTerminal("fecha", "'\\d{4}-((0\\d)|(1[012]))-(([012]\\d)|3[01])'");
            RegexBasedTerminal hora = new RegexBasedTerminal("hora", "'(00|[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])'");

            #endregion

            #region NO_TERMINALES

            NonTerminal VAL = new NonTerminal("VAL");
            NonTerminal VALUE = new NonTerminal("VALUE");
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal COLUMNA = new NonTerminal("COLUMNA");
            NonTerminal PERMISO = new NonTerminal("PERMISO");
            NonTerminal USUARIO = new NonTerminal("USUARIO");
            NonTerminal ELEMENTO = new NonTerminal("ELEMENTO");
            NonTerminal ATRIBUTO = new NonTerminal("ATRIBUTO");
            NonTerminal IMPORTAR = new NonTerminal("IMPORTAR");
            NonTerminal DATABASE = new NonTerminal("DATABASE");
            NonTerminal TIPO_DATO = new NonTerminal("TIPO_DATO");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal LISTA_VAL = new NonTerminal("LISTA_VAL");
            NonTerminal PARAMETRO = new NonTerminal("PARAMETRO");
            NonTerminal TIPO_PARAM = new NonTerminal("TIPO_PARAM");
            NonTerminal BLOQUE_DATA = new NonTerminal("BLOQUE_DATA");
            NonTerminal LISTA_VALUES = new NonTerminal("LISTA_VALUES");
            NonTerminal TIPO_BOOLEANO = new NonTerminal("TIPO_BOOLEANO");
            NonTerminal BLOQUE_VALUES = new NonTerminal("BLOQUE_VALUES");
            NonTerminal LISTA_COLUMNAS = new NonTerminal("LISTA_COLUMNAS");
            NonTerminal ELEMENTO_TABLA = new NonTerminal("ELEMENTO_TABLA");
            NonTerminal LISTA_DATABASE = new NonTerminal("LISTA_DATABASE");
            NonTerminal LISTA_PERMISOS = new NonTerminal("LISTA_PERMISOS");
            NonTerminal LISTA_USUARIOS = new NonTerminal("LISTA_USUARIOS");
            NonTerminal BLOQUE_COLUMNAS = new NonTerminal("BLOQUE_COLUMNAS");
            NonTerminal VALOR_PRIMITIVO = new NonTerminal("VALOR_PRIMITIVO");
            NonTerminal LISTA_ELEMENTOS = new NonTerminal("LISTA_ELEMENTOS");
            NonTerminal LISTA_ATRIBUTOS = new NonTerminal("LISTA_ATRIBUTOS");
            NonTerminal BLOQUE_PERMISOS = new NonTerminal("BLOQUE_PERMISOS");
            NonTerminal BLOQUE_USUARIOS = new NonTerminal("BLOQUE_USUARIOS");
            NonTerminal BLOQUE_DATABASE = new NonTerminal("BLOQUE_DATABASE");
            NonTerminal LISTA_PRIMITIVOS = new NonTerminal("LISTA_PRIMITIVOS");
            NonTerminal BLOQUE_ATRIBUTOS = new NonTerminal("BLOQUE_ATRIBUTOS");
            NonTerminal LISTA_PARAMETROS = new NonTerminal("LISTA_PARAMETROS");
            NonTerminal ELEMENTO_USERTYPE = new NonTerminal("ELEMENTO_USERTYPE");
            NonTerminal BLOQUE_PARAMETROS = new NonTerminal("BLOQUE_PARAMETROS");
            NonTerminal ELEMENTO_PROCEDURE = new NonTerminal("ELEMENTO_PROCEDURE");

            #endregion

            #region GRAMATICA

            INICIO.Rule = open_chison + r_databases + igual + cor_a + BLOQUE_DATABASE + cor_c + coma + r_users + igual + cor_a + BLOQUE_USUARIOS + cor_c + close_chison;

            IMPORTAR.Rule = open_file + identificador + punto + r_chison + close_file;

            ELEMENTO_USERTYPE.Rule = open + r_cql_type + igual + r_object + coma + r_name + igual + cadena + coma + r_attrs + igual + cor_a + BLOQUE_ATRIBUTOS + cor_c + close;

            ELEMENTO_TABLA.Rule = open + r_cql_type + igual + r_table + coma + r_name + igual + cadena + coma + r_columns + igual + cor_a + BLOQUE_COLUMNAS + cor_c + coma + r_data + igual + cor_a + BLOQUE_VALUES + cor_c + close;

            ELEMENTO_PROCEDURE.Rule = open + r_cql_type + igual + r_procedure + coma + r_name + igual + cadena + coma + r_parameters + igual + cor_a + BLOQUE_PARAMETROS + cor_c + coma + r_instr + igual + procedure_content + close;

            VAL.Rule = cadena + igual + EXPRESION;

            VALUE.Rule = open + LISTA_VAL + close;

            PERMISO.Rule = open + r_name + igual + cadena + close;

            ATRIBUTO.Rule = open + r_name + igual + cadena + coma + r_type + igual + TIPO_DATO + close;

            DATABASE.Rule = open + r_name + igual + cadena + coma + r_data + igual + cor_a + BLOQUE_DATA + cor_c + close;

            PARAMETRO.Rule = open + r_name + igual + cadena + coma + r_type + igual + TIPO_DATO + coma + r_as + igual + TIPO_PARAM + close;

            COLUMNA.Rule = open + r_name + igual + cadena + coma + r_type + igual + TIPO_DATO + coma + r_pk + igual + TIPO_BOOLEANO + close;

            USUARIO.Rule = open + r_name + igual + cadena + coma + r_password + igual + cadena + coma + r_permissions + igual + cor_a + BLOQUE_PERMISOS + cor_c + close;

            LISTA_VAL.Rule = MakePlusRule(LISTA_VAL, coma, VAL);

            LISTA_VALUES.Rule = MakeStarRule(LISTA_VALUES, coma, VALUE);

            LISTA_USUARIOS.Rule = MakeStarRule(LISTA_USUARIOS, coma, USUARIO);

            LISTA_PERMISOS.Rule = MakeStarRule(LISTA_PERMISOS, coma, PERMISO);

            LISTA_COLUMNAS.Rule = MakePlusRule(LISTA_COLUMNAS, coma, COLUMNA);

            LISTA_DATABASE.Rule = MakeStarRule(LISTA_DATABASE, coma, DATABASE);

            LISTA_ELEMENTOS.Rule = MakeStarRule(LISTA_ELEMENTOS, coma, ELEMENTO);

            LISTA_ATRIBUTOS.Rule = MakePlusRule(LISTA_ATRIBUTOS, coma, ATRIBUTO);

            LISTA_PARAMETROS.Rule = MakeStarRule(LISTA_PARAMETROS, coma, PARAMETRO);

            LISTA_PRIMITIVOS.Rule = MakeStarRule(LISTA_PRIMITIVOS, coma, VALOR_PRIMITIVO);

            BLOQUE_DATABASE.Rule
                = LISTA_DATABASE
                | IMPORTAR
                ;

            BLOQUE_USUARIOS.Rule
                = LISTA_USUARIOS
                | IMPORTAR
                ;

            BLOQUE_PERMISOS.Rule
                = LISTA_PERMISOS
                | IMPORTAR
                ;

            BLOQUE_DATA.Rule
                = LISTA_ELEMENTOS
                | IMPORTAR
                ;

            BLOQUE_PARAMETROS.Rule
                = LISTA_PARAMETROS
                | IMPORTAR
                ;

            BLOQUE_ATRIBUTOS.Rule
                = LISTA_ATRIBUTOS
                | IMPORTAR
                ;

            BLOQUE_COLUMNAS.Rule
                = LISTA_COLUMNAS
                | IMPORTAR
                ;

            BLOQUE_VALUES.Rule
                = LISTA_VALUES
                | IMPORTAR
                ;

            ELEMENTO.Rule = ELEMENTO_TABLA
                | ELEMENTO_USERTYPE
                | ELEMENTO_PROCEDURE
                ;

            EXPRESION.Rule
                = cor_a + LISTA_PRIMITIVOS + cor_c
                | VALOR_PRIMITIVO
                ;

            TIPO_PARAM.Rule
                = r_in
                | r_out
                ;

            VALOR_PRIMITIVO.Rule
                = numero
                | cadena
                | fecha
                | hora
                | r_true
                | r_false
                ;

            TIPO_DATO.Rule
                = r_int
                | r_double
                | r_string
                | r_boolean
                | r_date
                | r_time
                | r_counter
                | r_cursor
                | cadena
                ;

            TIPO_BOOLEANO.Rule
                = r_true
                | r_false
                ;

            #endregion

            this.Root = INICIO;
        }
    }
}