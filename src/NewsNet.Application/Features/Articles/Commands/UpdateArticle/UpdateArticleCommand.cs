using MediatR;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Commands.UpdateArticle;

public sealed record UpdateArticleCommand(
    Guid Id,
    string Title,
    string? Summary,
    string Content,
    string? Slug) : IRequest<ArticleDto>;
