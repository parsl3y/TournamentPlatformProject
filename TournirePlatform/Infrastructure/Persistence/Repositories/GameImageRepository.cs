using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using Domain.Faculties;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class GameImageRepository : IGameImageRepository
{
    private readonly ApplicationDbContext _context;

    public GameImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<GameImage>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.GameImages
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    public async Task<GameImage> Add(GameImage sneakerImage, CancellationToken cancellationToken)
    {
        await _context.GameImages.AddAsync(sneakerImage, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sneakerImage;
    }

    public async Task<GameImage> GetById(GameImageId id, CancellationToken cancellationToken)
    {
        return await _context.GameImages
            .FirstOrDefaultAsync(gi => gi.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<GameImage>> GetByGameId(GameId gameId, CancellationToken cancellationToken)
    {
        return await _context.GameImages
            .Where(gi => gi.GameId == gameId)
            .ToListAsync(cancellationToken);
    }
        
    public async Task<bool> ExistsByGameId(GameId gameId, CancellationToken cancellationToken)
    {
        var gameIdValue = gameId.Value; // Отримуємо значення напряму
        return await _context.GameImages
            .AnyAsync(image => image.GameId == gameId, cancellationToken); // Працюватиме!
    }

    
    public async Task<bool> Delete(GameImageId id, CancellationToken cancellationToken)
    {
        var sneakerImage = await _context.GameImages.FirstOrDefaultAsync(si => si.Id == id, cancellationToken);
        if (sneakerImage == null)
        {
            return false;
        }

        _context.GameImages.Remove(sneakerImage);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<GameImage> Update(GameImage gameImage, CancellationToken cancellationToken)
    {
        _context.GameImages.Update(gameImage);
        await _context.SaveChangesAsync(cancellationToken);
        return gameImage;
    }
}