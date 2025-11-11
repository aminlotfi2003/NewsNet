using MediatR;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Queries.ListArticles;

public sealed record ListArticlesQuery(int Page = 1, int PageSize = 20) : IRequest<PagedList<ArticleDto>>;
