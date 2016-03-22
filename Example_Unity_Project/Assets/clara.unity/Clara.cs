using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnet_clara.lib.resources;

namespace dotnet_clara.lib
{
    public class Clara
    {
        public Scenes scenes;
        public Jobs jobs;
        public User user;
        private Config config;

        public Clara(string username, string apiToken, string host)
        {
            this.config = new Config();
            config.initializeConfig();
            config.SetConfig("username", username);
            config.SetConfig("apiToken", apiToken);
            config.SetConfig("host", host);
            this.scenes = new Scenes();
            this.jobs = new Jobs();
            this.user = new User();
        }
        public Clara(Config config)
        {
            this.config = config;
            this.scenes = new Scenes();
            this.jobs = new Jobs();
            this.user = new User();
        }
    }
}
