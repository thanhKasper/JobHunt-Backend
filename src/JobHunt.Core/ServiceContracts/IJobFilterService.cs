using JobHunt.Core.DTO;

namespace JobHunt.Core.ServiceContracts;

public interface IJobFilterService
{
    /// <summary>
    /// Add new job filter to the list of job filters
    /// </summary>
    /// <param name="jobFilterRequest">List of fields related to the job for filtering</param>
    /// <returns>A simple JobFilter</returns>
    public Task<JobFilterResponseDetail?> CreateNewJobFilter(JobFilterCreationRequest? jobFilterRequest);


    /// <summary>
    /// A get list of job filters created by user but with fewer information, used for general display
    /// </summary>
    /// <returns>A list of Job Filter with restricted information</returns>
    public Task<List<JobFilterResponseSimple>> GetAllJobFilterSimple();


    /// <summary>
    /// Get a detail information of a job filter 
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter that user want to search for</param>
    /// <returns>A detail information of a job filter</returns>
    public Task<JobFilterResponseDetail?> GetJobFilterDetail(Guid? jobFilterId);


    /// <summary>
    /// Remove a job filter that is not need by user anymore.
    /// </summary>
    /// <param name="jobFilterId">The id of the job filter that user want to remove</param>
    /// <returns>A removed job filter information</returns>
    public Task<JobFilterResponseSimple?> DeleteJobFilter(Guid? jobFilterId);
}