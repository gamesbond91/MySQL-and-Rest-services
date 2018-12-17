using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using SimpleRestService.Models;
using System.Net.Http;
using System.Collections;

namespace SimpleRestService.Controllers
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public ArrayList Get()
        {
            var personHelper = new PersonHelper();
            return personHelper.GetPersons();
        }

        // GET: api/Person/5
        public ArrayList Get(int id)
        {
            var personHelper = new PersonHelper();
            return personHelper.getPerson(id);          
        }

        // POST: api/Person
        public HttpResponseMessage Post([FromBody] Person value)
        {
            PersonHelper personHelper = new PersonHelper();

            long id = personHelper.SavePerson(value);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri, String.Format(@"person/{0}", id));
            return response;
        }

        // PUT: api/Person/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Person/5
        public void Delete(int id)
        {
        }
    }
}
