using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fast.Github
{
    public class DownloadIssuesSuccessObject
    {
        public List<DownloadIssuesObject> issues = new List<DownloadIssuesObject>();
    }

    public class DownloadIssuesErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class DownloadIssuesObject
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Labels = new List<string>();
    }

    public class DownloadIssues : Request<DownloadIssuesSuccessObject, DownloadIssuesErrorObject>
    {
        private string github_username = "";
        private string github_access_token = "";
        private string github_repo_owner = "";
        private string github_repo = "";
        private int max_issues = 0;

        public DownloadIssues(string github_username, string github_access_token, string github_repo_owner, string github_repo, int max_issues)
        {
            this.github_username = github_username;
            this.github_access_token = github_access_token;
            this.github_repo_owner = github_repo_owner;
            this.github_repo = github_repo;
            this.max_issues = max_issues;
        }

        protected override void RunRequestInternal(Action<DownloadIssuesSuccessObject> on_success,
            Action<DownloadIssuesErrorObject> on_fail)
        {
            //For Github Api Issues documentation visit https://developer.github.com/v3/issues/

            RestClient client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(github_username, github_access_token);

            string url = "/repos/" + github_repo_owner + "/" + github_repo.ToLower() + "/issues";

            RestRequest request = new RestRequest(url, Method.GET);
            request.AddHeader("content-type", "application/vnd.github.symmetra-preview+json");
            request.AddParameter("per_page", max_issues);

            client.ExecuteTaskAsync(request).ContinueWith(response =>
            {
                if (response.IsCanceled)
                {
                    DownloadIssuesErrorObject ret = new DownloadIssuesErrorObject();
                    ret.ErrorMessage = "Add issue task canceled";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else if (response.IsFaulted)
                {
                    DownloadIssuesErrorObject ret = new DownloadIssuesErrorObject();
                    ret.ErrorMessage = "Add issue task faulted";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else
                {
                    if (response.Result.IsSuccessful)
                    {
                        DownloadIssuesSuccessObject ret = new DownloadIssuesSuccessObject();

                        IRestResponse rest_result = response.Result;

                        string content_json = response.Result.Content;

                        Newtonsoft.Json.Linq.JArray array = Parsers.JSONParser.ParseArray(content_json);

                        Newtonsoft.Json.Linq.JToken token = array.First;

                        while(token != null)
                        {
                            DownloadIssuesObject curr_obj = new DownloadIssuesObject();

                            curr_obj.Title = token.Value<string>("title");
                            curr_obj.Body = token.Value<string>("body");

                            Newtonsoft.Json.Linq.JArray labels_array = token.Value<Newtonsoft.Json.Linq.JArray>("labels");

                            Newtonsoft.Json.Linq.JToken labels_token = labels_array.First;

                            while (labels_token != null)
                            {
                                string name = labels_token.Value<string>("name");

                                curr_obj.Labels.Add(name);

                                labels_token = labels_token.Next;
                            }

                            ret.issues.Add(curr_obj);

                            token = token.Next;
                        }

                        if (on_success != null)
                            on_success.Invoke(ret);
                    }
                    else
                    {
                        DownloadIssuesErrorObject ret = new DownloadIssuesErrorObject();
                        ret.ErrorMessage = response.Result.ErrorMessage;
                        ret.ErrorException = response.Result.ErrorException;

                        if (on_fail != null)
                            on_fail.Invoke(ret);
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
