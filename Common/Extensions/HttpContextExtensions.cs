using Microsoft.AspNetCore.Http;

namespace Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUser(this HttpContext httpContext)
        {
#pragma warning disable CS8603 // Possible null reference return.
            if (httpContext is null)
            {
                return String.Empty;
            }

            var username = httpContext.Items.SingleOrDefault(x => x.Key.ToString() == "username");

            return username.Value is not null ? username.Value.ToString() : String.Empty;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
