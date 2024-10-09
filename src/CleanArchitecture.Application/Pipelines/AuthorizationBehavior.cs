using CleanArchitecture.Application.Abstractions.Services;
using MediatR;

namespace CleanArchitecture.Application.Pipelines
{
    internal class AuthorizationBehavior<TRequest, TResponse>
      : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IUser _currentUser;

        public AuthorizationBehavior(IUser currentUser)
        {
            _currentUser = currentUser;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var currentUser = _currentUser.UserName;
            if (currentUser == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            // Compare user against a list of authorized users
            if (currentUser != "admin")
            {
                throw new UnauthorizedAccessException("User is not authorized");
            }

            var result = await next();
            return result;
        }
    }
}
