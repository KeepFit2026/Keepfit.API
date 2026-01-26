using System.Reflection;
using System.Text;
using KeepFit.Backend.API.Filter;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.Mapping;
using KeepFit.Backend.Application.Services;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.User;
using KeepFit.Backend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        RoleClaimType = "roleId"
    };
});

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

builder.Services.AddControllers();

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IGenericService<Exercise>, GenericService<Exercise>>();

builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IGenericService<FitnessProgram>, GenericService<FitnessProgram>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenericService<User>, GenericService<User>>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/api/v1/health-check");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeepFit API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();