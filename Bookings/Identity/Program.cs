using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MySqlX.XDevAPI.Common; // Add this for Swagger

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Database and Identity
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});




// Register the Swagger generator
builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TokenAuthMinimalApi", Version = "v1" });
    
    // Configure Swagger to use JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenAuthMinimalApi v1"));
}

app.UseAuthentication();
app.UseAuthorization();

 app.MapPost("register",
     async (UserManager<IdentityUser> userMgr, User user) =>
     {
         try
         {
             var identityUser = new IdentityUser()
             {
                 UserName = user.Username,
                 Email = user.Email,
             };

             var result = await userMgr.CreateAsync
                 (identityUser, user.Password);

             return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
     });

// Login endpoint
app.MapPost("/login",async  (UserManager<IdentityUser> userMgr,User loginUser, IConfiguration config) =>
{
    var user = await userMgr.FindByNameAsync(loginUser.Username);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    if (await userMgr.CheckPasswordAsync(user, loginUser.Password))
    {
        return Results.Ok(GenerateToken(user.UserName, config));
    }
    return Results.Unauthorized();
});


 

// app.MapGet("/secure-data",   (() => "This is a secure endpoint")).RequireAuthorization();

app.Run();

string GenerateToken(string username, IConfiguration configuration)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, username)
    };

    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}


public class AppIdentityDbContext :
    IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AppIdentityDbContext
        (DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }
}

public class User
{
    public string Username { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
}