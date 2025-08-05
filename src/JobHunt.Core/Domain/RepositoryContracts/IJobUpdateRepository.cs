using JobHunt.Core.Domain.Entities;

namespace JobHunt.Core.Domain.RepositoryContracts
{
    public interface IJobUpdateRepository
    {
        void AddNewJobAsync(Job job);
    }
}
