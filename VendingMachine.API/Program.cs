using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.Auth;
using VendingMachine.Domain;
using VendingMachine.Domain.Auth;
using VendingMachine.Domain.User;
using VendingMachine.EF;
using VendingMachine.EF.Products;
using VendingMachine.EF.Users;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

var tokenOpts = new TokenGeneratorConfig
{
    Audience = configuration["JWTAuth:ValidAudience"],
    Issuer = configuration["JWTAuth:ValidIssuer"],
    Secret = configuration["JWTAuth:Secret"],
    TokenValidityInMinutes = int.Parse(configuration["JWTAuth:TokenValidityInMinutes"])
};

services.AddSingleton(tokenOpts);
services.AddSingleton<ITokenGenerator, TokenGenerator>();
services.AddScoped(
    _ => new VendingMachineDbContextFactory().CreateDbContext(
        Array.Empty<string>()
        // provider.GetService<ILoggerFactory>()
    )
);

services.AddTransient<IUserRepository, EfUserRepository>();

services.AddTransient<IAuthService, AuthService>();
services.AddTransient<ICurrentUserService, AuthService>();
services.AddTransient<IProductRepository, EfProductRepository>();
services.AddTransient<IProductsService, ProductService>();
services.AddHttpContextAccessor();

var allowDevPolicy = "AllowDev";
services.AddCors(options => options.AddPolicy(allowDevPolicy,
    builder => builder.AllowAnyMethod()
        .AllowAnyMethod()
        .AllowAnyOrigin()));

builder.Services.AddIdentity<VendingMachineUserDpo, IdentityRole>()
    .AddEntityFrameworkStores<VendingMachineDbContext>()
    .AddDefaultTokenProviders();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ClockSkew = TimeSpan.Zero,

        ValidAudience = tokenOpts.Audience,
        ValidIssuer = tokenOpts.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOpts.Secret))
    };
});

builder.Services.AddControllers(o =>
{
    var authByDefault = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    o.Filters.Add(new AuthorizeFilter(authByDefault));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors(corsPolicyBuilder =>
    corsPolicyBuilder.WithOrigins(new[] { "http://localhost:4200/", "http://localhost:4200", "localhost:4200" })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();