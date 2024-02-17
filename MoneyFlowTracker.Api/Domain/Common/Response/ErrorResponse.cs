namespace MoneyFlowTracker.Api.Common.Response;

public class ErrorResponse
{
    public required string Code { get; set; }
    public required string Message { get; set; }
}
