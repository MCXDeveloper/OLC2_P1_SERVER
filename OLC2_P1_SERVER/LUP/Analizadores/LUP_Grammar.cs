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

            KeyTerm r_open_login = ToTerm("[+LOGIN]");
            KeyTerm r_close_login = ToTerm("[-LOGIN]");
            KeyTerm r_open_logout = ToTerm("[+LOGOUT]");
            KeyTerm r_close_logout = ToTerm("[-LOGOUT]");
            KeyTerm r_open_query = ToTerm("[+QUERY]");
            KeyTerm r_close_query = ToTerm("[-QUERY]");
            KeyTerm r_open_struct = ToTerm("[+STRUCT]");
            KeyTerm r_close_struct = ToTerm("[-STRUCT]");

            MarkReservedWords("[+LOGIN]", "[-LOGIN]", "[+LOGOUT]", "[-LOGOUT]", "[+QUERY]", "[-QUERY]");

            #endregion

            #region REGEX_TERMINALS

            RegexBasedTerminal user_block = new RegexBasedTerminal("user_block", "\\[\\+USER\\](.|\\n)*\\[-USER\\]");
            RegexBasedTerminal pass_block = new RegexBasedTerminal("pass_block", "\\[\\+PASS\\](.|\\n)*\\[-PASS\\]");
            RegexBasedTerminal data_block = new RegexBasedTerminal("data_block", "\\[\\+DATA\\](.|\\n)*\\[-DATA\\]");


            #endregion

            #region NO_TERMINALES

            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal SENTENCIAS = new NonTerminal("SENTENCIAS");

            #endregion

            #region GRAMATICA

            INICIO.Rule = SENTENCIAS;

            SENTENCIAS.Rule = r_open_login + user_block + pass_block + r_close_login
                | r_open_logout + user_block + r_close_logout
                | r_open_query + user_block + data_block + r_close_query
                | r_open_struct + user_block + r_close_struct
                ;

            #endregion

            this.Root = INICIO;

        }

    }
}