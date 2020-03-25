using System;

namespace Fast.Github
{
    public class GithubUser
    {
        private readonly string github_username = "";
        private readonly string github_access_token = "";

        public GithubUser(string github_username, string github_access_token)
        {
            this.github_username = github_username;
            this.github_access_token = github_access_token;
        }

        public string GithubUsername => github_username;
        public string GithubAccessToken => github_access_token;
    }
}
