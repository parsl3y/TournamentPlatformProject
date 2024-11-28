using Domain.Teams;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface ITeamImageRepository
    {
        Task<IReadOnlyList<TeamImage>> GetAll(CancellationToken cancellationToken);
        Task<TeamImage> Add(TeamImage teamImage, CancellationToken cancellationToken);
        Task<TeamImage> GetById(TeamImageId id, CancellationToken cancellationToken);
        Task<IEnumerable<TeamImage>> GetByTeamId(TeamId teamId, CancellationToken cancellationToken);
        Task<bool> ExistsByTeamId(TeamId teamId, CancellationToken cancellationToken);
        Task<bool> Delete(TeamImageId id, CancellationToken cancellationToken);
        Task<TeamImage> Update(TeamImage teamImage, CancellationToken cancellationToken);
    }
}