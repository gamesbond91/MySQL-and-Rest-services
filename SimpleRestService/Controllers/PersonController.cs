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
        public ArrayList Get(int ID)
        {
            var personHelper = new PersonHelper();
            return personHelper.getPerson(ID);          
        }

        // POST: api/Person
        public HttpResponseMessage Post([FromBody] Person value)
        {
            PersonHelper personHelper = new PersonHelper();

            long id = personHelper.SavePerson(value);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.Created);
            httpResponse.Headers.Location = new Uri(Request.RequestUri, String.Format(@"person/{0}", id));
            return httpResponse;
        }

        // PUT: api/Person/5
        public  HttpResponseMessage Put(long ID, [FromBody]Person value)
        {
            PersonHelper personHelper = new PersonHelper();
            bool recordExisted = false;
            recordExisted = personHelper.UpdatePerson(ID, value);
            if (recordExisted)
            {
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        // DELETE: api/Person/5
        public HttpResponseMessage Delete(long ID)
        {
            PersonHelper personHelper = new PersonHelper();
            bool recordExisted = false;
            recordExisted = personHelper.DeletePerson(ID);
            
            if (recordExisted)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

        }
    }
}
