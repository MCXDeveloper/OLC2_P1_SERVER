using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Irony.Parsing;

namespace OLC2_P1_SERVER.LUP.Analizadores
{
    public class LUP_Grammar : Irony.Parsing.Grammar
    {

        public LUP_Grammar() : base(false)
        {

            #region PALABRAS_RESERVADAS

            KeyTerm r_open_login = ToTerm("[+LOGIN]"),
            r_close_login = ToTerm("[-LOGIN]"),
            r_open_logout = ToTerm("[+LOGOUT]"),
            r_close_logout = ToTerm("[-LOGOUT]"),
            r_open_user = ToTerm("[+USER]"),
            r_close_user = ToTerm("[-USER]"),
            r_open_pass = ToTerm("[+PASS]"),
            r_close_pass = ToTerm("[-PASS]"),
            r_open_query = ToTerm("[+QUERY]"),
            r_close_query = ToTerm("[-QUERY]"),
            r_open_data = ToTerm("[+DATA]"),
            r_close_data = ToTerm("[-DATA]");

            MarkReservedWords("[+LOGIN]", "[-LOGIN]", "[+LOGOUT]", "[-LOGOUT]", "[+USER]", "[-USER]", "[+PASS]", "[-PASS]", "[+QUERY]", "[-QUERY]", "[+DATA]", "[-DATA]");

            #endregion

            #region REGEX_TERMINALS

            RegexBasedTerminal contenido = new RegexBasedTerminal("contenido", "[^.]*");

            #endregion

            #region NO_TERMINALES

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal SENTENCIAS = new NonTerminal("SENTENCIAS");

            #endregion

            #region GRAMATICA

            INICIO.Rule = SENTENCIAS;

            SENTENCIAS.Rule = r_open_login + r_open_user + contenido + r_close_user + r_open_pass + contenido + r_close_pass + r_close_login
                | r_open_logout + r_open_user + contenido + r_close_user + r_close_logout
                | r_open_query + r_open_user + contenido + r_close_user + r_open_data + contenido + r_close_data + r_close_query
                ;

            #endregion

            this.Root = INICIO;

        }

    }
}