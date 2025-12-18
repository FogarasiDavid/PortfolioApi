using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using FluentAssertions; 
using Xunit;
using Octokit;

namespace Portfolio.IntegrationTests
{
    public class ProjectControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        public ProjectControllerTests(WebApplicationFactory<Program> applicationFactory)
        {
            _httpClient = applicationFactory.CreateClient();
        }
        [Fact]
        public async Task GetAll_ShouldReturnUnAuthentication_WhenTokenNotProvided()
        {
            //Arrange nem hiszen nemjelentkezünk be

            //act
            var respont = await _httpClient.GetAsync("/api/Project");

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, respont.StatusCode);
        }

    }
}
