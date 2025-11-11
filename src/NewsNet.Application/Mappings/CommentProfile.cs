using AutoMapper;
using NewsNet.Application.Contracts.Comments;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Mappings;

public sealed class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();
    }
}
