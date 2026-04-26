using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarehouseManagementSystem.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var userId = session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }

            base.OnActionExecuting(context);
        }

    }
}
