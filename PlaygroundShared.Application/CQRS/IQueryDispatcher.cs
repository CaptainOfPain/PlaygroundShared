namespace PlaygroundShared.Application.CQRS;

public interface IQueryDispatcher
{
    Task<TDto> DispatchAsync<TQuery, TDto>(TQuery query) where TQuery : IQuery;
}