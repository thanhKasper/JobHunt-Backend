using JobHunt.Core.Domain.Entities;
using JobHunt.Core.Domain.RepositoryContracts;
using JobHunt.Core.Services.JobAnalysisUseCase.JobPostingAnalyzer.DTO;

namespace JobHunt.Core.Services.JobAnalysisUseCase.JobPostingNormalizer.CompanySynchronizer
{
    public class CompanySynchronizerImp(
        ICompanyGetRepository _companyGetRepository,
        ICompanyUpdateRepository _companyUpdateRepository
    ) : ICompanySynchronizer
    {
        private readonly ICompanyGetRepository _companyGetRepository = _companyGetRepository;
        private readonly ICompanyUpdateRepository _companyUpdateRepository = _companyUpdateRepository;

        public Company SynchronizeCompanyFromJobPost(JobPosting jobPost)
        {
            Company company = FindCompanyWithPrecise(
                jobPost.CompanyName,
                jobPost.CompanyWebsite,
                jobPost.WorkingLocation
            );

            if (company.IsUnknownCompany())
            {
                company = jobPost.CreateComapny();
            }
            
            _companyUpdateRepository.AddAsync(company).Wait();

            return company;
        }

        private Company FindCompanyWithPrecise(string name, string website, string address)
        {
            var companyByName = _companyGetRepository.FindByNameAsync(name).Result;
            var companyByWebsite = _companyGetRepository.FindByWebsiteAsync(website).Result;
            var companyByAddress = _companyGetRepository.FindByAddressAsync(address).Result;

            Company companyFromNameAndWebsite = GetFinalCorrectCompany(companyByName, companyByWebsite);
            Company companyFromNameAndAddress = GetFinalCorrectCompany(companyByName, companyByAddress);
            Company companyFromWebsiteAndAddress = GetFinalCorrectCompany(companyByWebsite, companyByAddress);

            // When comparing this there will be a case when
            // find by name != find by website => return UnknownCompany and
            // find by name != find by address => return UnknownCompany
            // But find by website == find by address
            Company companyWhenFindFromNameAddressWebsite = GetFinalCorrectCompany(
                companyFromNameAndWebsite, companyFromNameAndAddress);

            // Therefore, we need this statement to ensure that we can find a company in case
            // find by website == find by address
            Company finalCompany = GetFinalCorrectCompany(
                companyWhenFindFromNameAddressWebsite, companyFromWebsiteAndAddress);

            return finalCompany;
        }

        private Company GetFinalCorrectCompany(Company company1, Company company2)
        {
            if (company1.IsUnknownCompany() && company2.IsUnknownCompany())
            {
                return Company.CreateUnknownCompany();
            }
            else if (company1.IsUnknownCompany())
            {
                return company2;
            }
            else if (company2.IsUnknownCompany())
            {
                return company1;
            }
            else if (company1.IsSameCompany(company2))
            {
                return company1;
            }
            else
            {
                return Company.CreateUnknownCompany();
            }
        }
    }
}
