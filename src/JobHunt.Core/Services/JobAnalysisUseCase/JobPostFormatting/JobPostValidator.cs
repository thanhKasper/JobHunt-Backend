using JobHunt.Core.Domain.ValueObjects;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting.Interface;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostFormatting;

internal class JobPostValidator : IJobPostRawValidator, IRawJobPostParser
{
    //private readonly IRawJobPostParser _parser = parser;
    private string _urlSection = "";
    private string _industrySection = "";
    private string _jobContentSection = "";
    private string _companyNameSection = "";
    private string _companyWebsiteSection = "";
    private string _companyAddressSection = "";

    private readonly int TOTAL_SECTIONS = 5;

    public bool ValidSyntax(string jobRawContent)
    {

        if (!ContainsAllSections(jobRawContent)) return false;

        SplitIntoSections(jobRawContent);

        return ValidURLSection() &&
                ValidIndustry() &&
                ValidJobContent() &&
                ValidCompanyWebsiteUrl() &&
                ValidCompanyName() && 
                ValidCompanyAddress();
    }

    private bool ContainsAllSections(string checkRawContent)
    {
        return checkRawContent.Split("/n").Length == TOTAL_SECTIONS;
    }

    private void SplitIntoSections(string rawContent)
    {
        string[] sections = rawContent.Split("/n");

        _urlSection = sections[0];
        _industrySection = sections[1];
        _jobContentSection = sections[2];
        _companyNameSection = sections[3];
        _companyWebsiteSection = sections[4];
        _companyAddressSection = sections[5];
    }

    private bool ValidURLSection()
    {
        return ValidUrl(_urlSection);
    }

    private bool ValidIndustry()
    {
        return !string.IsNullOrEmpty(_industrySection) && 
            Enum.TryParse(_industrySection, true, out JobFieldKey key);
    }
     
    private bool ValidCompanyWebsiteUrl()
    {
        return ValidUrl(_companyWebsiteSection) || string.IsNullOrEmpty(_companyWebsiteSection);
    }

    private static bool ValidUrl(string url)
    {
        return Uri.IsWellFormedUriString(url, UriKind.Absolute);
    }

    private bool ValidCompanyName()
    {
        return !string.IsNullOrEmpty(_companyNameSection);
    }

    private bool ValidJobContent()
    {
        return !string.IsNullOrEmpty(_jobContentSection);
    }

    private bool ValidCompanyAddress()
    {
        return !string.IsNullOrEmpty(_companyAddressSection);
    }

    public string GetJobPostUrl(string rawJob)
    {
        return rawJob.Split("\n")[0].Trim();
    }

    public string GetJobPostIndustry(string rawJob)
    {
        return rawJob.Split("\n")[1].Trim();
    }

    public string GetRawContent(string rawJob)
    {
        return rawJob.Split("\n")[2].Trim();
    }

    public string GetJobPostCompany(string rawJob)
    {
        return rawJob.Split("\n")[3].Trim();
    }

    public string GetJobPostCompanyWebsite(string rawJob)
    {
        return rawJob.Split("\n")[4].Trim();
    }

    public string GetJobPostCompanyAddress(string rawJob)
    {
        return rawJob.Split("\n")[5].Trim();
    }
}
