global using webblogapi.Data;
global using webblogapi.Models;
global using webblogapi.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using webblogapi.Services.CommentService;
using webblogapi.Services.FileService;
using webblogapi.Services.FollowerService;
using webblogapi.Services.PostService;
using webblogapi.Services.UserService;
using webblogapi.Services.VoteService;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Blog API", Version = "v1" });
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Disable automatic model validation
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddScoped<IVoteService, VoteService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddDbContext<DataContext>();

var configuration = builder.Configuration;

var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:SecretKey").Value!);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddHealthChecks();
var app = builder.Build();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Hello World!");
});

app.MapHealthChecks("/health");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web Blog API v1");
    });
}
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
