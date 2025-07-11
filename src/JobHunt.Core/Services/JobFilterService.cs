using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.DTO;
using JobHunt.Core.Helpers;
using JobHunt.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace JobHunt.Core.Services;

public class JobFilterService(
    IJobFilterRepository jobFilterRepo,
    UserManager<JobHunter> userManager) : IJobFilterService
{
    private readonly IJobFilterRepository _jobFilterRepo = jobFilterRepo;
    private readonly UserManager<JobHunter> _userManager = userManager;
    public async Task<JobFilterResponseDetail> CreateNewJobFilterAsync(JobFilterCreationRequest? jobFilterRequest, Guid? userId)
    {
        // Handle when jobFilterRequest is null
        ArgumentNullException.ThrowIfNull(jobFilterRequest);

        // Handle all Argument Exception cases
        ValidationContext validationCtx = new(jobFilterRequest);
        List<ValidationResult> errorList = [];
        bool isValid = Validator.TryValidateObject(jobFilterRequest, validationCtx, errorList, true);
        if (!isValid)
        {
            throw new ArgumentException(errorList.FirstOrDefault()?.ErrorMessage);
        }

        // Handling case when userid field inside DTO is empty
        if (!userId.HasValue)
        {
            throw new ArgumentNullException(nameof(jobFilterRequest), "UserId is required");
        }

        // Throws exception when UserId not exists.
        var foundUser =
            await _userManager.FindByIdAsync(userId.Value.ToString()) ??
            throw new ArgumentException(
                "Cannot find a user with that id", nameof(jobFilterRequest));


        // Remove duplication
        jobFilterRequest.Tools = Utils.RemoveKeywordDuplication(jobFilterRequest.Tools);
        jobFilterRequest.Languages = Utils.RemoveKeywordDuplication(jobFilterRequest.Languages);
        jobFilterRequest.SoftSkills = Utils.RemoveKeywordDuplication(jobFilterRequest.SoftSkills);
        jobFilterRequest.TechnicalKnowledge = Utils.RemoveKeywordDuplication(jobFilterRequest.TechnicalKnowledge);

        // Default assign IsStarred & IsActive
        jobFilterRequest.IsActive = true;
        jobFilterRequest.IsStarred = false;

        JobFilter jobFilter = jobFilterRequest.ToJobFilter();

        // Fill empty YearsOfExperience and Level
        if (jobFilter.YearsOfExperience == null) jobFilter.FillYearExp();
        else if (jobFilter.Level == null) jobFilter.FillJobLevel();

        JobFilter? newJobFilter = await _jobFilterRepo.AddJobFilterAsync(jobFilter, foundUser);
        JobFilterResponseDetail? response = newJobFilter?.ToJobFilterResponseDetail();
        return response ?? new()
        {
            Id = Guid.Empty
        };
    }

    public async Task<JobFilterResponseSimple> DeleteJobFilterAsync(Guid? jobFilterId)
    {
        if (!jobFilterId.HasValue)
            throw new ArgumentNullException(nameof(jobFilterId), "Job filter ID cannot be empty");
        JobFilter? deleteJobFilter = await _jobFilterRepo.RemoveJobFilterByIdAsync(jobFilterId.Value);
        if (deleteJobFilter != null) return deleteJobFilter.ToJobFilterResponseSimple();
        throw new ArgumentException("Cannot find job filter of this id", nameof(jobFilterId));
    }

    public async Task<List<JobFilterResponseSimple>> GetAllJobFilterSimpleAsync()
    {
        List<JobFilter> jobFilterList = await _jobFilterRepo.GetAllJobFiltersAsync() ?? [];
        return [.. jobFilterList.Select(ele => ele.ToJobFilterResponseSimple())];
    }

    public async Task<JobFilterResponseDetail> GetJobFilterDetailAsync(Guid? jobFilterId)
    {
        if (jobFilterId != null)
        {
            JobFilter? foundJobFilter = await _jobFilterRepo.FindOneJobFilterByIdAsync(jobFilterId.Value);

            if (foundJobFilter != null) return foundJobFilter.ToJobFilterResponseDetail();
        }

        return new JobFilterResponseDetail() { Id = Guid.Empty };
    }

    public async Task<bool> ToggleJobFilterActiveStateAsync(Guid? jobFilterId)
    {
        if (!jobFilterId.HasValue)
            throw new ArgumentNullException(nameof(jobFilterId), "Job filter ID cannot be empty");

        var jobFilter = await _jobFilterRepo.FindOneJobFilterByIdAsync(jobFilterId.Value)
            ?? throw new ArgumentException("Cannot find job filter of this id", nameof(jobFilterId));

        bool result = await _jobFilterRepo.ToggleJobFilterActiveStateAsync(jobFilter);

        return result;
    }

    public async Task<bool> ToggleJobFilterStarStateAsync(Guid? jobFilterId)
    {
        if (!jobFilterId.HasValue)
            throw new ArgumentNullException(nameof(jobFilterId), "Job filter ID cannot be empty");

        var jobFilter = await _jobFilterRepo.FindOneJobFilterByIdAsync(jobFilterId.Value)
            ?? throw new ArgumentException("Cannot find job filter of this id", nameof(jobFilterId));
        if (!jobFilter.IsActive!.Value) await _jobFilterRepo.ToggleJobFilterActiveStateAsync(jobFilter);
        bool result = await _jobFilterRepo.ToggleJobFilterStarStateAsync(jobFilter);

        return result;
    }

    public async Task<List<JobFieldsDTO>> GetAllJobFieldsAsync()
    {
        var jobfieldList = await _jobFilterRepo.GetAllJobFieldsAsync();
        return jobfieldList.Select(jobfield => new JobFieldsDTO()
        {
            Key = jobfield.JobFieldId.ToString(),
            Value = jobfield.VietNameseName
        }).ToList();
    }

    public async Task<List<JobLevelsDTO>> GetAllJobLevelsAsync()
    {
        var joblevelList = await _jobFilterRepo.GetAllJobLevelsAsync();
        return joblevelList.Select(jobfield => new JobLevelsDTO()
        {
            Key = jobfield.JobLevelId.ToString(),
            Value = jobfield.VietNameseName
        }).ToList();
    }

}