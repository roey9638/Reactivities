using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController: ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => 
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>()
                ?? throw new InvalidOperationException("IMediator service is unavailable");

        // I'm calling this [Function] in the [ActivitiesController]
        // What it does is [Checking] the [Result] we got back from the [ActivitiesController].
        // I'ts goes like this for [Example] VVV
        // 1) [CreateActivity] [Checking] For [Errors] and it will [Send] either [Result<string>.Failure] OR [Result<string>.Success]
        // 2) Then we [Pass] the [Result] to the [HandleResult] [Function] that is inside the [ActivitiesController]
        // 3) Then what ever the [Result] is will send [Http Response] like [NotFound()] or [BadRequest()]
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (!result.IsSuccess && result.Code == 404)
            {
                return NotFound();
            }

            if (result.IsSuccess && result.Value != null)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}