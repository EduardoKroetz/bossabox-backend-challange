using bossabox_backend_challange.Data;
using bossabox_backend_challange.DTOs;
using bossabox_backend_challange.Models;
using bossabox_backend_challange.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Invalid db connection");
builder.Services.AddDbContext<VUTTRDbContext>(options => options.UseSqlServer(connectionString));

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtKey") ?? throw new Exception("Invalid Jwt key"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/auth/register", async (AuthService authService ,VUTTRDbContext dbContext ,[FromBody] AuthDto dto) => 
{
    var userExists = await dbContext.Users.FirstOrDefaultAsync(x => x.Name.Equals(dto.Name));
    if (userExists != null)
    {
        return Results.BadRequest(new ResultDto<object> { Error = "Esse usuário já está cadastrado!" });
    }

    var user = new User
    {
        Name = dto.Name
    };

    await dbContext.Users.AddAsync(user);
    await dbContext.SaveChangesAsync();

    var token = authService.GenerateJwtToken(user);

    return Results.Ok(new ResultDto<object> { Data = new { user.Id, Token = token } });
});

app.MapPost("/auth/login", async (AuthService authService, VUTTRDbContext dbContext, [FromBody] AuthDto dto) =>
{
    var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Name.Equals(dto.Name));
    if (user == null)
    {
        return Results.NotFound(new ResultDto<object> { Error = "Usuário não encontrado" });
    }

    var token = authService.GenerateJwtToken(user);

    return Results.Ok(new ResultDto<object> { Data = new { user.Id, Token = token } });
});



app.UseHttpsRedirection();

app.Run();




