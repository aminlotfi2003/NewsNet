using MediatR;
using NewsNet.Application.Contracts.Comments;

namespace NewsNet.Application.Features.Comments.Commands.CreateComment;

public sealed record CreateCommentCommand(
    Guid ArticleId,
    Guid UserId,
    string Body
) : IRequest<CommentDto>;
