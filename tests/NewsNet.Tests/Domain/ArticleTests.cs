using FluentAssertions;
using NewsNet.Domain.Common;
using NewsNet.Domain.Entities;
using NewsNet.Domain.Enums;
using NewsNet.Domain.ValueObjects;

namespace NewsNet.Tests.Domain;

public class ArticleTests
{
    [Fact]
    public void Create_should_set_defaults_and_validate()
    {
        var slug = Slug.Create("my-article");
        var content = new string('x', 60);
        var a = Article.Create(slug, "Title OK", "Summary", content, new[] { "news", "dotnet" }, Guid.NewGuid(), DateTimeOffset.UtcNow);

        a.Status.Should().Be(ArticleStatus.Draft);
        a.Tags.Should().HaveCount(2);
    }

    [Fact]
    public void Create_should_throw_on_short_content()
    {
        var slug = Slug.Create("abc");
        var act = () => Article.Create(slug, "Title OK", null, "short", Array.Empty<string>(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("article.content.length");
    }

    [Fact]
    public void Update_should_apply_and_validate_tags_constraints()
    {
        var a = Article.Create(Slug.Create("slug"), "Long Title", null, new string('y', 60), new[] { "a" }, Guid.NewGuid(), DateTimeOffset.UtcNow);
        a.Update("Another Title", "S", new string('z', 60), Enumerable.Repeat("t", 10), DateTimeOffset.UtcNow);

        a.Title.Should().Be("Another Title");
        a.Tags.Should().HaveCount(10);

        var act = () => a.Update("Another Title", null, new string('z', 60), Enumerable.Repeat("t", 11), DateTimeOffset.UtcNow);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("article.tags.count");
    }

    [Fact]
    public void Publish_should_set_status_and_publishedAt_and_guard_against_double_publish()
    {
        var a = Article.Create(Slug.Create("slug"), "Title Okay", null, new string('c', 80), Array.Empty<string>(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        var now = DateTimeOffset.UtcNow;

        a.Publish(now);
        a.Status.Should().Be(ArticleStatus.Published);
        a.PublishedAt.Should().Be(now);

        var act = () => a.Publish(now);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("article.publish.already");
    }

    [Fact]
    public void ChangeSlug_should_validate_kebab_case_via_value_object()
    {
        var a = Article.Create(Slug.Create("old-slug"), "Title Okay", null, new string('c', 80), Array.Empty<string>(), Guid.NewGuid(), DateTimeOffset.UtcNow);

        var act = () => a.ChangeSlug(Slug.Create("Not-Kebab"), DateTimeOffset.UtcNow);
        act.Should().Throw<DomainException>().Which.Code.Should().Be("slug.format");
    }
}
