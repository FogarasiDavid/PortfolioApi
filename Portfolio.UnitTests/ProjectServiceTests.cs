using FluentAssertions;
using Moq;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using Portfolio.Domain.Entity;
using Portfolio.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.UnitTests
{
    public class ProjectServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock;

        private readonly IProjectService projectService;
        public ProjectServiceTests()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();

            projectService = new ProjectService(unitOfWorkMock.Object); 
        }
        [Fact]
        public async Task GetByIdAsync_ShouldReturnProject_WhenProjectExist()
        {
            //arrange
            var projectId = 1;
            var expectedProject = new Project
            {
                Id = projectId,
                Name = "Teszt Projekt",
                GitHubUrl = "https://github.com/test/project"
            };

            unitOfWorkMock.Setup(x=>x.ProjectRepository.GetByIdAsync(projectId))
                .ReturnsAsync(expectedProject);

            //act
            var result = await projectService.GetByIdAsync(projectId);
            
            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(projectId);
            result.Name.Should().Be("Teszt Projekt");

        }
        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenProjectDoesNotExist()
        {
            var projectId = 99;
            unitOfWorkMock.Setup(x => x.ProjectRepository.GetByIdAsync(projectId))
                .ReturnsAsync((Project)null);

            await projectService.Invoking(x => x.GetByIdAsync(projectId))
                .Should().ThrowAsync<NotFoundException>();
        }

    }
}
