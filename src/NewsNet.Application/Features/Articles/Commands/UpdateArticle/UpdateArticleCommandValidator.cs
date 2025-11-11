using FluentValidation;

namespace NewsNet.Application.Features.Articles.Commands.UpdateArticle;

public sealed class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
{
    public UpdateArticleCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();

        RuleFor(c => c.Title)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(160);

        RuleFor(c => c.Content)
            .NotEmpty()
            .MinimumLength(50);

        RuleFor(c => c.Slug)
            .MaximumLength(160)
            .Matches("^[a-z0-9\\- ]*$").When(c => !string.IsNullOrWhiteSpace(c.Slug));
    }
}
