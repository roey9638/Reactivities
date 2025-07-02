using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Activities.Commands;
using Application.Activities.Queries;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class CommentHub(IMediator mediator) : Hub
    {

        public async Task SendComment(AddComment.Command command)
        {
            var comment = await mediator.Send(command);

            // This [sends] the [new comment] to [all] [users] who are in a [group] [identified] by the [ActivityId]
            await Clients.Group(command.ActivityId).SendAsync("ReceiveComment", comment.Value);
        }


        /* 
            The [goal] of this is [when] a [client] [connects] Will [add] [them] to a [SignalR] [group] [based] on the [activity ID].
            And then [when] a [new comment] is [added] for that [particular] [activity]. Then we're going to [send] it to [all] of the [connected] [clients] in that [group].
        */
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();

            var activityId = httpContext?.Request.Query["activityId"];

            if (string.IsNullOrEmpty(activityId)) throw new HubException("Not Activity With This Id");

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId!);

            var result = await mediator.Send(new GetComments.Query { ActivityId = activityId! });

            // Here I'm [Sending] to the [Connected] [Client] to this [SignalR Hub] the [List] of [Comments]
            // The ["LoadComments"] is [Very Important]. Because it's what we [gonna] [use] on the [client]
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}