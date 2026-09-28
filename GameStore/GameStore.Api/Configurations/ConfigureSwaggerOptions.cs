using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GameStore.Api.Configurations;
public class ConfigureSwaggerOptions:IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this.provider=provider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        foreach(var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title="GameStore.Api",
                    Version=description.ApiVersion.ToString()
                });
        }
    }
}