using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthorization();

// swagger
builder.Services.AddOpenApiDocument();
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                x => x.Key,
                x => x.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        var response = new
        {
            statusCode = 400,
            message = "Validation failed",
            errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services

builder.Services.AddScoped<ICageRepository, CageRepository>();
builder.Services.AddScoped<ICageService, CageService>();

builder.Services.AddScoped<IRatRespository, RatRespository>();
builder.Services.AddScoped<IRatService, RatService>();


// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateCageValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRatValidator>();

builder.Services.AddScoped<IValidationService, ValidationService>();

// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register AutoMapper
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile).Assembly
);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Add OpenAPI 3.0 document serving middleware
    // Available at: http://localhost:<port>/swagger/v1/swagger.json
    app.UseOpenApi();

    // Add web UIs to interact with the document
    // Available at: http://localhost:<port>/swagger
    app.UseSwaggerUi(); // UseSwaggerUI Protected by if (env.IsDevelopment())
}
app.UseMiddleware<ExceptionMiddleware>();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();