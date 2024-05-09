using Microsoft.AspNetCore.Mvc;
using NLog;
using Vigig.Api.Constants;
using Vigig.Common.Exceptions;
using Vigig.Common.Helpers;
using Vigig.Service.Models;
using ILogger = NLog.ILogger;

namespace Vigig.Api.Controllers.Base;

public abstract class BaseApiController : ControllerBase
{
    private readonly ILogger logger = LogManager.GetLogger(AppDomain.CurrentDomain.FriendlyName);

    private IActionResult BuildSuccessResult(ServiceActionResult result)
    {
        var successResult = new ApiResponse(true)
        {
            Data = result.Data,
            StatusCode = result.StatusCode != default ? result.StatusCode : StatusCodes.Status200OK
        };

        var detail = result.Detail ?? ApiMessageConstants.Success;
        successResult.AddSuccessMessage(detail);
        return base.Ok(successResult);
    }

    private IActionResult BuildErrorResult(Exception e)
    {
        var errorResult = new ApiResponse(false);
        errorResult.AddErrorMessage(e.Message);

        var statusCode = StatusCodes.Status500InternalServerError;
        if (e.GetType().IsAssignableTo(typeof(INotFoundException)))
            statusCode = StatusCodes.Status404NotFound;
        else if (e.GetType().IsAssignableTo(typeof(IBusinessException)))
            statusCode = StatusCodes.Status409Conflict;
        else if (e.GetType().IsAssignableTo(typeof(IForbiddenException)))
            statusCode = StatusCodes.Status403Forbidden;
        errorResult.StatusCode = statusCode;
        return base.Ok(errorResult);
    }
    protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceActionFunc)
    {
        return await ExecuteServiceLogic(serviceActionFunc, null);
    }
    protected async Task<IActionResult> ExecuteServiceLogic(Func<Task<ServiceActionResult>> serviceActionFunc,
        Func<Task<ServiceActionResult>>? errorHandler)
    {
        var startTime = DateTime.Now;
        StringInterpolationHelper.AppendToStart(serviceActionFunc.Method.Name);
        var methodInfo = StringInterpolationHelper.BuildAndClear();
        logger.Info($"\n[Start] [API-Method] - {methodInfo}");
        try
        {
            var result = await serviceActionFunc();
            StringInterpolationHelper.Append("Result of [[");
            StringInterpolationHelper.Append(methodInfo);
            StringInterpolationHelper.Append($"]]. IsSuccess = {result.IsSuccess}");
            StringInterpolationHelper.Append($". Details: ");
            StringInterpolationHelper.Append(result.Detail ?? "no detail.");
            logger.Info(StringInterpolationHelper.BuildAndClear());
            return result.IsSuccess ? BuildSuccessResult(result) : Problem(result.Detail);
        }
        catch (Exception e)
        {
            var message = $"Result of {methodInfo}. IsSuccess = false. Details: {e.Message} at Specified file: \n {e.StackTrace}";
            logger.Error(message);
            return BuildErrorResult(e);
        }
        finally
        {
            StringInterpolationHelper.AppendToStart($"[END] - {methodInfo}. ");
            StringInterpolationHelper.Append("Total: ");
            StringInterpolationHelper.Append((DateTime.Now - startTime).Milliseconds.ToString());
            StringInterpolationHelper.Append(" ms.");
            logger.Info(StringInterpolationHelper.BuildAndClear());
        }
    }
}