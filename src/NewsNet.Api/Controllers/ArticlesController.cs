using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsNet.Application.Common.Models;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Application.Features.Articles.Commands.CreateArticle;
using NewsNet.Application.Features.Articles.Commands.PublishArticle;
using NewsNet.Application.Features.Articles.Commands.UpdateArticle;
using NewsNet.Application.Features.Articles.Queries.GetArticleById;
using NewsNet.Application.Features.Articles.Queries.ListArticles;

namespace NewsNet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ArticlesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(PagedList<ArticleDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedList<ArticleDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new ListArticlesQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetArticleByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ArticleDto>> Create([FromBody] CreateArticleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var article = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleDto>> Update(Guid id, [FromBody] UpdateArticleCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var updateCommand = command with { Id = id };
        var article = await _mediator.Send(updateCommand, cancellationToken);
        return Ok(article);
    }

    [HttpPost("{id:guid}/publish")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ArticleDto>> Publish(Guid id, CancellationToken cancellationToken)
    {
        var article = await _mediator.Send(new PublishArticleCommand(id), cancellationToken);
        return Ok(article);
    }
}
