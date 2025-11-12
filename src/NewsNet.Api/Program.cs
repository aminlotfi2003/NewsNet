using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NewsNet.Application.DependencyInjection;
using NewsNet.Infrastructure.Authentication;
using NewsNet.Infrastructure.DependencyInjection;
using NewsNet.Infrastructure.Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("JWT settings are not configured.");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    if (string.IsNullOrWhiteSpace(jwtSettings.SigningKey))
        throw new InvalidOperationException("JWT signing key is not configured.");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey)),
        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtSettings.Issuer),
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = !string.IsNullOrWhiteSpace(jwtSettings.Audience),
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

await app.Services.InitialiseDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
