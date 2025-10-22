using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> // MediatR pipeline behavior for validation
        where TRequest : ICommand<TResponse> // only for commands requests // etc: CreateProductCommand, UpdateProductCommand, DeleteProductCommand
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if(failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next(); // this will run next behaviour in the pipeline or the actual handler if there are no more behaviours
        }
    }
}
