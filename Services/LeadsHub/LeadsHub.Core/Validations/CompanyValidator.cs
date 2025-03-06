
using FluentValidation;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Validations
{
    public sealed class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("The Legal Name is required.")
                .SetValidator(new AddressValidator()); ;

            RuleFor(x => x.BrandName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Brand Name is required.");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Email is required.");

            RuleFor(x => x.IdentificationNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Identification number is required.");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Phone Number is required.");                   

            RuleFor(x => x.LegalName)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Legal Name is required.");
        }
    }
}
