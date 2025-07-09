using Application.Activities.Commands;
using Application.Activities.DTOs;
using Application.Activities.Queries;
using Application.Core;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<ActivityDto, DateTime?>>> GetActivities([FromQuery] ActivityParams activityParams)
        {
            return HandleResult(await Mediator.Send(new GetActivityList.Query { Params = activityParams }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityDetail(string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
        {
            // Usually [Command] not [returning] from are [Handler] in the [CreateActivity Class] Because [Command] DO NOT [return] anything.
            // But In [this case] i do [return] the [activity.Id] because we [creating] the [Id] Here in the [API]
            return HandleResult(await Mediator.Send(new CreateActivity.Command { ActivityDto = activityDto }));
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> EditActivity(string id, EditActivityDto activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto = activity }));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> DeleteActivity(string id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivity.Command { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend(string id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }));
        }
    }
}