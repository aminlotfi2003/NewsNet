using MediatR;
using Microsoft.AspNetCore.Mvc;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Comments;
using NewsNet.Application.Features.Comments.Commands.ApproveComment;
using NewsNet.Application.Features.Comments.Commands.CreateComment;
using NewsNet.Application.Features.Comments.Commands.RejectComment;
using NewsNet.Application.Features.Comments.Queries.ListCommentsByArticle;

namespace NewsNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CommentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetByArticle), new { articleId = comment.ArticleId, page = 1, pageSize = 1 }, comment);
    }

    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CommentDto>> Approve(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ApproveCommentCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CommentDto>> Reject(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RejectCommentCommand(id), cancellationToken);
        return Ok(result);
    }

    [HttpGet("article/{articleId:guid}")]
    [ProducesResponseType(typeof(PagedList<CommentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedList<CommentDto>>> GetByArticle(
        Guid articleId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new ListCommentsByArticleQuery(articleId, page, pageSize), cancellationToken);
        return Ok(result);
    }
}
