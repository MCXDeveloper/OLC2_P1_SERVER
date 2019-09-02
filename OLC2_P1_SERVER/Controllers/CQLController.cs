using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OLC2_P1_SERVER.Controllers
{
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
        public void Post([FromBody]Tester test)
        {
            Executer executer = new Executer();
            executer.Analizar(test.cadena);
            // System.Diagnostics.Debug.Write(test.cadena);
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

    public class Tester
    {
        public string cadena { get; set; }
    }
}
