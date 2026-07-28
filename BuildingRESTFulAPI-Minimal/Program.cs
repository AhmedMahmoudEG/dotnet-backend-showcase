using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
builder.Services.Configure<JsonOptions>(options=>
options.SerializerOptions.DefaultIgnoreCondition= System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
var app = builder.Build();

app.MapProductEndpoints();
app.Run();
