
using Chat_Application.Configurations;
//using ChatApp.Application;
//using ChatApp.Domain.Interfaces;
//using ChatApp.Infrastructure.Data;
//using ChatApp.Infrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;
//using System;

namespace ChatAppAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            
            // Configurations
            builder.Services.ConfigureJwtToken(builder.Configuration);    // JWT Configuration
            builder.Services.ExternalConfiguration(builder.Configuration); // SignalR, MediatR and Swagger Configurations

            
            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            //SignalR
            //app.MapHub<hub>("/notificationHub");

            app.Run();
        }
    }
}
