namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting.Interface
{
    public interface IRawJobPostParser
    {
        string GetJobPostUrl(string rawJob);
        string GetJobPostIndustry(string rawJob);
        string GetRawContent(string rawJob);
        string GetJobPostCompany(string rawJob);
        string GetJobPostCompanyWebsite(string rawJob);
        string GetJobPostCompanyAddress(string rawJob);

    }
}
