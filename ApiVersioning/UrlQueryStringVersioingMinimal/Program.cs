using Asp.Versioning;
using URLPATHVERSIONINGMINIMAL.Endpoints.V1;
using URLPATHVERSIONINGMINIMAL.Endpoints.V2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader= new QueryStringApiVersionReader("api-version");
});
var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

app.MapProductEndPointsV1(apiVersionSet);
app.MapProductEndPointsV2(apiVersionSet);

app.Run();
