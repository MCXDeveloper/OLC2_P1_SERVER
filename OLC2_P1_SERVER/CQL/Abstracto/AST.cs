using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AST : Instruccion
{
    public static Entorno global;
    public List<Instruccion> Instrucciones { get; set; }

    public AST(List<Instruccion> instrucciones)
    {
        Instrucciones = instrucciones;
    }

    public object Ejecutar(Entorno ent)
    {
        global = ent;

        // Este for representa las iteraciones por las que pasa el intérprete.  En la primera se reconocen
        // funciones y en la segunda todo lo demás.

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
            {
                EjecutarFunciones(ent);
            }
            else
            {
                EjecutarNormal(ent);
            }
        }        

        //System.Diagnostics.Debug.Write(CQL.TransformEntornoToTable(ent));
        //System.Diagnostics.Debug.Write(CQL.ObtenerListaUserTypesEnString());

        return new Nulo();
    }

    private void EjecutarFunciones(Entorno ent)
    {
        foreach (Instruccion ins in Instrucciones)
        {
            if (ins is DeclaracionFuncion)
            {
                DeclaracionFuncion x = (DeclaracionFuncion)ins;
                x.Ejecutar(ent);
            }
        }
    }

    private void EjecutarNormal(Entorno ent)
    {
        foreach (Instruccion ins in Instrucciones)
        {
            if (ins is Declaracion)
            {
                Declaracion x = (Declaracion)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Asignacion)
            {
                Asignacion x = (Asignacion)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Log)
            {
                Log x = (Log)ins;
                x.Ejecutar(ent);
            }
            else if (ins is CreateDatabase)
            {
                CreateDatabase x = (CreateDatabase)ins;
                x.Ejecutar(ent);
            }
            else if (ins is UseDatabase)
            {
                UseDatabase x = (UseDatabase)ins;
                x.Ejecutar(ent);
            }
            else if (ins is CreateUserType)
            {
                CreateUserType x = (CreateUserType)ins;
                x.Ejecutar(ent);
            }
            else if (ins is CreateTable)
            {
                CreateTable x = (CreateTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is AlterTable)
            {
                AlterTable x = (AlterTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is InsertTable)
            {
                InsertTable x = (InsertTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Select)
            {
                Select x = (Select)ins;
                x.Ejecutar(ent);
            }
            else if (ins is TruncateTable)
            {
                TruncateTable x = (TruncateTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is DropTable)
            {
                DropTable x = (DropTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is DeleteTable)
            {
                DeleteTable x = (DeleteTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is CreateUser)
            {
                CreateUser x = (CreateUser)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Grant)
            {
                Grant x = (Grant)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Revoke)
            {
                Revoke x = (Revoke)ins;
                x.Ejecutar(ent);
            }
            else if (ins is TryCatch)
            {
                TryCatch x = (TryCatch)ins;
                x.Ejecutar(ent);
            }
            else if (ins is SentenciaThrow)
            {
                SentenciaThrow x = (SentenciaThrow)ins;
                x.Ejecutar(ent);
            }
            else if (ins is UpdateTable)
            {
                UpdateTable x = (UpdateTable)ins;
                x.Ejecutar(ent);
            }
            else if (ins is DeclaracionCursor)
            {
                DeclaracionCursor x = (DeclaracionCursor)ins;
                x.Ejecutar(ent);
            }
            else if (ins is AccionCursor)
            {
                AccionCursor x = (AccionCursor)ins;
                x.Ejecutar(ent);
            }
            else if (ins is ForEach)
            {
                ForEach x = (ForEach)ins;
                x.Ejecutar(ent);
            }
            else if (ins is DeclaracionProcedimiento)
            {
                DeclaracionProcedimiento x = (DeclaracionProcedimiento)ins;
                x.Ejecutar(ent);
            }
            else if (ins is AsignacionMultiple)
            {
                AsignacionMultiple x = (AsignacionMultiple)ins;
                x.Ejecutar(ent);
            }
            else if (ins is AccesoObjeto)
            {
                AccesoObjeto x = (AccesoObjeto)ins;
                x.Ejecutar(ent);
            }
            else if (ins is If)
            {
                If x = (If)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Else)
            {
                Else x = (Else)ins;
                x.Ejecutar(ent);
            }
            else if (ins is While)
            {
                While x = (While)ins;
                x.Ejecutar(ent);
            }
            else if (ins is For)
            {
                For x = (For)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Switch)
            {
                Switch x = (Switch)ins;
                x.Ejecutar(ent);
            }
            else if (ins is DoWhile)
            {
                DoWhile x = (DoWhile)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Break)
            {
                Break x = (Break)ins;
                x.Ejecutar(ent);
            }
            else if (ins is Return)
            {
                Return x = (Return)ins;
                x.Ejecutar(ent);
            }
        }
    }

}