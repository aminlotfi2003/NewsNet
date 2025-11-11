using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Features.Articles.Queries.GetArticleById;

public sealed class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IMapper _mapper;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository, IMapper mapper)
    {
        _articleRepository = articleRepository;
        _mapper = mapper;
    }

    public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (article is null)
            throw new NotFoundException(nameof(Article), request.Id);

        return _mapper.Map<ArticleDto>(article);
    }
}
