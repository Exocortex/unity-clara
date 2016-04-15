using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnet_clara.lib.resources
{
    public class User
    {
        private Method method;

        public User()
        {
            method = new Method("users");
        }

        public class SceneQuery
        {
            public int page { get; set; }
            public int perPage { get; set; }
            public string query { get; set; }
        }
        public class JobQuery
        {
            public int page { get; set; }
            public int perPage { get; set; }
        }

        public class Profile
        {
            public string name { get; set; }
            public string jobTitle { get; set; }
            public string company { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string website { get; set; }
        }
        public class Scene
        {
            public string id { get; set; }
            public string name { get; set; }
            public string owner { get; set; }
        }
        public class SceneList
        {
            public int page { get; set; }
            public int perPage { get; set; }
            public int total { get; set; }
            public Scene[] models { get; set; } 
        }

        //Get user profile
        public IRestResponse Get(string username)
        {
            string requestUrl = username;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse response = method.Request("get", request);
            return response;
        }

        // Update user profile
        public IRestResponse Update(string username, string profile)
        {
            string requestUrl = username;

            Profile pro = JsonConvert.DeserializeObject<Profile>(profile);

            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            PropertyInfo[] properties = typeof(Profile).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                string value = null;
                if (property.GetValue(pro, null) != null)
                    value = property.GetValue(pro, null).ToString();

                request.AddParameter(key, value);
            }

            IRestResponse response = method.Request("put", request);
            return response;
        }

        //List your scenes
        public IRestResponse ListScenes(string username, string query)
        {
            string requestUrl = username + "/scenes";

            SceneQuery queryObj = JsonConvert.DeserializeObject<SceneQuery>(query);

            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            PropertyInfo[] properties = typeof(SceneQuery).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                string value = null;
                if (property.GetValue(queryObj, null) != null)
                    value = property.GetValue(queryObj, null).ToString();

                request.AddParameter(key, value);
            }

            IRestResponse response = method.Request("get", request);

            return response;
        }
        //List your scenes of a collection
        public string[] ListScenesOfCollection(string username, string collectionUUID)
        {
            string requestUrl = username + "/scenes?collection=" + collectionUUID;

            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            IRestResponse response = method.Request("get", request);
            SceneList scenes = JsonConvert.DeserializeObject<SceneList>(response.Content);

            List<string> sceneUuidList = new List<string>();
            for (int i = 0; i < scenes.models.Length; i++)
            {
                sceneUuidList.Add(scenes.models[i].id);
            }

            string[] sceneUuids = sceneUuidList.ToArray();

            return sceneUuids;
        }

        //List your jobs
        public IRestResponse ListJobs(string username, string query)
        {
            string requestUrl = username + "/jobs";

            JobQuery qry = JsonConvert.DeserializeObject<JobQuery>(query);

            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            PropertyInfo[] properties = typeof(JobQuery).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                string value = null;
                if (property.GetValue(qry, null) != null)
                    value = property.GetValue(qry, null).ToString();

                request.AddParameter(key, value);
            }

            IRestResponse response = method.Request("get", request);

            return response;
        }
    }
}
