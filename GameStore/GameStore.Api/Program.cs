using GameStore.Api.Data;
using GameStore.Api.Repositories;
using GameStore.Api.Services;
using GameStore.Api.Mappings;
using GameStore.Api.Middleware;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using GameStore.Api.Models;
using Microsoft.OpenApi;
using GameStore.Api.Configurations;
using GameStore.Api.Filters;
using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Components.Web;
using Asp.Versioning.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerGen;
using GameStore.Api.Configurations;
using GameStore.Api.BackgoundServices;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
var xmlFile=$"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath=Path.Combine(AppContext.BaseDirectory,xmlFile);

builder.Services.AddSwaggerGen(options=>
{
    
    options.IncludeXmlComments(xmlPath);
    options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "Enter: Bearer {your JWT token}"
        
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("GameStore"),
        ServerVersion.AutoDetect(
            builder.Configuration.GetConnectionString("GameStore")
        )));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<LoggingActionFilter>();
builder.Services.AddControllers(options=>
{
    options.Filters.Add<LoggingActionFilter>();
});
builder.Services.AddScoped<AppVersionResultFilter>();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion=new ApiVersion(1,0);
    options.AssumeDefaultVersionWhenUnspecified=true;
    options.ReportApiVersions=true;
    options.ApiVersionReader=new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options=>{
    options.GroupNameFormat="'v'VVV";
    options.SubstituteApiVersionInUrl=true;
});
builder.Services.AddTransient<
IConfigureOptions<SwaggerGenOptions>
,ConfigureSwaggerOptions>();

builder.Services.AddScoped<IGameService,GameService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options=>
    options.TokenValidationParameters=new TokenValidationParameters{
        ValidateIssuerSigningKey=true,
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer=true,
        ValidIssuer=builder.Configuration["Jwt:Issuer"],
        ValidateAudience=true,
        ValidAudience=builder.Configuration["Jwt:Audience"],
        ValidateLifetime=true
    });
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddHostedService<CleanupService>();
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
    );
builder.Services.AddScoped<IRefreshTokenRepository,RefreshTokenRepository>();


var app = builder.Build();
app.UseSwagger();
var provider =
    app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
    }
});
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


