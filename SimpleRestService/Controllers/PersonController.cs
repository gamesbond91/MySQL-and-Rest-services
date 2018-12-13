using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using SimpleRestService.Models;
using SimpleRestService;
using System.Net.Http;

namespace SimpleRestService.Controllers
{
    public class PersonController : ApiController
    {
        // GET: api/Person
        public IEnumerable<string> Get()
        {
            return new string[] { "Person1", "value2" };
        }

        // GET: api/Person/5
        public Person Get(int id)
        {
            var person = new Person()
            {
                ID = id,
                LastName = "Elod",
                FirstName = new Guid().ToString(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                PayRate = new Random(235).Next()
            };
          
            return person;
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
