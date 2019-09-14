using OLC2_P1_SERVER.LUP.Analizadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Irony.Parsing;

namespace OLC2_P1_SERVER.Controllers
{
    public class LUPackage
    {
        public string LUPMessage { get; set; }
    }

    public class CQLController : ApiController
    {
        // GET: api/CQL
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET: api/CQL/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CQL
        public string Post([FromBody]LUPackage package)
        {
            string response = String.Empty;

            // 1. Recibo el mensaje de LUP y procedo a enviarlo a su parser.
            LUP_Grammar gramatica = new LUP_Grammar();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(package.LUPMessage);

            if (arbol.ParserMessages.Count.Equals(0))
            {
                LUP_ASTBuilder builder = new LUP_ASTBuilder();
                LUP_AST auxArbol = builder.Analizar(arbol.Root);

                if (!(auxArbol is null))
                {
                    object parseResponse = auxArbol.Ejecutar();
                    response = (!(parseResponse is null)) ? (string)parseResponse : response;
                }
                else
                {
                    System.Diagnostics.Debug.Write("Error. No se pudo construir el árbol de LUP." + Environment.NewLine);
                }
            }

            return response;
        }

        // PUT: api/CQL/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CQL/5
        public void Delete(int id)
        {
        }
    }
}
