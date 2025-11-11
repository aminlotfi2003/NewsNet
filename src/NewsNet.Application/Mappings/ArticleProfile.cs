using AutoMapper;
using NewsNet.Application.Contracts.Articles;
using NewsNet.Domain.Entities;

namespace NewsNet.Application.Mappings;

public sealed class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleDto>()
            .ForMember(d => d.Slug, opt => opt.MapFrom(src => src.Slug.Value));
    }
}
