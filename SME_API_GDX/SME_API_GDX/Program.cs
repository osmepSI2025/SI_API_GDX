using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SME_API_GDX.Entities;
using SME_API_GDX.Repository;
using SME_API_GDX.Services;
using Microsoft.AspNetCore.Mvc; // Ensure this namespace is included
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<GDXDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConnectionString"),
        sqlOptions => sqlOptions.EnableRetryOnFailure() // Add this line
    )
);
//Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// ✅ Register NSwag (Swagger 2.0)
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "API_SME_GDX_V1";
    config.Title = "API SME GDX";
    config.Version = "v1";
    config.Description = "API documentation using Swagger 2.0";
    config.SchemaType = NJsonSchema.SchemaType.Swagger2; // This makes it Swagger 2.0
});

builder.Services.AddScoped<MOrganizationJuristicPersonRepository>();
builder.Services.AddScoped<MOrganizationJuristicPersonService>();

builder.Services.AddScoped<IApiInformationRepository, ApiInformationRepository>();
builder.Services.AddScoped<ICallAPIService, CallAPIService>(); // Register ICallAPIService with CallAPIService
builder.Services.AddHttpClient<CallAPIService>();
var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseOpenApi();     // Serve the Swagger JSON
    app.UseSwaggerUi3();  // Use Swagger UI v3
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
