using MediatR;
//IRequest<T> dolazi od MediatR biblioteke i koristi se za definisanje zahteva koji vraća rezultat tipa T.
//IRequestHandler < T, X > je interfejs koji definiše rukovalac za zahtev tipa T koji vraća rezultat tipa X.
namespace BuildingBlocks.CQRS
{
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    {
    }

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
    }
}
