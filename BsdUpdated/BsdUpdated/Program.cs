using BsdUpdated.Data;
using BsdUpdated.IRepositories;
using BsdUpdated.IServices;
using BsdUpdated.Repositories;
using BsdUpdated.Services;
using BsdUpdated.Data;
using FinalProject.Repositories;
using FinalProject.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// NOTE: CORS removed temporarily for debugging
// builder.Services.AddCors(...);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()   // .AllowAnyOrigin() במקום .WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGiftService, GiftService>();
builder.Services.AddScoped<IGiftRepository, GiftRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IWinnerRepository, WinnerRepository>();
builder.Services.AddScoped<IWinnerService, WinnerService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<SaleContextFactory>();
builder.Services.AddScoped<SaleContext>();  // אם SaleContext הוא שירות שצריך הזרקה


// DbContext
builder.Services.AddDbContext<SaleContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT configuration
var key = builder.Configuration["Jwt:Key"];
if (!string.IsNullOrEmpty(key))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(builder.Configuration["Jwt:Issuer"]),
            ValidateAudience = !string.IsNullOrEmpty(builder.Configuration["Jwt:Audience"]),
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at app root
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// CORS disabled for debug
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();