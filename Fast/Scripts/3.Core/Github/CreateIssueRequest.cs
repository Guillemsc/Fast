using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;

namespace Fast.Github
{
    public class CreateIssueSuccessObject
    {
    }

    public class CreateIssueErrorObject
    {
        private string message_error = "";
        private Exception exception = null;

        public CreateIssueErrorObject(string message_error, Exception exception)
        {
            this.message_error = message_error;
            this.exception = exception;
        }

        public string MessageError => message_error;
        public Exception Exception => exception;
    }

    public class CreateIssueRequest : AwaitRequest<CreateIssueSuccessObject, CreateIssueErrorObject>
    {
        private readonly GithubUser user = null;
        private readonly GithubRepo repo = null;
        private readonly GithubIssue issue = null;

        public CreateIssueRequest(GithubUser user, GithubRepo repo, GithubIssue issue)
        {
            this.user = user;
            this.repo = repo;
            this.issue = issue;
        }

        protected override async Task RunRequestInternal()
        {
            // For Github Api Issues documentation visit https://developer.github.com/v3/issues/

            if(user == null)
            {
                has_errors = true;
                error_result = new CreateIssueErrorObject("Github user is null", null);

                return;
            }

            if(repo == null)
            {
                error_result = new CreateIssueErrorObject("Github repo is null", null);

                return;
            }

            if(issue == null)
            {
                error_result = new CreateIssueErrorObject("Github issue is null", null);

                return;
            }

            string json_issue_object = Fast.Parsers.JSONParser.ComposeObject(issue);

            RestClient client = new RestClient("https://api.github.com/");
            client.Authenticator = new HttpBasicAuthenticator(user.GithubUsername, user.GithubAccessToken);

            string url = "/repos/" + repo.GithubRepoOwnerUsername + "/" + repo.GithubRepoName.ToLower() + "/issues";

            RestRequest request = new RestRequest(url, Method.POST);
            request.AddHeader("content-type", "application/vnd.github.symmetra-preview+json");
            request.AddJsonBody(json_issue_object);

            Task<IRestResponse> task = client.ExecuteTaskAsync(request);

            AwaitResult result = await AwaitUtils.AwaitTask(task);

            if(result.HasErrors)
            {
                has_errors = true;
                error_result = new CreateIssueErrorObject(result.Exception.Message, result.Exception);

                return;
            }

            if(task.Result == null)
            {
                has_errors = true;
                error_result = new CreateIssueErrorObject("Result is null", null);

                return;
            }

            if(!task.Result.IsSuccessful)
            {
                has_errors = true;
                error_result = new CreateIssueErrorObject(task.Result.Content, task.Result.ErrorException);

                return;
            }

            success_result = new CreateIssueSuccessObject();
        }
    }
}
