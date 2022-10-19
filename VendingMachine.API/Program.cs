using System.Text;
using  Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using VendingMachine.API;
using VendingMachine.Auth;
using VendingMachine.Domain;
using VendingMachine.Domain.User;
using VendingMachine.EF;
using VendingMachine.EF.Products;
using VendingMachine.EF.Users;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;


services.AddScoped(
    _ => new VendingMachineDbContextFactory().CreateDbContext(
        Array.Empty<string>()
    // provider.GetService<ILoggerFactory>()
    )
);
services.AddTransient<IUserRepository, EfUserRepository>();
services.AddTransient<IUserService, UserService>();
services.AddTransient<IAuthService, AuthService>();
services.AddTransient<ICurrentUserService, AuthService>();
services.AddSingleton(_ => new AuthConfig("super long secret gere"));
services.AddTransient<IProductRepository, EfProductRepository>();
services.AddTransient<IProductsService, ProductService>();
services.AddHttpContextAccessor();

var allowDevPolicy = "AllowDev";
services.AddCors(options => options.AddPolicy(allowDevPolicy,
    builder => builder.AllowAnyMethod()
        .AllowAnyMethod()
        .AllowAnyOrigin()));

services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
          
            var userName = context.Principal.Identity.Name;

            try
            {
                await userService.GetByUserName(userName);
            }
            catch
            {
                // If user is not found, an AppException is thrown
                context.Fail("Unauthorized");
            }
        }
    };
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("super long secret gere")),
        ValidateIssuer = false,
        ValidateAudience = false
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
app.UseRouting();
app.UseCors(corsPolicyBuilder => 
    corsPolicyBuilder.WithOrigins(new[]{ "http://localhost:4200/","http://localhost:4200","localhost:4200"})
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();