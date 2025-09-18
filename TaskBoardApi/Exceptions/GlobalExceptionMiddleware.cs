using TaskBoardApi.Models.Domain;

namespace TaskBoardApi.Exceptions
{
    public class GlobalExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                // log at Warning (expected business failure)
                _logger.LogWarning("Domain error {@ErrorCode} - {@Message}", ex.ErrorCode, ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex.ErrorCode switch
                {
                    "PROJECT_NOT_FOUND" => StatusCodes.Status404NotFound,
                    "USER_NOT_FOUND" => StatusCodes.Status404NotFound,
                    "MEMBER_ALREADY_EXISTS" => StatusCodes.Status409Conflict,
                    "PROJECT_NAME_ALREADY_EXISTS"=> StatusCodes.Status409Conflict,
                    "OWNER_ALREADY_EXISTS"  => StatusCodes.Status404NotFound,
                    "OWNER_ALREADY_ASSIGNED" => StatusCodes.Status409Conflict,
                    "UserNotFoundException" => StatusCodes.Status404NotFound,
                    "MemberNotFoundException" => StatusCodes.Status404NotFound,
                    "MemberAlreadyExistsException" => StatusCodes.Status409Conflict,

                    _ => StatusCodes.Status400BadRequest
                };

                await context.Response.WriteAsJsonAsync(new
                {
                    code = ex.ErrorCode,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // log at Error (unexpected crash)
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    code = "INTERNAL_ERROR",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
