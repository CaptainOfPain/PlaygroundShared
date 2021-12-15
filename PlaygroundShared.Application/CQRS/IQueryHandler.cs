namespace PlaygroundShared.Application.CQRS;

public interface IQueryHandler<in TQuery, TDto> where TQuery : IQuery
{
    Task<TDto> HandleAsync(TQuery query);
}