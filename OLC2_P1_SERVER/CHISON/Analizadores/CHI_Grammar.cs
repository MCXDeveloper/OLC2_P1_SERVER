using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

namespace OLC2_P1_SERVER.CHISON.Analizadores
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
            KeyTerm r_null = ToTerm("NULL");
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
            RegexBasedTerminal hora = new RegexBasedTerminal("hora", "'(0|0[0-9]|1[0-9]|2[0-3]):([0-9]|[0-5][0-9]):([0-9]|[0-5][0-9])'");

            #endregion

            #region NO_TERMINALES

            NonTerminal VAL = new NonTerminal("VAL");
            NonTerminal BLOCK = new NonTerminal("BLOCK");
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal PERMISO = new NonTerminal("PERMISO");
            NonTerminal ELEMENTO = new NonTerminal("ELEMENTO");
            NonTerminal IMPORTAR = new NonTerminal("IMPORTAR");
            NonTerminal TIPO_DATO = new NonTerminal("TIPO_DATO");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal LISTA_VAL = new NonTerminal("LISTA_VAL");
            NonTerminal TIPO_PARAM = new NonTerminal("TIPO_PARAM");
            NonTerminal BLOQUE_DATA = new NonTerminal("BLOQUE_DATA");
            NonTerminal LISTA_BLOCK = new NonTerminal("LISTA_BLOCK");
            NonTerminal TIPO_BOOLEANO = new NonTerminal("TIPO_BOOLEANO");
            NonTerminal LISTA_PERMISOS = new NonTerminal("LISTA_PERMISOS");
            NonTerminal VALOR_PRIMITIVO = new NonTerminal("VALOR_PRIMITIVO");
            NonTerminal LISTA_ELEMENTOS = new NonTerminal("LISTA_ELEMENTOS");
            NonTerminal BLOQUE_PERMISOS = new NonTerminal("BLOQUE_PERMISOS");
            NonTerminal LISTA_PRIMITIVOS = new NonTerminal("LISTA_PRIMITIVOS");

            #endregion

            #region GRAMATICA

            INICIO.Rule
                = open_chison + r_databases + igual + cor_a + BLOQUE_DATA + cor_c + coma + r_users + igual + cor_a + BLOQUE_DATA + cor_c + close_chison
                | open_chison + r_users + igual + cor_a + BLOQUE_DATA + cor_c + coma + r_databases + igual + cor_a + BLOQUE_DATA + cor_c + close_chison
                ;

            IMPORTAR.Rule = open_file + identificador + punto + r_chison + close_file;

            VAL.Rule = VALOR_PRIMITIVO + igual + EXPRESION;

            PERMISO.Rule = open + r_name + igual + cadena + close;

            LISTA_VAL.Rule = MakePlusRule(LISTA_VAL, coma, VAL);

            LISTA_BLOCK.Rule = MakePlusRule(LISTA_BLOCK, coma, BLOCK);

            LISTA_PERMISOS.Rule = MakeStarRule(LISTA_PERMISOS, coma, PERMISO);

            LISTA_ELEMENTOS.Rule = MakeStarRule(LISTA_ELEMENTOS, coma, ELEMENTO);

            LISTA_PRIMITIVOS.Rule = MakeStarRule(LISTA_PRIMITIVOS, coma, VALOR_PRIMITIVO);

            BLOCK.Rule
                = r_cql_type + igual + r_table
                | r_cql_type + igual + r_object
                | r_cql_type + igual + r_procedure
                | r_name + igual + cadena
                | r_attrs + igual + cor_a + BLOQUE_DATA + cor_c
                | r_columns + igual + cor_a + BLOQUE_DATA + cor_c
                | r_data + igual + cor_a + BLOQUE_DATA + cor_c
                | r_parameters + igual + cor_a + BLOQUE_DATA + cor_c
                | r_instr + igual + procedure_content
                | r_type + igual + TIPO_DATO
                | r_as + igual + TIPO_PARAM
                | r_pk + igual + TIPO_BOOLEANO
                | r_password + igual + cadena
                | r_permissions + igual + cor_a + BLOQUE_PERMISOS + cor_c
                ;

            BLOQUE_PERMISOS.Rule
                = LISTA_PERMISOS
                | IMPORTAR
                ;

            BLOQUE_DATA.Rule
                = LISTA_ELEMENTOS
                | IMPORTAR
                ;

            ELEMENTO.Rule
                = open + LISTA_BLOCK + close
                | open + LISTA_VAL + close
                ;

            EXPRESION.Rule
                = cor_a + LISTA_PRIMITIVOS + cor_c
                | open + LISTA_VAL + close
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
                | r_null
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