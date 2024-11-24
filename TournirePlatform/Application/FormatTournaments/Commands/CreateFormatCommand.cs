using System.Reflection;
using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.FormatTournaments.Exceptions;
using Domain.TournamentFormat;
using MediatR;
using MediatR.Wrappers;

namespace Application.FormatTournaments.Commands;

public class CreateFormatCommand : IRequest<Result<Format, TournamentFormatException>>
{
 public required string Name { get; set; }   
}

public class CreateFormatCommandHander(
 IFormatRepository _formatRepository,
 IFormatQueries _formatQueries) : IRequestHandler<CreateFormatCommand, Result<Format, TournamentFormatException>>
{
 public async Task<Result<Format, TournamentFormatException>> Handle(CreateFormatCommand request,
  CancellationToken cancellationToken)
 {
   var existingFormat = await _formatRepository.SearchByName(request.Name,cancellationToken);
   
   return await existingFormat.Match(
    f => Task.FromResult<Result<Format, TournamentFormatException>>(new FormatAlreadyExistsException(f.Id)),
    async () => await CreateEntity(request.Name, cancellationToken));
 }
 
 private async Task<Result<Format, TournamentFormatException>> CreateEntity(string name, CancellationToken cancellationToken)
 {
  try
  {
   var entity = new Format(FormatId.New(), name);
   return await _formatRepository.Add(entity, cancellationToken);
  }
  catch (Exception e)
  {
   return new FormatUnknownException(FormatId.Empty(), e);
  }
 }
}