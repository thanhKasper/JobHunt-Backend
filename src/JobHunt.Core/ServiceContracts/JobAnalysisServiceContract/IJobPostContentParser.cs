using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.ServiceContracts.JobAnalysisServiceContract
{
    public class JobPostContent
    {
        public string Title { get; set; } = string.Empty;
        public int YearOfExperience { get; set; }
        public JobLevelKey JobLevel { get; set; }
        public List<string> Requirements { get; set; } = new List<string>();
        public List<string> Responsibilities { get; set; } = new List<string>();

    }

    public interface IJobPostContentParser
    {
        public JobPostContent ParseContent(string rawJobPost);
    }
}
