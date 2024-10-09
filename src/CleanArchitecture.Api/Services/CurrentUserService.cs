using CleanArchitecture.Application.Abstractions.Services;

namespace CleanArchitecture.Api.Services
{
    public class CurrentUserService : IUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("userName");
        public string? UserName => "admin";
    }
}
