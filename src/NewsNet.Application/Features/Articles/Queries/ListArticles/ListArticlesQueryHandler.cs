using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Articles;

namespace NewsNet.Application.Features.Articles.Queries.ListArticles;

public sealed class ListArticlesQueryHandler : IRequestHandler<ListArticlesQuery, PagedList<ArticleDto>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public ListArticlesQueryHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<ArticleDto>> Handle(ListArticlesQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (items, totalCount) = await _articleRepository.GetPagedAsync(page, pageSize, cancellationToken);

        if (items.Count == 0 && totalCount == 0)
            return PagedList<ArticleDto>.Empty(page, pageSize);

        var dtoItems = _mapper.Map<IReadOnlyList<ArticleDto>>(items);

        return new PagedList<ArticleDto>(dtoItems, page, pageSize, totalCount);
    }
}
