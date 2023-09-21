using CafeExtensions.Exceptions;
using CafeExtensions.SimpleModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace CafeExtensions.Middleware;
/// <summary>
/// Catch exception
/// </summary>
public class ExceptionHandlingMiddleware : IMiddleware
{
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	/// <summary>
	/// ctor
	/// </summary>
	/// <param name="logger"></param>
	public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
	{
		_logger = logger;
	}

	/// <summary>
	/// implementation
	/// </summary>
	/// <param name="context"></param>
	/// <param name="next"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (ValidateErrorException e) // return 400
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

			SimpleAnswer answer = new()
			{
				State = false,
				Error = e.Message,
			};

			string json = JsonSerializer.Serialize(answer);
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(json);
		}
		catch (Exception e) // return 500
		{
			_logger.LogError(e, e.Message ?? string.Empty);

			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			ProblemDetails answer = new()
			{
				Status = (int)HttpStatusCode.InternalServerError,
				Type = "Server error",
				Title = "Server error",
				Detail = "An internal server has occurred"
			};

			string json = JsonSerializer.Serialize(answer);
			context.Response.ContentType = "application/json";
			await context.Response.WriteAsync(json);
		}
	}
}