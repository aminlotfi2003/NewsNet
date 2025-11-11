using FluentValidation;

namespace NewsNet.Application.Features.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.ArticleId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Body)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(2000);
    }
}
