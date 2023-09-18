using FluentValidation;

namespace Backend.DtoModels
{
    public record InsertRandomNamesReq
    {
        public List<string> Names { get; set; }
    }
    public sealed class InsertRandomNamesReqValidator : AbstractValidator<InsertRandomNamesReq>
    {
        public InsertRandomNamesReqValidator()
        {


            RuleFor(x => x.Names)
                .NotNull().WithMessage("names is required")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Names)
                        .Must(names => names?.Count == 10).WithMessage("10 names are required");
                });
        }
    }
}
