using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Features.Articles.Commands.PublishArticle;

public sealed class PublishArticleCommandHandler : IRequestHandler<PublishArticleCommand, ArticleDto>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public PublishArticleCommandHandler(
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IMapper mapper)
    {
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _mapper = mapper;
    }

    public Task<ArticleDto> Handle(PublishArticleCommand request, CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var article = await _articleRepository.GetByIdAsync(request.Id, ct);
            if (article is null)
                throw new NotFoundException(nameof(Article), request.Id);
            article.Publish(_clock.UtcNow);
            return _mapper.Map<ArticleDto>(article);
        }, cancellationToken);
    }
}
