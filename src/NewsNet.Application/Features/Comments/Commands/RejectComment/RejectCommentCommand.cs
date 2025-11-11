using MediatR;
using NewsNet.Application.Contracts.Comments;

namespace NewsNet.Application.Features.Comments.Commands.RejectComment;

public sealed record RejectCommentCommand(Guid Id) : IRequest<CommentDto>;
