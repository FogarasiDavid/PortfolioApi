using FluentValidation;
using Portfolio.Application.DTOs;

namespace Portfolio.Api.Validators
{
    public class CreateProjectRequestValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectRequestValidator() 
        {
            //nemlehet üres-max 100 betü
            RuleFor(x => x.Name).NotEmpty()
                .MaximumLength(100);
            //leiras nemlehet ures --max 5000 betu
            RuleFor(x => x.Description).NotEmpty()
                .MaximumLength(5000)
                .WithMessage("A leírás nemlehet ilyen hosszú!");

            //nemlehet ures url-- mindenkepp https://github.com/...-al kell kezdodnie
            RuleFor(x => x.GitHubUrl).NotEmpty().WithMessage("Nemlehet üres az url!")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("Érvényes url-t adj, Pl:https://github.com/...");
        }
    }
}
