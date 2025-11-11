using FluentValidation;

namespace NewsNet.Application.Features.Articles.Queries.ListArticles;

public sealed class ListArticlesQueryValidator : AbstractValidator<ListArticlesQuery>
{
    public ListArticlesQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThan(0);

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100);
    }
}
