using System;
using Application.Activities.Commands;
using Application.Activities.DTOs;

namespace Application.Validators;

// We [Passing] the [CreateActivity.Command] Because VVV
// We want to [Validate] the [CreateActivityDto] that is [inside] the [CreateActivity.Command] 
public class CreateActivityValidator : BaseActivityValidator<CreateActivity.Command, CreateActivityDto>
{
    public CreateActivityValidator() : base(x => x.ActivityDto)
    {
        
    }
}
