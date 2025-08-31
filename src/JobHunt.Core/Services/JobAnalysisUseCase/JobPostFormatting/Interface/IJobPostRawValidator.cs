namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting.Interface
{
    public interface IJobPostRawValidator
    {
        bool ValidSyntax(string rawJobPost);
    }
}
