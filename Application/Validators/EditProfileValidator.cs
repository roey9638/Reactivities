using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Application.Profiles.Commands;
using FluentValidation;

namespace Application.Validators
{
    public class EditProfileValidator : AbstractValidator<EditProfile.Command>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}