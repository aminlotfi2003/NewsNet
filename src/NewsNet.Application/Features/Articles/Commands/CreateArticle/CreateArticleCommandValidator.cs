using FluentValidation;

namespace NewsNet.Application.Features.Articles.Commands.CreateArticle;

public sealed class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(160);

        RuleFor(c => c.Content)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(c => c.AuthorId)
            .NotEmpty();

        RuleFor(c => c.Slug)
            .MaximumLength(160)
            .Matches("^[a-z0-9\\- ]*$").When(c => !string.IsNullOrWhiteSpace(c.Slug));
    }
}
