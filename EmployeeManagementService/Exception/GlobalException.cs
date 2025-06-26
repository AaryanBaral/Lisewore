using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Cafe_Management_System.Exceptions;

public class GlobalException : IExceptionHandler
{
    private readonly ILogger<GlobalException> _logger;

    public GlobalException(ILogger<GlobalException> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        _logger.LogError(exception, "Error processing request on machine {MachineName}, TraceId: {TraceId}", Environment.MachineName, traceId);

        var (statusCode, title) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                { "traceId", traceId }
            }
        ).ExecuteAsync(context);

        return true;
    }

    private static (int statusCode, string title) MapException(Exception exception)
    {
        return exception switch
        {
            ArgumentOutOfRangeException => ((int)HttpStatusCode.BadRequest, exception.Message),
            ArgumentNullException => ((int)HttpStatusCode.BadRequest, exception.Message),
            ArgumentException => ((int)HttpStatusCode.BadRequest, exception.Message),
            UnauthorizedAccessException => ((int)HttpStatusCode.Forbidden, exception.Message),
            InvalidOperationException => ((int)HttpStatusCode.BadRequest, exception.Message),
            TimeoutException => ((int)HttpStatusCode.InternalServerError, exception.Message),
            DbUpdateException => ((int)HttpStatusCode.BadRequest, exception.Message),
            InvalidCastException => ((int)HttpStatusCode.BadRequest, exception.Message),
            FormatException => ((int)HttpStatusCode.BadRequest, exception.Message),
            KeyNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
            AuthenticationFailureException => ((int)HttpStatusCode.Unauthorized, exception.Message),
            RequestFailedException => ((int)HttpStatusCode.BadRequest, exception.Message),
            ValidationException => ((int)HttpStatusCode.BadRequest, exception.Message),
            DuplicateNameException => ((int)HttpStatusCode.BadRequest, exception.Message),
            SqlException sqlEx => HandleSqlException(sqlEx),
            _ => ((int)HttpStatusCode.InternalServerError, exception.Message),
        };
    }

    private static (int statusCode, string title) HandleSqlException(SqlException sqlEx)
    {
        return sqlEx.Number switch
        {
            2627 => ((int)HttpStatusCode.Conflict, "Duplicate key violation (Unique constraint failed)."),
            547 => ((int)HttpStatusCode.BadRequest, "Foreign key constraint violation."),
            2601 => ((int)HttpStatusCode.Conflict, "Cannot insert duplicate key."),
            1205 => ((int)HttpStatusCode.InternalServerError, "Deadlock detected."),
            4060 => ((int)HttpStatusCode.InternalServerError, "Cannot open database requested by the login."),
            18456 => ((int)HttpStatusCode.Unauthorized, "Login failed for user."),
            53 => ((int)HttpStatusCode.InternalServerError, "Cannot connect to the server."),
            -2 => ((int)HttpStatusCode.RequestTimeout, "SQL query timeout."),
            8152 => ((int)HttpStatusCode.BadRequest, "String or binary data would be truncated."),
            8115 => ((int)HttpStatusCode.BadRequest, "Arithmetic overflow error."),
            515 => ((int)HttpStatusCode.BadRequest, "Cannot insert NULL value into a non-nullable column."),
            9002 => ((int)HttpStatusCode.InternalServerError, "Transaction log is full."),
            _ => ((int)HttpStatusCode.InternalServerError, $"SQL Error {sqlEx.Number}: {sqlEx.Message}"),
        };
    }
}
