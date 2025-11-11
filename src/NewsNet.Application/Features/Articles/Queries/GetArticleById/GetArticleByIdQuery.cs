using MediatR;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Queries.GetArticleById;

public sealed record GetArticleByIdQuery(Guid Id) : IRequest<ArticleDto>;
