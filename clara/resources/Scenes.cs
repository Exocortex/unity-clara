using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnet_clara.lib.resources
{
    public class Scenes
    {
        private string uuid;

        private Method method;

        public Scenes()
        {
            method = new Method("scenes");
        }

        public Scenes(string uuid)
        {
            this.uuid = uuid;
            method = new Method("scenes");
        }

        public class Query
        {
            public int page { get; set; }
            public int perPage { get; set; }
            public string query { get; set; }
        }
        public class RenderQuery
        {
            public int time { get; set; }
            public int width { get; set; }
            public int height { get; set; }
            public string gi { get; set; }
            public string cameraNode { get; set; }
            public string cameraType { get; set; }
            public int fov { get; set; }
            public string quality { get; set; }
            public float gamma { get; set; }
            public string setupCommand { get; set; }
            public JObject data { get; set; }

        }

        public class CommandOptions
        {
            public string command { get; set; }
            public JObject data { get; set; }
        }

        //Render an image
        public byte[] Render(string sceneId, string query, string options)
        {
            RenderQuery renderQuery = JsonConvert.DeserializeObject<RenderQuery>(query);
            CommandOptions option = JsonConvert.DeserializeObject<CommandOptions>(options);

            string requestUrl = sceneId + "/render";
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;



            if (option.command == null)
                renderQuery.setupCommand = "";
            else
                renderQuery.setupCommand = option.command;

            renderQuery.data = option.data;

            //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            PropertyInfo[] properties = typeof(RenderQuery).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string key = property.Name;
                string value = null;
                if (property.GetValue(renderQuery, null) != null)
                    value = property.GetValue(renderQuery, null).ToString();

                request.AddParameter(key, value);
            }
            IRestResponse resp = method.Request("post", request, true);
            return resp.RawBytes;
        }
        //Return the thumbnail of the scene
        public byte[] Thumbnail(string sceneId)
        {
            string requestUrl = sceneId + "/thumbnail.jpg";
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse resp = method.Request("get", request, true);

            return resp.RawBytes;
        }
        //Export a scene
        public byte[] Export(string sceneId, string extension, int useCache)
        {
            string requestUrl = sceneId + "/export/" + extension;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            request.AddParameter("cache", useCache);
            IRestResponse resp = method.Request("post", request, true);
            if (resp.Content != "failed")
                return resp.RawBytes;
            else
                return new byte[0];
        }

        //Run a command
        public IRestResponse Command(string sceneId, string commandOptions)
        {
            CommandOptions option = JsonConvert.DeserializeObject<CommandOptions>(commandOptions);
            string requestUrl = sceneId + "/command/" + option.command;

            var data = new { data = option.data };
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            if (option.data != null)
                request.AddParameter("data", data);

            IRestResponse response = method.Request("post", request);

            return response;
        }

        //Import files
        public IRestResponse Import(string sceneId, string[] fileList)
        {
            string requestUrl = sceneId + "/import";
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            foreach (string file in fileList)
            {
                request.AddFile(Path.GetFileNameWithoutExtension(file), file);
            }
            IRestResponse response = method.Request("post", request);

            return response;
        }

        //Clone a scene
        public IRestResponse Clone(string sceneId)
        {
            string requestUrl = sceneId + "/clone";
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse response = method.Request("post", request);
            return response;
        }

        //Delete a scene
        public IRestResponse Delete(string sceneId)
        {
            string requestUrl = sceneId;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse response = method.Request("delete", request);
            return response;
        }

        //Create a scene
        public IRestResponse Create()
        {
            string requestUrl = null;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            IRestResponse response = method.Request("post", request);
            return response;
        }

        //Update a scene
        public IRestResponse Update(string sceneId, string sceneName)
        {
            string requestUrl = sceneId;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;
            request.AddParameter("name", sceneId);

            IRestResponse response = method.Request("put", request);
            return response;
        }
        //Create a scene
        public IRestResponse Get(string sceneId)
        {
            string requestUrl = sceneId;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            IRestResponse response = method.Request("get", request);
            return response;
        }

        //List public scenes
        public IRestResponse Library(string query)
        {
            string requestUrl = null;
            RestRequest request = new RestRequest();
            request.Resource = requestUrl;

            Query queryObj = JsonConvert.DeserializeObject<Query>(query);

            PropertyInfo[] properties = typeof(Query).GetProperties();

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

    }
}
