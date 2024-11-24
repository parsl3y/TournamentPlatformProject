using Domain.TournamentFormat;

namespace Api.Dtos;

public record FormatDto(Guid? Id, string Name)
{
    public static FormatDto FromDomainModle(Format format)
        => new (format.Id.Value, format.Name);
}