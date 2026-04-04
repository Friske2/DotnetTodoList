namespace Todolist.Exceptions;

/// <summary>
/// Base class for all HTTP exceptions
/// </summary>
public abstract class HttpException(int statusCode, string message, string? details = null)
    : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string? Details { get; } = details;
}

// ─── 4xx Client Errors ───────────────────────────────────────────────────────

/// <summary>400 Bad Request</summary>
public class BadRequestException(string message, string? details = null)
    : HttpException(400, message, details);

/// <summary>401 Unauthorized</summary>
public class UnauthorizedException(string message = "Unauthorized", string? details = null)
    : HttpException(401, message, details);

/// <summary>403 Forbidden</summary>
public class ForbiddenException(string message = "Forbidden", string? details = null)
    : HttpException(403, message, details);

/// <summary>404 Not Found</summary>
public class NotFoundException(string message, string? details = null)
    : HttpException(404, message, details);

/// <summary>405 Method Not Allowed</summary>
public class MethodNotAllowedException(string message = "Method Not Allowed", string? details = null)
    : HttpException(405, message, details);

/// <summary>408 Request Timeout</summary>
public class RequestTimeoutException(string message = "Request Timeout", string? details = null)
    : HttpException(408, message, details);

/// <summary>409 Conflict</summary>
public class ConflictException(string message, string? details = null)
    : HttpException(409, message, details);

/// <summary>410 Gone</summary>
public class GoneException(string message, string? details = null)
    : HttpException(410, message, details);

/// <summary>422 Unprocessable Entity</summary>
public class UnprocessableEntityException(string message, string? details = null)
    : HttpException(422, message, details);

/// <summary>429 Too Many Requests</summary>
public class TooManyRequestsException(string message = "Too Many Requests", string? details = null)
    : HttpException(429, message, details);

// ─── 5xx Server Errors ───────────────────────────────────────────────────────

/// <summary>500 Internal Server Error</summary>
public class InternalServerErrorException(string message = "Internal Server Error", string? details = null)
    : HttpException(500, message, details);

/// <summary>501 Not Implemented</summary>
public class NotImplementedException(string message = "Not Implemented", string? details = null)
    : HttpException(501, message, details);

/// <summary>502 Bad Gateway</summary>
public class BadGatewayException(string message = "Bad Gateway", string? details = null)
    : HttpException(502, message, details);

/// <summary>503 Service Unavailable</summary>
public class ServiceUnavailableException(string message = "Service Unavailable", string? details = null)
    : HttpException(503, message, details);

/// <summary>504 Gateway Timeout</summary>
public class GatewayTimeoutException(string message = "Gateway Timeout", string? details = null)
    : HttpException(504, message, details);

// ─── Database Errors ──────────────────────────────────────────────────────────

/// <summary>503 - Cannot connect to the database</summary>
public class DatabaseConnectionException(string? details = null)
    : HttpException(503, "Database connection failed", details);

/// <summary>500 - Query or command failed</summary>
public class DatabaseQueryException(string? details = null)
    : HttpException(500, "Database query failed", details);

/// <summary>500 - Missing / invalid connection string</summary>
public class DatabaseConfigurationException(string? details = null)
    : HttpException(500, "Database configuration error", details);

