using Application.Activities.Commands;
using Application.Activities.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController: BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await Mediator.Send(new GetActivityList.Query());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivityDetail(string id)
        {
            return await Mediator.Send(new GetActivityDetails.Query{ Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity(Activity activity)
        {
            // Usually [Command] not [returning] from are [Handler] in the [CreateActivity Class] Because [Command] DO NOT [return] anything.
            // But In [this case] i do [return] the [activity.Id] because we [creating] the [Id] Here in the [API]
            return await Mediator.Send(new CreateActivity.Command { Activity = activity });
        }

        [HttpPut]
        public async Task<ActionResult> EditActivity(Activity activity)
        {
            // I'm not [returning] from are [Handler] in the [Create Class] Because [Command] DO NOT [return] anything.
            await Mediator.Send(new EditActivity.Command { Activity = activity });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(string id)
        {
            await Mediator.Send(new DeleteActivity.Command {Id = id});

            return Ok();
        }
    }
}