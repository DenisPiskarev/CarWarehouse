using System.Text;
using CarWarehouse.BLL.Interfaces;
using CarWarehouse.DAL.Repositories;
using CarWarehouse.BLL.Services;
using CarWarehouse.DAL;
using CarWarehouse.DAL.Models;
using CarWarehouse.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using CarWarehouse.DAL.Interfaces;
using CarWarehouse.BLL.Mappings;
using CarWarehouse.Web.Mappings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

builder.Services.AddAutoMapper(typeof(CarWarehouse.Web.Mappings.MappingProfile), typeof(CarWarehouse.BLL.Mappings.MappingProfile));

builder.Services.AddControllers();

builder.Services.AddDbContext<CarWarehouseContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CarWarehouse.DAL")));

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<CarWarehouseContext>();

builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthSettings"));

builder.Services.AddAuthentication((options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}))
                .AddJwtBearer(options =>
                {
                    var authSettings = builder.Configuration.GetSection("AuthSettings").Get<AuthSettings>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Key)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
