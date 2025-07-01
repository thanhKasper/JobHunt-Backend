using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IJobFilterService
{
    /// <summary>
    /// Add new job filter to the list of job filters
    /// </summary>
    /// <param name="jobFilterRequest">List of fields related to the job for filtering</param>
    /// <returns>A simple JobFilter</returns>
    public Task<JobFilterResponseDetail> CreateNewJobFilterAsync(JobFilterCreationRequest? jobFilterRequest, Guid? userId);


    /// <summary>
    /// A get list of job filters created by user but with fewer information, used for general display
    /// </summary>
    /// <returns>A list of Job Filter with restricted information</returns>
    public Task<List<JobFilterResponseSimple>> GetAllJobFilterSimpleAsync();


    /// <summary>
    /// Get a detail information of a job filter 
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter that user want to search for</param>
    /// <returns>A detail information of a job filter
    /// Job filter with empty guid in case no job filter found.</returns>
    public Task<JobFilterResponseDetail> GetJobFilterDetailAsync(Guid? jobFilterId);


    /// <summary>
    /// Remove a job filter that is not need by user anymore.
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter that user want to remove</param>
    /// <returns>A removed job filter information, job filter with empty Guid indicating not found job filter to delete</returns>
    public Task<JobFilterResponseSimple> DeleteJobFilterAsync(Guid? jobFilterId);


    /// <summary>
    /// Toggle the active state of a job filter on or off
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter user want to toggle</param>
    /// <returns>Return true if the job filter turn to active state, false otherwise</returns>
    /// <exception cref="ArgumentNullException">Throw if jobFilterId is null</exeception>
    /// <exception cref="ArgumentException">Throw if job filter of the provided Id cannot be found</exception>
    public Task<bool> ToggleJobFilterActiveStateAsync(Guid? jobFilterId);

    /// <summary>
    /// Toggle the star state of a job filter on or off
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter user want to toggle</param>
    /// <returns>Return true if the job filter turn to active state, false otherwise</returns>
    /// <exception cref="ArgumentNullException">Throw if jobFilterId is null</exeception>
    /// <exception cref="ArgumentException">Throw if job filter of the provided Id cannot be found</exception>
    public Task<bool> ToggleJobFilterStarStateAsync(Guid? jobFilterId);
}