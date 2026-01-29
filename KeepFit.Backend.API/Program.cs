using System.Reflection;
using System.Text;
using KeepFit.Backend.API.Filter;
using KeepFit.Backend.API.Hubs;
using KeepFit.Backend.Application.Contracts;
using KeepFit.Backend.Application.Mapping;
using KeepFit.Backend.Application.Services;
using KeepFit.Backend.Domain.Models;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLaravel", policy =>
    {
        policy.WithOrigins("http://localhost:8000", "http://127.0.0.1:8000") 
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Obligatoire pour SignalR
    });
});


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
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Tes services habituels
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));


builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

var app = builder.Build();


app.UseCors("AllowLaravel"); 

app.MapHealthChecks("/api/v1/health-check");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "KeepFit API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.MapHub<ChatHub>("/api/v1/hubs/chat");

app.Run();