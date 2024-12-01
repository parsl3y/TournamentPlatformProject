using Application.Common.Interfaces.Repositories;
using Domain.Teams;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TeamImageRepository : ITeamImageRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IReadOnlyList<TeamImage>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.TeamImages
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        
        public async Task<TeamImage> Add(TeamImage teamImage, CancellationToken cancellationToken)
        {
            await _context.TeamImages.AddAsync(teamImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return teamImage;
        }
        
        public async Task<TeamImage> GetById(TeamImageId id, CancellationToken cancellationToken)
        {
            return await _context.TeamImages
                .FirstOrDefaultAsync(ti => ti.Id == id, cancellationToken);
        }
        
        public async Task<IEnumerable<TeamImage>> GetByTeamId(TeamId teamId, CancellationToken cancellationToken)
        {
            return await _context.TeamImages
                .Where(ti => ti.TeamId == teamId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<bool> ExistsByTeamId(TeamId teamId, CancellationToken cancellationToken)
        {
            return await _context.TeamImages
                .AnyAsync(image => image.TeamId == teamId, cancellationToken);
        }
        
        
        public async Task<bool> Delete(TeamImageId id, CancellationToken cancellationToken)
        {
            var teamImage = await _context.TeamImages
                .FirstOrDefaultAsync(ti => ti.Id == id, cancellationToken);
            if (teamImage == null)
            {
                return false;
            }

            _context.TeamImages.Remove(teamImage);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        
     
        public async Task<TeamImage> Update(TeamImage teamImage, CancellationToken cancellationToken)
        {
            _context.TeamImages.Update(teamImage);
            await _context.SaveChangesAsync(cancellationToken);
            return teamImage;
        }
    }
}
