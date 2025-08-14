using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts
{
    public interface ICompanyUpdateRepository
    {
        Task AddAsync(Company company);
    }
}
