using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using WebAPI.Models;

namespace WebAPI;

public class CustomExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		var errorResponse = new ErrorModel
		{
			Message = exception.Message
		};

		var statusCode = exception switch
		{
			NotImplementedException => HttpStatusCode.NotImplemented,
			TimeoutException => HttpStatusCode.GatewayTimeout,
			_ => HttpStatusCode.InternalServerError
		};

		httpContext.Response.ContentType = "application/json";
		httpContext.Response.StatusCode = (int)statusCode;

		await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken: cancellationToken);

		return true;
	}
}