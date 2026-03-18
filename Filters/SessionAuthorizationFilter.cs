using apteka.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace apteka.Filters
{
    public class SessionAuthorizationFilter : IAsyncActionFilter
    {
        private readonly ApplicationDbContext2 _context;

        public SessionAuthorizationFilter(ApplicationDbContext2 context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.RouteData.Values["controller"]?.ToString() ?? string.Empty;
            var action = context.RouteData.Values["action"]?.ToString() ?? string.Empty;
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var roleName = NormalizeRoleName(context.HttpContext.Session.GetString("UserRole"));

            if (IsAuthenticationPage(controller, action))
            {
                if (userId.HasValue)
                {
                    if (string.IsNullOrWhiteSpace(roleName))
                    {
                        roleName = await LoadRoleNameAsync(userId.Value, context.HttpContext);
                    }

                    context.Result = new RedirectToActionResult("Index", "Home", null);
                    return;
                }

                await next();
                return;
            }

            if (AllowsAnonymous(context))
            {
                await next();
                return;
            }

            if (!userId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                roleName = await LoadRoleNameAsync(userId.Value, context.HttpContext);
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                context.HttpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (roleName == "admin")
            {
                await next();
                return;
            }

            if (roleName == "user" && IsAllowedForUser(controller, action))
            {
                await next();
                return;
            }

            context.Result = new RedirectToActionResult("Index", "Home", null);
        }

        private static bool AllowsAnonymous(FilterContext context)
        {
            return context.ActionDescriptor.EndpointMetadata.Any(metadata => metadata is AllowAnonymousAttribute);
        }

        private static bool IsAuthenticationPage(string controller, string action)
        {
            return controller.Equals("Account", StringComparison.OrdinalIgnoreCase)
                && (action.Equals("Login", StringComparison.OrdinalIgnoreCase)
                    || action.Equals("Register", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsAllowedForUser(string controller, string action)
        {
            return (controller.Equals("Home", StringComparison.OrdinalIgnoreCase)
                    && action.Equals("Index", StringComparison.OrdinalIgnoreCase))
                || (controller.Equals("Leks", StringComparison.OrdinalIgnoreCase)
                    && (action.Equals("Index", StringComparison.OrdinalIgnoreCase)
                        || action.Equals("Details", StringComparison.OrdinalIgnoreCase)))
                || (controller.Equals("Account", StringComparison.OrdinalIgnoreCase)
                    && action.Equals("Logout", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<string?> LoadRoleNameAsync(int userId, HttpContext httpContext)
        {
            var user = await _context.Users
                .Include(item => item.Role)
                .FirstOrDefaultAsync(item => item.IdUser == userId);

            var roleName = ResolveRoleName(user);
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                httpContext.Session.SetString("UserRole", roleName);
            }

            return roleName;
        }

        private static string? ResolveRoleName(apteka.Models.User? user)
        {
            var roleName = NormalizeRoleName(user?.Role?.name);
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                return roleName;
            }

            return user?.IdRole switch
            {
                1 => "admin",
                2 => "user",
                _ => null
            };
        }

        private static string? NormalizeRoleName(string? roleName)
        {
            return string.IsNullOrWhiteSpace(roleName)
                ? null
                : roleName.Trim().ToLowerInvariant();
        }
    }
}
