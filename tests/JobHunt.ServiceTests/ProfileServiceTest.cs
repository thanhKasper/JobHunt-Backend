using AutoFixture;
using FluentAssertions;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.ServiceContracts;
using JobHunt.Core.Services;
using Moq;

namespace JobHunt.ServiceTests;

public class ProfileServiceTest
{
    private readonly IProfileService _profileService;
    private readonly IFixture _fixture;
    private readonly Mock<IProfileRepository> _profileRepositoryMock;

    public ProfileServiceTest()
    {
        _fixture = new Fixture();
        _profileRepositoryMock = new Mock<IProfileRepository>();
        _profileService = new ProfileService(_profileRepositoryMock.Object);
    }


    #region GetProfileAsync Tests
    [Fact]
    public async Task GetProfileAsync_ReturnProfile_ToBeSuccessfull()
    {
        // Arrange
        var jobHunterId = Guid.NewGuid();
        var expectedProfile = new JobHunter
        {
            Id = jobHunterId,
            FullName = "John Doe",
            WorkingEmail = "john.doe@example.com"

        };
        _profileRepositoryMock.Setup(repo => repo.GetProfileAsync(jobHunterId))
            .ReturnsAsync(expectedProfile);

        // Act
        var result = await _profileService.GetProfileAsync(jobHunterId);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProfileAsync_ProfileNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var jobHunterId = Guid.NewGuid();
        _profileRepositoryMock.Setup(repo => repo.GetProfileAsync(jobHunterId))
            .ReturnsAsync((JobHunter?)null);

        // Act
        Func<Task> act = async () => await _profileService.GetProfileAsync(jobHunterId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetProfileAsync_InvalidJobHunterId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid? jobHunterId = null;

        // Act
        Func<Task> act = async () => await _profileService.GetProfileAsync(jobHunterId);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
    #endregion

    #region UpdateProfileAsync
    [Fact]
    public async Task UpdateProfileAsync_ValidProfile_ShouldReturnUpdatedProfile()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.JobFinderId, Guid.NewGuid())
            .With(pr => pr.FullName, "Jane Doe")
            .With(pr => pr.WorkingEmail, "jane.doe@example.com")
            .With(pr => pr.PhoneNumber, "123-456-7890")
            .With(pr => pr.Education, "BachelorDegree")
            .With(pr => pr.Major, "ComputerScience")
            .Create();

        JobHunter jobHunterProfile = profileRequest.ToJobHunter();
        _profileRepositoryMock.Setup(repo => repo.GetProfileAsync(profileRequest.JobFinderId))
            .ReturnsAsync(jobHunterProfile);
        _profileRepositoryMock.Setup(repo => repo.UpdateProfileAsync(It.IsAny<JobHunter>()))
            .ReturnsAsync(jobHunterProfile);

        // Act
        var result = await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(jobHunterProfile.ToProfileResponse());
    }

    [Fact]
    public async Task UpdateProfileAsync_NullProfileRequest_ShouldThrowArgumentNullException()
    {
        // Arrange
        ProfileRequest? profileRequest = null;

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
    [Fact]
    public async Task UpdateProfileAsync_InvalidEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.WorkingEmail, "invalid-email-format") // Invalid email format
            .Create();

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    public async Task UpdateProfileAsync_InvalidPhoneNumber_ShouldThrowArgumentException()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.PhoneNumber, "12345") // Invalid phone number format
            .Create();

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    public async Task UpdateProfileAsync_InvalidEducation_ShouldThrowArgumentException()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.Education, "InvalidEducation") // Invalid education level
            .Create();

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }
    [Fact]
    public async Task UpdateProfileAsync_InvalidMajor_ShouldThrowArgumentException()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.Major, "InvalidMajor") // Invalid major
            .Create();

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task UpdateProfileAsync_NonExistentJobHunter_ShouldThrowArgumentException()
    {
        // Arrange
        var profileRequest = _fixture.Build<ProfileRequest>()
            .With(pr => pr.JobFinderId, Guid.NewGuid()) // Non-existent JobHunter ID
            .Create();
        _profileRepositoryMock.Setup(repo => repo.GetProfileAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as JobHunter); // Simulate non-existent profile

        // Act
        Func<Task> act = async () => await _profileService.UpdateProfileAsync(profileRequest);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    #endregion
}