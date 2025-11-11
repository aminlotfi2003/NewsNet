using MediatR;
using NewsNet.Application.Contracts.Comments;

namespace NewsNet.Application.Features.Comments.Commands.ApproveComment;

public sealed record ApproveCommentCommand(Guid Id) : IRequest<CommentDto>;
