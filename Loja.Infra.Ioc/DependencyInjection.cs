using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Loja.Infra.Data.Context;
using Loja.Domain.Models;

namespace Loja.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectManagerDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, b =>
                    b.MigrationsAssembly("ProjectManager.Infra.Data"));
            });

            var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? configuration["JWT:SecretKey"];

            if (string.IsNullOrEmpty(jwtSecretKey))
            {
                throw new ArgumentNullException(nameof(jwtSecretKey), "A chave JWT não está configurada corretamente.");
            }

            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Configuração do CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Substitua pelo domínio que você quer permitir
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });

                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddAuthorization();
            services.AddHttpContextAccessor();


            return services;
        }
    }
}