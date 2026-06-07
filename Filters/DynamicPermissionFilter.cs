using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Website_QuanLyKhoHangThucPham.Services;

namespace Website_QuanLyKhoHangThucPham.Filters
{
    // Filter này chặn mọi request đến controller có [DynamicPermission]
    // Kiểm tra user có role được phép truy cập controller/action đó không
    public class DynamicPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IPermissionService _permService;

        public DynamicPermissionFilter(IPermissionService permService)
        {
            _permService = permService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Chưa đăng nhập → chuyển về Login
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString() ?? "";
            var action     = context.RouteData.Values["action"]?.ToString() ?? "";
            var roles      = context.HttpContext.User.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            var allowed = await _permService.HasPermissionAsync(roles, controller, action);

            if (!allowed)
                context.Result = new ForbidResult();
        }
    }
}
