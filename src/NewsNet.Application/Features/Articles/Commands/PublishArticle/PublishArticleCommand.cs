using MediatR;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Commands.PublishArticle;

public sealed record PublishArticleCommand(Guid Id) : IRequest<ArticleDto>;
