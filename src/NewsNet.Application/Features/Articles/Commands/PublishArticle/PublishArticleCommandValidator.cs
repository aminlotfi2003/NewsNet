using FluentValidation;

namespace NewsNet.Application.Features.Articles.Commands.PublishArticle;

public sealed class PublishArticleCommandValidator : AbstractValidator<PublishArticleCommand>
{
    public PublishArticleCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}
