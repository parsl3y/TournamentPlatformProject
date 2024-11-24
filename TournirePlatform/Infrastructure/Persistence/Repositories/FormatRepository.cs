using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Countries;
using Domain.Game;
using Domain.TournamentFormat;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class FormatRepository(ApplicationDbContext context) : IFormatRepository, IFormatQueries
{
    public async Task<IReadOnlyList<Format>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Formats
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Format> Add(Format format, CancellationToken cancellationToken)
    {
        await context.Formats.AddAsync(format, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return format;
    }

    public async Task<Option<Format>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var entity = await context.Formats
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Name == name, cancellationToken);
        
        return entity == null ? Option.None<Format>() : Option.Some(entity);
    }
    
    public async Task<Option<Format>> GetById(FormatId id, CancellationToken cancellationToken)
    {
        var entity = await context.Formats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Format>() : Option.Some(entity);
    }
    
    public async Task<Format> Update(Format format, CancellationToken cancellationToken)
    {
        context.Formats.Update(format);

        await context.SaveChangesAsync(cancellationToken);

        return format;
    }
    
    public async Task<Format> Delete(Format format, CancellationToken cancellationToken)
    {
        context.Formats.Remove(format);

        await context.SaveChangesAsync(cancellationToken);

        return format;
    }
}