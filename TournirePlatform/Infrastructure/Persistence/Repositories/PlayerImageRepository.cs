using Application.Common.Interfaces.Repositories;
using Domain.Players;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PlayerImageRepository : IPlayerImageRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IReadOnlyList<PlayerImage>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.PlayerImages
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        
        public async Task<PlayerImage> Add(PlayerImage playerImage, CancellationToken cancellationToken)
        {
            await _context.PlayerImages.AddAsync(playerImage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return playerImage;
        }
        
        public async Task<PlayerImage> GetById(PlayerImageId id, CancellationToken cancellationToken)
        {
            return await _context.PlayerImages
                .FirstOrDefaultAsync(pi => pi.Id == id, cancellationToken);
        }
        
        public async Task<IEnumerable<PlayerImage>> GetByPlayerId(PlayerId playerId, CancellationToken cancellationToken)
        {
            return await _context.PlayerImages
                .Where(pi => pi.PlayerId == playerId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<bool> ExistsByPlayerId(PlayerId playerId, CancellationToken cancellationToken)
        {
            return await _context.PlayerImages
                .AnyAsync(image => image.PlayerId == playerId, cancellationToken);
        }
        
        public async Task<bool> Delete(PlayerImageId id, CancellationToken cancellationToken)
        {
            var playerImage = await _context.PlayerImages
                .FirstOrDefaultAsync(pi => pi.Id == id, cancellationToken);
            if (playerImage == null)
            {
                return false;
            }

            _context.PlayerImages.Remove(playerImage);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        
        public async Task<PlayerImage> Update(PlayerImage playerImage, CancellationToken cancellationToken)
        {
            _context.PlayerImages.Update(playerImage);
            await _context.SaveChangesAsync(cancellationToken);
            return playerImage;
        }
    }
}
