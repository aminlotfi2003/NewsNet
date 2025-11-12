using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Application.Features.Articles.Commands.UpdateArticle;

public sealed class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueSlugChecker _uniqueSlugChecker;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public UpdateArticleCommandHandler(
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

    public Task<ArticleDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var article = await _articleRepository.GetByIdAsync(request.Id, ct);

            if (article is null)
                throw new NotFoundException(nameof(Article), request.Id);

            if (!string.IsNullOrWhiteSpace(request.Slug) && !request.Slug.Equals(article.Slug.Value, StringComparison.OrdinalIgnoreCase))
            {
                var slug = await Slug.CreateAsync(request.Slug, _uniqueSlugChecker, ct);
                article.ChangeSlug(slug, _clock.UtcNow);
            }
            article.Update(request.Title, request.Summary, request.Content, _clock.UtcNow);
            return _mapper.Map<ArticleDto>(article);
        }, cancellationToken);
    }
}
