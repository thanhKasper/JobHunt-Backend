namespace JobHunt.Core.ServiceContracts;

using JobHunt.Core.DTO;

public interface IEducationService
{
    /// <summary>
    /// Gets the list of degrees.
    /// </summary>
    /// <returns>A list of degrees as key-value pairs.</returns>
    Task<List<EducationKeyValuePair>> GetDegreeListAsync();
}