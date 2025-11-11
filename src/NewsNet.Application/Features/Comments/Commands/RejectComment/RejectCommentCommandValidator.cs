using FluentValidation;

namespace NewsNet.Application.Features.Comments.Commands.RejectComment;

public sealed class RejectCommentCommandValidator : AbstractValidator<RejectCommentCommand>
{
    public RejectCommentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
