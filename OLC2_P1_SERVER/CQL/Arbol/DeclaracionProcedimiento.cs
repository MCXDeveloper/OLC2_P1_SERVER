using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class DeclaracionProcedimiento : Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public string NombreProcedimiento { get; set; }
    public string InstruccionesEnString { get; set; }
    public List<Parametro> ListaRetornos { get; set; }
    public List<Parametro> ListaParametros { get; set; }
    public List<Instruccion> ListaInstrucciones { get; set; }

    public DeclaracionProcedimiento(string nombre_procedimiento, List<Parametro> lista_parametros, List<Parametro> lista_retornos, List<Instruccion> lista_instrucciones, string ins_en_texto, int fila, int columna)
    {
        this.fila = fila;
        this.columna = columna;
        ListaRetornos = lista_retornos;
        ListaParametros = lista_parametros;
        InstruccionesEnString = ins_en_texto;
        ListaInstrucciones = lista_instrucciones;
        NombreProcedimiento = nombre_procedimiento;
    }

    public object Ejecutar(Entorno ent)
    {
        // 1. Valido que exista una base de datos en uso.
        if (CQL.ExisteBaseDeDatosEnUso())
        {
            // 2. Genero la llave única que representa el procedimiento.
            string key = GenerateUniqueKey();

            // 3. Valido que la llave única generada no exista en la lista de procedimientos de la base de datos actual.
            if (!CQL.ExisteProcedimientoEnBD(NombreProcedimiento))
            {
                CQL.RegistrarProcedimientoEnBD(new Procedimiento(key, ListaParametros, ListaRetornos, ListaInstrucciones, InstruccionesEnString));
                CQL.AddLUPMessage("El procedimiento '"+ NombreProcedimiento +"' ha sido registrado correctamente en la base de datos.");
            }
            else
            {
                string mensaje = "Error.  No se puede crear un el procedimiento ''.  Uno con el mismo nombre ya existe en la base de datos.";
                CQL.AddLUPError("Semántico", "[DECLARACION_PROC]", mensaje, fila, columna);
                if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'ProcedureAlreadyExists' no capturada.  " + mensaje); }
                return new ProcedureAlreadyExists(mensaje);
            }
        }
        else
        {
            string mensaje = "Error.  No se puede crear un procedimiento si no se ha especificado la base de datos a utilizar.";
            CQL.AddLUPError("Semántico", "[DECLARACION_PROC]", mensaje, fila, columna);
            if (!CQL.TryCatchFlag) { CQL.AddLUPMessage("Excepción de tipo 'UseBDException' no capturada.  " + mensaje); }
            return new UseBDException(mensaje);
        }
        
        return new Nulo();
    }

    private string GenerateUniqueKey()
    {
        string id = "_" + NombreProcedimiento + "(";

        foreach (Parametro p in ListaParametros)
        {
            TipoDato.Tipo type = p.TipoDatoParametro.GetRealTipo();
            id += "_" + type.ToString();

            if (type.Equals(TipoDato.Tipo.OBJECT))
            {
                id += "_" + (string)p.TipoDatoParametro.GetElemento();
            }
        }

        id += ")";

        return id;
    }
}