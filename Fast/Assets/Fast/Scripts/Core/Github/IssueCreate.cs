using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Fast.Github
{
    public class IssueCreateObject
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class IssueCreateSuccessObject
    {

    }

    public class IssueCreateErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class IssueCreate : Request<IssueCreateSuccessObject, IssueCreateErrorObject>
    {
        private string github_username = "";
        private string github_access_token = "";
        private string github_repo = "";

        private IssueCreateObject issue_object = null;

        public IssueCreate(string github_username, string github_access_token, string github_repo,
            IssueCreateObject issue_object)
        {
            this.github_username = github_username;
            this.github_access_token = github_access_token;
            this.github_repo = github_repo;
            this.issue_object = issue_object;
        }

        protected override void RunRequestInternal(Action<IssueCreateSuccessObject> on_success, 
            Action<IssueCreateErrorObject> on_fail)
        {
            //For Github Api Issues documentation visit https://developer.github.com/v3/issues/

            string json_issue_object = JsonConvert.SerializeObject(issue_object);

            RestClient client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(github_username, github_access_token);

            string url = "/repos/" + github_username.ToLower() + "/" + github_repo.ToLower() + "/issues";

            RestRequest request = new RestRequest(url, Method.POST);
            request.AddHeader("content-type", "application/vnd.github.symmetra-preview+json");
            request.AddJsonBody(json_issue_object);

            client.ExecuteTaskAsync(request).ContinueWith(response =>
            {
                if (response.IsCanceled)
                {
                    IssueCreateErrorObject ret = new IssueCreateErrorObject();
                    ret.ErrorMessage = "Add issue task canceled";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else if (response.IsFaulted)
                {
                    IssueCreateErrorObject ret = new IssueCreateErrorObject();
                    ret.ErrorMessage = "Add issue task faulted";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else
                {
                    if (response.Result.IsSuccessful)
                    {
                        IssueCreateSuccessObject ret = new IssueCreateSuccessObject();

                        if (on_success != null)
                            on_success.Invoke(ret);
                    }
                    else
                    {
                        IssueCreateErrorObject ret = new IssueCreateErrorObject();
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
