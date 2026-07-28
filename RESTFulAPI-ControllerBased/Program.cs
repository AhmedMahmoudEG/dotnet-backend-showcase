using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling= Newtonsoft.Json.NullValueHandling.Ignore;
});

builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
