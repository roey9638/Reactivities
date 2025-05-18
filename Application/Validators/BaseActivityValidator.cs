using Application.Activities.DTOs;
using FluentValidation;

namespace Application.Validators
{
    // This [AbstractValidator] is to [Validate] [everything] inside in this case the [CreateActivity.Command]
    // The [<T>] in the [BaseActivityValidator] is what we want to [Validate]. For Example the [CreateActivity.Command].
    // The [<TDto>] in the [BaseActivityValidator] means that the type [TDto] must be a [subclass] of, or the same as, [BaseActivityDto] VVV 
    // In other words, [TDto] [must] [inherit] from [BaseActivityDto].
    public class BaseActivityValidator<T, TDto> : AbstractValidator<T> where TDto : BaseActivityDto
    {
        public BaseActivityValidator(Func<T, TDto> selector)
        {
            RuleFor(x => selector(x).Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters");
            RuleFor(x => selector(x).Description)
                .NotEmpty().WithMessage("Description is required");
            RuleFor(x => selector(x).Date)
                .GreaterThan(DateTime.UtcNow).WithMessage("Date must be in the future");
            RuleFor(x => selector(x).Category)
                .NotEmpty().WithMessage("Category is required");
            RuleFor(x => selector(x).City)
                .NotEmpty().WithMessage("City is required");
            RuleFor(x => selector(x).Venue)
                .NotEmpty().WithMessage("Venue is required");
            RuleFor(x => selector(x).Latitude)
                .NotEmpty().WithMessage("Latitude is required")
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");
            RuleFor(x => selector(x).Longitude)
                .NotEmpty().WithMessage("Longitude is required")
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
        }
    }
}