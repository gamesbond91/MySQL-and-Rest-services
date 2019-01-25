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
        /// <summary>
        /// Get all persons
        /// </summary>
        /// <returns></returns>
        // GET: api/Person
        public ArrayList Get()
        {
            var personHelper = new PersonHelper();
            return personHelper.GetPersons();
        }

        /// <summary>
        /// Get a person by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        // GET: api/Person/5
        public ArrayList Get(int ID)
        {
            var personHelper = new PersonHelper();
            return personHelper.getPerson(ID);          
        }

        /// <summary>
        /// Save a new person in the database
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        // POST: api/Person
        public HttpResponseMessage Post([FromBody] Person value)
        {
            PersonHelper personHelper = new PersonHelper();

            long id = personHelper.SavePerson(value);
            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.Created);
            httpResponse.Headers.Location = new Uri(Request.RequestUri, String.Format(@"person/{0}", id));
            return httpResponse;
        }

        /// <summary>
        /// Update an existing person by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Delete a person from database by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
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
