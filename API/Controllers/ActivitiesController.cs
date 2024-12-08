using Application.Activities;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            // The [Query] in the [Details] [Requires] an [Id].
            return await Mediator.Send(new Details.Query{ Id = id });; 
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            // I'm not [returning] from are [Handler] in the [Create Class] Because [Command] DO NOT [return] anything.
            await Mediator.Send(new Create.Command { Activity = activity });

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            // This [activity] will be [set] to the [activity] with the [id] we pass in.
            activity.Id = id;

            // I'm not [returning] from are [Handler] in the [Create Class] Because [Command] DO NOT [return] anything.
            await Mediator.Send(new Edit.Command { Activity = activity });

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            await Mediator.Send(new Delete.Command {Id = id});

            return Ok();
        }
    }
}