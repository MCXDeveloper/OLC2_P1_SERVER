
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class AccesoObjeto : Expresion, Instruccion
{
    private readonly int fila;
    private readonly int columna;
    public bool IsValor { get; set; }
    public string Variable { get; set; }
    public List<Expresion> ListaAcceso { get; set; }
    
    public AccesoObjeto(bool isValor, string variable, List<Expresion> lista_acceso, int fila, int columna)
    {
        this.fila = fila;
        IsValor = isValor;
        Variable = variable;
        this.columna = columna;
        ListaAcceso = lista_acceso;
    }

    public TipoDato GetTipo(Entorno ent)
    {
        object valor = Ejecutar(ent);

        if (valor is int)
        {
            return new TipoDato(TipoDato.Tipo.INT);
        }
        else if (valor is double)
        {
            return new TipoDato(TipoDato.Tipo.DOUBLE);
        }
        else if (valor is string)
        {
            return new TipoDato(TipoDato.Tipo.STRING);
        }
        else if (valor is bool)
        {
            return new TipoDato(TipoDato.Tipo.BOOLEAN);
        }
        else if (valor is Date)
        {
            return new TipoDato(TipoDato.Tipo.DATE);
        }
        else if (valor is Time)
        {
            return new TipoDato(TipoDato.Tipo.TIME);
        }
        else if (valor is Map)
        {
            return new TipoDato(TipoDato.Tipo.MAP);
        }
        else if (valor is XList)
        {
            return new TipoDato(TipoDato.Tipo.LIST);
        }
        else if (valor is XSet)
        {
            return new TipoDato(TipoDato.Tipo.SET);
        }
        else
        {
            return new TipoDato(TipoDato.Tipo.OBJECT);
        }
    }

    public object Ejecutar(Entorno ent)
    {

        // +----------------------------------------------------------------------------------------------------------------------------+
        // |                                                            NOTA                                                            |
        // +----------------------------------------------------------------------------------------------------------------------------+
        // | Para realizar de forma correcta el acceso al valor de un objeto, se deben seguir los siguientes pasos:                     |
        // | 1. Validar que la variable de la cual parte el acceso exista en el entorno.                                                |
        // | 2. Recorrer la lista de acceso de tal forma en la que se sepa que tipo de acceso es.  Pueden existir los siguientes tipos: |
        // |    a. Funciones propias de collections (se debe de validar que el valor anterior sea un Collection).                       |
        // |    b. Funciones propias de string (se debe validar que el valor anterior sea de tipo String).                              |
        // |    c. Funciones propias de Date (se debe validar que el valor anterior sea de tipo Date).                                  |
        // |    d. Funciones propias de Time (se debe validar que el valor anterior sea de tipo Time).                                  |
        // |    e. Nombre de un atributo (se debe validar que el valor anterior sea de tipo Objeto).                                    |
        // +----------------------------------------------------------------------------------------------------------------------------+

        object response = new Nulo();

        // Primero verifico que la variable exista en el entorno.
        object simbolo = ent.ObtenerVariable(Variable);

        if(!(simbolo is Nulo))
        {
            object padre = ((Variable)simbolo).Valor;

            foreach (Expresion exp in ListaAcceso)
            {
                object access_resp = ValidarAcceso(padre, exp, ent);
                
                if(access_resp is Nulo)
                {
                    break;
                }
                else
                {
                    padre = access_resp;
                }
            }

            response = padre;
        }
        else
        {
            CQL.AddLUPError("Semántico", "[ACCESO_OBJETO]", "El símbolo con el identificador '"+ Variable +"' no existe en el entorno.", fila, columna);
        }
        
        return response;
    }

    private object ValidarAcceso(object elemento, Expresion acceso, Entorno ent)
    {
        if (acceso is Atributo)
        {
            Atributo atr = (Atributo)acceso;
            atr.IsValor = IsValor;
            atr.Padre = elemento;
            return atr.Ejecutar(ent);
        }
        else if (acceso is FuncionContains)
        {
            FuncionContains fc = (FuncionContains)acceso;
            fc.Padre = elemento;
            return fc.Ejecutar(ent);
        }
        else if (acceso is FuncionGet)
        {
            FuncionGet fg = (FuncionGet)acceso;
            fg.Padre = elemento;
            return fg.Ejecutar(ent);
        }
        else if (acceso is FuncionSize)
        {
            FuncionSize fs = (FuncionSize)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionEndsWith)
        {
            FuncionEndsWith few = (FuncionEndsWith)acceso;
            few.Padre = elemento;
            return few.Ejecutar(ent);
        }
        else if (acceso is FuncionStartsWith)
        {
            FuncionStartsWith fsw = (FuncionStartsWith)acceso;
            fsw.Padre = elemento;
            return fsw.Ejecutar(ent);
        }
        else if (acceso is FuncionLength)
        {
            FuncionLength fl = (FuncionLength)acceso;
            fl.Padre = elemento;
            return fl.Ejecutar(ent);
        }
        else if (acceso is FuncionSubstring)
        {
            FuncionSubstring fs = (FuncionSubstring)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionToLowerCase)
        {
            FuncionToLowerCase fs = (FuncionToLowerCase)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionToUpperCase)
        {
            FuncionToUpperCase fs = (FuncionToUpperCase)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetDay)
        {
            FuncionGetDay fs = (FuncionGetDay)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetMonth)
        {
            FuncionGetMonth fs = (FuncionGetMonth)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetYear)
        {
            FuncionGetYear fs = (FuncionGetYear)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetHour)
        {
            FuncionGetHour fs = (FuncionGetHour)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetMinutes)
        {
            FuncionGetMinutes fs = (FuncionGetMinutes)acceso;
            fs.Padre = elemento;
            return fs.Ejecutar(ent);
        }
        else if (acceso is FuncionGetSeconds)
        {
            FuncionGetSeconds fgs = (FuncionGetSeconds)acceso;
            fgs.Padre = elemento;
            return fgs.Ejecutar(ent);
        }
        else
        {
            if (!IsValor)
            {
                if (acceso is FuncionInsert)
                {
                    FuncionInsert fi = (FuncionInsert)acceso;
                    fi.Padre = elemento;
                    fi.Ejecutar(ent);
                }
                else if (acceso is FuncionSet)
                {
                    FuncionSet fs = (FuncionSet)acceso;
                    fs.Padre = elemento;
                    fs.Ejecutar(ent);
                }
                else if (acceso is FuncionClear)
                {
                    FuncionClear fs = (FuncionClear)acceso;
                    fs.Padre = elemento;
                    fs.Ejecutar(ent);
                }
                else if (acceso is FuncionRemove)
                {
                    FuncionRemove fs = (FuncionRemove)acceso;
                    fs.Padre = elemento;
                    fs.Ejecutar(ent);
                }
            }
            else
            {
                CQL.AddLUPError("Semántico", "[ACCESO_OBJETO]", "Error de acceso.  El tipo de dato '" + acceso.GetType().ToString() + "' no es permitido en el acceso a elementos.", fila, columna);
            }
        }

        return new Nulo();
    }
}