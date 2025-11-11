using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Comments;

namespace NewsNet.Application.Features.Comments.Queries.ListCommentsByArticle;

public sealed class ListCommentsByArticleQueryHandler
    : IRequestHandler<ListCommentsByArticleQuery, PagedList<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public ListCommentsByArticleQueryHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<CommentDto>> Handle(ListCommentsByArticleQuery request, CancellationToken cancellationToken)
    {
        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (items, totalCount) = await _commentRepository.GetPagedByArticleAsync(
            request.ArticleId,
            page,
            pageSize,
            cancellationToken);

        if (items.Count == 0 && totalCount == 0)
            return PagedList<CommentDto>.Empty(page, pageSize);

        var dtoItems = _mapper.Map<IReadOnlyList<CommentDto>>(items);

        return new PagedList<CommentDto>(dtoItems, page, pageSize, totalCount);
    }
}
