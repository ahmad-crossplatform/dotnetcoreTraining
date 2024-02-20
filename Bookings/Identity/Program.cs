using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Bookings.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Add this for Swagger

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "ClientRequests", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:5210").WithHeaders("Content-Type");
    });
});

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);

});




// Register the Swagger generator
builder.Services.AddEndpointsApiExplorer(); // Enables API explorer for Swagger
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppIdentityDbContext>();
var app = builder.Build();

app.UseCors("ClientRequests");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TokenAuthMinimalApi v1"));
}

app.MapPost("register",
     async (UserManager<ApplicationUser> userMgr, RegisterUserDTO user) =>
     {
         try
         {
             var identityUser = new ApplicationUser
             {
                 FirstName = user.FirstName,
                 LastName = user.LastName,

                 NormalizedUserName = user.Email,

                 UserName = user.Email,
                 Email = user.Email,
             };


             var result = await userMgr.CreateAsync
                 (identityUser, user.Password);

             if (!result.Succeeded) return Results.BadRequest(result.Errors);
             var addToRoleResult = await userMgr.AddToRoleAsync(identityUser, "User");


             if (!addToRoleResult.Succeeded) return Results.BadRequest(addToRoleResult.Errors);

             // add claims 
             var claims = new List<Claim> {
             new Claim(ClaimTypes.Name, identityUser.FullName),
             new Claim(ClaimTypes.Email, identityUser.Email),
             new Claim(ClaimTypes.Role, "User"),
             new Claim(ClaimTypes.NameIdentifier, identityUser.Id),


             };
             var addClaimsResult = await userMgr.AddClaimsAsync(identityUser, claims);

             return Results.Ok();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             throw;
         }
     });

// register admin endpoint
app.MapPost("register-admin",
    async (UserManager<ApplicationUser> userMgr, RegisterUserDTO user) =>
    {
        try
        {
            if (user.Password != user.ConfirmPassword)
            {
                return Results.BadRequest("Password and Confirm password are not the same");
            }
            var identityUser = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,

                NormalizedUserName = user.Email,

                UserName = user.Email,
                Email = user.Email,
            };

            var result = await userMgr.CreateAsync
                (identityUser, user.Password);

            if (!result.Succeeded) return Results.BadRequest(result.Errors);
            await userMgr.AddToRoleAsync(identityUser, "Admin");
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    });

// Login endpoint
app.MapPost("/login", async (UserManager<ApplicationUser> userMgr, LoginDTO loginUser, IConfiguration config) =>
{
    var user = await userMgr.FindByNameAsync(loginUser.Email);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    if (await userMgr.CheckPasswordAsync(user, loginUser.Password))
    {
        await userMgr.AddLoginAsync(user, new UserLoginInfo("Email", loginUser.Email, user.FullName));
        try
        {
            var token = await GenerateToken(userMgr, user.UserName, config);
            return Results.Ok(token);
        }
        catch (System.Exception ex)
        {

            throw;
        }

    }
    return Results.Unauthorized();
});



// app.MapGet("/secure-data",   (() => "This is a secure endpoint")).RequireAuthorization();

app.Run();

async Task<string> GenerateToken(UserManager<ApplicationUser> userMgr, string username, IConfiguration configuration)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    // get user and roles from the database to add to the token
    var user = userMgr.FindByNameAsync(username).Result;

    var claims = await userMgr.GetClaimsAsync(user);




    var token = new JwtSecurityToken(
        issuer: configuration["Jwt:Issuer"],
        audience: configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);

    return new JwtSecurityTokenHandler().WriteToken(token);
}


public class AppIdentityDbContext :
    IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppIdentityDbContext
        (DbContextOptions<AppIdentityDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // seed IdentityRoles 
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Name = "User", NormalizedName = "USER" }
        );

        base.OnModelCreating(builder);
    }
}


public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

