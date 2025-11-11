using FluentValidation;

namespace NewsNet.Application.Features.Comments.Queries.ListCommentsByArticle;

public sealed class ListCommentsByArticleQueryValidator : AbstractValidator<ListCommentsByArticleQuery>
{
    public ListCommentsByArticleQueryValidator()
    {
        RuleFor(x => x.ArticleId)
            .NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
