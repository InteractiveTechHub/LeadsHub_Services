
using FluentValidation;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Validations
{
    public sealed class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("The city is required.");

            RuleFor(x => x.Neighborhood)
                .NotEmpty().WithMessage("The neighborhood is required.");

            RuleFor(x => x.State)
              .NotEmpty().WithMessage("The state is required.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("The street is required.");

            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("The zip code is required.");
        }
    }
}
