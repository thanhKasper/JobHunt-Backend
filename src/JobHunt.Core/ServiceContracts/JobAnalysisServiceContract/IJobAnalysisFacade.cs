namespace JobHunt.Core.ServiceContracts.JobAnalysisServiceContract;

public interface IJobAnalysisFacade
{
    public void AnalyseJobPosting(string rawJobPosting);
}