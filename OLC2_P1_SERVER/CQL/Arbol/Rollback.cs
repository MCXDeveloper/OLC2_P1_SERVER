using Irony;
using System.IO;
using System.Text;
using Irony.Parsing;
using OLC2_P1_SERVER.CHISON.Abstracto;
using OLC2_P1_SERVER.CHISON.Estaticas;
using OLC2_P1_SERVER.CHISON.Analizadores;
using System.Collections.Generic;
using System.Diagnostics;

public class Rollback : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public static bool IsInitFlag { get; set; }

    public Rollback(bool isInit, int fila, int columna)
    {
        this.fila = fila;
        IsInitFlag = isInit;
        this.columna = columna;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Defino la bandera de rollback para manipular de forma diferente los errores que surjan.
        CQL.IsInitRollback = IsInitFlag;

        // 2. Obtengo el archivo 'Principal.chison' el cual se encarga de contener en un solo núcleo toda la información.
        string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ChisonFilesContainer/Principal.chison");

        // 3. Verfico si existe el archivo maestro.
        if (File.Exists(path))
        {
            // 4. Verifico que el archivo tenga contenido.
            if (new FileInfo(path).Length != 0)
            {
                string[] lines = File.ReadAllLines(path, Encoding.GetEncoding("iso-8859-1"));
                string contenido = string.Join("\n", lines);

                // 5. Una vez tengo el contenido del archivo maestro, procedo a enviarlo a su respectivo parseo.
                BuildChisonParsing(contenido);
            }
            else
            {
                if (!IsInitFlag)
                {
                    CQL.AddLUPError("Semántico", "[ROLLBACK]", "Error.  El archivo principal se encuentra vacío.", fila, columna);
                }                
            }
        }
        else
        {
            if (!IsInitFlag)
            {
                CQL.AddLUPError("Semántico", "[ROLLBACK]", "Error.  No existe archivo principal.", fila, columna);
            }
        }

        // Creo la base de datos y la tabla de errores correspondiente a Chison únicamente si si hubieron errores.
        CreateAndLoadChisonLog();
        CQL.RestartSession();

        return new Nulo();
    }

    private object BuildChisonParsing(string entrada)
    {
        // +------------------------------------------------------------------------------------------------------------+
        // |                                                    Nota                                                    |
        // +------------------------------------------------------------------------------------------------------------+
        // | Esta función es de tipo recursiva ya que al leer la primera vez el archivo maestro, éste puede contener    |
        // | importaciones de otros archivos, entonces lo que realiza es ir a leer esos archivos importados e incrustar |
        // | el texto reemplazando la sentencia importar por el contenido del archivo.  Una vez terminado esto, vuelve  |
        // | a regresar a esta función para volver a ser parseada la nueva entrada.                                     |
        // +------------------------------------------------------------------------------------------------------------+

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
                    if (!IsInitFlag)
                    {
                        CQL.AddLUPMessage(" *** Rollback realizado exitosamente. *** ");
                    }
                }
                else if (parseResponse is string)
                {
                    string nuevaCadena = (string)parseResponse;
                    Debug.WriteLine(nuevaCadena);
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
            if (!IsInitFlag)
            {
                CQL.AddLUPMessage("Error en Rollback. Hay errores léxicos/sintácticos.  Cadena inválida.");
            }

            foreach (LogMessage err in arbol.ParserMessages)
            {
                string tipo_error = err.Message.Contains("Syntax") ? "Sintáctico" : "Léxico";
                CQL.AddLUPError(tipo_error, "[CQL_PARSER]", err.Message, err.Location.Line, err.Location.Column);
            }
        }

        return new Nulo();
    }

    private void CreateAndLoadChisonLog()
    {
        if (CQL.IsInitRollback && CQL.PilaErroresRollback.Count > 0)
        {
            string NombreBD = "ChisonLogger";
            string NombreTabla = "ChisonErrors";

            // 1. Primero verifico que no exista la base de datos con el nombre de 'ChisonLogger'.
            if (!CQL.ExisteBaseDeDatos(NombreBD))
            {
                // 2. Creo la base de datos.
                CQL.RegistrarBaseDeDatos(NombreBD);
                CQL.BaseDatosEnUso = NombreBD;

                // 3. Lleno con la información contenida en PilaErroresRollback.
                Table tabLog = new Table(NombreTabla);

                tabLog.AddColumn(new Columna(false, "tipo", new TipoDato(TipoDato.Tipo.STRING)));
                tabLog.AddColumn(new Columna(false, "ubicacion", new TipoDato(TipoDato.Tipo.STRING)));
                tabLog.AddColumn(new Columna(false, "descripcion", new TipoDato(TipoDato.Tipo.STRING)));
                tabLog.AddColumn(new Columna(false, "fila", new TipoDato(TipoDato.Tipo.INT)));
                tabLog.AddColumn(new Columna(false, "columna", new TipoDato(TipoDato.Tipo.INT)));

                foreach (RollbackError err in CQL.PilaErroresRollback)
                {
                    tabLog.AddRow(new List<object>() { err.TipoError, err.Ubicacion, err.Descripcion, err.Fila, err.Columna }, fila, columna);
                }

                CQL.RegistrarTabla(tabLog);
            }
        }
    }
}