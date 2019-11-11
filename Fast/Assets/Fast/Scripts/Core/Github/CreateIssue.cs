using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Fast.Github
{
    public class CreateIssueObject
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class CreateIssueSuccessObject
    {

    }

    public class CreateIssueErrorObject
    {
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
    }

    public class CreateIssue : Request<CreateIssueSuccessObject, CreateIssueErrorObject>
    {
        private string github_username = "";
        private string github_access_token = "";
        private string github_repo = "";

        private CreateIssueObject issue_object = null;

        public CreateIssue(string github_username, string github_access_token, string github_repo,
            CreateIssueObject issue_object)
        {
            this.github_username = github_username;
            this.github_access_token = github_access_token;
            this.github_repo = github_repo;
            this.issue_object = issue_object;
        }

        protected override void RunRequestInternal(Action<CreateIssueSuccessObject> on_success, 
            Action<CreateIssueErrorObject> on_fail)
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
                    CreateIssueErrorObject ret = new CreateIssueErrorObject();
                    ret.ErrorMessage = "Add issue task canceled";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else if (response.IsFaulted)
                {
                    CreateIssueErrorObject ret = new CreateIssueErrorObject();
                    ret.ErrorMessage = "Add issue task faulted";
                    ret.ErrorException = response.Exception;

                    if (on_fail != null)
                        on_fail.Invoke(ret);
                }
                else
                {
                    if (response.Result.IsSuccessful)
                    {
                        CreateIssueSuccessObject ret = new CreateIssueSuccessObject();

                        if (on_success != null)
                            on_success.Invoke(ret);
                    }
                    else
                    {
                        CreateIssueErrorObject ret = new CreateIssueErrorObject();
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
