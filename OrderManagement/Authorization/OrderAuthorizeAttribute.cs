using Microsoft.AspNetCore.Authorization;

namespace Sayad.Authorization
{
    public class OrderAuthorizeAttribute : AuthorizeAttribute
    {
        public OrderAuthorizeAttribute()
            : base(AuthorizationPolicy.Order)
        {
        }
    }
}
