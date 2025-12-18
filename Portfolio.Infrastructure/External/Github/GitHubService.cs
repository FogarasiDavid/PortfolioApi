using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Infrastructure.External.Github
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;

        public GitHubService(GitHubClient client)
        {
            _client = client;
        }
        //ez pedig egybegyüjti az adatokat
        public async Task<IEnumerable<GitHubRepoDto>> GetUserRepositoriesAsync(string userName)
        {
            return await _client.GetReposAsync(userName);
        }
    }
}
