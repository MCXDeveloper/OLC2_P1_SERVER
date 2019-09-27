﻿using Irony;
using Irony.Parsing;
using OLC2_P1_SERVER.CHISON.Arbol;
using OLC2_P1_SERVER.CHISON.Estaticas;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OLC2_P1_SERVER.CHISON.Abstracto
{
    public class CHI_AST : CHI_Instruccion
    {
        public object ObjetoUsuarios { get; set; } // List<CHI_Usuario> o Importar
        public object ObjetoBasesDatos { get; set; } // List<CHI_Database> o Importar

        public CHI_AST(object bds, object usuarios)
        {
            ObjetoBasesDatos = bds;
            ObjetoUsuarios = usuarios;
        }

        public object Ejecutar()
        {
            // +----------------------------------------------------------------------------------------------------------+
            // |                                                   Nota                                                   |
            // +----------------------------------------------------------------------------------------------------------+
            // | A este punto, cuando se realice la primera iteración, puede que dentro del archivo vengan sentencias     |
            // | de importación, es por ello que siempre antes de ejecutar esta funcion se tiene que definir la variable  |
            // | 'CadenaEntrada' ya que en cada posición donde venga un importar se va a reemplazar su texto por el       |
            // | contenido del archivo.  Al finalizar, en vez de retornar null (lo cual indica que el arbol fue ejecutado |
            // | correctamente), se retornará un string para que se vuelva a parsear el contenido.                        |
            // +----------------------------------------------------------------------------------------------------------+
            
            if (ObjetoBasesDatos is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoBasesDatos;
                ci.Ejecutar();
            }
            else
            {
                foreach (object db in (List<object>)ObjetoBasesDatos)
                {
                    CHI_Database xdb = (CHI_Database)db;
                    xdb.Ejecutar();
                }
            }

            if (ObjetoUsuarios is CHI_Importar)
            {
                CHI_Importar ci = (CHI_Importar)ObjetoUsuarios;
                ci.Ejecutar();
            }
            else
            {
                foreach (object user in (List<object>)ObjetoUsuarios)
                {
                    CHI_Instruccion ci = (CHI_Instruccion)user;
                    ci.Ejecutar();
                }
            }

            // En esta parte se decide que se va a retornar, si el arbol ya fue construido y ejecutado correctamente (null) o si venian imports y hay que volver a parsear (StaticChison.CadenaEntrada).
            if (StaticChison.HasImports)
            {
                string cadenaNueva = string.Join("\n", StaticChison.CadenaEntradaSeparada);
                Debug.WriteLine(cadenaNueva);
                return cadenaNueva;
            }
            else
            {
                string CadenaFinal = string.Join("\n", StaticChison.CadenaSalida);
                //Debug.WriteLine(CadenaFinal);

                Grammar gramatica = new Grammar();
                LanguageData lenguaje = new LanguageData(gramatica);
                Parser parser = new Parser(lenguaje);
                ParseTree arbol = parser.Parse(CadenaFinal);

                if (arbol.ParserMessages.Count.Equals(0))
                {
                    ASTBuilder builder = new ASTBuilder(CadenaFinal);
                    AST auxArbol = builder.Analizar(arbol.Root);

                    if (!(auxArbol is null))
                    {
                        object parseResponse = auxArbol.Ejecutar(new Entorno(null));

                        if (parseResponse is Nulo)
                        {
                            Debug.WriteLine("Análisis realizado exitosamente.");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Error. No se pudo construir el árbol de CQL.");
                    }

                    //return String.Join(String.Empty, PilaRespuestas.ToArray());
                }
                else
                {
                    Debug.WriteLine("*************************************************");
                    Debug.WriteLine("*************************************************");
                    Debug.WriteLine("Hay errores lexicos o sintacticos.");
                    Debug.WriteLine("El arbol de Irony no se construyó.");
                    Debug.WriteLine("La cadena es inválida.");
                    Debug.WriteLine("*************************************************");
                    Debug.WriteLine("*************************************************");

                    foreach (LogMessage err in arbol.ParserMessages)
                    {
                        //CQL.AddLUPError("Sintáctico", "Parser", err.Message, err.Location.Line, err.Location.Column);
                        Debug.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                        Debug.WriteLine("Tipo de Error: Sintáctico.");
                        Debug.WriteLine("Ubicación: Parser.");
                        Debug.WriteLine("Descripcion: " + err.Message);
                        Debug.WriteLine("Linea: " + err.Location.Line);
                        Debug.WriteLine("Columna: " + err.Location.Column);
                        Debug.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                    }
                }

            }

            return null;
        }
    }
}