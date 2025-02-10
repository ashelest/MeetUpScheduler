using System.Net;
using Application.Contracts;

namespace WebAPI;

internal class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var errorKey = Guid.NewGuid()
			.GetHashCode()
			.ToString("X");

		using var scope = logger.BeginScope(new { ErrorKey = errorKey });

		var statusCode = exception switch
		{
			NotImplementedException => HttpStatusCode.NotImplemented,
			TimeoutException => HttpStatusCode.GatewayTimeout,
			_ => HttpStatusCode.InternalServerError
		};

		var message = exception.Message;

		logger.LogError(exception, "An unhandled server error has occurred.");

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)statusCode;

		var errorResponse = new ErrorModel
		{
			Message = message
		};

		await context.Response.WriteAsJsonAsync(errorResponse);
	}
}