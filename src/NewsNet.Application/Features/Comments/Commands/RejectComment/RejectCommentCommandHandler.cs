using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Comments;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Features.Comments.Commands.RejectComment;

public sealed class RejectCommentCommandHandler : IRequestHandler<RejectCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RejectCommentCommandHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(RejectCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (comment is null)
            throw new NotFoundException(nameof(Comment), request.Id);

        comment.Reject();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentDto>(comment);
    }
}
