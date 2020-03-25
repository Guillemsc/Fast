using Newtonsoft.Json;
using System;

namespace Fast.Github
{
    [System.Serializable]
    public class GithubIssue
    {
        [JsonProperty("title")]
        private readonly string title = "";

        [JsonProperty("body")]
        private readonly string body = "";

        public GithubIssue(string title, string body)
        {
            this.title = title;
            this.body = body;
        }
    }
}
