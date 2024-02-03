// using System.IdentityModel.Tokens.Jwt;
// using System.Text;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
// using Microsoft.OpenApi.Models;
//
// var builder = WebApplication.CreateBuilder(args);
//
//
// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//
//
// // Database and Identity
// builder.Services.AddDbContext<AppIdentityDbContext>(options =>
// {
//     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//     options.UseMySQL(connectionString);
// });
//
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//     .AddEntityFrameworkStores<AppIdentityDbContext>()
//     .AddDefaultTokenProviders();
//
//
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = builder.Configuration["Jwt:Issuer"],
//             ValidAudience = builder.Configuration["Jwt:Audience"],
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//         };
//     });
//
// builder.Services.AddAuthorization();
//
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(opt =>
// {
//     opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
//     opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//     {
//         In = ParameterLocation.Header,
//         Description = "Please enter token",
//         Name = "Authorization",
//         Type = SecuritySchemeType.Http,
//         BearerFormat = "JWT",
//         Scheme = "bearer"
//     });
//     opt.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Type = ReferenceType.SecurityScheme,
//                     Id = "Bearer"
//                 }
//             },
//             new string[] { }
//         }
//     });
// });
//
//
// var app = builder.Build();
//
// app.UseAuthentication();
// app.UseAuthorization();
//
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
//
// app.MapPost("register",
//     async (UserManager<IdentityUser> userMgr, User user) =>
//     {
//         try
//         {
//             var identityUser = new IdentityUser()
//             {
//                 UserName = user.Username,
//                 Email = user.Email,
//             };
//
//             var result = await userMgr.CreateAsync
//                 (identityUser, user.Password);
//
//             return result.Succeeded ? Results.Ok() : Results.BadRequest(result.Errors);
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     });
//
// // create a minimal api for testing authenticationapp.
//
//
// app.MapPost("login",
//     async (UserManager<IdentityUser> userMgr, User user) =>
//     {
//         var identityUsr = await userMgr.FindByNameAsync(user.Username);
//
//         if (await userMgr.CheckPasswordAsync(identityUsr, user.Password))
//         {
//             var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
//             var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
//
//             var token = new JwtSecurityToken(
//                 issuer: builder.Configuration["Jwt:Issuer"],
//                 audience: builder.Configuration["Jwt:Audience"],
//                 expires: DateTime.Now.AddMinutes(120),
//                 signingCredentials: credentials);
//             var stringToken = new JwtSecurityTokenHandler().WriteToken(token);
//             return Results.Ok(stringToken);
//         }
//         else
//         {
//             return Results.Unauthorized();
//         }
//     });
//
// app.MapGet("test", () => "OK").RequireAuthorization();
//
//
// app.Run();
//
//
// public class AppIdentityDbContext :
//     IdentityDbContext<IdentityUser, IdentityRole, string>
// {
//     public AppIdentityDbContext
//         (DbContextOptions<AppIdentityDbContext> options)
//         : base(options)
//     {
//     }
// }
//
// public class User
// {
//     public string Username { get; set; }
//
//     public string Email { get; set; }
//     public string Password { get; set; }
// }
//
//
// public class loginDTO
// {
// }