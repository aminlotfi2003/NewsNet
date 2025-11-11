using FluentValidation;

namespace NewsNet.Application.Features.Articles.Queries.GetArticleById;

public sealed class GetArticleByIdQueryValidator : AbstractValidator<GetArticleByIdQuery>
{
    public GetArticleByIdQueryValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}
