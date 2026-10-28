using System.Globalization;
using System.Security.Claims;
using System.Text;
using FinancialManager.Api.Converters;
using FinancialManager.Api.Filters;
using FinancialManager.Application;
using FinancialManager.Domain.Repositories.User;
using FinancialManager.Infrastructure;
using FinancialManager.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo> { new("en-US") };

    options.DefaultRequestCulture = new RequestCulture("en-US");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders = [ new AcceptLanguageHeaderRequestCultureProvider() ];
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>()); 

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        var signingKey = builder.Configuration.GetValue<string>("Jwt:SigningKey")!;

        jwtOptions.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        jwtOptions.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userId = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                             ?? context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrWhiteSpace(userId) == false)
                {
                    context.Fail("Invalid subject");

                    return;
                }

                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

                var existUser = await userRepository.ExistsActiveUserWithId(Guid.Parse(userId));
                if (existUser == false)
                {
                    context.Fail("Invalid user");
                }
            }
        };
    });

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigrations();

app.Run();

async Task ExecuteMigrations()
{
    await using var scope = app.Services.CreateAsyncScope();

    DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}

public partial class Program {}