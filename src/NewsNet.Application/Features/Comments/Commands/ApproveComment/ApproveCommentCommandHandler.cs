using AutoMapper;
using MediatR;
using NewsNet.Application.Abstractions.Persistence;
using NewsNet.Application.Abstractions.Persistence.Repositories;
using NewsNet.Application.Common.Exceptions;
using NewsNet.Application.Contracts.Comments;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Features.Comments.Commands.ApproveComment;

public sealed class ApproveCommentCommandHandler : IRequestHandler<ApproveCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApproveCommentCommandHandler(
        ICommentRepository commentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(ApproveCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.Id, cancellationToken);

        if (comment is null)
            throw new NotFoundException(nameof(Comment), request.Id);

        comment.Approve();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CommentDto>(comment);
    }
}
