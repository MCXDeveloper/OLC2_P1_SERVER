using Irony;
using System.IO;
using System.Text;
using Irony.Parsing;
using System.Diagnostics;
using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using OLC2_P1_SERVER.CHISON.Analizadores;

public class Rollback : Instruccion
{
    private readonly int fila;
    private readonly int columna;

    public Rollback(int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Obtengo el archivo 'Principal.chison' el cual se encarga de contener en un solo núcleo toda la información.
        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ChisonFilesContainer/Principal.chison");

        // 2. Verfico si existe el archivo maestro.
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));
            string contenido = string.Join("\n", lines);

            // 3. Una vez tengo el contenido del archivo maestro, procedo a enviarlo a su respectivo parseo.
            BuildChisonParsing(contenido);
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ROLLBACK]", "Error.  No existe archivo principal.", fila, columna);
        }

        return new Nulo();
    }

    public static object BuildChisonParsing(string entrada)
    {
        // +------------------------------------------------------------------------------------------------------------+
        // |                                                    Nota                                                    |
        // +------------------------------------------------------------------------------------------------------------+
        // | Esta función es de tipo recursiva ya que al leer la primera vez el archivo maestro, éste puede contener    |
        // | importaciones de otros archivos, entonces lo que realiza es ir a leer esos archivos importados e incrustar |
        // | el texto reemplazando la sentencia importar por el contenido del archivo.  Una vez terminado esto, vuelve  |
        // | a regresar a esta función para volver a ser parseada la nueva entrada.                                     |
        // +------------------------------------------------------------------------------------------------------------+

        // 1. Defino la bandera de rollback para manipular de forma diferente los errores que surjan.
        CQL.RollbackFlag = true;

        CHI_Grammar gramatica = new CHI_Grammar();
        LanguageData lenguaje = new LanguageData(gramatica);
        Parser parser = new Parser(lenguaje);
        ParseTree arbol = parser.Parse(entrada);

        if (arbol.ParserMessages.Count.Equals(0))
        {
            CHI_ASTBuilder builder = new CHI_ASTBuilder();
            CHI_AST auxArbol = builder.Analizar(arbol.Root);

            if (!(auxArbol is null))
            {
                StaticChison.InitializeStaticEnvironment();
                StaticChison.EstablecerYSepararCadenaEntrada(entrada);

                // Destruyo todas las estructuras contenidas en memoria.
                CQL.AccionesIniciales();

                object parseResponse = auxArbol.Ejecutar();

                if (parseResponse is null)
                {
                    CQL.AddLUPMessage("Rollback realizado exitosamente.");
                    CQL.RollbackFlag = false;
                }
                else if (parseResponse is string)
                {
                    string nuevaCadena = (string)parseResponse;
                    BuildChisonParsing(nuevaCadena);
                }
            }
            else
            {
                CQL.AddLUPError("Ejecución", "[CHISON_PARSER]", "Error. No se pudo construir el árbol de CHISON.", 0, 0);
            }
        }
        else
        {
            CQL.AddLUPMessage("Error en Rollback. Hay errores léxicos/sintácticos.  Cadena inválida.");

            foreach (LogMessage err in arbol.ParserMessages)
            {
                string tipo_error = err.Message.Contains("Syntax") ? "Sintáctico" : "Léxico";
                CQL.AddLUPError(tipo_error, "[CQL_PARSER]", err.Message, err.Location.Line, err.Location.Column);
            }
        }

        return new Nulo();
    }
}