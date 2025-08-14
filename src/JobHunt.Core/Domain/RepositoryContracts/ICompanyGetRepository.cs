using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts
{
    public interface ICompanyGetRepository
    {
        Task<Company> FindByNameAsync(string companyName);
        Task<Company> FindByWebsiteAsync(string companyWebsite);
        Task<Company> FindByAddressAsync(string companyAddress);
    }
}
