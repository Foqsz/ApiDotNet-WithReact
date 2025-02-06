using ApiDotNet_WithReact.Context;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ApiDotNet_WithReact.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDotNet_WithReact", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer + {seu token JWT aqui}"
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
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // validando o emissor do token
        ValidateAudience = true, // validando o receptor do token
        ValidateLifetime = true, // validando o tempo de vida do token
        ValidateIssuerSigningKey = true, // validando a chave de assinatura do token
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // emissor do token
        ValidAudience = builder.Configuration["Jwt:Audience"], // receptor do token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

builder.Services.AddScoped<IAlunoService, AlunoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddMemoryCache(); //Cache em memória

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDotNet_WithReact v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseCors("AllowLocalhost3000");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();