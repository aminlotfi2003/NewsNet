using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Comments;
using NewsNet.Domain.Abstractions;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Features.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IArticleRepository _articleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        IArticleRepository articleRepository,
        IUnitOfWork unitOfWork,
        IClock clock,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _articleRepository = articleRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
        _mapper = mapper;
    }

    public Task<CommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var article = await _articleRepository.GetByIdAsync(request.ArticleId, ct);
            if (article is null)
                throw new NotFoundException(nameof(Article), request.ArticleId);
            var comment = Comment.Create(
                request.ArticleId,
                request.UserId,
                request.Body,
                _clock.UtcNow
            );
            await _commentRepository.AddAsync(comment, ct);
            return _mapper.Map<CommentDto>(comment);
        }, cancellationToken);
    }
}
