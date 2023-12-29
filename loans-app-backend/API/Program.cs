using API.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LoanAppDB")));

//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy(name: "NewPolicy", app =>
//    {
//        //app.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
//        app.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod();
//    });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularOrigins",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Configuration.AddJsonFile("appsettings.json");
var key = builder.Configuration.GetSection("settings").GetSection("Key").ToString();
var keyBites = Encoding.UTF8.GetBytes(key);

//  Configuración para implementar jwt
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBites),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddControllers();

builder.Services.AddGrpcReflection();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcReflectionService();
// app.MapGrpcService<GreeterService>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAngularOrigins");
app.Run();
