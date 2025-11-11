using MediatR;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Comments;

namespace NewsNet.Application.Features.Comments.Queries.ListCommentsByArticle;

public sealed record ListCommentsByArticleQuery(Guid ArticleId, int Page = 1, int PageSize = 20)
    : IRequest<PagedList<CommentDto>>;
