using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Pipelines
{
    internal class LoggingBehavior<TRequest, TResponse>
      : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public LoggingBehavior(ILogger<TRequest> logger)
          => _logger = logger;

        // Log the request (command) and whether it succeeded or failed
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Get the name of the request
            string name = request.GetType().Name;

            try
            {
                // Log the request
                _logger.LogInformation($"Executing request {name}", name);

                // Execute the request
                var result = await next();

                _logger.LogInformation($"Request {name} executed successfully", name);

                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                  exception,
                  $"Request {name} failed",
                  name);

                throw;
            }
        }
    }
}

