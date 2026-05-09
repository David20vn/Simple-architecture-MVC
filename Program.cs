using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Sistema_inventario_mvc.Services.Implementations;
using Sistema_inventario_mvc.Services.Interfaces;
using Sistema_inventario_mvc.Repositories.Implementations;
using Sistema_inventario_mvc.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicio de controladores (API o MVC)
builder.Services.AddControllers();

// 2. Registrar tus servicios personalizados como Singleton
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
builder.Services.AddSingleton<IInventoryService, InventoryService>();
builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();

// 3. Autenticación JWT (la configuración que ya tienes)
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
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// 4. Middleware
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();   // Activa las rutas de los controladores

app.Run();