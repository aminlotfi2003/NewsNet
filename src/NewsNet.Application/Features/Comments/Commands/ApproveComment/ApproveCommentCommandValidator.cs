using FluentValidation;

namespace NewsNet.Application.Features.Comments.Commands.ApproveComment;

public sealed class ApproveCommentCommandValidator : AbstractValidator<ApproveCommentCommand>
{
    public ApproveCommentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
