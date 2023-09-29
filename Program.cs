
using CtekDev_ManagedIdentity.Models;
using CtekDev_ManagedIdentity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CtekDev_ManagedIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IKeyVaultService, KeyVaultService>();

            var envSection = builder.Configuration.GetSection("EnvVariables");
            var envVariables = envSection.Get<EnvVariables>();

#if DEBUG
            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", envVariables.ClientId);
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", envVariables.TenantId);
            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", envVariables.ClientSecret);
#endif

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}