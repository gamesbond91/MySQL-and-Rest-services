using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace SimpleRestService.MessageHandlers
{
    public class APIMessageHandler:DelegatingHandler
    {
        private string APIKey = "LKSADJOPJOASIH";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validkey = false;
            IEnumerable<string> RequestHeaders;
            var checkAPIKeyExists = httpRequestMessage.Headers.TryGetValues("APIKey",out RequestHeaders);
            if(checkAPIKeyExists)
            {
                if (RequestHeaders.FirstOrDefault().Equals(APIKey))
                    validkey = true;
            }

            if(!validkey)
            {
              return  httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key!");
            }
            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
   }
}