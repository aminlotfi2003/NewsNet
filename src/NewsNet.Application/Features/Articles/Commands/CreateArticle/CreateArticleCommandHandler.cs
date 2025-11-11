using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Application.Features.Articles.Commands.CreateArticle;

public sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueSlugChecker _uniqueSlugChecker;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public CreateArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork,
        IUniqueSlugChecker uniqueSlugChecker,
        IClock clock,
        IMapper mapper)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _uniqueSlugChecker = uniqueSlugChecker;
        _clock = clock;
        _mapper = mapper;
    }

    public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var slugValue = string.IsNullOrWhiteSpace(request.Slug) ? request.Title : request.Slug!;

        var slug = await Slug.CreateAsync(slugValue, _uniqueSlugChecker, cancellationToken);

        var article = Article.Create(
            slug,
            request.Title,
            request.Summary,
            request.Content,
            request.AuthorId,
            _clock.UtcNow
        );

        await _articleRepository.AddAsync(article, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ArticleDto>(article);
    }
}
