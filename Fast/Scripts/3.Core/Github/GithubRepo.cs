using System;

namespace Fast.Github
{
    public class GithubRepo
    {
        private readonly string github_repo_name = "";
        private readonly string github_repo_owner_username = "";

        public GithubRepo(string github_repo_name, string github_repo_owner_username)
        {
            this.github_repo_name = github_repo_name;
            this.github_repo_owner_username = github_repo_owner_username;
        }

        public string GithubRepoName => github_repo_name;
        public string GithubRepoOwnerUsername => github_repo_owner_username;
    }
}
