using SteamApplication.Helpers;
using SteamApplication.Models.Response;
using SteamDomain.Exceptions;
using SteamShared.Constants;

namespace Steam.Web.Api.Middleware
{
    public class ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger) : IMiddleware // Se define como un middleware para manejar errores en la aplicación
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException exception)
            {
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status404NotFound));
            }
            catch (BadRequestException exception)
            {
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status400BadRequest));
            }
            catch (UnauthorizedException exception)
            {
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status401Unauthorized));
            }
            catch (Exception exception)
            {
                var traceId = Guid.NewGuid();
                var message = ResponseConstants.ErrorUnexpected(traceId.ToString());

                logger.LogInformation("Se generó una excepción no controlada, con el traceId: {traceId}. Excepción: {exception}", traceId, exception);

                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status500InternalServerError, message));
            }

        }

        public GenericResponse<string> ManageException(HttpContext context, Exception exception, int statusCode, string? message = null)
        {
            var response = ResponseHelper.Create(
                data: message ?? exception.Message,
                message: message ?? exception.Message,
                errors: [message ?? exception.Message]
                );

            context.Response.StatusCode = statusCode;
            return response;
        }

    }
}
