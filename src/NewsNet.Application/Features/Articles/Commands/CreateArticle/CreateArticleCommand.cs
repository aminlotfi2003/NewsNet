using MediatR;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Commands.CreateArticle;

public sealed record CreateArticleCommand(
    string Title,
    string? Summary,
    string Content,
    Guid AuthorId,
    string? Slug) : IRequest<ArticleDto>;
