using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnet_clara.lib.resources
{
    public class Jobs
    {
        private Method method;

        public Jobs()
        {
            method = new Method("jobs");
        }
        //Get job data
        public IRestResponse Get(string jobId)
        {
            string requestUrl = jobId;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse response = method.Request("get", request);
            return response;
        }
    }
}
