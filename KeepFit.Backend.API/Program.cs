using System.Reflection;
using KeepFit.Backend.API.Filter;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.Mapping;
using KeepFit.Backend.Application.Services;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<GenericControllerOperationFilter>();
    
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Controllers
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Services
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IGenericService<Exercise>, GenericService<Exercise>>();

builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IGenericService<FitnessProgram>, GenericService<FitnessProgram>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Build the app
var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeepFit API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
